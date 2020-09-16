using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;

public class corsairsaveload : MonoBehaviour
{
    public static int highscore = 0;
    public static int currentscore = 0;
    public static int level = 1;
    public static int adsFrequency = 0;

    public static int isreturn = 0;

    public static string filename = "infocorsair.dat";
    public static void Save()
	{
		BinaryFormatter bf = new BinaryFormatter ();
        FileStream file = File.Create(Application.persistentDataPath + "/" + filename);
		Finance_Data data = new Finance_Data ();
		

		
		data.Highscore = highscore;
        data.CurrentScore = currentscore;
        data.Level = level;
        data.AdsFrequency = adsFrequency;

        data.Isreturn = isreturn;

        bf.Serialize(file, data);
        file.Close();
    }

    public static void Load()
	{

        if (File.Exists(Application.persistentDataPath + "/" + filename))
        {
			BinaryFormatter bf = new BinaryFormatter ();
            FileStream file = File.Open(Application.persistentDataPath + "/" + filename, FileMode.Open);
			Finance_Data data = (Finance_Data)bf.Deserialize (file);

			
			highscore = data.Highscore;
            currentscore=data.CurrentScore;
            level = data.Level;
            adsFrequency = data.AdsFrequency;

            isreturn = data.Isreturn;
            file.Close();

        }
        else
            corsairsaveload.Save();
    }

}


[Serializable]
class Finance_Data
{
    public int Highscore;
    public int CurrentScore;
    public int Level;
    public int AdsFrequency;
    public int Isreturn;
}


