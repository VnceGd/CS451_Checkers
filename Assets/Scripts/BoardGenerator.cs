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

    // Check validity of a move, return true if valid
    public bool MovePiece(PlayerPiece p, int targetX, int targetY)
    {
        bool moveMade = false;
        int tmpX = p.x;
        int tmpY = p.y;
        if (!pieceList[targetX][targetY].occupied)
        {
            Debug.Log(p.x + ", " + p.y + " to " + targetX + ", " + targetY);
            if (p.isRed)
            {
                if (Mathf.Abs(targetX - p.x) == 1 && targetY - p.y == 1)
                {
                    moveMade = true;
                }
                else if (Mathf.Abs(targetX - p.x) == 2 && targetY - p.y == 2)
                {
                    CheckersBoardPiece jumping = pieceList[(targetX + p.x) / 2][p.y + 1];
                    if (jumping.occupied && !jumping.occupant.isRed)
                    {
                        moveMade = true;
                        Destroy(jumping.occupant.gameObject);
                    }
                }
                if (p.isKing)
                {
                    if (Mathf.Abs(targetX - p.x) == 1 && targetY - p.y == -1)
                    {
                        moveMade = true;
                    }
                    else if (Mathf.Abs(targetX - p.x) == 2 && targetY - p.y == 2)
                    {
                        CheckersBoardPiece jumping = pieceList[(targetX + p.x) / 2][p.y - 1];
                        if (jumping.occupied && !jumping.occupant.isRed)
                        {
                            moveMade = true;
                            Destroy(jumping.occupant.gameObject);
                        }
                    }
                }
            }
            else
            {
                if (Mathf.Abs(targetX - p.x) == 1 && targetY - p.y == -1)
                {
                    moveMade = true;
                }
                else if (Mathf.Abs(targetX - p.x) == 2 && targetY - p.y == 2)
                {
                    CheckersBoardPiece jumping = pieceList[(targetX + p.x) / 2][p.y - 1];
                    if (jumping.occupied && jumping.occupant.isRed)
                    {
                        moveMade = true;
                        Destroy(jumping.occupant.gameObject);
                    }
                }
                if (p.isKing)
                {
                    if (Mathf.Abs(targetX - p.x) == 1 && targetY - p.y == 1)
                    {
                        moveMade = true;
                    }
                    else if (Mathf.Abs(targetX - p.x) == 2 && targetY - p.y == 2)
                    {
                        CheckersBoardPiece jumping = pieceList[(targetX + p.x) / 2][p.y + 1];
                        if (jumping.occupied && jumping.occupant.isRed)
                        {
                            moveMade = true;
                            Destroy(jumping.occupant.gameObject);
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
                pieceList[targetX][targetY].occupant = p;
                pieceList[targetX][targetY].occupied = true;
                pieceList[tmpX][tmpY].occupant = null;
                pieceList[tmpX][tmpY].occupied = false;
            }
        }
        return moveMade;
    }
}
