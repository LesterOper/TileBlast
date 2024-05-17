using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.CoreBase.Configs.LevelSystem
{
    [CreateAssetMenu(menuName = "Configs/" + nameof(LevelsConfig), fileName = nameof(LevelsConfig))]
    public class LevelsConfig : ScriptableObject
    {
        [InfoBox("Only for debug, used for play exact level")]
        public int level;
        
        public List<Level> levels;

        [InfoBox("Level should be more than 0", InfoMessageType.Error, nameof(IsLevelLessThanZero))]
        public Level GetLevel() => levels[level - 1];

        public Level GetLevel(int currentLevel)
        {
            if (currentLevel > levels.Count)
                currentLevel = 1;
            return levels[currentLevel - 1];
        }

        private bool IsLevelLessThanZero() => level <= 0;

        [Button]
        private void SetupLevelNumbers()
        {
            for (int i = 0; i < levels.Count; i++)
            {
                levels[i].LevelNumber = i + 1;
            }
        }
    }


    [Serializable]
    public class LevelAim
    {
        public ItemType ItemType;
        public int amount;
    }
    

    [Serializable]
    public class LevelItem
    {
        public int row;
        public int column;
        public ItemType item;
        
        public FieldPosition FieldPosition => new FieldPosition(row, column);

        public LevelItem(int rowInd, int columnInd, ItemType itemType)
        {
            item = itemType;
            row = rowInd;
            column = columnInd;
        }
    }

    [Serializable]
    public class GeneratableItemsConfig
    {
        [InfoBox("Add generatable items", InfoMessageType.Error, nameof(IsEmpty))]
        [InfoBox("Check sum of percent, probably sum of percents may be less than 0", InfoMessageType.Error, nameof(IsSumOfPercentsMoreThanOneOrLessThanZero))]
        public List<GeneratableItem> generatable;

        private bool IsEmpty() => generatable.Count <= 0;
        
        private bool IsSumOfPercentsMoreThanOneOrLessThanZero()
        {
            float count = 0;

            for (int i = 0; i < generatable.Count; i++)
            {
                count += generatable[i].percent;
            }

            if (count < 0)
                return true;
            return false;
        }

        public ItemType Generate()
        {
            int sum = 0, i;

            for (int j = 0; j < generatable.Count; j++)
            {
                sum += generatable[j].percent;
            }

            int rand = Random.Range(0, sum);

            for (i = 0; i < generatable.Count; i++)
            {
                rand -= generatable[i].percent;
                if(rand < 0) break;
            }
            
            return generatable[i].item;

        }
    }

    [Serializable]
    public class GeneratableItem
    {
        public ItemType item;
        [Range(0, 100)] public int percent;
    }

    public enum LevelType
    {
        NONE = 0,
        DROP_OBJECTS_TO_BOTTOM = 1,
        COLLECT_OBJECTS = 2,
        CLEAR_FIELD = 3,
    }

    public enum LevelDifficultyType
    {
        Easy, Normal, Hard
    }
}