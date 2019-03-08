using UnityEngine;

public class PlayerPiece : MonoBehaviour
{
    public int x;
    public int y;

    public bool isBeingMoved;

    private SpriteRenderer renderer;
    public Sprite kingSprite;

    public bool isKing;
    public bool isRed;
    public bool mustJump;

    void Start()
    {
        renderer = gameObject.GetComponent<SpriteRenderer>();    
    }
    private void Update()
    {
        if (isBeingMoved)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = -2f;
            //transform.position = mousePosition;
        }
    }
    private void OnMouseOver()
    {
        ChangeColor();
    }
    private void OnMouseExit()
    {
        ResetColor();
    }
    private void ChangeColor()
    {
        renderer.color = new Color(0.8f, 0.8f, 0.8f, 1.0f);
    }
    private void ResetColor()
    {
        renderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }
    private void OnMouseDrag()
    {
          
    }
    private void OnMouseDown()
    {
        isBeingMoved = true;
        Player.instance.selectedPiece = this;
    }
    private void OnMouseUp()
    {
        Player.instance.MovePiece();
        Player.instance.selectedPiece = null;
    }

    public void Promote()
    {
        isKing = true;
        renderer.sprite = kingSprite;
    }
}
