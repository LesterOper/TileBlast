using CodeBase.CoreBase.Configs;

namespace CodeBase.CoreBase.CellBase
{
    public class Cell : CellBaseBase, ICell
    {
        public Cell() : base()
        {
        }
    }

    public class BlockedCell : CellBaseBase, IBlockerCell
    {
        public BlockedCell() : base() {}
    }

    public class EmptyCell : CellBaseBase, IEmptyCell
    {
        public EmptyCell() : base()
        {
        }
    }
    
    public class FieldBooster : CellBaseBase, IBooster
    {
        public ItemType discoTargetItemType { get; set; }
        
        public FieldBooster() : base()
        {
            
        }
    }

    public class DropDownItem : CellBaseBase, IDropDownItem
    {
        public DropDownItem() : base()
        {
        }
    }
    
    public class NotActiveCell : CellBaseBase, INotActiveCell
    {
        public NotActiveCell() : base()
        {
        }
    }
}