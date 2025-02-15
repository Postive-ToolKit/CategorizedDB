using System.Collections.Generic;
using Runtime.Attributes;
using UnityEditor;
using UnityEngine;

namespace Postive.CategorizedDB.Editor.Attributes
{
    [CustomPropertyDrawer(typeof(CategoryElementSelectorAttribute), true)]
    public class CategoryElementSelectorPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            
            if (property.propertyType == SerializedPropertyType.String)
            {
                EditorGUI.BeginProperty(position, label, property);
                //generate the taglist + custom tags
                List<string> tagList = new List<string>();
                
                //get attribute
                var categoryAttribute = attribute as CategoryElementSelectorAttribute;
                if (categoryAttribute == null) {
                    EditorGUI.HelpBox(position, "This attribute can only be applied to CategoryElementSelectorAttribute fields.", MessageType.Error);
                    return;
                }
                if (categoryAttribute.ElementFinder == null) {
                    EditorGUI.HelpBox(position, "Target DB is not set. Please create DB first.", MessageType.Error);
                    return;
                }
                var elements = categoryAttribute.ElementFinder.Elements;

                tagList.Add("NONE");
                foreach (var element in elements) {
                    var path = element.GetPath();
                    var realPath = string.IsNullOrEmpty(path) ? element.Name : path + "/" + element.Name;
                    tagList.Add(realPath);
                }
                var currentPath= "NONE";
                foreach (var element in elements) {
                    if (element.GUID == property.stringValue) {
                        var path = element.GetPath();
                        var realPath = string.IsNullOrEmpty(path) ? element.Name : path + "/" + element.Name;
                        currentPath = realPath;
                        break;
                    }
                }
                
                string propertyString = currentPath;
                int index = 0;
                for (int i = 0; i < tagList.Count; i++) {
                    if (tagList[i] == propertyString) {
                        index = i;
                        break;
                    }
                }
                
                index = EditorGUI.Popup(position, label.text, index, tagList.ToArray());
                
                if (index == 0) {
                    property.stringValue = string.Empty;
                }
                else {
                    property.stringValue = elements[index - 1].GUID;
                }
                
                EditorGUI.EndProperty();
            }
            else
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }
}