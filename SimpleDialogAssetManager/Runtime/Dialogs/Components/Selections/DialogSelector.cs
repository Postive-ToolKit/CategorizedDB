using System;
using System.Collections.Generic;
using DialogSystem.Dialogs.Components;
using DialogSystem.Dialogs.Components.Managers;
using DialogSystem.Nodes.Branches;
using UnityEngine;

namespace DialogSystem.Runtime.Structure.ScriptableObjects.Components.Selections
{
    public class DialogSelector : DialogTargetComponent,IDialogSelectionReceiver
    {
        public event Action<int> OnSelect; 
        [SerializeField] private GameObject _selectionPrefab = null;
        [SerializeField] private List<DialogSelection> _selectionComponents = new List<DialogSelection>();
        private DialogManager _currentTargetManager = null;
        public void CreateSelections(DialogContent[] selections, DialogManager manager)
        {
            _currentTargetManager = manager;
            int count = 0;
            for (count = 0; count  < selections.Length && count < _selectionComponents.Count; count++) {
                _selectionComponents[count].Init(count,selections[count]);
                _selectionComponents[count].OnSelectionSelected += Select;
            }
            for (; count < selections.Length; count++) {
                var selection = Instantiate(_selectionPrefab, transform).GetComponent<DialogSelection>();
                selection.Init(count,selections[count]);
                selection.OnSelectionSelected += Select;
                _selectionComponents.Add(selection);
            }
            for (; count < _selectionComponents.Count; count++) {
                _selectionComponents[count].Hide();
            }
        }
        public void HideSelections()
        {
            foreach (var dialogSelection in _selectionComponents) {
                dialogSelection.Hide();
            }
        }
        public void Select(int index)
        {
            OnSelect?.Invoke(index);
            HideSelections();
            _currentTargetManager?.Play();
        }
    }
}