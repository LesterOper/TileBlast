using CodeBase.ApplicationBase;
using CodeBase.CoreBase;
using CodeBase.CoreBase.GameStateModel;
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
        [SerializeField] private HealthRecoveryTimerView timerInCounter;
        private GlobalGameStateModel _globalGameStateModel;
        private GameModel _gameModel;
        private TimeModel _timeModel;

        [Inject]
        private void Construct(GlobalGameStateModel globalGameStateModel, TimeModel timeModel, GameModel gameModel)
        {
            _globalGameStateModel = globalGameStateModel;
            _gameModel = gameModel;
            _timeModel = timeModel;
        }
        
        public override void Initialize()
        {
            levelField.text = $"LEVEL {_globalGameStateModel.CurrentLevel}";
            _globalGameStateModel.HealthSetter.Value = -1;
            _globalGameStateModel.UpdateRecoveryData(_timeModel.GetUnixTimeSeconds());
            timerInCounter.Reinitialize();
            base.Initialize();
        }

        protected override void Confirm()
        {
            base.Confirm();
            if (!_globalGameStateModel.HealthSetter.HasHealth)
            {
                Hide();
                _gameModel.DialogsController.ShowReviveHealthDialog();
            }
            else
            {
                CloseDialog();
                SceneManager.LoadScene(2);
            }
        }

        public override void Show()
        {
            timer.Reinitialize();
            base.Show();
        }

        protected override void Decline()
        {
            base.Decline();
            SceneManager.LoadScene(1);
        }
    }
}