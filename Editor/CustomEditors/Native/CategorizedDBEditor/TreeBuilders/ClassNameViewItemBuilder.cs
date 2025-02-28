using System.Collections.Generic;
using Postive.CategorizedDB.Editor.CustomEditors.Native.CategorizedDBEditor.Data;
using Postive.CategorizedDB.Runtime.Categories;
using Postive.CategorizedDB.Runtime.Categories.Interfaces;
using UnityEngine.UIElements;

namespace Postive.CategorizedDB.Editor.CustomEditors.Native.CategorizedDBEditor.TreeBuilders
{
    /// <summary>
    /// The class name view item builder
    /// This class builds the tree view items based on the class name
    /// No category is created for this view
    /// The class name is the root category
    /// </summary>
    public class ClassNameViewItemBuilder : TreeViewItemBuilder
    {
        public override List<TreeViewItemData<ICategoryElement>> BuildTreeItems(List<CategoryScriptableObject> elements)
        {
            int currentId = 0;
            List<TreeViewItemData<ICategoryElement>> rootItems = new List<TreeViewItemData<ICategoryElement>>();
            Dictionary<string,List<CategoryScriptableObject>> 
                clsDict = new Dictionary<string,List<CategoryScriptableObject>>();
            
            for(int i = 0; i < elements.Count; i++) {
                if (elements[i] is Category) continue;
                string clsName = elements[i].GetType().Name;
                if (!clsDict.ContainsKey(clsName)) {
                    clsDict.Add(clsName, new List<CategoryScriptableObject>());
                }
                clsDict[clsName].Add(elements[i]);
            }
            foreach (var pair in clsDict) {
                if (pair.Value.Count == 0) continue;
                List<TreeViewItemData<ICategoryElement>> classItems = new List<TreeViewItemData<ICategoryElement>>();
                foreach (var data in pair.Value) {
                    int currentCreatedId = currentId++;
                    if (GetExpandedIds().ContainsKey(data.GUID))
                        GetExpandedIds()[data.GUID] = currentCreatedId;
                    classItems.Add(new TreeViewItemData<ICategoryElement>(currentCreatedId, data));
                }

                int currentClsId = currentId++;
                EmptyNameCategory classCategory = new EmptyNameCategory(pair.Key);
                if (GetExpandedIds().ContainsKey(classCategory.GUID))
                    GetExpandedIds()[classCategory.GUID] = currentClsId;
                rootItems.Add(new TreeViewItemData<ICategoryElement>(currentClsId, classCategory, classItems));
            }
            return rootItems;
        }
    }
}