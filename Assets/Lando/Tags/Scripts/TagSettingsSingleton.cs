using Lando.Core;
using UnityEngine;

namespace Lando.Tags
{
    [CreateAssetMenu(fileName = "TagSettings", menuName = "Create/Tag Settings")]
    public class TagSettingsSingleton : ScriptableObjectSingleton<TagSettingsSingleton>
    {
        public TagEntity[] Tags;

        public bool IncludeInactives;
    }
}