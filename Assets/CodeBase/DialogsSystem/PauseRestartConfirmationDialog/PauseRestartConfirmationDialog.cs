using CodeBase.CoreBase.GameStateModel;
using Zenject;

namespace CodeBase.DialogsSystem.PauseRestartConfirmationDialog
{
    public class PauseRestartConfirmationDialog : DialogBase
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
        }

        protected override void Decline()
        {
            base.Decline();
        }
    }
}