namespace CodeBase.CoreBase.GameStateModel
{
    public class GameEndingChecker
    {
        private GameModel _gameModel;

        public GameEndingChecker(GameModel gameModel)
        {
            _gameModel = gameModel;
        }

        public void CheckGameEnding()
        {
            if (_gameModel.GameplayCountersModelProperty.MovesModelProperty.Value <= 0)
            {
                if (PassedLevel()) return;
                _gameModel.DialogsController.ShowLooseDialog();
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
                return true;
            }

            return false;
        }
    }
}