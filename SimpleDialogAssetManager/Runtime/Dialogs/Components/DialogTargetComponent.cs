using System;
using DialogSystem.Runtime.Attributes;
using DialogSystem.Dialogs.Components.Managers;
using UnityEngine;

namespace DialogSystem.Dialogs.Components
{
    public class DialogTargetComponent : MonoBehaviour
    {
        public string TargetTag => _targetTag;
        [DialogTagSelector][SerializeField] protected string _targetTag = "NONE";
        [SerializeField] protected bool _useDefaultDialogManager = true;
        private void Awake() {
            if (_useDefaultDialogManager) {
                DialogManager.Instance.AddDialogTarget(this);
            }
        }
        protected virtual void OnAwake(){}
        public string GetTargetTag() => _targetTag;
    }
}