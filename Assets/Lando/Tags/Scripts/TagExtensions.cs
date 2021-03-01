using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lando.Tags
{
    public static partial class TagManager
    {
        public static IEnumerable<string> GetTags(this GameObject gameObject)
        {
            return gameObjectToTags[gameObject];
        }

        public static void AddTag(this GameObject gameObject, string tag)
        {
            RegisterGameObjectOnTag(gameObject, tag);
        }

        public static void RemoveTag(this GameObject gameObject, string tag)
        {
            UnregisterGameObjectOnTag(gameObject, tag);
        }

        public static bool HasTag(this GameObject gameObject, string tag)
        {
            return gameObject.GetTags().Contains(tag);
        }

        public static bool HasAllTags(this GameObject gameObject, params string[] tags)
        {
            foreach (string tag in tags)
                if (!gameObject.HasTag(tag))
                    return false;

            return true;
        }

        public static bool HasAnyTags(this GameObject gameObject, params string[] tags)
        {
            foreach (string tag in tags)
                if (gameObject.HasTag(tag))
                    return true;

            return false;
        }
    }
}