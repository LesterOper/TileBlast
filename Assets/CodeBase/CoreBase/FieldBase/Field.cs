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
                    
                    var cellView =
                        _spawnController.SpawnCell(_fieldParent.transform, position, _fieldBoard.TileSize,
                            fieldPosition);
                    if (item.isBlock)
                    {
                        cells[i, j].Cell = new BlockedCell();
                        cells[i, j].Cell.Initialize(cellView, item, i);
                        continue;
                    }

                    if (item.isBooster)
                    {
                        cells[i, j].Cell = new FieldBooster();
                        cells[i,j].Cell.Initialize(cellView, item, i);
                        continue;
                    }

                    if (item.isDropBottomItem)
                    {
                        cells[i, j].Cell = new DropDownItem();
                        cells[i,j].Cell.Initialize(cellView, item, i);
                        continue;
                    }

                    cells[i, j].Cell = new Cell();
                    cells[i,j].Cell.Initialize(cellView, item, i);
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
                            bgTileInst.transform.position = new Vector3(position.x, position.y + 0.02f);
                            bgTileInst.size = new Vector2(fieldBoard.TileSize + 0.2f, 0.3f + fieldBoard.TileSize * count);
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
                    bgTileInst.transform.position = new Vector3(position.x, position.y + 0.02f);
                    bgTileInst.size = new Vector2(fieldBoard.TileSize + 0.2f, (fieldBoard.TileSize * count) + 0.3f);
                }
            }
        }
        
        
    }
}