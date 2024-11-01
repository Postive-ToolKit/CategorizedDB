namespace Postive.CategorizedDB.Runtime.Categories.Interfaces
{
    public interface ICategoryElement
    {
        public CategorisedDB DB { get; }
        public string ParentGUID { get; }
        public void SetParent(string parentGuid);
    }
}