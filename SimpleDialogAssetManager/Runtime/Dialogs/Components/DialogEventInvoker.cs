using DialogSystem.Runtime.Attributes;
using DialogSystem.Dialogs.Components.Managers;
using UnityEngine;
using UnityEngine.Events;

namespace DialogSystem.Dialogs.Components
{
    public class DialogEventInvoker : DialogTargetComponent
    {
        [SerializeField] private UnityEvent<string> _onInvokeEvent;
        public void InvokeEvent(string eventName) {
            _onInvokeEvent?.Invoke(eventName);
        }

        
    }
}