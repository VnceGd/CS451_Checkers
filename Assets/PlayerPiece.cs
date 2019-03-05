using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPiece : MonoBehaviour
{
    public int x;
    public int y;

    public bool isBeingMoved;

    private SpriteRenderer renderer;

    public bool isKing = false;
    void Start()
    {
        renderer = this.gameObject.GetComponent<SpriteRenderer>();    
    }
    private void OnMouseOver()
    {
        ChangeColor();
        Player.instance.selectedPiece = this;
    }
    private void OnMouseExit()
    {
        ResetColor();
        //Player.instance.selectedPiece = null;
    }
    private void ChangeColor()
    {
        renderer.color = new Color(0.0f, 0.1f, 0.1f, 1.0f);
    }
    private void ResetColor()
    {
        renderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }
    private void OnMouseDrag()
    {
        //allow the user to drag the peice to its intended location   
    }
    private void OnMouseDown()
    {
        isBeingMoved = true;
    }
    private void OnMouseUp()
    {
        Player.instance.MovePiece();
    }
}
