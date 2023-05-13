
using Scripts.Tiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Cards
{
    public class DevelopmentCard : Card
    {
        public UtilityType developmentType;
        public int variant = 1;

        public DevelopmentCard(CardType type, UtilityType developmentType, List<Cost> costs, int variant) : base(type, costs)
        {
            this.developmentType = developmentType;
            this.variant = variant;
        }
    }
}
