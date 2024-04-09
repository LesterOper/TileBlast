using CodeBase.CoreBase.CellBase;
using UnityEngine;

namespace CodeBase.Factory
{
    public sealed class CellFactory
    {
        private CellView _cellPrefab;

        public CellFactory(CellView cellPrefab)
        {
            _cellPrefab = cellPrefab;
            
            if(_cellPrefab != null)
                Debug.Log($"CellPrefab injected");
        }

        public CellView CreateInstance(Transform parent)
        {
            CellView instance = Object.Instantiate(_cellPrefab, parent);
            return instance;
        }
    }
}