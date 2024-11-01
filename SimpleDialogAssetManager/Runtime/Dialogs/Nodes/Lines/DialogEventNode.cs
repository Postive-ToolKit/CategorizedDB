using System.Collections.Generic;
using DialogSystem.Dialogs.Components.Managers;
using DialogSystem.Runtime.Structure.ScriptableObjects;
using UnityEngine;

namespace DialogSystem.Nodes.Lines
{
    public class DialogEventNode : SingleChildNode
    {
        [SerializeField] private List<DialogEvent> _dialogEvents;
        public override void Play(DialogManager manager)
        {
            foreach (var eventData in _dialogEvents) {
                var eventTargets = manager.EventInvokers.FindAll(ei => eventData.EventTargets.Contains(ei.TargetTag));
                foreach (var target in eventTargets) {
                    target.InvokeEvent(eventData.EventMessage);
                }
            }
        }
    }
}