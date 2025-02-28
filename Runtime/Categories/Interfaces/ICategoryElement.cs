using UnityEngine;

namespace Postive.CategorizedDB.Runtime.Categories.Interfaces
{
    public interface ICategoryElement
    {
        /// <summary>
        /// GUID of the category
        /// </summary>
        public string GUID { get; }
        /// <summary>
        /// Name of the category
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// The database that the category belongs to
        /// </summary>
        public ICategoryPathFinder DB { get; }
        /// <summary>
        /// The data of the category
        /// </summary>
        public CategoryScriptableObject Data { get; }

    }
}