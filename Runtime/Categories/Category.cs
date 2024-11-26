using System;

namespace Postive.CategorizedDB.Runtime.Categories
{
    [Serializable]
    public class Category : CategoryScriptableObject {
        public override string GetPath() {
            var path = base.GetPath();
            string categoryName = Name;
            if (string.IsNullOrEmpty(categoryName)) categoryName = "Empty - " + GUID.Substring(0, 5);
            return string.IsNullOrEmpty(path) ? categoryName: $"{path}/{categoryName}";
        }
    }
}