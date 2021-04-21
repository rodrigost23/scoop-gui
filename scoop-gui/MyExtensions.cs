using System.Collections.Generic;

namespace ScoopGui
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

        /// <summary>
        /// Prepends all items from an enumerable to this collection
        /// </summary>
        /// <param name="newItems">The enumerable from which to get the items to add</param>
        public static void InsertAll<T>(this IList<T> list, int index, IEnumerable<T> newItems)
        {
            int i = index;
            foreach (T item in newItems)
            {
                list.Insert(i++, item);
            }
        }

        /// <summary>
        /// Removes all items in an enumerable from this collection
        /// </summary>
        /// <param name="items">The enumerable from which to get the items to remove</param>
        public static void RemoveAll<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                _ = collection.Remove(item);
            }
        }
    }
}
