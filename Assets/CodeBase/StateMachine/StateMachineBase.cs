using System;
using System.Collections.Generic;
using CodeBase.StateMachine.GameLoopStates;
using Zenject;

namespace CodeBase.StateMachine
{
    public class StateMachineBase
    {
        private SignalBus _signalBus;
        protected Dictionary<Type, State> states;
        protected State currentState;
        
        public StateMachineBase(SignalBus signalBus)
        {
            states = new Dictionary<Type, State>();
            _signalBus = signalBus;
            _signalBus.Subscribe<NextStateSignal>(ChangeState);
        }

        public void RunGameLoop()
        {
            currentState = states[typeof(InitializeLevelState)];
            currentState.Execute();
        }
        
        private void ChangeState(NextStateSignal nextStateSignal)
        {
            if(currentState != null)
                currentState.Exit( nextStateSignal);

            if (states.ContainsKey(nextStateSignal.nextState))
            {
                currentState = states[nextStateSignal.nextState];
                currentState.Execute();
            }
        }
    }

    public class NextStateSignal
    {
        public Type nextState;

        public NextStateSignal(Type state)
        {
            nextState = state;
        }
    }
}