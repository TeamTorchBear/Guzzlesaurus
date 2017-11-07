using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class SaveNLoadTxt
{
    [MenuItem("Tools/Write file")]
    static void WriteString(string fileName, string content)
    {
        var fileAddress = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(fileAddress, true);
        writer.WriteLine(content);
        writer.Flush();
        writer.Close();
        
    }

    [MenuItem("Tools/Read file")]
    static string ReadString(string fileName)
    {
        var fileAddress = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);
        FileInfo fInfo0 = new FileInfo(fileAddress);
        string s = "";
        if (fInfo0.Exists)
        {
            StreamReader r = new StreamReader(fileAddress);
            //byte[] data = new byte[1024];  
            // data = Encoding.UTF8.GetBytes(r.ReadToEnd());  
            // s = Encoding.UTF8.GetString(data, 0, data.Length);  
            s = r.ReadToEnd();
        }
        return s;
    }

    static string SetMessage()
    {
        return "";
    }
}
