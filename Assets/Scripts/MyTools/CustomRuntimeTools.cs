using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public class CustomRuntimeTools
{

    public static void DebugDump(object message, bool consoleLog = true, bool fileLog = true)
    {
        if (consoleLog)
        {
            Debug.Log(message);
        }
        if (fileLog)
        {

            string path = Application.dataPath + "/Debug/";
            Directory.CreateDirectory(path);
            path += "DebugLog.txt";
                
            FileStream stream = File.Create(path);
            stream.Close();


            string newLine = System.DateTime.Now.ToString() + " — " + message.ToString() + '\n';

            // Delete file if it is too big!
            {
                FileInfo fileinfo = new FileInfo(path);
                if (fileinfo.Length > 1024000000)
                {
                    File.Delete(path);
                }
            }
            File.AppendAllText(path, newLine);
        }
    }
}