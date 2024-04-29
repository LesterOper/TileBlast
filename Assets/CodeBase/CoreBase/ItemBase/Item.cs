using CodeBase.CoreBase.CellBase;
using CodeBase.CoreBase.Configs;
using CodeBase.CoreBase.Item;
using UnityEngine;

namespace CodeBase.CoreBase.ItemBase
{
    public class Item : IItem, IAnimate
    {
        private readonly ItemView _itemView;
        private readonly ItemData _itemData;

        public Item(ItemView itemView, ItemData itemData)
        {
            _itemView = itemView;
            _itemData = itemData;
            //_itemView.Initialize(itemData);
        }

        public ItemType ItemTypeProperty => _itemData.itemType;
        public ItemView ItemView => _itemView;
        public Sprite CurrentCellColor => _itemData.cellColor;

        public void MoveToZeroPositionOfParent(Vector2 target, ICellBase targetCellBase, ICellBase currentCellBase) => 
            _itemView.MoveToTargetCell(target,() => { });

        public void ShakeAnimation() => _itemView.ShakeAnimation();
    }
}