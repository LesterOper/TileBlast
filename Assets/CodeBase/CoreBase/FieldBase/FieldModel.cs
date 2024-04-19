using System.Collections.Generic;
using CodeBase.CoreBase.CellBase;
using CodeBase.CoreBase.Configs;

namespace CodeBase.CoreBase.FieldBase
{
    public class FieldModel
    {
        public FieldSize FieldSize;
        public ICell[,] Cells;
        public int activeCellsCount = 0;

        public List<ICell> GetEmptyCells()
        {
            List<ICell> emptyCells = new List<ICell>();
            
            for (int j = 0; j < FieldSize._column; j++)
            {
                for (int i = 0; i < FieldSize._row; i++)
                {
                    if(Cells[i,j].IsCellEmpty && !Cells[i,j].IsCellBlocked)
                        emptyCells.Add(Cells[i,j]);
                }
            }

            return emptyCells;
        }

        public int GetEmptyCellsCount()
        {
            int count = 0;
            for (int j = 0; j < FieldSize._column; j++)
            {
                for (int i = 0; i < FieldSize._row; i++)
                {
                    if (Cells[i, j].IsCellEmpty && !Cells[i,j].IsCellBlocked)
                        count++;
                }
            }

            return count;
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

        public bool IsClickedItemBooster(FieldPosition fieldPosition)
        {
            return (int) Cells[fieldPosition.Row, fieldPosition.Column].ItemDataProperty.itemType >=
                   (int) ItemType.BOMB;
        }

        public ItemType GetItemTypeByFieldPosition(FieldPosition fieldPosition) =>
            Cells[fieldPosition.Row, fieldPosition.Column].ItemDataProperty.itemType;

        public ICell this[FieldPosition fieldPosition] => Cells[fieldPosition.Row, fieldPosition.Column];

        public void SetActiveCellsCount() // TODO ADD IF-STATEMENT WHEN FIELD WILL BE NOT SQUERE MATRIX
        {
            for (int i = 0; i < FieldSize._row; i++)
            {
                for (int j = 0; j < FieldSize._column; j++)
                {
                    if (Cells[i, j] != null)
                    {
                        activeCellsCount++;
                    }
                }
            }
        }

        public ICell[] GetFirstRow()
        {
            ICell[] firstRow = new ICell[FieldSize._column];

            for (int column = 0; column < FieldSize._column; column++)
            {
                firstRow[column] = Cells[0, column];
            }

            return firstRow;
        }

    }
}