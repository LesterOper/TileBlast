using CodeBase.CoreBase;
using CodeBase.CoreBase.Configs;
using CodeBase.MetaBase.Views;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.DialogsSystem.BuyLivesDialog
{
    public class BuyMoreLivesDialog : DialogBase
    {
        [SerializeField] private TextMeshProUGUI costTextField;

        private GlobalGameStateModel _globalGameStateModel;
        private GlobalGameSettingsConfig _globalGameSettingsConfig;
        private HealthRecoveryTimerView _healthRecoveryTimerView;

        [Inject]
        private void Construct(
            GlobalGameStateModel globalGameStateModel,
            GlobalGameSettingsConfig gameSettingsConfig, 
            HealthRecoveryTimerView healthRecoveryTimerView)
        {
            _globalGameStateModel = globalGameStateModel;
            _globalGameSettingsConfig = gameSettingsConfig;
            _healthRecoveryTimerView = healthRecoveryTimerView;
        }

        public override void Initialize()
        {
            costTextField.text = _globalGameSettingsConfig.HealthReviveCost.ToString();

            if (_globalGameStateModel.CurrencySetter.Value < _globalGameSettingsConfig.HealthReviveCost)
            {
                confirm.interactable = false;
            }
            base.Initialize();
        }

        protected override void Confirm()
        {
            base.Confirm();

            _globalGameStateModel.HealthRecovery.Clear();
            _healthRecoveryTimerView.Reinitialize();
            _globalGameStateModel.HealthSetter.Value = _globalGameSettingsConfig.HealthReviveAmount;
            _globalGameStateModel.CurrencySetter.Value = -_globalGameSettingsConfig.HealthReviveCost;
            CloseDialog();
        }
    }
}