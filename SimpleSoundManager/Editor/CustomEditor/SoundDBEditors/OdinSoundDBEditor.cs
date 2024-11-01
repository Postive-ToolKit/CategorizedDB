#if ODIN_INSPECTOR
using Postive.CategorizedDB.Editor.CustomEditors.Odin.CategorizedDBEditor;
using Postive.CategorizedDB.Runtime.Categories;
using Postive.SimpleSoundAssetManager.Runtime;
using Postive.SimpleSoundAssetManager.Runtime.Data;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;


namespace Postive.SimpleSoundAssetManager.Editor.CustomEditor.SoundDBEditors
{
    public class OdinSoundDBEditor: OdinCategorizeDBEditor<SoundData>
    {
        protected override string WindowTarget => "Sound DB";
        protected override GenericCategorisedDB<SoundData> CurrentDB => SoundDB.Instance;
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            if (Selection.activeObject is SoundDB db)
            {
                OdinSoundDBEditor wnd = GetWindow<OdinSoundDBEditor>();
                wnd.position = GUIHelper.GetEditorWindowRect().AlignCenter(1280, 720);
                wnd.titleContent = new GUIContent($"Sound DB Editor");
                return true;
            }
            return false;
        }
    }
}
#endif