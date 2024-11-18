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
        protected abstract string WindowTarget{ get; }
        protected abstract GenericCategorisedDB<T> CurrentDB { get; }
        private const float UPDATE_INTERVAL = 0.1f;

        private Category _selectedCategory;
        private T _selectedData;
        //on window created
        protected override void OnEnable() {
            base.OnEnable();
            this.titleContent = new GUIContent(WindowTarget);
            this.WindowPadding = Vector4.zero;
            this.WindowPadding = Vector4.zero;
            
            this.position = new Rect(100, 100, 1280, 720);
            minSize = new Vector2(800, 600);
        }
        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(true) {
                { WindowTarget, CurrentDB, EditorIcons.File },
            };
            tree.Selection.SupportsMultiSelect = false;
            tree.Config.DrawSearchToolbar = false;
            tree.DefaultMenuStyle.IconSize = 28.00f;
            CurrentDB.OnDataChanged = ForceMenuTreeRebuild;
            var categories = CurrentDB.Categories;
            for (int i = 0; i < categories.Count; i++) {
                var category = categories[i];
                tree.Add(category.GetPath(), category);
            }
            for (int i = 0; i < CurrentDB.Elements.Count; i++) {
                var patternData = CurrentDB.Elements[i];
                string dataName = patternData.Name;
                if (string.IsNullOrEmpty(dataName)) dataName = $"Empty {i}";
                tree.Add($"{patternData.GetPath()}/{dataName}", patternData);
            }
            tree.Selection.SelectionChanged += t => {
                if (tree.Selection.Count == 0) return;
                if (tree.Selection[0].Value is T data) {
                    _selectedData = data;
                    _selectedCategory = null;
                }
                else if (tree.Selection[0].Value is Category category)
                {
                    _selectedCategory = category;
                    _selectedData = null;
                }
                else {
                    _selectedData = null;
                    _selectedCategory = null;
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
            _timer = 0;
            if (_selectedData != null)
            {
                ForceMenuTreeRebuild();
                if (this.MenuTree.Selection.Count != 0) return;
                MenuTree.Selection.Clear();
                MenuTree.Selection.Add(MenuTree.EnumerateTree().First(x => x.Value == _selectedData));
            }
            else if (_selectedCategory != null)
            {
                ForceMenuTreeRebuild();
                if (this.MenuTree.Selection.Count != 0) return;
                MenuTree.Selection.Clear();
                MenuTree.Selection.Add(MenuTree.EnumerateTree().First(x => x.Value == _selectedCategory));
            }
        }
        
        protected override void OnBeginDrawEditors()
        {
            var selected = this.MenuTree.Selection.FirstOrDefault();
            var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;
            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {
                string currentTitle = selected != null ? selected.Name : "No Selection";
                GUILayout.Label(currentTitle);
                OnCreateMenuFront();
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Save"))) {
                    EditorUtility.SetDirty(CurrentDB);
                    AssetDatabase.SaveAssets();
                }
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Category"))) {
                    CurrentDB.AddCategory(_selectedCategory);
                }
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Data"))) {

                    
                    var types = TypeCache.GetTypesDerivedFrom<T>().Where(x => !x.IsAbstract);
                    List<Type> typesList = new List<Type>();
                    if(!typeof(T).IsAbstract) typesList.Add(typeof(T));
                    typesList.AddRange(types);
                    typesList.Sort((x,y) => String.Compare(x.Name, y.Name, StringComparison.Ordinal));
                    switch (typesList.Count) {
                        case 0:
                            break;
                        case 1:
                            CurrentDB.CreateData(typesList[0],_selectedCategory);
                            break;
                        default:
                            List<EditorDropDownSelector.Content> contents = new List<EditorDropDownSelector.Content>();
                            foreach (var type in typesList) {
                                contents.Add(new EditorDropDownSelector.Content() {
                                    Title = $"Add {type.Name}",
                                    OnSelect = () => {
                                        CurrentDB.CreateData(type,_selectedCategory);
                                    }
                                });
                            }
                            var selector = new EditorDropDownSelector(contents.ToArray());
                            selector.ShowInPopup(200);
                            break;
                    }
                    

                }

                if (_selectedData != null||_selectedCategory != null) {
                    if (SirenixEditorGUI.ToolbarButton(new GUIContent("Delete"))) {
                        if (_selectedData != null) {
                            CurrentDB.RemoveData(_selectedData);
                        }
                        else if (_selectedCategory != null) {
                            CurrentDB.RemoveCategory(_selectedCategory);
                            _selectedCategory = null;
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