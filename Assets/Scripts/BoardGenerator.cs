using System.Collections.Generic;
using UnityEngine;

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
                        GameObject obj = Instantiate(pref, loc, Quaternion.identity);
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
                        pieceList[i][j].occupied = true;
                        Vector3 loc = new Vector3(pieceList[i][j].MoveToLocation.position.x, 
                                                  pieceList[i][j].MoveToLocation.position.y, 
                                                  -1.0f);
                        GameObject obj = Instantiate(pref, loc, Quaternion.identity);
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

    public bool JumpAvailable(PlayerPiece p)
    {
        if (p.isRed)
        {
            if (p.x < 7 && 
                pieceList[p.x + 1][p.y + 1].occupied &&
                !pieceList[p.x + 1][p.y + 1].occupant.isRed)
            {
                if (p.x < 6 && 
                    !pieceList[p.x + 2][p.y + 2].occupied)
                {
                    return true;
                }
            }
            if (p.x > 0 && 
                pieceList[p.x - 1][p.y + 1].occupied &&
                !pieceList[p.x - 1][p.y + 1].occupant.isRed)
            {
                if (p.x > 1 && 
                    !pieceList[p.x - 2][p.y + 2].occupied)
                {
                    return true;
                }
            }
            if (p.isKing)
            {
                if (p.x < 7 && 
                    pieceList[p.x + 1][p.y - 1].occupied &&
                    !pieceList[p.x + 1][p.y - 1].occupant.isRed)
                {
                    if (p.x < 6 && 
                        !pieceList[p.x + 2][p.y - 2].occupied)
                    {
                        return true;
                    }
                }
                if (p.x > 0 && 
                    pieceList[p.x - 1][p.y - 1].occupied &&
                    !pieceList[p.x - 1][p.y - 1].occupant.isRed)
                {
                    if (p.x > 1 && 
                        !pieceList[p.x - 2][p.y - 2].occupied)
                    {
                        return true;
                    }
                }
            }
        }
        else
        {
            if (p.x < 7 && 
                pieceList[p.x + 1][p.y - 1].occupied &&
                pieceList[p.x + 1][p.y - 1].occupant.isRed)
            {
                if (!pieceList[p.x + 2][p.y - 2].occupied)
                {
                    return true;
                }
            }
            if (p.x > 0 && 
                pieceList[p.x - 1][p.y - 1].occupied &&
                pieceList[p.x - 1][p.y - 1].occupant.isRed)
            {
                if (!pieceList[p.x - 2][p.y - 2].occupied)
                {
                    return true;
                }
            }
            if (p.isKing)
            {
                if (p.x < 7 && 
                    pieceList[p.x + 1][p.y + 1].occupied &&
                    pieceList[p.x + 1][p.y + 1].occupant.isRed)
                {
                    if (!pieceList[p.x + 2][p.y + 2].occupied)
                    {
                        return true;
                    }
                }
                if (p.x > 0 && 
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
            Debug.Log(p.x + ", " + p.y + " to " + targetX + ", " + targetY);
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
                        }
                    }
                }
            }
            else if (!playerTurn && !p.isRed)
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
                        }
                    }
                }
            }
            if (moveMade)
            {
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
                if (!JumpAvailable(p))
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
        forceJump = false;
        if (playerTurn)
        {
            playerTurn = false;
        }
        else
        {
            playerTurn = true;
        }
        StartOfTurn();
    }

    // Check if current player has a jump available
    public void StartOfTurn()
    {
        if (playerTurn)
        {
            foreach (PlayerPiece p in playerPieces)
            {
                if (JumpAvailable(p))
                {
                    p.mustJump = true;
                    if (!forceJump)
                    {
                        forceJump = true;
                    }
                }
            }
        }
        else
        {
            foreach (PlayerPiece p in competitorPieces)
            {
                if (JumpAvailable(p))
                {
                    p.mustJump = true;
                    if (!forceJump)
                    {
                        forceJump = true;
                    }
                }
            }
        }
    }

    // Remove piece from pieceList
    public void RemovePiece(int x, int y)
    {
        pieceList[x][y].occupant = null;
        pieceList[x][y].occupied = false;
    }
}
