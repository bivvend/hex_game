using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scripts
{
    public class BoardStateEnums
    {
        /// <summary>
        /// State of the interactions (e.g. clicking on tiles, placing unit etc.)
        /// </summary>
        public enum InteractionState
        {
            None = 0,  //Cannot interact as is not your turn or animations are running
            PlacingUnit = 1,
            PlacingDevelopment = 2,
            MovingHero = 3,
            Capital = 4,
            WaitingForStateChange = 5  //Default state during your turn  (doing nothing)
        }

        public enum SelectionState
        {
            SelectedOwnTile = 0,
            SelectedEnemyTile = 1,
            SelectedEnemyUnit = 2,
            SelectedFriendlyUnit = 3

        }

        public enum MapMode
        {
            All = 0, //All drawn
            Units = 1,  //Show units and no terrain
            Developments = 2,  //Hide units and terrain and just show Developments 
            Terrain = 3,  // Only show terrain
            
        }

        public enum PlayerActive
        {
            Good = 0,
            Evil = 1

        }

        public enum AnimationState
        {
            MovingUnit = 0,
            BattleAnimation = 1,
            PlacingDevelopmentFade = 2,
        }

    }
}
