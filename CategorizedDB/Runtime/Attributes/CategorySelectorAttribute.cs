using Postive.CategorizedDB.Runtime.Categories.Interfaces;
using UnityEngine;

namespace Postive.CategorizedDB.Runtime.Attributes
{
    public class CategorySelectorAttribute : PropertyAttribute {
        public ICategoryPathFinder PathFinder { get; protected set; }
    }
}