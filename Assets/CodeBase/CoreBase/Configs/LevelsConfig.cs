using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

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
        
        public LevelItemsContainer levelItemsContainer;
        
        [InfoBox("Generate random field")]
        [Button]
        public void GenerateFieldPosition()
        {
            levelItemsContainer.levelFieldConfig.GenerateRandomField();
        }
        
        private bool IsNotClearAllLevelType() => levelData.levelType != LevelType.CLEAR_FIELD;
        private bool IsClearAllLevelType() => levelData.levelType == LevelType.CLEAR_FIELD;
        
        [ShowIf(nameof(IsClearAllLevelType))]
        [InfoBox("Count aim amount")]
        [Button]
        private void CreateAimIfClearFieldLevelType()
        {
            levelData.LevelAims = new List<LevelAim>(1);
            LevelAim levelAim = new LevelAim
            {
                ItemType = ItemType.NONE,
                amount = levelItemsContainer.levelFieldConfig.types.Count(local => local.item != ItemType.NONE)
            };
            levelData.LevelAims.Add(levelAim);
        } 
    }

    [Serializable]
    public class LevelItemsContainer
    {
        public LevelFieldConfig levelFieldConfig;
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
    public class LevelItem
    {
        public int row;
        public int column;
        public ItemType item;

        private FieldPosition _fieldPosition;
        public FieldPosition FieldPosition => _fieldPosition;

        public LevelItem(int rowInd, int columnInd, ItemType itemType)
        {
            item = itemType;
            row = rowInd;
            column = columnInd;
            _fieldPosition = new FieldPosition(row, column);
        }
    }

    public enum LevelType
    {
        NONE = 0,
        DROP_OBJECTS_TO_BOTTOM = 1,
        COLLECT_OBJECTS = 2,
        CLEAR_FIELD = 3,
    }
}