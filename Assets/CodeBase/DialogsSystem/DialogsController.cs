using System.Collections.Generic;
using CodeBase.DialogsSystem.ReviveHealth;

namespace CodeBase.DialogsSystem
{
    public class DialogsController
    {
        private WinDialog.WinDialog _winDialog;
        private LooseDialog.LooseDialog _looseDialog;
        private ReviveMovesDialog.ReviveMovesDialog _reviveMovesDialog;
        private ReviveHealthDialog _reviveHealthDialog;
        private PauseDialog.PauseDialog _pauseDialog;
        private PauseRestartConfirmationDialog.PauseRestartConfirmationDialog _pauseRestartConfirmationDialog;

        private Stack<DialogBase> _activeDialogs;

        private DialogBase _activeDialog;

        public DialogsController(
            WinDialog.WinDialog winDialog,
            LooseDialog.LooseDialog looseDialog,
            ReviveMovesDialog.ReviveMovesDialog reviveMovesDialog,
            ReviveHealthDialog reviveHealthDialog,
            PauseDialog.PauseDialog pauseDialog,
            PauseRestartConfirmationDialog.PauseRestartConfirmationDialog pauseRestartConfirmationDialog)
        {
            _winDialog = winDialog;
            _looseDialog = looseDialog;
            _reviveMovesDialog = reviveMovesDialog;
            _reviveHealthDialog = reviveHealthDialog;
            _pauseDialog = pauseDialog;
            _pauseRestartConfirmationDialog = pauseRestartConfirmationDialog;
        }

        public void ShowWinDialog()
        {
            if(_activeDialog != null)
                _activeDialog.CloseDialog();
            _activeDialog = _winDialog;
            ShowDialog();
        }

        public void ShowReviveMovesDialog()
        {
            _activeDialog = _reviveMovesDialog;
            ShowDialog();
        }

        public void ShowLooseDialog()
        {
            _activeDialog = _looseDialog;
            ShowDialog();
        }

        public void ShowReviveHealthDialog()
        {
            _activeDialog = _reviveHealthDialog;
            ShowDialog();
        }

        public void ShowPauseDialog()
        {
            _activeDialog = _pauseDialog;
            ShowDialog();
        }

        public void ShowPauseRestartConfirmationDialog()
        {
            _activeDialog = _pauseRestartConfirmationDialog;
            ShowDialog();
        }

        private void ShowDialog()
        {
            _activeDialog.OnDialogClosedEvent += ActiveDialogClosed;
            _activeDialog.OnDialogHideEvent += HideDialog;
            _activeDialog.Initialize();
            _activeDialogs ??= new Stack<DialogBase>();
            _activeDialogs.Push(_activeDialog);
        }

        private void ActiveDialogClosed()
        {
            if (_activeDialogs.Contains(_activeDialog))
            {
                _activeDialogs.Pop();
                _activeDialog.OnDialogClosedEvent -= ActiveDialogClosed;
                _activeDialog.OnDialogHideEvent -= HideDialog;
            }

            if (_activeDialogs.TryPop(out DialogBase dialog))
            {
                _activeDialog = dialog;
                _activeDialogs.Push(_activeDialog);
                ShowDialogFromQueue();
            }
        }

        private void ShowDialogFromQueue()
        {
            _activeDialog.Show();
        }

        private void HideDialog()
        {
        }
    }
}