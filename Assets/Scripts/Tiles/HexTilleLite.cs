using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace Scripts.Tiles
{
    /// <summary>
    /// A light weight object for search routines.
    /// </summary>
    public class HexTileLite
    {
        

        //Indicies in cube coords
        public int qIndex = 0;
        public int rIndex = 0;
        public int sIndex = 0;

        //Tile data
        //Terrain

        public TerrainType TerrainType;
        //List of 6 utility types one for each side

        public List<UtilityType> Developments = new();

        public OwnerType owner = OwnerType.Good;

        public bool visited = false;

        public HexTileLite(int qIndex, int rIndex, int sIndex, TerrainType terrainType, List<UtilityType> developments, OwnerType owner, bool visited)
        {
            this.qIndex = qIndex;
            this.rIndex = rIndex;
            this.sIndex = sIndex;
            TerrainType = terrainType;
            Developments = developments;
            this.owner = owner;
            this.visited = visited;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="hexTile"></param>
        public HexTileLite(HexTile hexTile)
        {
            this.owner =hexTile.owner;
            this.visited = false;
            this.qIndex = hexTile.qIndex;
            this.rIndex =hexTile.rIndex;
            this.sIndex = hexTile.sIndex;
            this.Developments = hexTile.Developments;
            this.TerrainType = hexTile.TerrainType;
        }
    }
}