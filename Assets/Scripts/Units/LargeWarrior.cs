using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Scripts.Units
{
    public class LargeWarrior: Unit
    {
        public override int GetNumberOfTroops()
        {
            return GameScaling.largeTroopMultiplier;
        }

    }
}
