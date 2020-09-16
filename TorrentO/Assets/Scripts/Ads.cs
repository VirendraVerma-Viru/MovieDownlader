using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class Ads : MonoBehaviour
{
    string gameId = "3640873";
    string placementId = "9452";
    public bool testMode = false;

    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            Advertisement.Initialize(gameId, testMode);
            StartCoroutine(ShowBannerWhenReady());
        }
        
    }

    IEnumerator ShowBannerWhenReady()
    {
        while (!Advertisement.IsReady(placementId))
        {
            
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Show(placementId);
        
        
    }

    public void ShowInter()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            Advertisement.Show();
            Advertisement.Initialize(gameId, testMode);
        }
    }
}

