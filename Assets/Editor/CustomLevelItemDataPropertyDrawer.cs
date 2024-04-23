using CodeBase.CoreBase.Configs;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(LevelItemsData))]
    public class CustomLevelItemDataPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty fieldPosition = property.FindPropertyRelative("FieldPosition");
            EditorGUI.PropertyField(position, fieldPosition);
            position.y += 18f;
            SerializedProperty type = property.FindPropertyRelative("itemType");
            EditorGUI.PropertyField(position, type);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 18f * 2;
        }
    }
}