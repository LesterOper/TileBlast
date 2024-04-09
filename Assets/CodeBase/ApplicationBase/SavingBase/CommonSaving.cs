using System.Collections.Generic;
using CodeBase.CoreBase;
using UnityEngine;

namespace CodeBase.ApplicationBase.SavingBase
{
    public class CommonSaving
    {
        private GlobalGameStateModel _globalGameStateModel;

        public CommonSaving(GlobalGameStateModel globalGameState)
        {
            _globalGameStateModel = globalGameState;
        }

        public void Save()
        {
            int softMoney = _globalGameStateModel.CurrencySetter.Value;
            int health = _globalGameStateModel.HealthSetter.Value;
            int currentLevel = _globalGameStateModel.CurrentLevel;
            List<Recovery> recoveries = _globalGameStateModel.HealthRecovery;
            SavingData savingData = new SavingData(softMoney, health, currentLevel, recoveries);
            Debug.Log(savingData.ToString());
            ES3.Save("Profile", savingData);
        }

        public void Load()
        {
            SavingData savingData = ES3.Load<SavingData>("Profile");
            _globalGameStateModel.CurrencySetter.Value = savingData._softMoney;
            _globalGameStateModel.HealthSetter.Value = savingData._health;
            _globalGameStateModel.HealthRecovery = savingData._recoveryHealthData;
            _globalGameStateModel.CurrentLevel = savingData._currentLevel;
            Debug.Log(savingData.ToString());
        }
    }
}