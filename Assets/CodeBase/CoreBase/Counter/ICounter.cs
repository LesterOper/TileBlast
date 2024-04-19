using System;
using UnityEngine;

namespace CodeBase.CoreBase.Counter
{
    public interface ICounter
    {
        int Value { get; set; }
        event Action OnUpdateCounterEvent;
        Transform counterTr { get; set; }

    }
}