using System.Collections.Generic;

namespace Postive.CategorizedDB.Runtime.Categories.Interfaces
{
    public interface ICategoryElementFinder
    {
        public List<CategoryElement> Elements { get; }
    }
}