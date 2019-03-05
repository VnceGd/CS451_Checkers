using UnityEngine;
using UnityEngine.UI;

public class CheckerBoard : MonoBehaviour
{
    public CheckerPiece[,] checkerPieces = new CheckerPiece[8, 8];

    public GameObject checkerPieceRed;
    public GameObject checkerPieceBlue;

    private Vector3 boardOffset = new Vector3(-4f, 0f, -4f);
    private Vector3 checkerPieceOffset = new Vector3(0.5f, 0f, 0.5f);

    private Vector2 mousePosition;

    private CheckerPiece selectedPiece;

    private Vector2 startTile;
    private Vector2 destTile;

    private bool redPlayerTurn;
    public GameObject turnIconRed;
    public GameObject turnIconBlue;
    private float maxTurnTime = 30f; // seconds
    private float turnTimeRemaining = 30f;
    public Text turnTimerRed;
    public Text turnTimerBlue;
    private float timeElapsed;
    public Text matchTimer;

    // Start is called before the first frame update
    void Start()
    {
        InitializeBoard();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimers();
        if (Input.GetMouseButtonDown(0))
        {
            DetectClick();
        }
    }

    public void UpdateTimers()
    {
        timeElapsed += Time.deltaTime;

        float hoursElapsed = Mathf.Floor(timeElapsed / 360f);
        float minutesElapsed = (timeElapsed - hoursElapsed) / 60f;
        float secondsElapsed = timeElapsed - minutesElapsed;
        string timerString = "";

        if (hoursElapsed > 0f)
        {
            timerString += hoursElapsed.ToString() + ":";
        }
        timerString += minutesElapsed.ToString("00") + ":" + secondsElapsed.ToString("00");
        matchTimer.text = timerString;

        turnTimeRemaining -= Time.deltaTime;
        if (redPlayerTurn)
        {
            turnTimerRed.text = turnTimeRemaining.ToString("00");
        }
        else
        {
            turnTimerBlue.text = turnTimeRemaining.ToString("00");
        }
    }

    public void DetectClick()
    {
        if (!Camera.main)
        {
            return;
        }

        // if my turn
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), 
                            out RaycastHit hit, 
                            10f, 
                            LayerMask.GetMask("Board")))
        {
            mousePosition.x = (int)(hit.point.x - boardOffset.x);
            mousePosition.y = (int)(hit.point.z - boardOffset.z);


            if (selectedPiece)
            {
                TryMove((int)startTile.x, (int)startTile.y,
                        (int)mousePosition.x, (int)mousePosition.y);
            }
            else
            {
                SelectPiece((int)mousePosition.x, (int)mousePosition.y);
            }
        }
    }

    public void SelectPiece(int x, int y)
    {
        // Check Out of Bounds
        if (x < 0 || x >= checkerPieces.Length || y < 0 || y >= checkerPieces.Length)
        {
            return;
        }

        CheckerPiece cp = checkerPieces[x, y];
        if (cp != null)
        {
            if ((redPlayerTurn && cp.isRedPiece) || (!redPlayerTurn && !cp.isRedPiece))
            {
                selectedPiece = cp;
                startTile = mousePosition;
            }
        }
    }

    public void TryMove(int x1, int y1, int x2, int y2)
    {
        startTile = new Vector2(x1, y1);
        destTile = new Vector2(x2, y2);
        selectedPiece = checkerPieces[x1, y1];

        if (x2 < 0 || x2 >= checkerPieces.Length || 
            y2 < 0 || y2 >= checkerPieces.Length)
        {
            startTile = Vector3.zero;
            selectedPiece = null;
            return;
        }

        if (selectedPiece != null)
        {
            if (destTile == startTile)
            {
                MovePiece(selectedPiece, x1, y1);
                selectedPiece = null;
                return;
            }

            if (selectedPiece.CheckSameColor(checkerPieces, x2, y2))
            {
                SelectPiece(x2, y2);
            }
            else if (selectedPiece.CheckValidMove(checkerPieces, x1, y1, x2, y2))
            {
                if (Mathf.Abs(x2 - x1) == 2)
                {
                    CheckerPiece cp = checkerPieces[(x1 + x2) / 2, (y1 + y2) / 2];
                    if (cp != null)
                    {
                        checkerPieces[(x1 + x2) / 2, (y1 + y2) / 2] = null;
                        Destroy(cp.gameObject);
                    }
                }
                MovePiece(selectedPiece, x2, y2);
                checkerPieces[x2, y2] = selectedPiece;
                checkerPieces[x1, y1] = null;
                selectedPiece = null;
                EndTurn();
            }
        }
    }

    public void EndTurn()
    {
        if (redPlayerTurn)
        {
            redPlayerTurn = false;
            turnIconRed.SetActive(false);
            turnIconBlue.SetActive(true);
        }
        else
        {
            redPlayerTurn = true;
            turnIconRed.SetActive(true);
            turnIconBlue.SetActive(false);
        }
        turnTimeRemaining = maxTurnTime;
    }

    public void CoinFlip()
    {
        int rand = Random.Range(0, 2);
        if (rand > 0)
        {
            redPlayerTurn = true;
            turnIconRed.SetActive(true);
            turnIconBlue.SetActive(false);
        }
        else
        {
            redPlayerTurn = false;
            turnIconRed.SetActive(false);
            turnIconBlue.SetActive(true);
        }
    }

    public void InitializeBoard()
    {
        // Create red team
        for (int col = 0; col < 3; col++)
        {
            bool oddRow = col % 2 == 0;
            for (int row = 0; row < 8; row += 2)
            {
                GeneratePiece(checkerPieceRed, oddRow ? row : row + 1, col);
            }
        }
        // Create blue team
        for (int col = 7; col > 4; col--)
        {
            bool oddRow = col % 2 == 0;
            for (int row = 0; row < 8; row += 2)
            {
                GeneratePiece(checkerPieceBlue, oddRow ? row : row + 1, col);
            }
        }
        CoinFlip();
    }

    public void GeneratePiece(GameObject piece, int row, int col)
    {
        GameObject checkerPieceClone = Instantiate(piece);
        checkerPieceClone.transform.SetParent(transform);

        CheckerPiece cp = checkerPieceClone.GetComponent<CheckerPiece>();
        checkerPieces[row, col] = cp;
        MovePiece(cp, row, col);
    }

    public void MovePiece(CheckerPiece cp, int row, int col)
    {
        cp.transform.position = (Vector3.right * row) + 
                                (Vector3.forward * col) + 
                                boardOffset + checkerPieceOffset;
    }
}
