#if !ODIN_INSPECTOR
using Postive.CategorizedDB.Editor.CustomEditors.Native.CategorizedDBEditor;
using Postive.CategorizedDB.Runtime.Categories;
using Postive.SimpleSoundAssetManager.Runtime;
using UnityEditor;
using UnityEditor.Callbacks;

namespace Postive.SimpleSoundAssetManager.Editor.CustomEditor.SoundDBEditors
{
    public class NativeSoundDBEditor: CategorizeDBEditor
    {
        protected override CategorisedElementDB CurrentDB => SoundDB.Instance;
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            if (Selection.activeObject is SoundDB db) {
                NativeSoundDBEditor wnd = GetWindow<NativeSoundDBEditor>();
                wnd.titleContent = new UnityEngine.GUIContent("Sound DB Editor");
                return true;
            }
            return false;
        }
    }
}
#endif