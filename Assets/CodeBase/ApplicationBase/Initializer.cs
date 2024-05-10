using System;
using CodeBase.ApplicationBase.SavingBase;
using CodeBase.CoreBase;
using CodeBase.CoreBase.Configs;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;
using Zenject;

namespace CodeBase.ApplicationBase
{
    public class Initializer : IInitializable, IDisposable
    {
        private const string FIRST_LAUNCH = nameof(FIRST_LAUNCH);
        private GlobalGameStateModel _globalGameStateModel;
        private GlobalGameSettingsConfig _gameSettingsConfig;
        private CommonSaving _commonSaving;
        private TimeModel _timeModel;

        public Initializer(
            GlobalGameStateModel gameStateModel, 
            GlobalGameSettingsConfig gameSettingsConfig,
            CommonSaving commonSaving,
            TimeModel timeModel)
        {
            _globalGameStateModel = gameStateModel;
            _gameSettingsConfig = gameSettingsConfig;
            _commonSaving = commonSaving;
            _timeModel = timeModel;
        }
            
        public async void Initialize()
        {
            Application.targetFrameRate = 60;
            var success = await _timeModel.TryToRefreshServerTime();
            StandardPurchasingModule.Instance().useFakeStoreAlways = true;
            if (PlayerPrefs.GetInt(FIRST_LAUNCH) == 0)
            {
                _globalGameStateModel.HealthSetter.MaxHealthAmount = _gameSettingsConfig.MaxHealthAmount;
                _globalGameStateModel.HealthSetter.ResetDuration =
                    _gameSettingsConfig.ResetDurationOfOneHealthInSeconds;
                _globalGameStateModel.CurrencySetter.Value = _gameSettingsConfig.DefaultSoftMoneyAmount;
                _globalGameStateModel.HealthSetter.Value = _gameSettingsConfig.HealthAmountOnGameStartedFirstTime;
                _globalGameStateModel.CurrentLevel = 1;
                PlayerPrefs.SetInt(FIRST_LAUNCH, 1);
            }
            else
            {
                _globalGameStateModel.HealthSetter.MaxHealthAmount = _gameSettingsConfig.MaxHealthAmount;
                _globalGameStateModel.HealthSetter.ResetDuration =
                    _gameSettingsConfig.ResetDurationOfOneHealthInSeconds;
                _commonSaving.Load();
            }

            if (success)
                SceneManager.LoadScene(1);
        }

        public void Dispose()
        {
            _commonSaving.Save();
        }
    }
}