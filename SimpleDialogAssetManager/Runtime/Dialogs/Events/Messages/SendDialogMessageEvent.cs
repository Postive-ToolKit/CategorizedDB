using System;
using System.Collections.Generic;
using DialogSystem.Dialogs.Components.Managers;
using DialogSystem.Runtime.Attributes;
using UnityEngine;

namespace Postive.SimpleDialogAssetManager.Runtime.Dialogs.Events.Messages
{
    [Serializable]
    public class SendDialogMessageEvent : DialogEvent
    {
        public override bool IsEventFinished {
            get {
                if(_receivers == null) return true;
                if(_receivers.Count == 0) return true;
                for(int i = 0; i < _receivers.Count; i++){
                    if(!_receivers[i].WasMessageTaskEnded) return false;
                }
                return true;
            }
        }
        public override string Content => base.Content + EventMessage;
        [DialogTagSelector] public List<string> EventTargets;
        public string EventMessage;
        private List<IDialogMessageReceiver> _receivers;
        public override void Invoke(DialogManager manager){
            _receivers = new List<IDialogMessageReceiver>();
            var dialogHandlers = manager.DialogHandlers;
            for(int i = 0; i < EventTargets.Count; i++){
                if (!EventTargets.Contains(dialogHandlers[i].DialogTargetTag)) continue;
                var dialogTarget = dialogHandlers[i].DialogTarget;
                if (dialogTarget == null) continue;
                IDialogMessageReceiver[] receivers = dialogTarget.GetComponents<IDialogMessageReceiver>();
                for(int j = 0; j < receivers.Length; j++){
                    receivers[j].ReceiveMessage(EventMessage);
                    _receivers.Add(receivers[j]);
                }
            }
        }
    }
}