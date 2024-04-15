using System;
using CodeBase.CoreBase.Configs;
using CodeBase.CoreBase.Item;
using CodeBase.CoreBase.ItemBase;

namespace CodeBase.CoreBase.CellBase
{
    public interface IAnimate
    {
        //public void ShakeAnimation();
    }
    public interface ICell
    {
        public void Initialize(CellView cellView, ItemData item);

        public void BlockCell();

        public bool IsCellEmpty { get; }
        
        public CellView CellViewProperty { get; }
        
        public FieldPosition FieldPositionProperty { get; }
        public void ClearCell();

        public void PlayEffect(Action callback);
        
        public ItemType itemTypeToDestroyForDiscoBooster { get; set; }

        public bool IsBooster();
        
        public ItemData ItemDataProperty { get; }
        
        public bool IsCellBlocked { get; }
    }
}