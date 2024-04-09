using CodeBase.CoreBase;
using CodeBase.CoreBase.FieldBase;
using CodeBase.CoreBase.Item;
using UnityEngine;
using Zenject;

namespace CodeBase.Pool
{
    public class ItemPool : MonoMemoryPool<FieldPosition, ItemView>
    {
        protected override void Reinitialize(FieldPosition fieldPosition,ItemView item)
        {
            base.Reinitialize(fieldPosition, item);
        }
    }
}