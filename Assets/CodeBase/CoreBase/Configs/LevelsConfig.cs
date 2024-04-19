using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.DialogsSystem;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace CodeBase.CoreBase.Configs
{
    [CreateAssetMenu(menuName = "Configs/" + nameof(LevelsConfig), fileName = nameof(LevelsConfig))]
    public class LevelsConfig : ScriptableObject
    {
        [InfoBox("Only for debug, used for play exact level")]
        public int level;
        [BoxGroup]
        public List<Level> levels;

        [InfoBox("Level should be more than 0", InfoMessageType.Error, nameof(IsLevelLessThanZero))]
        public Level GetLevel() => levels[level - 1];

        private bool IsLevelLessThanZero() => level <= 0;
    }

    [Serializable]
    public class Level
    {
        public LevelType levelType;
        
        [BoxGroup("Amount of moves", CenterLabel = true)]
        public float moves;
        [InfoBox("Assigned row count", InfoMessageType.Error, nameof(IsRowCountLessThanZero))]
        public int row;
        [InfoBox("Assigned column count", InfoMessageType.Error, nameof(IsColumnCountLessThanZero))]
        public int column;
        
        [InfoBox("Assigned level's aims")]
        public List<LevelAim> LevelAims;
        
        [BoxGroup]
        [ShowIf(nameof(IsRowAndColumnCountAssigned))]
        public List<LevelItemsData> levelItemsDatas;
        
        [BoxGroup]
        [ShowIf(nameof(IsRowAndColumnCountAssigned))]
        [InfoBox("Generate empty list of items or regenerate exist")]
        [Button]
        private void GenerateListOfLevelItems()
        {
            levelItemsDatas = new List<LevelItemsData>(row * column);
            int levelListIndex = 0;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    levelItemsDatas.Add(new LevelItemsData());
                    levelItemsDatas[levelListIndex].FieldPosition = new FieldPosition(i, j);
                    levelListIndex++;
                }
            }
        }
        
        [BoxGroup]
        [ShowIf(nameof(IsLevelItemsDataListNotEmpty))]
        [InfoBox("Generate randomly items")]
        [Button]
        private void RandomGenerationOfItemsType()
        {
            foreach (var item in levelItemsDatas)
            {
                item.itemType = (ItemType) Random.Range(1, Enum.GetValues(typeof(ItemType)).Length - 5);
            }
        }

        [BoxGroup]
        [ShowIf(nameof(IsLevelItemsDataListNotEmpty))]
        [InfoBox("Generate field position indexes of items or regenerate exist")]
        [Button]
        public void GenerateFieldPosition()
        {
            int levelListIndex = 0;

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    levelItemsDatas[levelListIndex].FieldPosition = new FieldPosition(i, j);
                    levelListIndex++;
                }
            }
        }
        [BoxGroup]
        [ShowIf(nameof(IsElementsCountIncreased))]
        [InfoBox("Count of elements changed, click button to add", InfoMessageType.Error)]
        [Button]
        private void AddElements()
        {
            int diff = (row * column) - levelItemsDatas.Count;

            for (int i = 0; i < diff; i++)
            {
                levelItemsDatas.Add(new LevelItemsData());
            }
        }
        
        [BoxGroup]
        [ShowIf(nameof(IsElementsCountDecreased))]
        [InfoBox("Count of elements changed, click button to remove", InfoMessageType.Error)]
        [Button]
        private void RemoveElements()
        {
            int diff = levelItemsDatas.Count - (row * column);

            for (int i = 0; i < diff; i++)
            {
                levelItemsDatas.RemoveAt(levelItemsDatas.Count - 1);
            }
        }

        private bool IsNotClearAllLevelType() => levelType != LevelType.CLEAR_FIELD;
        private bool IsClearAllLevelType() => levelType == LevelType.CLEAR_FIELD;


        private bool IsLevelItemsDataListNotEmpty() => levelItemsDatas is {Count: > 0};
        private bool IsElementsCountIncreased() => levelItemsDatas !=null && levelItemsDatas.Count < row * column;
        private bool IsElementsCountDecreased() => levelItemsDatas !=null && levelItemsDatas.Count > row * column;
        private bool IsRowAndColumnCountAssigned() => row > 0 && column > 0;
        private bool IsRowCountLessThanZero() => row <= 0;
        private bool IsColumnCountLessThanZero() => column <= 0;

        
        [ShowIf(nameof(IsClearAllLevelType))]
        [InfoBox("Count aim amount")]
        [Button]
        private void CreateAimIfClearFieldLevelType()
        {
            LevelAims = new List<LevelAim>(1);
            LevelAim levelAim = new LevelAim
            {
                ItemType = ItemType.NONE,
                amount = levelItemsDatas.Count(local => local.itemType != ItemType.NONE)
            };
            LevelAims.Add(levelAim);
        } 
    }

    [Serializable]
    public class LevelAim
    {
        public ItemType ItemType;
        public int amount;
    }
    
    [Serializable]
    public class LevelItemsData
    {
        [BoxGroup]
        public FieldPosition FieldPosition;
        [BoxGroup]
        public ItemType itemType;
    }

    public enum LevelType
    {
        NONE = 0,
        DROP_OBJECTS_TO_BOTTOM = 1,
        COLLECT_OBJECTS = 2,
        CLEAR_FIELD = 3,
    }
}