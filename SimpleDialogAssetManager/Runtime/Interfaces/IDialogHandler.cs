using UnityEngine;

namespace Postive.SimpleDialogAssetManager.Runtime.Interfaces
{
    /// <summary>
    /// Interface for handling dialog plot
    /// Include dialog plot, dialog node, dialog line, dialog event and etc.
    /// </summary>
    public interface IDialogHandler
    {
        public string DialogTargetTag { get; }
        public GameObject DialogTarget { get; }
        public void OnStartPlot();
        public void OnEndPlot();
    }
}