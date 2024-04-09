using CodeBase.CoreBase.Configs;
using CodeBase.CoreBase.ItemBase;

namespace CodeBase.CoreBase.CellBase
{
    public class Cell : ICell, IAnimate
    {
        private readonly CellView _cellView;
        private IItem _item;
        public IItem ItemProperty => _item;
        public bool IsTargetForMoving { get; set; }
        public FieldPosition FieldPositionProperty { get; }
        public ItemType ItemType => _item?.ItemTypeProperty ?? ItemType.NONE;
        public bool IsCellEmpty => _item == null;
        public Cell(CellView cellView, IItem item, FieldPosition fieldPosition)
        {
            _cellView = cellView;
            _item = item;
            FieldPositionProperty = fieldPosition;
            
            PaintCell(0);
        }
        
        public void ClearItem()
        {
            _item = null;
        }

        public void SetItem(IItem item)
        {
            _item = item;
            PaintCell(0.2f);
        }

        private void PaintCell(float duration)
        {
            if(_item != null)
                _cellView.PaintCell(_item.CurrentCellColor);
        }

        public void ShakeAnimation()
        {
            var animateComponent = _item as IAnimate;
            animateComponent?.ShakeAnimation();
        }
    }
}