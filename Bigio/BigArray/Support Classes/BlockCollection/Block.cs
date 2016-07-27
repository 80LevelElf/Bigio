using System.Collections.Generic;

namespace Bigio.BigArray.Support_Classes.BlockCollection
{
    /// <summary>
    /// It is layer over <see cref="List{T}"/> to have simple way to add new functional and change internal
    /// structure of block if it need to be done.
    /// </summary>
    public class Block<T> : List<T>
    {
        /// <summary>
        /// Create new instance of Block with specified capacity.
        /// </summary>
        public Block(int capacity) : base(capacity)
        {
            
        }
    }
}