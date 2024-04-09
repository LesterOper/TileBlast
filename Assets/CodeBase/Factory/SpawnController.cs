using System;
using CodeBase.CoreBase;
using CodeBase.CoreBase.CellBase;
using CodeBase.CoreBase.Configs;
using CodeBase.CoreBase.FieldBase;
using CodeBase.CoreBase.Item;
using CodeBase.CoreBase.ItemBase;
using CodeBase.Pool;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.Factory
{
    public class SpawnController
    {
        private CellFactory _cellFactory;
        private ItemsConfig _itemsConfig;
        private ItemPool _itemPool;

        public SpawnController(
            CellFactory cellFactory, 
            ItemsConfig itemsConfig,
            ItemPool itemPool,
            FieldBoard fieldBoard)
        {
            _cellFactory = cellFactory;
            _itemsConfig = itemsConfig;
            _itemPool = itemPool;
        }

        public CellView SpawnCell(Transform parent, Vector3 position, float scale, FieldPosition fieldPosition)
        {
            CellView cellView = _cellFactory.CreateInstance(parent);
            cellView.transform.position = position;
            cellView.CellTransform.localScale = Vector3.one * scale;
            return cellView;
        }

        public IItem SpawnItem(FieldPosition fieldPosition, ItemType itemType, Vector2 position, float scale)
        {
            ItemView itemView = _itemPool.Spawn(fieldPosition);
            itemView.transform.position = position;
            itemView.transform.localScale *= scale;
            ItemData itemData = _itemsConfig.GetItem(itemType);
            return new Item(itemView, itemData);
        }
        
        public IItem SpawnRandomItem(FieldPosition fieldPosition, Vector2 position, float scale)
        {
            ItemView itemView = _itemPool.Spawn(fieldPosition);
            itemView.transform.localPosition = position;
            itemView.transform.localScale =Vector3.one * 0.2f * scale;
            ItemData itemData = GetNewItemData();
            return new Item(itemView, itemData);
        }

        public ItemData GetNewItemData()
        {
            return _itemsConfig.GetItem((ItemType) Random.Range(1, Enum.GetValues(typeof(ItemType)).Length));
        }
    }
}