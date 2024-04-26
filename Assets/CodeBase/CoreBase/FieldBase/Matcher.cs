using System.Collections.Generic;
using CodeBase.CoreBase.AimTopBar;
using CodeBase.CoreBase.BoostersOnField;
using CodeBase.CoreBase.CellBase;
using CodeBase.CoreBase.Configs;
using CodeBase.CoreBase.GameStateModel;
using CodeBase.Factory;
using UnityEngine;

namespace CodeBase.CoreBase.FieldBase
{
    public class Matcher
    {
        private List<IFieldCell> _matchedCells;
        private readonly GameModel _gameModel;
        private SpawnController _spawnController;
        private BoostersOnFieldActivator _boostersOnFieldActivator;
        private AimController _aimController;
        private List<IFieldCell> _neigboursBlockers;
        
        public Matcher(GameModel gameModel, SpawnController spawnController, AimController aimController)
        {
            _gameModel = gameModel;
            _spawnController = spawnController;
            _aimController = aimController;
            _boostersOnFieldActivator = new BoostersOnFieldActivator(_gameModel, _spawnController, aimController);
        }
        public void TryToMatch(FieldPosition fieldPosition)
        {
            var currentClicked = _gameModel.FieldModel.Cells[fieldPosition.Row, fieldPosition.Column];
            if (currentClicked.IsEmpty || currentClicked.IsNonActive)
            {
                return;
            }
            
            if (currentClicked.IsBooster)
            {
                ActivateBooster(currentClicked);
            }
            else TryToMatch(currentClicked);
        }

        private void ActivateBooster(IFieldCell cellBase)
        {
            _boostersOnFieldActivator.ActivateFieldBooster(cellBase.FieldPositionProperty, cellBase.Cell.ItemDataProperty.itemType);
        }
        
        private void TryToMatch(IFieldCell cellBase)
        {
            ItemType clickedItemType = cellBase.Cell.ItemDataProperty.itemType;

            if (cellBase.Cell.ItemDataProperty.canMatch)
            {
                _matchedCells = new List<IFieldCell> {cellBase};
                _neigboursBlockers = new List<IFieldCell>();
                CheckMatch(clickedItemType, cellBase.FieldPositionProperty.Row, cellBase.FieldPositionProperty.Column);
                Debug.Log(_matchedCells.Count);

                if (_matchedCells.Count == 1)
                {
                    Debug.Log("Not enough matched elements");
                    _matchedCells.Clear();
                    return;
                }

                _gameModel.GameplayCountersModelProperty.MovesModelProperty.Value = -1;
                CheckMatchItemsToMakeBooster(cellBase.FieldPositionProperty);
                //MatchBlockers();
                RemoveMatchedCells();
            }
        }

        private void RemoveMatchedCells()
        {
            for (int i = 0; i < _matchedCells.Count; i++)
            {
                CellView cellView = _matchedCells[i].Cell.CellViewProperty;
                ItemType itemType = _matchedCells[i].Cell.ItemDataProperty.itemType;
                FieldPosition fieldPosition = _matchedCells[i].FieldPositionProperty;
                cellView.PlayEffect(() =>
                {
                    _spawnController.Despawn(cellView);
                });
                _aimController.UpdateAims(itemType, 1, fieldPosition);
                _gameModel.FieldModel.Cells[fieldPosition._row, fieldPosition._column].Cell = new EmptyCell();
            }
            
            _matchedCells.Clear();
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
            if ((!cell.IsEmpty || !cell.IsNonActive) && cell.Cell.ItemDataProperty != null)
            {
                if (_matchedCells.Exists(local => local.FieldPositionProperty.Equals(cell.FieldPositionProperty)))
                {
                    Debug.Log($"Item exist in list - FieldPosition - {cell.FieldPositionProperty}");
                    return;
                }

                
                if (targetType == cell.Cell.ItemDataProperty.itemType)
                {
                    _matchedCells.Add(cell);
                    CheckMatch(targetType, rowIndex, columnIndex);
                }
            }
        }

        private void MatchBlockers()
        {
            for (int i = 0; i < _matchedCells.Count; i++)
            {
                var position = _matchedCells[i].FieldPositionProperty;
                    
                CheckBlocker(position._row, position._column + 1);
                CheckBlocker(position._row, position._column - 1);
                CheckBlocker(position._row + 1, position._column);
                CheckBlocker(position._row - 1, position._column);
            }
        }
        
        private void CheckBlocker(int row, int column)
        {
            if (_gameModel.FieldModel.IsFieldPositionOutOfSize(row, column)) return;

            var cell = _gameModel.FieldModel.Cells[row, column];
            if (cell.IsBlocked)
            {
                cell.Cell.PlayEffect(() =>
                {
                    _spawnController.Despawn(cell.Cell.CellViewProperty);
                });

                cell.Cell = new EmptyCell();
            }
        }

        private void CheckMatchItemsToMakeBooster(FieldPosition fieldPosition)
        {
            var booster = _spawnController.SpawnBoosterCellView(fieldPosition, _matchedCells.Count);
            if (booster.boosterView != null && booster.boosterData != null)
            {
                ItemType itemType = _matchedCells[0].Cell.ItemDataProperty.itemType;
                _gameModel.FieldModel.Cells[fieldPosition.Row, fieldPosition.Column].Cell.itemTypeToDestroyForDiscoBooster =
                    itemType;
                _matchedCells.RemoveAt(0);
                _spawnController.Despawn(_gameModel.FieldModel.Cells[fieldPosition.Row, fieldPosition.Column].Cell.CellViewProperty);
                _gameModel.FieldModel.Cells[fieldPosition.Row, fieldPosition.Column].Cell.Initialize(booster.boosterView, booster.boosterData);
            } 
        }
    }
}