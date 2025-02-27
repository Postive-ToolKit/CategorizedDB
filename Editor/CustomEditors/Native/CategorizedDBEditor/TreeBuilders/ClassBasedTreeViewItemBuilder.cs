using System.Collections.Generic;
using Postive.CategorizedDB.Runtime.Categories;
using Postive.CategorizedDB.Runtime.Categories.Interfaces;
using UnityEngine.UIElements;

namespace Postive.CategorizedDB.Editor.CustomEditors.Native.CategorizedDBEditor.TreeBuilders
{
    public class ClassBasedTreeViewItemBuilder : TreeViewItemBuilder
    {
        private int _currentId = 0;
        public override List<TreeViewItemData<ICategoryElement>> BuildTreeItems(List<CategoryScriptableObject> elements)
        {
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
                    int currentCreatedId = _currentId++;
                    if (GetExpandedIds().ContainsKey(data.GUID))
                        GetExpandedIds()[data.GUID] = currentCreatedId;
                    classItems.Add(new TreeViewItemData<ICategoryElement>(currentCreatedId, data));
                }

                int currentClsId = _currentId++;
                EmptyNameCategory classCategory = new EmptyNameCategory(pair.Key);
                if (GetExpandedIds().ContainsKey(pair.Key))
                    GetExpandedIds()[pair.Key] = currentClsId;
                rootItems.Add(new TreeViewItemData<ICategoryElement>(currentClsId, classCategory, classItems));
            }
            return rootItems;
        }
    }
}