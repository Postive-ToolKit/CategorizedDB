using System.Collections.Generic;
using Postive.CategorizedDB.Runtime.Categories;
using Postive.CategorizedDB.Runtime.Categories.Interfaces;
using UnityEngine.UIElements;

namespace Postive.CategorizedDB.Editor.CustomEditors.Native.CategorizedDBEditor.TreeBuilders
{
    /// <summary>
    /// The default tree view item builder
    /// Build tree based on the parent guid(Category)
    /// The root category has no parent guid
    /// </summary>
    public class DefaultTreeViewItemBuilder : TreeViewItemBuilder
    {
        private int _currentId = 0;
        public override List<TreeViewItemData<ICategoryElement>> BuildTreeItems(List<CategoryScriptableObject> elements)
        {
            _currentId = 0;
            List<TreeViewItemData<ICategoryElement>> rootItems = new List<TreeViewItemData<ICategoryElement>>();
            foreach (var data in elements.FindAll(x => string.IsNullOrEmpty(x.ParentGUID))) {
                rootItems.Add(BuildTreeItem(data, elements));
            }
            return rootItems;
        }
        private TreeViewItemData<ICategoryElement> BuildTreeItem(CategoryScriptableObject data, List<CategoryScriptableObject> elements) {
            var children = elements.FindAll(x => x.ParentGUID.Equals(data.GUID));
            int currentCreatedId = _currentId++;

            if (GetExpandedIds().ContainsKey(data.GUID))
                GetExpandedIds()[data.GUID] = currentCreatedId;

            if (children.Count > 0) {
                var childItems = new List<TreeViewItemData<ICategoryElement>>();
                foreach (var child in children) {
                    childItems.Add(BuildTreeItem(child, elements));
                }
                return new TreeViewItemData<ICategoryElement>(currentCreatedId, data, childItems);
            }

            return new TreeViewItemData<ICategoryElement>(currentCreatedId, data);
        }


    }
}