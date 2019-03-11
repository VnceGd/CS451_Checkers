using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    public PlayerPiece selectedPiece;
    public CheckersBoardPiece targetBoardLocation;
    public bool canMakeMove;
    public bool isCompetitor = false;


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
    public bool MovePiece()
    {
        try
        {
            if (targetBoardLocation != null && selectedPiece != null)
            {
                Debug.Log("Piece moved to: " + targetBoardLocation.x + " , " + targetBoardLocation.y);
                var res = BoardGenerator.instance.MovePiece(selectedPiece, targetBoardLocation.x, targetBoardLocation.y);
                if (res && ClientManager.instance != null)
                {
                    ClientManager.instance.SendPieceCoordinates(selectedPiece);
                    ClientManager.instance.SendMoveComplete();
                }
                else if(res && GameServerManager.instance != null)
                {
                    GameServerManager.instance.SendMoveChange();
                }
            }
            return false;
        }
        catch(System.Exception ex)
        {
            if (selectedPiece != null)
            {
                selectedPiece.SetMask(0);
            }
            return false;
        }
    }
    public void ResetPiece()
    {
        if (selectedPiece != null)
        {
            BoardGenerator.instance.ResetPiece(selectedPiece);
            selectedPiece.SetMask(0);
        }
    }
}
