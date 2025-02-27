using Postive.CategorizedDB.Runtime.Categories;
using Postive.CategorizedDB.Runtime.Categories.Interfaces;

namespace Postive.CategorizedDB.Editor.CustomEditors.Native.CategorizedDBEditor.TreeBuilders
{
    public class EmptyNameCategory : ICategoryElement
    {
        public string GUID => Name;
        public string Name { get; private set; }
        public ICategoryPathFinder DB => null;
        public CategoryScriptableObject Data => null;
        public EmptyNameCategory(string name) {
            Name = name;
        }
    }
}