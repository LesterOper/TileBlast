using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeBase.CoreBase.Configs
{
    [CreateAssetMenu(menuName = "Configs/" + nameof(ItemsConfig), fileName = nameof(ItemsConfig))]
    public class ItemsConfig : ScriptableObject
    {
        public List<ItemData> items;

        public ItemData GetItem(ItemType itemType)
        {
            return items.FirstOrDefault(local => local.itemType == itemType);
        }
    }

    [Serializable]
    public class ItemData
    {
        public ItemType itemType;
        public Sprite item;
        public Sprite cellColor;
    }

    public enum ItemType
    {
        NONE = 0,
        RED = 1,
        YELLOW = 2,
        BLUE = 3,
        GREEN = 4,
        PURPLE = 5,
    }
}