using System;
using CodeBase.CoreBase.Counter;

namespace CodeBase.CoreBase
{
    public class HealthModel : IHealthSetter
    {
        private float _resetDuration;
        private int _maxHealthAmount;
        private int _healthValue;
        private bool _hasHealth;
        
        public int Value
        {
            get => _healthValue;
            set
            {
                _healthValue += value;
                if (_healthValue > _maxHealthAmount)
                    _healthValue = _maxHealthAmount;
                if (_healthValue <= 0)
                {
                    _healthValue = 0;
                    _hasHealth = false;
                }
                else _hasHealth = true;

                if(_healthValue < _maxHealthAmount)
                    OnTimerEvent?.Invoke();
                
                OnUpdateCounterEvent?.Invoke();
            }
        }
        public event Action OnUpdateCounterEvent;

        public int MaxHealthAmount
        {
            get => _maxHealthAmount;
            set => _maxHealthAmount = value;
        }

        public float ResetDuration
        {
            get => _resetDuration;
            set => _resetDuration = value;
        }
        
        public event Action OnTimerEvent;
        public bool HasHealth => _hasHealth;
    }

    public interface IHealthSetter : ICounter
    {
        int MaxHealthAmount { get; set; }
        float ResetDuration { get; set; }

        event Action OnTimerEvent;
        bool HasHealth { get;}
    }
}