﻿using System;
using DialogSystem.Runtime.Attributes;
using DialogSystem.Dialogs.Components.Managers;
using DialogSystem.Runtime.Structure.ScriptableObjects;
using Postive.SimpleDialogAssetManager.Runtime.Interfaces;
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

        public override bool IsNextExist => Children.Length > 0;
        public override bool IsAvailableToPlay => IsNextExist;// && SelectIndex >= 0 && SelectIndex < Children.Length;
        public DialogContent[] Selections => _selections;
        private int _selectIndex = -1;
        [DialogTagSelector]
        [SerializeField] private string _selectorTag = "Selections";
        [SerializeField] private DialogContent[] _selections = Array.Empty<DialogContent>();
        private bool _isSelectionCreated = false;
        public DialogBranchNode() {
            Type = DialogType.BRANCH;
        }
        public override DialogBaseNode GetNext()
        {
            #if UNITY_EDITOR
                Debug.Log("DialogBranchNode : GetNext / SelectIndex : " + SelectIndex);
            #endif
            if (SelectIndex < 0 || SelectIndex >= Children.Length) {
                return this;
            }
            return Children[SelectIndex];
        }
        protected override void OnPlay(DialogManager manager){
            if (_isSelectionCreated) return;
            var selections = Selections;
            if (selections.Length == 0) {
                Debug.LogWarning("Selections is empty");
                return;
            }
            for (int i = 0; i < manager.DialogHandlers.Length; i++) {
                IDialogHandler handler = manager.DialogHandlers[i];
                if(!handler.DialogTargetTag.Equals(SelectorTag)) continue;
                if(!handler.DialogTarget.TryGetComponent(out IDialogSelectionReceiver receiver)) continue;
                receiver.CreateSelections(selections, manager);
                receiver.OnSelect += OnSelect;
                _isSelectionCreated = true;
                break;
            }
        }
        private void OnSelect(int index) {
            SelectIndex = index;
        }
        public override void Reset() {
            SelectIndex = -1;
            _isSelectionCreated = false;
        }
        protected override void CheckIntegrity()
        {
            if (Children.Length == 0) Debug.LogWarning("Selections is empty");
            #if UNITY_EDITOR
            if (Selections.Length == Children.Length) return; 
            Array.Resize(ref _selections, Children.Length);
            for (int i = 0; i < Children.Length; i++) {
                if (_selections[i] == null) {
                    _selections[i] = new DialogContent();
                }
            }
            #endif
        }
    }
}