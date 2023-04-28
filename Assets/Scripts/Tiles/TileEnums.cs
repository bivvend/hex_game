using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scripts.Tiles
{

        
    /// <summary>
    /// Base terrain of map
    /// </summary>
    public enum TerrainType
    {

        Grass = 0,
        Mountains = 1,
        Hills = 2,
        Swamp = 3,
        Water = 4,
        Forest = 5,
    }

    //Development overlay (from cards). There are 6 of these per hex
    public enum UtilityType
    {
        Mine = 0,
        Town = 1,
        Farm = 2,
        Fort = 3,
        Capital = 4,
    }

    //Determines which graphics set is used for rendering base and overlay
    public enum OwnerType
    {
        Good = 0,
        Evil = 1,
    }


    public enum HighlightColor
    {
        Green = 0,
        Red = 1
    }

    /// <summary>
    /// Method used to find a set of connected tiles by type
    /// </summary>
    public enum SearchStrategy
    {
        Terrain = 0,
        Owner = 1,
        DevelopmentType = 2
    }

    public enum SearchMatchMethod
    {
        All = 0,
        Any = 1,
    }


    
}
