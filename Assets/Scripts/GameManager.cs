using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public CheckersSettings settings;
    // Start is called before the first frame update

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Debug.Log(Application.dataPath);
        //on start load the settings file
        settings = CheckersSettings.DeserializeSettingsFile(Application.dataPath + "/");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool SaveChanges()
    {
        try
        {
            CheckersSettings.SerializeSettingsFile(Application.dataPath + "/", settings);
            return true;
        }
        catch(Exception ex)
        {
            return false;
        }
    }
    public void LoadDefaultSettings()
    {
        if(this.settings != null)
        {
            this.settings.LoadDefaults();
        }
    }

    public void SetPlayerColorIndex(int i)
    {
        this.settings.PlayerColorIndex = i;
    }
    public void SetCompetitorColorIndex(int i)
    {
        this.settings.PlayerColorIndex = i;
    }
    public void SetCombinedMessageSize(string s)
    {
        try
        {
            int value = int.Parse(s);

            this.settings.MaxCombinedReliableMessageSize = value;
        }
        catch(Exception ex)
        {
            this.settings.MaxCombinedReliableMessageSize = 1024;
        }
    }
    public void SetMessageSize(string s)
    {
        try
        {
            int value = int.Parse(s);

            this.settings.MaxMessageQueueSize = value;
        }
        catch (Exception ex)
        {
            this.settings.MaxMessageQueueSize = 1024;
        }
    }
    public void SetNetworkPort(string s)
    {
        this.settings.DefaultNetworkPort = s;
    }
    public void SetNetworkAddress(string s)
    {
        this.settings.DefaultNetworkAddress = s;
    }
    public void SetRoomName(string s)
    {
        this.settings.RoomName = s;
    }
    public void SetPlayerName(string s)
    {
        this.settings.PlayerName = s;
    }
}
