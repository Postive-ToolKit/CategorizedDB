using DialogSystem.Dialogs.Components.Managers;
using UnityEngine;

namespace DialogSystem.Nodes.Lines
{
    public abstract class SingleChildNode : DialogBaseNode
    {
        public override bool UseAutoPlay => _useAutoPlay;
        public override bool IsNextExist => Child != null;
        public override bool IsAvailableToPlay => true;
        [SerializeField] private bool _useAutoPlay = false;
        [HideInInspector] public DialogBaseNode Child = null;
        public override DialogBaseNode GetNext() => Child;
    }
}