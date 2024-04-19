using System;
using CodeBase.CoreBase.Counter;
using UnityEngine;

namespace CodeBase.CoreBase.GameStateModel
{
    public class MovesModel : IMovesSetter
    {
        private int _moves;
        
        public int Value { 
            get => _moves;
            set
            {
                _moves += value;
                if (_moves <= 0)
                {
                    _moves = 0;
                    OnAmountOfMovesEqualsZeroEvent?.Invoke();
                }
                OnUpdateCounterEvent?.Invoke();
            } 
        }
        public event Action OnUpdateCounterEvent;
        public Transform counterTr { get; set; }
        public event Action OnAmountOfMovesEqualsZeroEvent;
    }

    public interface IMovesSetter : ICounter
    {
        event Action OnAmountOfMovesEqualsZeroEvent;
    }
}