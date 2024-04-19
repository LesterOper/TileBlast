using CodeBase.CoreBase.GameStateModel;
using Zenject;

namespace CodeBase.DialogsSystem.PauseDialog
{
    public class PauseDialog : DialogBase
    {
        private GameModel _gameModel;

        [Inject]
        private void Construct(GameModel gameModel)
        {
            _gameModel = gameModel;
        }

        protected override void Confirm()
        {
            base.Confirm();
            _gameModel.DialogsController.ShowPauseRestartConfirmationDialog(false);
        }

        protected override void Decline()
        {
            base.Decline();
            _gameModel.DialogsController.ShowPauseRestartConfirmationDialog(true);
        }
    }
}