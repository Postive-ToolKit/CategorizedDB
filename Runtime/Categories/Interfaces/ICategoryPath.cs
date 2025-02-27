namespace Postive.CategorizedDB.Runtime.Categories.Interfaces
{
    public interface ICategoryPath
    {
        public string ParentGUID { get; }
        public void SetParent(string parentGuid);
        public string GetPath();
    }
}