using Scripts;
using Scripts.Tiles;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Players
{
    /// <summary>
    /// Player object tracks a players balance
    /// </summary>
    public class Player: MonoBehaviour 
    {
        private List<ResourceBalance> _balances = new List<ResourceBalance>();
        public OwnerType ownerType;

        /// <summary>
        /// Remove a cost from the balance
        /// </summary>
        /// <param name="cost"></param>
        public void RemoveCostFromBalance(Cost cost)
        {
            var bal = _balances.Where((b) => b.resourceType == cost.costType).ToList();

            if (bal.Count > 0)
            {
                bal.First().balance -= cost.cost;
            }
            else
            {
                Debug.LogError("Cost type not found");
            }
        }

        /// <summary>
        /// Add an income to balance
        /// </summary>
        /// <param name="income"></param>
        public void AddIncomeToBalance(Cost income)
        {
            var bal = _balances.Where((b) => b.resourceType == income.costType).ToList();

            if (bal.Count > 0)
            {
                bal.First().balance += income.cost;
            }
            else
            {
                Debug.LogError("Income type not found");
            }
        }



        /// <summary>
        /// Get the balance of a resource
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ResourceBalance GetBalance(ResourceType type)
        {
            var bal = _balances.Where((b) =>  b.resourceType == type).ToList();

            if(bal.Count > 0)
            {
                return bal.First();
            }
            else
            {
                return new ResourceBalance(type, 0);
            }

        }

        public void Start()
        {

            _balances.Add(new ResourceBalance(ResourceType.Food, 0));
            _balances.Add(new ResourceBalance(ResourceType.Metal, 0));
            _balances.Add(new ResourceBalance(ResourceType.Gold, 0));
            _balances.Add(new ResourceBalance(ResourceType.Mana, 0));

            GameScaling.initialBalances.ForEach((b) => {
                AddIncomeToBalance(b);
            });

        }


        public void Update()
        {
            
        }
    }
}
