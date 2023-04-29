using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scripts
{
    public class GameStateEnums
    {
        /// <summary>
        /// State of the interactions (e.g. clicking on tiles, placing unit etc.)
        /// </summary>
        public enum InteractionState
        {
            None = 0,  //Cannot interact as animations are running or is AI turn
            PlacingUnit = 1, //Waiting for a unit to be placed
            PlacingHero = 2,
            PlacingDevelopment = 3,
            MovingHero = 4,
            Capital = 5,
            SelectTile = 6  //Default state during your turn  (doing nothing) but can click on tile for info
        }

        public enum SelectionState
        {
            SelectedOwnTile = 0,
            SelectedEnemyTile = 1,
            None = 2,

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

        public enum CurrentAnimationState
        {
            MovingUnit = 0,
            BattleAnimation = 1,
            PlacingDevelopmentFade = 2,
            None = 3
        }

    }
}
