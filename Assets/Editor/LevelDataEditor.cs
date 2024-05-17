using CodeBase.CoreBase.Configs;
using CodeBase.CoreBase.Configs.LevelSystem;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(LevelFieldConfig))]
    public class LevelDataEditor : UnityEditor.Editor
    {
        private SerializedProperty dataArrayProperty;
        private SerializedProperty rowsProperty;
        private SerializedProperty columnsProperty;

        private const float MinColumnWidth = 80f;

        private void OnEnable()
        {
            dataArrayProperty = serializedObject.FindProperty("types");
            rowsProperty = serializedObject.FindProperty("row");
            columnsProperty = serializedObject.FindProperty("columns");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(rowsProperty);
            EditorGUILayout.PropertyField(columnsProperty);
            
            GUILayout.Box(GUIContent.none, GUILayout.ExpandWidth(true), GUILayout.Height(3));
            

            int rows = rowsProperty.intValue;
            int columns = columnsProperty.intValue;
            int arraySize = rows * columns;

            if (dataArrayProperty.arraySize != arraySize)
            {
                dataArrayProperty.arraySize = arraySize;
            }

            EditorGUIUtility.labelWidth = 60f; // Adjust the label width as desired
            float itemSize = Mathf.Min(EditorGUIUtility.currentViewWidth / columns, MinColumnWidth);

            SerializedProperty elementProperty;
            int index = 0;

            for (int i = rows-1; i >=0; i--)
            {
                EditorGUILayout.BeginHorizontal();

                for (int j = 0; j < columns; j++)
                {
                    elementProperty = dataArrayProperty.GetArrayElementAtIndex(index);
                    SerializedProperty rowProperty = elementProperty.FindPropertyRelative("row");
                    SerializedProperty columnProperty = elementProperty.FindPropertyRelative("column");
                    SerializedProperty itemProperty = elementProperty.FindPropertyRelative("item");

                    EditorGUILayout.BeginVertical(GUILayout.Width(itemSize * 1.6f));
                    EditorGUILayout.PropertyField(rowProperty);
                    EditorGUILayout.PropertyField(columnProperty);
                    EditorGUILayout.PropertyField(itemProperty);
                    EditorGUILayout.EndVertical();
                    GUILayout.Box(GUIContent.none, GUILayout.ExpandHeight(true), GUILayout.Width(3));
                    index++;
                }

                EditorGUILayout.EndHorizontal();

                if (i < rows)
                {
                    GUILayout.Box(GUIContent.none, GUILayout.ExpandWidth(true), GUILayout.Height(3));
                    
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}