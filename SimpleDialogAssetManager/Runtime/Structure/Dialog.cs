using System;
using System.Collections.Generic;
using DialogSystem.Runtime.Attributes;
using UnityEngine;

namespace DialogSystem.Runtime.Structure.ScriptableObjects
{
    [Serializable]
    public class Dialog
    {
        public List<DialogEvent> DialogEvents => _dialogEvents;
        [DialogTagSelector] public string SpeakerTag;
        [SerializeField] private List<DialogEvent> _dialogEvents;
        [SerializeField] private DialogContent _dialogContent;
        public DialogContent DialogContent => _dialogContent;

    }
}