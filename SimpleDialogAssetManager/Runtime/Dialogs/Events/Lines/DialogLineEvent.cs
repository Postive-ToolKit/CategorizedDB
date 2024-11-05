using System.Collections.Generic;
using DialogSystem.Dialogs.Components.Managers;
using DialogSystem.Runtime.Structure.ScriptableObjects;
using UnityEngine;

namespace Postive.SimpleDialogAssetManager.Runtime.Dialogs.Events.Lines
{
    public class DialogLineEvent : DialogEvent
    {
        public override bool IsEventFinished {
            get {
                for (int i = 0; i < _dialogLineReceivers.Count; i++) {
                    if (!_dialogLineReceivers[i].IsLineFinished) {
                        return false;
                    }
                }
                return true;
            }
        }
        public override string Content => base.Content + "\n" + _dialogContent.Content;
        [SerializeField] private DialogContent _dialogContent;
        private List<IDialogLineReceiver> _dialogLineReceivers;
        public override void Invoke(DialogManager manager) {
            //get all line receivers
            var dialogHandlers = manager.DialogHandlers;
            _dialogLineReceivers = new List<IDialogLineReceiver>();
            for (int i = 0; i < dialogHandlers.Length; i++) {
                _dialogLineReceivers.AddRange(dialogHandlers[i].DialogTarget.GetComponents<IDialogLineReceiver>());
            }
            for(int i = 0; i < _dialogLineReceivers.Count; i++){
                if (_dialogLineReceivers[i] == null) continue;
                if (_dialogLineReceivers[i].DialogTargetTag.Equals(DialogTargetTag)) {
                    _dialogLineReceivers[i].Speak(_dialogContent);
                    Debug.Log("DialogLineEvent: " + _dialogLineReceivers[i].DialogTarget.name);
                }
                else {
                    _dialogLineReceivers[i].OnOtherSpeakerSpeak();
                }
            }
        }
    }
}