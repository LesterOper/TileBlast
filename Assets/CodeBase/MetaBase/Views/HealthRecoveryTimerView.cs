using CodeBase.ApplicationBase;
using CodeBase.ApplicationBase.SavingBase;
using CodeBase.CoreBase;
using CodeBase.MetaBase.MetaControllers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityTimer;
using Zenject;

namespace CodeBase.MetaBase.Views
{
    public class HealthRecoveryTimerView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timerField;

        private GlobalGameStateModel _globalGameStateModel;
        private TimeModel _timeModel;
        private Timer _timer;

        private TimerChecker _timerChecker;

        [Inject]
        private void Construct(GlobalGameStateModel globalGameStateModel, TimeModel timeModel)
        {
            _globalGameStateModel = globalGameStateModel;
            _timeModel = timeModel;
        }
        
        private void Start()
        {
            _timerChecker = new TimerChecker(_globalGameStateModel, _timeModel);
            TryToRecoverHealth();
        }
        
        private void TryToRecoverHealth()
        {
            (long duration, bool needRecover) = _timerChecker.TryToRecoverHealth();
            if(needRecover)
                StartRecovery(duration);
            else
            {
                if (_timer != null)
                {
                    _timer.Cancel();
                    _timer = null;
                    timerField.text = "Full";
                }
            }
        }

        private void StartRecovery(long duration)
        {
            if (_timer == null)
            {
                _timer = Timer.Register(duration, AddHealth, UpdateTimerField);
            }
        }

        private void AddHealth()
        {
            _timer = null;
            TryToRecoverHealth();
        }

        private void UpdateTimerField(float value)
        {
            float remainingSeconds = _timer.GetTimeRemaining();
            int minutes = (int)remainingSeconds / 60;
            int hours = minutes / 60;
            int seconds = (int)remainingSeconds - minutes * 60;

            timerField.text = $"{hours}h {minutes}m {seconds}s";
        }

        public void StopTimer()
        {
            if (_timer != null)
            {
                _timer.Cancel();
            }

            timerField.text = $"0h 0m 0s";
            TryToRecoverHealth();
        }

        public void Reinitialize()
        {
            TryToRecoverHealth();
        }
        
        private void OnDestroy()
        {
            //_globalGameStateModel.HealthSetter.OnTimerEvent -= TryToRecoverHealth;
        }
    }
}