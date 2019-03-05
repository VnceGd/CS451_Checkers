using UnityEngine;

public class CheckersBoardPiece : MonoBehaviour
{
    public PlayerPiece occupant;
    public bool occupied;
    public Transform MoveToLocation;

    public int x, y;

    private void OnMouseEnter()
    {
        Player.instance.targetBoardLocation = this;
    }
    private void OnMouseExit()
    {
        Player.instance.targetBoardLocation = null;
    }
}
