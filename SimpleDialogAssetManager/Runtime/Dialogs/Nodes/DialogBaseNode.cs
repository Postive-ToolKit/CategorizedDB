using System.Collections.Generic;
using DialogSystem.Dialogs.Components;
using DialogSystem.Dialogs.Components.Managers;
using DialogSystem.Runtime.Attributes;
using DialogSystem.Runtime.Structure.ScriptableObjects;
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
        public abstract bool UseAutoPlay { get; }
        [HideInInspector][SerializeField] private string _guid = "";
        [HideInInspector][SerializeField] private Vector2 _position = new Vector2(0,0);

        public bool IsPlayed {
            get => _isPlayed;
            set => _isPlayed = value;
        }
        private bool _isPlayed = false;

        public DialogBaseNode() {
            if (string.IsNullOrEmpty(_guid)) {
                _guid = System.Guid.NewGuid().ToString();
            }
        }
        public abstract DialogBaseNode GetNext();
        public abstract void Play(DialogManager manager);
        public virtual void ResetNode() { }
        protected virtual void CheckIntegrity() { }

        public virtual void OnValidate() {
            CheckIntegrity();
        }
    }
}