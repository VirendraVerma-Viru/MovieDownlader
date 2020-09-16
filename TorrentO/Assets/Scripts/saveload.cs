using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;


public class saveload : MonoBehaviour
{

    public static string appVersion = "0.2";

    public static string accountID = " ";
    public static string playerName = " ";
    public static int isGamePannelOn = 0;
    public static int adsDelayTime = 100;
    public static bool dubbedMovies = false;
    public static int usageTime = 0;
    public static bool isnewversionClose = false;
    public static bool isFirstTime = false;
    public static string current_filename = "info.dat";

    public static void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + current_filename);
        Notebook_Data data = new Notebook_Data();


        data.AccountID = accountID;
        data.PlayerName = playerName;
        data.DubbedMovies = dubbedMovies;
        data.IsGamePannelOn = isGamePannelOn;
        data.UsageTime = usageTime;
        data.IsNewVersionClose = isnewversionClose;
        data.IsFirstTime = isFirstTime;
        data.AdsDelayTime = adsDelayTime;
        
        bf.Serialize(file, data);
        file.Close();
    }

    public static void Load()
    {

        if (File.Exists(Application.persistentDataPath + "/" + current_filename))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + current_filename, FileMode.Open);/* */
            Notebook_Data data = (Notebook_Data)bf.Deserialize(file);

            accountID = data.AccountID;
            playerName = data.PlayerName;
            dubbedMovies = data.DubbedMovies;
            isGamePannelOn = data.IsGamePannelOn;
            usageTime = data.UsageTime;
            isnewversionClose = data.IsNewVersionClose;
            isFirstTime = data.IsFirstTime;
            adsDelayTime = data.AdsDelayTime;

            //accountID = "8";//---------------------------------remove this
            file.Close();
            appVersion = "0.2";
        }
        else
        {
            current_filename = "info.dat";
            accountID = " ";
            saveload.Save();

        }
    }

    private static string hash = "9452@abc";

    public static string Encrypt(string input)
    {
        byte[] data = UTF8Encoding.UTF8.GetBytes(input);
        using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        {
            byte[] key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            using (TripleDESCryptoServiceProvider trip = new TripleDESCryptoServiceProvider() { Key = key, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
            {
                ICryptoTransform tr = trip.CreateEncryptor();
                byte[] results = tr.TransformFinalBlock(data, 0, data.Length);
                return Convert.ToBase64String(results, 0, results.Length);
            }
        }
    }

    public static string Decrypt(string input)
    {
        byte[] data = Convert.FromBase64String(input);
        using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        {
            byte[] key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            using (TripleDESCryptoServiceProvider trip = new TripleDESCryptoServiceProvider() { Key = key, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
            {
                ICryptoTransform tr = trip.CreateDecryptor();
                byte[] results = tr.TransformFinalBlock(data, 0, data.Length);
                return UTF8Encoding.UTF8.GetString(results);
            }
        }
    }


}


[Serializable]
class Notebook_Data
{
    public string AccountID;
    public string PlayerName;
    public bool DubbedMovies;
    public int IsGamePannelOn;
    public int UsageTime;
    public bool IsNewVersionClose;
    public bool IsFirstTime;
    public int AdsDelayTime;
}