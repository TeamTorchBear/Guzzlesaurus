using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Initialize:MonoBehaviour {

    private void Start()
    {
        if (!File.Exists(Application.dataPath + "/Data/Inventory.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            Data data = new Data();
            data.moneyWeHave = 500;
            File.Create(Application.dataPath + "/Data/Inventory.dat").Dispose();
            FileStream file = File.Open(Application.dataPath + "/Data/Inventory.dat", FileMode.Open);
            bf.Serialize(file, data);
            file.Close();
        }
    }
}
