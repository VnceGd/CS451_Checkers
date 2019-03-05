using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    public static BoardGenerator instance = null;

    public int BoardSize = 0;
    public GameObject WhitePiecePrefab;
    public GameObject BlackPiecePrefab;
    public float spacing = 2.0f;
    public Transform originPoint;

    public List<List<CheckersBoardPiece>> pieceList = null;
    public List<PlayerPiece> playerPieces;

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
    // Start is called before the first frame update
    void Start()
    {
        //GenerateBoard();        
    }

    //instantiate method at first
    //dont really know if we want to set up a pool for this
    public List<List<CheckersBoardPiece>> GenerateBoard()
    {
        pieceList = new List<List<CheckersBoardPiece>>();
        for (int i = 0; i < BoardSize; i++)
        {
            List<CheckersBoardPiece> row = new List<CheckersBoardPiece>();
            for(int j = 0; j < BoardSize; j++)
            {
                GameObject obj;
                if ((j % 2 == 0 && i % 2 == 0) || (j % 2 != 0 && i % 2 != 0))
                {
                    obj = GameObject.Instantiate(BlackPiecePrefab, originPoint.transform.position + new Vector3(i * spacing, j * spacing, 0), Quaternion.identity);
                    obj.GetComponent<CheckersBoardPiece>().x = i;
                    obj.GetComponent<CheckersBoardPiece>().y = j;
                    row.Add(obj.GetComponent<CheckersBoardPiece>());
                    obj.transform.parent = this.transform;
                }
                else if ((j % 2 == 0 && i % 2 != 0) || (j % 2 != 0 && i % 2 == 0))
                {
                    obj = GameObject.Instantiate(WhitePiecePrefab, originPoint.transform.position + new Vector3(i * spacing, j * spacing, 0), Quaternion.identity);
                    obj.GetComponent<CheckersBoardPiece>().x = i;
                    obj.GetComponent<CheckersBoardPiece>().y = j;
                    row.Add(obj.GetComponent<CheckersBoardPiece>());
                    obj.transform.parent = this.transform;
                }               
            }
            pieceList.Add(row);
        }
        return pieceList;
    }

    //function that will place the pieces on either side of the board 
    public void PlaceClientPieces(GameObject pref)
    {
        for(int i = 0; i < BoardSize; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                if ((j % 2 == 0 && i % 2 == 0) || (j % 2 != 0 && i % 2 != 0))
                {
                    pieceList[i][j].occupied = true;
                    Vector3 loc = new Vector3(pieceList[i][j].MoveToLocation.position.x, pieceList[i][j].MoveToLocation.position.y, -1.0f);
                    var obj = GameObject.Instantiate(pref, loc, Quaternion.identity);
                    var pieceComp = obj.GetComponent<PlayerPiece>();
                    pieceComp.x = i;
                    pieceComp.y = j;
                    playerPieces.Add(pieceComp);
                }
            }
        }
    }

    public bool MovePiece(PlayerPiece p, int targetX, int targetY)
    {
        bool moveMade = false;
        int tmpX = p.x;
        int tmpY = p.y;
        if (!pieceList[targetX][targetY].occupied)
        {
            if (p.x + 1 == targetX && p.y + 1 == targetY)
            {
                p.transform.position = pieceList[targetX][targetY].MoveToLocation.position;
                p.x = targetX;
                p.y = targetY;
                moveMade = true;
            }
            else if (p.x - 1 == targetX && p.y + 1 == targetY)
            {
                p.transform.position = pieceList[targetX][targetY].MoveToLocation.position;
                p.x = targetX;
                p.y = targetY;
                moveMade = true;
            }
            if (moveMade)
            {
                pieceList[targetX][targetY].occupied = true;
                pieceList[tmpX][tmpY].occupied = false;
            }
        }
        return moveMade;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
