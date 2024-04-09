using System;
using CodeBase.CoreBase.Counter;

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
    }

    public interface ICurrencySetter : ICounter { }
    
}