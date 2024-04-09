using Zenject;

namespace CodeBase.StateMachine
{
    public class State
    {
        protected SignalBus _signalBus;
        public State(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        public virtual void Execute()
        {
            
        }

        public virtual void Exit()
        {
            
        }
    }
}