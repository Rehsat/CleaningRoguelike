using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GameStateMachine
{
    public class GameStateMachine : StateMachine<ILevelState>
    {
        public GameStateMachine(List<ILevelState> states) : base(states)
        {
            foreach (var state in States.Values)
            {
                state.SetStateMachine(this);
            }
            EnterState<BootstrapState>();
        }
    }

    public interface ILevelState : IState
    {
        public void SetStateMachine(GameStateMachine stateMachine);
    }
}