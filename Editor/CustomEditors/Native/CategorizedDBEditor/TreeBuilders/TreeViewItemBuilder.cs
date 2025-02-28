using System;
using System.Collections.Generic;
using Postive.CategorizedDB.Runtime.Categories;
using Postive.CategorizedDB.Runtime.Categories.Interfaces;
using UnityEngine.UIElements;

namespace Postive.CategorizedDB.Editor.CustomEditors.Native.CategorizedDBEditor.TreeBuilders
{
    public abstract class TreeViewItemBuilder
    {
        /// <summary>
        /// Get the expanded ids
        /// </summary>
        public Func<Dictionary<string, int>> GetExpandedIds;
        /// <summary>
        /// Build the tree items
        /// </summary>
        /// <param name="elements"> The elements to build the tree from</param>
        /// <returns> The tree items</returns>
        public abstract List<TreeViewItemData<ICategoryElement>> BuildTreeItems(List<CategoryScriptableObject> elements);
    }
}