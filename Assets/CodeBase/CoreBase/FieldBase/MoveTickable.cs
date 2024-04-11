using System;
using System.Collections.Generic;
using CodeBase.CoreBase.CellBase;
using CodeBase.CoreBase.ItemBase;
using UnityEngine;
using Zenject;

namespace CodeBase.CoreBase.FieldBase
{
    public class MoveTickable : ITickable
    {
        private MovableItemsContainer _movableItemsContainer = new();
        
        public void AddMovableItems(List<MovableItemsContainer.MovableItem> movableItems) =>
            _movableItemsContainer.AddItems(movableItems);
        
        public void Tick() => _movableItemsContainer.MoveItems();
    }

    public class MovableItemsContainer
    {
        public class MovableItem
        {
            private CellView _cellView;
            private Vector2 _targetPosition;
            private ICell _cell;
            private ICell _currentCell;

            public MovableItem(CellView cellView, Vector2 targetPosition)
            {
                _cellView = cellView;
                _targetPosition = targetPosition;
            }
            
            public void MoveToTarget()
            {
                _cellView.MoveToTargetCell(_targetPosition, ()=>
                {
                    
                });
            }
        }

        private List<MovableItem> _movableItems = new(10);

        public void AddItems(List<MovableItem> items)
        {
            _movableItems.AddRange(items);
        }

        private void ClearList()
        {
            _movableItems.Clear();
        }

        public void MoveItems()
        {
            for (int i = 0; i < _movableItems.Count; i++)
            {
                _movableItems[i].MoveToTarget();
            }
            ClearList();
        }
    }
}