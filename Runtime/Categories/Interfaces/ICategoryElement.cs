using UnityEngine;

namespace Postive.CategorizedDB.Runtime.Categories.Interfaces
{
    public interface ICategoryElement
    {
        public string GUID { get; }
        public string Name { get; }
        public ICategoryPathFinder DB { get; }
        public CategoryScriptableObject Data { get; }

    }
}