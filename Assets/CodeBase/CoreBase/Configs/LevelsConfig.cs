using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.CoreBase.Configs
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
    
    public class Level
    {
        [BoxGroup("Level number", CenterLabel = true)] [ReadOnly]
        public int LevelNumber;
        [BoxGroup("Main settings for level", CenterLabel = true)]
        public LevelData levelData;
        
        [InfoBox("Need to assigned LevelFieldConfig", InfoMessageType.Error, nameof(IsLevelFieldConfigNotAssigned))]
        public LevelFieldConfig levelFieldConfig;
        
        [HideIf(nameof(IsLevelFieldConfigNotAssigned))]
        [HideIf(nameof(IsGeneratableListEmpty))]
        [InfoBox("Generate random field")]
        [Button]
        public void GenerateRandomField()
        {
            levelFieldConfig.GenerateRandomField(levelData.generatable);
        }


        private bool IsGeneratableListEmpty() => levelData.generatable.generatable.Count <= 0;
        
        private bool IsLevelFieldConfigNotAssigned() => levelFieldConfig == null;
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
                amount = levelFieldConfig.types.Count(local => local.item != ItemType.NONE)
            };
            levelData.LevelAims.Add(levelAim);
        } 
    }

    [Serializable]
    public class LevelData
    {
        [BoxGroup("Difficulty", CenterLabel = true)]
        public LevelDifficultyType levelDifficultyType;
        [BoxGroup("Level type by type of aim", CenterLabel = true)]
        [InfoBox("Assigned type of level, NONE is redundant", InfoMessageType.Error, nameof(IsLevelTypeEqualsNone))]
        public LevelType levelType;
        
        [BoxGroup("Amount of moves", CenterLabel = true)]
        [InfoBox("Assigned moves amount for this level", InfoMessageType.Error, nameof(IsMovesAmountEqualsOrLessThanZero))]
        public float moves;
        [BoxGroup("Aims", CenterLabel = true)]
        [InfoBox("Assigned level's aims")]
        public List<LevelAim> LevelAims;

        [BoxGroup("Items which can be generated on this level", CenterLabel = true)]
        public GeneratableItemsConfig generatable;
        
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