using UnityEditor.IMGUI.Controls;

namespace Lando.Tags
{
    public class TagSettingsDropdownItem : AdvancedDropdownItem
    {
        public readonly TagEntity TagEntity;
        
        public TagSettingsDropdownItem(TagEntity tagEntity) : base(tagEntity.Name)
        {
            TagEntity = tagEntity;
        }
    }
}