using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Tiles;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;
using UnityEngine.U2D.Animation;
using UnityEngine.WSA;
using Unity.VisualScripting.Antlr3.Runtime;
using System;
using System.Linq;
using static UnityEngine.RuleTile.TilingRuleOutput;
using System.Drawing;
using UnityEngine.U2D;

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
            Array values = Enum.GetValues(typeof(TerrainType));
            System.Random random = new System.Random();

            

            //Create a circular map of given radius
            for (int q = (-1* (mapRadius +2)); q <= (mapRadius + 2); q++)
            {
                for (int r = (-1 * (mapRadius + 2)); r <= (mapRadius + 2); r++)
                {
                    for (int s = (-1 * (mapRadius + 2)); s <= (mapRadius + 2); s++)
                    {
                        if (s == -q - r)
                        {
                            GameObject tile = new GameObject("HexTile");
                            tile.AddComponent<HexTile>();
                            HexTile t = tile.GetComponent<HexTile>();
                            t.tileSpritePrefab = baseTilePrefab;
                            t.edgeSpritePrefab = edgePrefab;
                            t.boardController = this;

                            TerrainType randomType = (TerrainType)values.GetValue(random.Next(values.Length));
                            if(randomType == TerrainType.Water)
                            {
                                randomType = TerrainType.Grass;
                            }    

                            if(Math.Abs(q) == mapRadius + 2 || Math.Abs(r) == mapRadius + 2 || Math.Abs(s) == mapRadius + 2)
                            {
                                randomType = TerrainType.Water;
                            }
                            if (Math.Abs(q) == mapRadius + 1 || Math.Abs(r) == mapRadius + 1|| Math.Abs(s) == mapRadius + 1)
                            {
                                //Make a rough coast
                                if (random.Next(0, 2) > 0)
                                {
                                    randomType = TerrainType.Water;
                                }
                            }

                            t.TerrainType = randomType;
                            t.qIndex = q;
                            t.rIndex = r;
                            t.sIndex = s;

                            t.owner = OwnerType.Neutral;
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


        public void ClickedTile(HexTile hexTile)
        {
            //This looks at current game state to determine the role of the click
            
            
            Debug.Log($"{hexTile.qIndex},{hexTile.rIndex},{hexTile.sIndex}");
            //Go through all tiles and deselect rest
            tiles.ForEach((t) =>
            {
                HexTile tFound = t.GetComponent<HexTile>();
                if (tFound.qIndex == hexTile.qIndex && tFound.rIndex == hexTile.rIndex && tFound.sIndex == hexTile.sIndex)
                {
                    tFound.ChangeSelectionStatus(true);


                }
                else
                {
                    tFound.ChangeSelectionStatus(false);
                }

            });

            //HighlightNeighbours(hexTile, HighlightColor.Green);

            //HighlightMatchedAndConnectedTiles(hexTile, new List<SearchStrategy> { SearchStrategy.Terrain }, SearchMatchMethod.All, HighlightColor.Green);

            //HighlightMatchedNeighbours(hexTile, new List<SearchStrategy> { SearchStrategy.Owner, SearchStrategy.Terrain }, SearchMatchMethod.Any, HighlightColor.Green);

            HighlightBuildableTiles(OwnerType.Neutral, HighlightColor.Red);

        }

        /// <summary>
        /// Highlights all the neigbours 
        /// </summary>
        /// <param name="hexTile"></param>
        public void HighlightNeighbours(HexTile hexTile, HighlightColor colour)
        {
            //Get a list of all possible indicies for neighbours
            List<(int, int, int)> neighbours = TIleUtilities.GetNeighbours(hexTile.qIndex, hexTile.rIndex);

            //Highlight all these tiles
            bool found = false;
            tiles.ForEach((t) =>
            {
                var tile = t.GetComponent<HexTile>();
                found = false;
                neighbours.ForEach((tN) =>
                {
                    if(tN.Item1 == tile.qIndex && tN.Item2 == tile.rIndex && tN.Item3 == tile.sIndex)
                    {
                        found = true;
                    }
                });
                if(found)
                {
                    switch(colour)
                    {
                        case HighlightColor.Green:
                            tile.ChangeHighlightGreenStatus(true);
                            break;

                        case HighlightColor.Red:
                            tile.ChangeHighlightRedStatus(true);
                            break;

                    }
                    
                }
                else
                {
                    tile.ChangeHighlightGreenStatus(false);
                    tile.ChangeHighlightRedStatus(false);

                }

            });

        }

        /// <summary>
        /// Flood fills highlights for tiles connected in chains to the clicked tile
        /// Flexible match criteria
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="searchStrategies"></param>
        /// <param name="method"></param>
        /// <param name="colour"></param>
        public void HighlightMatchedAndConnectedTiles(HexTile tile, List<SearchStrategy> searchStrategies, SearchMatchMethod method, HighlightColor colour)
        {
            //Convert tiles to a new list of HexTileLite
            HexTileLite liteTile = new HexTileLite(tile.GetComponent<HexTile>());
            var liteList = tiles.Select((t) => new HexTileLite(t.GetComponent<HexTile>())).ToList();
            var matchedTiles = TIleUtilities.FindConnectedTilesByCategory(liteList, liteTile, searchStrategies, method);

            bool found = false;
            tiles.ForEach((t) =>
            {
                var tile = t.GetComponent<HexTile>();
                found = false;
                matchedTiles.ForEach((tN) =>
                {
                    if (tN.qIndex == tile.qIndex && tN.rIndex == tile.rIndex && tN.sIndex == tile.sIndex)
                    {
                        found = true;
                    }
                });
                if (found)
                {
                    switch (colour)
                    {
                        case HighlightColor.Green:
                            tile.ChangeHighlightGreenStatus(true);
                            break;

                        case HighlightColor.Red:
                            tile.ChangeHighlightRedStatus(true);
                            break;

                    }

                }
                else
                {
                    tile.ChangeHighlightGreenStatus(false);
                    tile.ChangeHighlightRedStatus(false);

                }

            });
        }

        /// <summary>
        /// Highlight the neighbour tiles with a matched set of criteria
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="searchStrategies"></param>
        /// <param name="method"></param>
        /// <param name="colour"></param>
        public void HighlightMatchedNeighbours(HexTile tile, List<SearchStrategy> searchStrategies, SearchMatchMethod method, HighlightColor colour)
        {
            //Convert tiles to a new list of HexTileLite
            HexTileLite liteTile = new HexTileLite(tile.GetComponent<HexTile>());
            var liteList = tiles.Select((t) => new HexTileLite(t.GetComponent<HexTile>())).ToList();
            var matchedTiles = TIleUtilities.GetNeighboursWithCriteria(liteList, liteTile, searchStrategies, method);

            bool found = false;
            tiles.ForEach((t) =>
            {
                var tile = t.GetComponent<HexTile>();
                found = false;
                matchedTiles.ForEach((tN) =>
                {
                    if (tN.qIndex == tile.qIndex && tN.rIndex == tile.rIndex && tN.sIndex == tile.sIndex)
                    {
                        found = true;
                    }
                });
                if (found)
                {
                    switch (colour)
                    {
                        case HighlightColor.Green:
                            tile.ChangeHighlightGreenStatus(true);
                            break;

                        case HighlightColor.Red:
                            tile.ChangeHighlightRedStatus(true);
                            break;

                    }

                }
                else
                {
                    tile.ChangeHighlightGreenStatus(false);
                    tile.ChangeHighlightRedStatus(false);

                }

            });
        }


        public void HighlightBuildableTiles(OwnerType ownerType, HighlightColor colour)
        {
            //Convert tiles to a new list of HexTileLite
            HexTileLite targetTile = new HexTileLite(0, 0, 0, TerrainType.Grass, new List<UtilityType> { }, ownerType, false) ;
            var liteList = tiles.Select((t) => new HexTileLite(t.GetComponent<HexTile>())).ToList();
            var matchedTiles = TIleUtilities.FilterTilesWithCriteria(liteList, targetTile, new List<SearchStrategy> { SearchStrategy.Owner}, SearchMatchMethod.All);


            bool found = false;
            tiles.ForEach((t) =>
            {
                var tile = t.GetComponent<HexTile>();
                found = false;
                matchedTiles.ForEach((tN) =>
                {
                    if (tN.qIndex == tile.qIndex && tN.rIndex == tile.rIndex && tN.sIndex == tile.sIndex)
                    {
                        found = true;
                    }
                });
                if (found)
                {
                    switch (colour)
                    {
                        case HighlightColor.Green:
                            tile.ChangeHighlightGreenStatus(true);
                            break;

                        case HighlightColor.Red:
                            tile.ChangeHighlightRedStatus(true);
                            break;

                    }

                }
                else
                {
                    tile.ChangeHighlightGreenStatus(false);
                    tile.ChangeHighlightRedStatus(false);

                }

            });

        }
    }
}
