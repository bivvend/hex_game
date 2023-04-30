using Scripts.Tiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Scripts.Units.UnitEnums;

namespace Scripts.Units
{
    public abstract class Unit
    {
        public int numberOfTroops;
        public UnitType unitType;
        public OwnerType ownerType;

        public abstract int GetNumberOfTroops();


    }
}
