using System;
using UnityEngine;

namespace CodeBase.CoreBase.FieldBase
{
    public sealed class FieldBoard
    {
        private BoardEdge _boardEdge;
        public BoardEdge BoardEdge => _boardEdge;
        
        public float TileSize { get; private set; }
        public float TileScale { get; private set; }

        public Vector2 OriginalPosition { get; private set; }

        private readonly float _spriteOriginal;
        private const float PixelPerfectPerUnit = 110f;
        public float _borderDelta;
        private Camera _camera;
        

        public FieldBoard(FieldSettings fieldSettings, Camera camera)
        {
            _camera = camera;
            var fieldSettings1 = fieldSettings;
            _spriteOriginal = fieldSettings1.tileOriginalSize / PixelPerfectPerUnit;
            _borderDelta = fieldSettings.borderDelta;
            _boardEdge = new BoardEdge(fieldSettings1.top, fieldSettings1.bottom, fieldSettings1.edge, _borderDelta, _camera);
        }

        public void Calculate(FieldSize fieldSize)
        {
            float totalWidth = _spriteOriginal * fieldSize._column;
            float totalHeight = _spriteOriginal * fieldSize._row;

            float scaleX = _boardEdge.BottomSize / totalWidth;
            float scaleY = _boardEdge.TopSize / totalHeight;
            
            TileScale = scaleX < scaleY ? scaleX : scaleY;

            var countOnBottom = _boardEdge.BottomSize / scaleY;
            TileScale *= 1 - fieldSize._column / countOnBottom;
            //TileScale *= 0.8f;
            /*if (scaleY < scaleX)
            {
                var k = (int)(Math.Ceiling(_boardEdge.BottomSize * scaleY));
                if (k <= fieldSize._column)
                {
                    TileScale = scaleY;
                }
                else
                {
                    TileScale = scaleY * ((float)fieldSize._column / k );
                }
            }*/
            
            
            TileSize = _spriteOriginal * TileScale;

            float offsetX = scaleY < scaleX ? (_boardEdge.BottomSize - TileSize * fieldSize._column) * 0.5f : 0;
            
            float startX = _boardEdge.BottomLeft.x + TileSize * 0.5f + offsetX;
            float startY = _boardEdge.BottomLeft.y + TileSize * 0.5f;

            OriginalPosition = new Vector2(startX, startY);
        }

        public void Calc(FieldSize fieldSize)
        {
            float totalWidth = fieldSize._column;
            float totalHeight = fieldSize._row;

            float scaleX = _boardEdge.BottomSize / totalWidth;
            float scaleY = _boardEdge.TopSize / totalHeight;
            
            if (scaleX > _spriteOriginal)
            {
                if (scaleY <= _spriteOriginal)
                {
                    float delta = _boardEdge.BottomSize / scaleY;

                    float percent = fieldSize._column / delta;

                    TileScale = scaleX * percent;
                    TileSize = _spriteOriginal * TileScale;
                }
            }
            
            float offsetX = scaleY < scaleX ? (_boardEdge.BottomSize - TileSize * fieldSize._column) * 0.5f : 0;
            
            float startX = _boardEdge.BottomLeft.x + TileSize * 0.5f + offsetX;
            float startY = _boardEdge.BottomLeft.y + TileSize * 0.5f;

            OriginalPosition = new Vector2(startX, startY);
        }

        public Vector3 WorldPosition(int rowIndex, int columnIndex) =>
            new(OriginalPosition.x + columnIndex * TileSize, OriginalPosition.y + rowIndex * TileSize, 0);

        public Vector3 WorldPosition(FieldPosition fieldPosition)
        {
            return new(OriginalPosition.x + fieldPosition.Column * TileSize, OriginalPosition.y + fieldPosition.Row * TileSize, 0);
        }
    }
    
    public struct BoardEdge
    {
        public Vector3 BottomLeft;
        public Vector3 BottomRight;
        public Vector3 TopLeft;
        public Vector3 TopRight;

        public BoardEdge(float top, float bottom, float edge, float borferDelta, Camera camera)
        {
            BottomLeft = camera.ViewportToWorldPoint(new Vector3(edge , bottom));
            BottomRight = camera.ViewportToWorldPoint(new Vector3(1 - edge, bottom));
            TopLeft = camera.ViewportToWorldPoint(new Vector3(edge , top));
            TopRight = camera.ViewportToWorldPoint(new Vector3(1- edge, top));
        }

        public float BottomSize => BottomRight.x - BottomLeft.x;
        public float TopSize => TopLeft.y - BottomLeft.y;
    }

    [Serializable]
    public struct FieldSize
    {
        public int _row;
        public int _column;

        public FieldSize(int row, int column)
        {
            _row = row;
            _column = column;
        }
    }
}