using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoPlayerController : MonoBehaviour
{
    public GameObject MainContoller;

    public GameObject VideoPlayerGO;
    public GameObject SmallVideoPlayerGO;
    public GameObject BigVideoPlayerGO;

    public GameObject VideoPlayerData;
    public Animator anim;

    private string videoURL1 = "https://r4---sn-gwpa-a0iz.googlevideo.com/videoplayback?expire=1593201971&ei=0wD2XrCHLo6gvwTUuo_YBg&ip=2405%3A201%3A6401%3Ac702%3A658f%3Ade61%3Ad2d%3A9756&id=o-AAg-n7ZE9NGhgz5j8if4xnS6m3CLQI04Xm4U6V2iyM-V&itag=18&source=youtube&requiressl=yes&mh=2x&mm=31%2C29&mn=sn-gwpa-a0iz%2Csn-qxaeen7e&ms=au%2Crdu&mv=m&mvi=3&pcm2cms=yes&pl=37&initcwndbps=1558750&vprv=1&mime=video%2Fmp4&gir=yes&clen=5952811&ratebypass=yes&dur=120.813&lmt=1550167003502127&mt=1593180283&fvip=5&c=WEB&txp=5531432&sparams=expire%2Cei%2Cip%2Cid%2Citag%2Csource%2Crequiressl%2Cvprv%2Cmime%2Cgir%2Cclen%2Cratebypass%2Cdur%2Clmt&sig=AOq0QJ8wRgIhAPe4FKHjweD8aeffXdiYH66MWSTHzZ70hZKdHWu9YDI-AiEAtWUz9Fj7pjaYX3HNZNHitHq0YMmiwuHvPN5Q0vI8bHs%3D&lsparams=mh%2Cmm%2Cmn%2Cms%2Cmv%2Cmvi%2Cpcm2cms%2Cpl%2Cinitcwndbps&lsig=AG3C_xAwRQIhAJuTtu9JtT4eoS88ym2IphAbg6IR8jBiTiKH2oUm8XJ3AiA5KW_Tnj9jiJ6LVF2uDsfjf5nHBPLi8OUxscTNyJC2iw%3D%3D";
    private string videoURL2= "https://r4---sn-gwpa-pmjs.googlevideo.com/videoplayback?expire=1592879244&ei=LBTxXumkDKTOz7sP56CEqAk&ip=2405%3A201%3A6401%3Ac702%3Ad4d8%3A1b87%3A5676%3A1188&id=o-AACSXwQ2c5Ypz6dhBzp5bJCNEYWnLi_JAAerDBq_Dm6e&itag=18&source=youtube&requiressl=yes&mh=C9&mm=31%2C29&mn=sn-gwpa-pmjs%2Csn-qxa7snel&ms=au%2Crdu&mv=m&mvi=3&pl=37&initcwndbps=860000&vprv=1&mime=video%2Fmp4&gir=yes&clen=67234769&ratebypass=yes&dur=1100.858&lmt=1592748592722713&mt=1592857541&fvip=4&c=WEB&txp=5511222&sparams=expire%2Cei%2Cip%2Cid%2Citag%2Csource%2Crequiressl%2Cvprv%2Cmime%2Cgir%2Cclen%2Cratebypass%2Cdur%2Clmt&sig=AOq0QJ8wRAIgGqQlrPZHshu6de0JhINsZ3NoKdUwpMmq8nEHlqhV630CIEpstiDdpxqwY2s56HnFWPgai6skmBfOlJK0huZPDHyf&lsparams=mh%2Cmm%2Cmn%2Cms%2Cmv%2Cmvi%2Cpl%2Cinitcwndbps&lsig=AG3C_xAwRgIhAKCQnK76SExpfRiEtZFLkERm2wvvKZhA2ngfK0wlyzwoAiEAmvg0HW1biA3imLcuWr57iKUBWr-IpkN8ONQ3_0umt1M%3D";

    [Header("VideoPlayerICOn")]
    public Image PlayPositionFullscreen;
    public Image PlayPositionHalfscreen;
    public Image MutePositionFullscreen;
    public Image MutePositionHalfscreen;

    public Sprite voiumeUpIcon;
    public Sprite MuteIcon;
    public Sprite PlayIcon;
    public Sprite PauseIcon;

    public void WatchVideo(string videoURL)
    {
        
        VideoPlayerGO.SetActive(true);
        SmallVideoPlayerGO.SetActive(true);
        BigVideoPlayerGO.SetActive(false);

        VideoPlayerData.GetComponent<VideoPlayer>().url = videoURL;
        VideoPlayerData.GetComponent<VideoPlayer>().Play();

        screenRotate = 0;
        isPlay = 0;
        isMute = 0;
        PlayPositionFullscreen.sprite = PauseIcon;
        PlayPositionHalfscreen.sprite = PauseIcon;
        MutePositionFullscreen.sprite = MuteIcon;
        MutePositionHalfscreen.sprite = MuteIcon;

        MainContoller.GetComponent<GetData>().videoDelay = true;

    }

    int screenRotate = 0;//0 means small screen 1 means full screen
    public void OnRotateScreenButtonPressed()
    {
        if (screenRotate == 0)
        {
            screenRotate = 1;
            SmallVideoPlayerGO.SetActive(false);
            BigVideoPlayerGO.SetActive(true);
            FullScreenTap();
        }
        else
        {
            screenRotate = 0;
            SmallVideoPlayerGO.SetActive(true);
            BigVideoPlayerGO.SetActive(false);
        }
    }

    bool isBarOnScreen=false;
    public void FullScreenTap()
    {
        if (isBarOnScreen == false)
        {
            isBarOnScreen = true;
            anim.Play("FullScreenWelcomecome");
            StartCoroutine(WaitToHide());
        }
        else
        {
            isBarOnScreen = false;
            anim.Play("FullScreenWelcom");
        }
    }

    IEnumerator WaitToHide()
    {
        yield return new WaitForSeconds(3);

        if (isBarOnScreen)
        {
            isBarOnScreen = false;
            anim.Play("FullScreenWelcom");
        }
    }

    int isPlay = 0;//0 means is playing and 1 means pause
    public void OnPlayPauseButtonPressed()
    {
        if (isPlay == 0)
        {
            isPlay = 1;
            VideoPlayerData.GetComponent<VideoPlayer>().Pause();
            PlayPositionFullscreen.sprite = PlayIcon;
            PlayPositionHalfscreen.sprite = PlayIcon;
            
        }
        else
        {
            isPlay = 0;
            VideoPlayerData.GetComponent<VideoPlayer>().Play();
            PlayPositionFullscreen.sprite = PauseIcon;
            PlayPositionHalfscreen.sprite = PauseIcon;
        }
    }

    int isMute = 0;// 0 means not mute and 1 means mute
    public void OnMuteButtonPressed()
    {
        if (isMute == 0)
        {
            isMute = 1;
            VideoPlayerData.GetComponent<VideoPlayer>().SetDirectAudioMute(0, true);
            MutePositionFullscreen.sprite = voiumeUpIcon;
            MutePositionHalfscreen.sprite = voiumeUpIcon;
        }
        else
        {
            isMute = 0;
            VideoPlayerData.GetComponent<VideoPlayer>().SetDirectAudioMute(0, false);
            MutePositionFullscreen.sprite = MuteIcon;
            MutePositionHalfscreen.sprite = MuteIcon;
        }
    }

    public void OnCloseButtonPressedWindow()
    {
        VideoPlayerGO.SetActive(false);
        VideoPlayerData.GetComponent<VideoPlayer>().Stop();
        MainContoller.GetComponent<GetData>().videoDelay = false;
    }

}
