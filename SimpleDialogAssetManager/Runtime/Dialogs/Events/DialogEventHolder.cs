using System;
using System.Collections.Generic;
using System.Text;
using DialogSystem.Dialogs.Components.Managers;
using UnityEngine;

namespace Postive.SimpleDialogAssetManager.Runtime.Dialogs.Events
{
    [Serializable]
    public struct DialogEventHolder : IEventState
    {
        public bool UseSkip {
            get {
                if (_dialogEvents == null||_dialogEvents.Count <= 0) return true;
                foreach (var dialogEvent in _dialogEvents) {
                    if (dialogEvent.UseSkip) return true;
                }
                return false;
            }
        }
        public bool IsEventFinished {
            get {
                if (_dialogEvents == null||_dialogEvents.Count <= 0) return true;
                foreach (var dialogEvent in _dialogEvents) {
                    if (!dialogEvent.IsEventFinished) return false;
                }
                return true;
            }
        }

        public string Content {
            get {
                StringBuilder sb = new StringBuilder();
                for(int i = 0; i < _dialogEvents.Count; i++){
                    if (_dialogEvents[i] == null) continue;
                    sb.Append(_dialogEvents[i].Content);
                    sb.Append("\n");
                }
                return sb.ToString();
            }
        }
        [SerializeReference] [SerializeField] private List<DialogEvent> _dialogEvents;
#if UNITY_EDITOR
        [SerializeField] private string _type;
#endif
        public DialogEventHolder(List<DialogEvent> dialogEvents)
        {
            _type = string.Empty;
            _dialogEvents = dialogEvents;

        }
        public void Invoke(DialogManager manager) {
            if (_dialogEvents == null||_dialogEvents.Count <= 0) return;
            for(int i = 0; i < _dialogEvents.Count; i++){
                var dialogEvent = _dialogEvents[i];
                if (dialogEvent == null) continue;
                dialogEvent.Invoke(manager);
            }
        }
        public void AddEvent(DialogEvent dialogEvent) {
            if (_dialogEvents == null) _dialogEvents = new List<DialogEvent>();
            _dialogEvents.Add(dialogEvent);
        }
        public void RemoveEvent(DialogEvent dialogEvent) {
            if (_dialogEvents == null) return;
            _dialogEvents.Remove(dialogEvent);
        }
    }
}