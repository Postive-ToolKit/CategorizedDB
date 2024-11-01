﻿using System;
using System.Collections.Generic;
using Postive.CategorizedDB.Runtime.Categories;
using UnityEngine;
using UnityEngine.UIElements;

namespace Postive.CategorizedDB.Editor.CustomEditors.Native.CategorizedDBEditor
{
    public class CategorizeDBEditorTreeView : TreeView
    {
        public Action<ContextualMenuPopulateEvent> OnCreateContextMenu;
        public new class UxmlFactory : UxmlFactory<CategorizeDBEditorTreeView, TreeView.UxmlTraits> {}
        public Action<CategoryScriptableObject> OnSelectionChanged;
        private Category _selectedCategory = null;
        private CategoryElement _selectedData = null;

        public CategorisedElementDB DB {
            get => _db;
            set {
                _db = value;
                _wasRebuildRequested = true;
                Update();
            }
        }
        private CategorisedElementDB _db;
        private int _selectedId;
        private Dictionary<string,int> _expandedIds = new Dictionary<string, int>();
        private Dictionary<string,int> _elementIds = new Dictionary<string, int>();
        private int _currentId = 0;
        private bool _wasRebuildRequested = false;
        public CategorizeDBEditorTreeView()
        {
            style.flexGrow = 1;
            
            schedule.Execute(Update).Every(100);
            BuildTree();
            makeItem = () => new Label() {
                style = {
                    unityTextAlign = TextAnchor.MiddleLeft,
                    flexGrow = 1
                },
            };
            bindItem = CreateVisualElement;
            selectionChanged += SelectionChanged;
            this.AddManipulator(new ContextualMenuManipulator(BuildContextMenu));
            //add click event
            this.RegisterCallback<MouseDownEvent>(evt => {
                if (evt.clickCount == 2) {
                    SetSelection(new List<int>());
                    _selectedCategory = null;
                    _selectedData = null;
                }
            });
            
        }

        private void CreateVisualElement(VisualElement element, int index) {
            CategoryScriptableObject data = GetItemDataForIndex<CategoryScriptableObject>(index);
            (element as Label).text = string.IsNullOrEmpty(data.Name) ? $"Empty - {data.GUID.Substring(0,5)}" : data.Name;
            string lastParentGUID = data.ParentGUID;
            data.OnDataChanged = () => {
                RefreshItem(index);
                _wasRebuildRequested = !lastParentGUID.Equals(data.ParentGUID);
            };
        }
        private void OnExpandedChanged(TreeViewExpansionChangedArgs changeState) {
            CategoryScriptableObject data = GetItemDataForIndex<CategoryScriptableObject>(changeState.id);
            if(data == null) return;
            if (changeState.isExpanded) {
                if (!_expandedIds.ContainsKey(data.GUID)) {
                    _expandedIds.Add(data.GUID,changeState.id);
                }
            }
            else if (_expandedIds.ContainsKey(data.GUID)) {
                _expandedIds.Remove(data.GUID);
            }

        }
        private void BuildTree() {
            if (_db == null) return;
            itemExpandedChanged -= OnExpandedChanged;
            _db.OnDataChanged = RequestRebuild;
            _currentId = 0;
            List<TreeViewItemData<CategoryScriptableObject>> rootItems = new List<TreeViewItemData<CategoryScriptableObject>>();
            
            List<CategoryScriptableObject> expression = new List<CategoryScriptableObject>();
            expression.AddRange(_db.Elements);
            expression.AddRange(_db.Categories);
            foreach (var data in expression.FindAll(x => string.IsNullOrEmpty(x.ParentGUID))) {
                rootItems.Add(GetTreeViewItems(data, expression));
            }
            SetRootItems(rootItems);
            itemExpandedChanged += OnExpandedChanged;
        }
        private TreeViewItemData<CategoryScriptableObject> GetTreeViewItems(CategoryScriptableObject data,List<CategoryScriptableObject> expression)
        {
            var children = new List<CategoryScriptableObject>();
            children.AddRange(expression.FindAll(x => x.ParentGUID.Equals(data.GUID)));
            int currentCreatedId = _currentId;
            if(_expandedIds.ContainsKey(data.GUID)) _expandedIds[data.GUID] = currentCreatedId;
            _currentId++;
            if (children.Count > 0) {
                var root = new List<TreeViewItemData<CategoryScriptableObject>>();
                foreach (var dataItem in children) {
                    root.Add(GetTreeViewItems(dataItem, expression));
                }
                return new TreeViewItemData<CategoryScriptableObject>(currentCreatedId, data, root);
            }
            return new TreeViewItemData<CategoryScriptableObject>(currentCreatedId, data);
        }
        private void Update() {
            if (!_wasRebuildRequested) return;
            _wasRebuildRequested = false;
            BuildTree();
            Rebuild();
            //remove null items
            List<string> removedKeys = new List<string>();
            foreach (var pair in _expandedIds) {
                if (GetItemDataForId<CategoryScriptableObject>(pair.Value) == null) {
                    removedKeys.Add(pair.Key);
                }
            }
            foreach (var key in removedKeys) {
                _expandedIds.Remove(key);
            }
            foreach (var id in _expandedIds.Values) {
                ExpandItem(id);
            }
        }
        private void RequestRebuild() {
            _wasRebuildRequested = true;
        }
        private void BuildContextMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction(_selectedData == null ? "Target : Root":  $"Target : {_selectedCategory.Name}",null);
            evt.menu.AppendSeparator();
            evt.menu.AppendAction("Add Category", (action) => {
                _db.AddCategory(_selectedCategory);
            });
            evt.menu.AppendAction("Add Element", (action) => {
                _db.CreateData(_selectedCategory);
            });
            if (_selectedData != null || _selectedCategory != null) {
                evt.menu.AppendAction("Delete", (action) => {
                    if (_selectedData != null) {
                        _db.RemoveData(_selectedData);
                    }
                    else if (_selectedCategory != null) {
                        _db.RemoveCategory(_selectedCategory);
                        _selectedCategory = null;
                    }
                    RequestRebuild();
                });
            }
            OnCreateContextMenu?.Invoke(evt);
        }
        private void SelectionChanged(IEnumerable<object> items) {
            var item = items.GetEnumerator();
            if (!item.MoveNext()) return;
            var data = item.Current;
            if (data is CategoryElement categoryElement) {
                _selectedData = categoryElement;
                OnSelectionChanged?.Invoke(categoryElement);
            }
            if (data is Category category) {
                _selectedCategory = category;
                OnSelectionChanged?.Invoke(category);
            }
        }
    }
}