using System;
using System.Collections.Generic;
using Postive.CategorizedDB.Runtime.Categories.Interfaces;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Postive.CategorizedDB.Runtime.Categories
{
    public abstract class CategorisedElementDB : CategorisedDB , ICategoryElementFinder
    {
        public List<CategoryElement> Elements => new List<CategoryElement>(_elements);
#if ODIN_INSPECTOR
        [TableList]
#endif
        [SerializeField] protected List<CategoryElement> _elements = new List<CategoryElement>();
#if UNITY_EDITOR
        public abstract void CreateData(Type type,Category selectedCategory = null);
        public abstract void RemoveData(CategoryElement data);
#endif
        protected override void CheckIntegrity() {
            base.CheckIntegrity();
#if UNITY_EDITOR
            for(int i = 0; i < _elements.Count; i++) {
                _elements[i].GetPath();
            }    
            _elements.Sort((a,b) => string.Compare(a.Path, b.Path, StringComparison.Ordinal));
#endif
        }
    }
}