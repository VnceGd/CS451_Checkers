using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public static ScrollingBackground instance = null;

    public BackgroundObject ClumpOne;
    public BackgroundObject ClumpTwo;

    public Transform startPosition;

    public List<BackgroundObject> BackgroundClumps;
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
    void Start()
    {
        for(int i = 0; i < BackgroundClumps.Count; i++)
        {
            BackgroundClumps[i].clumpId = i;
        }
    }
    //disables a clump
    public void DisableClump(int id)
    {
        if(id + 1 <= BackgroundClumps.Count)
        {
            RefreshClump(id);
        }
        else
        {
            RefreshClump(0);
        }
    }
    //refreshes a clump
    void RefreshClump(int id)
    {
        BackgroundClumps[id].gameObject.transform.position = startPosition.position;
        BackgroundClumps[id].gameObject.SetActive(true);
    }
}
