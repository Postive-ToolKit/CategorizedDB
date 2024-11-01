using UnityEngine.UIElements;

namespace Postive.SimpleDialogAssetManager.Editor.CustomEditors
{
    public class DialogEditorSplitView : TwoPaneSplitView
    {
        public new class UxmlFactory : UxmlFactory<DialogEditorSplitView, TwoPaneSplitView.UxmlTraits> {}
    }
}