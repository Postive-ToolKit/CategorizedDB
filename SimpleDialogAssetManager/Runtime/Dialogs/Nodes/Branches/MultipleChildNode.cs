using System.Collections.Generic;
using DialogSystem.Runtime.Attributes;
using UnityEngine;

namespace DialogSystem.Nodes.Branches
{
    public abstract class MultipleChildNode : DialogBaseNode
    {
        public override bool UseAutoPlay => _useAutoPlay;
        public abstract int ChildCount { get; }
        [SDAMReadOnly][SerializeField] private bool _useAutoPlay = false;
        [HideInInspector] public List<DialogBaseNode> Children = new List<DialogBaseNode>();
    }
}