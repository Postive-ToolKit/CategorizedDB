using UnityEngine;
using UnityEngine.UIElements;

namespace Postive.SimpleDialogAssetManager.Editor.CustomEditors.PlotEditors
{
    public class DialogEditorInspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<DialogEditorInspectorView, VisualElement.UxmlTraits> {}
        private UnityEditor.Editor _editor;
        public DialogEditorInspectorView()
        {
            style.flexGrow = 1;
            style.paddingBottom = 5;
            style.paddingTop = 5;
            style.paddingLeft = 5;
            style.paddingRight = 5;
        }
        internal void UpdateSelection(ScriptableObject data)
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
    }
}