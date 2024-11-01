using Postive.CategorizedDB.Runtime.Categories.Interfaces;
using UnityEngine;

namespace Runtime.Attributes
{
    public class CategoryElementSelectorAttribute : PropertyAttribute
    {
        public ICategoryElementFinder ElementFinder { get; protected set; }
    }
}