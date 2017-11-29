using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Initialize:MonoBehaviour {

    private void Start()
    {
        Debug.Log(Application.persistentDataPath);
        if (!File.Exists(Application.persistentDataPath + "/Inventory.txt"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            Data data = new Data();
            data.moneyWeHave = 500;
            data.eggQuantity = 0;
            data.flourQuantity = 0;
            data.milkQuantity = 0;
            data.sugarQuantity = 0;
            data.saltQuantity = 0;
            data.butterQuantity = 0;
            data.tableLevel = 1;
            data.kitchenLevel = 1;
            data.unreadMail = true;
            data.enoughIngredients = false;
            File.Create(Application.persistentDataPath + "/Inventory.txt").Dispose();
            FileStream file = File.Open(Application.persistentDataPath + "/Inventory.txt", FileMode.Open);
            bf.Serialize(file, data);
            file.Close();
        }
    }
}
