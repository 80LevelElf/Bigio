using System;
using System.Collections;
using System.Collections.Generic;
using Bigio.BigArray.Interfaces;
using Bigio.BigArray.Support_Classes.Balancer;
using Bigio.Common.Managers;

namespace Bigio.BigArray.Support_Classes.BlockCollection
{
    /// <summary>
    /// BlockCollection is collection of blocks which abstracts you from internal work with block collection.
    /// </summary>
    /// <typeparam name="T">Type of block elements.</typeparam>
    public partial class BlockCollection<T> : ICollection<Block<T>>
    {
        //Data

        /// <summary>
        /// Collection which used to real containing of blocks
        /// </summary>
        private readonly IBigList<Block<T>> _blocks;

		/// <summary>
		/// Balancer we use to get information about block sizes
		/// </summary>
        private readonly IBalancer _balancer;

		//API

		/// <summary>
		/// Create new instance of <see cref="BlockCollection{T}"/> with specified <paramref name="balancer"/>
		/// and <paramref name="internalBlockCollection"/>.
		/// </summary>
		/// <param name="balancer">Balancer we use to get information about block sizes</param>
		/// <param name="internalBlockCollection">Collection which used to real containing of blocks</param>
		public BlockCollection(IBalancer balancer, IBigList<Block<T>> internalBlockCollection)
        {
            _balancer = balancer;
            _blocks = internalBlockCollection;

            IsReadOnly = false;
        }

		/// <summary>
		/// Create new instance of <see cref="BlockCollection{T}"/> with specified <paramref name="balancer"/>
		/// and default intenalBlockCollection (<see cref="InternalBlockList{T}"/>).
		/// </summary>
		/// <param name="balancer">Balancer we use to get information about block sizes</param>
		public BlockCollection(IBalancer balancer) : this(balancer, new InternalBlockList<Block<T>>())
        {
            
        }

		/// <summary>
		/// Create new instance of <see cref="BlockCollection{T}"/> with specified <paramref name="internalBlockCollection"/>
		/// and default balancer (<see cref="FixedBalancer"/>).
		/// </summary>
		/// <param name="internalBlockCollection">Collection which used to real containing of blocks</param>
		public BlockCollection(IBigList<Block<T>> internalBlockCollection) : this(new FixedBalancer(), internalBlockCollection)
        {
            
        }

		/// <summary>
		/// Create new instance of <see cref="BlockCollection{T}"/> with default intenalBlockCollection 
		/// (<see cref="InternalBlockList{T}"/>) and balancer (<see cref="FixedBalancer"/>).
		/// </summary>
		public BlockCollection() : this(new FixedBalancer(), new InternalBlockList<Block<T>>())
        {
            
        }

        /// <summary>
        /// Add new block to the end of collection of blocks of the <see cref="BlockCollection{T}"/>.
        /// If your block is too big, function will try to divite it into blocks.
        /// If the block is empty, function won't add it(If you want to do it, you will must to use <see cref="AddNewBlock"/> function).
        /// If block is too big, it will be divide into several blocks.
        /// </summary>
        /// <param name="block">Block to add.</param>
        public void Add(Block<T> block)
        {
            if (block == null)
                throw new ArgumentNullException("block");

            Add(block, 0, block.Count);
        }

        /// <summary>
        /// Add new block to the end of collection of blocks of the <see cref="BlockCollection{T}"/>.
        /// If your block is too big, function will try to divite it into blocks.
        /// If the block is empty, function wont add it. 
        /// it(If you want to do it, you will must to use <see cref="AddNewBlock"/> function).
        /// If block is too big, it will be divided into several blocks.
        /// </summary>
        /// <param name="block">Block to add.</param>
        public void Add(ICollection<T> block)
        {
            if (block == null)
                throw new ArgumentNullException("block");

            Add(block, 0, block.Count);
        }

        /// <summary>
        /// Add data starting with index of block to the end of collection 
        /// of blocks of the <see cref="BlockCollection{T}"/> as new blocks.
        /// If your block is too big, function will try to divite it into blocks.
        /// If the block is empty, function wont add it. 
        /// it(If you want to do it, you will must to use <see cref="AddNewBlock"/> function).
        /// If block is too big, it will be divided into several blocks.
        /// </summary>
        /// <param name="block">Block to add.</param>
        /// <param name="blockSubindex">The zero-based index in block at which copying begins.</param>
        public void Add(ICollection<T> block, int blockSubindex)
        {
            Add(block, blockSubindex, block.Count - blockSubindex);
        }

        /// <summary>
        /// Add data from specified range of block to the end of collection
        /// of blocks of the <see cref="BlockCollection{T}"/> as new blocks.
        /// If your block is too big, function will try to divite it into blocks.
        /// If the block is empty, function wont add it. 
        /// it(If you want to do it, you will must to use <see cref="AddNewBlock"/> function).
        /// If block is too big, it will be divided into several blocks.
        /// </summary>
        /// <param name="block">Block to add.</param>
        /// <param name="blockSubindex">The zero-based index in block at which copying begins.</param>
        /// <param name="blockCount">The number of elements of block to copy.</param>
        public void Add(ICollection<T> block, int blockSubindex, int blockCount)
        {
            if (block.Count != 0)
                _blocks.AddRange(DivideIntoBlocks(block, blockSubindex, blockCount));
        }

        /// <summary>
        /// Add new empty block as last block.
        /// </summary>
        public void AddNewBlock()
        {
            _blocks.Add(new Block<T>(_balancer.GetNewBlockSize(_blocks.Count)));
        }

        /// <summary>
        /// Adds the blocks of the specified collection to the end of the <see cref="BlockCollection"/>.
        /// If block is too big, it will be divide into several blocks.
        /// All blocks in the range can't be null, but block can be empty
        /// (in this way function don't add block in the <see cref="BlockCollection"/>).
        /// </summary>
        /// <param name="range">The collection of blocks whose elements should be added 
        /// to the end of the <see cref="BlockCollection{T}"/>.
        /// The collection itself cannot benull and cant contain null elements.</param>
        public void AddRange(ICollection<Block<T>> range)
        {
            if (range == null)
                throw new ArgumentNullException("range");

            //Divide each block
            foreach (var block in range)
            {
                _blocks.AddRange(DivideIntoBlocks(block));
            }
        }

        /// <summary>
        /// Remove all blocks from the <see cref="BlockCollection{T}"/>.
        /// </summary>
        public void Clear()
        {
            _blocks.Clear();
        }

        /// <summary>
        /// Remove true if <see cref="BlockCollection{T}"/> contains value, otherwise return false.
        /// </summary>
        /// <param name="item">Block to be checked.</param>
        public bool Contains(Block<T> item)
        {
            return _blocks.Contains(item);
        }

        /// <summary>
        /// Copies the entire <see cref="BlockCollection{T}"/> to a compatible one-dimensional array
        /// , starting at the specified <see cref="arrayIndex"/> of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional Array that is the destination of the blocks copied from <see cref="BlockCollection{T}"/>.
        ///  The <see cref="array"/> must have zero-based indexing. </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(Block<T>[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            if (!array.IsValidRange(arrayIndex, Count))
                throw new ArgumentOutOfRangeException("arrayIndex");

            _blocks.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection of blocks.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Block<T>> GetEnumerator()
        {
            return new BlockCollectionEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Inserts an element into the <see cref="BigArray{T}"/> at the specified index.
        /// If your block is too big, function will try to divite it into blocks.
        /// If the block is empty, function will insert it. 
        /// it(If you want to do it, you will must to use InsertNewBlock function).
        /// </summary>
        /// <param name="index">Index of collection of block where the new block will be.</param>
        /// <param name="block">Block to be inserted.</param>
        public void Insert(int index, Block<T> block)
        {
            InsertRange(index, DivideIntoBlocks(block));
        }

        /// <summary>
        /// Insert new empty block at the specified idex.
        /// You need to use this function istead of <see cref="Insert"/>(index, emptyBlock) because if
        /// block is empty, <see cref="Insert"/> function won't add it in the <see cref="BlockCollection"/>. 
        /// </summary>
        /// <param name="indexOfBlock"></param>
        public void InsertNewBlock(int indexOfBlock)
        {
            _blocks.Insert(indexOfBlock, new Block<T>(_balancer.GetNewBlockSize(indexOfBlock)));
        }

        /// <summary>
        /// Inserts the elements of a block range into the block collection at the specified <see cref="index"/>.
        /// If block is too big, it will be divide into several blocks.
        /// All blocks in the range can't be null, but block can be empty
        /// (in this way function dont insert block in the <see cref="BlockCollection"/>).
        /// </summary>
        /// <param name="index">The zero-based index at which the new elements should be inserted.</param>
        /// <param name="range">The range whose elements should be inserted into the <see cref="BlockCollection{T}"/>.
        ///  The range it self cannot be null, and cant contain null blocks.</param>
        public void InsertRange(int index, ICollection<Block<T>> range)
        {
            if (range == null)
                throw new ArgumentNullException("range");

            if (!this.IsValidIndex(index) && index != Count/*We also can insert item as last block*/)
                throw new ArgumentOutOfRangeException("index");

            var blocksToInsert = new List<Block<T>>(range.Count);

            //Divide each block
            foreach (var block in range)
            {
                blocksToInsert.AddRange(DivideIntoBlocks(block));
            }

            if (blocksToInsert.Count != 0)
                _blocks.InsertRange(index, blocksToInsert);
        }

        /// <summary>
        /// Removes the first occurrence of a specific block from the <see cref="BlockCollection{T}"/>.
        /// </summary>
        /// <param name="block">The object to remove from the <see cref="BigArray{T}"/>.</param>
        /// <returns>True if item is successfully removed; otherwise, false.
        ///  This method also returns false if item was not found in the <see cref="BlockCollection{T}"/>.</returns>
        public bool Remove(Block<T> block)
        {
            return _blocks.Remove(block);
        }

        /// <summary>
        /// Removes the block at the specified index of the <see cref="BlockCollection{T}"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the block to remove.</param>
        public void RemoveAt(int index)
        {
            if (!this.IsValidIndex(index))
                throw new ArgumentOutOfRangeException("index");

            _blocks.RemoveAt(index);
        }

        /// <summary>
        /// Reverses the order of the blocks in the entire <see cref="BlockCollection{T}"/>.
        /// </summary>
        public void Reverse()
        {
           _blocks.Reverse();
        }

        /// <summary>
        /// Try to divide block into severals blocks if it has index greater or equeal than max size of
        /// block at this index (depend on balancer).
        /// </summary>
        /// <param name="indexOfBlock">Index of block to be divided.</param>
        public void TryToDivideBlock(int indexOfBlock)
        {
            if (!this.IsValidIndex(indexOfBlock))
                throw new ArgumentOutOfRangeException("indexOfBlock");

            int count = _blocks[indexOfBlock].Count;
            if (count >= _balancer.GetMaxBlockSize(indexOfBlock))
            {
                var newBlocks = DivideIntoBlocks(_blocks[indexOfBlock]);
                RemoveAt(indexOfBlock);
                InsertRange(indexOfBlock, newBlocks);
            }
        }

        /// <summary>
        /// If there is no any blocks - add first empty block.
        /// </summary>
        public void AddFirstBlockIfThereIsNeeded()
        {
            if (Count == 0)
                AddNewBlock();
        }

        //Support functions

        /// <summary>
        /// Divide specified collection into blocks with <see cref="DefaultBlockSize"/> size.
        /// </summary>
        /// <param name="collection">Collection, which must be divided.</param>
        /// <returns>Blocks constructed on the basis of the collection with <see cref="DefaultBlockSize"/> size.</returns>
        private ICollection<Block<T>> DivideIntoBlocks(ICollection<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            if (collection.Count == 0)
                return new Block<T>[0];

            return DivideIntoBlocks(collection, 0, collection.Count);
        }

		/// <summary>
		/// Divide specified block into several blocks(if it's needed) according to correspond default block sizes.
		/// </summary>
		/// <param name="collection">Collection, which must be divided.</param>
		/// <param name="collectionIndex">The zero-based starting index of the <see cref="collection"/> of elements to divide.</param>
		/// <param name="countToDivide">The number of elements of the <see cref="collection"/> to divide.</param>
		/// <returns>Collection of new blocks.</returns>
		private IList<Block<T>> DivideIntoBlocks(ICollection<T> collection, int collectionIndex, int countToDivide)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            if (!collection.IsValidRange(collectionIndex, countToDivide))
                throw new ArgumentOutOfRangeException();

            List<Block<T>> blockList = new List<Block<T>>();

            int alreadyProcessedCount = 0;

            var collectionEnumerator = collection.GetEnumerator();
            for (int i = 0; i < collectionIndex; i++) //Move item to the index position
            {
                collectionEnumerator.MoveNext();
            }

            for (int i = 0; alreadyProcessedCount != countToDivide; i++)
            {
                int currentBlockSize = _balancer.GetDefaultBlockSize(i);

                Block<T> newBlock = new Block<T>(currentBlockSize);
                blockList.Add(newBlock);

                currentBlockSize = Math.Min(currentBlockSize, countToDivide - alreadyProcessedCount);

                for (int j = 0; j < currentBlockSize; j++)
                {
                    collectionEnumerator.MoveNext();
                    newBlock.Add(collectionEnumerator.Current);
                }

                alreadyProcessedCount += currentBlockSize;
            }

            return blockList;
        }

        //Data

        /// <summary>
        /// Gets or sets the block at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the block to get or set.</param>
        public Block<T> this[int index]
        {
            get
            {
                return _blocks[index];
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

        public bool IsReadOnly { get; }
    }
}

