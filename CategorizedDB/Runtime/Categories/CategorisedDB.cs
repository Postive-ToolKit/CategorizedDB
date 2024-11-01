using System;
using System.Collections.Generic;
using Postive.CategorizedDB.Runtime.Categories.Interfaces;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Postive.CategorizedDB.Runtime.Categories
{
    public abstract class CategorisedDB: ScriptableObject,ICategoryPathFinder
    {
        public List<Category> Categories => new List<Category>(_categories);
        [SerializeField] protected List<Category> _categories = new List<Category>();
        public string GetPath(string parentGuid) {
            if (string.IsNullOrEmpty(parentGuid)) return string.Empty;
            int result = -1;
            for (int i = 0; i < _categories.Count; i++) {
                if (_categories[i].GUID.Equals(parentGuid)) {
                    result = i;
                    break;
                }
            }
            if (result == -1) {
                Debug.LogError($"Parent GUID {parentGuid} not found.");
                return string.Empty;
            }
            return _categories[result].GetPath();
        }

        public CategoryScriptableObject GetParent(string guid) {
            if (string.IsNullOrEmpty(guid)) return null;
            int result = -1;
            for (int i = 0; i < _categories.Count; i++) {
                if (_categories[i].GUID.Equals(guid)) {
                    result = i;
                    break;
                }
            }
            if (result == -1) {
                Debug.LogError($"Parent GUID {guid} not found.");
                return null;
            }
            return _categories[result];
        }
        public bool IsInSameBranch(string parentGuid, string childGuid) {
            if (string.IsNullOrEmpty(parentGuid) || string.IsNullOrEmpty(childGuid)) return false;
            if (parentGuid.Equals(childGuid)) return true;
            List<string> rootPath = new List<string>();
            do {
                CategoryScriptableObject nextPath = GetParent(childGuid);
                if (nextPath == null) break;
                rootPath.Add(nextPath.ParentGUID);
                childGuid = nextPath.ParentGUID;
            }while (true);
            return rootPath.Contains(parentGuid);
        }
#if UNITY_EDITOR
        public void AddCategory(Category selectedCategory = null) {
            var category = CreateInstance<Category>();
            category.name = category.GUID.Substring(0, 5);
            category.DB = this;
            if (selectedCategory != null) category.SetParent(selectedCategory.GUID);
            _categories.Add(category);
            AssetDatabase.AddObjectToAsset(category, this);
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            OnValidate();
        }
        public void RemoveCategory(Category category) {
            _categories.Remove(category);
            DestroyImmediate(category, true);
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            OnValidate();
        }
        public Action OnDataChanged;
        public void OnValidate() {
            CheckIntegrity();
            EditorUtility.SetDirty(this);
            OnDataChanged?.Invoke();

        }
#endif
        protected virtual void CheckIntegrity() { }
    }
}