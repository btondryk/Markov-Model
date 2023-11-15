using System.ComponentModel.Design.Serialization;

namespace TreeSymbolTable
{
    /// <summary>
    /// Create Symbol Table takes a key and a value
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class TreeSymbolTable<K, V> : IEnumerable<K> where K : IComparable<K>
    {
        /// <summary>
        /// Create a Node that also take key and value
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        class MyNode<K, V>
        {
            public MyNode<K, V> L;
            public MyNode<K, V> R;
            public K key;
            public V value;
            public int count;
            /// <summary>
            /// Constructor for node
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            public MyNode(K key, V value = default)
            {
                this.key = key;
                this.value = value;
                L = null;
                R = null;
                count = 1;
            }
        }

        private MyNode<K, V> root;
        /// <summary>
        /// Constructor for symbol tree
        /// </summary>
        public TreeSymbolTable()
        {
            root = null;
        }
        /// <summary>
        /// return count of binary tree
        /// </summary>
        public int Count
        {
            get
            {
                if (root == null)
                { 
                    return 0; 
                }
                else 
                {
                    return root.count; 
                }
            }
        }

        /// <summary>
        /// Adds a key and value in the correct position of the binary search tree
        /// </summary>
        /// <param name="key">adds key to the right or left of a certain node</param>
        /// <param name="value">value is just carried wherever key is</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Add(K key, V value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("The key caanot be null");
            }
            root = Add(root, key, value);
        }
        private MyNode<K,V> Add(MyNode<K, V> subroot, K key, V value)
        {
            if(subroot == null)
            {
                //new node goes here
                return new MyNode<K, V>(key, value);
            }
            if (key.CompareTo(subroot.key) == -1)
            {
                subroot.L = Add(subroot.L, key, value);
            }
            else if (key.CompareTo(subroot.key) == 1)
            { 
                subroot.R = Add(subroot.R, key, value);
            }
            else
            {
                throw new ArgumentException($"A node with the key {key} already exists in the symbol table.");
            }
            subroot.count++;
            return subroot;
        }
        /// <summary>
        /// created this method to more easily call the other methods, walks to the key entered
        /// </summary>
        /// <param name="key"></param>
        /// <param name="subroot"></param>
        /// <returns>subroot</returns>
        private MyNode<K, V> WalkToKey( MyNode<K, V> subroot, K key)
        {
            while (subroot != null)
            {
                if (key.CompareTo(subroot.key)==0)
                {
                    return subroot;
                }

                if (key.CompareTo(subroot.key) < 0)
                {
                    subroot = subroot.L; // Move to the left subtree
                }
                else
                {
                    subroot = subroot.R; // Move to the right subtree
                }
            }

            return null; // Key not found in the tree
            

        }
        /// <summary>
        /// Tells if a key is in the binary search tree or not
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains( K key) 
        {
            
            MyNode<K,V>subroot = WalkToKey(root, key);
            if(subroot!=null)
            {
                return true;
            }
            return false;
        }

        public K Min()
        {
            return Min(root);
        }
        private K Min(MyNode<K, V> subroot)
        {
            if (subroot == null)
            {
                throw new KeyNotFoundException("Empty tree there is no maximum key");
            }
            if (subroot.L == null)
            {
                subroot = subroot.L;
            }
            return subroot.key;
        }
        public K Max()
        {
            return Max(root);
        }
        private K Max(MyNode<K, V> subroot)
        {
            if (subroot == null)
            {
                throw new KeyNotFoundException("Empty tree there is no maximum key");
            }
           if(subroot.R == null)
           {
                return subroot.key;
           }
           return Max(subroot.R);
        }
        
        /// <summary>
        /// Returns the successor key. Seems to work, but I am not sure it covers all cases
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public K Successor(K key)
        {
            MyNode<K, V> successor = null;
            MyNode<K, V> subroot = root;
            if(subroot == null)
            {
                throw new KeyNotFoundException("The tree is empty");
            }

            // Search for the node with the given key
            while (subroot != null && subroot.key.CompareTo(key) != 0)
            {
                if (key.CompareTo(subroot.key) < 0)
                {
                    successor = subroot;
                    subroot = subroot.L; //move to the left subtree
                }
                else
                {
                    subroot = subroot.R; //move to the right subtree
                    
                }
            }

            //This is the easy code move one right and then move left till you can't
            if (subroot.R != null)
            {
                subroot = subroot.R;
                while (subroot.L != null)
                {
                    subroot = subroot.L;
                }
                return subroot.key;
            }
            if (successor != null)
            {
                return successor.key; // Return the key of the successor node
            }
            else
            {
                throw new InvalidOperationException("No successor exists"); // Throw an exception if no successor node is found
            }
        }



        /// <summary>
        /// gets the value of a key that is entered by the user
        /// </summary>
        /// <param name="key"></param>
        /// <returns>value of the key</returns>
        /// <exception cref="KeyNotFoundException">throws this exception if a key is not found</exception>
        public V this[K key]
        {
            get
            {
                MyNode<K, V> subroot = WalkToKey(root, key);
                if (subroot != null)
                {
                    return subroot.value;
                }
                else
                {
                    throw new KeyNotFoundException($"The key: {key} is not found");
                }
            }
            set
            {
                MyNode<K, V> subroot = WalkToKey(root, key);
                if (subroot != null)
                {
                    subroot.value = value; 
                }
                else
                {
                    throw new KeyNotFoundException($"The key: {key} is not found");
                }
            }
        }
        //public MyNode<K,V> Remove(K key)
        //{
        //    MyNode<K, V> subroot = Remove(key, subroot);
        //}
        //private MyNode<K,V> Remove
        private IEnumerable<K>GetEnumerator(MyNode<K, V> subroot)
        {
            if (subroot != null)
            {
                foreach (K key in GetEnumerator(subroot.L)) yield return key;
                yield return subroot.key;
                foreach (K key in GetEnumerator(subroot.R)) yield return key;
            }
        }
        public IEnumerator<K> GetEnumerator()
        {
            foreach (K key in GetEnumerator(root))
            {
                yield return key;
            }
               
        }
        System.Collections.IEnumerator 
            System.Collections.IEnumerable.GetEnumerator() 
        {
            return GetEnumerator();
        }
    }
}