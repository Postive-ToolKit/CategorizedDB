using System;
using System.Collections.Generic;
using Postive.CategorizedDB.Editor.CustomEditors.Native.CategorizedDBEditor.Data;
using Postive.CategorizedDB.Runtime.Categories;
using Postive.CategorizedDB.Runtime.Categories.Interfaces;
using UnityEngine.UIElements;

namespace Postive.CategorizedDB.Editor.CustomEditors.Native.CategorizedDBEditor.TreeBuilders
{
    public class ClassTreeViewItemBuilder : TreeViewItemBuilder
    {
        private int _currentId = 0;
        private bool _isInitialized = false;
        private List<Type> _classTypes = new List<Type>();
        private Dictionary<Type, List<Type>> _classTree = new Dictionary<Type, List<Type>>();
        private Type _rootType = null;

        private void Initialize(List<CategoryScriptableObject> elements)
        {
            //create class data
            foreach (var element in elements) {
                if (element is Category) continue;
                Type type = element.GetType();
                if (!_classTypes.Contains(type)) {
                    _classTypes.Add(type);
                }
            }
            Type rootType = _classTypes[0];
            while (rootType.BaseType != typeof(CategoryElement)) {
                rootType = rootType.BaseType;
            }
            if(rootType == null) {
                throw new Exception("Root type not found");
            }
            _rootType = rootType;
            foreach (var type in _classTypes) {
                if (type.BaseType == null) continue;
                if (!_classTree.ContainsKey(type.BaseType)) {
                    _classTree.Add(type.BaseType, new List<Type>());
                }
                _classTree[type.BaseType].Add(type);
            }
        }
        public override List<TreeViewItemData<ICategoryElement>> BuildTreeItems(List<CategoryScriptableObject> elements)
        {
            if(!_isInitialized) {
                Initialize(elements);
                _isInitialized = true;
            }
            _currentId = 0;
            List<TreeViewItemData<ICategoryElement>> rootItems = new List<TreeViewItemData<ICategoryElement>>();
            //먼저 클래스 서브클래스 탐색을 해서 탐색할 트리를 먼저 구성한다
            //이후 데이터들을 탐색해서 해당 클래스의 타입을 가져와 딕셔너리에 저장한다
            //이후 탐색을 통해 트리를 완성한다
            Dictionary<Type,List<CategoryScriptableObject>> clsDict = new Dictionary<Type,List<CategoryScriptableObject>>();
            foreach (var element in elements) {
                if (element is Category) continue;
                Type type = element.GetType();
                if (!clsDict.ContainsKey(type)) {
                    clsDict.Add(type, new List<CategoryScriptableObject>());
                }
                clsDict[type].Add(element);
            }
            //dfs class tree to build tree view
            rootItems.Add(TraversalClass(_rootType, clsDict));
            return rootItems;
        }
        private TreeViewItemData<ICategoryElement> TraversalClass(Type type, Dictionary<Type, List<CategoryScriptableObject>> clsDict) {
            int currentCreatedId = _currentId++;
            var childItems = new List<TreeViewItemData<ICategoryElement>>();
            //Add sub class items first
            if (_classTree.ContainsKey(type)) {
                foreach (var childType in _classTree[type]) {
                    childItems.Add(TraversalClass(childType, clsDict));
                }
            }
            //Add data items
            if (clsDict.ContainsKey(type)) {
                foreach (var data in clsDict[type]) {
                    childItems.Add(new TreeViewItemData<ICategoryElement>(_currentId++, data));
                }
            }
            //first add sub class items
            return new TreeViewItemData<ICategoryElement>(currentCreatedId, new EmptyNameCategory(type.Name),childItems);
        }
    }
}