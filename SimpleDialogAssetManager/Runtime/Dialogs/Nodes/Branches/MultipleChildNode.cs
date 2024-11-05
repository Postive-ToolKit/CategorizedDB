using System.Collections.Generic;
using DialogSystem.Runtime.Attributes;
using UnityEngine;

namespace DialogSystem.Nodes.Branches
{
    public abstract class MultipleChildNode : DialogBaseNode
    {
        public override int ChildCount => _childCount;
        [SerializeField] private int _childCount = 0;
    }
}