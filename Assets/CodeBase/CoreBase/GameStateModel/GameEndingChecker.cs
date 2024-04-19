using CodeBase.CoreBase.Input;

namespace CodeBase.CoreBase.GameStateModel
{
    public class GameEndingChecker
    {
        private GameModel _gameModel;
        private IInputSystem _inputSystem;

        public GameEndingChecker(GameModel gameModel, IInputSystem inputSystem)
        {
            _gameModel = gameModel;
            _inputSystem = inputSystem;
        }

        public void SubscribeAimSetters()
        {
            _gameModel.GameplayCountersModelProperty.AimModelSetterProperty.Aims.ForEach(local => local.OnAimChanged += CheckGameEnding);
        }
        
        public void CheckGameEnding()
        {
            if (_gameModel.GameplayCountersModelProperty.MovesModelProperty.Value <= 0)
            {
                if (PassedLevel()) return;
                _gameModel.DialogsController.ShowReviveMovesDialog();
                _inputSystem.SetEnable(false);
            }
            else
            {
                PassedLevel();
            }
        }

        private bool PassedLevel()
        {
            if (_gameModel.GameplayCountersModelProperty.AimModelSetterProperty.IsLevelPassed())
            {
                _gameModel.DialogsController.ShowWinDialog();
                _inputSystem.SetEnable(false);
                return true;
            }

            return false;
        }
    }
}