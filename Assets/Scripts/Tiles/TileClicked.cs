using Scripts.Tiles;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TileClicked : MonoBehaviour
{
    // Start is called before the first frame update
    
    void OnMouseDown()
    {
        Debug.Log("Click from script!");
        transform.parent.GetComponent<HexTile>().Clicked();
    }
}
