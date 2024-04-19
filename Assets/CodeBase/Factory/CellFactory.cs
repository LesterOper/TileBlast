using CodeBase.CoreBase.CellBase;
using CodeBase.CoreBase.Item;
using UnityEngine;

namespace CodeBase.Factory
{
    public sealed class CellFactory
    {
        private CellView _cellPrefab;
        private ItemView _itemView;

        public CellFactory(CellView cellPrefab, ItemView itemView)
        {
            _cellPrefab = cellPrefab;
            _itemView = itemView;
            
            if(_cellPrefab != null)
                Debug.Log($"CellPrefab injected");
        }

        public CellView CreateInstance(Transform parent)
        {
            CellView instance = Object.Instantiate(_cellPrefab, parent);
            return instance;
        }

        public ItemView CreateInstance(Vector3 position)
        {
            ItemView itemView = Object.Instantiate(_itemView, position, Quaternion.identity);
            return itemView;
        }
    }
}