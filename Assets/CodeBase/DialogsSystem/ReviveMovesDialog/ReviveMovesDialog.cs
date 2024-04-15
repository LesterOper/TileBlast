using CodeBase.CoreBase;
using CodeBase.CoreBase.Configs;
using CodeBase.CoreBase.GameStateModel;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.DialogsSystem.ReviveMovesDialog
{
    public class ReviveMovesDialog : DialogBase
    {
        [SerializeField] private TextMeshProUGUI reviveCostText;
        [SerializeField] private TextMeshProUGUI reviveMovesAmountText;
        [SerializeField] private TextMeshProUGUI reviveCostTextInInfo;

        private GlobalGameSettingsConfig _globalGameSettingsConfig;
        private GameModel _gameModel;
        private GlobalGameStateModel _globalGameStateModel;

        [Inject]
        private void Construct(
            GlobalGameSettingsConfig gameSettingsConfig,
            GlobalGameStateModel globalGameStateModel,
            GameModel gameModel)
        {
            _globalGameSettingsConfig = gameSettingsConfig;
            _globalGameStateModel = globalGameStateModel;
            _gameModel = gameModel;
        }

        public override void Initialize()
        {
            base.Initialize();
            reviveCostText.text = $"{_globalGameSettingsConfig.MovesReviveCost}";
            reviveMovesAmountText.text = $"+{_globalGameSettingsConfig.MovesReviveAmount}";
            reviveCostTextInInfo.text = $"{_globalGameSettingsConfig.MovesReviveCost}";

            if (_globalGameStateModel.CurrencySetter.Value < _globalGameSettingsConfig.MovesReviveCost)
                confirm.interactable = false;
        }

        protected override void Confirm()
        {
            base.Confirm();
            _gameModel.GameplayCountersModelProperty.MovesModelProperty.Value +=
                _globalGameSettingsConfig.MovesReviveAmount;
            CloseDialog();
        }

        protected override void Decline()
        {
            base.Decline();
            CloseDialog();
            _gameModel.DialogsController.ShowLooseDialog();
        }
    }
}