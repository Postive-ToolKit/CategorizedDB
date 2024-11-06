using System;
using System.Collections.Generic;
using DialogSystem.Dialogs.Components;
using DialogSystem.Dialogs.Components.Managers;
using DialogSystem.Runtime.Attributes;
using DialogSystem.Runtime.Structure.ScriptableObjects;
using Postive.SimpleDialogAssetManager.Runtime.Dialogs.Events;
using UnityEngine;
namespace DialogSystem.Nodes
{
    public abstract class DialogBaseNode : ScriptableObject
    {
        public DialogType Type { get; protected set; } = DialogType.DIALOG;
        public string Guid => _guid;
        public Vector2 Position {
            get => _position;
            set {
                _position = value;
                //set dirty
                #if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
                #endif
            }
        }
        public abstract bool IsNextExist { get; }
        public abstract bool IsAvailableToPlay { get; }
        public virtual int ChildCount => 0;
        [HideInInspector][SerializeField] private string _guid = "";
        [HideInInspector][SerializeField] private Vector2 _position = new Vector2(0,0);
        [HideInInspector] public DialogBaseNode[] Children = Array.Empty<DialogBaseNode>();
        public virtual string Target => "";
        public virtual string Content => "";

        public DialogBaseNode() {
            if (string.IsNullOrEmpty(_guid)) {
                _guid = System.Guid.NewGuid().ToString();
            }
        }
        public abstract DialogBaseNode GetNext();
        public void Play(DialogManager manager) {
            Debug.Log("Play Dialog : " + name);
            OnPlay(manager);
        }
        protected abstract void OnPlay(DialogManager manager);
        public virtual void Reset() { }
        protected virtual void CheckIntegrity() { }
        #if UNITY_EDITOR
        public Action OnNodeChanged;
        #endif
        public virtual void OnValidate() {
            Array.Resize(ref Children, ChildCount);
            CheckIntegrity();
            #if UNITY_EDITOR
            OnNodeChanged?.Invoke();
            #endif
        }
    }
}