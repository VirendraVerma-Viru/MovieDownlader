using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;

public class taptapsaveload : MonoBehaviour
{
    public static int highscore = 0;
    public static int currentscore = 0;
    public static int level = 1;
    public static int adsFrequency = 0;

    public static int isreturn = 0;
    public static string filename = "infotaptap.dat";
    public static void Save()
	{
		BinaryFormatter bf = new BinaryFormatter ();
        FileStream file = File.Create(Application.persistentDataPath + "/" + filename);
        TapTap_Data data = new TapTap_Data();
		

		data.Highscore = highscore;
        data.CurrentScore = currentscore;
        data.Level = level;
        data.Isreturn = isreturn;
        data.AdsFrequency = adsFrequency;

        bf.Serialize(file, data);
        file.Close();
    }

    public static void Load()
	{

        if (File.Exists(Application.persistentDataPath + "/" + filename))
        {
			BinaryFormatter bf = new BinaryFormatter ();
            FileStream file = File.Open(Application.persistentDataPath + "/" + filename, FileMode.Open);
            TapTap_Data data = (TapTap_Data)bf.Deserialize(file);

			highscore = data.Highscore;
            currentscore=data.CurrentScore;
            level = data.Level;
            isreturn = data.Isreturn;
            adsFrequency = data.AdsFrequency;
            
            file.Close();

        }
        else
            taptapsaveload.Save();
    }

}


[Serializable]
class TapTap_Data
{
    public int Highscore;
    public int CurrentScore;
    public int Level;
    public int Isreturn;
    public int AdsFrequency;
}


