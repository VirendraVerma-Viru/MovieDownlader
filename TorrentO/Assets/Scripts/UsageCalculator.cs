using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsageCalculator : MonoBehaviour
{
    private string getusageappLink = "https://torrentodownloader.000webhostapp.com/Movies/savetime.php";

    void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Usage");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
        Timer();
    }

    void Timer()
    {
        StartCoroutine(CalculateTimer());
    }

    IEnumerator CalculateTimer()
    {
        yield return new WaitForSeconds(15);
        saveload.usageTime += 15;
        saveload.Save();
        WWWForm form1 = new WWWForm();
        form1.AddField("id", saveload.accountID);
        form1.AddField("usage", saveload.usageTime);
        WWW www = new WWW(getusageappLink, form1);
        yield return www;
        StartCoroutine(CalculateTimer());
    }
}
