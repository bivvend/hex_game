using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public static class GameScaling
    {
        //Scalings for different troops

        public static int normalTroopMultiplier = 1;

        public static int largeTroopMultiplier = 5;

        public static int veryLargeTroopMultiplier = 10;

        public static int heroMultiplier = 1;

        public static int generalMultiplier = 2;

        public static int wizardMultiplier = 3;

        //Cost of tiles developments
        public static List<Cost> costList = new List<Cost>();

        //Income from developments



        //Adjacency bonuses


        //Cost of troops

        
        

    }

    public class Cost
    {
        public CostType costType;
        public int cost;

        public Cost(CostType costType, int cost)
        {
            this.costType = costType;
            this.cost = cost;
        }
    }

    public enum CostType
    {
        Food = 0,
        Gold = 1,
        Metal = 2,
        Mana = 3, 
    }
}
