using System;
using CodeBase.CoreBase.Configs;
using CodeBase.CoreBase.Counter;
using UnityEngine;

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

        public Transform counterTr { get; set; }
        public ItemType AimItemType => _levelAimItemType;
        public bool IsComplete => _complete;
        public event Action OnAimChanged;

        public int Value
        {
            get => _levelAimAmount;
            set
            {
                _levelAimAmount += value;
                if (_levelAimAmount <= 0 && !_complete)
                {
                    _levelAimAmount = 0;
                    _complete = true;
                    OnAimChanged?.Invoke();
                }
                OnUpdateCounterEvent?.Invoke();
            }
        }
    }
    
    public interface IAimSetter : ICounter
    {
        ItemType AimItemType { get;}
        bool IsComplete { get; }

        event Action OnAimChanged;
    }
}