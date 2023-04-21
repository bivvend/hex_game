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
        Water = 4
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


    
}