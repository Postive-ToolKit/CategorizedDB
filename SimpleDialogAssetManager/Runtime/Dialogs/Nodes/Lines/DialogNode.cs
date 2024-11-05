using System.Collections.Generic;
using DialogSystem.Dialogs.Components.Managers;
using Postive.SimpleDialogAssetManager.Runtime.Dialogs.Events;
using Postive.SimpleDialogAssetManager.Runtime.Dialogs.Events.Lines;
using UnityEngine;

namespace DialogSystem.Nodes.Lines
{
    public class DialogNode : SingleChildNode
    {
        public override bool IsAvailableToPlay => _dialogEvents.UseSkip || _dialogEvents.IsEventFinished;

        public override string Content {
            get => _dialogEvents.Content;
        }
        [SerializeField] private DialogEventHolder _dialogEvents = new(new List<DialogEvent>{new DialogLineEvent()});
        protected override void OnPlay(DialogManager manager) {
            _dialogEvents.Invoke(manager);
        }
    }
}
