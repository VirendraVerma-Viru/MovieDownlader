using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class Ads : MonoBehaviour
{
    public string gameId = "3640873";
    public string placementId = "9452";
    public bool testMode = true;

    void Start()
    {
        Advertisement.Initialize(gameId, testMode);
        StartCoroutine(ShowBannerWhenReady());
    }

    IEnumerator ShowBannerWhenReady()
    {
        while (!Advertisement.IsReady(placementId))
        {
            yield return new WaitForSeconds(0.5f);
        }
        //Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Show(placementId);
        print("Loaded");
        
    }

    public void ShowInter()
    {
        // Initialize the Ads service:
        Advertisement.Initialize(gameId, testMode);
        // Show an ad:
        Advertisement.Show();
    }
}

/*

string gameId = "1234567";
    bool testMode = true;

    void Start () {
        // Initialize the Ads service:
        Advertisement.Initialize (gameId, testMode);
        // Show an ad:
        Advertisement.Show ();
    }

*/