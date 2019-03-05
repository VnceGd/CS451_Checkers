using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance = null;
    public PlayerPiece selectedPiece;
    public CheckersBoardPiece targetBoardLocation;


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
    public void MovePiece()
    {
        if (targetBoardLocation != null && selectedPiece != null)
        {
            Debug.Log("Piece moved to: " + targetBoardLocation.x + " , " + targetBoardLocation.y);
            BoardGenerator.instance.MovePiece(selectedPiece, targetBoardLocation.x, targetBoardLocation.y);
        }
    }
}
