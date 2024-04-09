using System;
using CodeBase.CoreBase.Configs;
using CodeBase.CoreBase.Counter;

namespace CodeBase.CoreBase.AimTopBar
{
    public class Aim : IAimSetter
    {
        private int _levelAimAmount;
        private ItemType _levelAimItemType;
        private bool _complete;

        public event Action OnUpdateCounterEvent;

        public Aim(ItemType itemType, int amount)
        {
            _levelAimAmount = amount;
            _levelAimItemType = itemType;
        }

        public ItemType AimItemType => _levelAimItemType;
        public bool IsComplete => _complete;
        public int Value
        {
            get => _levelAimAmount;
            set
            {
                _levelAimAmount += value;
                if (_levelAimAmount <= 0)
                {
                    _levelAimAmount = 0;
                    _complete = true;
                }
                OnUpdateCounterEvent?.Invoke();
            }
        }
    }
    
    public interface IAimSetter : ICounter
    {
        ItemType AimItemType { get;}
        bool IsComplete { get; }
    }
}