using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

namespace Scripts.Tiles
{
    //Useful methods for handling hexes
    public static class TIleUtilities
    {
        /// <summary>
        /// Converts hex basis vector indicies into XY coords.  Note: No scaling
        /// For flat top configuration
        /// </summary>
        /// <param name="q"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static PointF convertHexIndiciesToCartesianFlatTop(int q, int r)
        {
            var x = (3.0f / 2.0f * q);
            var y =Mathf.Sqrt(3.0f) / 2.0f * q + Mathf.Sqrt(3.0f) * r;
            return new PointF(x, y);


        }

        /// <summary>
        /// Returns all the neighbours for a given pair of hex indices
        /// Assumes infinite array, so user needs to check if these tiles exist
        /// </summary>
        /// <param name="q"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static List<(int,int,int)> GetNeighbours(int q, int r)
        {
            List<(int, int, int)> neighbours = new();

            List<(int, int)> axialDirectionVectors = new() {
            (1, 0),
            (1, -1),
            (0, -1),
            (-1, 0),
            (-1, +1),
            (0, +1) };

            axialDirectionVectors.ForEach((v) =>
            {
                int newQ = q + v.Item1;
                int newR = r + v.Item2;
                int newS = -newQ - newR; 
                neighbours.Add((newQ, newR, newS));
            });

            return neighbours;
        }

        /// <summary>
        /// Searches through "tiles" list for those connected in chains to tile.  
        /// </summary>
        /// <param name="tiles"></param>
        /// <param name="tile"></param>
        /// <param name="strategy"></param>
        /// <returns></returns>
        public static List<HexTileLite> FindConnectedTilesByCategory(List<HexTileLite> tiles, HexTileLite tile, List<SearchStrategy> searchStrategies, SearchMatchMethod searchMatchMethod)
        {
            List<HexTileLite> connectedTiles = new();

            Queue<HexTileLite> toVisit = new();
            //Add initial tile
            toVisit.Enqueue(tile);
            connectedTiles.Add(tile);
            while(toVisit.Count > 0)
            {
                //Visit the first tile 
                HexTileLite poppedTile = toVisit.Dequeue(); 
                //mark it as visited in the tile list
                tiles.Where((p) => HasSameIndicies(p, poppedTile)).ToList().First().visited = true;
                //Add all neighbours of the popped tile if they exist in tiles and are valid tiles and match the criteria
                List<(int, int, int)> neighbours = GetNeighbours(poppedTile.qIndex, poppedTile.rIndex);

                neighbours.ForEach((n) => {
                    var matchedTiles =  tiles.Where((p) => HasSameIndiciesSimple(p, n.Item1, n.Item2, n.Item3) && p.visited == false).ToList();
                    matchedTiles.ForEach((m) => {
                        if (MatchesCriteria(m, tile, searchStrategies, searchMatchMethod) && m.visited == false)
                        {
                            tiles.Where((p) => HasSameIndicies(p, m)).ToList().First().visited = true;
                            toVisit.Enqueue(m);
                            connectedTiles.Add(m);
                            
                        }
                    });
                    

                });

            }

            return connectedTiles;
        }

        /// <summary>
        /// Method to compare two tiles based on a set of criterea, can match all or any
        /// </summary>
        /// <param name="tile1"></param>
        /// <param name="tile2"></param>
        /// <param name="strategies"></param>
        /// <param name="searchMatchMethod"></param>
        /// <returns></returns>
        public static bool MatchesCriteria(HexTileLite tile1, HexTileLite tile2, List<SearchStrategy> strategies, SearchMatchMethod searchMatchMethod)
        {
            bool matched = searchMatchMethod switch
            {
                SearchMatchMethod.All => true,
                SearchMatchMethod.Any => false,
                _ => false

            };
            bool nextMatch = false;

            strategies.ForEach((s) => { 
                
                nextMatch = false;
                switch(s)
                {
                    
                    case SearchStrategy.Terrain:
                        if (tile1.TerrainType ==  tile2.TerrainType)
                        {
                            nextMatch = true;
                        }
                        break;
                    case SearchStrategy.Owner:
                        if (tile1.owner == tile2.owner)
                        {
                            nextMatch = true;
                        }
                        break;
                    case SearchStrategy.DevelopmentType:
                        if (tile1.Developments.Intersect(tile2.Developments).ToList().Count > 0)
                        {
                            nextMatch = true;
                        }
                        break;
                    default:
                        nextMatch = false;
                        break;
                }


                matched = searchMatchMethod switch
                {
                    SearchMatchMethod.All => matched && nextMatch,
                    SearchMatchMethod.Any => nextMatch || matched,
                    _ => false

                };

            });



            return matched;
        }

        public static bool HasSameIndicies(HexTileLite tile1, HexTileLite tile2)
        {
            return (tile1.qIndex == tile2.qIndex && tile1.rIndex == tile2.rIndex && tile1.sIndex == tile2.sIndex);
   

        }


        public static bool HasSameIndiciesSimple(HexTileLite tile1, int qIndex, int rIndex, int sIndex)
        {
            return (tile1.qIndex == qIndex && tile1.rIndex == rIndex && tile1.sIndex == sIndex);


        }

        //    function flat_hex_to_pixel(hex):



    }
}
