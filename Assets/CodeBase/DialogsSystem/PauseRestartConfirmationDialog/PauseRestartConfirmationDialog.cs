using CodeBase.ApplicationBase;
using CodeBase.CoreBase;
using CodeBase.CoreBase.GameStateModel;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CodeBase.DialogsSystem.PauseRestartConfirmationDialog
{
    public class PauseRestartConfirmationDialog : DialogBase
    {
        private string RESTART = "Are you sure you want <b>to restart this level</b> and <b>lose 1 life?</b>";
        private string RESTART_BACK_HOME = "Are you sure you want <b>to go back home</b> and <b>lose 1 life?</b>";
        private const string ButtonTextRestart = "Restart";
        private const string ButtonTextHome = "Back home";
        [SerializeField] private TextMeshProUGUI infoTextField;
        [SerializeField] private TextMeshProUGUI buttonTextField;
        private GlobalGameStateModel _globalGameStateModel;
        private TimeModel _timeModel;

        private bool _backHome;

        [Inject]
        private void Construct( GlobalGameStateModel globalGameStateModel, TimeModel timeModel)
        {
            _globalGameStateModel = globalGameStateModel;
            _timeModel = timeModel;
        }

        public override void InitializePauseRestart(bool backHome)
        {
            _backHome = backHome;
            infoTextField.text = backHome ? RESTART_BACK_HOME : RESTART;
            buttonTextField.text = backHome ? ButtonTextHome : ButtonTextRestart;
            base.InitializePauseRestart(backHome);
        }

        protected override void Confirm()
        {
            _globalGameStateModel.HealthSetter.Value = -1;
            _globalGameStateModel.UpdateRecoveryData(_timeModel.GetUnixTimeSeconds());

            SceneManager.LoadScene(_backHome ? 1 : 2);
            base.Confirm();
        }

        protected override void Decline()
        {
            base.Decline();
        }
    }
}