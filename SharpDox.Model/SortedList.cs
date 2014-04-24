using System;
using System.Collections;
using System.Collections.Generic;

namespace SharpDox.Model
{
    public class SortedList<T> : IList<T>
    {
        private List<T> list = new List<T>();

        public int IndexOf(T item)
        {
            var index = list.BinarySearch(item);
            return index < 0 ? -1 : index;
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException("Cannot insert at index; must preserve order.");
        }

        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
        }

        public T this[int index]
        {
            get
            {
                return list[index];
            }
            set
            {
                list.RemoveAt(index);
                list.Add(value);
            }
        }

        public void Add(T item)
        {
            list.Insert(~list.BinarySearch(item), item);
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(T item)
        {
            return list.BinarySearch(item) >= 0;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return list.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            var index = list.BinarySearch(item);
            if (index < 0) return false;
            list.RemoveAt(index);
            return true;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}
