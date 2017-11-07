using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

public class SaveNLoadTxt 
{
    public static void Save(Data commentData)
    {
        BinaryFormatter bf = new BinaryFormatter();
        if (!File.Exists(Application.dataPath + "/Inventory.txt"))
        {
            File.Create(Application.dataPath + "/Inventory.txt");
        }
        FileStream file = File.Open(Application.dataPath + "/Inventory.txt", FileMode.Open);
        bf.Serialize(file, commentData);
        file.Close();
    }

    public static Data Load()
    {
        Data commentData = new Data();
        if (File.Exists(Application.dataPath + "/Inventory.txt"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.dataPath + "/Inventory.txt", FileMode.Open);
            commentData = (Data)bf.Deserialize(file);
            file.Close();
            return commentData;
        }
        return null;
    }
}
