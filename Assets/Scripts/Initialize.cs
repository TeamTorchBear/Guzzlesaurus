using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Initialize:MonoBehaviour {

    private void Start()
    {
        if (!File.Exists(Application.dataPath + "/Inventory.txt"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            Data data = new Data();
            data.moneyWeHave = 500;
            File.Create(Application.dataPath + "/Inventory.txt").Dispose();
            FileStream file = File.Open(Application.dataPath + "/Inventory.txt", FileMode.Open);
            bf.Serialize(file, data);
            file.Close();
        }
    }
}
