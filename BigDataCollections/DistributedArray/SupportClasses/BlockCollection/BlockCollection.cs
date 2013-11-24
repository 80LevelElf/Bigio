using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BigDataCollections.DistributedArray.Managers;

namespace BigDataCollections.DistributedArray.SupportClasses.BlockCollection
{
    /// <summary>
    /// BlockCollection is collection of blocks which abstracts you from internal work with block 
    /// collection. BlockCollection always contain at least one block(it is can be empty)
    /// that called "insuring block".
    /// </summary>
    /// <typeparam name="T">Type of block elements.</typeparam>
    partial class BlockCollection<T> : ICollection<List<T>>
    {
        //API
        /// <summary>
        /// Create a new instance of Blocks(T) class.
        /// Blocks(T) is a shell of blocks collection, which use for more
        /// simple usage of it.
        /// </summary>
        public BlockCollection():this(new Collection<T>())
        {
        }
        /// <summary>
        /// Create a new instance of Blocks(T) class.
        /// Blocks(T) is a shell of blocks collection, which use for more
        /// simple usage of it. 
        /// </summary>
        /// <param name="collection">Collection whitch use as base for new DistributedArray(T).
        /// The collection it self cannot be null and cant contain null blocks
        /// , if type T is a reference type. </param>
        public BlockCollection(ICollection<T> collection)
        {
            MaxBlockSize = DefaultValuesManager.MaxBlockSize;
            DefaultBlockSize = DefaultValuesManager.DefaultBlockSize;
            _blocks = new List<List<T>>();

            var blocks = DivideIntoBlocks(collection);
            if (blocks.Count != 0)
            {
                AddRange(blocks);
            }
            else
            {
                TryToAddInsuringBlock();
            }
        }
        /// <summary>
        /// Add new block to the end of collection of blocks of the BlockCollection(T).
        /// If your block is too big - function try to divite it into blocks.
        /// If the block is empty - function dont add 
        /// it(If you want to do it - use AddNewBlock function).
        /// </summary>
        /// <param name="block">Block to add. Block cant be null.</param>
        public void Add(List<T> block)
        {
            Add(block, 0, block.Count);
        }
        /// <summary>
        /// Add new block to the end of collection of blocks of the BlockCollection(T).
        /// If your block is too big - function try to divite it into blocks.
        /// If the block is empty - function dont add 
        /// it(If you want to do it - use AddNewBlock function).
        /// </summary>
        /// <param name="block">Block to add. Block cant be null.</param>
        public void Add(ICollection<T> block)
        {
            Add(block, 0, block.Count);
        }
        /// <summary>
        /// Add data starting with index of block to the end of collection 
        /// of blocks of the BlockCollection(T) as new blocks.
        /// If your block is too big - function try to divite it into blocks.
        /// If the block is empty - function dont add 
        /// it(If you want to do it - use AddNewBlock function).
        /// </summary>
        /// <param name="block">Block to add. Block cant be null.</param>
        /// <param name="blockSubindex">The zero-based index in block at which copying begins.</param>
        public void Add(ICollection<T> block, int blockSubindex)
        {
            Add(block, blockSubindex, block.Count - blockSubindex);
        }
        /// <summary>
        /// Add data from specified range of block to the end of collection
        /// of blocks of the BlockCollection(T) as new blocks.
        /// If your block is too big - function try to divite it into blocks.
        /// If the block is empty - function dont add 
        /// it(If you want to do it - use AddNewBlock function).
        /// </summary>
        /// <param name="block">Block to add. Block cant be null.</param>
        /// <param name="blockSubindex">The zero-based index in block at which copying begins.</param>
        /// <param name="blockCount">The number of elements of block to copy.</param>
        public void Add(ICollection<T> block, int blockSubindex, int blockCount)
        {
            if (block == null)
            {
                throw new ArgumentNullException("block");
            }

            var blocks = DivideIntoBlocks(block, blockSubindex, blockCount);
            AddRange(blocks);
        }
        /// <summary>
        /// Add new block as last block. New block will have DefaultBlockSize capacity.
        /// You need to use this function istead of Add(emptyBlock) because if
        /// block is empty Add() function dont add it in the BlockCollection. 
        /// </summary>
        public void AddNewBlock()
        {
            TryToRemoveInsuringBlock();
            _blocks.Add(new List<T>(DefaultBlockSize));
        }
        /// <summary>
        /// Adds the blocks of the specified collection to the end of the BlockCollection.
        /// If block is too big - it divide into several blocks.
        /// All blocks in the range cant be null, but block can be empty
        /// (in this way function dont add block in the BlockCollection).
        /// </summary>
        /// <param name="range">The collection of blocks whose elements should be added 
        /// to the end of the BlockCollection(T).
        /// The collection itself cannot benull and cant contain null elements.</param>
        public void AddRange(ICollection<List<T>> range)
        {
            var blocksToAdd = new List<List<T>>(range.Count);
            foreach (var block in range)
            {
                blocksToAdd.AddRange(DivideIntoBlocks(block));
            }

            if (blocksToAdd.Count != 0)
            {
                TryToRemoveInsuringBlock();
                _blocks.AddRange(blocksToAdd);
            }
        }
        /// <summary>
        /// Remove all blocks from the BlockCollection(T) and add insuring block.
        /// </summary>
        public void Clear()
        {
            TryToRemoveInsuringBlock(); //Unload InsuringBlock to _blocks
            _blocks.Clear();

            TryToAddInsuringBlock();
        }
        /// <summary>
        /// Remove true if BlockCollection(T) contains value, otherwise return false.
        /// </summary>
        /// <param name="item">Block to be checked.</param>
        public bool Contains(List<T> item)
        {
            var result = _blocks.Contains(item);

            if (!result && InsuringBlock != null)
            {
                result = item.Equals(InsuringBlock);
            }

            return result;
        }
        /// <summary>
        /// Copies the entire BlockCollection(T) to a compatible one-dimensional array
        /// , starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional Array that is the destination of the blocks copied from BlockCollection(T).
        ///  The Array must have zero-based indexing. </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins. </param>
        public void CopyTo(List<T>[] array, int arrayIndex)
        {
            if (InsuringBlock != null)
            {
                array[arrayIndex] = InsuringBlock;
                arrayIndex++;
            }

            _blocks.CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// Removes the first occurrence of a specific block from the BlockCollection(T).
        /// </summary>
        /// <param name="block">The object to remove from the DistributedArray(T).</param>
        /// <returns>True if item is successfully removed; otherwise, false.
        ///  This method also returns false if item was not found in the BlockCollection(T).</returns>
        public bool Remove(List<T> block)
        {
            bool result;
            if (InsuringBlock != null && block.Equals(InsuringBlock))
            {
                TryToRemoveInsuringBlock();
                result = true;
            }
            else
            {
                result = _blocks.Remove(block);                
            }

            TryToAddInsuringBlock();
            return result;
        }
        /// <summary>
        /// Removes the block at the specified index of the _blocks.
        /// </summary>
        /// <param name="index">The zero-based index of the block to remove.</param>
        public void RemoveAt(int index)
        {
            if (InsuringBlock == null)
            {
                _blocks.RemoveAt(index);
            }
                //Remove InsuringBlock
            else
            {
                if (index == 0)
                {
                    TryToRemoveInsuringBlock();
                }
                else
                {
                    throw new ArgumentOutOfRangeException("index");
                }
            }
            TryToAddInsuringBlock();
        }
        /// <summary>
        /// Returns an enumerator that iterates through the collection of blocks.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<List<T>> GetEnumerator()
        {
            return new BlockCollectionEnumerator(this);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        /// <summary>
        /// Inserts an element into the DistributedArray(T) at the specified index.
        /// If your block is too big - function try to divite it into blocks.
        /// If the block is empty - function dont insert 
        /// it(If you want to do it - use InsertNewBlock function).
        /// </summary>
        /// <param name="index">Index of collection of block where the new block will be.</param>
        /// <param name="block">Block to insert.</param>
        public void Insert(int index, List<T> block)
        {
            InsertRange(index, DivideIntoBlocks(block));
        }
        public void InsertNewBlock(int index)
        {
            TryToRemoveInsuringBlock();
            _blocks.Insert(index, new List<T>(DefaultBlockSize));
        }
        /// <summary>
        /// Inserts the elements of a block range into the block collection at the specified index.
        /// If block is too big - it divide into several blocks.
        /// All blocks in the range cant be null, but block can be empty
        /// (in this way function dont insert block in the BlockCollection).
        /// </summary>
        /// <param name="index">The zero-based index at which the new elements should be inserted.</param>
        /// <param name="range">The range whose elements should be inserted into the block collection.
        ///  The range it self cannot be null, and cant contain null blocks.</param>
        public void InsertRange(int index, ICollection<List<T>> range)
        {
            var blocksToInsert = new List<List<T>>(range.Count);
            foreach (var block in range)
            {
                blocksToInsert.AddRange(DivideIntoBlocks(block));
            }

            if (blocksToInsert.Count != 0)
            {
                TryToRemoveInsuringBlock();
                _blocks.InsertRange(index, blocksToInsert);                
            }
        }
        public void TryToDivideBlock(int indexOfBlock)
        {
            if (InsuringBlock == null)
            {
                if (_blocks[indexOfBlock].Count >= MaxBlockSize)
                {
                    var newBlocks = DivideIntoBlocks(_blocks[indexOfBlock]);
                    RemoveAt(indexOfBlock);
                    InsertRange(indexOfBlock, newBlocks);
                }
            }
            //Try to divide InsuringBlock
            else
            {
                if (indexOfBlock == 0)
                {
                    TryToRemoveInsuringBlock();
                    TryToDivideBlock(0); //Divide InsuringBlock as simple block
                }
                else
                {
                    throw new ArgumentOutOfRangeException("indexOfBlock");
                }
            }
        }
        /// <summary>
        /// Gets or sets the block at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the block to get or set.</param>
        public List<T> this[int index]
        {
            get
            {
                if (_blocks.Count == 0 && index == 0)
                {
                    return InsuringBlock;
                }
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
                if (_blocks.Count == 0)
                {
                    return 1; //In this way we have InsuringBlock.
                }
                return _blocks.Count;
            }
        }
        public bool IsReadOnly { get; private set; }

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
        private List<T> InsuringBlock { get; set; }

        //Support function
        private void TryToAddInsuringBlock()
        {
            if (_blocks.Count == 0 && InsuringBlock == null)
            {
                InsuringBlock = new List<T>(DefaultBlockSize);
            }
        }
        private void TryToRemoveInsuringBlock()
        {
            if (InsuringBlock != null)
            {
                if (InsuringBlock.Count != 0)
                {
                    _blocks.Insert(0, InsuringBlock); //We cant use Insert method of BlockCollection
                    //because there is eternal recursion
                }
                InsuringBlock = null;
            }
        }
        /// <summary>
        /// Divide specified collection into blocks with DefaultBlockSize size.
        /// </summary>
        /// <param name="collection">Collection, which must be divided.</param>
        /// <returns>Blocks constructed on the basis of the collection with DefaultBlockSize size.</returns>
        private ICollection<List<T>> DivideIntoBlocks(ICollection<T> collection)
        {
            return DivideIntoBlocks(collection, 0, collection.Count);
        }
        /// <summary>
        /// Divide specified collection into blocks with DefaultBlockSize size that starts at the specified index.
        /// </summary>
        /// <param name="collection">Collection, which must be divided.</param>
        /// <param name="index">The zero-based starting index of the collection of elements to divide.</param>
        /// <returns>Blocks constructed on the basis of the collection with DefaultBlockSize size.</returns>
        private ICollection<List<T>> DivideIntoBlocks(ICollection<T> collection, int index)
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
        private ICollection<List<T>> DivideIntoBlocks(ICollection<T> collection, int index, int count)
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
    }
}
