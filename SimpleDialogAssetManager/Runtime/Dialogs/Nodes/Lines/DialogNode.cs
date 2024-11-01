using System.Collections.Generic;
using DialogSystem.Dialogs.Components;
using DialogSystem.Dialogs.Components.Managers;
using DialogSystem.Runtime.Attributes;
using DialogSystem.Runtime.Structure.ScriptableObjects;
using UnityEngine;

namespace DialogSystem.Nodes.Lines
{
    public class DialogNode : SingleChildNode {
        [DialogTagSelector] [SerializeField] private string _speakerTag = "NONE";
        [SerializeField] private DialogContent _dialogContent = new DialogContent();
        public override void Play(DialogManager manager)
        {
            if (_speakerTag.Equals("NONE")) {
                return;
            }

            List<DialogSpeaker> dialogTargets = new List<DialogSpeaker>();
            List<DialogSpeaker> otherSpeakers = new List<DialogSpeaker>();
            for (int i = 0; i < manager.Speakers.Count; i++) {
                if (manager.Speakers[i].TargetTag.Equals(_speakerTag)) {
                    dialogTargets.Add(manager.Speakers[i]);
                } else {
                    otherSpeakers.Add(manager.Speakers[i]);
                }
            }
            dialogTargets.ForEach(target => target.Speak(_dialogContent));
            otherSpeakers.ForEach(target => target.OnOtherSpeakerSpeak());
        }
    }
}
