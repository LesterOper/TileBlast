using System;
using CodeBase.CoreBase;
using CodeBase.CoreBase.CellBase;
using CodeBase.CoreBase.Configs;
using CodeBase.CoreBase.FieldBase;
using CodeBase.CoreBase.GameStateModel;
using CodeBase.CoreBase.Item;
using CodeBase.Pool;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace CodeBase.Factory
{
    public class SpawnController
    {
        private CellFactory _cellFactory;
        private ItemsConfig _itemsConfig;
        private ItemPool _itemPool;
        private FieldBoard _fieldBoard;
        private VfxConfig _vfxConfig;
        private GameModel _gameModel;

        public SpawnController(
            CellFactory cellFactory, 
            ItemsConfig itemsConfig,
            ItemPool itemPool,
            FieldBoard fieldBoard,
            VfxConfig vfxConfig,
            GameModel gameModel)
        {
            _cellFactory = cellFactory;
            _itemsConfig = itemsConfig;
            _itemPool = itemPool;
            _fieldBoard = fieldBoard;
            _vfxConfig = vfxConfig;
            _gameModel = gameModel;
        }

        public CellView SpawnCell(Transform parent, Vector3 position, float scale, FieldPosition fieldPosition)
        {
            CellView cellView = _itemPool.Spawn(fieldPosition);
            cellView.transform.position = position;
            cellView.CellTransform.localScale = Vector3.one * scale;
            return cellView;
        }

        public ItemData GetItemDataByType(ItemType itemType)
        {
            return _itemsConfig.GetItem(itemType);
        }
        
        public (CellView, ItemData) SpawnRandomCell(FieldPosition fieldPosition, Vector2 position, float scale)
        {
            CellView cellView = _itemPool.Spawn(fieldPosition);
            cellView.transform.localPosition = position;
            cellView.transform.localScale =Vector3.one *  scale;
            ItemData itemData = GetNewItemData();
            cellView.Initialize(itemData.cellColor, itemData.item, itemData.color);
            return (cellView, itemData);
        }

        public (CellView boosterView, ItemData boosterData) SpawnBoosterCellView(FieldPosition fieldPosition, int matchedCount)
        {
            ItemData itemData = GetBoosterItemData(matchedCount);
            CellView cellView = null;
            if (itemData != null)
            {
                cellView = _itemPool.Spawn(fieldPosition);
                cellView.transform.position = _fieldBoard.WorldPosition(fieldPosition.Row, fieldPosition.Column);
                cellView.transform.localScale *=  _fieldBoard.TileSize;
                cellView.Initialize(itemData.cellColor, itemData.item, itemData.color);
            }

            return (cellView, itemData);
        }
        
        public (CellView boosterView, ItemData boosterData) SpawnBoosterCellView(FieldPosition fieldPosition, ItemType boosterType)
        {
            ItemData itemData = _itemsConfig.GetItem(boosterType);
            CellView cellView = null;
            if (itemData != null)
            {
                cellView = _itemPool.Spawn(fieldPosition);
                cellView.transform.position = _fieldBoard.WorldPosition(fieldPosition.Row, fieldPosition.Column);
                cellView.transform.localScale *=  _fieldBoard.TileSize;
            }

            return (cellView, itemData);
        }

        public ItemView SpawnAnimatedItemView(Vector3 position, ItemType itemType)
        {
            ItemView itemView = _cellFactory.CreateInstance(position);
            itemView.R.sprite = _itemsConfig.GetItem(itemType).item;
            itemView.transform.localScale *= _fieldBoard.TileSize;
            return itemView;
        }

        private ItemData GetBoosterItemData(int matchedCount)
        {
            ItemData boosterData = _itemsConfig.GetBoosterItemData(matchedCount);

            return boosterData;
        }

        public GameObject SpawnEffect(VfxType vfxType, FieldPosition fieldPosition)
        {
            GameObject pref = _vfxConfig.GetVfxByType(vfxType);
            GameObject inst = Object.Instantiate(pref, _fieldBoard.WorldPosition(fieldPosition), Quaternion.identity);
            return inst;
        }

        private ItemData GetNewItemData()
        {
            return _itemsConfig.GetItem(_gameModel.Level.levelData.generatable.Generate());
        }

        public void Despawn(CellView cellView) => _itemPool.Despawn(cellView);
    }
}