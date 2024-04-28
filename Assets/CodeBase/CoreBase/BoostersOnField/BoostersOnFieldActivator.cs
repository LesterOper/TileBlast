using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.CoreBase.AimTopBar;
using CodeBase.CoreBase.CellBase;
using CodeBase.CoreBase.Configs;
using CodeBase.CoreBase.GameStateModel;
using CodeBase.Factory;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.CoreBase.BoostersOnField
{
    public class BoostersOnFieldActivator
    {
        private GameModel _gameModel;
        private SpawnController _spawnController;
        private AimController _aimController;
        

        public BoostersOnFieldActivator(
            GameModel gameModel,
            SpawnController spawnController,
            AimController aimController
        )
        {
            _gameModel = gameModel;
            _spawnController = spawnController;
            _aimController = aimController;
        }

        public void ActivateFieldBooster(FieldPosition fieldPosition, ItemType boosterItemType)
        {
            var checkedNeighbour = CheckNeighboursCellsBeforeBoosterActivated(fieldPosition);
            if (checkedNeighbour.isBooster)
            {
                if (boosterItemType == ItemType.DISCO || checkedNeighbour.itemType == ItemType.DISCO)
                {
                    DiscoCombo(fieldPosition);
                    return;
                }

                if (boosterItemType == ItemType.BOMB || checkedNeighbour.itemType == ItemType.BOMB)
                {
                    if (boosterItemType is ItemType.ROCKET_VERTICAL or ItemType.ROCKET_HORIZONTAL ||
                        checkedNeighbour.itemType is ItemType.ROCKET_VERTICAL or ItemType.ROCKET_HORIZONTAL)
                    {
                        TrippleRocketCombo(fieldPosition);
                        return;
                    }
                }

                if (boosterItemType is ItemType.ROCKET_VERTICAL or ItemType.ROCKET_HORIZONTAL)
                {
                    if (checkedNeighbour.itemType is ItemType.ROCKET_VERTICAL or ItemType.ROCKET_HORIZONTAL)
                    {
                        RocketCombo(fieldPosition);
                        return;
                    }
                }
            }
            
            switch (boosterItemType)
            {
                case ItemType.ROCKET_VERTICAL:
                    ActivateRocketVertical(fieldPosition);
                    break;
                case ItemType.ROCKET_HORIZONTAL:
                    ActivateRocketHorizontal(fieldPosition);
                    break;
                case ItemType.BOMB:
                    ActivateBomb(fieldPosition);
                    break;
                case ItemType.DISCO:
                    ActivateDisco(fieldPosition);
                    break;
            }
            
        }

        private void ActivateBomb(FieldPosition fieldPosition)
        {
            _spawnController.SpawnEffect(VfxType.BOMB, fieldPosition);
            foreach (DirectionForBooster direction in Enum.GetValues(typeof(DirectionForBooster)))
            {
                var cell = GetNeighbour(fieldPosition, direction);

                if (cell != null)
                {
                    if(cell.IsEmpty) continue;

                    if(cell.Cell.ItemDataProperty.IsDropBottomItem) continue;
                    
                    ItemType itemType = cell.Cell.ItemDataProperty.itemType;
                    FieldPosition position = cell.FieldPositionProperty;
                    CellView cellView = cell.Cell.CellViewProperty;
                    cellView.PlayEffect(()=>_spawnController.Despawn(cellView));
                    _aimController.UpdateAims(itemType, 1, position);
                    cell.Cell = new EmptyCell();
                }
            }
            
            DespawnCell(_gameModel.FieldModel.Cells[fieldPosition.Row, fieldPosition.Column]);
        }

        private IFieldCell GetNeighbour(FieldPosition fieldPosition, DirectionForBooster direction)
        {
            IFieldCell cellBase = null;
            DirectionForBombBoosterAttribute directionAttribute = direction.GetAttribute<DirectionForBombBoosterAttribute>();
            FieldPosition neighbourPosition = new FieldPosition(fieldPosition.Row + directionAttribute.RowIncrement,
                fieldPosition.Column + directionAttribute.ColumnIncrement);
            if(!_gameModel.FieldModel.IsFieldPositionOutOfSize(neighbourPosition))
                cellBase = _gameModel.FieldModel.Cells[neighbourPosition._row, neighbourPosition._column];
            return cellBase;
        }

        private void ActivateRocketHorizontal(FieldPosition fieldPosition)
        {
            int row = fieldPosition.Row;
            for (int i = 0; i < _gameModel.FieldModel.FieldSize._column; i++)
            {
                if(_gameModel.FieldModel.Cells[row, i].IsEmpty) continue;
                if(_gameModel.FieldModel.Cells[row, i].Cell.ItemDataProperty is {IsDropBottomItem: true}) continue;
                
                CellView cellView = _gameModel.FieldModel.Cells[row, i].Cell.CellViewProperty;
                ItemType itemType = _gameModel.FieldModel.Cells[row, i].Cell.ItemDataProperty.itemType;
                FieldPosition position = _gameModel.FieldModel.Cells[row, i].FieldPositionProperty;
                _aimController.UpdateAims(itemType, 1, position);
                cellView.PlayEffect(() => { _spawnController.Despawn(cellView);});
                _gameModel.FieldModel.Cells[row, i].Cell = new EmptyCell();
            }
        }

        private void ActivateRocketVertical(FieldPosition fieldPosition)
        {
            int column = fieldPosition.Column;
            
            for (int i = 0; i < _gameModel.FieldModel.FieldSize._row; i++)
            {
                if(_gameModel.FieldModel.Cells[i, column].IsEmpty) continue;
                if(_gameModel.Level.levelData.levelType == LevelType.DROP_OBJECTS_TO_BOTTOM && 
                   _gameModel.FieldModel.Cells[i, column].Cell.ItemDataProperty.IsDropBottomItem) continue;
                
                CellView cellView = _gameModel.FieldModel.Cells[i, column].Cell.CellViewProperty;
                ItemType itemType = _gameModel.FieldModel.Cells[i, column].Cell.ItemDataProperty.itemType;
                FieldPosition position = _gameModel.FieldModel.Cells[i, column].FieldPositionProperty;
                cellView.PlayEffect(()=>_spawnController.Despawn(cellView));
                _aimController.UpdateAims(itemType, 1, position);
                _gameModel.FieldModel.Cells[i, column].Cell.ClearCell();
            }
        }

        private void ActivateDisco(FieldPosition fieldPosition)
        {
            int row = _gameModel.FieldModel.FieldSize._row;
            int column = _gameModel.FieldModel.FieldSize._column;
            IFieldCell[,] cells = _gameModel.FieldModel.Cells;
            ICellBase disco = cells[fieldPosition.Row, fieldPosition.Column].Cell;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    if(cells[i,j].Cell.ItemDataProperty == null) continue;
                    if(i == fieldPosition.Row && j == fieldPosition.Column) continue;
                    
                    ItemType itemType = cells[i, j].Cell.ItemDataProperty.itemType;
                    if ( itemType == disco.itemTypeToDestroyForDiscoBooster)
                    {
                        ICellBase cellBase = cells[i, j].Cell;
                        CellView cellView = cellBase.CellViewProperty;
                        _aimController.UpdateAims(itemType, 1, cellBase.FieldPositionProperty);
                        cellView.PlayEffect(()=>_spawnController.Despawn(cellView));
                        cells[i, j].Cell = new EmptyCell();
                    }
                }
            }
            
            DespawnCell(_gameModel.FieldModel.Cells[fieldPosition.Row, fieldPosition.Column]);
        }

        private (bool isBooster, ItemType itemType) CheckNeighboursCellsBeforeBoosterActivated(FieldPosition fieldPosition)
        {
            ItemType itemType = ItemType.NONE;
            for (int i = 0; i < Enum.GetValues(typeof(DirectionForBooster)).Length - 4; i++)
            {
                DirectionForBooster direction = (DirectionForBooster) i;
                DirectionForBombBoosterAttribute directionForBombBoosterAttribute =
                    direction.GetAttribute<DirectionForBombBoosterAttribute>();
                FieldPosition neighbourPosition =
                    new FieldPosition(fieldPosition.Row + directionForBombBoosterAttribute.RowIncrement,
                        fieldPosition.Column + directionForBombBoosterAttribute.ColumnIncrement);

                if (!_gameModel.FieldModel.IsFieldPositionOutOfSize(neighbourPosition))
                {
                    IFieldCell cellBase = _gameModel.FieldModel.Cells[neighbourPosition._row, neighbourPosition._column];

                    if (cellBase.IsBooster)
                    {
                        itemType = cellBase.Cell.ItemDataProperty.itemType;
                        return (true, itemType);
                    }
                }
            }

            return (false, itemType);
        }

        private void DespawnCell(IFieldCell cellBase)
        {
            _spawnController.Despawn(cellBase.Cell.CellViewProperty);
            cellBase.Cell = new EmptyCell();
        }

        private void RocketCombo(FieldPosition fieldPosition)
        {
            ActivateRocketHorizontal(fieldPosition);
            ActivateRocketVertical(fieldPosition);
        }

        private void DiscoCombo(FieldPosition fieldPosition)
        {
            ActivateDisco(fieldPosition);
        }

        private void TrippleRocketCombo(FieldPosition fieldPosition)
        {
            var position = new FieldPosition(fieldPosition.Row + 1, fieldPosition.Column);
            if(!_gameModel.FieldModel.IsFieldPositionOutOfSize(position))
                ActivateRocketHorizontal(position);
            position = new FieldPosition(fieldPosition.Row - 1, fieldPosition.Column);
            if(!_gameModel.FieldModel.IsFieldPositionOutOfSize(position))
                ActivateRocketHorizontal(position);
            position = new FieldPosition(fieldPosition.Row, fieldPosition.Column - 1);
            if(!_gameModel.FieldModel.IsFieldPositionOutOfSize(position))
                ActivateRocketVertical(position);
            position = new FieldPosition(fieldPosition.Row, fieldPosition.Column + 1);
            if(!_gameModel.FieldModel.IsFieldPositionOutOfSize(position))
                ActivateRocketVertical(position);
            ActivateRocketHorizontal(new FieldPosition(fieldPosition.Row, fieldPosition.Column));
            ActivateRocketVertical(new FieldPosition(fieldPosition.Row, fieldPosition.Column));
        }

        protected enum DirectionForBooster
        {
            [DirectionForBombBoosterAttribute(1, 0)]
            UP = 0,
            [DirectionForBombBoosterAttribute(-1, 0)]
            DOWN = 1,
            [DirectionForBombBoosterAttribute(0, -1)]
            LEFT = 2,
            [DirectionForBombBoosterAttribute(0, 1)]
            RIGHT = 3,
            [DirectionForBombBoosterAttribute(1, -1)]
            LEFT_UP = 4,
            [DirectionForBombBoosterAttribute(-1, -1)]
            LEFT_DOWN = 5,
            [DirectionForBombBoosterAttribute(1, 1)]
            RIGHT_UP = 6,
            [DirectionForBombBoosterAttribute(-1, 1)]
            RIGHT_DOWN = 7
        }

        protected class DirectionForBombBoosterAttribute : Attribute
        {
            public int RowIncrement;
            public int ColumnIncrement;
            
            public DirectionForBombBoosterAttribute(int rowIncrement, int columnIncrement)
            {
                RowIncrement = rowIncrement;
                ColumnIncrement = columnIncrement;
            }   
        }
    }
}