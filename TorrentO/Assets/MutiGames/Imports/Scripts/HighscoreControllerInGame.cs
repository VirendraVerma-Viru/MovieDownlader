using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreControllerInGame : MonoBehaviour
{

    private string gettingallhighscores = "https://torrentodownloader.000webhostapp.com/MultiGames/gethighscore.php";
    private string updatehighscores = "https://torrentodownloader.000webhostapp.com/MultiGames/updatehighscore.php";
    private string updateusername = "https://torrentodownloader.000webhostapp.com/MultiGames/updatename.php";

    public Text resultStatusText;

    public string[] items;
    public Text rankText;
    public Text nameText;
    public Text scoreText;
    public GameObject Score;
    public Transform scoresTransform;

    private GameObject go;
    public string namesdb;
    public int highscoredb;
    public int iddb;
    private int t = 0;
    private int tt = 0;
    private bool waitforUpdateHighscore = true;


    private string gameName = "";
    public Text MainGameNameText;

    
    public void OnCloseButtonPressed()
    {
        gameObject.SetActive(false);
    }
    [Header("Menu Stuff")]
    public GameObject MenuPannel;
    bool ismenuOn = false;
    public void OnMenuButtonPressed()
    {
        if (ismenuOn)
        {
            ismenuOn = false;
            MenuPannel.SetActive(false);
        }
        else
        {
            ismenuOn = true;
            MenuPannel.SetActive(true);
        }
    }

    public void OnQuitMenuButtonPressed()
    {
        MenuPannel.SetActive(false);
        ismenuOn = false;
    }

    public GameObject ChangeNamePannel;

    public void OnChangeNameButtonPressed()
    {
        ChangeNamePannel.SetActive(true);
        MenuPannel.SetActive(false);
        ismenuOn = false;
    }

    public void OnCloseChangeNamePannel()
    {
        ChangeNamePannel.SetActive(false);
    }

    public InputField NameInputField;
    public void OnChangeButtonPressedMainPannel()
    {
        string name = NameInputField.text;
        if (name != "")
        {
            StartCoroutine(UpdateUsername(name));
            ChangeNamePannel.SetActive(false);
        }
    }

    IEnumerator UpdateUsername(string name)
    {
        WWWForm form1 = new WWWForm();
        form1.AddField("id", saveload.accountID);
        form1.AddField("name", name);
        WWW www = new WWW(updateusername, form1);
        yield return www;

        print("updated player score " + www.text);
        if (www.text == "sucess")
        {
            highscoregenerator(gameName);
        }
        else if (www.text == "Problem")
        {
            resultStatusText.text = "Server is in the maintainence";
            resultStatusText.color = Color.red;
        }
        else
        {
            resultStatusText.text = "Server is in the maintainence";
            resultStatusText.color = Color.red;
        }
    }


    void SetName()
    {
        MainGameNameText.text = gameName;
    }

    //--------------------------------------updating player highscore---------------
    public void UpdatePlayerHighscore(string name,int highscore)
    {
        gameName = name;
        SetName();
        GameObject[] go = GameObject.FindGameObjectsWithTag("Box");
        foreach (GameObject g in go)
        {
            Destroy(g);
        }

        resultStatusText.text = "Updating...";
        resultStatusText.color = Color.green;
        saveload.Load();
        if (saveload.accountID != "" && saveload.accountID != null)
            StartCoroutine(UpdatePlayerScore(saveload.accountID, highscore, name));
        else
        {
            highscoregenerator(name);
        }

    }

    IEnumerator UpdatePlayerScore(string id, int highscore,string name)
    {
        WWWForm form1 = new WWWForm();
        form1.AddField("id", id);
        form1.AddField("highscore", highscore);
        form1.AddField("name", name);
        WWW www = new WWW(updatehighscores, form1);
        yield return www;
        
        print("updated player score " + www.text);
        if (www.text == "sucess")
        {
            highscoregenerator(name);
        }
        else if (www.text == "Problem")
        {
            resultStatusText.text = "Server is in the maintainence";
            resultStatusText.color = Color.red;
        }
        else
        {
            resultStatusText.text = "Server is in the maintainence";
            resultStatusText.color = Color.red;
        }

    }

    //------------------------------------------------print highscore--------------------------------
    public void highscoregenerator(string name)
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("Box");
        foreach (GameObject g in go)
        {
            Destroy(g);
        }
        resultStatusText.text = "Loading...";
        resultStatusText.color = Color.green;
        //print ("high score");
        
        StartCoroutine(highscoresdb(name));
    }

    IEnumerator highscoresdb(string name)
    {
        WWWForm form1 = new WWWForm();
        form1.AddField("id", saveload.accountID);
        form1.AddField("name", name);
        WWW www = new WWW(gettingallhighscores, form1);
        yield return www;
       
        print(www.text + "|||");
        if (www.text != "")
        {
            resultStatusText.text = " ";
            string itemsDataString = www.text;
            items = itemsDataString.Split(';');

            namesdb = " ";
            highscoredb = 0;
            iddb = 0;
            saveload.Load();

            for (int i = 0; i < items.Length - 1; i++)
            {
                if (i < items.Length - 2)
                {
                    namesdb = GetDataValue(items[i], "Name:");
                    string temp = GetDataValue(items[i], "Highscore:");
                    string tempid = GetDataValue(items[i], "ID:");

                    //conversion of string to int highscore
                    for (int j = 0; j < temp.Length; j++)
                    {
                        char a = temp[j];
                        t = t * 10 + ChartoIntConverter(a);
                    }

                    //conversion of string to int id
                    for (int j = 0; j < tempid.Length; j++)
                    {
                        char a = tempid[j];
                        tt = tt * 10 + ChartoIntConverter(a);
                    }


                    iddb = tt;



                    tt = 0;
                    highscoredb = t;
                    t = 0;
                    go = (GameObject)Instantiate(Score) as GameObject;
                    if (highscoredb > 0)
                    {
                        go.transform.Find("Rank").gameObject.GetComponent<Text>().text = (i + 1).ToString();
                        go.transform.Find("Name").gameObject.GetComponent<Text>().text = namesdb.ToString();
                        go.transform.Find("Score").gameObject.GetComponent<Text>().text = highscoredb.ToString();

                        if (saveload.accountID == iddb.ToString())
                        {
                            go.transform.Find("Rank").gameObject.GetComponent<Text>().color = Color.green;
                            go.transform.Find("Name").gameObject.GetComponent<Text>().color = Color.green;
                            go.transform.Find("Score").gameObject.GetComponent<Text>().color = Color.green;
                        }
                        else
                        {
                            go.transform.Find("Rank").gameObject.GetComponent<Text>().color = Color.white;
                            go.transform.Find("Name").gameObject.GetComponent<Text>().color = Color.white;
                            go.transform.Find("Score").gameObject.GetComponent<Text>().color = Color.white;
                        }



                        go.transform.SetParent(scoresTransform.transform);
                        go.transform.position = scoresTransform.position;
                        go.transform.localScale = scoresTransform.localScale;
                    }
                }
                else
                {
                    rankText.text = GetDataValue(items[i], "ID:");
                    nameText.text = GetDataValue(items[i], "Name:");
                    scoreText.text = GetDataValue(items[i], "Highscore:");
                }
            }
        }
        else
        {
            resultStatusText.text = "Check the connection";
            resultStatusText.color = Color.red;
        }
    }

    string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains(","))
            value = value.Remove(value.IndexOf(","));
        return value;
    }

    int ChartoIntConverter(char str)
    {
        switch (str)
        {
            case '0':
                return 0;
                break;
            case '1':
                return 1;
                break;
            case '2':
                return 2;
                break;
            case '3':
                return 3;
                break;
            case '4':
                return 4;
                break;
            case '5':
                return 5;
                break;
            case '6':
                return 6;
                break;
            case '7':
                return 7;
                break;
            case '8':
                return 8;
                break;
            case '9':
                return 9;
                break;
            default:
                return 0;
        }
    }
     

}