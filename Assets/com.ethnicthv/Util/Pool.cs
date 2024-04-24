using System;
using System.Collections.Generic;

namespace com.ethnicthv.Util
{
    /// <summary>
    /// Pool is a generic class that can be used to pool objects.
    /// </summary>
    /// <typeparam name="T">
    /// Type of object to pool.
    /// </typeparam>
    public class Pool<T>
    {
        private uint _capacity;
        private Queue<T> _pool = new Queue<T>();
        private readonly Func<T> _generator;

        /// <summary>
        /// Constructor for Pool.
        /// </summary>
        /// <param name="maxCapacity">
        /// Maximum number of objects to pool.
        /// <list type="bullet">
        ///     <item> 0: No limit. </item>
        ///     <item> n: Pool will only have n object. (n need to be bigger or equal to one) </item>
        /// </list>
        /// </param>
        /// <param name="generator">
        /// Function to generate object when pool is empty.
        /// </param>
        public Pool(uint maxCapacity, Func<T> generator)
        {
            _capacity = maxCapacity;
            _generator = generator;
        }
        
        /// <summary>
        /// Constructor for Pool with unlimited capacity.
        /// </summary>
        /// <param name="generator">
        /// Function to generate object when pool is empty.
        /// </param>
        public Pool(Func<T> generator) : this(0, generator)
        {
        }

        /// <summary>
        /// Take an object from the pool. If pool is empty, generate a new object.
        /// </summary>
        public T Take()
        {
            return _pool.Count > 0 ? _pool.Dequeue() : _generator();
        }
        
        /// <summary>
        /// Return an object to the pool.
        /// </summary>
        public void Return(T obj)
        {
            if (_capacity == 0 || _pool.Count < _capacity)
            {
                _pool.Enqueue(obj);
            }
        }
        
        /// <summary>
        /// Clear the pool.
        /// </summary>
        public void Clear()
        {
            _pool.Clear();
        }
        
        /// <summary>
        /// Number of object in the pool.
        /// </summary>
        public int Count()
        {
            return _pool.Count;
        }
    }
}