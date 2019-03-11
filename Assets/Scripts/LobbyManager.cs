using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    //allows the user to start the game when players are connected, etc
    public GameManager gm;
    void Start()
    {
        gm = GameManager.instance;    
    }
    public void StartGame()
    {
        gm.TraverseToServer();
    }
}
