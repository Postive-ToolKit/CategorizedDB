using DialogSystem.Dialogs.Components.Managers;
using UnityEngine;

namespace DialogSystem.Nodes
{
    public class DialogStartNode : DialogBaseNode
    {
        public override int ChildCount => 1;
        public override bool IsNextExist => Children[0] != null;
        public override bool IsAvailableToPlay => true;
        [SerializeField] private bool _useAutoPlay = true;
        public override DialogBaseNode GetNext() => Children[0];
        protected override void OnPlay(DialogManager manager)
        {
            #if UNITY_EDITOR
            Debug.Log("Play Plot.");
            #endif
        }
    }
}