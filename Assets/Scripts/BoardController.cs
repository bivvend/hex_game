using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Tiles;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;
using UnityEngine.U2D.Animation;

namespace Scripts
{
    public class BoardController : MonoBehaviour
    {
        [HideInInspector]
        public List<GameObject> tiles = new List<GameObject>();

        public GameObject baseTilePrefab;
        public GameObject edgePrefab;
        public int mapRadius;

        // Start is called before the first frame update
        void Start()
        {

            //Create a circular map of given radius
            for (int q = (-1* (mapRadius +1)); q <= (mapRadius + 1); q++)
            {
                for (int r = (-1 * (mapRadius + 1)); r <= (mapRadius + 1); r++)
                {
                    for (int s = (-1 * (mapRadius + 1)); s <= (mapRadius + 1); s++)
                    {
                        if (s == -q - r)
                        {
                            GameObject tile = new GameObject("HexTile");
                            tile.AddComponent<HexTile>();
                            HexTile t = tile.GetComponent<HexTile>();
                            t.tileSpritePrefab = baseTilePrefab;
                            t.edgeSpritePrefab = edgePrefab;
                            t.TerrainType = TerrainType.Forest;
                            t.qIndex = q;
                            t.rIndex = r;
                            t.sIndex = s;
                            tiles.Add(tile);
                        }
                    }

                }
            }
            
           
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
