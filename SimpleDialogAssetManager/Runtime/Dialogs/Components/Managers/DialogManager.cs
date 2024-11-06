using System.Collections.Generic;
using DialogSystem.Runtime.Structure.ScriptableObjects;
using Postive.SimpleDialogAssetManager.Runtime.Interfaces;
using UnityEngine;

namespace DialogSystem.Dialogs.Components.Managers
{
    public class DialogManager : MonoBehaviour
    {
        /// <summary>
        /// Singleton instance of DialogManager
        /// </summary>
        public static DialogManager Instance {
            get {
                if (_instance != null) {
                    return _instance;
                }
                _instance = FindObjectOfType<DialogManager>();
                if (_instance != null) {
                    return _instance;
                }
                var singleton = new GameObject("DialogManager");
                _instance = singleton.AddComponent<DialogManager>();
                return _instance;
            }
        }
        private static DialogManager _instance;
        /// <summary>
        /// Pause dialog load when IsPause is true
        /// </summary>
        public bool IsPause { get; set; } = false;
        public bool IsStopRequest { get; set; } = false;
        public bool IsPlaying => _currentDialog != null;
        public IDialogHandler[] DialogHandlers => _dialogHandlers.ToArray();
        public DialogGraph CurrentDialog => _currentDialog;
        [SerializeField] private DialogGraph _currentDialog = null;

        private readonly List<IDialogHandler> _dialogHandlers = new List<IDialogHandler>();
        private void Start() {
            _currentDialog.PlayPlot();
            Play();
        }

        /// <summary>
        /// Select dialog plot from dialog set
        /// </summary>
        /// <param name="plotId">Id of Plot</param>
        public void SelectDialogPlot(string plotId)
        {
            #if UNITY_EDITOR
                Debug.Log("Select Dialog Plot: " + plotId);
            #endif
            DialogGraph plot = DialogDB.Instance.Get(plotId)?.Plot;
            if (plot == null) {
                Debug.LogWarning("Plot not found");
                return;
            }
            _currentDialog = plot;
            _currentDialog.PlayPlot();
            Play();
        }
        /// <summary>
        /// Add dialog target to dialog manager
        /// </summary>
        /// <param name="dialogTarget"></param>
        public void AddDialogTarget(IDialogHandler dialogTarget) {
            _dialogHandlers.Add(dialogTarget);
        }
        public void RemoveDialogTarget(IDialogHandler dialogTarget) {
            _dialogHandlers.Remove(dialogTarget);
        }
        /// <summary>
        /// Load dialog from dialog graph
        /// </summary>
        public void Play()
        {
            //If dialog is paused, return
            if (IsPause) return;
            //If dialog is stop request, return
            if (IsStopRequest) return;
            //If current dialog plot is null, return
            if (_currentDialog == null) return;
            if (_currentDialog.IsPlotEnd) {
                Debug.Log("Dialog End");
                EndPlot();
                return;
            }
            //Read Plot
            _currentDialog.Play(this);
            if (_currentDialog.IsPlotEnd) {
                Debug.Log("Dialog End");
                EndPlot();
            }
        }
        /// <summary>
        /// Invoke when dialog is end
        /// </summary>
        public void EndPlot()
        {
            _currentDialog = null;
            _dialogHandlers.ForEach(handler => handler.OnEndPlot());
        }
        /// <summary>
        /// Clear all data when dialog manager disabled
        /// </summary>
        private void OnDisable()
        {
            _dialogHandlers.Clear();
        }

    }
}