using Scripts.Cards;
using Scripts.Tiles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Scripts.Units.UnitEnums;

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
        public static readonly Dictionary<UtilityType, List<Cost>> developmentPurchaseCostList = new Dictionary<UtilityType, List<Cost>>()
        {
            {UtilityType.Mine, new List<Cost>(){new Cost(ResourceType.Gold, 1)} },
            {UtilityType.Capital, new List<Cost>(){ new Cost(ResourceType.Gold, 0) } },
            {UtilityType.Fort, new List<Cost>(){ new Cost(ResourceType.Gold, 2), new Cost(ResourceType.Metal, 2) } },
            {UtilityType.Town, new List<Cost>(){ new Cost(ResourceType.Gold, 2), new Cost(ResourceType.Metal, 2), new Cost(ResourceType.Food, 2) } },
            {UtilityType.Farm, new List<Cost>(){ new Cost(ResourceType.Gold, 1), new Cost(ResourceType.Metal, 1) } },
            {UtilityType.QuestSite, new List<Cost>(){  } },
            {UtilityType.SorcerersTower, new List<Cost>(){ new Cost(ResourceType.Gold, 5), new Cost(ResourceType.Metal, 2) } },

        };


        //Income from developments

        public static readonly Dictionary<UtilityType, List<Cost>> developmentBaseIncomeList = new Dictionary<UtilityType, List<Cost>>()
        {
            {UtilityType.Mine, new List<Cost>(){new Cost(ResourceType.Metal, 2)} },
            {UtilityType.Capital, new List<Cost>(){ new Cost(ResourceType.Gold, 3), new Cost(ResourceType.Metal, 2), new Cost(ResourceType.Food, 2) } },
            {UtilityType.Fort, new List<Cost>(){}},
            {UtilityType.Town, new List<Cost>(){ new Cost(ResourceType.Gold, 3)} },
            {UtilityType.Farm, new List<Cost>(){ new Cost(ResourceType.Food, 2) } },
            {UtilityType.QuestSite, new List<Cost>(){ new Cost(ResourceType.Gold, 5)} },
            {UtilityType.SorcerersTower, new List<Cost>(){ new Cost(ResourceType.Mana, 2) } },

        };

        public static readonly Dictionary<UtilityType, List<Cost>> developmentUpkeepList = new Dictionary<UtilityType, List<Cost>>()
        {
            {UtilityType.Mine, new List<Cost>(){new Cost(ResourceType.Food, 1)} },
            {UtilityType.Capital, new List<Cost>(){} },
            {UtilityType.Fort, new List<Cost>(){new Cost(ResourceType.Gold, 1), new Cost(ResourceType.Metal, 1)}},
            {UtilityType.Town, new List<Cost>(){ new Cost(ResourceType.Food, 1), new Cost(ResourceType.Metal, 1) } },
            {UtilityType.Farm, new List<Cost>(){ new Cost(ResourceType.Gold, 1) } },
            {UtilityType.QuestSite, new List<Cost>(){} },
            {UtilityType.SorcerersTower, new List<Cost>(){ new Cost(ResourceType.Gold, 2) } },

        };

        public static readonly Dictionary<UnitType, List<Cost>> unitUpkeepList = new Dictionary<UnitType, List<Cost>>()
        {
            //These are all the same as the multipliers define the cost (a large is N normal etc)
            {UnitType.NormalWarrior, new List<Cost>(){ new Cost(ResourceType.Gold, 1) } },
            {UnitType.LargeWarrior, new List<Cost>(){ new Cost(ResourceType.Gold, 1) } },
            {UnitType.VeryLargeWarrior, new List<Cost>(){ new Cost(ResourceType.Gold, 1) } },
            {UnitType.Wizard, new List<Cost>(){ new Cost(ResourceType.Gold, 1) } },
            {UnitType.General, new List<Cost>(){ new Cost(ResourceType.Gold, 1) } },
            {UnitType.Hero, new List<Cost>(){ new Cost(ResourceType.Gold, 1) } },

        };

        public static readonly Dictionary<UnitType, List<Cost>> unitPurchaseCostList = new Dictionary<UnitType, List<Cost>>()
        {
            //These are all the same as the multipliers define the cost (a large is N normal etc)
            {UnitType.NormalWarrior, new List<Cost>(){ new Cost(ResourceType.Gold, 1), new Cost(ResourceType.Food, 1), new Cost(ResourceType.Metal, 1), } },
            {UnitType.LargeWarrior, new List<Cost>(){ new Cost(ResourceType.Gold, 1), new Cost(ResourceType.Food, 1), new Cost(ResourceType.Metal, 1), } },
            {UnitType.VeryLargeWarrior, new List<Cost>(){ new Cost(ResourceType.Gold, 1), new Cost(ResourceType.Food, 1), new Cost(ResourceType.Metal, 1), } },
            {UnitType.Wizard, new List<Cost>(){ new Cost(ResourceType.Gold, 5), new Cost(ResourceType.Mana, 5), } },
            {UnitType.General, new List<Cost>(){ new Cost(ResourceType.Gold, 5), new Cost(ResourceType.Food, 5), new Cost(ResourceType.Metal, 5), } },
            {UnitType.Hero, new List<Cost>(){ new Cost(ResourceType.Gold, 5) } },

        };


        //Cost to shuffle cards
        public static readonly List<Cost> shuffleCost = new List<Cost>
        {
            new Cost(ResourceType.Gold, 0)

        };


        //Adjacency bonuses


        //Cost of troops


        //Card setup
        public static Dictionary<UtilityType, int> intialDeckBalance = new Dictionary<UtilityType, int>
        {
            {UtilityType.Capital, 0},
            {UtilityType.Mine, 30},
            {UtilityType.Fort, 10},
            {UtilityType.Town, 30},
            {UtilityType.Farm, 30},
            {UtilityType.QuestSite, 4},
            {UtilityType.SorcerersTower, 4},

        };

        public static int numberOfCards => GetNumberOfCards();


        //Initial balances
        public static List<Cost> initialBalances = new List<Cost>() {
            new Cost(ResourceType.Food, 4),
            new Cost(ResourceType.Gold, 3),
            new Cost(ResourceType.Metal, 2),
            new Cost(ResourceType.Mana, 1)
        };
        

        public static DevelopmentCard GetRandomDevelopmentCard()
        {
            System.Random random = new System.Random();
            Array values = Enum.GetValues(typeof(UtilityType));
            UtilityType randomType = (UtilityType)values.GetValue(random.Next(values.Length));

            return new DevelopmentCard(CardType.Development, randomType, developmentPurchaseCostList[randomType], random.Next(1,5));

        }

        public static DevelopmentCard GetNewDevelopmentCard(UtilityType utilityType)
        {
            System.Random random = new System.Random();

            return new DevelopmentCard(CardType.Development, utilityType, developmentPurchaseCostList[utilityType], random.Next(1, 5));

        }


        private static int GetNumberOfCards()
        {
            int cardCount = 0;
            intialDeckBalance.Keys.ToList().ForEach((k) =>
            {
                cardCount+= intialDeckBalance[k];
            });

            return cardCount;
        }


        public static List<DevelopmentCard> BuildNewDeck()
        {

            List<DevelopmentCard> cards = new List<DevelopmentCard>();
            intialDeckBalance.Keys.ToList().ForEach((k) =>
            {
                for(int i = 0 ; i< intialDeckBalance[k]; i++)
                {
                    cards.Add(GetNewDevelopmentCard(k));
                }
            });

            return cards;
        }

    }

    public class Cost
    {
        public ResourceType costType;
        public int cost;

        public Cost(ResourceType costType, int cost)
        {
            this.costType = costType;
            this.cost = cost;
        }
    }

    public class ResourceBalance
    {
        public ResourceType resourceType;
        public int balance;
        
        public ResourceBalance(ResourceType type, int balance)
        {
            this.resourceType = type;
            this.balance = balance;
        }

    }

    public enum ResourceType
    {
        Food = 0,
        Gold = 1,
        Metal = 2,
        Mana = 3, 
    }
}
