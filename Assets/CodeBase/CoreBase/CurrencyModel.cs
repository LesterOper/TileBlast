using System;
using CodeBase.CoreBase.Counter;
using UnityEngine;

namespace CodeBase.CoreBase
{
    public class CurrencyModel : ICurrencySetter
    {
        private int _softMoneyValue;

        public int Value
        {
            get => _softMoneyValue;
            set
            {
                _softMoneyValue += value;
                OnUpdateCounterEvent?.Invoke();
            }
        }

        public event Action OnUpdateCounterEvent;
        public Transform counterTr { get; set; }
    }

    public interface ICurrencySetter : ICounter { }
    
}