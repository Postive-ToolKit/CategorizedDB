using System;
using System.Collections.Generic;
using Postive.CategorizedDB.Runtime.Categories;
using Postive.CategorizedDB.Runtime.Categories.Interfaces;
using UnityEngine.UIElements;

namespace Postive.CategorizedDB.Editor.CustomEditors.Native.CategorizedDBEditor.TreeBuilders
{
    public abstract class TreeViewItemBuilder
    {
        public Func<Dictionary<string, int>> GetExpandedIds;
        public abstract List<TreeViewItemData<ICategoryElement>> BuildTreeItems(List<CategoryScriptableObject> elements);
    }
}