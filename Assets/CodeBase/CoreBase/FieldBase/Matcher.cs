using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.CoreBase.AimTopBar;
using CodeBase.CoreBase.CellBase;
using CodeBase.CoreBase.Configs;
using CodeBase.CoreBase.GameStateModel;
using CodeBase.Pool;
using UnityEngine;

namespace CodeBase.CoreBase.FieldBase
{
    public class Matcher
    {
        public event Action OnMove;
        private List<ICell> _matchedCells;
        private readonly GameModel _gameModel;
        private ItemPool _itemPool;

        public Matcher(GameModel gameModel, ItemPool itemPool)
        {
            _gameModel = gameModel;
            _itemPool = itemPool;
        }
        public void TryToMatch(FieldPosition fieldPosition)
        {
            var currentClicked = _gameModel.FieldModel.Cells[fieldPosition.Row, fieldPosition.Column];
            ItemType clickedItemType = currentClicked.ItemType;

            if (clickedItemType == ItemType.NONE)
            {
                Debug.Log($"Empty item - FieldPosition - {fieldPosition}");
                //OnMove?.Invoke();
                return;
            }

            _matchedCells = new List<ICell>();
            _matchedCells.Add(currentClicked);
            CheckMatch(clickedItemType, fieldPosition.Row, fieldPosition.Column);
            Debug.Log(_matchedCells.Count);

            if (_matchedCells.Count == 1)
            {
                Debug.Log("Not enough matched elements");
                var animate = _matchedCells[0] as IAnimate;
                animate?.ShakeAnimation();
                //OnMove?.Invoke();
                return;
            }
            _gameModel.GameplayCountersModelProperty.MovesModelProperty.Value = -1;
            RemoveMatchedCells();
            //OnMove?.Invoke();
        }

        private void RemoveMatchedCells()
        {
            CheckLevelAim();
            for (int i = 0; i < _matchedCells.Count; i++)
            {
                _itemPool.Despawn(_matchedCells[i].ItemProperty.ItemView);
                _matchedCells[i].ClearItem();
            }
        }

        private void CheckLevelAim()
        {
            ItemType itemType = _matchedCells[0].ItemType;
            IAimSetter aim = _gameModel.GameplayCountersModelProperty.AimModelSetterProperty.Aims.FirstOrDefault(local => local.AimItemType == itemType);
            if (aim != null)
                aim.Value = -_matchedCells.Count;
        }

        private void CheckMatch(ItemType targetType, int rowIndex, int columnIndex)
        {
            if (rowIndex > 0)
            {
                CheckItem(targetType, rowIndex - 1, columnIndex);
            }

            if (rowIndex < _gameModel.FieldModel.Cells.GetUpperBound(0))
            {
                CheckItem(targetType, rowIndex + 1, columnIndex);
            }

            if (columnIndex < _gameModel.FieldModel.Cells.GetUpperBound(1))
            {
                CheckItem(targetType, rowIndex, columnIndex + 1);
            }

            if (columnIndex > 0)
            {
                CheckItem(targetType, rowIndex, columnIndex - 1);
            }
        }

        private void CheckItem(ItemType targetType, int rowIndex, int columnIndex)
        {
            var cell = _gameModel.FieldModel.Cells[rowIndex, columnIndex];
            if (_matchedCells.Exists(local => local.FieldPositionProperty.Equals(cell.FieldPositionProperty)))
            {
                Debug.Log($"Item exist in list - FieldPosition - {cell.FieldPositionProperty}");
                return;
            }

            if (targetType == cell.ItemType)
            {
                _matchedCells.Add(cell);
                CheckMatch(targetType, rowIndex, columnIndex);
            }
        }
    }
}