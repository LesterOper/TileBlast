using System;

namespace CodeBase.CoreBase.Counter
{
    public interface ICounter
    {
        int Value { get; set; }
        event Action OnUpdateCounterEvent;
    }
}