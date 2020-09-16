using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MazeGameController : MonoBehaviour
{
    public GameObject[] AllObstacle;
    private float timer = 2f;
    private float counntdown = 2f;
    private float drag = 3;

    private int currentScore;
    private bool isGameStart = false;
    private bool isNewHighscore = false;
    private bool isGameOver = false;

    public GameObject GameOverPannel;
    public GameObject Player;
    public GameObject TapTOStartButton;
    

    void Start()
    {
        mazesaveload.Load();
        Initialize();
        checkifreload();
    }

    void checkifreload()
    {
        if (mazesaveload.isreturn > 0)
        {
            isGameStart = true;
            TapTOStartButton.SetActive(false);
            Player.SetActive(true);
            mazesaveload.isreturn = 0;
            mazesaveload.Save();
        }
    }

    void Initialize()
    {
        TapTOStartButton.SetActive(true);
        Player.SetActive(false);
        timer = 2;
        counntdown = 2;
        currentScore = 0;
        isGameStart = true;
        isNewHighscore = false;
        isGameOver = false;
        UpdateUI();
        Dificulty();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0 && isGameStart && isGameOver==false)
        {
            timer = counntdown;
            int random = Random.Range(0, AllObstacle.Length);
            GameObject go = Instantiate(AllObstacle[random]);
            go.GetComponent<Rigidbody>().drag = drag;
            Destroy(go, 10);
            Dificulty();
        }
    }

    void Dificulty()
    {

        int t = currentScore / 5;
        counntdown = 2f - t * 0.05f;
        drag = 3f - t * 0.2f;

        if (counntdown <= 0.7f)
            counntdown = 0.7f;
        if (drag <= 0f)
            drag = 0f;

    }

    

    public void GameOver()
    {
        isGameOver = true;
        FindObjectOfType<AudioManager>().Play("GameOver");
        StartCoroutine(WaitForPannel());
        if (isNewHighscore)
        {
            NewHighScoreGO.SetActive(true);
        }
        Destroy(Player);
    }

    IEnumerator WaitForPannel()
    {
        yield return new WaitForSeconds(1.5f);
        GameOverPannel.SetActive(true);
        if (mazesaveload.adsFrequency > 3)
        {
            mazesaveload.adsFrequency = 0;
            ShowAds();
        }
        else
        {
            mazesaveload.adsFrequency += 1;
        }
        mazesaveload.Save();
    }


    public void AddScore()
    {
        if (isGameOver == false)
        {
            currentScore += 1;
            FindObjectOfType<AudioManager>().Play("LevelUp");
            if (currentScore > mazesaveload.highscore)
            {
                mazesaveload.highscore = currentScore;
                mazesaveload.Save();
                isNewHighscore = true;
            }
            UpdateUI();
        }
    }

    [Header("UI")]
    public Text HighscoreText;
    public Text CurrentScoreText;
    public Text CurrentScoreGameOverText;

    public GameObject NewHighScoreGO;

    void UpdateUI()
    {
        HighscoreText.text = mazesaveload.highscore.ToString();
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
        mazesaveload.isreturn = 1;
        mazesaveload.Save();
        SceneManager.LoadScene("Maze");
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
        HighscorePannel.GetComponent<HighscoreControllerInGame>().UpdatePlayerHighscore("MazeRunner", mazesaveload.highscore);
    }

    void ShowAds()
    {
        GameObject.FindGameObjectWithTag("Ads").gameObject.GetComponent<Ads>().ShowInter();
    }
   
}
