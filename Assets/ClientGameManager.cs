using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientGameManager : MonoBehaviour
{
    //REMOVE IN FUTURE
    //USED FOR TESTING PURPOSES
    //links the loaded game manager to the gui 
    private GameManager gm;
    private BoardGenerator bg;
    public Text moveText;

    private void Start()
    {
        gm = GameManager.instance;
        bg = BoardGenerator.instance;
        gm.boardGenerator = bg;
        gm.LoadPrefabs();
    }
    private void Update()
    {
        moveText.text = Player.instance.canMakeMove.ToString() + Player.instance.isCompetitor.ToString();
    }
    public void GenerateBoard()
    {
        gm.GenerateBoard();
    }
    public void GeneratePlayerPieces()
    {
        gm.GeneratePlayerPieces();
    }
    public void GenerateCompetitorPieces()
    {
        gm.GenerateCompetitorPieces();
    }
}
