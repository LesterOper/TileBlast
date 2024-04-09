using System;
using Sirenix.OdinInspector;

namespace CodeBase.CoreBase
{
    [Serializable]
    public class FieldPosition
    {
        [ShowInInspector] [ReadOnly]
        private int _row;

        public int Row => _row;

        public int Column => _column;

        [ShowInInspector] [ReadOnly]        
        private int _column;

        public FieldPosition(int row, int column)
        {
            _row = row;
            _column = column;
        }

        public override string ToString()
        {
            return $"[{_row},{_column}]";
        }

        public bool Equals(FieldPosition fieldPosition)
        {
            return fieldPosition._row == _row && fieldPosition._column == _column;
        }
    }
}