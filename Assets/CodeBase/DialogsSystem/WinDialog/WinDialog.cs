using System;
using CodeBase.CoreBase;
using CodeBase.CoreBase.Configs;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CodeBase.DialogsSystem.WinDialog
{
    public class WinDialog : DialogBase
    {

        [SerializeField] private TextMeshProUGUI softMoney;
        [SerializeField] private TextMeshProUGUI starsAmount;
        [SerializeField] private TextMeshProUGUI level;
        [SerializeField] private Transform shine;

        private GlobalGameStateModel _globalGameStateModel;
        private GlobalGameSettingsConfig _globalGameSettingsConfig;

        private Tween shineRotation;

        [Inject]
        private void Construct(GlobalGameStateModel globalGameStateModel, GlobalGameSettingsConfig gameSettingsConfig)
        {
            _globalGameStateModel = globalGameStateModel;
            _globalGameSettingsConfig = gameSettingsConfig;
        }

        public override void Initialize()
        {
            softMoney.text = $"+{_globalGameSettingsConfig.softMoneyPrizeForCompleteLevel}";
            starsAmount.text = $"+{_globalGameSettingsConfig.starsPrizeForCompleteLevel}";
            level.text = $"LEVEL {_globalGameStateModel.CurrentLevel}";
            shineRotation = shine.DORotate(new Vector3(0f, 0f, 360f), 10f, RotateMode.FastBeyond360).SetLoops(-1);
            base.Initialize();
        }

        protected override void Confirm()
        {
            base.Confirm();
            _globalGameStateModel.CurrencySetter.Value = _globalGameSettingsConfig.softMoneyPrizeForCompleteLevel;
            _globalGameStateModel.Stars = _globalGameSettingsConfig.starsPrizeForCompleteLevel;
            SceneManager.LoadScene(1);
        }

        private void OnDisable()
        {
            shineRotation.Kill();
        }
    }
}