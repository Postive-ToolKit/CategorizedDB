#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using Postive.CategorizedDB.Runtime.Categories;
using UnityEngine;

namespace DialogSystem.Runtime.Structure.ScriptableObjects
{
    public class DialogPlot : CategoryElement
    {
        private const string DEFAULT_PATH = "Dialogs";
        public DialogPlotGraph Plot => _plot;
        [SerializeField] private DialogPlotGraph _plot;
        public override void CheckIntegrity()
        {
            base.CheckIntegrity();
#if UNITY_EDITOR
            //create folder at resources path and create new dialog plot graph
            //check is DEFAULT_PATH exists
            if (_plot != null)
            {
                _plot.Name = Name + " - " + GUID.Substring(0, 8);
                return;
            }

            if (!AssetDatabase.IsValidFolder($"Assets/Resources/{DEFAULT_PATH}"))
            {
                AssetDatabase.CreateFolder("Assets/Resources", DEFAULT_PATH);
            }

            //check is plot is null
            _plot = CreateInstance<DialogPlotGraph>();
            _plot.name = Name + " - " + GUID.Substring(0, 8);
            AssetDatabase.CreateAsset(_plot, $"Assets/Resources/{DEFAULT_PATH}/{_plot.name}.asset");
            AssetDatabase.SaveAssets();
#endif
        }
        //remove _plot when delete this object
        //create new plot when this object is created
#if UNITY_EDITOR
        private void Awake() {
            CheckIntegrity();
        }
        private void OnDestroy() {
            if (_plot != null) {
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(_plot));
            }
        }
#endif
    }
}