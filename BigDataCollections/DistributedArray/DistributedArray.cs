﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BigDataCollections.DistributedArray.Managers;
using BigDataCollections.DistributedArray.Managers.StructureManager;
using BigDataCollections.DistributedArray.SupportClasses;
using BigDataCollections.DistributedArray.SupportClasses.BlockCollection;

namespace BigDataCollections
{
    /// <summary>
    /// DistributedArray(T) provides collection of elements of T type, with indices of the items ranging from 0 to one less
    /// than the count of items in the collection. DistributedArray(T) consist of blocks. Size of block more or equal 0 and less then
    /// MaxBlockSize. Because of architecture many actions(remove, insert, add) faster than their counterparts in List or LinedList, 
    /// but some operations might slower because of due to the nature of architecture and the overhead. 
    /// It makes no sense to use it with a small number of items(less than 1000), because in that case List and LinkedList
    /// most operations will be more efficient.
    /// </summary>
    /// <typeparam name="T">Type of array elements.</typeparam>
    public partial class DistributedArray<T> : IList<T>
    {
        //API
        /// <summary>
        /// Create a new instance of the DistributedArray(T) class that is empty 
        /// and has one empty block with DefaultBlockSize capacity.
        /// </summary>
        public DistributedArray() : this(new Collection<T>())
        {
        }
        /// <summary>
        /// Create a new instance of the DistributedArray(T) class using elements from specified collection.
        /// </summary>
        /// <param name="collection">Collection whitch use as base for new DistributedArray(T).
        /// The collection it self cannot be null, but it can contain elements that are null, if type T is a reference type.</param>
        public DistributedArray(ICollection<T> collection)
        {
            IsReadOnly = false;
            Count = collection.Count;

            _blockCollection = new BlockCollection<T>(collection);
            _structureManager = new StructureManager<T>(_blockCollection);
        }
        /// <summary>
        /// Add an object to the end of last block of the DistributedArray(T).
        /// </summary>
        public void Add(T value)
        {            
            int indexOfBlock = _blockCollection.Count - 1;
            if (_blockCollection[indexOfBlock].Count >= MaxBlockSize)
            {
                _blockCollection.AddNewBlock();
                indexOfBlock++;
            }

            _blockCollection[indexOfBlock].Add(value);

            Count++;
            _structureManager.DataChanged();
        }
        /// <summary>
        /// Adds the elements of the specified collection to the end of the last block of DistributedArray(T)
        /// or if there is needed - create another block.
        /// </summary>
        /// <param name="collection">The collection whose elements should be added to the end of the DistributedArray(T).
        ///  The collection it self cannot benull, but it can contain elements that are null, if type T is a reference type. </param>
        public void AddRange(ICollection<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            var lastBlock = _blockCollection[_blockCollection.Count - 1];

            //Transfer data to the last block while it is possible
            var sizeToFill = 0;
            var emptySize = MaxBlockSize - lastBlock.Count;

            if (emptySize != 0)
            {
                sizeToFill = (emptySize < collection.Count) ? emptySize : collection.Count;
                var enumerator = collection.GetEnumerator();

                for (int i = 0; i < sizeToFill; i++)
                {
                    enumerator.MoveNext();
                    lastBlock.Add(enumerator.Current);
                }
            }

            //Transfer other data as new blocks
            _blockCollection.Add(collection, sizeToFill);

            Count += collection.Count;
            _structureManager.DataChanged();
        }
        /// <summary>
        /// Returns a read-only wrapper based on current DistributedArray(T).
        /// </summary>
        /// <returns>A ReadOnlyCollection(T) that acts as a read-only wrapper around the current DistributedArray(T).</returns>
        public ReadOnlyCollection<T> AsReadOnly()
        {
            return new ReadOnlyCollection<T>(this);
        }
        /// <summary>
        /// Searches the entire sorted DistributedArray(T) for an element using the default comparer and returns the zero-based index of the element.
        /// </summary>
        /// <param name="item">The object to locate. The value can be null for reference types.</param>
        /// <returns>The zero-based index of item in the sorted DistributedArray(T), ifitem is found; otherwise,
        ///  a negative number that is the bitwise complement of the index of the next element that is larger than item or,
        ///  if there is no larger element, the bitwise complement of Count. </returns>
        public int BinarySearch(T item)
        {
            return BinarySearch(0, Count, item, Comparer<T>.Default);
        }
        /// <summary>
        /// Searches the entire sorted DistributedArray(T) for an element using the specified comparer and returns the zero-based index of the element.
        /// </summary>
        /// <param name="item">The object to locate. The value can be null for reference types.</param>
        /// <param name="comparer">The IComparer(T) implementation to use when comparing elements.
        ///-or- 
        ///null to use the default comparer Comparer(T).Default.
        ///</param>
        /// <returns>The zero-based index of item in the sorted DistributedArray(T), ifitem is found; otherwise,
        ///  a negative number that is the bitwise complement of the index of the next element that is larger than item or,
        ///  if there is no larger element, the bitwise complement of Count. </returns>
        public int BinarySearch(T item, IComparer<T> comparer)
        {
            return BinarySearch(0, Count, item, comparer);
        }
        /// <summary>
        /// Searches a range of elements in the sorted DistributedArray(T) for an element using the specified comparer and returns the zero-based index of the element.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="item">The object to locate. The value can be null for reference types.</param>
        /// <param name="comparer">The IComparer(T) implementation to use when comparing elements, ornull to use the default comparer Comparer(T).Default.</param>
        /// <returns></returns>
        public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
        {
            if (!this.IsValidRange(index, count))
            {
                throw new ArgumentOutOfRangeException();
            }

            if (comparer == null)
            {
                comparer = Comparer<T>.Default;
            }

            if (index == 0 && count == 0)
            {
                return ~0;
            }

            int startIndex = index;
            int endIndex = index + count;

            while (startIndex <= endIndex)
            {
                int middlePosition = (startIndex + endIndex)/2;
                T middleValue = this[middlePosition];
                //Compare
                int compareResult = comparer.Compare(item, middleValue);
                switch (compareResult)
                {
                    case -1:
                        endIndex = middlePosition - 1;
                        break;
                    case 0:
                        return middlePosition;
                    case 1:
                        startIndex = middlePosition + 1;
                        break;
                }
            }
            //If there is no such item specify the location where the element should be
            if (endIndex == -1) // if we need first element
            {
                return -1;
            }

            //Because there is no such item, we will find plae for it
            var enumerator = GetEnumerator();
            ((DistributedArrayEnumerator)enumerator).MoveToIndex(endIndex); // Move to start position
            var counter = 0;
            while (endIndex + counter != index + count && comparer.Compare(enumerator.Current, item) != 1)
            {
                enumerator.MoveNext();
                counter++;
            }
            return ~(endIndex + counter);
        }
        /// <summary>
        /// Removes all elements from the DistributedArray(T).
        /// </summary>
        public void Clear()
        {
            _blockCollection.Clear();
            Count = 0;
        }
        /// <summary>
        /// Remove true if DistributedArray(T) contains value, otherwise return false.
        /// </summary>
        /// <param name="item">Data to be checked.</param>
        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }
        /// <summary>
        /// Converts the elements in the current DistributedArray(T) to another type, and returns a list containing the converted elements.
        /// </summary>
        /// <typeparam name="TOutput">The type of the elements of the target array.</typeparam>
        /// <param name="converter">A Converter(TInput, TOutput) delegate that converts each element from one type to another type.</param>
        /// <returns>A DistributedArray(T) of the target type containing the converted elements from the current DistributedArray(T).</returns>
        public DistributedArray<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
        {
            if (converter == null)
            {
                throw new ArgumentNullException("converter");
            }

            var result = new DistributedArray<TOutput>();
            //Convert all blocks
            foreach (var block in _blockCollection)
            {
                result._blockCollection.Add(block.ConvertAll(converter));
            }
            return result;
        }
        /// <summary>
        /// Copies the entire DistributedArray(T) to a compatible one-dimensional array, starting at the beginning of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional Array that is the destination of the elements copied from DistributedArray(T).
        ///  The Array must have zero-based indexing.</param>
        public void CopyTo(T[] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException();
            }
            CopyTo(0, array, 0, array.Length);
        }
        /// <summary>
        /// Copies the entire DistributedArray(T) to a compatible one-dimensional array
        /// , starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional Array that is the destination of the elements copied from DistributedArray(T).
        ///  The Array must have zero-based indexing. </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins. </param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            CopyTo(0, array, arrayIndex, Count);
        }
        /// <summary>
        /// Copies a range of elements from the DistributedArray(T) to a compatible one-dimensional array,
        ///  starting at the specified index of the target array.
        /// </summary>
        /// <param name="index">The zero-based index in the source DistributedArray(T) at which copying begins.</param>
        /// <param name="array">The one-dimensional Array that is the destination of the elements copied from DistributedArray(T). 
        /// The Array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <param name="count">The number of elements to copy.</param>
        public void CopyTo(int index, T[] array, int arrayIndex, int count)
        {
            //If there is empty distributed array
            if (index == 0 && Count == 0)
            {
                return;
            }

            if (!this.IsValidIndex(index) || !array.IsValidRange(arrayIndex, count))
            {
                throw new ArgumentOutOfRangeException();
            }

            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            var enumerator = GetEnumerator();
            ((DistributedArrayEnumerator)enumerator).MoveToIndex(index);

            //Transfer data
            for (int i = arrayIndex; i < arrayIndex + count; i++)
            {
                array[i] = enumerator.Current;

                if (!enumerator.MoveNext())
                {
                    break;
                }
            }
        }
        /// <summary>
        /// Determines whether the DistributedArray(T) contains elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The Predicate(T) delegate that defines the conditions of the elements to search for.</param>
        /// <returns>True if the DistributedArray(T) contains one or more elements that match the conditions defined by the specified predicate; otherwise false. </returns>
        public bool Exists(Predicate<T> match)
        {
            return _blockCollection.Any(block => block.Exists(match));
        }
        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate,
        ///  and returns the first occurrence within the entire DistributedArray(T).
        /// </summary>
        /// <param name="match">The Predicate(T) delegate that defines the conditions of the element to search for.</param>
        /// <returns>The first element that matches the conditions defined by the specified predicate, if found;
        ///  otherwise, the default value for type T. </returns>
        public T Find(Predicate<T> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            foreach (var item in this)
            {
                if (match.Invoke(item))
                {
                    return item;
                }
            }
            //If there is not needed item
            return default(T);
        }
        /// <summary>
        /// Retrieves all the elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The Predicate(T) delegate that defines the conditions of the elements to search for.</param>
        /// <returns>A DistributedArray(T) containing all the elements that match the conditions defined by the specified predicate,
        ///  if found; otherwise, an empty DistributedArray(T).</returns>
        public DistributedArray<T> FindAll(Predicate<T> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            var result = new DistributedArray<T>();
            foreach (var item in this)
            {
                if (match.Invoke(item))
                {
                    result.Add(item);
                }
            }
            return result;
        }
        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate,
        ///  and returns the zero-based index of the first occurrence within the entire DistributedArray(T).
        /// </summary>
        /// <param name="match">The Predicate(T) delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by match, if found; otherwise, –1.</returns>
        public int FindIndex(Predicate<T> match)
        {
            return FindIndex(0, Count, match);
        }
        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate,
        ///  and returns the zero-based index of the first occurrence within the range of elements in the DistributedArray(T)
        ///  that extends from the specified index to the last element.
        /// </summary>
        /// <param name="index">The zero-based starting index of the search.</param>
        /// <param name="match">The Predicate(T) delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by match, if found; otherwise, –1. </returns>
        public int FindIndex(int index, Predicate<T> match)
        {
            return FindIndex(index, Count - index, match);
        }
        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate,
        ///  and returns the zero-based index of the first occurrence within the range of elements in the DistributedArray(T)
        ///  that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="index">The zero-based starting index of the search. </param>
        /// <param name="count">The number of elements in the section to search. </param>
        /// <param name="match">The Predicate(T) delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by match,
        ///  if found; otherwise, –1. </returns>
        public int FindIndex(int index, int count, Predicate<T> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            var range = _structureManager.MultyblockRange(index, count);

            for (int i = range.IndexOfStartBlock; i < range.IndexOfStartBlock + range.Count; i++)
            {
                var blockRange = range[i - range.IndexOfStartBlock];
                int findIndexResult = _blockCollection[i].FindIndex(blockRange.Subindex, blockRange.Count, match);

                if (findIndexResult != -1)
                {
                    return blockRange.CommonBlockStartIndex + findIndexResult;
                }
            }
            //If there is no needed value
            return -1;
        }
        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate,
        ///  and returns the last occurrence within the entire DistributedArray(T).
        /// </summary>
        /// <param name="match">The Predicate(T) delegate that defines the conditions of the element to search for.</param>
        /// <returns>The last element that matches the conditions defined by the specified predicate, if found; otherwise, the default value for type T.</returns>
        public T FindLast(Predicate<T> match)
        {
            for (int i = _blockCollection.Count - 1; i != 0; i--)
            {
                var block = _blockCollection[i];
                for (int j = block.Count - 1; j != 0; j--)
                {
                    if (match.Invoke(block[j]))
                    {
                        return block[j];
                    }
                }
            }
            //If there is no such item
            return default(T);
        }
        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate,
        ///  and returns the zero-based index of the last occurrence within the entire DistributedArray(T).
        /// </summary>
        /// <param name="match">The Predicate(T) delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the last occurrence of an element that matches the conditions defined by match, if found; otherwise, –1.</returns>
         public int FindLastIndex(Predicate<T> match)
        {
            int index = (Count == 0) ? 0 : Count - 1;
            return FindLastIndex(index, Count, match);
        }
        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last 
        /// occurrence within the range of elements in the DistributedArray(T) that extends from the first element to the specified index.
        /// </summary>
        /// <param name="index">The zero-based starting index of the backward search.</param>
        /// <param name="match">The Predicate(T) delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the last occurrence of an element that matches the conditions defined by match, if found; otherwise, –1.</returns>
        public int FindLastIndex(int index, Predicate<T> match)
        {
            return FindLastIndex(index, index + 1, match);
        }
        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the 
        /// last occurrence within the range of elements in the DistributedArray(T) that contains the specified number of elements and ends at the specified index.
        /// </summary>
        /// <param name="index">The zero-based starting index of the backward search.</param>
        /// <param name="count">The number of elements to search.</param>
        /// <param name="match">The Predicate(T) delegate that defines the conditions of the element to search for.</param>
        /// <returns></returns>
        public int FindLastIndex(int index, int count, Predicate<T> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            var range = _structureManager.ReverseMultyblockRange(index, count);

            //Find it
            for (int i = range.IndexOfStartBlock; i >= range.IndexOfStartBlock - range.Count + 1; i--)
            {
                var blockRange = range[i];

                //Try to find it in current block
                int findLastIndexResult = _blockCollection[i].FindLastIndex(blockRange.Subindex, blockRange.Count, match);
                if (findLastIndexResult != -1)
                {
                    return blockRange.CommonBlockStartIndex + findLastIndexResult;
                }
            }
            //If there is no needed value
            return -1;
        }
        /// <summary>
        /// Returns an enumerator that iterates through the DistributedArray(T).
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            return new DistributedArrayEnumerator(this);
        }
        /// <summary>
        /// Creates a shallow copy of a range of elements in the source DistributedArray(T).
        /// </summary>
        /// <param name="index">The zero-based DistributedArray(T) index at which the range starts.</param>
        /// <param name="count">The number of elements in the range.</param>
        /// <returns></returns>
        public DistributedArray<T> GetRange(int index, int count)
        {
            var range = _structureManager.MultyblockRange(index, count);
            var newArray = new DistributedArray<T>();
            for (int i = range.IndexOfStartBlock; i < range.IndexOfStartBlock + range.Count; i++)
            {
                var blockRange = range[i - range.IndexOfStartBlock];
                var block = _blockCollection[i];

                if (blockRange.Count != 0)
                {
                    if (block.Count == blockRange.Count)
                    {
                        newArray._blockCollection.Add(block);
                    }
                    else
                    {
                        newArray.AddRange(block.GetRange(blockRange.Subindex, blockRange.Count));
                    }
                }
            }

            return newArray;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new DistributedArrayEnumerator(this);
        }
        /// <summary>
        /// If value conatins in DistributedArray(T) returns index of this value, otherwise return -1.
        /// </summary>
        /// <param name="item">Data, the location of which is necessary to calculate</param>
        /// <returns>The zero-based index of the first occurrence of item within the entire DistributedArray(T), if found; otherwise, –1.</returns>
        public int IndexOf(T item)
        {
            return IndexOf(item, 0, Count);
        }
        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first
        ///  occurrence within the range of elements in the DistributedArray(T) that extends from the 
        /// specified index to the last element.
        /// </summary>
        /// <param name="item">The object to locate in the DistributedArray(T). The value can benull for reference types.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <returns>The zero-based index of the first occurrence of item within the range of elements 
        /// in the DistributedArray(T) that extends fromindex to the last element, if found; otherwise, –1.</returns>
        public int IndexOf(T item, int index)
        {
            return IndexOf(item, index, Count - index);
        }
        /// <summary>
        ///  Searches for the specified object and returns the zero-based index of the first occurrence
        ///  within the range of elements in the DistributedArray(T) that starts at the specified index
        ///  and contains the specified number of elements.
        /// </summary>
        /// <param name="item">The object to locate in the DistributedArray(T). The value can benull for reference types.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list. </param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <returns>The zero-based index of the first occurrence of item within the range of elements
        ///  in the DistributedArray(T) that starts atindex and contains count number of elements, if found; otherwise, –1. </returns>
        public int IndexOf(T item, int index, int count)
        {
            var range = _structureManager.MultyblockRange(index, count);

            for (int i = range.IndexOfStartBlock; i < range.IndexOfStartBlock + range.Count; i++)
            {
                var blockRange = range[i - range.IndexOfStartBlock];
                int indexOfResult = _blockCollection[i].IndexOf(item, blockRange.Subindex, blockRange.Count);

                if (indexOfResult != -1)
                {
                    return blockRange.CommonBlockStartIndex + indexOfResult;
                }
            }
            //If there is no needed value
            return -1;
        }
        /// <summary>
        /// Inserts an element into the DistributedArray(T) at the specified index.
        /// </summary>
        /// <param name="index">Index of DistibutedArray(T) where the value will be.</param>
        /// <param name="item">The data to be placed.</param>
        public void Insert(int index, T item)
        {
            if (index == Count)
            {
                Add(item);
                return;
            }

            var blockInfo = _structureManager.BlockInformation(index);

            int blockSubindex = index - blockInfo.BlockStartIndex;
            var block = _blockCollection[blockInfo.IndexOfBlock];

            bool isMaxSize = (block.Count == MaxBlockSize);
            bool isStartSubindex = (blockSubindex == 0);

            if (isMaxSize)
            {
                _blockCollection.TryToDivideBlock(blockInfo.IndexOfBlock);
                Insert(index, item);
                return;
            }
            
            //Insertion
            if (!isStartSubindex)
            {
                _blockCollection[blockInfo.IndexOfBlock].Insert(blockSubindex, item);
                _blockCollection.TryToDivideBlock(blockInfo.IndexOfBlock);
            }
                //Try to add to the previous block
            else
            {
                //If there is need - add new block
                bool isStartBlock = (blockInfo.IndexOfBlock == 0);
                bool isPrevBlockFull = false;

                if (!isStartBlock)
                {
                    isPrevBlockFull = (_blockCollection[blockInfo.IndexOfBlock].Count == MaxBlockSize);
                }

                if (isStartBlock || isPrevBlockFull)
                {
                    _blockCollection.InsertNewBlock(blockInfo.IndexOfBlock);
                    blockInfo.IndexOfBlock++;
                }

                _blockCollection[blockInfo.IndexOfBlock - 1].Add(item);
            }

            Count++;
            _structureManager.DataChanged();
        }
        /// <summary>
        /// Inserts the elements of a collection into the DistributedArray(T) at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the new elements should be inserted. </param>
        /// <param name="collection">The collection whose elements should be inserted into the DistributedArray(T).
        ///  The collection it self cannot be null, but it can contain elements that are null, if type T is a reference type. </param>
        public void InsertRange(int index, ICollection<T> collection)
        {
            //Validity of index and count check in BlockInformation
            var blockInfo = new BlockInformation();

            //Determine indexOfBlock and blockStartIndex
            if (index == Count)
            {
                blockInfo.IndexOfBlock = _blockCollection.Count - 1;
                blockInfo.BlockStartIndex = Count - _blockCollection[blockInfo.IndexOfBlock].Count;
            }
            else if (Count == 0 && index == 0) // If there is insertion in empty DistributedArray
            {
                blockInfo.IndexOfBlock = 0;
                blockInfo.BlockStartIndex = 0;
            }
            else
            {
                blockInfo = _structureManager.BlockInformation(index); // Default case
            }
            //Insert
            _blockCollection[blockInfo.IndexOfBlock].InsertRange(
                index - blockInfo.BlockStartIndex, collection);
            _blockCollection.TryToDivideBlock(blockInfo.IndexOfBlock);

            Count += collection.Count;
            _structureManager.DataChanged();
        }
        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the last occurrence within the entire DistributedArray(T).
        /// </summary>
        /// <param name="item">The object to locate in the DistributedArray(T). The value can benull for reference types.</param>
        /// <returns>The zero-based index of the last occurrence of item within the entire the DistributedArray(T), if found; otherwise, –1.</returns>
        public int LastIndexOf(T item)
        {
            int index = (Count == 0) ? 0 : Count - 1;
            return LastIndexOf(item, index, Count);
        }
        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the last occurrence
        /// within the range of elements in the DistributedArray(T) that extends from the first element to the specified index.
        /// </summary>
        /// <param name="item">The object to locate in the DistributedArray(T). The value can benull for reference types.</param>
        /// <param name="index">The zero-based starting index of the backward search.</param>
        /// <returns>The zero-based index of the last occurrence of item within the range of 
        /// elements in the DistributedArray(T) that extends from the first element toindex, if found; otherwise, –1. </returns>
        public int LastIndexOf(T item, int index)
        {
            return LastIndexOf(item, index, index + 1);
        }
        /// <summary>
        /// Searches for the specified object and returns the zero-based index
        ///  of the last occurrence within the range of elements in the DistributedArray(T) that contains the specified 
        /// number of elements and ends at the specified index.
        /// </summary>
        /// <param name="item">The object to locate in the DistributedArray(T). The value can benull for reference types.</param>
        /// <param name="index">The zero-based starting index of the backward search. </param>
        /// <param name="count">The number of elements in the section to search. </param>
        /// <returns>The zero-based index of the last occurrence of item within the range of elements in the DistributedArray(T)
        ///  that containscount number of elements and ends at index, if found; otherwise, –1. </returns>
        public int LastIndexOf(T item, int index, int count)
        {
            var range = _structureManager.ReverseMultyblockRange(index, count);

            //Find it
            for (int i = range.IndexOfStartBlock; i >= range.IndexOfStartBlock - range.Count + 1; i--)
            {
                var blockRange = range[i];

                //Try to find it in current block
                int lastIndexOfResult = _blockCollection[i].LastIndexOf(item, blockRange.Subindex, blockRange.Count);
                if (lastIndexOfResult != -1)
                {
                    return blockRange.CommonBlockStartIndex + lastIndexOfResult;
                }
            }
            //If there is no needed value
            return -1;
        }
        /// <summary>
        /// Rebalance DistributedArray(T) to every block have DefaultBlockSize elements.
        /// </summary>
        public void Rebalance()
        {
            var divideBlocks = new BlockCollection<T>(this);

            _blockCollection.Clear();
            _blockCollection.AddRange(divideBlocks);

            //We must do it because we have changed count of elements in blocks
            _structureManager.DataChanged(); 
        }
        /// <summary>
        /// Removes the first occurrence of a specific object from the DistributedArray(T).
        /// </summary>
        /// <param name="item">The object to remove from the DistributedArray(T).
        /// The value can be null for reference types.</param>
        /// <returns>True if item is successfully removed; otherwise, false.
        ///  This method also returns false if item was not found in the DistributedArray(T).</returns>
        public bool Remove(T item)
        {
            for (int i = 0; i < _blockCollection.Count; i++)
            {
                var block = _blockCollection[i];
                int blockIndexOf = block.IndexOf(item);
                //If there is value in this block 
                if (blockIndexOf != -1)
                {
                    block.Remove(item);
                    //If there is empty block - remove it
                    if (block.Count == 0)
                    {
                        _blockCollection.RemoveAt(i);
                    }

                    Count--;
                    _structureManager.DataChanged();

                    return true;
                }
            }
            //If there is not value in this distributed array
            return false;
        }
        /// <summary>
        /// Removes the element at the specified index of the DistributedArray(T).
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        public void RemoveAt(int index)
        {
            //Validity of index check in BlockInformation
            var blockInfo = _structureManager.BlockInformation(index);

            //Remove
            _blockCollection[blockInfo.IndexOfBlock].RemoveAt(index - blockInfo.BlockStartIndex);

            //If there is empty block, we will remove it
            if (_blockCollection[blockInfo.IndexOfBlock].Count == 0)
            {
                _blockCollection.RemoveAt(blockInfo.IndexOfBlock);
            }

            Count--;
            _structureManager.DataChanged();
        }
        /// <summary>
        /// Removes a range of elements from the DistributedArray(T).
        /// </summary>
        /// <param name="index">The zero-based starting index of the range of elements to remove.</param>
        /// <param name="count">The number of elements to remove.</param>
        public void RemoveRange(int index, int count)
        {
            var range = _structureManager.MultyblockRange(index, count);
            int shift = 0;
            for (int i = range.IndexOfStartBlock; i < range.IndexOfStartBlock + range.Count; i++)
            {
                var blockRange = range[i - range.IndexOfStartBlock];
                var block = _blockCollection[i - shift];

                if (blockRange.Count != 0)
                {
                    if (block.Count == blockRange.Count)
                    {
                        _blockCollection.RemoveAt(i - shift);
                        shift++;
                    }
                    else
                    {
                        block.RemoveRange(blockRange.Subindex, blockRange.Count);
                    }
                }
            }

            _structureManager.DataChanged();
            Count -= count;
        }
        /// <summary>
        /// Reverses the order of the elements in the entire DistributedArray(T).
        /// </summary>
        public void Reverse()
        {
            foreach (var block in _blockCollection)
            {
                block.Reverse();
            }
            _blockCollection.Reverse();
        }
        /// <summary>
        /// Copies the elements of the DistributedArray(T) to a new array.
        /// </summary>
        /// <returns>An array containing copies of the elements of the DistributedArray(T).</returns>
        public T[] ToArray()
        {
            var array = new T[Count];
            int count = 0;
            //Transfer data
            foreach (var item in this)
            {
                array[count++] = item;
            }
            return array;
        }
        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set. </param>
        public T this[int index]
        {
            get
            {
                //Check for exceptions in BlockInformation()
                var blockInfo = _structureManager.BlockInformation(index);
                return _blockCollection[blockInfo.IndexOfBlock][index - blockInfo.BlockStartIndex];
            }
            set
            {
                //Check for exceptions in BlockInformation()
                var blockInfo = _structureManager.BlockInformation(index);
                _blockCollection[blockInfo.IndexOfBlock][index - blockInfo.BlockStartIndex] = value;
            }
        }
        /// <summary>
        /// Get the number of elements actually contained in the DistributedArray(T).
        /// </summary>
        public int Count
        {
            get
            {
                return _count;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", "Count cant be less then 0!");
                }
                _count = value;
            }
        }
        /// <summary>
        /// Default size of one DistributedArray(T) block. 
        /// Because of the way memory allocation is most effective that it is a power of 2.
        /// </summary>
        public int DefaultBlockSize
        {
            get
            {
                return _blockCollection.DefaultBlockSize;
            }
            set
            {
                _blockCollection.DefaultBlockSize = value;
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
                return _blockCollection.MaxBlockSize;
            }
            set
            {
                _blockCollection.MaxBlockSize = value;
            }
        }
        /// <summary>
        /// Gets a value indicating whether the DistributedArray(T) is read-only.
        /// </summary>
        public bool IsReadOnly { get; private set; }

        //Data
        private readonly StructureManager<T> _structureManager; 
        /// <summary>
        /// The blocks object provides API for easy work with blocks.
        /// </summary>
        private readonly BlockCollection<T> _blockCollection;
        /// <summary>
        /// It is main data container where we save information.
        /// It is cant be null. There is always one block even it is empty.
        /// </summary>
        private int _count;

        //Debug functions
#if DEBUG
        public void Display()
        {
            foreach (var item in this)
            {
                Console.WriteLine(item.ToString());
            }
        }
        public void DisplayByBlocks()
        {
            foreach (var block in _blockCollection)
            {
                foreach (var item in block)
                {
                    Console.WriteLine(item.ToString());
                }
                Console.WriteLine();
            }
        }
#endif
    }
}