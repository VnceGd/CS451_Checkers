using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    public int BoardSize = 0;
    public GameObject WhitePiecePrefab;
    public GameObject BlackPiecePrefab;
    public float spacing = 2.0f;
    public Transform originPoint;
    // Start is called before the first frame update
    void Start()
    {
        GenerateBoard();
    }

    //instantiate method at first
    //dont really know if we want to set up a pool for this
    public void GenerateBoard()
    {
        for(int i = 0; i < BoardSize; i++)
        {
            for(int j = 0; j < BoardSize; j++)
            {
                if ((j % 2 == 0 && i % 2 == 0) || (j % 2 != 0 && i % 2 != 0))
                {
                    var obj = GameObject.Instantiate(BlackPiecePrefab, originPoint.transform.position + new Vector3(i * spacing, j * spacing, 0), Quaternion.identity);
                }
                else if ((j % 2 == 0 && i % 2 != 0) || (j % 2 != 0 && i % 2 == 0))
                {
                    var obj = GameObject.Instantiate(WhitePiecePrefab, originPoint.transform.position + new Vector3(i * spacing, j * spacing, 0), Quaternion.identity);
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
