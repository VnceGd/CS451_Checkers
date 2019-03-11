using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public CheckersSettings settings;

    public BoardGenerator boardGenerator;
    public List<List<CheckersBoardPiece>> pieceList;

    public GameObject playerPrefab;
    public GameObject competitorPrefab;


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
        boardGenerator = BoardGenerator.instance;
        if (boardGenerator != null)
        {
           GenerateBoard();
        }
       
    }
    public void LoadPrefabs()
    {
        playerPrefab = (GameObject)Resources.Load("pieceRed");
        competitorPrefab = (GameObject)Resources.Load("pieceBlue");
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
        if(settings != null)
        {
            settings.LoadDefaults();
        }
    }

    public void SetPlayerColorIndex(int i)
    {
        settings.PlayerColorIndex = i;
    }
    public void SetCompetitorColorIndex(int i)
    {
        settings.PlayerColorIndex = i;
    }
    public void SetCombinedMessageSize(string s)
    {
        try
        {
            int value = int.Parse(s);

            settings.MaxCombinedReliableMessageSize = value;
        }
        catch(Exception ex)
        {
            settings.MaxCombinedReliableMessageSize = 1024;
        }
    }
    public void SetMessageSize(string s)
    {
        try
        {
            int value = int.Parse(s);

            settings.MaxMessageQueueSize = value;
        }
        catch (Exception ex)
        {
            settings.MaxMessageQueueSize = 1024;
        }
    }
    public void SetNetworkPort(string s)
    {
        settings.DefaultNetworkPort = s;
    }
    public void SetNetworkAddress(string s)
    {
        settings.DefaultNetworkAddress = s;
    }
    public void SetRoomName(string s)
    {
        settings.RoomName = s;
    }
    public void SetPlayerName(string s)
    {
        settings.PlayerName = s;
    }
    public void ExitApplication()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    //load the lobby scene, bring over the game manager
    public void TraverseToLobby()
    {
        DontDestroyOnLoad(instance);
        SceneManager.LoadScene("Lobby");
    }

    public void TraverseToClient()
    {
        DontDestroyOnLoad(instance);
        SceneManager.LoadScene("ClientGame");
    }
    public void TraverseToServer()
    {
        DontDestroyOnLoad(instance);
        SceneManager.LoadScene("Server");
    }
    public void StartGame()
    {
        DontDestroyOnLoad(instance);
        SceneManager.LoadScene("Game");
    }

    // Generate board and place each player's pieces
    public void GenerateBoard()
    {
        if (boardGenerator != null)
        {
            pieceList = boardGenerator.GenerateBoard();
            GeneratePlayerPieces();
            GenerateCompetitorPieces();
        }
    }
    public void GeneratePlayerPieces()
    {
        if(boardGenerator != null)
        {
            boardGenerator.PlaceClientPieces(playerPrefab);
        }
    }
    public void GenerateCompetitorPieces()
    {
        if (boardGenerator != null)
        {
            boardGenerator.PlaceClientPieces(competitorPrefab);
        }
    }
}
