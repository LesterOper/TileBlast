using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeBase.CoreBase.Configs.LevelSystem
{
    [CreateAssetMenu(menuName = "Configs/" + nameof(LevelFieldConfig), fileName = nameof(LevelFieldConfig))]
    [Serializable]
    public class LevelFieldConfig : ScriptableObject
    {
        [ReadOnly]
        public int row;
        [ReadOnly]
        public int columns;
        public List<LevelItem> types;
        
        public LevelItem GetValue(int row, int column)
        {
            int index = (row * columns) + column;
            return types[index];
        }

        public void SetValue(int row, int column, LevelItem value)
        {
            int index = (row * columns) + column;
            types[index] = value;
        }

        
        public void GenerateRandomField(GeneratableItemsConfig generatableItemsConfig)
        {
            int index = 0;

            for (int i = row - 1; i >=0; i--)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (types[index].item is ItemType.RED or ItemType.BLUE or ItemType.GREEN or ItemType.PURPLE
                        or ItemType.YELLOW or ItemType.NONE)
                    {
                        types[index] = new LevelItem(i, j,
                            generatableItemsConfig.Generate());
                    }

                    index++;
                }
            }
        }
    }
}