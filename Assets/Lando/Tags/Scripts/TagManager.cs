using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lando.Tags
{
    public static partial class TagManager
    {
        private static readonly Dictionary<GameObject, List<string>> gameObjectToTags = new Dictionary<GameObject, List<string>>();
        private static readonly Dictionary<string, List<GameObject>> tagToGameObjects = new Dictionary<string, List<GameObject>>();
        
        public static void RegisterGameObject(GameObject gameObject, params TagEntity[] tags)
        {
            if (!gameObjectToTags.ContainsKey(gameObject))
                gameObjectToTags[gameObject] = new List<string>();
            
            foreach (string tag in tags.Select(x => x.Name))
                RegisterGameObjectOnTag(gameObject, tag);
        }
        
        public static void UnregisterGameObject(GameObject gameObject, params TagEntity[] tags)
        {            
            foreach (string tag in tags.Select(x => x.Name))
                UnregisterGameObjectOnTag(gameObject, tag);

            gameObjectToTags.Remove(gameObject);
        }
        
        private static void RegisterGameObjectOnTag(GameObject gameObject, string tag)
        {
            if (!tagToGameObjects.ContainsKey(tag))
                tagToGameObjects[tag] = new List<GameObject>();
            tagToGameObjects[tag].Add(gameObject);
            
            gameObjectToTags[gameObject].Add(tag);
        }
        
        private static void UnregisterGameObjectOnTag(GameObject gameObject, string tag)
        {
            if(tagToGameObjects.ContainsKey(tag))
                tagToGameObjects[tag].Remove(gameObject);
            
            if(gameObjectToTags.ContainsKey(gameObject))
                gameObjectToTags[gameObject].Remove(tag);
        }
        
        public static IEnumerable<GameObject> GetAllGameObjectsWithTag(string tag)
        {
            return tagToGameObjects[tag];
        }
        
        public static IEnumerable<GameObject> GetAllGameObjectsWithAllTags(params string[] tags)
        {
            List<GameObject> gameObjects = new List<GameObject>();
            
            foreach (GameObject gameObject in gameObjectToTags.Keys)
                if(gameObject.HasAllTags(tags))
                    gameObjects.Add(gameObject);
            
            return gameObjects;
        }
        
        public static IEnumerable<GameObject> GetAllGameObjectsWithAnyTags(params string[] tags)
        {
            List<GameObject> gameObjects = new List<GameObject>();
            
            foreach (GameObject gameObject in gameObjectToTags.Keys)
                if(gameObject.HasAnyTags(tags))
                    gameObjects.Add(gameObject);
            
            return gameObjects;
        }
    }
}