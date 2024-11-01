namespace Postive.CategorizedDB.Runtime.Categories.Interfaces
{
    public interface ICategory : ICategoryPath
    {
        public string GUID { get; }
        public string Name { get; }
    }
}