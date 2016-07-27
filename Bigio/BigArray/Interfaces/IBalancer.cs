using Bigio.BigArray.Support_Classes.BlockCollection;

namespace Bigio.BigArray.Interfaces
{
    /// <summary>
    /// Object to manage block size logic
    /// </summary>
    public interface IBalancer
    {
        /// <summary>
        /// Return size of block we want to add in specified place
        /// </summary>
        /// <param name="indexOfBlock">Index of new block in <see cref="BlockCollection{T}"/></param>
        /// <returns>Size of new block</returns>
        int GetNewBlockSize(int indexOfBlock);

        /// <summary>
        /// Return default size of block without reference to the situation(adding, deletion and so on)
        /// </summary>
        /// <param name="indexOfBlock">Index of block in <see cref="BlockCollection{T}"/></param>
        /// <returns>Default size of specified block</returns>
        int GetDefaultBlockSize(int indexOfBlock);

        /// <summary>
        /// Return max size of block with specified <see cref="indexOfBlock"/>
        /// </summary>
        /// <param name="indexOfBlock">Index of block in <see cref="BlockCollection{T}"/></param>
        /// <returns>Max size of specified block</returns>
        int GetMaxBlockSize(int indexOfBlock);

        /// <summary>
        /// Returns true if block in <see cref="BlockCollection{T}"/> is approximately equal, otherwise return false
        /// </summary>
        /// <returns>Returns true if block in <see cref="BlockCollection{T}"/> is approximately equal, otherwise return false</returns>
        bool IsBlockSizesEqual();
    }
}
