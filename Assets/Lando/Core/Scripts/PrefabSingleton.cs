using System.Linq;
using UnityEngine;

namespace Lando.Core
{
    public class PrefabSingleton<T> : MonoBehaviour  where T : MonoBehaviour
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    string typeName = typeof(T).Name;
                    T newInstance = Instantiate(Resources.LoadAll<T>($"Prefabs/").FirstOrDefault());
                    if (newInstance == null)
                        newInstance = new GameObject(typeName, typeof(T)).GetComponent<T>();

                    newInstance.transform.position = Vector3.zero;
                    newInstance.transform.localScale = Vector3.one;
                    newInstance.transform.rotation = Quaternion.identity;
                    
                    DontDestroyOnLoad(newInstance);
                    
                    instance = newInstance;
                }

                return instance;
            }   
        }

        protected virtual void Awake()
        {
            if(instance != null)
                Destroy(gameObject);
        }
    }
}