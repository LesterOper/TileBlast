using CodeBase.CoreBase;
using Zenject;

namespace CodeBase.MetaBase.Views
{
    public class SoftMoneyCounter : CounterBase
    {
        [Inject]
        private void Construct(ICurrencySetter currencySetter)
        {
            _counterSetter = currencySetter;
        }
    }
}