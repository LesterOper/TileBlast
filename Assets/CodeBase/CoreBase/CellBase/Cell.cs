using System;
using CodeBase.CoreBase.Configs;
using CodeBase.CoreBase.ItemBase;

namespace CodeBase.CoreBase.CellBase
{
    public class Cell : ICell, IAnimate
    {
        private CellView _cellView;

        private ItemData _itemData;

        private bool isCellBlocked;

        public bool IsCellBlocked => isCellBlocked;


        public void BlockCell()
        {
            isCellBlocked = true;
        }

        public bool IsCellEmpty => _cellView == null;

        public CellView CellViewProperty => _cellView;
        
        public FieldPosition FieldPositionProperty { get; }

        public void ClearCell()
        {
            _cellView = null;
            _itemData = null;
        }

        public void PlayEffect(Action callback)
        {
            
        }

        public ItemType itemTypeToDestroyForDiscoBooster { get; set; }
        public bool IsBooster()
        {
            return !IsCellEmpty && _itemData.IsBooster;
        }

        public ItemData ItemDataProperty => _itemData;

        public Cell(FieldPosition fieldPosition)
        {
            FieldPositionProperty = fieldPosition;
        }

        public void Initialize(CellView cellView, ItemData item)
        {
            _cellView = cellView;
            _itemData = item;
            _cellView.Initialize(_itemData.cellColor, _itemData.item);
            isCellBlocked = false;
        }
    }
}