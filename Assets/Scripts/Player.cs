using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
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
            BoardGenerator.instance.MovePiece(selectedPiece, targetBoardLocation.x, targetBoardLocation.y);
        }
    }
}
