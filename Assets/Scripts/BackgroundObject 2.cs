using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundObject : MonoBehaviour
{
    private ScrollingBackground _bg;
    public float speed;
    public Vector3 direction;
    void Start()
    {
        _bg = ScrollingBackground.instance;
    }
    public int clumpId;
    /*
    private void OnBecameInvisible()
    {
        _bg.DisableClump(clumpId);
    }
    */
    void Update()
    {
        transform.position += direction * Time.deltaTime * speed;
        if(transform.position.x > 8.893 && transform.position.y < -4.996)
        {
            _bg.DisableClump(clumpId);
        }
    }
}
