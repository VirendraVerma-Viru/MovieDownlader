using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ColorCollectorGameController : MonoBehaviour
{
    [Header("Main Game")]
    public Transform[] PositionTODrop;
    public Material[] ColorMaterials;//0 for blue , 2 for red, 1 for green, 4 for yellow
    public GameObject Ball;
    public GameObject ColorChanger;

    private float timer = 2f;
    private float counntdown = 2f;
    private float drag = 3;
    private float colorchangertimer = 0f;
    private float colorchangercountdown = 10f;

    private int currentScore;
    private bool isgamepause = false;
    private bool isGameStart = false;
    private bool isNewHighscore = false;
    private bool isGameOver = false;

    public GameObject GameOverPannel;
    public GameObject Player;
    public GameObject TapTOStartButton;

    void Start()
    {
        colorcollectorsaveload.Load();
        Initialize();
        checkifreload();
    }

    void checkifreload()
    {
        if (colorcollectorsaveload.isreturn > 0)
        {
            isGameStart = true;
            TapTOStartButton.SetActive(false);
            Player.SetActive(true);
            colorcollectorsaveload.isreturn = 0;
            colorcollectorsaveload.Save();
        }
    }

    void Initialize()
    {
        TapTOStartButton.SetActive(true);
        Player.SetActive(false);
        isgamepause = false;
        timer = 5;
        counntdown = 0.5f;
        currentScore = 0;
        colorchangercountdown = 30;
        isGameStart = false;
        isNewHighscore = false;
        isGameOver = false;
        UpdateUI();
        Dificulty();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0 && isGameStart && isGameOver == false && isgamepause==false)
        {
            timer = counntdown;
            GameObject go = Instantiate(Ball);

            int randomColor = Random.Range(0, ColorMaterials.Length);
            go.GetComponent<Renderer>().material = ColorMaterials[randomColor];
            go.GetComponent<ColorCollectorBallDetail>().colorCode = randomColor;

            int randomPosition = Random.Range(0, PositionTODrop.Length);
            go.transform.position = PositionTODrop[randomPosition].transform.position;

            go.GetComponent<Rigidbody>().drag = drag;
            Dificulty();
        }

        colorchangertimer -= Time.deltaTime;
        if (colorchangertimer < 0)
        {
            colorchangertimer = colorchangercountdown;
            isgamepause = true;
            StartCoroutine(WaitToSendColorChanger());
        }
    }

    void Dificulty()
    {
        int nu = currentScore / 5;
        counntdown = 0.7f - nu * 0.02f;
        drag = 3 - nu * 0.2f;
        if (counntdown <= 0.3f)
            counntdown = 0.3f;
        if (drag <=0)
            drag = 0;
       
    }

    IEnumerator WaitToSendColorChanger()
    {
        yield return new WaitForSeconds(2);
        GameObject go = Instantiate(ColorChanger);
        int randomColor = Random.Range(0, ColorMaterials.Length);
        go.GetComponent<Renderer>().material = ColorMaterials[randomColor];
        go.GetComponent<ColorCollectorBallDetail>().colorCode = randomColor;
        go.GetComponent<Rigidbody>().drag = drag;
        yield return new WaitForSeconds(2);
        isgamepause = false;
    }

    public void OnAddScore()
    {
        if (isGameOver == false)
        {
            currentScore += 1;
            FindObjectOfType<AudioManager>().Play("LevelUp");
            if (colorcollectorsaveload.highscore < currentScore)
            {
                NewHighScoreGO.SetActive(true);
                colorcollectorsaveload.highscore = currentScore;
                colorcollectorsaveload.Save();
            }
            UpdateUI();
        }
    }

    
    public void OnGameOver()
    {
        isGameOver = true;
        FindObjectOfType<AudioManager>().Play("GameOver");
        StartCoroutine(WaitForPannel());
        UpdateUI();
    }

    IEnumerator WaitForPannel()
    {
        yield return new WaitForSeconds(1.5f);
        GameOverPannel.SetActive(true);
        if (colorcollectorsaveload.adsFrequency > 3)
        {
            colorcollectorsaveload.adsFrequency = 0;
            ShowAds();
        }
        else
        {
            colorcollectorsaveload.adsFrequency += 1;
        }
        colorcollectorsaveload.Save();
    }

    public void OnChangeColor(int colorCode)
    {
        Player.GetComponent<Renderer>().material = ColorMaterials[colorCode];
    }

    [Header("UI")]
    public Text HighscoreText;
    public Text CurrentScoreText;
    public Text CurrentScoreGameOverText;

    public GameObject NewHighScoreGO;

    void UpdateUI()
    {
        HighscoreText.text = colorcollectorsaveload.highscore.ToString();
        CurrentScoreText.text = currentScore.ToString();
        CurrentScoreGameOverText.text = "Current score : " + currentScore.ToString();
    }

    public void OnGameStartButtonPressed()
    {
        isGameStart = true;
        Player.SetActive(true);
        TapTOStartButton.SetActive(false);
    }


    public void RestartButton()
    {
        colorcollectorsaveload.isreturn = 1;
        colorcollectorsaveload.Save();
        SceneManager.LoadScene("colorcollector");
    }

    public void OnBackButton()
    {
        saveload.isGamePannelOn = 1;
        saveload.Save();
        SceneManager.LoadScene(0);
    }

    public GameObject HighscorePannel;

    public void OnHighscoreButtonPresseed()
    {
        HighscorePannel.SetActive(true);
        HighscorePannel.GetComponent<HighscoreControllerInGame>().UpdatePlayerHighscore("ColorCollector", colorcollectorsaveload.highscore);
    }

    void ShowAds()
    {
        GameObject.FindGameObjectWithTag("Ads").gameObject.GetComponent<Ads>().ShowInter();
    }
   
}
