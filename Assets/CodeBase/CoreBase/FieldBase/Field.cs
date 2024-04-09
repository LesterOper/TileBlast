using System;
using System.Collections.Generic;
using System.Linq;
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

        public Field(
            FieldBoard fieldBoard,
            SpawnController spawnController,
            GameModel gameModel,
            Transform fieldParent,
            SpriteRenderer spriteRendererFieldBg)
        {
            _fieldBoard = fieldBoard;
            _spawnController = spawnController;
            _gameModel = gameModel;
            _fieldParent = fieldParent;
            _spriteRendererFieldBg = spriteRendererFieldBg;
        }

        public void Initialize()
        {
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
                    var cell = new Cell(cellView, item, fieldPosition);
                    cells[i, j] = cell;
                }
            }
            
            _spriteRendererFieldBg.transform.localPosition = new Vector3(
                Mathf.Lerp(_fieldBoard.BoardEdge.BottomLeft.x, _fieldBoard.BoardEdge.BottomRight.x, 0.5f), 
                Mathf.Lerp(_fieldBoard.WorldPosition(0, 0).y, _fieldBoard.WorldPosition(_gameModel.FieldModel.FieldSize._row - 1, 0).y, 0.5f), 0);
        }

        private IItem CreateItem(FieldPosition fieldPosition, float scale)
        {
            ItemType itemType = _gameModel.Level.levelItemsDatas
                .FirstOrDefault(local => local.FieldPosition.Equals(fieldPosition))!.itemType;
            Vector2 position = _fieldBoard.WorldPosition(fieldPosition.Row, fieldPosition.Column);
            return _spawnController.SpawnItem(fieldPosition, itemType, position,scale);
        }

        public List<MovableItemsContainer.MovableItem> GetMovableItems()
        {
            List<MovableItemsContainer.MovableItem> movableItems = new List<MovableItemsContainer.MovableItem>(10);

            List<ICell> empty = _gameModel.FieldModel.GetEmptyCells();
            List<IItem> itemsFromPool = new List<IItem>(empty.Count);

            for (int i = 0; i < empty.Count; i++)
            {
                FieldPosition fieldPosition = empty[i].FieldPositionProperty;
                Vector2 position = _fieldBoard.WorldPosition(fieldPosition.Row, fieldPosition.Column);
                IItem item = _spawnController.SpawnRandomItem(empty[i].FieldPositionProperty, position, _fieldBoard.TileSize);
                itemsFromPool.Add(item);
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
                        IItem item = currentColumn[j].ItemProperty;
                        emptyCells[0].SetItem(item);
                        currentColumn[j].SetItem(null);
                        Vector2 position = _fieldBoard.WorldPosition(emptyCells[0].FieldPositionProperty.Row,
                            emptyCells[0].FieldPositionProperty.Column);
                        MovableItemsContainer.MovableItem movableItem =
                            new MovableItemsContainer.MovableItem(item, position);
                        movableItems.Add(movableItem);
                        emptyCells.RemoveAt(0);
                        emptyCells.Add(currentColumn[j]);
                    }
                }

                for (int k = emptyCells.Count-1; k >= 0; k--)
                {
                    IItem item = itemsFromPool[0];
                    emptyCells[k].SetItem(item);
                    itemsFromPool.RemoveAt(0);
                    Vector2 position = _fieldBoard.WorldPosition(emptyCells[k].FieldPositionProperty.Row,
                        emptyCells[k].FieldPositionProperty.Column);
                    Debug.Log($"{emptyCells[k].FieldPositionProperty} - {position.ToString()}");
                    MovableItemsContainer.MovableItem movableItem =
                        new MovableItemsContainer.MovableItem(item, position);
                    movableItems.Add(movableItem);
                }
            }
            return movableItems;
        }
    }
}