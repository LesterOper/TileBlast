using System.Collections.Generic;
using CodeBase.CoreBase.CellBase;
using CodeBase.CoreBase.Configs;
using Unity.VisualScripting;
using UnityEngine;

namespace CodeBase.CoreBase.FieldBase
{
    public class FieldModel
    {
        public FieldSize FieldSize;
        public IFieldCell[,] Cells;

        public List<IFieldCell> GetEmptyCells()
        {
            List<IFieldCell> emptyCells = new List<IFieldCell>();
            
            for (int j = 0; j < FieldSize._column; j++)
            {
                for (int i = 0; i < FieldSize._row; i++)
                {
                    if(Cells[i,j].IsEmpty)
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
                    if (Cells[i, j].IsEmpty)
                        count++;
                }
            }

            return count;
        }

        public List<IFieldCell> GetColumnIfHasEmptyCellsElseReturnNull(int columnIndex)
        {
            List<IFieldCell> currentColumn = new List<IFieldCell>(FieldSize._row);

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

        public bool NeedToStartMoveState()
        {
            for (int i = 0; i < FieldSize._column; i++)
            {
                var column = GetColumnIfHasEmptyCellsElseReturnNull(i);
                if(column == null) continue;
                int index = column.FindLastIndex(local => local.IsBlocked);
                if (index < 0)
                {
                    return true;
                }

                return column.FindIndex(index, local => local.IsEmpty) >= 0;
            }

            return false;
        }

        public bool IsFieldPositionOutOfSize(FieldPosition fieldPosition)
        {
            return fieldPosition.Row >= FieldSize._row || fieldPosition.Row < 0 ||
                   fieldPosition.Column >= FieldSize._column || fieldPosition.Column < 0;
        }
        
        public bool IsFieldPositionOutOfSize(int row, int column)
        {
            return row >= FieldSize._row || row < 0 ||
                   column >= FieldSize._column || column < 0;
        }

        public bool IsClickedItemBooster(FieldPosition fieldPosition)
        {
            return (int) Cells[fieldPosition.Row, fieldPosition.Column].Cell.ItemDataProperty.itemType >=
                   (int) ItemType.BOMB;
        }

        public ItemType GetItemTypeByFieldPosition(FieldPosition fieldPosition) =>
            Cells[fieldPosition.Row, fieldPosition.Column].Cell.ItemDataProperty.itemType;

        public List<IFieldCell> GetFirstActiveCellsInColumnWithDropDownItems()
        {
            //IFieldCell[] firstRow = new IFieldCell[FieldSize._column];
            List<IFieldCell> firstActiveCells = new List<IFieldCell>();

            for (int j = 0; j < FieldSize._column; j++)
            {
                List<IFieldCell> cellsColumn = GetColumn(j);
                if(!cellsColumn.Exists(local => local.IsDropDown)) continue;
                
                for (int i = 0; i < FieldSize._row; i++)
                {
                    if(cellsColumn[i].IsNonActive) continue;
                    
                    firstActiveCells.Add(cellsColumn[i]);
                    break;
                }
            }
            

            return firstActiveCells;
        }
        
        public List<IFieldCell> GetColumn(int column)
        {
            List<IFieldCell> currentColumn = new List<IFieldCell>(FieldSize._row);
            for (int i = 0; i < FieldSize._row; i++)
            {
                currentColumn.Add(Cells[i, column]);
            }

            return currentColumn;
        }
    }
}