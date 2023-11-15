namespace SymbolTables
{
    /// <summary>
    /// create a LinkedList class that is generic and enumerable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListSymbolTable<K, V> : IEnumerable<K> where K : IComparable<K>
    {
        /// <summary>
        /// Create a MyNode generic class within the LinkedList class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class MyNode<K, V>
        {
            public K key;
            public V value;
            public MyNode<K, V> next;
            public MyNode<K, V> prev;

            /// <summary>
            /// Node class contsructor
            /// </summary>
            /// <param name="value"></param>
            public MyNode(K key, V value = default)
            {
                this.key = key;
                this.value = value;
                next = null;
                prev = null;

            }
        }
        private MyNode<K, V> head;
        private MyNode<K, V> tail;
        private int count;
        /// <summary>
        /// LinkedList constructor
        /// </summary>
        public ListSymbolTable()
        {
            head = null;
            tail = null;
            count = 0;

        }
        /// <summary>
        /// ADD function:Adds an element to the symbol table
        /// </summary>
        /// <param name="key">the keys</param>
        /// <param name="value">the values</param>
        public void Add(K key, V value)
        {
            //FIXES: IF key exists throws a exception and make sure exceptions are proper

            if (count > 0)
            {
                MyNode<K, V> node = new MyNode<K, V>(key, value);

                
                tail.next = node;
                node.prev = tail;

                // Update the tail to be the new node
                tail = node;

                count++;
            }
            else
            {
                // The list is empty, create a new node and set both head and tail to it
                head = new MyNode<K, V>(key, value);
                tail = head;

                count++;
            }
        }

        /// <summary>
        /// walks to a particular node when the user passes in a key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private MyNode<K, V> WalkToNode(K key)
        {

            MyNode<K, V> curr = head;
            while (curr != null)
            {
                if (curr.key.Equals(key))
                {
                    return curr;
                }
                curr = curr.next;
            }
            return null;
        }


        /// <summary>
        /// gets the value to a key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue">default value of the values</param>
        /// <returns></returns>
        public V Get(K key, V defaultValue = default)
        {
            MyNode<K, V> curr = head;
            while (curr != null)
            {
                if (curr.key.CompareTo(key) == 0)
                {
                    return curr.value;
                }
                curr = curr.next;
            }
            return defaultValue;
        }
        /// <summary>
        /// also gets the value to the key - i may have messed up here
        /// The textbook did not actually explain what the methods do
        /// It just coded them, but from the code in the book I think its right
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">Found this exception handling in the textbook</exception>
        public V this[K key]
        {
            get
            {
                MyNode<K, V> curr = head;
                while (curr != null)
                {
                    if (curr.key.CompareTo(key) == 0)
                    {
                        return curr.value;
                    }
                    curr = curr.next;
                }

                throw new KeyNotFoundException($"key {key} is not found");
            }
            set
            {
                MyNode<K, V> curr = head;
                while (curr != null)
                {
                    if (curr.key.CompareTo(key) == 0)
                    {
                        curr.value = value;
                        return;
                    }
                    curr = curr.next;
                }
                throw new KeyNotFoundException($"Key {key} is not found.");
            }
        }

        /// <summary>
        /// tells if an item is in the list
        /// </summary>
        /// <param name="key"></param>
        /// <returns>returns true if there is a key, false if there is not the key</returns>

        public bool Contains(K key)
        {
            MyNode<K, V> curr = head;
            while (curr != null)
            {
                if (curr.key.CompareTo(key) == 0)
                {
                    return true;
                }
                curr = curr.next;
            }
            return false;
        }
        /// <summary>
        /// removes a key and corresponding value (hopefully)
        /// </summary>
        /// <param name="key"></param>
        /// <returns>true if key is removed, false if the key is not in the list (which maybe  it should 
        /// return true, but it was not removed. </returns>
        /// <exception cref="InvalidOperationException"></exception>
        public bool Remove(K key)
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("Symbol Table is already empty");
            }
            if (Count == 1)
            {
                head = null;
                tail = null;
            }
            if (Contains(key) == false)
            {
                return false;
            }
            MyNode<K, V> curr = head;
            while (curr != null)
            {

                if (curr.key.CompareTo(key) == 0)
                {
                    if (curr.prev == null)
                    {
                        head = head.next;
                        head.prev = null;
                    }
                    else if (curr.next == null)
                    {
                        tail = tail.prev;
                        tail.next = null;
                    }
                    else
                    {
                        curr.prev.next = curr.next;
                        curr.next.prev = curr.prev;
                    }

                }
                curr = curr.next;
            }
            count--;
            return true;
        }

        /// <summary>
        /// returns the current count
        /// </summary>
        public int Count
        { get { return count; } }

        /// <summary>
        /// OUR ENUMERATOR
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator
          System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        /// <summary>
        /// same as the previous docstring. These methods work together to enumerate the the table
        /// </summary>
        /// <returns></returns>
        public IEnumerator<K> GetEnumerator()
        {
            MyNode<K, V> curr = head;
            while (curr != null)
            {
                yield return curr.key;
                curr = curr.next;
            }

        }
    }
}
