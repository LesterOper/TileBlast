using CodeBase.CoreBase;
using CodeBase.CoreBase.CellBase;
using CodeBase.CoreBase.FieldBase;
using CodeBase.CoreBase.Item;
using UnityEngine;
using Zenject;

namespace CodeBase.Pool
{
    public class ItemPool : MonoMemoryPool<FieldPosition, CellView>
    {
        protected override void Reinitialize(FieldPosition fieldPosition,CellView item)
        {
            base.Reinitialize(fieldPosition, item);
        }

        protected override void OnDespawned(CellView item)
        {
            item.CellTransform.localPosition = Vector3.zero;
            base.OnDespawned(item);
            item.FadeCell(1, 0);
        }
    }
}