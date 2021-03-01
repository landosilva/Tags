using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Lando.Tags
{
    [CustomEditor(typeof(Tag))]
    public class TagEditor : Editor
    {
        private SerializedProperty tagsSerializedProperty;
        
        private Texture2D texture;
        private GameObject targetGameObject;
        
        private Vector2 scrollPosition;
        
        float darkerRatio = 0.75f;
        
        private void OnEnable()
        {
            tagsSerializedProperty = serializedObject.FindProperty("tags");
            texture = new Texture2D(1, 1);
            targetGameObject = ((Tag) target).gameObject;
        }
        
        public override void OnInspectorGUI()
        {   
            DrawTags();
            EditorGUILayout.Separator();
            DrawAddTagButton();
            
            serializedObject.ApplyModifiedProperties();
        }
        
        private void DrawTags()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, false, false);

            EditorGUILayout.BeginHorizontal();

            bool shouldBreakLine = false;
            float lineWidth = 0;
            
            for (int i = 0; i < tagsSerializedProperty.arraySize; i++)
            {
                if (shouldBreakLine)
                {
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    
                    lineWidth = 0;
                }
                
                (string tagName, Color tagColor) = GetProperties(tagsSerializedProperty.GetArrayElementAtIndex(i));
                
                ChangeTextureColor(tagColor);

                GUIStyle tagStyle = new GUIStyle(GUI.skin.button);
                tagStyle.normal.background = texture;
                tagStyle.stretchWidth = false;
                tagStyle.stretchHeight = false;
                tagStyle.margin = new RectOffset(0, 4, 5, 5);
                tagStyle.padding = new RectOffset(6, 12, 2, 2);
                
                if (GUILayout.Button($"{tagName}    ✖", tagStyle))
                {   
                    tagsSerializedProperty.DeleteArrayElementAtIndex(i);
                    
                    if (Application.isPlaying)
                        targetGameObject.RemoveTag(tagName);
                }

                Rect rect = GUILayoutUtility.GetLastRect();
                lineWidth += rect.width;

                shouldBreakLine = false; // lineWidth >= EditorGUIUtility.currentViewWidth; 
            }
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.EndScrollView();
        }
        
        private void DrawAddTagButton()
        {   
            GUILayout.Space(5);
            
            Rect buttonRect = GUILayoutUtility.GetRect(new GUIContent("Add Tag"), EditorStyles.toolbarButton);
            if (GUI.Button(buttonRect, new GUIContent("Add Tag"), EditorStyles.toolbarButton))
            {
                TagSettingsDropdown dropdown = new TagSettingsDropdown(new AdvancedDropdownState());
                dropdown.OnItemSelected += OnItemSelected;
                dropdown.Show(buttonRect);
            }
        }

        private void OnItemSelected(TagEntity tagEntity)
        {
            AddTag(tagEntity);
        }

        private void AddTag(TagEntity tagEntity)
        {   
            tagsSerializedProperty.InsertArrayElementAtIndex(tagsSerializedProperty.arraySize);
            SerializedProperty newTagSerializedProperty = tagsSerializedProperty.GetArrayElementAtIndex(tagsSerializedProperty.arraySize - 1);
            SerializedProperty newTagNameSerializedProperty = newTagSerializedProperty.FindPropertyRelative("Name");
            SerializedProperty newTagColorSerializedProperty = newTagSerializedProperty.FindPropertyRelative("Color");
            
            newTagNameSerializedProperty.stringValue = tagEntity.Name;
            newTagColorSerializedProperty.colorValue = tagEntity.Color;
            
            EditorUtility.SetDirty(target);

            if (Application.isPlaying)
                targetGameObject.AddTag(tagEntity.Name);
            
            serializedObject.ApplyModifiedProperties();
        }

        private (string, Color) GetProperties(SerializedProperty serializedProperty)
        {
            string tagName = serializedProperty.FindPropertyRelative("Name").stringValue;
                
            Color tagColor = serializedProperty.FindPropertyRelative("Color").colorValue;
            tagColor = new Color
            (
                tagColor.r * darkerRatio,
                tagColor.g * darkerRatio,
                tagColor.b * darkerRatio,
                1
            );

            return (tagName, tagColor);
        }

        private void ChangeTextureColor(Color color)
        {
            texture.SetPixel(0, 0, color);
            texture.Apply();
        }
    }
}

