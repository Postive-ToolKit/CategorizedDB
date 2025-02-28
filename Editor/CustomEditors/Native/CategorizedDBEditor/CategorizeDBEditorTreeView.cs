using System;
using System.Collections.Generic;
using System.Linq;
using Postive.CategorizedDB.Editor.CustomEditors.Native.CategorizedDBEditor.TreeBuilders;
using Postive.CategorizedDB.Runtime.Categories;
using Postive.CategorizedDB.Runtime.Categories.Interfaces;
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
            }
        }
        public TreeViewItemBuilder ItemBuilder {
            get => _itemBuilder;
            set {
                if (_itemBuilder == null) return;
                _itemBuilder = value;
                _itemBuilder.GetExpandedIds = () => _expandedIds;
                _wasRebuildRequested = true;
            }
        }
        private CategorisedElementDB _db;
        private TreeViewItemBuilder _itemBuilder = new DefaultTreeViewItemBuilder();
        private int _selectedId;
        private bool _wasRebuildRequested = false;
        private Dictionary<string,int> _expandedIds = new Dictionary<string, int>();
        public CategorizeDBEditorTreeView()
        {
            style.flexGrow = 1;
            ItemBuilder = ItemBuilder;
            itemExpandedChanged += OnExpandedChanged;
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
            BuildTree();
            schedule.Execute(Update).Every(100);
        }
        private void CreateVisualElement(VisualElement element, int index) {
            ICategoryElement ice= GetItemDataForIndex<ICategoryElement>(index);
            (element as Label).text = string.IsNullOrEmpty(ice.Name) ? $"Empty - {ice.GUID.Substring(0,5)}" : ice.Name;
            if (ice.Data == null) return;
            string lastParentGUID = ice.Data.ParentGUID;
            ice.Data.OnDataChanged = () => {
                RefreshItem(index);
                _wasRebuildRequested = !lastParentGUID.Equals(ice.Data.ParentGUID);
            };
        }
        private void OnExpandedChanged(TreeViewExpansionChangedArgs changeState) {
            ICategoryElement ice = GetItemDataForIndex<ICategoryElement>(changeState.id);
            if(ice == null) return;
            if (changeState.isExpanded) {
                if (!_expandedIds.ContainsKey(ice.GUID)) {
                    _expandedIds.Add(ice.GUID,changeState.id);
                }
            }
            else if (_expandedIds.ContainsKey(ice.GUID)) {
                _expandedIds.Remove(ice.GUID);
            }
        }
        private void BuildTree() {
            if (_db == null) return;
            _db.OnDataChanged = RequestRebuild;
            List<CategoryScriptableObject> elements = new List<CategoryScriptableObject>();
            elements.AddRange(_db.Elements);
            elements.AddRange(_db.Categories);
            List<TreeViewItemData<ICategoryElement>> rootItems = _itemBuilder.BuildTreeItems(elements);
            SetRootItems(rootItems);
        }
        private void Update() {
            if (!_wasRebuildRequested) return;
            _wasRebuildRequested = false;
            BuildTree();
            Rebuild();
            //remove null items
            List<string> removedKeys = new List<string>();
            foreach (var pair in _expandedIds) {
                if (GetItemDataForId<ICategoryElement>(pair.Value) == null) {
                    removedKeys.Add(pair.Key);
                }
            }
            foreach (var key in removedKeys) {
                _expandedIds.Remove(key);
            }
            List<int> keys = new List<int>(_expandedIds.Values);
            keys.ForEach((id) => {
                ExpandItem(id);
            });
        }
        private void RequestRebuild() {
            _wasRebuildRequested = true;
        }
        private void BuildContextMenu(ContextualMenuPopulateEvent evt)
        {
            Category currentCategory = _selected as Category;
            evt.menu.AppendAction(currentCategory == null ? "Target : Root":  $"Target : {currentCategory.Name}",null);
            // Create View Setting
            evt.menu.AppendAction("View/Category View", (action) => {
                ItemBuilder = new DefaultTreeViewItemBuilder();
            });
            evt.menu.AppendAction("View/Class Name View", (action) => {
                ItemBuilder = new ClassNameViewItemBuilder();
            });
            evt.menu.AppendAction("View/Class Tree View", (action) => {
                ItemBuilder = new ClassTreeViewItemBuilder();
            });
            
            
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
            if (data is ICategoryElement ice) {
                _selected = ice.Data;
                OnSelectionChanged?.Invoke(_selected);
            }
        }
    }
}