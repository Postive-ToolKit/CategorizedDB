using System.Collections.Generic;

namespace Postive.CategorizedDB.Runtime.Categories.Interfaces
{
    public interface ICategoryPathFinder
    {
        public List<Category> Categories { get; }
        //if parentIndex is -1, then it is the root category
        public string GetPath(string parentGuid);
        public CategoryScriptableObject GetParentByGuid(string guid);
        public bool IsInSameBranch(string parentGuid, string childGuid);
    }
}