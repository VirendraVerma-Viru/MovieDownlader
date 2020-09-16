using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GetData : MonoBehaviour
{
    public GameObject VideoPlayer;

    private string getMovieDataHome = "https://torrentodownloader.000webhostapp.com/Movies/getmovies.php";
    private string getMovieSearchDataHome = "https://torrentodownloader.000webhostapp.com/Movies/searchget.php";
    private string getappversionLink = "https://torrentodownloader.000webhostapp.com/Movies/getappversion.php";
    private string reportMovieLink = "https://torrentodownloader.000webhostapp.com/Movies/insertreport.php";
    private string createAccountLink = "https://torrentodownloader.000webhostapp.com/Movies/createaccount.php";
    private string checkNotificationLink = "https://torrentodownloader.000webhostapp.com/Movies/checkNotification.php";
    private string givefeedbackLink = "https://torrentodownloader.000webhostapp.com/Movies/insertfeedback.php";
    private string gethowtodownloadLink = "https://torrentodownloader.000webhostapp.com/Movies/gethowtodownloadLink.php";
    private string getyoutubeOrignalLink = "https://torrentodownloader.000webhostapp.com/Movies/getyoutubeorignallink.php";
    private string getdubbedmoveisLink = "https://torrentodownloader.000webhostapp.com/Movies/getdubbedmovies.php";
    private string getAdsDelayTime = "https://torrentodownloader.000webhostapp.com/Movies/getadstime.php";

    private string newAppDownloadURL = "https://torrentodownloader.000webhostapp.com/Movies/Torrento.apk";

    public string[] items;
    private string howtodownloadLink;
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
    string[] movieLanguage;

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
    public Text LanguageTextt;

    public GameObject FullScreenScreenShot;

    public GameObject FullScreenshot1;
    public GameObject FullScreenshot2;
    public GameObject FullScreenshot3;

    [Header("Dashbord main Text")]
    public Text MovieText;
    public Text DubbedMovieText;
    public Text MiniGameText;

    void Awake()
    {
        saveload.Load();  
    }

    void Start()
    {
        ratingInput = "";
        genreInput = "";
        yearInput = "";

        adsdelaytime = saveload.adsDelayTime;
        videoDelay = false;
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

        if (saveload.dubbedMovies)
        {
            print("DubbedMovies");
            GetDubbedMovies();
            MovieText.color = Color.white;
            DubbedMovieText.color = Color.green;
            MiniGameText.color = Color.white;
        }
        else
        {
            MovieText.color = Color.green;
            DubbedMovieText.color = Color.white;
            MiniGameText.color = Color.white;
            GetDataa();
        }
        StartCoroutine(DelayAndAnalyse());
        StartCoroutine(WaitAndCloseResultText("Loading...",3));
        ShowAds(adsdelaytime);

        if (saveload.isFirstTime == false)
        {
            saveload.isFirstTime = true;
            saveload.Save();
            OnDashBordButtonPressed();
        }

        if (saveload.isGamePannelOn > 0)
        {
            OnMiniGamesButtonPressed();
            saveload.isGamePannelOn = 0;
            saveload.Save();
        }
        else
        {
            MiniGamePannel.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ActivatePanel(SearchPage.name);
            //GetDataa();
        }
    }

    IEnumerator DelayAndAnalyse()
    {
        HowtodownloadbuttonContent.SetActive(false);
        Howtodownloadbuttonondashbord.SetActive(false);
        yield return new WaitForSeconds(5);
        CheckVersion();
        yield return new WaitForSeconds(0.5f);
        CheckIfAccountExist();
        yield return new WaitForSeconds(0.5f);
        GetHowToDownloadLink();
        yield return new WaitForSeconds(0.5f);
        GetAdsDelayTime();
    }

    #region DashBord

    #region Main Fuctions
    [Header("Dashbord")]
    public GameObject BackBigUi;
    public GameObject Dashbord;
    public GameObject MiniGamePannel;

    #region MainPannel

    public void OnTorrentoButtonPressed()
    {
        SceneManager.LoadScene(0);
    }

    public void OnMoviesButtonPressed()
    {
        saveload.dubbedMovies = false;
        saveload.Save();
        Dashbord.SetActive(false);
        MiniGamePannel.SetActive(false);
        SceneManager.LoadScene(0);
    }
    public void OnDubbedMoviesButtonPressed()
    {
        saveload.dubbedMovies = true;
        saveload.Save();
        Dashbord.SetActive(false);
        MiniGamePannel.SetActive(false);
        SceneManager.LoadScene(0);
    }
    public void OnMiniGamesButtonPressed()
    {
        Dashbord.SetActive(false);
        MiniGamePannel.SetActive(true);
        MovieText.color = Color.white;
        DubbedMovieText.color = Color.white;
        MiniGameText.color = Color.green;
    }

    #endregion

    private bool isdashbordOpen = false;
    public void OnDashBordButtonPressed()
    {
        if (isdashbordOpen == false)
        {
            isdashbordOpen = true;
            BackBigUi.SetActive(true);
            Dashbord.SetActive(true);
        }
        else
        {
            isdashbordOpen = false;
            BackBigUi.SetActive(false);
            Dashbord.SetActive(false);
        }
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

    #region how to download

    public void OnHowToDownloadButtonPressed()
    {
        Application.OpenURL(howtodownloadLink);
    }

    #endregion

    #region Download new version

    [Header("Download new version")]
    public GameObject DownloadNewVersionButtonOnDashbord;
    private bool isNewVersion = false;
    void CheckIfNewVersion()
    {
        if (isNewVersion)
        {
            //show download new version Button
            DownloadNewVersionButtonOnDashbord.SetActive(true);
        }
        else
        {
            DownloadNewVersionButtonOnDashbord.SetActive(false);
        }
    }

    public void OnShowDownloadNewVersionButtonPressed()
    {
        Updatepannel.SetActive(true);
    }

    #endregion

    #region Feedback

    [Header("Feedback")]
    public GameObject Feedbackpannel;
    public InputField FeedbackMessage;

    public void OnFeedBackButtonPressed()
    {
        Feedbackpannel.SetActive(true);
    }



    public void OnFeedbackSubmitButtonPressed()
    {
        string msg = FeedbackMessage.text;
        StartCoroutine(SendFeedback(msg));
        OnFeedbackCloseButtonPressed();
        StartCoroutine(WaitAndCloseResultText("Submitted", 3));
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

    #endregion

    #region Privacy Policy

    [Header("Privacy Policy")]
    public GameObject PrivacyPolicyPannel;
    public void OnPrivacyPolicyButtonPressed()
    {
        PrivacyPolicyPannel.SetActive(true);
    }

    public void OnCloseaPrivacyPolicyButtonPressed()
    {
        PrivacyPolicyPannel.SetActive(false);
    }

    #endregion

    #region share

    [Header("Share")]
    public GameObject SharePannel;
    public InputField ShareInputField;

    public void OnShareButtonPressed()
    {
        SharePannel.SetActive(true);
        ShareInputField.text = newAppDownloadURL;
    }

    public void OnShareCloseButtonPressed()
    {
        SharePannel.SetActive(false);
    }

    #endregion


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
        if (saveload.accountID == " ")
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
        form1.AddField("appVersion", saveload.appVersion);
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
                    isNewVersion = true;
                    if(saveload.isnewversionClose==false)
                        Updatepannel.SetActive(true);
                }
            }
        }
        CheckIfNewVersion();

    }

    public void OnDownloadNewVersionButtonPressed()
    {
        Application.OpenURL(newAppDownloadURL);
    }

    public void OnUpdateAppCloseButtonPressed()
    {
        saveload.isnewversionClose = true;
        saveload.Save();
        Updatepannel.SetActive(false);
    }

    #endregion

    #region get add delay time

    void GetAdsDelayTime()
    {
        StartCoroutine(GetAdsDelayTimeWait());
    }

    IEnumerator GetAdsDelayTimeWait()
    {
        WWW www = new WWW(getAdsDelayTime);
        yield return www;

        if (www.text != "")
        {
            if (www.text.Contains("Ads"))
            {
                string adsTime = GetDataValue(www.text, "Ads:");
                print("Ads" + adsTime);
                saveload.adsDelayTime =Convert.ToInt32(adsTime);
                adsdelaytime = saveload.adsDelayTime;
                saveload.Save();
                
            }
        }
        
    }

    #endregion

    #region Get How TO Download Link

    [Header("How to download")]
    public GameObject Howtodownloadbuttonondashbord;
    public GameObject HowtodownloadbuttonContent;

    void GetHowToDownloadLink()
    {

        StartCoroutine(GetHowToDownloadLinkWait());
    }

    IEnumerator GetHowToDownloadLinkWait()
    {
        WWW www = new WWW(gethowtodownloadLink);
        yield return www;
        print(www.text);

        if (www.text.Contains("Link"))
        {
            string Linkn = GetDataValue(www.text, "Link:");
            howtodownloadLink = Linkn;
            HowtodownloadbuttonContent.SetActive(true);
            Howtodownloadbuttonondashbord.SetActive(true);
        }
        else
        {
            HowtodownloadbuttonContent.SetActive(false);
            Howtodownloadbuttonondashbord.SetActive(false);
        }
        
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
        StartCoroutine(WaitAndCloseResultText("Submitted",3));
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
        print(www.text);
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
                movieLanguage = new string[len];
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
                    movieLanguage[i] = GetDataValue(items[i], "Language:");

                    GameObject go = Instantiate(Content);
                    go.transform.SetParent(PlaceContent.transform);
                    go.transform.localScale = Vector3.one;
                    go.transform.Find("MovieName").GetComponent<Text>().text = movieName[i] + " (" + movieYear[i] + ")";
                    go.transform.Find("MovieRating").GetComponent<Text>().text = movieRating[i];
                    go.transform.Find("Category").GetComponent<Text>().text = movieCategory[i];
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
                StartCoroutine( WaitAndCloseResultText("Error",3));
                GameObject g = Instantiate(ReloadButton);
                g.transform.SetParent(PlaceContent.transform);
                g.transform.localScale = Vector3.one;
                g.transform.Find("ReloadButton").GetComponent<Button>().onClick.AddListener(() => ReloadButtonPressed(0));
            }
        
    }

    public void MoreButtonPressed(GameObject g)
    {
        print("More");
        Destroy(g);
        GetDataa();

    }

    public void ReloadButtonPressed(int t)
    {
        SceneManager.LoadScene(0);
    }

    #endregion

    #region Get Dubbed Movies

    void GetDubbedMovies()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("Box");
        foreach (GameObject g in go)
        {
            Destroy(g);
        }
        position = "";
        StartCoroutine( WaitAndCloseResultText("Loading",0));
        StartCoroutine(GetDubbedMoviesFromURL());
    }

    IEnumerator GetDubbedMoviesFromURL()
    {
        WWWForm form1 = new WWWForm();
        form1.AddField("pos", position);
        WWW www = new WWW(getdubbedmoveisLink, form1);
        yield return www;
        print("Hello:"+www.text);
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
            movieLanguage = new string[len];
            for (int i = 0; i < items.Length - 1; i++)
            {
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
                movieLanguage[i] = GetDataValue(items[i], "Language:");

                GameObject go = Instantiate(Content);
                go.transform.SetParent(PlaceContent.transform);
                go.transform.localScale = Vector3.one;
                go.transform.Find("MovieName").GetComponent<Text>().text = movieName[i] + " (" + movieYear[i] + ")";
                go.transform.Find("MovieRating").GetComponent<Text>().text = movieRating[i];
                go.transform.Find("Category").GetComponent<Text>().text = movieCategory[i];
                int sa = i;
                go.transform.GetComponent<Button>().onClick.AddListener(() => OnContentButtonPressed(sa));
                StartCoroutine(PlaceImageToObject(go.transform.Find("ProfilePhoto").GetComponent<Image>(), movieImage[i], 0));

            }

            print("Length" + position);
            print(lengthTotal);
            if (Convert.ToInt32(position) < Convert.ToInt32(lengthTotal))
            {
                GameObject g = Instantiate(MoreButton);
                g.transform.SetParent(PlaceContent.transform);
                g.transform.localScale = Vector3.one;
                MoveButton = g;
                g.transform.Find("MoreButton").GetComponent<Button>().onClick.AddListener(() => SearchAgainafterclickonMoreButtonDubbed());

            }

        }
        else
        {
            //dont know
            StartCoroutine(WaitAndCloseResultText("Error", 3));
            GameObject g = Instantiate(ReloadButton);
            g.transform.SetParent(PlaceContent.transform);
            g.transform.localScale = Vector3.one;
            g.transform.Find("ReloadButton").GetComponent<Button>().onClick.AddListener(() => ReloadButtonPressed(0));
        }
    }

    void SearchAgainafterclickonMoreButtonDubbed()
    {
        Destroy(MoveButton);
        StartCoroutine(GetDubbedMoviesFromURL());
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
            ResultTextGO.SetActive(true);
            ResultTextGO.GetComponent<Text>().text = "Searching...";
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
        print(www.text);
        ResultTextGO.GetComponent<Text>().text = "";
        ResultTextGO.SetActive(false);
        isSearchedButtonPressed = false;
        print(www.text);

        if (www.text.Contains("No Record is there"))
        {
            StartCoroutine(WaitAndCloseResultText("Not Found \n try to search with some other sort name",6));
        }
        else
        {
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
                movieLanguage = new string[len];

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
                    movieLanguage[i] = GetDataValue(items[i], "Language:");

                    GameObject go = Instantiate(Content);
                    go.transform.SetParent(PlaceContent.transform);
                    go.transform.localScale = Vector3.one;
                    go.transform.Find("MovieName").GetComponent<Text>().text = movieName[i] + " (" + movieYear[i] + ")";
                    go.transform.Find("MovieRating").GetComponent<Text>().text = movieRating[i];
                    go.transform.Find("Category").GetComponent<Text>().text = movieCategory[i];
                    int sa = i;
                    go.transform.GetComponent<Button>().onClick.AddListener(() => OnContentButtonPressed(sa));
                    StartCoroutine(PlaceImageToObject(go.transform.Find("ProfilePhoto").GetComponent<Image>(), movieImage[i], 0));

                }
                
                if (Convert.ToInt32(position) < Convert.ToInt32(lengthTotal))
                {
                    GameObject g = Instantiate(MoreButton);
                    g.transform.SetParent(PlaceContent.transform);
                    g.transform.localScale = Vector3.one;
                    MoveButton = g;
                    g.transform.Find("MoreButton").GetComponent<Button>().onClick.AddListener(() => SearchAgainafterclickonMoreButton());

                }

            }
            else
            {

                //dont know
                StartCoroutine(WaitAndCloseResultText("Error",3));
                GameObject g = Instantiate(ReloadButton);
                g.transform.SetParent(PlaceContent.transform);
                g.transform.localScale = Vector3.one;
                g.transform.GetComponent<Button>().onClick.AddListener(() => ReloadButtonPressed(0));

            }
        }
        
    }

    void SearchAgainafterclickonMoreButton()
    {
        Destroy(MoveButton);
        StartCoroutine(SearchDataHome());
    }

    #endregion

    #region Common Method

    #region Content Page 
    [Header("Content Things")]
    public GameObject LanguageContent;
    public GameObject RatingContent;
    public GameObject CategoryContent;
    public GameObject SizeContent;
    public GameObject ScreenShotContent;
    public GameObject ScreenShot1Content;
    public GameObject ScreenShot2Content;
    public GameObject ScreenShot3Content;
    public GameObject TrailerContent;
    public GameObject MagnetLinkContent;
    
    private bool isContentPage = false;
    public void OnContentButtonPressed(int index)
    {
        
        ActivatePanel(ContentPage.name);
        MainMenuButton.SetActive(false);
        BackButton.SetActive(true);
        MovieNameText.text = movieName[index].ToString();
        LanguageTextt.text = movieLanguage[index].ToString();
        RatingText.text = movieRating[index].ToString();
        SizeText.text = movieSize[index].ToString();
        CategoryText.text = movieCategory[index].ToString();
        YearText.text = movieYear[index].ToString();
        StartCoroutine(PlaceImageToObject(ProfileImage, movieImage[index],0));

        if (movieScreenshot1[index] != "")
        {
            ScreenShot1Content.SetActive(true);
            StartCoroutine(PlaceImageToObject(ScreenshotImage1, movieScreenshot1[index], 1));
        }
        else
        {
            ScreenShot1Content.SetActive(false);
        }
        if (movieScreenshot2[index] != "")
        {
            ScreenShot2Content.SetActive(true);
            StartCoroutine(PlaceImageToObject(ScreenshotImage2, movieScreenshot2[index], 2));
        }
        else
        {
            ScreenShot2Content.SetActive(false);
        }
        if (movieScreenshot3[index] != "")
        {
            ScreenShot3Content.SetActive(true);
            StartCoroutine(PlaceImageToObject(ScreenshotImage3, movieScreenshot3[index], 3));
        }
        else
        {
            ScreenShot3Content.SetActive(false);
        }
        
        
        //StartCoroutine(PlaceImageToObject(TrailerImage, Torrent[index]));
        DescriptionText.text = movieDescription[index].ToString();
        MagnetLinkText.text = Torrent[index].ToString();

        linkMagnet = Torrent[index];
        watchTrailer = Trailer[index];
        currentMovieID = index;

        //send and get the youtube orignal video if exist
        StartCoroutine(GetTheYoutubeOrignalLink(watchTrailer));

        //test and set actives
        CheckAndOff(movieLanguage[index].ToString(),movieRating[index].ToString(), movieCategory[index].ToString(), movieSize[index].ToString(), movieScreenshot1[index].ToString(), Trailer[index].ToString(), Torrent[index].ToString());
    }

    void CheckAndOff(string language,string rating,string category,string size,string screenshot,string trailer,string magnetlink)
    {
        if (language == "")
        {
            LanguageContent.SetActive(false);
        }
        else
        {
            LanguageContent.SetActive(true);
        }

        if (rating == "")
        {
            RatingContent.SetActive(false);
        }
        else
        {
            RatingContent.SetActive(true);
        }

        if (category == "")
        {
            CategoryContent.SetActive(false);
        }
        else
        {
            CategoryContent.SetActive(true);
        }

        if (size == "")
        {
            SizeContent.SetActive(false);
        }
        else
        {
            SizeContent.SetActive(true);
        }

        if (screenshot == "")
        {
            ScreenShotContent.SetActive(false);
        }
        else
        {
            ScreenShotContent.SetActive(true);
        }

        if (trailer == "")
        {
            TrailerContent.SetActive(false);
        }
        else
        {
            TrailerContent.SetActive(true);
        }

        if (magnetlink == "")
        {
            MagnetLinkContent.SetActive(false);
        }
        else
        {
            MagnetLinkContent.SetActive(true);
        }

    }

    IEnumerator GetTheYoutubeOrignalLink(string youtubelink)
    {
        WWWForm form1 = new WWWForm();
        form1.AddField("youtube", youtubelink);
        form1.AddField("id", saveload.accountID);
        WWW www = new WWW(getyoutubeOrignalLink, form1);
        yield return www;
        if (www.text.Contains("Youtube"))
        {
            
            youtubemainLink = GetDataValue(www.text, "Youtube:");
            
            if (youtubemainLink == "" || youtubemainLink == " " || youtubemainLink==null)
            {
                
                TrailerContent.SetActive(false);
            }
            else
            {
                
                TrailerContent.SetActive(true);
            }
        }
        else
        {
            TrailerContent.SetActive(false);
            
        }
    }

    private string youtubemainLink="";
    public void OnWatchTrailerButtonPressed()
    {
        if (youtubemainLink != "")
        {
            VideoPlayer.GetComponent<VideoPlayerController>().WatchVideo(youtubemainLink);
        }
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

    #endregion

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

    IEnumerator WaitAndCloseResultText(string msg,float time)
    {
        ResultTextGO.SetActive(true);
        ResultTextGO.GetComponent<Text>().text = msg;
        yield return new WaitForSeconds(time);
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
    public bool videoDelay = false;
    private int adsdelaytime = 100;
    void ShowAds(float time)
    {
        StartCoroutine(ShowAdsWait(time));
        
    }
    
    IEnumerator ShowAdsWait(float time)
    {
        yield return new WaitForSeconds(time);

        if (videoDelay == false)
        {
            Adsd.GetComponent<Ads>().ShowInter();
            ShowAds(adsdelaytime);
        }
        else
        {
            ShowAds(20);
        }
    }
}
