using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Scripts.Units.UnitEnums;

namespace Scripts.Cards
{
    public class UnitCard : Card
    {
        public UnitType unitType;

        public UnitCard(CardType type, UnitType unitType,  List<Cost> costs) : base(type, costs)
        {
            this.unitType = unitType;
        }
    }
}
