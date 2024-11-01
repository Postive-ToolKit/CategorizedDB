using DialogSystem.Runtime.Structure.ScriptableObjects;
using Runtime.Attributes;
using UnityEngine;

namespace DialogSystem.Runtime.Attributes
{
    public class DialogSelectorAttribute : CategoryElementSelectorAttribute
    {
        public DialogSelectorAttribute() : base() {
            ElementFinder = DialogDB.Instance;
        }
    }
}