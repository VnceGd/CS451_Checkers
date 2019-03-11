using UnityEngine;

public class CheckersBoardPiece : MonoBehaviour
{
    public PlayerPiece occupant;
    public bool occupied;

    public Transform MoveToLocation;

    private Vector3 targetLocation;
    public bool isMoving;
    private float speed = 1.0f;

    public int x, y;

    private void OnMouseEnter()
    {
        Player.instance.targetBoardLocation = this;
    }
    private void OnMouseExit()
    {
        Player.instance.targetBoardLocation = null;
    }
    private void Start()
    {
        
    }
    private void Update()
    {
        if (isMoving)
        {
            float s = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetLocation, s);
            if(transform.position == targetLocation)
            {
                isMoving = false;
            }
        }
       
    }
    //animates the piece to its intended location
    public void MoveTo(Vector3 loc)
    {
        isMoving = true;
        targetLocation = loc;
    }
}
