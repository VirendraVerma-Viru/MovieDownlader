using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public bool forward = false;
    public bool pause = true;

    //------------------for bomber-----------
    public Transform[] cannons;
    public GameObject bomb;
    float time = 1f;
    private bool isGameOver = false;

    //-------------for UI-----------
    public Sprite playSprite;
    public Sprite ReverseSprite;
    public Button playButton;

    public Text CurrentScoreText;
    public Text HighScoreText;
    public Text LevelText;
    public Text CurrentScoreGameOverText;

    public Material[] backgroundSprites;
    public Renderer backgroundImage;

    public GameObject gameOverPannel;

    //----------------------value------
    public int currentMoney;
    public int highscoreMoney;
    public int level;
    private float waitTime = 0.1f;

    private float minFreq = 0.1f;
    private float maxFreq = 1f;

    void Awake()
    {
        waitTime = 3;

        corsairsaveload.Load();
        highscoreMoney = corsairsaveload.highscore;
        currentMoney = corsairsaveload.currentscore;
        level = corsairsaveload.level;
        gameOverPannel.SetActive(false);
        isGameOver = false;
        forward = false;
        pause = true;
        playButton.GetComponent<Image>().sprite = playSprite;
        int t = Random.Range(0, backgroundSprites.Length);
        backgroundImage.material = backgroundSprites[t];
        UpdateUI();
        SetMinMaxFreq();
        playButton.gameObject.SetActive(false);
        StartCoroutine(WaitForPlayButton());
    }

    IEnumerator WaitForPlayButton()
    {
        yield return new WaitForSeconds(3);
        playButton.gameObject.SetActive(true);
    }

    void SetMinMaxFreq()
    {
        if (corsairsaveload.level < 5)
        {
            minFreq = 0.1f;
            maxFreq = 1f;
        }
        else if (corsairsaveload.level < 10)
        {
            minFreq = 0.09f;
            maxFreq = 1f;
        }
        else if (corsairsaveload.level < 15)
        {
            minFreq = 0.08f;
            maxFreq = 1f;
        }
        else if (corsairsaveload.level < 20)
        {
            minFreq = 0.07f;
            maxFreq = 1f;
        }
        else if (corsairsaveload.level < 25)
        {
            minFreq = 0.06f;
            maxFreq = 0.9f;
        }
        else if (corsairsaveload.level < 30)
        {
            minFreq = 0.05f;
            maxFreq = 0.9f;
        }
        else if (corsairsaveload.level < 35)
        {
            minFreq = 0.04f;
            maxFreq = 0.9f;
        }
        else if (corsairsaveload.level < 40)
        {
            minFreq = 0.03f;
            maxFreq = 0.9f;
        }
        else if (corsairsaveload.level < 45)
        {
            minFreq = 0.02f;
            maxFreq = 0.9f;
        }
        else
        {
            
            minFreq = 0.01f;
            maxFreq = 0.8f;
        
        }
        
    }

    public void ForwardAndReverseButton()
    {
        if (forward)
        {
            forward = false;
        }
        else
        {
            forward = true;
        }
        pause = false;
        playButton.GetComponent<Image>().sprite = ReverseSprite;
        if (isGameOver)
        {
            SceneManager.LoadScene("Corsairgame");
            
        }
        
    }

    void Update()
    {
        time -= Time.deltaTime;
        waitTime -= Time.deltaTime;
        if (!pause && !isGameOver)
        {
            if (time < 0)
            {
                time = Random.Range(minFreq, maxFreq);
                int randomCannon = Random.Range(0, cannons.Length);
                GameObject go = Instantiate(bomb, cannons[randomCannon].position, cannons[randomCannon].rotation);
                Destroy(go, 5);
            }

            //check if there is no money then update Level
            GameObject[] temp = GameObject.FindGameObjectsWithTag("Money");

            if (temp.Length <= 1 && waitTime<0)
            {
                pause = true;
                FindObjectOfType<AudioManager>().Play("LevelUp");
                corsairsaveload.currentscore = currentMoney;
                corsairsaveload.level++;
                corsairsaveload.Save();
                SceneManager.LoadScene("Corsairgame");
            }
        }

    }

    public void AddScore()
    {
        currentMoney += corsairsaveload.level;
        if (currentMoney > corsairsaveload.highscore)
        {
            corsairsaveload.highscore = currentMoney;
            
        }
        corsairsaveload.Save();
    }

    public GameObject NewHighScore;
    public void GameOver()
    {
        //destroy all bombs
        playButton.gameObject.SetActive(false);
        pause = true;
        isGameOver = true;
        CurrentScoreGameOverText.text = "Current Score : "+currentMoney.ToString();
        corsairsaveload.currentscore = 0;
        corsairsaveload.level = 1;
        if (currentMoney > corsairsaveload.highscore)
        {
            corsairsaveload.highscore = currentMoney;
            NewHighScore.SetActive(true);
        }
        corsairsaveload.Save();
        StartCoroutine(WaitForPannel());
        playButton.GetComponent<Image>().sprite = playSprite;
    }

    IEnumerator WaitForPannel()
    {
        yield return new WaitForSeconds(1.5f);
        gameOverPannel.SetActive(true);
        if (corsairsaveload.adsFrequency > 3)
        {
            corsairsaveload.adsFrequency = 0;
            ShowAds();
        }
        else
        {
            corsairsaveload.adsFrequency += 1;
        }
        corsairsaveload.Save();
    }

    public void UpdateUI()
    {
        CurrentScoreText.text = currentMoney.ToString();
        HighScoreText.text = highscoreMoney.ToString();
        LevelText.text = level.ToString();
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("Corsairgame");
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
        HighscorePannel.GetComponent<HighscoreControllerInGame>().UpdatePlayerHighscore("Corsair", corsairsaveload.highscore);
    }

    void ShowAds()
    {
        GameObject.FindGameObjectWithTag("Ads").gameObject.GetComponent<Ads>().ShowInter();
    }
}
