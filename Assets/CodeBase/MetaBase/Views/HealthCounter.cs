using CodeBase.CoreBase;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.MetaBase.Views
{
    public class HealthCounter : CounterBase
    {
        [SerializeField] private TextMeshProUGUI infoField;

        [Inject]
        private void Construct(IHealthSetter healthSetter)
        {
            _counterSetter = healthSetter;
        }

        public override void UpdateCounter()
        {
            base.UpdateCounter();
            if(_counterSetter.Value == 5)
                infoField.text = "Full";
        }
    }
}