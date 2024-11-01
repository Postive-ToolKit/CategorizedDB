using System.Collections.Generic;
using DialogSystem.Dialogs.Components.Managers;
using DialogSystem.Runtime.Attributes;
using UnityEngine;

namespace DialogSystem.Dialogs.Components
{
    public class DialogPlotRequester : MonoBehaviour
    {
        [DialogSelector][SerializeField] private List<string> _plotIds = new List<string>();
        public void RequestDialogByIndex(int index)
        {
            if (index < 0 || index >= _plotIds.Count) {
                Debug.LogError("Index out of range");
                return;
            }
            DialogManager.Instance.SelectDialogPlot(_plotIds[index]);
        }
    }
}