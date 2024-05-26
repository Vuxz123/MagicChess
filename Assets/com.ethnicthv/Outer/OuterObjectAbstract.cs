using System.Threading;
using UnityEngine;

namespace com.ethnicthv.Outer
{
    public abstract class OuterObjectAbstract: MonoBehaviour
    {
        private int _isDirty = 0;
        
        /// <summary>
        /// If you want to override this method, you must call base.Update() in the derived class. <br/>
        /// This a need, must be remembered. If you don't call this method, the object will not be cleaned. <br/>
        /// </summary>
        public void BaseUpdate()
        {
            if (!IsDirty()) return;
            Cleaning();
        }

        protected abstract void Cleaning();
        
        /// <summary>
        /// Mark the object as dirty. <br/>
        /// This method is thread safe.
        /// </summary>
        public void MarkDirty()
        {
            //prevent multiple threads from accessing the same variable
            Interlocked.Exchange(ref _isDirty, 1);
        }
        
        /// <summary>
        /// Check if the object is dirty. <br/>
        /// This action must be performed by the main thread only, if u use this method in another thread, pls make sure of what u gonna do. <br/>
        /// After calling this method, the object will be marked as clean. <br/>
        /// This method is thread safe. <br/>
        /// </summary>
        /// <returns>
        /// <list type="dotted">
        ///     <item> <description>true: the object is dirty</description> </item>
        ///     <item> <description>false: the object is clean</description> </item>
        /// </list>
        /// </returns>
        public bool IsDirty()
        {
            //prevent multiple threads from accessing the same variable
            return Interlocked.CompareExchange(ref _isDirty, 0, 1) == 1;
        }
    }
}