using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
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
        [InfoBox("List of boosters has invalid item, which IS booster", InfoMessageType.Error, nameof(HasItemInListWhichIsABooster))]
        public List<ItemData> items;
        [Space(30)]
        [BoxGroup]
        [InfoBox("Booster game items")]
        [InfoBox("List of boosters has invalid item, which ISN'T booster", InfoMessageType.Error, nameof(HasItemInListWhichNotABooster))]
        public List<ItemData> boosterDatas;

        public ItemData GetItem(ItemType itemType)
        {
            return items.FirstOrDefault(local => local.itemType == itemType);
        }

        public ItemData GetBoosterItemData(int matchCount)
        {
            List<ItemData> boosterData = boosterDatas.FindAll(local => local.IsCorrectBoosterByMatchItemsCount(matchCount));
            return boosterData.Count > 0 ? boosterData[Random.Range(0, boosterData.Count)] : null;
        }

        private bool HasItemInListWhichNotABooster()
        {
            var notBooster = boosterDatas.Find(local => !local.IsBooster);
            return notBooster != null;
        }
        
        private bool HasItemInListWhichIsABooster()
        {
            var notBooster = items.Find(local => local.IsBooster);
            return notBooster != null;
        }
    }

    [Serializable]
    public class ItemData
    {
        public ItemType itemType;
        public Sprite item;
        public Sprite cellColor;
        
        [ShowIf(nameof(IsBooster))]
        [SerializeField] private int minCountNeedItems;
        [ShowIf(nameof(IsBooster))]
        [SerializeField] private int maxCountNeedItems;

        public bool IsBooster => itemType.GetAttribute<ItemAttribute>()._isBooster;
        
        public bool IsCorrectBoosterByMatchItemsCount(int matchCount)
        {
            if(maxCountNeedItems != 0)
                return matchCount >= minCountNeedItems && matchCount <= maxCountNeedItems;
            return matchCount >= minCountNeedItems;
        }
    }
    
    public enum ItemType
    {
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
    }

    public class ItemAttribute : Attribute
    {
        public bool _isBooster;

        public ItemAttribute(bool isBooster)
        {
            _isBooster = isBooster;
        }
    }
}