using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scripts.Units
{
    public class Warrior : Unit
    {
        public override int GetNumberOfTroops()
        {
            return GameScaling.normalTroopMultiplier;
        }

    }
}
