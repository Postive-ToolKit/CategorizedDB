using System;
using System.Collections.Generic;
using DialogSystem.Dialogs.Components;
using DialogSystem.Runtime.Attributes;
using DialogSystem.Dialogs.Components.Managers;
using DialogSystem.Runtime.Structure.ScriptableObjects;
using DialogSystem.Runtime.Structure.ScriptableObjects.Components.Selections;
using UnityEngine;

namespace DialogSystem.Nodes.Branches
{
    public class DialogBranchNode : MultipleChildNode
    {
        public string SelectorTag => _selectorTag;
        public int SelectIndex {
            get {
                return _selectIndex;
            }
            set {
                _selectIndex = value;
            }
        }

        public override bool IsNextExist => Children.Count > 0;
        public override bool IsAvailableToPlay => IsNextExist && SelectIndex >= 0 && SelectIndex < Children.Count;
        public override int ChildCount => _selections.Count;
        public List<DialogContent> Selections => _selections;
        private int _selectIndex = -1;
        [DialogTagSelector]
        [SerializeField] private string _selectorTag = "Selections";
        [SerializeField] private List<DialogContent> _selections = new List<DialogContent>();
        public DialogBranchNode() {
            Type = DialogType.BRANCH;
        }
        public override DialogBaseNode GetNext()
        {
            #if UNITY_EDITOR
                Debug.Log("DialogBranchNode : GetNext / SelectIndex : " + SelectIndex);
            #endif
            if (SelectIndex < 0 || SelectIndex >= Children.Count) {
                return null;
            }
            return Children[SelectIndex];
        }
        public override void Play(DialogManager manager){
            var selections = Selections;
            if (selections.Count == 0) {
                Debug.LogWarning("Selections is empty");
                return;
            }
            var targetsWithTags = manager.Selectors.FindAll(t => t.TargetTag == SelectorTag);
            if (targetsWithTags.Count == 0) {
                Debug.LogWarning("Target with tag " + SelectorTag + " not found");
                return;
            }
            targetsWithTags[0].CreateSelections(selections, this, manager);
        }
        public override void ResetNode()
        {
            SelectIndex = -1;
        }
        protected override void CheckIntegrity()
        {
            if (Children.Count == 0) Debug.LogWarning("Selections is empty");
            #if UNITY_EDITOR
            if (_selections.Count != Children.Count) {
                Debug.LogWarning("Selection  is not equal");
                for (int i = _selections.Count; i < Children.Count; i++) {
                    _selections.Add(new DialogContent());
                }
            }
            #endif
        }
    }
}