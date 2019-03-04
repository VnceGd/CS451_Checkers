using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System;
using UnityEngine;
using System.Diagnostics;

[System.Serializable]
public class CheckersSettings
{
    public string PlayerName;
    public string RoomName;
    public string DefaultNetworkAddress;
    public int PlayerColorIndex;
    public int CompetitorColorIndex;
    public string DefaultNetworkPort;
    public string DefaultServerError;
    public int MaxMessageQueueSize;
    public int MaxCombinedReliableMessageSize;
    public float ThreadAwakeTimeout;

    public CheckersSettings()
    {
        PlayerName = "Default Name";
        RoomName = "Room Name";
        DefaultNetworkAddress = "0.0.0.00";
        PlayerColorIndex = 0;
        CompetitorColorIndex = 0;
        DefaultNetworkPort = "0000";
        MaxMessageQueueSize = 1024;
        MaxCombinedReliableMessageSize = 2048;
        ThreadAwakeTimeout = 10.0f;
    }
    public void LoadDefaults()
    {
        PlayerName = "Default Name";
        RoomName = "Room Name";
        DefaultNetworkAddress = "0.0.0.00";
        PlayerColorIndex = 0;
        CompetitorColorIndex = 0;
        DefaultNetworkPort = "0000";
        MaxMessageQueueSize = 1024;
        MaxCombinedReliableMessageSize = 2048;
        ThreadAwakeTimeout = 10.0f;
    }
    public static CheckersSettings DeserializeSettingsFile(string path)
    {
        var fileName = path + "settings.xml";
        UnityEngine.Debug.Log(fileName);
        CheckersSettings tmp = new CheckersSettings();
        try
        {
            if (File.Exists(fileName))
            {
                using (var r = new StreamReader(fileName, false))
                {
                    XmlSerializer deserializer = new XmlSerializer(typeof(CheckersSettings));
                    tmp = (CheckersSettings)deserializer.Deserialize(r);
                    return tmp;
                }
            }
            else
            {
                //if the file doesnt exist, create a new object, save it, then reload it
                CheckersSettings newObj = new CheckersSettings();
                CheckersSettings.SerializeSettingsFile(path, newObj);
                CheckersSettings.DeserializeSettingsFile(path);
            }
            return tmp;
        }
        catch(Exception e)
        {
            Trace.WriteLine(e);
            return tmp;
        }
    }

    public static void SerializeSettingsFile(string path, CheckersSettings settings)
    {
        var fileName = path + "settings.xml";
        UnityEngine.Debug.Log(fileName);
        //load settings.xml or create a new file
        try
        {
            using (var s = new StreamWriter(fileName, false))
            {
                XmlSerializer serailizer = new XmlSerializer(typeof(CheckersSettings));
                serailizer.Serialize(s, settings);
            }
        }
        catch(Exception e)
        {
            Trace.WriteLine(e);
        }
    }
}
