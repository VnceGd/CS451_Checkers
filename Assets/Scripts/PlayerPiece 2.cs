using UnityEngine;

public class PlayerPiece : MonoBehaviour
{
    public int x;
    public int y;

    public bool isBeingMoved;

    private SpriteRenderer renderer;
    public Sprite kingSprite;
    public Sprite kingHoverSprite;
    public Sprite baseSprite;
    public Sprite hoverSprite;

    public bool isKing;
    public bool isRed;
    public bool mustJump;

    private void Update()
    {
        if (isBeingMoved)
        {
            Vector3 tgt = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            tgt.z = -1.0f;
            transform.position = Vector3.MoveTowards(transform.position, tgt, 0.1f);
        }
    }
    public void SetMask(int i)
    {
        gameObject.layer = i;
    }
    void Start()
    {
        renderer = this.gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnDestroy()
    {
        if(ClientManager.instance != null)
        {
            ClientManager.instance.SendRemovePiece(this);
        }
        else if(GameServerManager.instance != null)
        {
            GameServerManager.instance.SendRemovePiece(this);
        }
    }
    private void OnMouseOver()
    {
        if ((Player.instance.isCompetitor && !isRed) || (!Player.instance.isCompetitor && isRed))
        {
            //glow 
            if (!isKing)
            {
                this.renderer.sprite = hoverSprite;
            }
            else
            {
                this.renderer.sprite = kingHoverSprite;
            }
        }
    }
    public void SetPieceSprite(Sprite sprite)
    {
        if (sprite != null)
        {
            renderer.sprite = sprite;
        }
    }
    private void OnMouseExit()
    {
        if ((Player.instance.isCompetitor && !isRed) || (!Player.instance.isCompetitor && isRed))
        {
            if (!isKing)
            {
                this.renderer.sprite = baseSprite;
            }
            else
            {
                this.renderer.sprite = kingSprite;
            }
        }
        //Player.instance.selectedPiece = null;
    }
    private void ChangeColor()
    {
        renderer.color = new Color(1.0f, 1.0f, 0.1f, 1.0f);
    }
    private void ResetColor()
    {
        renderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }
    private void OnMouseDown()
    {
        if ((Player.instance.isCompetitor && !isRed) || (!Player.instance.isCompetitor && isRed))
        {
            if (Player.instance.canMakeMove)
            {
                Player.instance.selectedPiece = this;
                isBeingMoved = true;
                SetMask(2);
            }
        }
    }
    private void OnMouseUp()
    {
        if ((Player.instance.isCompetitor && !isRed) || (!Player.instance.isCompetitor && isRed))
        {
            if (Player.instance.canMakeMove)
            {
                isBeingMoved = false;
                if (!Player.instance.MovePiece())
                {
                    //do something
                    Player.instance.ResetPiece();
                }
                SetMask(0);
            }
        }
    }
    public void Promote()
    {
        isKing = true;
        renderer.sprite = kingSprite;
        SetMask(0);
    }
}
