using CodeBase.CoreBase;
using CodeBase.CoreBase.Configs;
using CodeBase.MetaBase.Views;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CodeBase.DialogsSystem.ReviveHealth
{
    public class ReviveHealthDialog : DialogBase
    {
        [SerializeField] private TextMeshProUGUI currentHealthAmountText;
        [SerializeField] private TextMeshProUGUI reviveHealthAmountText;
        [SerializeField] private TextMeshProUGUI reviveCost;
        [SerializeField] private HealthRecoveryTimerView _healthRecoveryTimerView;
        
        private GlobalGameStateModel _globalGameStateModel;
        private GlobalGameSettingsConfig _globalGameSettingsConfig;

        [Inject]
        private void Construct(GlobalGameStateModel globalGameStateModel, GlobalGameSettingsConfig globalGameSettingsConfig)
        {
            _globalGameStateModel = globalGameStateModel;
            _globalGameSettingsConfig = globalGameSettingsConfig;
        }

        public override void Initialize()
        {
            currentHealthAmountText.text = $"{_globalGameStateModel.HealthSetter.Value}";
            reviveHealthAmountText.text = $"+{_globalGameSettingsConfig.HealthReviveAmount}";
            reviveCost.text = $"{_globalGameSettingsConfig.HealthReviveCost}";

            if (_globalGameStateModel.CurrencySetter.Value < _globalGameSettingsConfig.HealthReviveCost)
                confirm.interactable = false;
            base.Initialize();
        }

        protected override void Confirm()
        {
            base.Confirm();
            _globalGameStateModel.CurrencySetter.Value = -_globalGameSettingsConfig.HealthReviveCost;
            _globalGameStateModel.HealthSetter.Value = _globalGameSettingsConfig.HealthReviveAmount;
            _globalGameStateModel.HealthRecovery.Clear();
            _healthRecoveryTimerView.StopTimer();
            CloseDialog();
        }

        protected override void Decline()
        {
            base.Decline();
            CloseDialog();
        }
    }
}