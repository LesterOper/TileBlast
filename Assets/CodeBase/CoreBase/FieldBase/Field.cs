using System.Collections.Generic;
using System.Linq;
using CodeBase.CoreBase.CellBase;
using CodeBase.CoreBase.Configs;
using CodeBase.CoreBase.GameStateModel;
using CodeBase.Factory;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.CoreBase.FieldBase
{
    public interface IField
    {
        public void CreateField();
        public void Initialize();
        List<MovableItemsContainer.MovableItem> GetMovableItems();
    }

    public class Field : IField
    {
        private Transform _fieldParent;
        
        private FieldBoard _fieldBoard;
        private SpawnController _spawnController;
        private GameModel _gameModel;

        private SpriteRenderer bgEffect;
        
        private SpriteRenderer _spriteRendererFieldBg;

        public Field(
            FieldBoard fieldBoard,
            SpawnController spawnController,
            GameModel gameModel,
            Transform fieldParent,
            GameObject spriteRendererFieldBg,
            SpriteRenderer bgEffect)
        {
            _fieldBoard = fieldBoard;
            _spawnController = spawnController;
            _gameModel = gameModel;
            _fieldParent = fieldParent;
            this.bgEffect = bgEffect;
            _spriteRendererFieldBg = spriteRendererFieldBg.GetComponent<SpriteRenderer>();
        }

        public void Initialize()
        {
            CalculateFieldSize();
            InitializeFieldCellMatrix();
            _spriteRendererFieldBg.size = new Vector2(_gameModel.FieldModel.FieldSize._column * _fieldBoard.TileSize + _fieldBoard._borderDelta, _gameModel.FieldModel.FieldSize._row * _fieldBoard.TileSize + _fieldBoard._borderDelta);
            _spriteRendererFieldBg.transform.GetChild(0).localScale = new Vector2(
                _gameModel.FieldModel.FieldSize._column * _fieldBoard.TileSize + _fieldBoard._borderDelta,
                _gameModel.FieldModel.FieldSize._row * _fieldBoard.TileSize + _fieldBoard._borderDelta);

        }
        
        private void CalculateFieldSize()
        {
            _fieldBoard.Calculate(_gameModel.FieldModel.FieldSize);
        }

        private void InitializeFieldCellMatrix()
        {
            FieldSize fieldSize = _gameModel.FieldModel.FieldSize;
            for (int i = 0; i < fieldSize._row; i++)
            {
                for (int j = 0; j < fieldSize._column; j++)
                {
                    FieldPosition fieldPosition = new FieldPosition(i, j);
                    _gameModel.FieldModel.Cells[i, j] = new FieldCell(fieldPosition);
                }
            }
        }

        public void CreateField()
        {
            IFieldCell[,] cells = _gameModel.FieldModel.Cells;
            for (int i = 0; i < cells.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < cells.GetUpperBound(1) + 1; j++)
                {
                    var fieldPosition = new FieldPosition(i, j);
                    var position = _fieldBoard.WorldPosition(i, j);
                    var item = CreateItem(fieldPosition, _fieldBoard.TileSize);

                    if (item.itemType == ItemType.EMPTY)
                    {
                        cells[i, j].Cell = new NotActiveCell();
                        continue;
                    }

                    if (item.itemType == ItemType.NONE)
                    {
                        cells[i, j].Cell = new EmptyCell();
                        continue;
                    }
                    
                    var itemAttribute = item.itemType.GetAttribute<ItemAttribute>();
                    var cellView =
                        _spawnController.SpawnCell(_fieldParent.transform, position, _fieldBoard.TileSize,
                            fieldPosition);
                    if (itemAttribute._isBlock)
                    {
                        cells[i, j].Cell = new BlockedCell();
                        cells[i, j].Cell.Initialize(cellView, item);
                        continue;
                    }

                    cells[i, j].Cell = new Cell();
                    cells[i,j].Cell.Initialize(cellView, item);
                }
            }
            
            BackgroundCreator.CreateFieldBackground(_gameModel, bgEffect, _fieldBoard);
            _spriteRendererFieldBg.transform.localPosition = new Vector3(
                Mathf.Lerp(_fieldBoard.BoardEdge.BottomLeft.x, _fieldBoard.BoardEdge.BottomRight.x, 0.5f), 
                Mathf.Lerp(_fieldBoard.WorldPosition(0, 0).y, _fieldBoard.WorldPosition(_gameModel.FieldModel.FieldSize._row - 1, 0).y, 0.5f), 0);

        }

        private ItemData CreateItem(FieldPosition fieldPosition, float scale)
        {
            ItemType itemType = _gameModel.Level.levelFieldConfig.types
                .FirstOrDefault(local => local.FieldPosition.Equals(fieldPosition))!.item;

            return _spawnController.GetItemDataByType(itemType);
        }

        public List<MovableItemsContainer.MovableItem> GetMovableItems()
        {
            List<MovableItemsContainer.MovableItem> movableItems = new List<MovableItemsContainer.MovableItem>(10);

            List<IFieldCell> empty = _gameModel.FieldModel.GetEmptyCells();
            List<CellView> itemsFromPool = new List<CellView>(empty.Count);
            List<ItemData> itemsDataFromPool = new List<ItemData>(empty.Count);

            if (_gameModel.Level.levelData.levelType != LevelType.CLEAR_FIELD)
            {
                float yDelta = 1;
                float y = 0;
                for (int i = 0; i < empty.Count; i++)
                {
                    FieldPosition fieldPosition = empty[i].FieldPositionProperty;
                    Vector2 position = _fieldBoard.WorldPosition(fieldPosition.Row, fieldPosition.Column);
                    
                    var cell = _spawnController.SpawnRandomCell(empty[i].FieldPositionProperty,
                        new Vector2(position.x, y), _fieldBoard.TileSize);
                    itemsFromPool.Add(cell.Item1);
                    itemsDataFromPool.Add(cell.Item2);
                    y += yDelta;
                }
            }

            for (int i = 0; i < _gameModel.FieldModel.FieldSize._column; i++)
            {
                List<IFieldCell> currentColumn = _gameModel.FieldModel.GetColumnIfHasEmptyCellsElseReturnNull(i);
                if(currentColumn == null) continue;
                int jIndexStart = currentColumn.FindLastIndex(local => local.IsBlocked);
                
                
                List<IFieldCell> emptyCells = new List<IFieldCell>();
                for (int j = jIndexStart + 1; j < currentColumn.Count; j++)
                {
                    if (currentColumn[j].IsEmpty)
                    {
                        emptyCells.Add(currentColumn[j]);
                        continue;
                    }

                    if (emptyCells.Count > 0 && !currentColumn[j].IsNonActive)
                    {
                        IFieldCell cellBase = currentColumn[j];

                        emptyCells[0].Cell = ConvertEmptyCellToParticularType(cellBase.Cell, emptyCells[0].Cell); 
                        emptyCells[0].Cell.Initialize(cellBase.Cell.CellViewProperty, cellBase.Cell.ItemDataProperty);
                        emptyCells[0].Cell.itemTypeToDestroyForDiscoBooster = cellBase.Cell.itemTypeToDestroyForDiscoBooster;
                        
                        Vector2 position = _fieldBoard.WorldPosition(emptyCells[0].FieldPositionProperty.Row,
                            emptyCells[0].FieldPositionProperty.Column);
                        MovableItemsContainer.MovableItem movableItem =
                            new MovableItemsContainer.MovableItem(emptyCells[0].Cell.CellViewProperty, position);
                        movableItems.Add(movableItem);
                        emptyCells.RemoveAt(0);
                        var currentColumnItemFieldPos = currentColumn[j].FieldPositionProperty;
                        currentColumn[j].Cell = new EmptyCell();
                        emptyCells.Add(currentColumn[j]);
                    }
                }

                if (_gameModel.Level.levelData.levelType != LevelType.CLEAR_FIELD)
                {
                    for (int k = 0; k < emptyCells.Count; k++)
                    {
                        emptyCells[k].Cell = new Cell();
                        emptyCells[k].Cell.Initialize(itemsFromPool[0], itemsDataFromPool[0]);
                        emptyCells[k].Cell.itemTypeToDestroyForDiscoBooster = ItemType.NONE;
                        itemsFromPool.RemoveAt(0);
                        itemsDataFromPool.RemoveAt(0);
                        Vector2 position = _fieldBoard.WorldPosition(emptyCells[k].FieldPositionProperty.Row,
                            emptyCells[k].FieldPositionProperty.Column);
                        MovableItemsContainer.MovableItem movableItem =
                            new MovableItemsContainer.MovableItem(emptyCells[k].Cell.CellViewProperty, position);
                        movableItems.Add(movableItem);
                    }
                }
            }
            
            return movableItems;
        }

        private ICellBase ConvertEmptyCellToParticularType(ICellBase cellBase, ICellBase emptyCell)
        {
            var fieldPos = emptyCell.FieldPositionProperty;
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

    public static class BackgroundCreator
    {
        public static void CreateFieldBackground(GameModel gameModel, SpriteRenderer bgTile, FieldBoard fieldBoard)
        {
            IFieldCell[,] cells = gameModel.FieldModel.Cells;
            int rowCount = gameModel.FieldModel.FieldSize._row;
            int columnCount = gameModel.FieldModel.FieldSize._column;

            int count;
            for (int j = 0; j < columnCount; j++)
            {
                int startRowPos = 0;
                count = 0;
                for (int i = 0; i < rowCount; i++)
                {
                    if (cells[i, j].IsNonActive)
                    {
                        if (count > 0)
                        {
                            Vector3 position = Vector3.Lerp(fieldBoard.WorldPosition(startRowPos, j), fieldBoard.WorldPosition(startRowPos+count - 1, j), 0.5f);
                            position.z = 0;
                            SpriteRenderer bgTileInst = Object.Instantiate(bgTile);
                            bgTileInst.transform.position = position;
                            bgTileInst.size = new Vector2(fieldBoard.TileSize + 0.2f, 0.2f + fieldBoard.TileSize * count);
                        }

                        startRowPos = i + 1;
                        count = 0;
                        continue;
                    }
                    
                    count++;
                }

                if (count > 0)
                {
                    Vector3 position = Vector3.Lerp(fieldBoard.WorldPosition(startRowPos, j), fieldBoard.WorldPosition(startRowPos+count - 1, j), 0.5f);
                    position.z = 0;
                    SpriteRenderer bgTileInst = Object.Instantiate(bgTile);
                    bgTileInst.transform.position = position;
                    bgTileInst.size = new Vector2(fieldBoard.TileSize + 0.2f, (fieldBoard.TileSize * count) + 0.2f);
                }
            }
        }
        
        
    }
}