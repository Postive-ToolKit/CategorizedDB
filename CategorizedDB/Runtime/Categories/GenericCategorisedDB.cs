using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Postive.CategorizedDB.Runtime.Categories
{
    public class GenericCategorisedDB<T> : CategorisedElementDB where T : CategoryElement
    {
        private Dictionary<string,CategoryElement> _cache = new Dictionary<string, CategoryElement>();
        public T Get(string guid) {
            if (_cache.TryGetValue(guid, out var data)) return (T)data;
            T result = null;
            for (int i = 0; i < _elements.Count; i++) {
                if (!_elements[i].GUID.Equals(guid)) continue;
                result = _elements[i] as T;
                _cache.Add(guid,result);
                break;
            }
            return result;
        }
#if UNITY_EDITOR
        public override void CreateData(Category selectedCategory = null)
        {
            T data = CreateInstance<T>();
            if (data == null) {
                Debug.LogError("Data is null.");
                return;
            }
            if (selectedCategory != null) data.SetParent(selectedCategory.GUID);
            data.DB = this;
            data.name = data.GUID.Substring(0, 5);
            _elements.Add(data);
            AssetDatabase.AddObjectToAsset(data,this);
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            OnValidate();
        }
        public override void RemoveData(CategoryElement data)
        {
            for (int i = 0; i < _elements.Count; i++) {
                if (!_elements[i].GUID.Equals(data.GUID)) continue;
                _elements.RemoveAt(i);
                DestroyImmediate(data,true);
                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssets();
                break;
            }
            OnValidate();
        }
#endif
    }
}