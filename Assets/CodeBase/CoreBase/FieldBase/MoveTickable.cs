using System;
using System.Collections;
using System.Collections.Generic;
using CodeBase.CoreBase.CellBase;
using CodeBase.CoreBase.ItemBase;
using CodeBase.StateMachine;
using CodeBase.StateMachine.GameLoopStates;
using UnityEngine;
using Zenject;

namespace CodeBase.CoreBase.FieldBase
{
    public class MoveTickable
    {
        private MovableItemsContainer _movableItemsContainer = new();
        [Inject]private CoroutineRunner _coroutineRunner;
        [Inject] private SignalBus _signalBus;

        private Coroutine _coroutine;

        public void AddMovableItems(List<MovableItemsContainer.MovableItem> movableItems)
        {
            if (_coroutine != null)
            {
                _coroutineRunner.StopCoroutine(_coroutine);
                _coroutine = null;
            }
            _movableItemsContainer.ClearCompleteAnimationItems();
            _movableItemsContainer.AddItems(movableItems);
            StartMoving();
        }

        private void StartMoving()
        {
            _coroutine = _coroutineRunner.StartCoroutine(_movableItemsContainer.MoveItems(()=> _signalBus.Fire(new NextStateSignal(typeof(UserClickState)))));
        }
    }

    public class MovableItemsContainer
    {
        public class MovableItem
        {
            private CellView _cellView;
            private Vector2 _targetPosition;
            private int layer;
            public  bool complete;

            public MovableItem(CellView cellView, Vector2 targetPosition)
            {
                _cellView = cellView;
                _targetPosition = targetPosition;
            }
            
            public void MoveToTarget()
            {
                _cellView.MoveToTargetCell(_targetPosition, ()=>
                {
                    complete = true;
                });
            }
        }

        private List<MovableItem> _movableItems = new(10);

        public void AddItems(List<MovableItem> items)
        {
            _movableItems.AddRange(items);
        }

        public void ClearCompleteAnimationItems()
        {
            _movableItems.RemoveAll(local => local.complete);
        }
        
        private void ClearList()
        {
            _movableItems.Clear();
        }

        private WaitForSeconds _waitForSeconds = new(0.10f * 0.03f);
        public IEnumerator MoveItems(Action callback)
        {
            for (int i = 0; i < _movableItems.Count; i++)
            {
                _movableItems[i].MoveToTarget();
                yield return _waitForSeconds;
            }
            ClearList();
            callback.Invoke();
        }
    }
}