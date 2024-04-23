using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.DialogsSystem;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.UIElements;
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
        public LevelData levelData;
        //[BoxGroup]
        //[ShowIf(nameof(IsRowAndColumnCountAssigned))]
        public LevelItemsContainer levelItemsContainer;
        /*[BoxGroup]
        [ShowIf(nameof(IsRowAndColumnCountAssigned))]
        [InfoBox("Generate empty list of items or regenerate exist")]
        [Button]
        private void GenerateListOfLevelItems()
        {
            levelItemsContainer.LevelItemsDatas = new List<LevelItemsData>(levelData.row * levelData.column);
            int levelListIndex = 0;
            for (int i = 0; i < levelData.row; i++)
            {
                for (int j = 0; j < levelData.column; j++)
                {
                    levelItemsContainer.LevelItemsDatas.Add(new LevelItemsData());
                    levelItemsContainer.LevelItemsDatas[levelListIndex].FieldPosition = new FieldPosition(i, j);
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
            foreach (var item in levelItemsContainer.LevelItemsDatas)
            {
                item.itemType = (ItemType) Random.Range(1, Enum.GetValues(typeof(ItemType)).Length - 5);
            }
        }*/

        [BoxGroup]
        [ShowIf(nameof(IsLevelItemsDataListNotEmpty))]
        [InfoBox("Generate field position indexes of items or regenerate exist")]
        [Button]
        public void GenerateFieldPosition()
        {
            int levelListIndex = 0;

            for (int i = 0; i < levelItemsContainer.row; i++)
            {
                for (int j = 0; j < levelItemsContainer.column; j++)
                {
                    levelItemsContainer.LevelItemsDatas[levelListIndex].FieldPosition = new FieldPosition(i, j);
                    levelListIndex++;
                }
            }
        }
        
        
        private bool IsNotClearAllLevelType() => levelData.levelType != LevelType.CLEAR_FIELD;
        private bool IsClearAllLevelType() => levelData.levelType == LevelType.CLEAR_FIELD;


        private bool IsLevelItemsDataListNotEmpty() => levelItemsContainer.LevelItemsDatas is {Count: > 0};
        //private bool IsRowAndColumnCountAssigned() => levelData.row > 0 && levelData.column > 0;
        

        
        [ShowIf(nameof(IsClearAllLevelType))]
        [InfoBox("Count aim amount")]
        [Button]
        private void CreateAimIfClearFieldLevelType()
        {
            levelData.LevelAims = new List<LevelAim>(1);
            LevelAim levelAim = new LevelAim
            {
                ItemType = ItemType.NONE,
                amount = levelItemsContainer.LevelItemsDatas.Count(local => local.itemType != ItemType.NONE)
            };
            levelData.LevelAims.Add(levelAim);
        } 
    }

    [Serializable]
    public class LevelItemsContainer
    {
        [InfoBox("Assigned row count", InfoMessageType.Error, nameof(IsRowCountLessThanZero))]
        public int row;
        [InfoBox("Assigned column count", InfoMessageType.Error, nameof(IsColumnCountLessThanZero))]
        public int column;
        
        [TableList]
        public List<LevelItemsData> LevelItemsDatas;

        public int Count => row * column;
        
        private bool IsRowCountLessThanZero() => row <= 0;
        private bool IsColumnCountLessThanZero() => column <= 0;
        
        [BoxGroup]
        [ShowIf(nameof(IsElementsCountIncreased))]
        [InfoBox("Count of elements changed, click button to add", InfoMessageType.Error)]
        [Button]
        private void AddElements()
        {
            int diff = (row * column) - LevelItemsDatas.Count;

            for (int i = 0; i < diff; i++)
            {
                LevelItemsDatas.Add(new LevelItemsData());
            }
        }
        
        [BoxGroup]
        [ShowIf(nameof(IsElementsCountDecreased))]
        [InfoBox("Count of elements changed, click button to remove", InfoMessageType.Error)]
        [Button]
        private void RemoveElements()
        {
            int diff = LevelItemsDatas.Count - (row * column);

            for (int i = 0; i < diff; i++)
            {
                LevelItemsDatas.RemoveAt(LevelItemsDatas.Count - 1);
            }
        }
        
        private bool IsElementsCountIncreased() => LevelItemsDatas !=null && LevelItemsDatas.Count < Count;
        private bool IsElementsCountDecreased() => LevelItemsDatas !=null && LevelItemsDatas.Count > Count;


        static LevelItemsData DrawElement(Rect rect, LevelItemsData value)
        {
            //SerializedProperty propertyField = new SerializedProperty();
            //EditorGUI.PropertyField(rect, )
            return value;
        }
        
    }
    
    
    [Serializable]
    public class LevelData
    {
        [InfoBox("Assigned type of level, NONE is redundant", InfoMessageType.Error, nameof(IsLevelTypeEqualsNone))]
        public LevelType levelType;
        
        [BoxGroup("Amount of moves", CenterLabel = true)]
        [InfoBox("Assigned moves amount for this level", InfoMessageType.Error, nameof(IsMovesAmountEqualsOrLessThanZero))]
        public float moves;
        [InfoBox("Assigned level's aims")]
        public List<LevelAim> LevelAims;
        
        private bool IsLevelTypeEqualsNone() => levelType == LevelType.NONE;

        private bool IsMovesAmountEqualsOrLessThanZero() => moves <= 0;
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