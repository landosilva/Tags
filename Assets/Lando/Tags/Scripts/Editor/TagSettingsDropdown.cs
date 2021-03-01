using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Lando.Tags
{
    public class TagSettingsDropdown : AdvancedDropdown
    {
        public delegate void WeekdaysItemDelegate(TagEntity tagEntity);
        public event WeekdaysItemDelegate OnItemSelected;
        
        public TagSettingsDropdown(AdvancedDropdownState state) : base(state)
        {
            this.minimumSize = new Vector2(200, EditorGUIUtility.singleLineHeight * 6 + 44);
        }

        protected override AdvancedDropdownItem BuildRoot()
        {
            AdvancedDropdownItem root = new AdvancedDropdownItem("Tags");

            foreach (TagEntity tagEntity in TagSettingsSingleton.Instance.Tags)
                root.AddChild(new TagSettingsDropdownItem(tagEntity));

            return root;
        }

        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            base.ItemSelected(item);
            
            TagSettingsDropdownItem tagSettingsDropdownItem = (TagSettingsDropdownItem) item;
            OnItemSelected?.Invoke(tagSettingsDropdownItem.TagEntity);
        }
    }
}