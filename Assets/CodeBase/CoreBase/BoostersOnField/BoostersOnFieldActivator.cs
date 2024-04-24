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
                    if(cell.IsCellBlocked) continue;

                    if(cell.ItemDataProperty.IsDropBottomItem) continue;
                    if (!cell.IsCellEmpty)
                    {
                        ItemType itemType = cell.ItemDataProperty.itemType;
                        FieldPosition position = cell.FieldPositionProperty;
                        CellView cellView = cell.CellViewProperty;
                        cellView.PlayEffect(()=>_spawnController.Despawn(cellView));
                        _aimController.UpdateAims(itemType, 1, position);
                        cell.ClearCell();
                    }
                }
            }
            
            DespawnCell(_gameModel.FieldModel[fieldPosition]);
        }

        private ICell GetNeighbour(FieldPosition fieldPosition, DirectionForBooster direction)
        {
            ICell cell = null;
            DirectionForBombBoosterAttribute directionAttribute = direction.GetAttribute<DirectionForBombBoosterAttribute>();
            FieldPosition neighbourPosition = new FieldPosition(fieldPosition.Row + directionAttribute.RowIncrement,
                fieldPosition.Column + directionAttribute.ColumnIncrement);
            if(!_gameModel.FieldModel.IsFieldPositionOutOfSize(neighbourPosition))
                cell = _gameModel.FieldModel[neighbourPosition];
            return cell;
        }

        private void ActivateRocketHorizontal(FieldPosition fieldPosition)
        {
            int row = fieldPosition.Row;
            for (int i = 0; i < _gameModel.FieldModel.FieldSize._column; i++)
            {
                if(_gameModel.FieldModel.Cells[row, i].IsCellBlocked) continue;
                if(_gameModel.FieldModel.Cells[row, i].ItemDataProperty is {IsDropBottomItem: true}) continue;
                if (!_gameModel.FieldModel.Cells[row, i].IsCellEmpty)
                {
                    CellView cellView = _gameModel.FieldModel.Cells[row, i].CellViewProperty;
                    ItemType itemType = _gameModel.FieldModel.Cells[row, i].ItemDataProperty.itemType;
                    FieldPosition position = _gameModel.FieldModel.Cells[row, i].FieldPositionProperty;
                    _aimController.UpdateAims(itemType, 1, position);
                    cellView.PlayEffect(() => { _spawnController.Despawn(cellView);});
                    _gameModel.FieldModel.Cells[row, i].ClearCell();
                    //DOVirtual.DelayedCall(0.05f, () => );
                }
            }
        }

        private void ActivateRocketVertical(FieldPosition fieldPosition)
        {
            int column = fieldPosition.Column;
            
            for (int i = 0; i < _gameModel.FieldModel.FieldSize._row; i++)
            {
                if(_gameModel.FieldModel.Cells[i, column].IsCellBlocked) continue;
                if(_gameModel.Level.levelData.levelType == LevelType.DROP_OBJECTS_TO_BOTTOM && 
                   _gameModel.FieldModel.Cells[i, column].ItemDataProperty.IsDropBottomItem) continue;
                
                if (!_gameModel.FieldModel.Cells[i, column].IsCellEmpty)
                {
                    
                    CellView cellView = _gameModel.FieldModel.Cells[i, column].CellViewProperty;
                    ItemType itemType = _gameModel.FieldModel.Cells[i, column].ItemDataProperty.itemType;
                    FieldPosition position = _gameModel.FieldModel.Cells[i, column].FieldPositionProperty;
                    cellView.PlayEffect(()=>_spawnController.Despawn(cellView));
                    _aimController.UpdateAims(itemType, 1, position);
                    _gameModel.FieldModel.Cells[i, column].ClearCell();
                    //DOVirtual.DelayedCall(0.05f, () => );
                }
            }
        }

        private void ActivateDisco(FieldPosition fieldPosition)
        {
            int countToDespawn = 0;
            int row = _gameModel.FieldModel.FieldSize._row;
            int column = _gameModel.FieldModel.FieldSize._column;
            ICell[,] cells = _gameModel.FieldModel.Cells;
            ICell disco = cells[fieldPosition.Row, fieldPosition.Column];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    if(cells[i,j].ItemDataProperty == null) continue;
                    if(i == fieldPosition.Row && j == fieldPosition.Column) continue;
                    
                    ItemType itemType = cells[i, j].ItemDataProperty.itemType;
                    if ( itemType == disco.itemTypeToDestroyForDiscoBooster)
                    {
                        countToDespawn++;
                        ICell cell = cells[i, j];
                        CellView cellView = cell.CellViewProperty;
                        _aimController.UpdateAims(itemType, 1, cell.FieldPositionProperty);
                        cellView.PlayEffect(()=>_spawnController.Despawn(cellView));
                        cell.ClearCell();
                    }
                }
            }
            
            DespawnCell(_gameModel.FieldModel[fieldPosition]);
        }

        private void UpdateAim(ItemType itemType, int count)
        {
            _aimController.UpdateAims(itemType, count);
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
                    ICell cell = _gameModel.FieldModel[neighbourPosition];

                    if (cell.IsBooster())
                    {
                        itemType = cell.ItemDataProperty.itemType;
                        return (true, itemType);
                    }
                }
            }

            return (false, itemType);
        }

        private void DespawnCell(ICell cell)
        {
            _spawnController.Despawn(cell.CellViewProperty);
            cell.ClearCell();
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