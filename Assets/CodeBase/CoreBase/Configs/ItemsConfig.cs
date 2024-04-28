using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.CoreBase.Configs
{
    [CreateAssetMenu(menuName = "Configs/" + nameof(ItemsConfig), fileName = nameof(ItemsConfig))]
    public class ItemsConfig : ScriptableObject
    {
        [BoxGroup]
        [InfoBox("Basic game items")]
        public List<ItemData> items;
       
        public ItemData GetItem(ItemType itemType)
        {
            return items.FirstOrDefault(local => local.itemType == itemType);
        }

        public ItemData GetBoosterItemData(int matchCount)
        {
            List<ItemData> boosterDatas = items.FindAll(local => local.isBooster);
            List<ItemData> boosterData = boosterDatas.FindAll(local => local.IsCorrectBoosterByMatchItemsCount(matchCount));
            return boosterData.Count > 0 ? boosterData[Random.Range(0, boosterData.Count)] : null;
        }
    }

    [Serializable]
    public class ItemData
    {
        public ItemType itemType;
        public Sprite item;
        public Sprite cellColor;
        public Color color;
        
        public bool isBlock = false;
        public bool canMatch = true;
        [HideIf(nameof(isBlock))]
        public bool isBooster = true;
        
        [HideIf(nameof(isBlock))]
        public bool isDropBottomItem = false;
        
        [ShowIf(nameof(isBooster))]
        [SerializeField] private int minCountNeedItems;
        [ShowIf(nameof(isBooster))]
        [SerializeField] private int maxCountNeedItems;
        
        public bool IsCorrectBoosterByMatchItemsCount(int matchCount)
        {
            if(maxCountNeedItems != 0)
                return matchCount >= minCountNeedItems && matchCount <= maxCountNeedItems;
            return matchCount >= minCountNeedItems;
        }

        public bool IsDropBottomItem => isDropBottomItem;
    }
    
    public enum ItemType
    {
        [ItemAttribute(false)]
        EMPTY = -1,
        [ItemAttribute(false)]
        NONE = 0,
        [ItemAttribute(false)]
        RED = 1,
        [ItemAttribute(false)]
        YELLOW = 2,
        [ItemAttribute(false)]
        BLUE = 3,
        [ItemAttribute(false)]
        GREEN = 4,
        [ItemAttribute(false)]
        PURPLE = 5,


        #region OnFieldBoosters

        [ItemAttribute(true)]
        BOMB = 6,
        [ItemAttribute(true)]
        ROCKET_HORIZONTAL = 7,
        [ItemAttribute(true)]
        ROCKET_VERTICAL = 8,
        [ItemAttribute(true)]
        DISCO = 9,

        #endregion
        
        [ItemAttribute(false, true)]
        DROP_BOTTOM = 10,
        
        [ItemAttribute(false, false, true)]
        BOX = 11,
    }

    public class ItemAttribute : Attribute
    {
        public bool _isBooster;
        public bool _isDropBottomItem;
        public bool _isBlock;

        public ItemAttribute(bool isBooster, bool isDropBottomItem = false, bool isBlock = false)
        {
            _isBooster = isBooster;
            _isDropBottomItem = isDropBottomItem;
            _isBlock = isBlock;
        }
    }
}