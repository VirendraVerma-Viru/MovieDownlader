using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private string getAdsDelayTime = "http://kreasaard.atwebpages.com/test.php";

    void Start()
    {
        StartCoroutine(SendFeedback());
    }
    IEnumerator SendFeedback()
    {
        WWWForm form1 = new WWWForm();
        form1.AddField("Hello", "This is fun");
        
        WWW www = new WWW(getAdsDelayTime, form1);
        yield return www;
        print("thishihsdfhidoiashfoiashfoiasf - "+www.text);
    }
}
