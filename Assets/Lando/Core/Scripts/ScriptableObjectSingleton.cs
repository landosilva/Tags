using System.Linq;
using UnityEngine;

namespace Lando.Core
{
    public class ScriptableObjectSingleton<T> : ScriptableObject where T : ScriptableObject
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    T scriptableObject = Resources.LoadAll<T>($"ScriptableObjects/").FirstOrDefault();
                    if (scriptableObject == null)
                        scriptableObject = CreateInstance(typeof(T)) as T;

                    instance = scriptableObject;
                }

                return instance;
            }   
        }  
    }
}