using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.CoreBase.AimTopBar;
using CodeBase.CoreBase.CellBase;
using CodeBase.CoreBase.Configs;
using CodeBase.CoreBase.GameStateModel;
using CodeBase.CoreBase.ItemBase;
using CodeBase.Factory;
using UnityEngine;

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
        private SpriteRenderer _spriteRendererFieldBg;
        
        private FieldBoard _fieldBoard;
        private SpawnController _spawnController;
        private GameModel _gameModel;
        private AimController _aimController;

        public Field(
            FieldBoard fieldBoard,
            SpawnController spawnController,
            GameModel gameModel,
            Transform fieldParent,
            SpriteRenderer spriteRendererFieldBg,
            AimController aimController)
        {
            _fieldBoard = fieldBoard;
            _spawnController = spawnController;
            _gameModel = gameModel;
            _fieldParent = fieldParent;
            _spriteRendererFieldBg = spriteRendererFieldBg;
            _aimController = aimController;
        }

        public void Initialize()
        {
            InitializeFieldCellMatrix();
            CalculateFieldSize();
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
                    _gameModel.FieldModel.Cells[i, j] = new Cell(fieldPosition);
                }
            }
        }

        public void CreateField()
        {
            ICell[,] cells = _gameModel.FieldModel.Cells;
            for (int i = 0; i < cells.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < cells.GetUpperBound(1) + 1; j++)
                {
                    var position = _fieldBoard.WorldPosition(i, j);
                    var fieldPosition = new FieldPosition(i, j);
                    var cellView =
                        _spawnController.SpawnCell(_fieldParent.transform, position, _fieldBoard.TileSize, fieldPosition);
                    var item = CreateItem(fieldPosition, _fieldBoard.TileSize);
                    
                    cells[i, j].Initialize(cellView, item);
                }
            }
            
            _spriteRendererFieldBg.transform.localPosition = new Vector3(
                Mathf.Lerp(_fieldBoard.BoardEdge.BottomLeft.x, _fieldBoard.BoardEdge.BottomRight.x, 0.5f), 
                Mathf.Lerp(_fieldBoard.WorldPosition(0, 0).y, _fieldBoard.WorldPosition(_gameModel.FieldModel.FieldSize._row - 1, 0).y, 0.5f), 0);
        }

        private ItemData CreateItem(FieldPosition fieldPosition, float scale)
        {
            ItemType itemType = _gameModel.Level.levelItemsDatas
                .FirstOrDefault(local => local.FieldPosition.Equals(fieldPosition))!.itemType;

            return _spawnController.GetItemDataByType(itemType);
        }

        public List<MovableItemsContainer.MovableItem> GetMovableItems()
        {
            List<MovableItemsContainer.MovableItem> movableItems = new List<MovableItemsContainer.MovableItem>(10);

            List<ICell> empty = _gameModel.FieldModel.GetEmptyCells();
            List<CellView> itemsFromPool = new List<CellView>(empty.Count);
            List<ItemData> itemsDataFromPool = new List<ItemData>(empty.Count);

            if (_gameModel.Level.levelType != LevelType.CLEAR_FIELD)
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
                List<ICell> currentColumn = _gameModel.FieldModel.GetColumnIfHasEmptyCellsElseReturnNull(i);
                if(currentColumn == null) continue;

                List<ICell> emptyCells = new List<ICell>();
                for (int j = 0; j < currentColumn.Count; j++)
                {
                    if (currentColumn[j].IsCellEmpty)
                    {
                        emptyCells.Add(currentColumn[j]);
                        continue;
                    }

                    if (emptyCells.Count > 0)
                    {
                        ICell cell = currentColumn[j];
                        emptyCells[0].Initialize(cell.CellViewProperty, cell.ItemDataProperty);
                        emptyCells[0].itemTypeToDestroyForDiscoBooster = cell.itemTypeToDestroyForDiscoBooster;
                        cell.ClearCell();
                        Vector2 position = _fieldBoard.WorldPosition(emptyCells[0].FieldPositionProperty.Row,
                            emptyCells[0].FieldPositionProperty.Column);
                        MovableItemsContainer.MovableItem movableItem =
                            new MovableItemsContainer.MovableItem(emptyCells[0].CellViewProperty, position);
                        movableItems.Add(movableItem);
                        emptyCells.RemoveAt(0);
                        emptyCells.Add(currentColumn[j]);
                    }
                }

                if (_gameModel.Level.levelType != LevelType.CLEAR_FIELD)
                {
                    for (int k = 0; k < emptyCells.Count; k++)
                    {
                        emptyCells[k].Initialize(itemsFromPool[0], itemsDataFromPool[0]);
                        emptyCells[k].itemTypeToDestroyForDiscoBooster = ItemType.NONE;
                        itemsFromPool.RemoveAt(0);
                        itemsDataFromPool.RemoveAt(0);
                        Vector2 position = _fieldBoard.WorldPosition(emptyCells[k].FieldPositionProperty.Row,
                            emptyCells[k].FieldPositionProperty.Column);
                        MovableItemsContainer.MovableItem movableItem =
                            new MovableItemsContainer.MovableItem(emptyCells[k].CellViewProperty, position);
                        movableItems.Add(movableItem);
                    }
                }
            }
            
            return movableItems;
        }
    }
}