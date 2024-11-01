using UnityEngine.UIElements;

namespace Postive.CategorizedDB.Editor.CustomEditors.Native.CategorizedDBEditor
{
    public class CategorizeDBEditorSplitView : TwoPaneSplitView
    {
        public new class UxmlFactory : UxmlFactory<CategorizeDBEditorSplitView, TwoPaneSplitView.UxmlTraits> {}

        public CategorizeDBEditorSplitView() {
            style.flexGrow = 1;
        }
    }
}