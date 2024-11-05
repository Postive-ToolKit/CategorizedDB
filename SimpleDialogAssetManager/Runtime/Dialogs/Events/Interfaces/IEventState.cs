namespace Postive.SimpleDialogAssetManager.Runtime.Dialogs.Events
{
    public interface IEventState {
        public bool UseSkip { get; }
        public bool IsEventFinished { get; }
        public string Content { get; }
    }
}