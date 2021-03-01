using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Lando.Tags
{
    [CustomEditor(typeof(TagSearcher))]
    public class TagSearcherEditor : Editor
    {
        private GameObject targetGameObject;
        private string[] tagsToSearch;
        
        private void OnEnable()
        {
            targetGameObject = ((TagSearcher) target).gameObject;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.HelpBox("Put the tags you want to search in the Tag component", MessageType.Warning);
            
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.HelpBox("Will return the objects matching ALL the tags.", MessageType.Info);
            EditorGUILayout.HelpBox("Will return the objects matching ANY of the tags", MessageType.Info);
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Search with ALL"))
            {   
                tagsToSearch = targetGameObject.GetTags().ToArray();
                
                if (tagsToSearch.Length == 0)
                    return;
                
                LogSearchingTags();
                LogResults(TagManager.GetAllGameObjectsWithAllTags(tagsToSearch));
            }
            
            if (GUILayout.Button("Search with ANY"))
            {
                tagsToSearch = targetGameObject.GetTags().ToArray();
                
                if (tagsToSearch.Length == 0)
                    return;
                
                LogSearchingTags();
                LogResults(TagManager.GetAllGameObjectsWithAnyTags(tagsToSearch));
            }
            
            EditorGUILayout.EndHorizontal();
        }

        private void LogSearchingTags()
        {
            string tags = "";
            foreach (string tag in tagsToSearch)
                tags += $"{tag}/";
            tags = tags.Remove(tags.Length - 1);
            Debug.Log($"Searching for objects with tags: {tags}");
        }

        private void LogResults(IEnumerable<GameObject> gameObjects)
        {   
            Debug.Log("Found:");
            foreach (GameObject gameObject in gameObjects)
                if(gameObject != targetGameObject)
                    Debug.Log(gameObject.name);
        }
    }
}