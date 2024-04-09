using CodeBase.ApplicationBase;
using CodeBase.CoreBase;
using CodeBase.MetaBase.Views;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CodeBase.DialogsSystem.LooseDialog
{
    public class LooseDialog : DialogBase
    {
        [SerializeField] private TextMeshProUGUI levelField;
        [SerializeField] private HealthRecoveryTimerView timer;
        private GlobalGameStateModel _globalGameStateModel;
        private TimeModel _timeModel;

        [Inject]
        private void Construct(GlobalGameStateModel globalGameStateModel, TimeModel timeModel)
        {
            _globalGameStateModel = globalGameStateModel;
            _timeModel = timeModel;
        }
        
        public override void Initialize()
        {
            levelField.text = $"LEVEL {_globalGameStateModel.CurrentLevel}";
            _globalGameStateModel.HealthSetter.Value = -1;
            _globalGameStateModel.UpdateRecoveryData(_timeModel.GetUnixTimeSeconds());
            if (!_globalGameStateModel.HealthSetter.HasHealth)
                confirm.interactable = false;
            base.Initialize();
        }

        protected override void Confirm()
        {
            base.Confirm();
            SceneManager.LoadScene(2);
        }

        protected override void Decline()
        {
            base.Decline();
            SceneManager.LoadScene(1);
        }
    }
}