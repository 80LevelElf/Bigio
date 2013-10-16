using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BigDataCollections.DistributedArray.Managers;

namespace BigDataCollections.DistributedArray.SupportClasses
{
    class Blocks<T> : IEnumerable<List<T>>
    {
        //API
        /// <summary>
        /// Create a new instance of Blocks(T) class.
        /// Blocks(T) is a shell of blocks collection, which use for more
        /// simple usage of it.
        /// </summary>
        public Blocks():this(new Collection<T>())
        {
        }
        /// <summary>
        /// Create a new instance of Blocks(T) class.
        /// Blocks(T) is a shell of blocks collection, which use for more
        /// simple usage of it. 
        /// </summary>
        /// <param name="collection">Collection whitch use as base for new DistributedArray(T).
        /// The collection it self cannot be null, but it can contain elements that are null, if type T is a reference type. </param>
        public Blocks(ICollection<T> collection)
        {
            MaxBlockSize = DefaultValuesManager.MaxBlockSize;
            DefaultBlockSize = DefaultValuesManager.DefaultBlockSize;
            _blocks = new List<List<T>>(DivideIntoBlocks(collection));
        }
        /// <summary>
        /// Add new block to the end of collection of blocks of the DistributedArray(T).
        /// </summary>
        /// <param name="block">Block to add. Block cant be null.</param>
        public void Add(List<T> block)
        {
            if (block == null)
            {
                throw new ArgumentNullException("block");
            }
            _blocks.Add(block);
        }
        /// <summary>
        /// Add range of blocks to the end of the collection of blocks.
        /// </summary>
        /// <param name="range">Collection of blocks to add. It cant be null.</param>
        public void AddRange(IEnumerable<List<T>> range)
        {
            if (range == null)
            {
                throw new ArgumentNullException("range");
            }
            _blocks.AddRange(range);
        }
        /// <summary>
        /// Divide specified collection into blocks with DefaultBlockSize size.
        /// </summary>
        /// <param name="collection">Collection, which must be divided.</param>
        /// <returns>Blocks constructed on the basis of the collection with DefaultBlockSize size.</returns>
        public ICollection<List<T>> DivideIntoBlocks(ICollection<T> collection)
        {
            return DivideIntoBlocks(collection, 0, collection.Count);
        }
        /// <summary>
        /// Divide specified collection into blocks with DefaultBlockSize size that starts at the specified index.
        /// </summary>
        /// <param name="collection">Collection, which must be divided.</param>
        /// <param name="index">The zero-based starting index of the collection of elements to divide.</param>
        /// <returns>Blocks constructed on the basis of the collection with DefaultBlockSize size.</returns>
        public ICollection<List<T>> DivideIntoBlocks(ICollection<T> collection, int index)
        {
            return DivideIntoBlocks(collection, index, collection.Count - index);
        }
        /// <summary>
        /// Divide specified collection into blocks with DefaultBlockSize size that starts at the specified index.
        /// </summary>
        /// <param name="collection">Collection, which must be divided.</param>
        /// <param name="index">The zero-based starting index of the collection of elements to divide.</param>
        /// <param name="count">The number of elements of the collection to divide.</param>
        /// <returns>Blocks constructed on the basis of the collection with DefaultBlockSize size.</returns>
        public ICollection<List<T>> DivideIntoBlocks(ICollection<T> collection, int index, int count)
        {
            if (index + count > collection.Count)
            {
                throw new IndexOutOfRangeException();
            }

            //Calculate blocks count
            int countOfBlocks = count / DefaultBlockSize;
            if (count % DefaultBlockSize != 0)
            {
                countOfBlocks++;
            }

            var blocks = new List<T>[countOfBlocks];
            //Transfer data from list to new blocks
            var item = collection.GetEnumerator();
            for (int i = 0; i < index; i++) //Move item to the index position
            {
                item.MoveNext();
            }

            for (int i = 0; i < countOfBlocks; i++)
            {
                //Calculate curent block size
                int currentBlockSize;
                if (i != countOfBlocks - 1)
                {
                    currentBlockSize = DefaultBlockSize;
                }
                else
                {
                    currentBlockSize = count - (i * DefaultBlockSize);
                }
                //Declare new block
                blocks[i] = new List<T>(currentBlockSize);
                //Transfer data
                for (int j = 0; j < currentBlockSize; j++)
                {
                    item.MoveNext();
                    //Insert data
                    T insertion = item.Current;
                    blocks[i].Add(insertion);
                }
            }
            //Return result
            return blocks;
        }
        public void Clear()
        {
            _blocks.Clear();
            _blocks.Add(new List<T>(DefaultBlockSize));
        }
        /// <summary>
        /// Removes the block at the specified index of the _blocks.
        /// </summary>
        /// <param name="index">The zero-based index of the block to remove.</param>
        public void RemoveAt(int index)
        {
            _blocks.RemoveAt(index);
            //If there is no any blocks - add empty block
            if (_blocks.Count == 0)
            {
                _blocks.Add(new List<T>(DefaultBlockSize));
            }
        }
        /// <summary>
        /// Returns an enumerator that iterates through the collection of blocks.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<List<T>> GetEnumerator()
        {
            return _blocks.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        /// <summary>
        /// Inserts an element into the DistributedArray(T) at the specified index.
        /// </summary>
        /// <param name="index">Index of collection of block where the new block will be.</param>
        /// <param name="block">Block to insert.</param>
        public void Insert(int index, List<T> block)
        {
            _blocks.Insert(index, block);
        }
        /// <summary>
        /// Inserts the elements of a block range into the block collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the new elements should be inserted.</param>
        /// <param name="range">The range whose elements should be inserted into the block collection.
        ///  The range it self cannot be null, and cant contain null blocks.</param>
        public void InsertRange(int index, IEnumerable<List<T>> range)
        {
            //We need to get enumerable because of multiple enumaration.
            var enumerable = range as List<T>[] ?? range.ToArray();
            if (enumerable.Any(block => block == null))
            {
                throw new ArgumentNullException();
            }

            _blocks.InsertRange(index, enumerable);
        }
        /// <summary>
        ///  Reverses the order of the blocks in the entire block collection.
        /// </summary>
        public void Reverse()
        {
            _blocks.Reverse();
        }
        /// <summary>
        /// Gets or sets the block at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the block to get or set.</param>
        public List<T> this[int index]
        {
            get
            {
                return _blocks[index];
            }
        }

        //Data
        /// <summary>
        /// Default size of one DistributedArray(T) block. 
        /// Because of the way memory allocation is most effective that it is a power of 2.
        /// </summary>
        public int DefaultBlockSize
        {
            get
            {
                return _defaultBlockSize;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                if (value > MaxBlockSize)
                {
                    throw new ArgumentOutOfRangeException("value", "DefaultBlockSize cant be more than MaxBlockSize");
                }

                _defaultBlockSize = value;
            }
        }
        /// <summary>
        /// The size of any block never will be more than this number.
        /// Because of the way memory allocation is most effective that it is a power of 2.
        /// </summary>
        public int MaxBlockSize
        {
            get
            {
                return _maxBlockSize;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                _maxBlockSize = value;
            }
        }
        /// <summary>
        /// Get the number of blocks actually contained in the block collection.
        /// </summary>
        public int Count
        {
            get
            {
                return _blocks.Count;
            }
        }

        /// <summary>
        /// Internal value of DefaultBlockSize. Never used it out of DefaultBlockSize set and get method.
        /// </summary>
        private int _defaultBlockSize;
        /// <summary>
        /// Internal value of MaxBlockSize. Never used it out of DefaultBlockSize set and get method.
        /// </summary>
        private int _maxBlockSize;
        /// <summary>
        /// Collection with blocks provides the main data object in the Blocks class.
        /// It contain at least one block(this block can be empty).
        /// </summary>
        private readonly List<List<T>> _blocks;
    }
}
