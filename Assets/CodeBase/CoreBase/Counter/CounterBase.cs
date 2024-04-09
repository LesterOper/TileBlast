using CodeBase.CoreBase.Counter;
using TMPro;
using UnityEngine;

public abstract class CounterBase : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI _counterField;
    
    protected ICounter _counterSetter;

    protected virtual void Start()
    {
        if (_counterSetter != null)
        {
            UpdateCounter();
            _counterSetter.OnUpdateCounterEvent += UpdateCounter;
        }
    }

    public virtual void UpdateCounter()
    {
        _counterField.text = _counterSetter.Value.ToString();
    }

    public virtual void SetCounterSetter(ICounter counter)
    {
        _counterSetter = counter;
        UpdateCounter();
    }
}
