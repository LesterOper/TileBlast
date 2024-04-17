using CodeBase.CoreBase.FieldBase;
using Zenject;

namespace CodeBase.StateMachine.GameLoopStates
{
    public class MoveState : State
    {
        private MoveTickable _moveTickable;
        private IField _field;

        public MoveState(
            SignalBus signalBus,
            MoveTickable moveTickable,
            IField field) : base(signalBus)
        {
            _moveTickable = moveTickable;
            _field = field;
        }

        public override void Execute()
        {
            base.Execute();
            var movableItems = _field.GetMovableItems();
            _moveTickable.AddMovableItems(movableItems);
            _signalBus.Fire(new NextStateSignal(typeof(LevelProgressCheckerState)));
        }
    }
}