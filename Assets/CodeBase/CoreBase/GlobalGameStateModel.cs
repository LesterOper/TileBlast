using System.Collections.Generic;

namespace CodeBase.CoreBase
{
    public class GlobalGameStateModel
    {
        private ICurrencySetter _currencySetter;
        private IHealthSetter _healthSetter;

        private List<Recovery> _healthRecovery;
        private int _currentLevel;
        
        public GlobalGameStateModel(
            ICurrencySetter currencySetter,
            IHealthSetter healthSetter)
        {
            _currencySetter = currencySetter;
            _healthSetter = healthSetter;
            _healthRecovery = new List<Recovery>(_healthSetter.MaxHealthAmount);
        }

        public ICurrencySetter CurrencySetter => _currencySetter;
        public IHealthSetter HealthSetter => _healthSetter;

        public List<Recovery> HealthRecovery
        {
            get => _healthRecovery;
            set => _healthRecovery.AddRange(value);
        }

        public void UpdateRecoveryData(long time)
        {
            Recovery recovery = new Recovery();

            if (_healthRecovery.Count == 0)
            {
                recovery.EndTime = time + (long)_healthSetter.ResetDuration;
                recovery.NeedToRecover = true;
            }
            else
            {
                recovery.EndTime = _healthRecovery[^1].EndTime + (long) _healthSetter.ResetDuration;
                recovery.NeedToRecover = true;
            }
            
            _healthRecovery.Add(recovery);
            
        }

        public bool IsNeedToRecoverHealth() => _healthRecovery.Count > 0;

        public int CurrentLevel
        {
            get => _currentLevel;
            set => _currentLevel = value;
        }
    }

    public class Recovery
    {
        public long EndTime = 0;
        public bool NeedToRecover;
    }
}