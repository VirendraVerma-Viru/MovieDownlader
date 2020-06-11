using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GetData : MonoBehaviour
{
    private string getMovieDataHome = "https://torrentodownloader.000webhostapp.com/Movies/getmovies.php";
    private string getMovieSearchDataHome = "https://torrentodownloader.000webhostapp.com/Movies/searchget.php";
    private string getappversionLink = "https://torrentodownloader.000webhostapp.com/Movies/getappversion.php";
    private string reportMovieLink = "https://torrentodownloader.000webhostapp.com/Movies/insertreport.php";
    private string createAccountLink = "https://torrentodownloader.000webhostapp.com/Movies/createaccount.php";
    private string checkNotificationLink = "https://torrentodownloader.000webhostapp.com/Movies/checkNotification.php";
    private string givefeedbackLink = "https://torrentodownloader.000webhostapp.com/Movies/insertfeedback.php";

    private string newAppDownloadURL = "https://torrentodownloader.000webhostapp.com/Movies/Torrento.apk";

    public string[] items;

    //Variable to store
    string[] movieID;
    string[] movieName;
    string[] movieRating;
    string[] movieYear;
    string[] movieCategory;
    string[] movieDescription;
    string[] movieSize;
    string[] movieSizeCompany;
    string[] movieImage;
    string[] movieScreenshot1;
    string[] movieScreenshot2;
    string[] movieScreenshot3;
    string[] Torrent;
    string[] Trailer;

    string linkMagnet="";
    string watchTrailer = "";
    int currentMovieID = 0;

    public GameObject ResultTextGO;

    [Header("TopBars")]
    public GameObject MainMenuButton;
    public GameObject BackButton;
    public InputField searchInputField;

    private GameObject MoreButtonPrivate;
    private GameObject ReloadButtonPrivate;

    [Header("Search Page")]
    public GameObject SearchPage;
    public GameObject Content;
    public Transform PlaceContent;

    public GameObject MoreButton;
    public GameObject ReloadButton;

    [Header("Content Page")]
    public GameObject ContentPage;
    public Text MovieNameText;
    public Text RatingText;
    public Text SizeText;
    public Text CategoryText;
    public Text YearText;
    public Image ProfileImage;
    public Image ScreenshotImage1;
    public Image ScreenshotImage2;
    public Image ScreenshotImage3;
    public Image TrailerImage;
    public Text DescriptionText;
    public InputField MagnetLinkText;

    public GameObject FullScreenScreenShot;

    public GameObject FullScreenshot1;
    public GameObject FullScreenshot2;
    public GameObject FullScreenshot3;

    void Start()
    {
        ratingInput = "";
        genreInput = "";
        yearInput = "";
        saveload.Load();
        MainMenuButton.SetActive(true);
        BackButton.SetActive(false);
        Updatepannel.SetActive(false);
        ReportPannel.SetActive(false);
        SharePannel.SetActive(false);
        Feedbackpannel.SetActive(false);
        OnDashbordBackButtonPressed();

        inappropriateData = "0";
        downloadingLinkNotWorking = "0";
        others = "0";

        ActivatePanel(SearchPage.name);
        GetDataa();
        StartCoroutine(DelayAndAnalyse());
        StartCoroutine(WaitAndCloseResultText("Loading..."));
        ShowAds(300);
    }

    IEnumerator DelayAndAnalyse()
    {
        yield return new WaitForSeconds(5);
        CheckVersion();
        CheckIfAccountExist();
    }

    #region DashBord

    [Header("Dashbord")]
    public GameObject BackBigUi;
    public GameObject Dashbord;
    public GameObject Feedbackpannel;
    public GameObject SharePannel;

    public InputField FeedbackMessage;
    public InputField ShareInputField;


    public void OnDashBordButtonPressed()
    {
        BackBigUi.SetActive(true);
        Dashbord.SetActive(true);
    }

    public void OnFeedBackButtonPressed()
    {
        Feedbackpannel.SetActive(true);
    }

    public void OnShareButtonPressed()
    {
        SharePannel.SetActive(true);
        ShareInputField.text = newAppDownloadURL;
    }

    public void OnFeedbackSubmitButtonPressed()
    {
        string msg = FeedbackMessage.text;
        StartCoroutine(SendFeedback(msg));
        OnFeedbackCloseButtonPressed();
        WaitAndCloseResultText("Submitted");
    }

    IEnumerator SendFeedback(string msg)
    {
        WWWForm form1 = new WWWForm();
        form1.AddField("id", saveload.accountID);
        form1.AddField("msg", msg);
        WWW www = new WWW(givefeedbackLink, form1);
        yield return www;
    }

    public void OnFeedbackCloseButtonPressed()
    {
        Feedbackpannel.SetActive(false);
    }
    public void OnShareCloseButtonPressed()
    {
        SharePannel.SetActive(false);
    }

    public void OnDashbordBackButtonPressed()
    {
        Dashbord.SetActive(false);
        BackBigUi.SetActive(false);
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }

    #endregion

    #region Create Account OnServer and get notification

    [Header("Notification")]
    public GameObject NotificationPannel;
    public Text Message;
    public Text SubmitButtonText;
    public Image MessageImage;

    private string NotificationSubmitButton;

    void CheckIfAccountExist()
    {
        print(saveload.accountID);
        NotificationSubmitButton = "";
        NotificationPannel.SetActive(false);
        if (saveload.playerName == " ")
        {
            //create account
            string createRandomAccount = UnityEngine.Random.Range(10000, 1000000).ToString();
            StartCoroutine(CreateAccountToServer(createRandomAccount));
        }
        else
        {
            //check notification
            StartCoroutine(GetNotification());
        }
    }


    IEnumerator GetNotification()
    {
        WWWForm form1 = new WWWForm();
        form1.AddField("id", saveload.accountID);
        WWW www = new WWW(checkNotificationLink, form1);
        yield return www;

        
        if (www.text.Contains("Notification"))
        {
            
            if (GetDataValue(www.text, "Notification:") =="1")
            {
                
                if (GetDataValue(www.text, "Message:") != "" || GetDataValue(www.text, "Message:") != " ")
                {
                    Message.text = GetDataValue(www.text, "Message:");
                }

                if (GetDataValue(www.text, "Image:") != "" || GetDataValue(www.text, "Image:") != " ")
                {
                    
                    string s=GetDataValue(www.text, "ImageLink:");
                    print(s);
                    StartCoroutine( PlaceImageToObject(MessageImage, s,0));
                }

                if (GetDataValue(www.text, "DownloadButton:") != "" || GetDataValue(www.text, "DownloadButton:") != " ")
                {
                    SubmitButtonText.text=GetDataValue(www.text, "DownloadName:");
                    NotificationSubmitButton = GetDataValue(www.text, "DownloadLink:");
                }

                StartCoroutine(ActivateNotificationPannel());
            }
        }

    }

    IEnumerator ActivateNotificationPannel()
    {
        yield return new WaitForSeconds(3);
        NotificationPannel.SetActive(true);
    }

    IEnumerator CreateAccountToServer(string randomname)
    {
        WWWForm form1 = new WWWForm();
        form1.AddField("name", randomname);
        WWW www = new WWW(createAccountLink, form1);
        yield return www;
        
        if (www.text.Contains("ID"))
        {
            saveload.accountID = GetDataValue(www.text, "ID:");
            saveload.playerName = randomname;
            saveload.Save();
            
        }

    }

    public void OnNotificationDownloadButtonPressed()
    {
        if (NotificationSubmitButton != "")
        {
            Application.OpenURL(NotificationSubmitButton);
        }
    }

    public void OnNotificationCloseButtonPressed()
    {
        NotificationPannel.SetActive(false);
    }

    #endregion

    #region get app version

    [Header("Update App")]
    public GameObject Updatepannel;

    void CheckVersion()
    {
        StartCoroutine(GetVersion());
    }

    IEnumerator GetVersion()
    {
        WWW www = new WWW(getappversionLink);
        yield return www;

        if (www.text != "")
        {
            if (www.text.Contains("AppVersion"))
            {
                string ver = GetDataValue(www.text, "AppVersion:");
                print(ver);
                if (ver == saveload.appVersion)
                {
                    //same version
                    print("Same Version");
                }
                else
                {
                    //update new version
                    Updatepannel.SetActive(true);
                }
            }
        }

    }

    public void OnDownloadNewVersionButtonPressed()
    {
        Application.OpenURL(newAppDownloadURL);
    }

    public void OnUpdateAppCloseButtonPressed()
    {
        Updatepannel.SetActive(false);
    }

    #endregion

    #region app report

    [Header("Report")]
    public GameObject ReportPannel;
    public Image InappropriateImage;
    public Image LinkNotWorkImage;
    public Image OtherImage;
    public Sprite TickSprite;
    public InputField ReportMessage;

    private string inappropriateData;
    private string downloadingLinkNotWorking;
    private string others;
    private string message;

    public void OnReportButtonPressed()
    {
        ReportPannel.SetActive(true);
    }

    public void OnInappropriateButtonPressed()
    {
        if (inappropriateData=="1")
        {
            inappropriateData = "0";
            InappropriateImage.sprite = null;
        }
        else
        {
            inappropriateData = "1";
            InappropriateImage.sprite = TickSprite;
        }
    }

    public void OnDownloadingLinkNotWorkButtonPressed()
    {
        if (downloadingLinkNotWorking=="1")
        {
            downloadingLinkNotWorking = "0";
            LinkNotWorkImage.sprite = null;
        }
        else
        {
            downloadingLinkNotWorking = "1";
            LinkNotWorkImage.sprite = TickSprite;
        }
    }

    public void OnOthersButtonPressed()
    {
        if (others=="1")
        {
            others = "0";
            OtherImage.sprite = null;
        }
        else
        {
            others = "1";
            OtherImage.sprite = TickSprite;
        }
    }

    public void OnSubmitReportButtonPressed()
    {
        string id = movieID[currentMovieID];
        message = ReportMessage.text;
        StartCoroutine(SubmitReportData(id, inappropriateData, downloadingLinkNotWorking, others, message));
        ReportPannel.SetActive(false);
        StartCoroutine(WaitAndCloseResultText("Submitted"));
    }

    IEnumerator SubmitReportData(string id,string inapp,string linknot,string other,string msg)
    {
        WWWForm form1 = new WWWForm();
        form1.AddField("id", id);
        form1.AddField("account", saveload.accountID);
        form1.AddField("inapp", inapp);
        form1.AddField("linknot", linknot);
        form1.AddField("other", other);
        form1.AddField("msg", msg);
        WWW www = new WWW(reportMovieLink, form1);
        yield return www;

    }

    public void OnReportPannelCloseButtonPressed()
    {
        ReportPannel.SetActive(false);
    }

    #endregion

    #region get Data for Home Page
    //-------------------------------------------get Data for Home Page------------------------------

    void GetDataa()
    {
        StartCoroutine(DataHome());
    }

    IEnumerator DataHome()
    {
        WWW www = new WWW(getMovieDataHome);
        yield return www;

           if (www.text.Contains("ID"))
            {
                string itemsDataString = www.text;
                items = itemsDataString.Split(';');
                int len = items.Length;
                movieID = new string[len];
                movieName = new string[len];
                movieRating = new string[len];
                movieYear = new string[len];
                movieCategory = new string[len];
                movieDescription = new string[len];
                movieSize = new string[len];
                movieSizeCompany = new string[len];
                movieImage = new string[len];
                movieScreenshot1 = new string[len];
                movieScreenshot2 = new string[len];
                movieScreenshot3 = new string[len];
                Torrent = new string[len];
                Trailer = new string[len];
                for (int i = 0; i < items.Length - 1; i++)
                {
                    movieID[i] = GetDataValue(items[i], "ID:");
                    movieName[i] = GetDataValue(items[i], "Name:");
                    movieRating[i] = GetDataValue(items[i], "Rating:");
                    movieYear[i] = GetDataValue(items[i], "Year:");
                    movieCategory[i] = GetDataValue(items[i], "Category:");
                    //movieDescription[i] = GetDataValue(items[i], "Description:");
                    movieDescription[i] = "No Data";
                    movieSize[i] = GetDataValue(items[i], "Size:");
                    movieSizeCompany[i] = GetDataValue(items[i], "SizeCompany:");
                    movieImage[i] = GetDataValue(items[i], "Image:");
                    movieScreenshot1[i] = GetDataValue(items[i], "Screenshot1:");
                    movieScreenshot2[i] = GetDataValue(items[i], "Screenshot2:");
                    movieScreenshot3[i] = GetDataValue(items[i], "Screenshot3:");
                    Torrent[i] = GetDataValue(items[i], "MagentLink:");
                    Trailer[i] = GetDataValue(items[i], "Trailer:");


                    GameObject go = Instantiate(Content);
                    go.transform.SetParent(PlaceContent.transform);
                    go.transform.localScale = Vector3.one;
                    go.transform.Find("MovieName").GetComponent<Text>().text = movieName[i] + " (" + movieYear[i] + ")";
                    go.transform.Find("MovieRating").GetComponent<Text>().text = movieRating[i];
                    int sa = i;
                    go.transform.GetComponent<Button>().onClick.AddListener(() => OnContentButtonPressed(sa));
                    StartCoroutine(PlaceImageToObject(go.transform.Find("ProfilePhoto").GetComponent<Image>(), movieImage[i], 0));

                }
                GameObject g = Instantiate(MoreButton);
                g.transform.SetParent(PlaceContent.transform);
                g.transform.localScale = Vector3.one;
                GameObject gsa = g;
                g.transform.Find("MoreButton").GetComponent<Button>().onClick.AddListener(() => MoreButtonPressed(gsa));
                
            }
            else
            {
                //dont know
                StartCoroutine( WaitAndCloseResultText("Error"));
                GameObject g = Instantiate(ReloadButton);
                g.transform.SetParent(PlaceContent.transform);
                g.transform.localScale = Vector3.one;
                g.transform.Find("ReloadButton").GetComponent<Button>().onClick.AddListener(() => ReloadButtonPressed());
            }
        
    }

    public void MoreButtonPressed(GameObject g)
    {
        Destroy(g);
        GetDataa();

    }

    public void ReloadButtonPressed()
    {
        SceneManager.LoadScene(0);
    }

    #endregion

    #region get Data for Search Page
    //-------------------------------------------get Data for Search Page------------------------------
    public Dropdown ratingDropdown;
    public Dropdown genreDropdown;
    public Dropdown yearDropdown;

    private string searchedItem;
    private string ratingInput;
    private string genreInput;
    private string yearInput;

    private string position="";
    private string lengthTotal;

    private GameObject MoveButton;
    private bool isSearchedButtonPressed=false;

    public void OnRatingChanged()
    {
        ratingInput = ratingDropdown.options[ratingDropdown.value].text;
        
    }

    public void OnGenreChanged()
    {
        genreInput = genreDropdown.options[genreDropdown.value].text;
        
    }
    public void OnYearChanged()
    {
        yearInput = yearDropdown.options[yearDropdown.value].text;
        if (yearInput.Contains("Before 2000"))
        {
            yearInput = "19";
        }
    }


    public void GetSearchDataa()
    {
        if (isSearchedButtonPressed == false)
        {
            WaitAndCloseResultText("Searching");
            position = "";
            isSearchedButtonPressed = true;
            OnBackButtonPressed();
            GameObject[] go = GameObject.FindGameObjectsWithTag("Box");
            foreach (GameObject g in go)
            {
                Destroy(g);
            }
            Destroy(MoreButtonPrivate);
            Destroy(ReloadButtonPrivate);
            string str = "";
            str = searchInputField.text;
            searchedItem = str;
            if (str != null || str != "" || str != " ")
            {
                StartCoroutine(SearchDataHome());
            }
        }
    }

    IEnumerator SearchDataHome()
    {
        if (ratingInput == "Rating")
            ratingInput = "";
        if (genreInput == "Genre")
            genreInput = "";
        if (yearInput == "Year")
            yearInput = "";
        ratingInput = ratingInput.Replace("+", "");
        WWWForm form1 = new WWWForm();
        print(ratingInput);
        print(genreInput);
        form1.AddField("pos", position);
        form1.AddField("id", saveload.accountID);
        form1.AddField("rating",ratingInput);
        form1.AddField("genre", genreInput);
        form1.AddField("year", yearInput);
        form1.AddField("thing", searchedItem);
        WWW www = new WWW(getMovieSearchDataHome, form1);
        yield return www;
        isSearchedButtonPressed = false;
        print(www.text);
        
            if (www.text.Contains("ID"))
            {
                string itemsDataString = www.text;
                items = itemsDataString.Split(';');
                int len = items.Length;
                movieName = new string[len];
                movieRating = new string[len];
                movieYear = new string[len];
                movieCategory = new string[len];
                movieDescription = new string[len];
                movieSize = new string[len];
                movieSizeCompany = new string[len];
                movieImage = new string[len];
                movieScreenshot1 = new string[len];
                movieScreenshot2 = new string[len];
                movieScreenshot3 = new string[len];
                Torrent = new string[len];
                Trailer = new string[len];

                for (int i = 0; i < items.Length - 1; i++)
                {
                    print(i);
                    position = GetDataValue(items[i], "Position:");
                    lengthTotal = GetDataValue(items[i], "Length:");
                    movieID[i] = GetDataValue(items[i], "ID:");
                    movieName[i] = GetDataValue(items[i], "Name:");
                    movieRating[i] = GetDataValue(items[i], "Rating:");
                    movieYear[i] = GetDataValue(items[i], "Year:");
                    movieCategory[i] = GetDataValue(items[i], "Category:");
                    //movieDescription[i] = GetDataValue(items[i], "Description:");
                    movieDescription[i] = "No Data";
                    movieSize[i] = GetDataValue(items[i], "Size:");
                    movieSizeCompany[i] = GetDataValue(items[i], "SizeCompany:");
                    movieImage[i] = GetDataValue(items[i], "Image:");
                    movieScreenshot1[i] = GetDataValue(items[i], "Screenshot1:");
                    movieScreenshot2[i] = GetDataValue(items[i], "Screenshot2:");
                    movieScreenshot3[i] = GetDataValue(items[i], "Screenshot3:");
                    Torrent[i] = GetDataValue(items[i], "MagentLink:");
                    Trailer[i] = GetDataValue(items[i], "Trailer:");

                    GameObject go = Instantiate(Content);
                    go.transform.SetParent(PlaceContent.transform);
                    go.transform.localScale = Vector3.one;
                    go.transform.Find("MovieName").GetComponent<Text>().text = movieName[i] + " (" + movieYear[i] + ")";
                    go.transform.Find("MovieRating").GetComponent<Text>().text = movieRating[i];
                    int sa = i;
                    go.transform.GetComponent<Button>().onClick.AddListener(() => OnContentButtonPressed(sa));
                    StartCoroutine(PlaceImageToObject(go.transform.Find("ProfilePhoto").GetComponent<Image>(), movieImage[i], 0));
                    
                }
                print("Length" + position);
                print(lengthTotal);
                if(Convert.ToInt32( position) < Convert.ToInt32(lengthTotal))
                {
                    GameObject g = Instantiate(MoreButton);
                    g.transform.SetParent(PlaceContent.transform);
                    g.transform.localScale = Vector3.one;
                    MoveButton = g;
                    g.transform.Find("MoreButton").GetComponent<Button>().onClick.AddListener(() => SearchAgainafterclickonMoreButton());
                    
                }
            }else
            {
               
                //dont know
                StartCoroutine( WaitAndCloseResultText("Error"));
                GameObject g = Instantiate(ReloadButton);
                g.transform.SetParent(PlaceContent.transform);
                g.transform.localScale = Vector3.one;
                g.transform.GetComponent<Button>().onClick.AddListener(() => ReloadButtonPressed());
            
            }
            
        
    }

    void SearchAgainafterclickonMoreButton()
    {
        Destroy(MoveButton);
        StartCoroutine(SearchDataHome());
    }

    #endregion

    #region Common Method

    private bool isContentPage = false;
    public void OnContentButtonPressed(int index)
    {
        
        ActivatePanel(ContentPage.name);
        MainMenuButton.SetActive(false);
        BackButton.SetActive(true);

        MovieNameText.text = movieName[index].ToString();
        RatingText.text = movieRating[index].ToString();
        SizeText.text = movieSize[index].ToString();
        CategoryText.text = movieCategory[index].ToString();
        YearText.text = movieYear[index].ToString();
        StartCoroutine(PlaceImageToObject(ProfileImage, movieImage[index],0));
        StartCoroutine(PlaceImageToObject(ScreenshotImage1, movieScreenshot1[index],1));
        StartCoroutine(PlaceImageToObject(ScreenshotImage2, movieScreenshot2[index],2));
        StartCoroutine(PlaceImageToObject(ScreenshotImage3, movieScreenshot3[index],3));
        //StartCoroutine(PlaceImageToObject(TrailerImage, Torrent[index]));
        DescriptionText.text = movieDescription[index].ToString();
        MagnetLinkText.text = Torrent[index].ToString();

        linkMagnet = Torrent[index];
        watchTrailer = Trailer[index];
        currentMovieID = index;
    }

    public void OnWatchTrailerButtonPressed()
    {
        Application.OpenURL(watchTrailer);
    }

    public void OnBackButtonPressed()
    {
        ActivatePanel(SearchPage.name);
        MainMenuButton.SetActive(true);
        BackButton.SetActive(false);
    }

    public void OnMagnetButtonPressed()
    {
        Application.OpenURL(linkMagnet);
        
    }

    public void OnUTorrentButtonPressed()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.utorrent.client&hl=en");
    }

    public void OnBittorentButtonPressed()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.bittorrent.client&hl=en");
    }

    public void OpenSceenshotFullScreen(int n)
    {
        FullScreenScreenShot.SetActive(true);

        if (n == 1)
        {
            FullScreenshot1.SetActive(true);
            FullScreenshot2.SetActive(false);
            FullScreenshot3.SetActive(false);
        }

        if (n == 2)
        {
            FullScreenshot1.SetActive(false);
            FullScreenshot2.SetActive(true);
            FullScreenshot3.SetActive(false);
        }

        if (n == 3)
        {
            FullScreenshot1.SetActive(false);
            FullScreenshot2.SetActive(false);
            FullScreenshot3.SetActive(true);
        }
    }

    public void OnFullScreenCloseButtonPressed()
    {
        FullScreenScreenShot.SetActive(false);
    }

    public void ActivatePanel(string panelToBeActivated)
    {
        SearchPage.SetActive(panelToBeActivated.Equals(SearchPage.name));
        ContentPage.SetActive(panelToBeActivated.Equals(ContentPage.name));

        if (SearchPage.active)
        {
            isContentPage = false;
        }
        else
        {
            isContentPage = true;
        }
    }

    IEnumerator PlaceImageToObject(Image go, string URL,int num)
    {
        go.sprite = null;
        WWW www = new WWW(URL);
        yield return www;

        go.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
        if (isContentPage)
        {
            if (num == 1)
            {
                FullScreenshot1.GetComponent<Image>().sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
            }
            if (num == 2)
            {
                FullScreenshot2.GetComponent<Image>().sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
            }
            if (num == 3)
            {
                FullScreenshot3.GetComponent<Image>().sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
            }
        }
        
    }

    IEnumerator WaitAndCloseResultText(string msg)
    {
        ResultTextGO.SetActive(true);
        ResultTextGO.GetComponent<Text>().text = msg;
        yield return new WaitForSeconds(3);
        ResultTextGO.SetActive(false);
    }

    string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains(","))
            value = value.Remove(value.IndexOf(","));
        return value;
    }

    #endregion

    [Header("Ads")]
    public GameObject Adsd;

    void ShowAds(float time)
    {
        StartCoroutine(ShowAdsWait(time));
        
    }
    
    IEnumerator ShowAdsWait(float time)
    {
        yield return new WaitForSeconds(time);
        Adsd.GetComponent<Ads>().ShowInter();
        ShowAds(300+200);
    }
}
