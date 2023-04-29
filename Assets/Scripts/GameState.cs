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



    }
}
