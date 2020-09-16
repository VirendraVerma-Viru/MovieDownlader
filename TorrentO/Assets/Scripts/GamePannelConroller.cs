using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePannelConroller : MonoBehaviour
{
    
    public GameObject LoadingPannel;
    public Image LoadingBar;
    public Text LoadingGameName;


    public void CorsairGameButtonPressed()
    {
        StartCoroutine(WaitForCorsairFlashDelay("Corsair","Corsairgame"));
    }

    public void TapTapGameButtonPressed()
    {
        StartCoroutine(WaitForCorsairFlashDelay("Tap Tap","taptap"));
    }

    public void MazeRunnerGameButtonPressed()
    {
        StartCoroutine(WaitForCorsairFlashDelay("Maze Runner", "Maze"));
    }

    public void ColorCollectorGameButtonPressed()
    {
        StartCoroutine(WaitForCorsairFlashDelay("Color Collector", "colorcollector"));
    }

    IEnumerator WaitForCorsairFlashDelay(string gamename,string sceneName)
    {
        LoadingPannel.SetActive(true);
        LoadingGameName.text = gamename;
        AsyncOperation game = SceneManager.LoadSceneAsync(sceneName);

        while (game.progress < 1)
        {
            LoadingBar.rectTransform.localScale = new Vector3(game.progress, 1, 1);
            yield return new WaitForEndOfFrame();
        }
    }
}
