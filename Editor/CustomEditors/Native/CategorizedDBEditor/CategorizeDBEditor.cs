using Postive.CategorizedDB.Runtime.Categories;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Postive.CategorizedDB.Editor.CustomEditors.Native.CategorizedDBEditor
{
    public abstract class CategorizeDBEditor<T> : EditorWindow where T : CategoryElement
    {
        protected abstract CategorisedElementDB CurrentDB { get; }
    
        protected CategorizeDBElementInspector _inspectorView;
        protected CategorizeDBEditorTreeView<T> _treeView;

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            
            _inspectorView = new CategorizeDBElementInspector();
            _treeView = new CategorizeDBEditorTreeView<T>();

            CategorizeDBEditorSplitView splitView = new CategorizeDBEditorSplitView {
                //set split view default left size
                fixedPaneInitialDimension = 200
            };

            var treeViewContainer = new VisualElement() { style = { flexGrow = 1 } };
            treeViewContainer.Add(_treeView);
            splitView.Add(treeViewContainer);
            
            ColorUtility.TryParseHtmlString("#3C3C3C", out Color color);
            var inspectorViewContainer = new VisualElement() { style = { flexGrow = 1 ,
                backgroundColor = new StyleColor() {
                value = color
            }} };
            
            inspectorViewContainer.Add(_inspectorView);
            splitView.Add(inspectorViewContainer);
            
            root.Add(splitView);
            
            minSize = new Vector2(800, 600);
            
            
            _treeView.DB = CurrentDB;
            _treeView.OnSelectionChanged = _inspectorView.UpdateSelection;
        }
    }
}
