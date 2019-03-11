using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BoardGenerator : MonoBehaviour
{
    public static BoardGenerator instance;

    public int BoardSize;
    public GameObject WhitePiecePrefab;
    public GameObject BlackPiecePrefab;
    public float spacing = 2.0f;
    public Transform originPoint;

    public List<List<CheckersBoardPiece>> pieceList;
    public List<PlayerPiece> playerPieces;
    public List<PlayerPiece> competitorPieces;

    public bool playerTurn = true;
    public bool forceJump;
    public int jumpCount = 0;

    public GameObject matchEndScreen;
    public Text matchEndText;

    private void Awake()
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
    public void Start()
    {
        GameManager.instance.GenerateBoard();
        if (ClientManager.instance == null)
        {
            Player.instance.isCompetitor = false;
        }
        else if (GameServerManager.instance == null)
        {
            Player.instance.isCompetitor = true;
        }
        if (!Player.instance.isCompetitor)
        {
            Player.instance.canMakeMove = false;
        }
        else
        {
            Player.instance.canMakeMove = true;
        }
    }
    // Generate the checker board
    public List<List<CheckersBoardPiece>> GenerateBoard()
    {
        pieceList = new List<List<CheckersBoardPiece>>();
        for (int i = 0; i < BoardSize; i++)
        {
            List<CheckersBoardPiece> row = new List<CheckersBoardPiece>();
            for (int j = 0; j < BoardSize; j++)
            {
                GameObject obj;
                if ((j % 2 == 0 && i % 2 == 0) || (j % 2 != 0 && i % 2 != 0))
                {
                    obj = Instantiate(BlackPiecePrefab, 
                                      originPoint.transform.position 
                                    + new Vector3(i * spacing, j * spacing, 0), 
                                      Quaternion.identity);
                    obj.GetComponent<CheckersBoardPiece>().x = i;
                    obj.GetComponent<CheckersBoardPiece>().y = j;
                    row.Add(obj.GetComponent<CheckersBoardPiece>());
                    obj.transform.SetParent(transform);
                }
                else if ((j % 2 == 0 && i % 2 != 0) || (j % 2 != 0 && i % 2 == 0))
                {
                    obj = Instantiate(WhitePiecePrefab, 
                                      originPoint.transform.position 
                                    + new Vector3(i * spacing, j * spacing, 0), 
                                      Quaternion.identity);
                    obj.GetComponent<CheckersBoardPiece>().x = i;
                    obj.GetComponent<CheckersBoardPiece>().y = j;
                    row.Add(obj.GetComponent<CheckersBoardPiece>());
                    obj.transform.SetParent(transform);
                }
            }
            pieceList.Add(row);
        }
        return pieceList;
    }

    // Place pieces on their respective side
    public void PlaceClientPieces(GameObject pref)
    {
        PlayerPiece p = pref.GetComponent<PlayerPiece>();
        if (p.isRed)
        {
            foreach (PlayerPiece pp in playerPieces)
            {
                Destroy(pp.gameObject);
            }
          
            playerPieces.Clear();
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if ((j % 2 == 0 && i % 2 == 0) || (j % 2 != 0 && i % 2 != 0))
                    {
                        pieceList[i][j].occupied = true;
                        Vector3 loc = new Vector3(pieceList[i][j].MoveToLocation.position.x,
                                                  pieceList[i][j].MoveToLocation.position.y,
                                                  -1.0f);
                        GameObject obj = (GameObject)Instantiate(pref, loc, Quaternion.identity);
                        Debug.Log(obj.scene.name);
                        PlayerPiece pieceComp = obj.GetComponent<PlayerPiece>();
                        pieceList[i][j].occupant = pieceComp;
                        pieceComp.x = i;
                        pieceComp.y = j;
                        playerPieces.Add(pieceComp);
                    }
                }
            }
        }
        else
        {            
            foreach (PlayerPiece pp in competitorPieces)
            {
               Destroy(pp.gameObject);
            }
            competitorPieces.Clear();
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 7; j > 4; j--)
                {
                    if ((j % 2 == 0 && i % 2 == 0) || (j % 2 != 0 && i % 2 != 0))
                    {
                        Debug.Log("Competitor Piece created");
                        pieceList[i][j].occupied = true;
                        Vector3 loc = new Vector3(pieceList[i][j].MoveToLocation.position.x, 
                                                  pieceList[i][j].MoveToLocation.position.y, 
                                                  -1.0f);
                        GameObject obj = (GameObject)Instantiate(pref, loc, Quaternion.identity);
                        PlayerPiece pieceComp = obj.GetComponent<PlayerPiece>();
                        pieceList[i][j].occupant = pieceComp;
                        pieceComp.x = i;
                        pieceComp.y = j;
                        competitorPieces.Add(pieceComp);
                    }
                }
            }
        }
    }

    // Check if piece has a move available
    public bool MoveAvailable(PlayerPiece p)
    {
        if (p.isRed)
        {
            if (p.y < 7)
            {
                if (p.x < 7 && !pieceList[p.x + 1][p.y + 1].occupied)
                {
                    return true;
                }
                if (p.x > 0 && !pieceList[p.x - 1][p.y + 1].occupied)
                {
                    return true;
                }
            }
            if (p.isKing && p.y > 0)
            {
                if (p.x < 7 && !pieceList[p.x + 1][p.y - 1].occupied)
                {
                    return true;
                }
                if (p.x > 0 && !pieceList[p.x - 1][p.y - 1].occupied)
                {
                    return true;
                }
            }
        }
        else
        {
            if (p.y > 0)
            {
                if (p.x < 7 && !pieceList[p.x + 1][p.y - 1].occupied)
                {
                    return true;
                }
                if (p.x > 0 && !pieceList[p.x - 1][p.y - 1].occupied)
                {
                    return true;
                }
            }
            if (p.isKing && p.y < 7)
            {
                if (p.x < 7 && !pieceList[p.x + 1][p.y + 1].occupied)
                {
                    return true;
                }
                if (p.x > 0 && !pieceList[p.x - 1][p.y + 1].occupied)
                {
                    return true;
                }
            }
        }
        return false;
    }

    // Check if piece has a jump available
    public bool JumpAvailable(PlayerPiece p)
    {
        if (p.isRed)
        {
            if (p.y < 6)
            {
                if (p.x < 6 &&
                    pieceList[p.x + 1][p.y + 1].occupied &&
                    !pieceList[p.x + 1][p.y + 1].occupant.isRed)
                {
                    if (!pieceList[p.x + 2][p.y + 2].occupied)
                    {
                        return true;
                    }
                }
                if (p.x > 1 &&
                    pieceList[p.x - 1][p.y + 1].occupied &&
                    !pieceList[p.x - 1][p.y + 1].occupant.isRed)
                {
                    if (!pieceList[p.x - 2][p.y + 2].occupied)
                    {
                        return true;
                    }
                }
            }
            if (p.isKing && p.y > 1)
            {
                if (p.x < 6 &&
                    pieceList[p.x + 1][p.y - 1].occupied &&
                    !pieceList[p.x + 1][p.y - 1].occupant.isRed)
                {
                    if (!pieceList[p.x + 2][p.y - 2].occupied)
                    {
                        return true;
                    }
                }
                if (p.x > 1 &&
                    pieceList[p.x - 1][p.y - 1].occupied &&
                    !pieceList[p.x - 1][p.y - 1].occupant.isRed)
                {
                    if (!pieceList[p.x - 2][p.y - 2].occupied)
                    {
                        return true;
                    }
                }
            }
        }
        else
        {
            if (p.y > 1)
            {
                if (p.x < 6 && 
                    pieceList[p.x + 1][p.y - 1].occupied &&
                    pieceList[p.x + 1][p.y - 1].occupant.isRed)
                {
                    if (!pieceList[p.x + 2][p.y - 2].occupied)
                    {
                        return true;
                    }
                }
                if (p.x > 1 && 
                    pieceList[p.x - 1][p.y - 1].occupied &&
                    pieceList[p.x - 1][p.y - 1].occupant.isRed)
                {
                    if (!pieceList[p.x - 2][p.y - 2].occupied)
                    {
                        return true;
                    }
                }
            }
            if (p.isKing && p.y < 6)
            {
                if (p.x < 6 && 
                    pieceList[p.x + 1][p.y + 1].occupied &&
                    pieceList[p.x + 1][p.y + 1].occupant.isRed)
                {
                    if (!pieceList[p.x + 2][p.y + 2].occupied)
                    {
                        return true;
                    }
                }
                if (p.x > 1 && 
                    pieceList[p.x - 1][p.y + 1].occupied &&
                    pieceList[p.x - 1][p.y + 1].occupant.isRed)
                {
                    if (!pieceList[p.x - 2][p.y + 2].occupied)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    // Check validity of a move, return true if valid
    public bool MovePiece(PlayerPiece p, int targetX, int targetY)
    {
        bool moveMade = false;
        int tmpX = p.x;
        int tmpY = p.y;
        if (!pieceList[targetX][targetY].occupied)
        {
            if (playerTurn && p.isRed)
            {
                if (Mathf.Abs(targetX - p.x) == 1 && targetY - p.y == 1 &&
                    !forceJump)
                {
                    moveMade = true;
                }
                else if (Mathf.Abs(targetX - p.x) == 2 && targetY - p.y == 2)
                {
                    int jumpX = (targetX + p.x) / 2;
                    int jumpY = p.y + 1;
                    CheckersBoardPiece jumping = pieceList[jumpX][jumpY];
                    if (jumping.occupied && !jumping.occupant.isRed)
                    {
                        moveMade = true;
                        Destroy(jumping.occupant.gameObject);
                        RemovePiece(jumpX, jumpY);
                        jumpCount++;
                    }
                }
                if (p.isKing)
                {
                    if (Mathf.Abs(targetX - p.x) == 1 && targetY - p.y == -1 &&
                        !forceJump)
                    {
                        moveMade = true;
                    }
                    else if (Mathf.Abs(targetX - p.x) == 2 && targetY - p.y == 2)
                    {
                        int jumpX = (targetX + p.x) / 2;
                        int jumpY = p.y - 1;
                        CheckersBoardPiece jumping = pieceList[jumpX][jumpY];
                        if (jumping.occupied && !jumping.occupant.isRed)
                        {
                            moveMade = true;
                            Destroy(jumping.occupant.gameObject);
                            RemovePiece(jumpX, jumpY);
                            jumpCount++;
                        }
                    }
                }
            }
            else if (playerTurn && Player.instance.isCompetitor && !p.isRed)
            {
                if (Mathf.Abs(targetX - p.x) == 1 && targetY - p.y == -1 && 
                    !forceJump)
                {
                    moveMade = true;
                }
                else if (Mathf.Abs(targetX - p.x) == 2 && targetY - p.y == -2)
                {
                    int jumpX = (targetX + p.x) / 2;
                    int jumpY = p.y - 1;
                    CheckersBoardPiece jumping = pieceList[jumpX][jumpY];
                    if (jumping.occupied && jumping.occupant.isRed)
                    {
                        moveMade = true;
                        Destroy(jumping.occupant.gameObject);
                        RemovePiece(jumpX, jumpY);
                        jumpCount++;
                    }
                }
                if (p.isKing)
                {
                    if (Mathf.Abs(targetX - p.x) == 1 && targetY - p.y == 1 && 
                        !forceJump)
                    {
                        moveMade = true;
                    }
                    else if (Mathf.Abs(targetX - p.x) == 2 && targetY - p.y == 2)
                    {
                        int jumpX = (targetX + p.x) / 2;
                        int jumpY = p.y + 1;
                        CheckersBoardPiece jumping = pieceList[jumpX][jumpY];
                        if (jumping.occupied && jumping.occupant.isRed)
                        {
                            moveMade = true;
                            Destroy(jumping.occupant.gameObject);
                            RemovePiece(jumpX, jumpY);
                            jumpCount++;
                        }
                    }
                }
            }
            if (moveMade)
            {
                Debug.Log(p.x + ", " + p.y + " to " + targetX + ", " + targetY);
                p.transform.position = pieceList[targetX][targetY].MoveToLocation.position 
                                     + Vector3.back;
                p.x = targetX;
                p.y = targetY;
                p.isBeingMoved = false;

                if (!p.isKing)
                {
                    if (p.isRed && targetY == 7)
                    {
                        p.Promote();
                    }
                    else if (targetY == 0)
                    {
                        p.Promote();
                    }
                }
                if (p.mustJump && !JumpAvailable(p))
                {
                    p.mustJump = false;
                }

                pieceList[targetX][targetY].occupant = p;
                pieceList[targetX][targetY].occupied = true;
                RemovePiece(tmpX, tmpY);

                if (!p.mustJump)
                {
                    EndTurn();
                }
            }
        }
        return moveMade;
    }

    // Pass turn to other player
    public void EndTurn()
    {
        jumpCount = 0;
        forceJump = false;
        if (!Player.instance.isCompetitor)
        {
            Player.instance.canMakeMove = playerTurn;
        }
        else
        {
            Player.instance.canMakeMove = !playerTurn;
        }
        Player.instance.canMakeMove = playerTurn;
        StartOfTurn();
    }

    // End the match and declare a winner
    public void EndMatch()
    {
        matchEndScreen.SetActive(true);
        if (playerTurn)
        {
            matchEndText.text = "You Win";
            // Also send "You Lose" message to client
        }
        else
        {
            matchEndText.text = "You Lose";
            // Also send "You Win" message to client
        }
    }

    // Check if current player has a jump available
    public void StartOfTurn()
    {
        bool canMove = false;
        if (playerTurn)
        {
            if (playerPieces.Count <= 0)
            {
                EndMatch();
                return;
            }
            foreach (PlayerPiece p in playerPieces)
            {
                if (!canMove && MoveAvailable(p))
                {
                    canMove = true;
                }
                if (JumpAvailable(p) && jumpCount > 0)
                {
                    p.mustJump = true;
                    if (!forceJump)
                    {
                        forceJump = true;
                    }
                }
            }
            if (!canMove)
            {
                EndMatch();
                return;
            }
        }
        else
        {
            if (competitorPieces.Count <= 0)
            {
                EndMatch();
                return;
            }
            foreach (PlayerPiece p in competitorPieces)
            {
                if (!canMove && MoveAvailable(p))
                {
                    canMove = true;
                }
                if (JumpAvailable(p) && jumpCount > 0)
                {
                    p.mustJump = true;
                    if (!forceJump)
                    {
                        forceJump = true;
                    }
                }
            }
            if (!canMove)
            {
                EndMatch();
                return;
            }
        }
    }

    // Remove piece from pieceList
    public void RemovePiece(int x, int y)
    {
        pieceList[x][y].occupant = null;
        pieceList[x][y].occupied = false;
    }
    //move to piece back to its original location
    public void ResetPiece(PlayerPiece p)
    {
        if (p != null)
        {
            p.transform.position = pieceList[p.x][p.y].MoveToLocation.position;
        }
    }
    //attempts to set a piece location without validating the move (called when the component makes a move)
    public void SetPieceLocation(PlayerPiece p, int targetX, int targetY)
    {
        pieceList[p.x][p.y].occupied = false;
        pieceList[p.x][p.y].occupant = null;
        p.x = targetX;
        p.y = targetY;
        p.transform.position = pieceList[targetX][targetY].MoveToLocation.position;
        pieceList[targetX][targetY].occupant = p;
        pieceList[targetX][targetY].occupied = true;
    }
}
