using System;
using System.Collections.Generic;

namespace Postive.CategorizedDB.Runtime.Categories.Interfaces
{
    public interface ICategoryElementFinder
    {
        public List<CategoryElement> Elements { get; }
#if UNITY_EDITOR
        public void CreateData(Type type,Category selectedCategory = null);
        public void RemoveData(CategoryElement data); 
#endif
    }
}