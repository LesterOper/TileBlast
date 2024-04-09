using CodeBase.CoreBase.CellBase;
using CodeBase.CoreBase.Configs;
using CodeBase.CoreBase.Item;
using UnityEngine;

namespace CodeBase.CoreBase.ItemBase
{
    public interface IItem
    {
        public ItemType ItemTypeProperty { get;  } 
        public ItemView ItemView { get; }
        
        public Sprite CurrentCellColor { get; }
        
        public void MoveToZeroPositionOfParent(Vector2 target, ICell targetCell, ICell currentCell);
    }
}