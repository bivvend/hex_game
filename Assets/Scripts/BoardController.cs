using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Tiles;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;

namespace Scripts
{
    public class BoardController : MonoBehaviour
    {
        [HideInInspector]
        public List<GameObject> tiles = new List<GameObject>();

        public GameObject baseTilePrefab;

        // Start is called before the first frame update
        void Start()
        {
            GameObject tile = new GameObject("HexTile");
            tile.AddComponent<HexTile>();
            HexTile t = tile.GetComponent<HexTile>();
            t.tileSpritePrefab = baseTilePrefab;
            //Instantiate(tile);
            tiles.Add(tile);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
