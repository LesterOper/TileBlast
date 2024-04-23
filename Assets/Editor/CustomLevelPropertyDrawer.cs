using System.Collections.Generic;
using CodeBase.CoreBase.Configs;
using UnityEditor;
using UnityEngine;

namespace Editor
{
   // [CustomPropertyDrawer(typeof(LevelItemsContainer))]
    public class CustomLevelPropertyDrawer : PropertyDrawer
    {
        private int rowCount;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //base.OnGUI(position, property, label);

            EditorGUI.LabelField(position, label);
            Rect newPosition = position;
            newPosition.y += 18f;
            EditorGUI.BeginProperty(position, label, property);
            SerializedProperty rowsCount = property.FindPropertyRelative("row");
            //EditorGUI.LabelField(newPosition, rowsCount.displayName);
            newPosition.x += newPosition.width / 10;
            EditorGUI.PropertyField(newPosition, rowsCount, GUIContent.none);
            newPosition.x += newPosition.width / 10;
            SerializedProperty columnsCount = property.FindPropertyRelative("column");
            //EditorGUI.LabelField(newPosition, columnsCount.displayName);
            newPosition.x += newPosition.width / 10;
            EditorGUI.PropertyField(newPosition, columnsCount, GUIContent.none);
            newPosition.x = position.x;
            newPosition.y += 66f;
            SerializedProperty itemsMass = property.FindPropertyRelative("LevelItemsDatas");
            itemsMass.arraySize = rowsCount.intValue * columnsCount.intValue;
            EditorGUI.PropertyField(newPosition, itemsMass, GUIContent.none);
            newPosition.y += 66f;

            rowCount = rowsCount.intValue;

            for (int i = 0; i < rowsCount.intValue; i++)
            {
                
                newPosition.width = position.width / columnsCount.intValue;
                for (int j = 0; j < columnsCount.intValue; j++)
                {
                    EditorGUI.PropertyField(newPosition, itemsMass.GetArrayElementAtIndex(j), GUIContent.none);
                    newPosition.x += newPosition.width;
                }

                newPosition.x = position.x;
                newPosition.y += 68;
            }
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 68f * (rowCount) + 18 + 66+66;
        }
    }
}