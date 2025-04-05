using System;
using Postive.CategorizedDB.Runtime.Categories.Interfaces;
using UnityEngine;

namespace Runtime.Attributes
{
    public abstract class CategoryElementSelectorAttribute : PropertyAttribute
    {
        public abstract ICategoryElementFinder ElementFinder { get; }
        public virtual Type ElementType => null;
    }
}