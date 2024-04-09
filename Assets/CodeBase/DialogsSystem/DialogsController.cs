namespace CodeBase.DialogsSystem
{
    public class DialogsController
    {
        private WinDialog.WinDialog _winDialog;
        private LooseDialog.LooseDialog _looseDialog;

        private DialogBase _activeDialog;

        public DialogsController(
            WinDialog.WinDialog winDialog,
            LooseDialog.LooseDialog looseDialog)
        {
            _winDialog = winDialog;
            _looseDialog = looseDialog;
            _winDialog.OnDialogClosedEvent += ActiveDialogClosed;
            _looseDialog.OnDialogClosedEvent += ActiveDialogClosed;
        }

        public void ShowWinDialog()
        {
            if(_activeDialog != null)
                _activeDialog.CloseDialog();
            _activeDialog = _winDialog;
            ShowDialog();
        }

        public void ShowLooseDialog()
        {
            if(_activeDialog != null)
                _activeDialog.CloseDialog();
            _activeDialog = _looseDialog;
            ShowDialog();
        }

        private void ShowDialog() => _activeDialog.Initialize();

        private void ActiveDialogClosed()
        {
            _activeDialog = null;
        }
    }
}