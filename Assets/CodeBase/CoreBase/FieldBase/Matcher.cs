using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.CoreBase.AimTopBar;
using CodeBase.CoreBase.CellBase;
using CodeBase.CoreBase.Configs;
using CodeBase.CoreBase.GameStateModel;
using CodeBase.Factory;
using CodeBase.Pool;
using UnityEngine;

namespace CodeBase.CoreBase.FieldBase
{
    public class Matcher
    {
        private List<ICell> _matchedCells;
        private readonly GameModel _gameModel;
        private SpawnController _spawnController;
        
        public Matcher(GameModel gameModel, SpawnController spawnController)
        {
            _gameModel = gameModel;
            _spawnController = spawnController;
        }
        public void TryToMatch(FieldPosition fieldPosition)
        {
            var currentClicked = _gameModel.FieldModel.Cells[fieldPosition.Row, fieldPosition.Column];
            ItemType clickedItemType = currentClicked.ItemDataProperty.itemType;

            if (clickedItemType == ItemType.NONE)
            {
                Debug.Log($"Empty item - FieldPosition - {fieldPosition}");
                return;
            }

            _matchedCells = new List<ICell> {currentClicked};
            CheckMatch(clickedItemType, fieldPosition.Row, fieldPosition.Column);
            Debug.Log(_matchedCells.Count);

            if (_matchedCells.Count == 1)
            {
                Debug.Log("Not enough matched elements");
                _matchedCells.Clear();
                return;
            }
            _gameModel.GameplayCountersModelProperty.MovesModelProperty.Value = -1;
            CheckLevelAim();
            CheckMatchItemsToMakeBooster(fieldPosition);
            RemoveMatchedCells();
        }

        private void RemoveMatchedCells()
        {
            for (int i = 0; i < _matchedCells.Count; i++)
            {
                CellView cellView = _matchedCells[i].CellViewProperty;
                cellView.PlayEffect(() =>
                {
                    _spawnController.Despawn(cellView);
                });
                _matchedCells[i].ClearCell();
            }
        }

        private void CheckLevelAim()
        {
            ItemType itemType = _matchedCells[0].ItemDataProperty.itemType;
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

            if (targetType == cell.ItemDataProperty.itemType)
            {
                _matchedCells.Add(cell);
                CheckMatch(targetType, rowIndex, columnIndex);
            }
        }

        
        private void CheckMatchItemsToMakeBooster(FieldPosition fieldPosition)
        {
            var booster = _spawnController.SpawnBoosterCellView(fieldPosition, _matchedCells.Count);
            if (booster.boosterView != null && booster.boosterData != null)
            {

                ItemType itemType = _matchedCells[0].ItemDataProperty.itemType;
                _gameModel.FieldModel.Cells[fieldPosition.Row, fieldPosition.Column].itemTypeToDestroyForDiscoBooster =
                    itemType;
                _matchedCells.RemoveAt(0);
                _spawnController.Despawn(_gameModel.FieldModel.Cells[fieldPosition.Row, fieldPosition.Column].CellViewProperty);
                _gameModel.FieldModel.Cells[fieldPosition.Row, fieldPosition.Column].Initialize(booster.boosterView, booster.boosterData);
                

            } 
        }
    }
}