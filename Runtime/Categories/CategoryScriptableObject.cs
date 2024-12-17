using System;
using Postive.CategorizedDB.Runtime.Attributes;
using Postive.CategorizedDB.Runtime.Categories.Interfaces;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace Postive.CategorizedDB.Runtime.Categories
{
    [Serializable]
    public abstract class CategoryScriptableObject : ScriptableObject, ICategoryElement,ICategoryObject
    {
        public string GUID => _guid;
        public string Name => _name;
        public string ParentGUID => _parentGUID;

        public ICategoryPathFinder DB {
            get => _db;
            set => _db = value as CategorisedDB;
        }
#if ODIN_INSPECTOR
        [HideInTables]
#endif
        [CategorySelector]
        [SerializeField] protected string _parentGUID = string.Empty;
        #if UNITY_EDITOR
        public string Path => _path;
        
#if ODIN_INSPECTOR
        [TableColumnWidth(200, Resizable = false)]
        [ReadOnly]
#endif
        [SerializeField] private string _path = string.Empty;
        #endif
        [HideInInspector][SerializeField] private CategorisedDB _db;
        [HideInInspector][SerializeField] private string _guid = System.Guid.NewGuid().ToString();
#if ODIN_INSPECTOR
        [TableColumnWidth(100, Resizable = false)]
#endif
        [SerializeField] private string _name = System.Guid.NewGuid().ToString().Substring(0, 8);
        public void SetParent(string parentGuid) {
            _parentGUID = parentGuid;
        }
        public virtual string GetPath() {
            string result = DB.GetPath(_parentGUID);
            #if UNITY_EDITOR
            _path = result;
            #endif
            if (!string.IsNullOrEmpty(result)) return result;
            if (!string.IsNullOrEmpty(_parentGUID)) {
                Debug.LogError($"Parent category not found.");
            }
            _parentGUID = string.Empty;
            return string.Empty;
        }
        #if UNITY_EDITOR
        public Action OnDataChanged;
        private void OnValidate() {
            CheckIntegrity();
            OnDataChanged?.Invoke();
            
        }
        #endif
        public virtual void CheckIntegrity() { }
    }
}