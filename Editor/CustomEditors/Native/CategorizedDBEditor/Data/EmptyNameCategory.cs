using Postive.CategorizedDB.Runtime.Categories;
using Postive.CategorizedDB.Runtime.Categories.Interfaces;

namespace Postive.CategorizedDB.Editor.CustomEditors.Native.CategorizedDBEditor.Data
{
    /// <summary>
    /// A category element that has no data, used to group elements by class type
    /// Use when adding elements to the tree view
    /// </summary>
    public class EmptyNameCategory : ICategoryElement
    {
        /// <summary>
        /// Pass the name
        /// </summary>
        public string GUID => Name;
        /// <summary>
        /// Name of the category
        /// </summary>
        public string Name { get; private set; }
        public ICategoryPathFinder DB => null;
        /// <summary>
        /// Null data
        /// </summary>
        public CategoryScriptableObject Data => null;
        /// <summary>
        /// Create a new category with a name
        /// </summary>
        /// <param name="name"> Name of the category </param>
        public EmptyNameCategory(string name) {
            Name = name;
        }
    }
}