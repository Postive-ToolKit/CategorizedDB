using System;
using DialogSystem.Dialogs.Components.Managers;
using DialogSystem.Runtime.Attributes;
using UnityEngine;

namespace Postive.SimpleDialogAssetManager.Runtime.Dialogs.Events
{
    [Serializable]
    public abstract class DialogEvent : IDialogEvent {
        public bool UseSkip => _useSkip;
        public abstract bool IsEventFinished { get; }
        public virtual string Content => $"<size=16><b>{DialogTargetTag}</size></b>\n";
        public string DialogTargetTag => _dialogTargetTag;
        public abstract void Invoke(DialogManager manager);
        [SerializeField] private bool _useSkip = true;
        [DialogTagSelector][SerializeField] private string _dialogTargetTag;
    }
}