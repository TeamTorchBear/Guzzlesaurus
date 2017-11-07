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
        if (!File.Exists(Application.dataPath + "/Data/Inventory.dat"))
        {
            File.Create(Application.dataPath + "/Data/Inventory.dat");
        }
        FileStream file = File.Open(Application.dataPath + "/Data/Inventory.dat", FileMode.Open);
        bf.Serialize(file, commentData);
        file.Close();
    }

    public static Data Load()
    {
        Data commentData = new Data();
        if (File.Exists(Application.dataPath + "/Data/Inventory.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.dataPath + "/Data/Inventory.dat", FileMode.Open);
            commentData = (Data)bf.Deserialize(file);
            file.Close();
            return commentData;
        }
        return null;
    }
}
