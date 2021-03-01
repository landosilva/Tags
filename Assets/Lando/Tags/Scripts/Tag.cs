using UnityEngine;

namespace Lando.Tags
{
    public sealed partial class Tag : MonoBehaviour
    {
        [SerializeField] private TagEntity[] tags;
        
        private void Awake()
        {
            TagManager.RegisterGameObject(gameObject, tags);
        }

        private void OnDestroy()
        {
            TagManager.UnregisterGameObject(gameObject, tags);
        }
    }
}