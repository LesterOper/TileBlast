using CodeBase.ApplicationBase.AudioManager;
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
            AudioManager.Inst.PlayButton();
            _gameModel.DialogsController.ShowPauseRestartConfirmationDialog(false);
        }

        protected override void Decline()
        {
            base.Decline();
            AudioManager.Inst.PlayButton();
            _gameModel.DialogsController.ShowPauseRestartConfirmationDialog(true);
        }
    }
}