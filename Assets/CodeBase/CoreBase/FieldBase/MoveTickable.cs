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
            private IItem _item;
            private Vector2 _targetPosition;
            private ICell _cell;
            private ICell _currentCell;

            public MovableItem(IItem item, Vector2 targetPosition)
            {
                _item = item;
                _targetPosition = targetPosition;
            }
            
            public void MoveToTarget()
            {
                _item.MoveToZeroPositionOfParent(_targetPosition, _cell, _currentCell);
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