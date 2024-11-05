using System;
using DialogSystem.Dialogs.Components.Managers;
using DialogSystem.Runtime.Structure.ScriptableObjects;

namespace DialogSystem.Nodes.Branches
{
    public interface IDialogSelectionReceiver
    {
        public event Action<int> OnSelect; 
        public void CreateSelections(DialogContent[] selections, DialogManager manager);
    }
}