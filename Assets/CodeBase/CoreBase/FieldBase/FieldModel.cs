using System.Collections.Generic;
using CodeBase.CoreBase.CellBase;

namespace CodeBase.CoreBase.FieldBase
{
    public class FieldModel
    {
        public FieldSize FieldSize;
        public ICell[,] Cells;

        public List<ICell> GetEmptyCells()
        {
            List<ICell> emptyCells = new List<ICell>();
            
            for (int j = 0; j < FieldSize._column; j++)
            {
                for (int i = 0; i < FieldSize._row; i++)
                {
                    if(Cells[i,j].IsCellEmpty)
                        emptyCells.Add(Cells[i,j]);
                }
            }

            return emptyCells;
        }

        public List<ICell> GetColumnIfHasEmptyCellsElseReturnNull(int columnIndex)
        {
            List<ICell> currentColumn = new List<ICell>(FieldSize._row);

            var emptyCells = GetEmptyCells();

            if (emptyCells.Exists(local => local.FieldPositionProperty.Column == columnIndex))
            {
                for (int i = 0; i < FieldSize._row; i++)
                {
                    currentColumn.Add(Cells[i, columnIndex]);
                }

                return currentColumn;
            }

            return null;
        }

        public bool IsFieldPositionOutOfSize(FieldPosition fieldPosition)
        {
            return fieldPosition.Row >= FieldSize._row || fieldPosition.Row < 0 ||
                   fieldPosition.Column >= FieldSize._column || fieldPosition.Column < 0;
        }
        
    }
}