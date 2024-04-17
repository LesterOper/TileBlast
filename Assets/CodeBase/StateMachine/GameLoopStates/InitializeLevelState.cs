using CodeBase.CoreBase.CellBase;
using CodeBase.CoreBase.Configs;
using CodeBase.CoreBase.FieldBase;
using CodeBase.CoreBase.GameStateModel;
using Zenject;

namespace CodeBase.StateMachine.GameLoopStates
{
    public class InitializeLevelState : State
    {
        private LevelsConfig _levelsConfig;
        private GameModel _gameModel;

        public InitializeLevelState(SignalBus signalBus,
            LevelsConfig levelsConfig,
            GameModel gameModel) : base(signalBus)
        {
            _levelsConfig = levelsConfig;
            _gameModel = gameModel;
        }

        public override void Execute()
        {
            base.Execute();
            _levelsConfig.GetLevel().GenerateFieldPosition();
            var level = _levelsConfig.GetLevel();
            _gameModel.GameplayCountersModelProperty.AimModelSetterProperty.SetLevelAims(level.LevelAims);
            _gameModel.GameplayCountersModelProperty.MovesModelProperty.Value = (int) level.moves;
            _gameModel.FieldModel.Cells = new ICell[level.row, level.column];
            _gameModel.FieldModel.SetActiveCellsCount();
            _gameModel.FieldModel.FieldSize = new FieldSize(level.row, level.column);
            _gameModel.Level = level;
            _signalBus.Fire(new NextStateSignal(typeof(CreateFieldState)));
        }
        
    }
}