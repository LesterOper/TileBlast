using CodeBase.CoreBase.AimTopBar;
using CodeBase.CoreBase.Configs;
using CodeBase.CoreBase.FieldBase;
using CodeBase.CoreBase.GameStateModel;
using Zenject;

namespace CodeBase.StateMachine.GameLoopStates
{
    public class MoveState : State
    {
        private GameModel _gameModel;
        private MoveTickable _moveTickable;
        private IField _field;
        private AimController _aimController;

        public MoveState(
            SignalBus signalBus,
            MoveTickable moveTickable,
            IField field,
            GameModel gameModel,
            AimController aimController) : base(signalBus)
        {
            _aimController = aimController;
            _moveTickable = moveTickable;
            _gameModel = gameModel;
            _field = field;
        }

        public override void Execute()
        {
            base.Execute();
            var movableItems = _field.GetMovableItems();
            if (movableItems.Count > 0)
            {
                _moveTickable.AddMovableItems(movableItems);
            }
            else _signalBus.Fire(new NextStateSignal(typeof(UserClickState)));
        }

        public override void Exit(NextStateSignal nextStateSignal)
        {
            base.Exit(nextStateSignal);
            if (_gameModel.Level.levelType == LevelType.DROP_OBJECTS_TO_BOTTOM)
            {
                _aimController.UpdateAims();
            }
            if (_gameModel.Level.levelType != LevelType.CLEAR_FIELD)
            {
                if (_gameModel.FieldModel.GetEmptyCellsCount() > 0)
                {
                    nextStateSignal.nextState = typeof(MoveState);
                }
            }
        }
    }
}