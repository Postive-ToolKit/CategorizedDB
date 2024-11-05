using DialogSystem.Dialogs.Components.Managers;
using UnityEngine;

namespace DialogSystem.Nodes.Lines
{
    public abstract class SingleChildNode : DialogBaseNode
    {
        public override int ChildCount => 1;
        public override bool IsNextExist => Children[0] != null;
        [SerializeField] private bool _useAutoPlay = false;
        public override DialogBaseNode GetNext() => Children[0];
    }
}