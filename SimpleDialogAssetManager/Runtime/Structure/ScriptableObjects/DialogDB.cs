using System.Collections.Generic;
using Postive.CategorizedDB.Runtime.Categories;
using UnityEngine;

namespace DialogSystem.Runtime.Structure.ScriptableObjects
{
    [CreateAssetMenu(fileName = "DialogDB", menuName = "Data/DialogDB", order = 0)]
    public class DialogDB : GenericCategorisedDB<DialogPlot>
    {
        private static readonly List<string> STANDARD_TARGETS = new List<string> {
            "===Base Target===",
            "DialogPlotSelector",
        }; 
        public static DialogDB Instance {
            get {
                if (_instance != null) return _instance;
                _instance = Resources.Load<DialogDB>("DialogDB");
#if UNITY_EDITOR
                if (_instance == null) {
                    //create DialogDB to Resources
                    _instance = CreateInstance<DialogDB>();
                    _instance.name = "DialogDB";
                    //save as asset
                    //if resource folder does not exist, create it
                    if (!UnityEditor.AssetDatabase.IsValidFolder("Assets/Resources")) {
                        UnityEditor.AssetDatabase.CreateFolder("Assets", "Resources");
                    }
                    UnityEditor.AssetDatabase.CreateAsset(_instance, "Assets/Resources/DialogDB.asset");
                    UnityEditor.AssetDatabase.SaveAssets();
                }
#endif
                return _instance;
            }
        }
        private static DialogDB _instance;
        public static List<string> DialogTargetIds {
            get {
                var result = new List<string>();
                if (Instance != null) {
                    result.AddRange(Instance._dialogTargetIds);
                }
                result.AddRange(STANDARD_TARGETS);
                return result;
            }
        }
        [SerializeField] private List<string> _dialogTargetIds = new List<string>();
        
    }
}