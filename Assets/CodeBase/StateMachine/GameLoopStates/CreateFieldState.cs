using CodeBase.CoreBase.FieldBase;
using CodeBase.CoreBase.GameStateModel;
using CodeBase.Factory;
using Zenject;

namespace CodeBase.StateMachine.GameLoopStates
{
    public class CreateFieldState : State
    {
        private IField _field;
        
        public CreateFieldState(
            SignalBus signalBus,
            IField field) : base(signalBus)
        {
            _field = field;
        }

        public override void Execute()
        {
            base.Execute();
            _field.Initialize();
            _field.CreateField();
            _signalBus.Fire(new NextStateSignal(typeof(UserClickState)));
        }
    }
}