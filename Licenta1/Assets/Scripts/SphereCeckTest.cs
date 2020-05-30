using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Tilemaps;


public class SphereCeckTest : MonoBehaviour
{
    
    public Tilemap tilemap;
    // Start is called before the first frame update
    void Start()
    {
        Vector3Int coords = new Vector3Int(-1, 0, 0);
        TileBase tile = tilemap.GetTile(coords);
        Debug.Log(tile.name);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
