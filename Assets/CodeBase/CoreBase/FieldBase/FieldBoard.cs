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
        private const float PixelPerfectPerUnit = 100f;
        public float _borderDelta;
        

        public FieldBoard(FieldSettings fieldSettings)
        {
            var fieldSettings1 = fieldSettings;
            _spriteOriginal = fieldSettings1.tileOriginalSize / PixelPerfectPerUnit;
            _boardEdge = new BoardEdge(fieldSettings1.top, fieldSettings1.bottom, fieldSettings1.edge);
            _borderDelta = fieldSettings.borderDelta;
        }

        public void Calculate(FieldSize fieldSize)
        {
            float totalWidth = _spriteOriginal * fieldSize._column;
            float totalHeight = _spriteOriginal * fieldSize._row;

            float scaleX = _boardEdge.BottomSize / totalWidth;
            float scaleY = _boardEdge.TopSize / totalHeight;
            
            TileScale = scaleX < scaleY ? scaleX : scaleY;
            TileSize = _spriteOriginal * TileScale;

            float offsetX = scaleY < scaleX ? (_boardEdge.BottomSize - TileSize * fieldSize._column) * 0.5f : 0;
            
            float startX = _boardEdge.BottomLeft.x + TileSize * 0.5f + offsetX;
            float startY = _boardEdge.BottomLeft.y + TileSize * 0.5f;

            OriginalPosition = new Vector2(startX, startY);
        }

        public Vector3 WorldPosition(int rowIndex, int columnIndex) =>
            new(OriginalPosition.x + columnIndex * TileSize, OriginalPosition.y + rowIndex * TileSize, 0);
    }
    
    public struct BoardEdge
    {
        public Vector3 BottomLeft;
        public Vector3 BottomRight;
        public Vector3 TopLeft;
        public Vector3 TopRight;

        public BoardEdge(float top, float bottom, float edge)
        {
            BottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(edge, bottom));
            BottomRight = Camera.main.ViewportToWorldPoint(new Vector3(1 - edge, bottom));
            TopLeft = Camera.main.ViewportToWorldPoint(new Vector3(edge, top));
            TopRight = Camera.main.ViewportToWorldPoint(new Vector3(1- edge, top));
        }

        public float BottomSize => BottomRight.x - BottomLeft.x;
        public float TopSize => TopLeft.y - BottomRight.y;
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