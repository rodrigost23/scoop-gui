using System.Collections.Generic;

namespace scoop_gui
{
    public static class MyExtensions
    {
        /// <summary>
        /// Adds all items from an enumerable to this collection
        /// </summary>
        /// <param name="newItems">The enumerable from which to get the items to add</param>
        public static void AddAll<T>(this ICollection<T> collection, IEnumerable<T> newItems)
        {
            foreach (T item in newItems)
            {
                collection.Add(item);
            }
        }
    }
}
