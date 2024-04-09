using CodeBase.CoreBase.Configs;
using CodeBase.CoreBase.Item;
using CodeBase.CoreBase.ItemBase;

namespace CodeBase.CoreBase.CellBase
{
    public interface IAnimate
    {
        public void ShakeAnimation();
    }
    public interface ICell
    {
        public ItemType ItemType { get; }
        public IItem ItemProperty { get; }
        public bool IsCellEmpty { get; }
        
        public bool IsTargetForMoving { get; set; }
        public FieldPosition FieldPositionProperty { get; }
        public void ClearItem();
        public void SetItem(IItem item);
    }
}