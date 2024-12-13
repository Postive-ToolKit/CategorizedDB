using System;
using System.Collections.Generic;
using System.Linq;
using Postive.CategorizedDB.Runtime.Categories;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Postive.CategorizedDB.Editor.CustomEditors.Native.CategorizedDBEditor
{
    public class CategorizeDBEditorTreeView<T> : TreeView where T : CategoryElement
    {
        public Action<ContextualMenuPopulateEvent> OnCreateContextMenu;
        public Action<ScriptableObject> OnSelectionChanged;
        private CategoryScriptableObject _selected = null;
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
                    _selected = null;
                    OnSelectionChanged?.Invoke(DB);
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
            Category currentCategory = _selected as Category;
            evt.menu.AppendAction(currentCategory == null ? "Target : Root":  $"Target : {currentCategory.Name}",null);
            evt.menu.AppendSeparator();
            evt.menu.AppendAction("Add Category", (action) => {
                _db.AddCategory(currentCategory);
            });
            
            var types = TypeCache.GetTypesDerivedFrom<T>().Where(x => !x.IsAbstract);
            List<Type> typesList = new List<Type>();
            if(!typeof(T).IsAbstract) typesList.Add(typeof(T));
            typesList.AddRange(types);
            typesList.Sort((x,y) => String.Compare(x.Name, y.Name, StringComparison.Ordinal));
            switch (typesList.Count) {
                case 0:
                    break;
                case 1:
                    evt.menu.AppendAction($"Add {typesList[0].Name}", (action) => {
                        _db.CreateData(typesList[0],currentCategory);
                    });
                    break;
                default:
                    foreach (var type in typesList) {
                        evt.menu.AppendAction($"Add Element/{type.Name}", (action) => {
                            _db.CreateData(type,currentCategory);
                        });
                    }
                    break;
            }
            
            if (_selected != null) {
                evt.menu.AppendAction("Delete", (action) => {
                    if (_selected is Category category) {
                        _db.RemoveCategory(category);
                    }
                    else if (_selected is CategoryElement element) {
                        _db.RemoveData(element);
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
            if (data is CategoryScriptableObject categoryScriptable) {
                _selected = categoryScriptable;
                OnSelectionChanged?.Invoke(_selected);
            }
        }
    }
}