using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Initialize:MonoBehaviour {

    private void Start()
    {
        if (!File.Exists(Application.persistentDataPath + "/Inventory.txt"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            Data data = new Data();
            data.moneyWeHave = 500;
            File.Create(Application.persistentDataPath + "/Inventory.txt").Dispose();
            FileStream file = File.Open(Application.persistentDataPath + "/Inventory.txt", FileMode.Open);
            bf.Serialize(file, data);
            file.Close();
        }
    }
}
