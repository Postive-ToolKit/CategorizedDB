namespace Postive.CategorizedDB.Runtime.Categories.Interfaces
{
    public interface ICategoryElement
    {
        public ICategoryPathFinder DB { get; }
        public string ParentGUID { get; }
        public void SetParent(string parentGuid);
    }
}