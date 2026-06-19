using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Security;
using System.Security.Cryptography;
using System.Threading;

public class SetEncryptionKeys : MonoBehaviour
{
    private void Awake()
    {
        EncryptedPlayerPrefs.keys[0] = "23Wrudre";
        EncryptedPlayerPrefs.keys[1] = "SP9DupHa";
        EncryptedPlayerPrefs.keys[2] = "frA5rAS3";
        EncryptedPlayerPrefs.keys[3] = "tHat2epr";
        EncryptedPlayerPrefs.keys[4] = "jaw3eDAs";

        string loadedKey = LoadKey();
        if (loadedKey == "null")
        {
            print("No key saved, generating and saving new key.");
            string newKey = GenerateKey(12);
            saveKey(newKey);
            loadedKey = newKey;
        }
        else
        {
            print("Found saved key: " + loadedKey);
        }
        EncryptedPlayerPrefs.privateKey = loadedKey;
    }
    public static void saveKey(string key)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/key.jumpy";

        FileStream stream = new FileStream(path, FileMode.Create);

        string data = key;

        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static string GenerateKey(int length)
    {
        char[] chars = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', 'q', 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p', 'a', 's', 'd', 'f', 'g', 'g', 'h', 'j', 'k', 'l', 'z', 'x', 'c', 'v', 'b', 'n', 'm' };
        string key = "";
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < length; i++)
        {
            sb.Append(chars[Random.Range(0, chars.Length)]);
        }
        key = sb.ToString();
        return key;
    }
    public static string LoadKey()
    {
        string path = Application.persistentDataPath + "/key.jumpy";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            string key = formatter.Deserialize(stream) as string;
            stream.Close();

            return key;
        }
        else
        {
            //No file found
            FileStream stream = new FileStream(path, FileMode.Create);
            stream.Close();
            return "null";
        }
    }
}
