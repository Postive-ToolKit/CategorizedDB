using System;
using System.Collections.Generic;
using Postive.CategorizedDB.Runtime.Attributes;
using Postive.CategorizedDB.Runtime.Categories;
using Postive.CategorizedDB.Runtime.Categories.Interfaces;
using UnityEditor;
using UnityEngine;

namespace Postive.CategorizedDB.Editor.Attributes
{
    [CustomPropertyDrawer(typeof(CategorySelectorAttribute), true)]
    public class CategorySelectorPropertyDrawer : PropertyDrawer {
        //private string[] _ignoreProperties = new string[] {"_guid", "_parentGUID"};
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String) {
                EditorGUI.HelpBox(position, "This attribute can only be applied to string fields.", MessageType.Error);
                return;
            }
            CategoryScriptableObject category = property.serializedObject.targetObject as CategoryScriptableObject;
            if (category == null) {
                EditorGUI.HelpBox(position, "This attribute can only be applied to CategoryScriptableObject fields.", MessageType.Error);
                return;
            }
            string parentGuid = property.stringValue;
            string guid = category.GUID;
            CategorisedDB db = category.DB;
            
            ICategoryPathFinder pathFinder = db; 
            if (pathFinder == null) {
                Debug.LogError("PathFinder is null.");
                return;
            }
            
            var categories = pathFinder.Categories;
            
            categories.Sort((a, b) => String.Compare(a.GetPath(), b.GetPath(), StringComparison.Ordinal));
            
            EditorGUI.BeginProperty(position, label, property);
            //generate the taglist + custom tags
            List<string> selections = new List<string>();
            selections.Add("NONE");
            for (int i = 0; i < categories.Count; i++) {
                //Debug.Log(categories[i].GetPath());
                selections.Add(categories[i].GetPath());
            }
            
            string currentCategory = pathFinder.GetPath(parentGuid);
            currentCategory = string.IsNullOrEmpty(currentCategory) ? "NONE" : currentCategory;
            
            string propertyString = currentCategory;
            int index = 0;
            for (int i = 0; i < selections.Count; i++) {
                if (selections[i] == propertyString) {
                    index = i;
                    break;
                }
            }
            index = EditorGUI.Popup(position, label.text, index, selections.ToArray());
            string result = property.stringValue;
            if (index == 0) {
                result = string.Empty;
            }
            else {
                if (categories[index - 1].GUID.Equals(guid)) {
                    Debug.LogError("Category cannot be parent of itself.");
                }
                else if (category is Category) {
                    if (!pathFinder.IsInSameBranch(categories[index - 1].GUID, guid) && !pathFinder.IsInSameBranch(guid, categories[index - 1].GUID)) {
                        result = categories[index - 1].GUID;
                    }
                }
                else {
                    result = categories[index - 1].GUID;
                }
            }
            if (!result.Equals(property.stringValue)) {
                property.stringValue = result;
                //db.OnValidate();
                //category.OnParentChanged?.Invoke();
            }
            
            EditorGUI.EndProperty();
        }
    }
}