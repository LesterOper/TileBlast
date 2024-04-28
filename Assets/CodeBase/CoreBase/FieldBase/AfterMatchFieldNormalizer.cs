using System.Collections.Generic;
using System.Linq;
using CodeBase.CoreBase.CellBase;
using CodeBase.CoreBase.Configs;
using CodeBase.CoreBase.GameStateModel;
using CodeBase.Factory;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace CodeBase.CoreBase.FieldBase
{
    public class AfterMatchFieldNormalizer
    {
        private GameModel _gameModel;
        private SpawnController _spawnController;
        private FieldBoard _fieldBoard;

        public AfterMatchFieldNormalizer(GameModel gameModel, SpawnController spawnController, FieldBoard fieldBoard)
        {
            _gameModel = gameModel;
            _spawnController = spawnController;
            _fieldBoard = fieldBoard;
        }

        public List<MovableItemsContainer.MovableItem> GetMovableItems()
        {
            List<MovableItemsContainer.MovableItem> movableItems = new List<MovableItemsContainer.MovableItem>(10);
            List<IFieldCell> empty = new List<IFieldCell>();

            IFieldCell previous = null;
            IFieldCell current = null;
            for (int j = 0; j < _gameModel.FieldModel.FieldSize._column; j++)
            {
                List<IFieldCell> fieldCells = _gameModel.FieldModel.GetColumn(j);
                
                if (fieldCells.Exists(local => local.IsEmpty))
                {
                    for (int i = 0; i < _gameModel.FieldModel.FieldSize._row; i++)
                    {
                        current = _gameModel.FieldModel.Cells[i, j];
                        if(current.IsNonActive) continue;
                        if (current.IsBlocked)
                        {
                            empty.Clear();
                            continue;
                        }
                        
                        if (current.IsEmpty)
                        {
                            empty.Add(current);
                            continue;
                        }

                        if (empty.Count > 0)
                        {
                            ItemData itemData = current.Cell.ItemDataProperty;
                            CellView cellView = current.Cell.CellViewProperty;
                            empty[0].Cell = ConvertEmptyCellToParticularType(current.Cell, empty[0].Cell);
                            empty[0].Cell.Initialize(cellView, itemData);
                            empty[0].Cell.itemTypeToDestroyForDiscoBooster =
                                current.Cell.itemTypeToDestroyForDiscoBooster;
                            Vector2 worldPosition = _fieldBoard.WorldPosition(empty[0].FieldPositionProperty);
                            MovableItemsContainer.MovableItem movableItem =
                                new MovableItemsContainer.MovableItem(empty[0].Cell.CellViewProperty, worldPosition);
                            movableItems.Add(movableItem);
                            current.Cell = new EmptyCell();
                            empty.RemoveAt(0);
                            empty.Add(current);
                        }
                    }

                    if (empty.Count > 0)
                    {
                        for (int i = 0; i < empty.Count; i++)
                        {
                            FieldPosition fieldPosition = empty[i].FieldPositionProperty;
                            Vector2 worldPosition = _fieldBoard.WorldPosition(fieldPosition);
                            var cell = _spawnController.SpawnRandomCell(fieldPosition, new Vector2(worldPosition.x, 0),
                                _fieldBoard.TileSize);
                            empty[i].Cell = new Cell();
                            empty[i].Cell.Initialize(cell.Item1, cell.Item2);
                            empty[i].Cell.itemTypeToDestroyForDiscoBooster = ItemType.NONE;
                            MovableItemsContainer.MovableItem movableItem =
                                new MovableItemsContainer.MovableItem(empty[i].Cell.CellViewProperty, worldPosition);
                            movableItems.Add(movableItem);
                            //empty.RemoveAt(0);
                        }
                    }
                }
                empty.Clear();
            }


            return movableItems;
        }

        private ICellBase ConvertEmptyCellToParticularType(ICellBase cellBase, ICellBase emptyCell)
        {
            if (cellBase is IBooster)
            {
                emptyCell = new FieldBooster();
            }
            else if (cellBase is IDropDownItem)
            {
                emptyCell = new DropDownItem();
            }
            else if (cellBase is ICell)
            {
                emptyCell = new Cell();
            }
            else if (cellBase is IBlockerCell)
            {
                emptyCell = new BlockedCell();
            }

            return emptyCell;
        }
    }
}