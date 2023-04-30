
using Scripts.Tiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Cards
{
    public class DevelopmentCard : Card
    {
        UtilityType developmentType;

        public DevelopmentCard(CardType type, UtilityType developmentType, List<Cost> costs) : base(type, costs)
        {
            this.developmentType = developmentType;

        }
    }
}
