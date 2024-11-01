using Postive.CategorizedDB.Runtime.Categories;
using UnityEngine.UIElements;

namespace Postive.CategorizedDB.Editor.CustomEditors.Native.CategorizedDBEditor
{
    public class CategorizeDBElementInspector : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<CategorizeDBElementInspector, UxmlTraits> {}
        private UnityEditor.Editor _editor;

        public CategorizeDBElementInspector() {
            style.flexGrow = 1;
            style.paddingBottom = 5;
            style.paddingLeft = 5;
            style.paddingRight = 5;
            style.paddingTop = 5;
        }
        internal void UpdateSelection(CategoryScriptableObject data)
        {
            if (data == null) {
                Clear();
                return;
            }
            Clear();
            UnityEngine.Object.DestroyImmediate(_editor);
            _editor = UnityEditor.Editor.CreateEditor(data);
            IMGUIContainer container = new IMGUIContainer(() => {
                //ignore multiple targets
                if (_editor.targets == null) {
                    return;
                }
                if (_editor.targets.Length > 1) {
                    return;
                }
                if (_editor.target != null) {
                    _editor.OnInspectorGUI();
                }
            });
            Add(container);
        }
        //if inspector value changed
        
    }
}