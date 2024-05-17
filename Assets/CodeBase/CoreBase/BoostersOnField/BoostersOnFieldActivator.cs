using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeBase.CoreBase.AimTopBar;
using CodeBase.CoreBase.CellBase;
using CodeBase.CoreBase.Configs;
using CodeBase.CoreBase.GameStateModel;
using CodeBase.Factory;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.CoreBase.BoostersOnField
{
    public class BoostersOnFieldActivator
    {
        private GameModel _gameModel;
        private SpawnController _spawnController;
        private AimController _aimController;
        private List<IFieldCell> _boostersActivatedByOthersBoosters;
        private CoroutineRunner _coroutineRunner;

        public BoostersOnFieldActivator(
            GameModel gameModel,
            SpawnController spawnController,
            AimController aimController,
            CoroutineRunner coroutineRunner
        )
        {
            _gameModel = gameModel;
            _spawnController = spawnController;
            _aimController = aimController;
            _coroutineRunner = coroutineRunner;
        }


        public async void ActivateBooster(FieldPosition fieldPosition, ItemType boosterItemType, Action callback)
        {
            _boostersActivatedByOthersBoosters = new List<IFieldCell>(10);
            var checkedNeighbour = CheckNeighboursCellsBeforeBoosterActivated(fieldPosition);
            Task activate = null;
            if (checkedNeighbour.isBooster)
            {
                activate = ActivateCombo(checkedNeighbour.neighbour, boosterItemType, fieldPosition);
            }
            else
            {
                activate = ActivateSingleBooster(fieldPosition, boosterItemType);
            }

            await activate;
            await ActivateBoostersWhichActivatedByOtherBoosters();
            await Task.Delay(200);
            callback.Invoke();
        }

        private async Task ActivateCombo(IFieldCell neighbour, ItemType boosterItemType, FieldPosition fieldPosition)
        {
            if (boosterItemType == ItemType.DISCO || neighbour.Cell.ItemDataProperty.itemType == ItemType.DISCO)
            {
                if (boosterItemType == ItemType.DISCO && neighbour.Cell.ItemDataProperty.itemType == ItemType.DISCO)
                {
                    await DiscoPlusDiscoCombo();
                }
                else await DiscoCombo(fieldPosition, neighbour.FieldPositionProperty);
                
                return;
            }

            if (boosterItemType == ItemType.BOMB || neighbour.Cell.ItemDataProperty.itemType == ItemType.BOMB)
            {
                if (boosterItemType is ItemType.ROCKET_VERTICAL or ItemType.ROCKET_HORIZONTAL ||
                    neighbour.Cell.ItemDataProperty.itemType is ItemType.ROCKET_VERTICAL or ItemType.ROCKET_HORIZONTAL)
                {
                    await TrippleRocketCombo(fieldPosition, neighbour);
                    return;
                }
                else if (boosterItemType == ItemType.BOMB && neighbour.Cell.ItemDataProperty.itemType == ItemType.BOMB)
                {
                    await DespawnCell(neighbour);
                    await DespawnCell(_gameModel.FieldModel.Cells[fieldPosition._row, fieldPosition._column]);
                    await ActivateBomb(fieldPosition, 3);
                }
            }

            if (boosterItemType is ItemType.ROCKET_VERTICAL or ItemType.ROCKET_HORIZONTAL)
            {
                if (neighbour.Cell.ItemDataProperty.itemType is ItemType.ROCKET_VERTICAL or ItemType.ROCKET_HORIZONTAL)
                {
                    await RocketCombo(fieldPosition, neighbour.FieldPositionProperty);
                    return;
                }
            }
        }
        
        private async Task ActivateSingleBooster(FieldPosition fieldPosition, ItemType boosterItemType)
        {
            switch (boosterItemType)
            {
                case ItemType.ROCKET_VERTICAL:
                    await DespawnCell(_gameModel.FieldModel.Cells[fieldPosition._row, fieldPosition._column]);
                    await ActivateRocketVertical(fieldPosition);
                    break;
                case ItemType.ROCKET_HORIZONTAL:
                    await DespawnCell(_gameModel.FieldModel.Cells[fieldPosition._row, fieldPosition._column]);
                    await ActivateRocketHorizontal(fieldPosition);
                    break;
                case ItemType.BOMB:
                    await DespawnCell(_gameModel.FieldModel.Cells[fieldPosition._row, fieldPosition._column]);
                    await ActivateBomb(fieldPosition);
                    break;
                case ItemType.DISCO:
                    await DespawnCell(_gameModel.FieldModel.Cells[fieldPosition._row, fieldPosition._column]);
                    await ActivateDisco(fieldPosition);
                    break;
            }
        }

        private async Task ActivateBoostersWhichActivatedByOtherBoosters()
        {
            if (_boostersActivatedByOthersBoosters.Count > 0)
            {
                List<IFieldCell> boosters =
                    new List<IFieldCell>(_boostersActivatedByOthersBoosters.FindAll(local => local.IsBooster));
                _boostersActivatedByOthersBoosters.Clear();
                for (int i = 0; i < boosters.Count; i++)
                {
                    if(boosters[i].IsBooster)
                    {
                        ItemType itemType = boosters[i].Cell.ItemDataProperty.itemType;
                        await DespawnCell(boosters[i]);
                        await CheckBoosterByType(itemType,
                            boosters[i].FieldPositionProperty);
                        
                    }
                }

                await ActivateBoostersWhichActivatedByOtherBoosters();
            }
        }

        private async Task ActivateBomb(FieldPosition fieldPosition, int increment = 0)
        {
            _spawnController.SpawnEffect(VfxType.BOMB, fieldPosition);

            int rowStart = fieldPosition._row - (1 + increment);
            int rowEnd = fieldPosition._row + (1 + increment);
            int columnStart = fieldPosition._column - (1 + increment);
            int columnEnd = fieldPosition._column + (1 + increment);

            while (rowStart <= rowEnd && columnStart <= columnEnd)
            {
                for (int col = columnStart; col <= columnEnd; col++)
                {
                    if(_gameModel.FieldModel.IsFieldPositionOutOfSize(rowStart, col)) continue;
                    IFieldCell cell = _gameModel.FieldModel.Cells[rowStart, col];
                    if(cell.IsNonActive) continue;
                    if(cell.IsEmpty) continue;
                    if(cell.IsDropDown) continue;
                    
                    CellView cellView = cell.Cell.CellViewProperty;
                    FieldPosition position = cell.FieldPositionProperty;
                    if (cell.IsBooster)
                    {
                        _boostersActivatedByOthersBoosters.Add(cell);
                    }
                    else
                    {
                        await DespawnCellWithEffect(cellView, position, cell, 0);
                    }
                }
                rowStart++;
        
                // Traverse the right column
                for (int row = rowStart; row <= rowEnd; row++)
                {
                    if(_gameModel.FieldModel.IsFieldPositionOutOfSize(row, columnEnd)) continue;
                    IFieldCell cell = _gameModel.FieldModel.Cells[row, columnEnd];

                    if(cell.IsNonActive) continue;
                    if(cell.IsEmpty) continue;
                    if(cell.IsDropDown) continue;
                    
                    CellView cellView = cell.Cell.CellViewProperty;
                    FieldPosition position = cell.FieldPositionProperty;
                    if (cell.IsBooster)
                    {
                        _boostersActivatedByOthersBoosters.Add(cell);
                    }
                    else
                    {
                        await DespawnCellWithEffect(cellView, position, cell, 0);
                    }
                }
                columnEnd--;
        
                // Traverse the bottom row
                if (rowStart <= rowEnd)
                {
                    for (int col = columnEnd; col >= columnStart; col--)
                    {
                        if(_gameModel.FieldModel.IsFieldPositionOutOfSize(rowEnd, col)) continue;
                        IFieldCell cell = _gameModel.FieldModel.Cells[rowEnd, col];
                        if(cell.IsNonActive) continue;
                        if(cell.IsEmpty) continue;
                        if(cell.IsDropDown) continue;
                        
                        CellView cellView = cell.Cell.CellViewProperty;
                        FieldPosition position = cell.FieldPositionProperty;
                        if (cell.IsBooster)
                        {
                            _boostersActivatedByOthersBoosters.Add(cell);
                        }
                        else
                        {
                            await DespawnCellWithEffect(cellView, position, cell, 0);
                        }
                    }
                    rowEnd--;
                }
        
                // Traverse the left column
                if (columnStart <= columnEnd)
                {
                    for (int row = rowEnd; row >= rowStart; row--)
                    {                        
                        if(_gameModel.FieldModel.IsFieldPositionOutOfSize(row, columnStart)) continue;
                        IFieldCell cell = _gameModel.FieldModel.Cells[row, columnStart];

                        if(cell.IsNonActive) continue;
                        if(cell.IsEmpty) continue;
                        if(cell.IsDropDown) continue;
                        
                        CellView cellView = cell.Cell.CellViewProperty;
                        FieldPosition position = cell.FieldPositionProperty;
                        if (cell.IsBooster)
                        {
                            _boostersActivatedByOthersBoosters.Add(cell);
                        }
                        else
                        {
                            await DespawnCellWithEffect(cellView, position, cell, 0);
                        }
                    }
                    columnStart++;
                }
            }
        }
        
        private async Task ActivateRocketHorizontal(FieldPosition fieldPosition)
        {
            int row = fieldPosition.Row;

            Task leftRocket = LeftRocket(fieldPosition._column, row);
            Task rightRocket = RightRocket(fieldPosition._column, row);

            await Task.WhenAll(leftRocket, rightRocket);
        }

        private async Task ActivateRocketVertical(FieldPosition fieldPosition)
        {
            int column = fieldPosition.Column;

            Task upRocket = UpRocket(fieldPosition._row, column);
            Task downRocket = DownRocket(fieldPosition._row, column);

            await Task.WhenAll(upRocket, downRocket);
        }
        
        private async Task ActivateDisco(FieldPosition fieldPosition)
        {
            int row = _gameModel.FieldModel.FieldSize._row;
            int column = _gameModel.FieldModel.FieldSize._column;
            IFieldCell[,] cells = _gameModel.FieldModel.Cells;
            ICellBase disco = cells[fieldPosition.Row, fieldPosition.Column].Cell;
            if (disco.itemTypeToDestroyForDiscoBooster == ItemType.NONE)
            {
                disco.itemTypeToDestroyForDiscoBooster =
                    (ItemType) Random.Range(1, Enum.GetValues(typeof(ItemType)).Length - 7);
            }
            
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    if(cells[i,j].Cell.ItemDataProperty == null) continue;
                    if(i == fieldPosition.Row && j == fieldPosition.Column) continue;
                    
                    ItemType itemType = cells[i, j].Cell.ItemDataProperty.itemType;
                    if ( itemType == disco.itemTypeToDestroyForDiscoBooster)
                    {
                        IFieldCell cellBase = cells[i, j];
                        CellView cellView = cellBase.Cell.CellViewProperty;
                        _aimController.UpdateAims(itemType, 1, cellBase.FieldPositionProperty);
                        cellView.PlayEffect(()=>_spawnController.Despawn(cellView));
                        cells[i, j].Cell = new EmptyCell();
                        await Task.Delay(10);
                    }
                }
            }
        }

        private async Task LeftRocket(int startColumn, int row)
        {
            for (int i = startColumn - 1; i >= 0; i--)
            {
                IFieldCell currentCell = _gameModel.FieldModel.Cells[row, i];
                if(currentCell.IsNonActive) continue;
                if(currentCell.IsEmpty) continue;
                if(currentCell.Cell.ItemDataProperty is {IsDropBottomItem: true}) continue;
            
                CellView cellView = currentCell.Cell.CellViewProperty;
                FieldPosition position = currentCell.FieldPositionProperty;
                if (currentCell.IsBooster)
                {
                    _boostersActivatedByOthersBoosters.Add(currentCell);
                }
                else
                {
                    await DespawnCellWithEffect(cellView, position, currentCell, 5);
                }
            }
        }

        private async Task RightRocket(int startColumn, int row)
        {
            for (int i = startColumn + 1; i < _gameModel.FieldModel.FieldSize._column; i++)
            {
                IFieldCell currentCell = _gameModel.FieldModel.Cells[row, i];
                if(currentCell.IsNonActive) continue;
                if(currentCell.IsEmpty) continue;
                if(currentCell.Cell.ItemDataProperty is {IsDropBottomItem: true}) continue;
            
                CellView cellView = currentCell.Cell.CellViewProperty;
                FieldPosition position = currentCell.FieldPositionProperty;
                if (currentCell.IsBooster)
                {
                    _boostersActivatedByOthersBoosters.Add(currentCell);
                }
                else
                {
                    await DespawnCellWithEffect(cellView, position, currentCell, 5);
                }
            }
        }

        private async Task DownRocket(int rowStart, int column)
        {
            for (int i = rowStart - 1; i >= 0; i--)
            {
                IFieldCell currentCell = _gameModel.FieldModel.Cells[i, column];
                if(currentCell.IsNonActive) continue;
                if(currentCell.IsEmpty) continue;
                if(_gameModel.Level.levelData.levelType == LevelType.DROP_OBJECTS_TO_BOTTOM && 
                   currentCell.Cell.ItemDataProperty.IsDropBottomItem) continue;
            
                CellView cellView = currentCell.Cell.CellViewProperty;
                FieldPosition position = currentCell.FieldPositionProperty;

                if (currentCell.IsBooster)
                {
                    _boostersActivatedByOthersBoosters.Add(currentCell);
                }
                else
                {
                    await DespawnCellWithEffect(cellView, position, currentCell, 5);
                }
                
            }
        }

        private async Task UpRocket(int rowStart, int column)
        {
            for (int i = rowStart + 1; i < _gameModel.FieldModel.FieldSize._row; i++)
            {
                IFieldCell currentCell = _gameModel.FieldModel.Cells[i, column];
                if(currentCell.IsNonActive) continue;
                if(currentCell.IsEmpty) continue;
                if(_gameModel.Level.levelData.levelType == LevelType.DROP_OBJECTS_TO_BOTTOM && 
                   currentCell.Cell.ItemDataProperty.IsDropBottomItem) continue;
            
                CellView cellView = currentCell.Cell.CellViewProperty;
                FieldPosition position = currentCell.FieldPositionProperty;

                if (currentCell.IsBooster)
                {
                    _boostersActivatedByOthersBoosters.Add(currentCell);
                }
                else
                {
                    await DespawnCellWithEffect(cellView, position, currentCell, 5);
                }
            }
        }

        private async Task DespawnCellWithEffect(CellView cellView, FieldPosition fieldPosition, IFieldCell fieldCell, int delay = 25)
        {
            cellView.PlayEffect(() => _spawnController.Despawn(cellView));
            _aimController.UpdateAims(fieldCell.Cell.ItemDataProperty.itemType, 1, fieldPosition);
            fieldCell.Cell = new EmptyCell();
            await Task.Delay(delay);
        }
        
        private (bool isBooster, IFieldCell neighbour) CheckNeighboursCellsBeforeBoosterActivated(FieldPosition fieldPosition)
        {
            IFieldCell cellBase = null;
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
                    cellBase = _gameModel.FieldModel.Cells[neighbourPosition._row, neighbourPosition._column];

                    if (cellBase.IsBooster)
                    {
                        return (true, cellBase);
                    }
                }
            }

            return (false, cellBase);
        }

        private async Task DespawnCell(IFieldCell cellBase)
        {
            _spawnController.Despawn(cellBase.Cell.CellViewProperty);
            cellBase.Cell = new EmptyCell();
            await Task.Delay(50);
        }

        private async Task DiscoPlusDiscoCombo()
        {
            for (int i = 0; i < _gameModel.FieldModel.FieldSize._row; i++)
            {
                for (int j = 0; j < _gameModel.FieldModel.FieldSize._column; j++)
                {
                    IFieldCell fieldCell = _gameModel.FieldModel.Cells[i, j];
                    if(fieldCell.IsEmpty 
                       || fieldCell.IsNonActive 
                       || fieldCell.IsDropDown) continue;

                    CellView cellView = fieldCell.Cell.CellViewProperty;
                    FieldPosition fieldPosition = fieldCell.FieldPositionProperty;
                    await DespawnCellWithEffect(cellView, fieldPosition, fieldCell, 0);
                }
            }
        }
        
        private async Task RocketCombo(FieldPosition clickedBooster, FieldPosition neighbour)
        {
            Task despawnClickedCell =
                DespawnCell(_gameModel.FieldModel.Cells[clickedBooster._row, clickedBooster._column]);
            Task despawnNeighbourCell = DespawnCell(_gameModel.FieldModel.Cells[neighbour._row, neighbour._column]);
            await Task.WhenAll(despawnClickedCell, despawnNeighbourCell);
            Task horizontal = ActivateRocketHorizontal(clickedBooster);
            Task vertical = ActivateRocketVertical(clickedBooster);
            await Task.WhenAll(horizontal, vertical);
        }

        private async Task DiscoCombo(FieldPosition discoFieldPosition, FieldPosition neighbourBooster)
        {
            int row = _gameModel.FieldModel.FieldSize._row;
            int column = _gameModel.FieldModel.FieldSize._column;
            IFieldCell[,] cells = _gameModel.FieldModel.Cells;
            List<IFieldCell> spawned = new List<IFieldCell>();

            if (cells[neighbourBooster._row, neighbourBooster._column].Cell.ItemDataProperty.itemType == ItemType.DISCO)
            {
                (discoFieldPosition, neighbourBooster) = (neighbourBooster, discoFieldPosition);
            }

            IFieldCell discoBooster = _gameModel.FieldModel.Cells[discoFieldPosition._row, discoFieldPosition._column];
            IFieldCell neighbourCellBooster =
                _gameModel.FieldModel.Cells[neighbourBooster._row, neighbourBooster._column];
        
        
            if (discoBooster.Cell.itemTypeToDestroyForDiscoBooster == ItemType.NONE)
            {
                discoBooster.Cell.itemTypeToDestroyForDiscoBooster =
                    (ItemType) Random.Range(1, Enum.GetValues(typeof(ItemType)).Length - 8);
            }
        
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    if(cells[i,j].IsDropDown) continue;
                    if(!cells[i,j].IsDefaultCell) continue;
                    if(cells[i,j].Cell.ItemDataProperty == null) continue;
                    if(i == discoFieldPosition.Row && j == discoFieldPosition.Column) continue;
                
                    ItemType itemType = cells[i, j].Cell.ItemDataProperty.itemType;
                
                    if ( itemType == discoBooster.Cell.itemTypeToDestroyForDiscoBooster)
                    {
                        IFieldCell fieldCell = cells[i, j];
                        CellView cellView = fieldCell.Cell.CellViewProperty;
                        _aimController.UpdateAims(itemType, 1, fieldCell.FieldPositionProperty);
                        cellView.PlayEffect(()=>_spawnController.Despawn(cellView));
                        cells[i, j].Cell = new FieldBooster();
                        await Task.Delay(10);
                        var boosters = _spawnController.SpawnBoosterCellView(cells[i, j].FieldPositionProperty,
                            neighbourCellBooster.Cell.ItemDataProperty.itemType);
                        cells[i,j].Cell.Initialize(boosters.boosterView, boosters.boosterData, fieldCell.FieldPositionProperty._row);
                    
                        spawned.Add(cells[i,j]);
                        await Task.Delay(10);
                    }
                }
            }
        
            spawned.Add(neighbourCellBooster);
            await DespawnCell(discoBooster);
            ItemType spawnedBoosterType = neighbourCellBooster.Cell.ItemDataProperty.itemType;
            for (int i = 0; i < spawned.Count; i++)
            {
                await DespawnCell(spawned[i]);
                await CheckBoosterByType(spawnedBoosterType, spawned[i].FieldPositionProperty);
            }
            
        }

        private async Task CheckBoosterByType(ItemType spawnedBoosterType, FieldPosition fieldPosition)
        {
            if (spawnedBoosterType is ItemType.ROCKET_VERTICAL)
            {
                await ActivateRocketVertical(fieldPosition);
            }

            if (spawnedBoosterType is ItemType.ROCKET_HORIZONTAL)
            {
                await ActivateRocketHorizontal(fieldPosition);
            }

            if (spawnedBoosterType is ItemType.BOMB)
            {
                await ActivateBomb(fieldPosition);
            }
        }

        private async Task TrippleRocketCombo(FieldPosition fieldPosition, IFieldCell neighbour)
        {
            Task despawnClickCell =  DespawnCell(_gameModel.FieldModel.Cells[fieldPosition._row, fieldPosition._column]);
            Task despawnNeighbour =  DespawnCell(neighbour);
            
            await Task.WhenAll(despawnClickCell, despawnNeighbour);
            
            Task vertical = ThreeVerticalRocket(fieldPosition);
            Task horizontal = ThreeHorizontalRocket(fieldPosition);

            await Task.WhenAll(vertical, horizontal);
        }

        private async Task ThreeHorizontalRocket(FieldPosition fieldPosition)
        {
            var position = new FieldPosition(fieldPosition._row + 1, fieldPosition._column);
            List<Task> tasks = new List<Task>(3);
            if (!_gameModel.FieldModel.IsFieldPositionOutOfSize(position))
                tasks.Add(ActivateRocketHorizontal(position));
            position = new FieldPosition(fieldPosition.Row - 1, fieldPosition.Column);
            if(!_gameModel.FieldModel.IsFieldPositionOutOfSize(position))
                tasks.Add(ActivateRocketHorizontal(position));

            tasks.Add(ActivateRocketHorizontal(fieldPosition));

            await Task.WhenAll(tasks);

        }

        private async Task ThreeVerticalRocket(FieldPosition fieldPosition)
        {
            var position = new FieldPosition(fieldPosition._row, fieldPosition._column + 1);
            
            List<Task> tasks = new List<Task>(3);
            if (!_gameModel.FieldModel.IsFieldPositionOutOfSize(position))
                tasks.Add(ActivateRocketVertical(position));
            position = new FieldPosition(fieldPosition.Row, fieldPosition.Column - 1);
            if(!_gameModel.FieldModel.IsFieldPositionOutOfSize(position))
               tasks.Add(ActivateRocketVertical(position));

            tasks.Add(ActivateRocketVertical(fieldPosition));
            
            await Task.WhenAll(tasks);
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