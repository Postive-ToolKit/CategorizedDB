using System;
using DialogSystem.Runtime.Attributes;
using DialogSystem.Dialogs.Components.Managers;
using Postive.SimpleDialogAssetManager.Runtime.Interfaces;
using UnityEngine;

namespace DialogSystem.Dialogs.Components
{
    public class DialogTargetComponent : MonoBehaviour, IDialogHandler
    {
        public GameObject DialogTarget => gameObject;
        public string DialogTargetTag => _targetTag;
        [DialogTagSelector][SerializeField] protected string _targetTag = "NONE";
        [SerializeField] protected bool _useDefaultDialogManager = true;
        private void Awake() {
            if (_useDefaultDialogManager) {
                DialogManager.Instance.AddDialogTarget(this);
            }
        }
        protected virtual void OnAwake(){}
        public string GetTargetTag() => _targetTag;
        public virtual void OnStartPlot() {}
        public virtual void OnEndPlot() { }
    }
}