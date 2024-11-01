#if ODIN_INSPECTOR
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
                    CurrentDB.CreateData(_selectedCategory);
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