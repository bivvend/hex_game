
using Scripts.Units;
using System.Collections.Generic;
using static Scripts.Units.UnitEnums;

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

        public List<Unit> Units = new();


        public OwnerType owner = OwnerType.Good;

        //Used in some search routines
        public bool visited = false;


        public bool hasUnits = false;

        public HexTileLite(int qIndex, int rIndex, int sIndex, TerrainType terrainType, List<UtilityType> developments, OwnerType owner, bool visited, bool hasUnits, List<Unit> units)
        {
            this.qIndex = qIndex;
            this.rIndex = rIndex;
            this.sIndex = sIndex;
            TerrainType = terrainType;
            Developments = developments;
            this.owner = owner;
            this.visited = visited;
            this.hasUnits = hasUnits;
            this.Units = units;
        }

        /// <summary>
        /// Copy constructor from Hextile
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
            this.hasUnits = hexTile.Units.Count > 0;
            this.Units = hexTile.Units;

        }

        /// <summary>
        /// Copy construstor 
        /// </summary>
        /// <param name="hexTile"></param>
        public HexTileLite(HexTileLite hexTile)
        {
            this.owner = hexTile.owner;
            this.visited = false;
            this.qIndex = hexTile.qIndex;
            this.rIndex = hexTile.rIndex;
            this.sIndex = hexTile.sIndex;
            this.Developments = hexTile.Developments;
            this.TerrainType = hexTile.TerrainType;
            this.hasUnits = hexTile.hasUnits;
            this.Units = hexTile.Units;
        }
    }
}
