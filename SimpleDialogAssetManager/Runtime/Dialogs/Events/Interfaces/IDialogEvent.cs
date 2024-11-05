using DialogSystem.Dialogs.Components.Managers;

namespace Postive.SimpleDialogAssetManager.Runtime.Dialogs.Events
{
    public interface IDialogEvent : IEventState
    {
        public void Invoke(DialogManager manager);
    }
}