using DialogSystem.Runtime.Structure.ScriptableObjects;
using Postive.SimpleDialogAssetManager.Runtime.Interfaces;

namespace Postive.SimpleDialogAssetManager.Runtime.Dialogs.Events.Lines
{
    public interface IDialogLineReceiver : IDialogHandler
    {
        public bool IsLineFinished { get; }
        public void Speak(DialogContent dialogContent);
        public void OnOtherSpeakerSpeak();
    }
}