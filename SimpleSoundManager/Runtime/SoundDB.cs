using Postive.CategorizedDB.Runtime.Categories;
using Postive.SimpleSoundAssetManager.Runtime.Data;
using UnityEngine;

namespace Postive.SimpleSoundAssetManager.Runtime
{
    [CreateAssetMenu(fileName = "SoundDB", menuName = "Data/SoundDB")]
    public class SoundDB : GenericCategorisedDB<SoundData>{
        public static SoundDB Instance {
            get {
                if (_instance == null) {
                    _instance = Resources.Load<SoundDB>("SoundDB");
                }
                return _instance;
            }
        }
        private static SoundDB _instance = null;
    }
}