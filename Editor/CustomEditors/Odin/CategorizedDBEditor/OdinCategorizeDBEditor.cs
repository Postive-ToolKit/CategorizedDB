#if ODIN_INSPECTOR
using System;
using System.Collections.Generic;
using System.Linq;
using Postive.CategorizedDB.Runtime.Categories;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Postive.CategorizedDB.Editor.CustomEditors.Odin.CategorizedDBEditor
{
    public abstract class OdinCategorizeDBEditor<T> : OdinMenuEditorWindow where T : CategoryElement
    {
        protected abstract GenericCategorisedDB<T> CurrentDB { get; }
        private const float UPDATE_INTERVAL = 0.1f;
        
        private CategoryScriptableObject _selected;
        //on window created
        private bool _rebuildRequested = false;
        private void RequestRebuild() {
            _rebuildRequested = true;
        }
        protected override void OnEnable() {
            base.OnEnable();
            this.WindowPadding = Vector4.zero;
            this.WindowPadding = Vector4.zero;
            
            // this.position = new Rect(100, 100, 1280, 720);
            // minSize = new Vector2(800, 600);
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(true) {
                { CurrentDB.name, CurrentDB, EditorIcons.File },
            };
            tree.Selection.SupportsMultiSelect = false;
            tree.Config.DrawSearchToolbar = false;
            tree.DefaultMenuStyle.IconSize = 28.00f;
            CurrentDB.OnDataChanged = RequestRebuild;
            var categories = CurrentDB.Categories;
            for (int i = 0; i < categories.Count; i++) {
                var category = categories[i];
                tree.Add(category.GetPath(), category);
            }
            Dictionary<string,int> samePath = new Dictionary<string, int>();
            for (int i = 0; i < CurrentDB.Elements.Count; i++) {
                var element = CurrentDB.Elements[i];
                string dataName = element.Name;
                element.OnDataChanged = RequestRebuild;
                if (string.IsNullOrEmpty(dataName)) dataName = $"Empty {i}";
                if (samePath.ContainsKey(dataName)) {
                    samePath[dataName]++;
                    dataName = $"{dataName} ({samePath[dataName]})";
                }
                else {
                    samePath.Add(dataName, 0);
                }
                tree.Add($"{element.GetPath()}/{dataName}", element);
            }
            tree.Selection.SelectionChanged += t => {
                if (tree.Selection.Count == 0) return;
                if (tree.Selection[0].Value is CategoryScriptableObject data) {
                    _selected = data;
                }
                else {
                    _selected = null;
                }
            };
            return tree;
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            CurrentDB.OnDataChanged = null;
        }
        private float _timer;
        private void OnInspectorUpdate()
        {
            _timer += Time.deltaTime;
            if (_timer < UPDATE_INTERVAL) return;
            if (!_rebuildRequested) return;
            _timer = 0;
            //save last selection
            var selected = this.MenuTree.Selection.FirstOrDefault();
            if (_selected != null) {
                ForceMenuTreeRebuild();
                if (this.MenuTree.Selection.Count != 0) return;
                MenuTree.Selection.Clear();
                MenuTree.Selection.Add(selected);
            }
        }
        
        protected override void OnBeginDrawEditors()
        {
            
            var selected = this.MenuTree.Selection.FirstOrDefault();
            var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;
            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {
                string currentTitle = selected != null ? selected.Name : "No Selection";
                Category category = _selected as Category;
                GUILayout.Label(currentTitle);
                OnCreateMenuFront();
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Save"))) {
                    EditorUtility.SetDirty(CurrentDB);
                    AssetDatabase.SaveAssets();
                }
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Category"))) {
                    CurrentDB.AddCategory(category);
                }
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Element"))) {
                    var types = TypeCache.GetTypesDerivedFrom<T>().Where(x => !x.IsAbstract);
                    List<Type> typesList = new List<Type>();
                    if(!typeof(T).IsAbstract) typesList.Add(typeof(T));
                    typesList.AddRange(types);
                    typesList.Sort((x,y) => String.Compare(x.Name, y.Name, StringComparison.Ordinal));
                    switch (typesList.Count) {
                        case 0:
                            break;
                        case 1:
                            CurrentDB.CreateData(typesList[0],category);
                            break;
                        default:
                            List<EditorDropDownSelector.Content> contents = new List<EditorDropDownSelector.Content>();
                            foreach (var type in typesList) {
                                contents.Add(new EditorDropDownSelector.Content() {
                                    Title = $"{type.Name}",
                                    OnSelect = () => {
                                        CurrentDB.CreateData(type,category);
                                    }
                                });
                            }
                            var selector = new EditorDropDownSelector(contents.ToArray());
                            selector.ShowInPopup(200);
                            break;
                    }
                    

                }

                if (_selected != null) {
                    if (SirenixEditorGUI.ToolbarButton(new GUIContent("Delete"))) {
                        if (_selected is Category) {
                            CurrentDB.RemoveCategory(_selected as Category);
                        }
                        else {
                            CurrentDB.RemoveData(_selected as T);
                        }
                    }
                }
                OnCreateMenuBehind();
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }
        protected virtual void OnCreateMenuFront() { }
        protected virtual void OnCreateMenuBehind() { }
    }
}
#endif