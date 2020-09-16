using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TapTapGameController : MonoBehaviour
{
    private float tempBoxAppearTime = 0.1f;
    private float tempCountDownBoxAppearTime;
    private bool isGameStart = false;
    private int levelRange = 20;
    private int currentScore;
    private bool isGameOver = false;

    public GameObject GameOverPannel;
    public GameObject StartButton;
    
    void Start()
    {
        taptapsaveload.Load();
        InitializeData();
        checkifreload();
        
    }


    void checkifreload()
    {
        if (taptapsaveload.isreturn > 0)
        {
            isGameStart = true;
            StartButton.SetActive(false);
            taptapsaveload.isreturn = 0;
            taptapsaveload.Save();
        }
    }
    
    void Update()
    {
        tempBoxAppearTime -= Time.deltaTime;

        if (tempBoxAppearTime < 0 && isGameStart && isGameOver==false)
        {
            Dificulty();
            tempBoxAppearTime = tempCountDownBoxAppearTime;
            if (checkIfSpace(levelRange) == false && !isGameOver)
            {
                int random = GetRandom(levelRange);
                GameObject go = Instantiate(BoxGO);
                go.transform.position = TapPosition[random].transform.position;
                go.GetComponent<TapBox>().codeNumber = random;
            }
            else
            {
                //gameOver
                isGameOver = true;
                FindObjectOfType<AudioManager>().Play("GameOver");
                if (isNewScore)
                {
                    NewHighScoreText.SetActive(true);
                }
                
                GameOverPannel.SetActive(true);
                if (taptapsaveload.adsFrequency > 3)
                {
                    taptapsaveload.adsFrequency = 0;
                    ShowAds();
                }
                else
                {
                    taptapsaveload.adsFrequency += 1;
                }
                taptapsaveload.Save();
                //StartButton.SetActive(true);
            }
            
            
        }

        //raycasting
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.tag == "TapBox" && !isGameOver)
                {
                    int nu = hit.collider.gameObject.GetComponent<TapBox>().codeNumber;
                    TapPositionNumber[nu] = 0;
                    currentScore += 1;
                    FindObjectOfType<AudioManager>().Play("LevelUp");
                    Destroy(hit.collider.gameObject);

                    if (currentScore > taptapsaveload.highscore)
                    {
                        isNewScore = true;
                        taptapsaveload.highscore = currentScore;
                        taptapsaveload.Save();
                    }
                    UpdateUI();
                }

            }
        }
    }


    void Dificulty()
    {
        
        int t = currentScore / 5;
        tempCountDownBoxAppearTime = 0.7f - t * 0.02f;

        if (tempCountDownBoxAppearTime<=0.2f)
            tempCountDownBoxAppearTime = 0.2f;
        
    }

    bool checkIfSpace(int range)
    {
        bool flag = false;
        
        for (int i = 0; i < range; i++)
        {
            if (TapPositionNumber[i] == 1)
            {
                flag = true;
            }
            else
            {
                flag = false;
                break;
            }
        }
        return flag;
    }

    int GetRandom(int range)
    {
        int r=0;
        int n = 1;
        
        while (n>0)
        {
            int random = Random.Range(0, range);
            int flag = 0;
            for(int i=0;i<TapPositionNumber.Length;i++)
            {
                if (random == i && TapPositionNumber[i]==1)
                {
                    flag = i;
                }
            }
            
            if (flag == 0 )
            {
                n = 0;
                r = random;
                TapPositionNumber[random] = 1;
            }
        }
        return r;
    }


    #region Initialize

    [Header("Initialize")]
    public Transform[] TapPosition;
    public int[] TapPositionNumber;
    public GameObject BoxGO;
    void InitializeData()
    {
        tempBoxAppearTime = 0.1f;
        isNewScore = false;
        tempCountDownBoxAppearTime = 0.1f;
        isGameStart = false;
        levelRange = 20;
        currentScore = 0;
        GameOverPannel.SetActive(false);
        StartButton.SetActive(true);
        isGameOver = false;
        TapPositionNumber = new int[TapPosition.Length];
        for (int i = 0; i < TapPosition.Length; i++)
        {
            TapPositionNumber[i] = 0;
        }
        UpdateUI();
    }

    #endregion


    #region UpdateUI

    [Header("UI")]
    public Text HighscoreText;
    public Text CurrentScoreText;
    public Text CurrentScoreGameOver;
    public GameObject NewHighScoreText;
    private bool isNewScore = false;

    void UpdateUI()
    {
        HighscoreText.text = taptapsaveload.highscore.ToString();
        CurrentScoreText.text = currentScore.ToString();
        CurrentScoreGameOver.text = "Current Score : "+currentScore.ToString();
    }

    #endregion

    public void OnPlayButtonPressed()
    {
        isGameStart = true;
        StartButton.SetActive(false);
    }
    public void OnGameRestart()
    {
        taptapsaveload.isreturn = 1;
        taptapsaveload.Save();
        SceneManager.LoadScene("taptap");
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
        HighscorePannel.GetComponent<HighscoreControllerInGame>().UpdatePlayerHighscore("TapTap", taptapsaveload.highscore);
    }

    void ShowAds()
    {
        GameObject.FindGameObjectWithTag("Ads").gameObject.GetComponent<Ads>().ShowInter();
    }
}
