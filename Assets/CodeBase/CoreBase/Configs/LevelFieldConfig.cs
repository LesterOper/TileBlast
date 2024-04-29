using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections.Generic;

namespace CodeBase.CoreBase.Configs
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

        
        public void GenerateRandomField()
        {
            int index = 0;

            for (int i = row - 1; i >=0; i--)
            {
                for (int j = 0; j < columns; j++)
                {
                    types[index] = new LevelItem(i, j,
                        (ItemType) Random.Range(0, Enum.GetValues(typeof(ItemType)).Length - 5));
                    /*types[index].item = (ItemType) Random.Range(0, Enum.GetValues(typeof(ItemType)).Length - 5);
                    types[index].row = i;
                    types[index].column = j;*/
                    index++;
                }
            }
        }
    }
}