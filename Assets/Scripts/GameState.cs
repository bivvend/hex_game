using Scripts.Tiles;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Scripts.GameStateEnums;

namespace Scripts
{
    public class GameState
    {
        public InteractionState interactionState { get; private set; } = InteractionState.None;

        public SelectionState selectionState { get; private set; } = SelectionState.None ;

        public MapMode mapMode { get; private set; } = MapMode.All;

        public PlayerActive playerActive { get; private set; } = PlayerActive.Good;

        public CurrentAnimationState animationState { get; private set; } = CurrentAnimationState.None;

        public GameState()
        {


        }


        public void SetSelectionState(SelectionState stateIn)
        {

            selectionState = stateIn;
        }


        public void SetInteractionState(InteractionState interactionStateIn)
        {

            interactionState = interactionStateIn;
        }


        public void SetMapMode(MapMode mapModeIn)
        {
            mapMode = mapModeIn;
        }

        public void SetPlayerActive(PlayerActive playerActiveIn)
        {
            playerActive = playerActiveIn;
        }

        public void SetAnimationState(CurrentAnimationState animationStateIn)
        {

            animationState = animationStateIn;
        }

        public OwnerType PlayerActiveToOwnerType()
        {
            if (this.playerActive == GameStateEnums.PlayerActive.Good)
            {
                return OwnerType.Good;
            }
            else
            {
                return OwnerType.Evil;
            }
        }



    }
}
