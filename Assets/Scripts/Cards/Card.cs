using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace Scripts.Cards
{

    public abstract class Card: MonoBehaviour
    {

        GameObject spritePrefab;
        SpriteResolver spriteResolver;

        public CardType Type { get; set; }

        public List<Cost> Costs { get; set; }

        public Card(CardType type, List<Cost> costs)
        {
            Type = type;
            Costs = costs;
        }

        private void Start()
        {
            
        }


        private void Update()
        {
            
        }
    }
}

