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
        private AfterMatchFieldNormalizer _movables;
        private AimController _aimController;

        public MoveState(
            SignalBus signalBus,
            MoveTickable moveTickable,
            AfterMatchFieldNormalizer movables,
            GameModel gameModel,
            AimController aimController) : base(signalBus)
        {
            _aimController = aimController;
            _moveTickable = moveTickable;
            _gameModel = gameModel;
            _movables = movables;
        }

        public override void Execute()
        {
            base.Execute();
            var movableItems = _movables.GetMovableItems();
            if (movableItems.Count > 0)
            {
                _moveTickable.AddMovableItems(movableItems);
            }
            else _signalBus.Fire(new NextStateSignal(typeof(UserClickState)));
        }

        public override void Exit(NextStateSignal nextStateSignal)
        {
            base.Exit(nextStateSignal);
            if (_gameModel.Level.levelData.levelType == LevelType.DROP_OBJECTS_TO_BOTTOM)
            {
                _aimController.UpdateAims();
            }
            if (_gameModel.Level.levelData.levelType != LevelType.CLEAR_FIELD)
            {
                if (_gameModel.FieldModel.GetEmptyCellsCount() > 0)
                {
                    if(_gameModel.FieldModel.NeedToStartMoveState())
                        nextStateSignal.nextState = typeof(MoveState);
                }
            }
        }
    }
}