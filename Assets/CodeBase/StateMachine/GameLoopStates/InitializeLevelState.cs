using CodeBase.CoreBase;
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
        private GameEndingChecker _gameEndingChecker;
        private GlobalGameStateModel _globalGameStateModel;

        public InitializeLevelState(SignalBus signalBus,
            LevelsConfig levelsConfig,
            GameModel gameModel,
            GameEndingChecker gameEndingChecker,
            GlobalGameStateModel globalGameStateModel) : base(signalBus)
        {
            _levelsConfig = levelsConfig;
            _gameModel = gameModel;
            _gameEndingChecker = gameEndingChecker;
            _globalGameStateModel = globalGameStateModel;
        }

        public override void Execute()
        {
            base.Execute();
            
            var level = _levelsConfig.GetLevel(_globalGameStateModel.CurrentLevel);
            _gameModel.GameplayCountersModelProperty.AimModelSetterProperty.SetLevelAims(level.levelData.LevelAims);
            _gameEndingChecker.SubscribeAimSetters();
            _gameModel.GameplayCountersModelProperty.MovesModelProperty.Value = (int) level.levelData.moves;
            _gameModel.FieldModel.Cells = new IFieldCell[level.levelFieldConfig.row, level.levelFieldConfig.columns];
            _gameModel.FieldModel.FieldSize = new FieldSize(level.levelFieldConfig.row, level.levelFieldConfig.columns);
            _gameModel.Level = level;
            _signalBus.Fire(new NextStateSignal(typeof(CreateFieldState)));
        }
        
    }
}