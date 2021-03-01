using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Lando.Tags
{
    [CustomEditor(typeof(TagSettingsSingleton))]
    public class TagSettingsEditor : Editor
    {
        private ReorderableList tagsList;

        private void OnEnable()
        {
            tagsList = new ReorderableList(serializedObject, serializedObject.FindProperty("Tags"), 
                true, true, true, true);

            tagsList.drawHeaderCallback = DrawHeaderCallback;
            tagsList.drawElementCallback = DrawElementCallback;
            tagsList.onAddCallback = OnAddCallBack;
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            tagsList.DoLayoutList();
            
            DrawGenerateButton();
            
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawHeaderCallback(Rect rect)
        {
            EditorGUI.LabelField(rect, "Tags");
        }

        private void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty element = tagsList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            
            Rect labelRect = new Rect(rect.x, rect.y, rect.width - 40, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(labelRect, element.FindPropertyRelative("Name"), GUIContent.none);
            
            Rect colorRect = new Rect(rect.x + rect.width - 40, rect.y, 40, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(colorRect, element.FindPropertyRelative("Color"), GUIContent.none);
        }

        private void DrawGenerateButton()
        {
            EditorGUILayout.Space(10);

            if (GUILayout.Button("Generate class"))
                CreateClass();
        }
        
        private void OnAddCallBack(ReorderableList reorderableList)
        {
            SerializedProperty list = reorderableList.serializedProperty;
            
            list.InsertArrayElementAtIndex(list.arraySize);
            
            SerializedProperty lastItem = list.GetArrayElementAtIndex(list.arraySize - 1);
            SerializedProperty lastItemName = lastItem.FindPropertyRelative("Name");
            SerializedProperty lastItemColor = lastItem.FindPropertyRelative("Color");

            lastItemName.stringValue = "";
            lastItemColor.colorValue = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
        }

        private void CreateClass()
        {
            string copyPath = "Assets/Lando/Tags/Scripts/Generated/TagCollection.cs";
            
            using (StreamWriter outfile = new StreamWriter(copyPath))
            {
                outfile.WriteLine("namespace Lando.Tags");
                outfile.WriteLine("{");
                outfile.WriteLine(GetIndentationLevel(1) + "public sealed partial class Tag");
                outfile.WriteLine(GetIndentationLevel(1) + "{");

                SerializedProperty list = tagsList.serializedProperty;
                for (int i = 0; i < list.arraySize; i++)
                {
                    SerializedProperty tag = list.GetArrayElementAtIndex(i);
                    string tagName = tag.FindPropertyRelative("Name").stringValue;
                    string variableName = tagName.Replace(" ", "");

                    outfile.WriteLine(GetIndentationLevel(2) + "public static readonly string " + variableName + " = \"" + tagName + "\";" );
                }
                
                outfile.WriteLine(GetIndentationLevel(1) + "}");
                outfile.WriteLine("}");
            }
                
            AssetDatabase.Refresh();
        }

        private string GetIndentationLevel(int level)
        {
            string indent = string.Empty;
            for (int i = 0; i < level * 4; i++)
                indent += " ";

            return indent;
        }
    }
}