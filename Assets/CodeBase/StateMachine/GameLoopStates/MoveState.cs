using CodeBase.CoreBase.FieldBase;
using Zenject;

namespace CodeBase.StateMachine.GameLoopStates
{
    public class MoveState : State
    {
        private MoveTickable _moveTickable;
        private Matcher _matcher;
        private IField _field;

        public MoveState(
            SignalBus signalBus,
            MoveTickable moveTickable,
            IField field, 
            Matcher matcher) : base(signalBus)
        {
            _moveTickable = moveTickable;
            _matcher = matcher;
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