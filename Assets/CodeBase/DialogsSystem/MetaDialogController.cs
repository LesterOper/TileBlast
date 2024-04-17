using System.Collections.Generic;
using CodeBase.DialogsSystem.BuyLivesDialog;
using CodeBase.DialogsSystem.Settings;

namespace CodeBase.DialogsSystem
{
    public class MetaDialogController
    {
        private BuyMoreLivesDialog _buyMoreLivesDialog;
        private SettingsDialog _settingsDialog;
        
        private Stack<DialogBase> _activeDialogs;

        private DialogBase _activeDialog;

        public MetaDialogController(BuyMoreLivesDialog buyMoreLivesDialog, SettingsDialog settingsDialog)
        {
            _buyMoreLivesDialog = buyMoreLivesDialog;
            _settingsDialog = settingsDialog;
        }

        public void ShowBuyMoreLivesDialog()
        {
            _activeDialog = _buyMoreLivesDialog;
            ShowDialog();
        }
        
        public void ShowSettingsDialog()
        {
            _activeDialog = _settingsDialog;
            ShowDialog();
        }
        
        private void SubscribeActiveDialogEvent()
        {
            _activeDialog.OnDialogClosedEvent += ActiveDialogClosed;
            _activeDialog.OnDialogHideEvent += HideDialog;
        }
        
        private void ShowDialog()
        {
            SubscribeActiveDialogEvent();
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