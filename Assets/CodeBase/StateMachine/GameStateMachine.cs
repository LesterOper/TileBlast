using System;
using System.Collections.Generic;
using CodeBase.StateMachine.GameLoopStates;
using UnityEngine;
using Zenject;

namespace CodeBase.StateMachine
{
    public class GameStateMachine : StateMachineBase
    {
        public GameStateMachine(
            SignalBus signalBus,
            InitializeLevelState initializeLevelState,
            CreateFieldState createFieldState,
            UserClickState userClickState,
            MatchState matchState,
            MoveState moveState,
            LevelProgressChecker levelProgressChecker
        ) : base(signalBus)
        {
            states.Add(typeof(InitializeLevelState), initializeLevelState);
            states.Add(typeof(CreateFieldState), createFieldState);
            states.Add(typeof(UserClickState), userClickState);
            states.Add(typeof(MatchState), matchState);
            states.Add(typeof(MoveState), moveState);
            //states.Add(typeof(LevelProgressChecker), levelProgressChecker);
        }
    }
}