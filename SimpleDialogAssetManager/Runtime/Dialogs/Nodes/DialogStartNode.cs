using System.Collections.Generic;
using DialogSystem.Dialogs.Components;
using DialogSystem.Dialogs.Components.Managers;
using UnityEngine;

namespace DialogSystem.Nodes
{
    public class DialogStartNode : DialogBaseNode
    {
        public override bool IsNextExist => Child != null;
        public override bool IsAvailableToPlay => true;
        public override bool UseAutoPlay => _useAutoPlay;
        [SerializeField] private bool _useAutoPlay = true;
        [HideInInspector] public DialogBaseNode Child = null;
        public override DialogBaseNode GetNext() => Child;

        public override void Play(DialogManager manager)
        {
            #if UNITY_EDITOR
            Debug.Log("Play Plot.");
            #endif
        }
    }
}