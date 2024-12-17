namespace Postive.CategorizedDB.Runtime.Categories.Interfaces
{
    public interface ICategoryObject
    {
        public string GUID { get; }
        public string Name { get; }
        public string GetPath();
    }
}