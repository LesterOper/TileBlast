using System;
using CodeBase.CoreBase.Configs;

namespace CodeBase.CoreBase.CellBase
{
    public abstract class CellBaseBase : ICellBase
    {
        private CellView _cellView;
        private ItemData _itemData;
        private FieldPosition _fieldPosition;

        public CellView CellViewProperty => _cellView;
        public FieldPosition FieldPositionProperty => _fieldPosition;


        public CellBaseBase()
        {
        }
        
        
        public void Initialize(CellView cellView, ItemData item, int layer)
        {
            _cellView = cellView;
            _itemData = item;
            _cellView.Initialize(item.cellColor, item.item, item.color, layer);
        }

        public void BlockCell()
        {
            
        }

        public bool IsCellEmpty { get; }


        public void ClearCell()
        {
            
        }

        public void PlayEffect(Action callback)
        {
            
        }

        public ItemType itemTypeToDestroyForDiscoBooster { get; set; }
        public bool IsBooster()
        {
            return true;
        }

        public ItemData ItemDataProperty => _itemData;
        public bool IsCellBlocked { get; }
    }
}