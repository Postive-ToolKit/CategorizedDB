namespace Postive.SimpleDialogAssetManager.Runtime.Dialogs.Events
{
    public interface IDialogMessageReceiver
    {
        public bool WasMessageTaskEnded { get; }
        public void ReceiveMessage(string message);
    }
}