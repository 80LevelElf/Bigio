﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bigio.BigArray;
using Bigio.BigArray.Interfaces;
using Bigio.BigArray.JITO;
using Bigio.BigArray.JITO.Operations.Contains;
using Bigio.BigArray.Support_Classes.ArrayMap;
using Bigio.BigArray.Support_Classes.BlockCollection;
using Bigio.Common.Classes;
using Bigio.Common.Managers;

namespace Bigio
{
    /// <summary>
    /// <see cref="BigArray"/> provides collection of elements of T type, with indices of the items ranging from 0 to one less
    /// than the count of items in the collection. <see cref="BigArray"/> consist of blocks. Size of block more or equal 0 and less then
    /// MaxBlockSize. Because of architecture many actions(remove, insert, add) faster than their counterparts in List or LinedList, 
    /// but some operations might slower because of due to the nature of architecture and the overhead. 
    /// It makes no sense to use it with a small number of items(less than 5000-10000), because in that case List and LinkedList
    /// operations will be more efficient.
    /// </summary>
    /// <typeparam name="T">Type of array elements.</typeparam>
    [Serializable]
    public partial class BigArray<T> : IBigList<T>
    {
        //Data

        [NonSerialized]
        private readonly ArrayMap<T> _arrayMap;

        /// <summary>
        /// The blocks object provides API for easy work with blocks
        /// </summary>
        [NonSerialized]
        private readonly BlockCollection<T> _blockCollection;

        /// <summary>
        /// Balancer for determination of blocks size
        /// </summary>
        [NonSerialized]
        private readonly IBalancer _balancer;

        /// <summary>
        /// It is main data container where we save information.
        /// </summary>
        [NonSerialized]
        private int _count;

	    private readonly ContainsOperation<T> _containsOperation;

        //API

        /// <summary>
        /// Create new empty instance of <see cref="BigArray{T}"/> with default configuration.
        /// </summary>
        public BigArray() : this(BigArrayConfiguration<T>.DefaultConfiguration)
        {
            
        }

        /// <summary>
        /// Create new empty instance of <see cref="BigArray{T}"/> with specified <paramref name="configuration"/>.
        /// </summary>
        /// <param name="configuration">Configation object of new array.</param>
        public BigArray(BigArrayConfiguration<T> configuration)
        {
            _balancer = configuration.Balancer;
            _blockCollection = new BlockCollection<T>(_balancer, configuration.BlockCollection);
            _arrayMap = new ArrayMap<T>(_balancer, _blockCollection);

	        _containsOperation = JITOMethodFactory.GetContainsOperation(this, configuration.UseJustInTimeOptimization);
        }

        /// <summary>
        /// Create new empty instance of <see cref="BigArray{T}"/> with default configuration and fill it by elements
        /// from <paramref name="collection"/>.
        /// </summary>
        /// <param name="collection">Copy all elements from this collection to the new <see cref="BigArray{T}"/></param>
        public BigArray(ICollection<T> collection) : this(BigArrayConfiguration<T>.DefaultConfiguration, collection)
        {
            
        }

        /// <summary>
        /// Create new instance of <see cref="BigArray{T}"/> with specified <paramref name="configuration"/> and fill it by elements
        /// from <paramref name="collection"/>
        /// </summary>
        /// <param name="configuration">Configation object of new array.</param>
        /// <param name="collection">Copy all elements from this collection to the new <see cref="BigArray{T}"/></param>
        public BigArray(BigArrayConfiguration<T> configuration, ICollection<T> collection) : this(configuration)
        {
            AddRange(collection);
        }

		/// <summary>
		/// Add an object to the end of last block of the <see cref="BigArray{T}"/>.
		/// </summary>
		public void Add(T value)
        {
            int indexOfBlock = _blockCollection.Count - 1;
            if (_blockCollection.Count == 0 || _blockCollection[indexOfBlock].Count >= _balancer.GetMaxBlockSize(indexOfBlock))
            {
                indexOfBlock++;
                _blockCollection.AddNewBlock();
            }

            _blockCollection[indexOfBlock].Add(value);
            _arrayMap.DataChanged(indexOfBlock);

            Count++;
        }

        /// <summary>
        /// Adds the elements of the specified collection to the end of <see cref="BigArray{T}"/>.
        /// </summary>
        /// <param name="collection">The collection whose elements should be added to the end of the <see cref="BigArray{T}"/>.
        ///  The collection it self can't benull, but it can contain elements that are null, if type T is a reference type. </param>
        public void AddRange(ICollection<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            if (collection.Count == 0)
                return;

            _blockCollection.AddFirstBlockIfThereIsNeeded();
            int lastBlockIndex = _blockCollection.Count - 1;
            var lastBlock = _blockCollection[lastBlockIndex];

            //Transfer data to the last block while it is possible
            var sizeOfTransferToLastBlock = 0;
            var emptySize = _balancer.GetMaxBlockSize(lastBlockIndex) - lastBlock.Count;
            
            if (emptySize != 0)
            {
                sizeOfTransferToLastBlock = Math.Min(emptySize, collection.Count);
                var enumerator = collection.GetEnumerator();

                for (int i = 0; i < sizeOfTransferToLastBlock; i++)
                {
                    enumerator.MoveNext();
                    lastBlock.Add(enumerator.Current);
                }
            }

            //Transfer other data as new blocks
            if (sizeOfTransferToLastBlock != collection.Count)
                _blockCollection.Add(collection, sizeOfTransferToLastBlock);

            Count += collection.Count;
            _arrayMap.DataChanged(lastBlockIndex);
        }

        /// <summary>
        /// Returns a read-only wrapper based on current <see cref="BigArray{T}"/>.
        /// </summary>
        /// <returns>A <see cref="ReadOnlyCollection{T}"/> that acts as a read-only wrapper around the current <see cref="BigArray{T}"/>.</returns>
        public ReadOnlyCollection<T> AsReadOnly()
        {
            return new ReadOnlyCollection<T>(this);
        }

        /// <summary>
        /// Searches the entire sorted <see cref="BigArray{T}"/> for an element using the default comparer and returns the zero-based index of the element.
        /// </summary>
        /// <param name="item">The object to locate. The value can be null for reference types.</param>
        /// <returns>The zero-based index of item in the sorted <see cref="BigArray{T}"/>, if item is found; otherwise,
        ///  a negative number that is the bitwise complement of the index of the next element that is larger than item or,
        ///  if there is no larger element, the bitwise complement of <see cref="Count"/>. </returns>
        public int BinarySearch(T item)
        {
            return BinarySearch(0, Count, item, Comparer<T>.Default);
        }

        /// <summary>
        /// Searches the entire sorted <see cref="BigArray{T}"/> for an element using the specified comparer and returns the zero-based index of the element.
        /// </summary>
        /// <param name="item">The object to locate. The value can be null for reference types.</param>
        /// <param name="comparer">The IComparer{T} implementation to use when comparing elements.
        /// or null to use the default comparer Comparer{T}.Default.
        ///</param>
        /// <returns>The zero-based index of item in the sorted <see cref="BigArray{T}"/>, if item is found; otherwise,
        ///  a negative number that is the bitwise complement of the index of the next element that is larger than item or,
        ///  if there is no larger element, the bitwise complement of Count. </returns>
        public int BinarySearch(T item, IComparer<T> comparer)
        {
            return BinarySearch(0, Count, item, comparer);
        }

        /// <summary>
        /// Searches a range of elements in the sorted <see cref="BigArray{T}"/> for an element using the specified comparer and returns the zero-based index of the element.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="item">The object to locate. The value can be null for reference types.</param>
        /// <param name="comparer">The IComparer{T} implementation to use when comparing elements,
        /// or null to use the default comparer Comparer{T}.Default.</param>
        /// <returns>The zero-based index of item in the sorted <see cref="BigArray{T}"/>, ifitem is found; otherwise,
        ///  a negative number that is the bitwise complement of the index of the next element that is larger than item or,
        ///  if there is no larger element, the bitwise complement of <see cref="Count"/>.</returns>
        public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
        {
            if (Count == 0 && index == 0 && count == 0)
                return ~0;

            if (!this.IsValidRange(index, count))
                throw new ArgumentOutOfRangeException();

            if (comparer == null)
                comparer = Comparer<T>.Default;

            if (index == 0 && count == 0)
                return ~0;

            int startIndex = index;
            int endIndex = index + count;

            while (startIndex <= endIndex)
            {
                int middlePosition = (startIndex + endIndex) / 2;
                T middleValue = this[middlePosition];

                //Compare
                int compareResult = comparer.Compare(item, middleValue);
				if (compareResult < 0)
					endIndex = middlePosition - 1;
				else if(compareResult > 0)
					startIndex = middlePosition + 1;
				else // = 
					return middlePosition;
            }
            //If there is no such item specify the location where the element should be
            if (endIndex == -1) // if we need first element
                return -1;

            //Because there is no such item, we will find plae for it
            var enumerator = GetEnumerator();
            ((BigArrayEnumerator)enumerator).MoveToIndex(endIndex); // Move to start position
            var counter = 0;
            while (endIndex + counter != index + count && comparer.Compare(enumerator.Current, item) <= 0)
            {
                enumerator.MoveNext();
                counter++;
            }
            return ~(endIndex + counter);
        }

        /// <summary>
        /// Removes all elements from the <see cref="BigArray{T}"/>.
        /// </summary>
        public void Clear()
        {
            _blockCollection.Clear();
            Count = 0;
        }

        /// <summary>
        /// Returns true if <see cref="BigArray{T}"/> contains value, otherwise return false.
        /// </summary>
        /// <param name="item">Data to be checked.</param>
        public bool Contains(T item)
        {
            //return IndexOf(item) != -1;
	        return _containsOperation.Contains(item);
        }

        /// <summary>
        /// Converts the elements in the current <see cref="BigArray{T}"/> to another type, and returns a list containing the converted elements.
        /// </summary>
        /// <typeparam name="TOutput">The type of the elements of the target array.</typeparam>
        /// <param name="converter">A Converter{TInput, TOutput} delegate that converts each element from one type to another type.</param>
        /// <returns>A <see cref="BigArray{T}"/> of the target type containing the converted elements from the current <see cref="BigArray{T}"/>.</returns>
        public BigArray<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
        {
            if (converter == null)
                throw new ArgumentNullException("converter");

            var result = new BigArray<TOutput>();

            //Convert all blocks
            foreach (var block in _blockCollection)
                result._blockCollection.Add(block.ConvertAll(converter));

            return result;
        }

        /// <summary>
        /// Copies the entire <see cref="BigArray{T}"/> to a compatible one-dimensional array, starting at the beginning of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional Array that is the destination of the elements copied from <see cref="BigArray{T}"/>.
        ///  The Array must have zero-based indexing.</param>
        public void CopyTo(T[] array)
        {
            if (array == null)
                throw new ArgumentNullException();

            CopyTo(0, array, 0, array.Length);
        }

        /// <summary>
        /// Copies the entire <see cref="BigArray{T}"/> to a compatible one-dimensional array
        /// , starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional array that is the destination of the elements copied from <see cref="BigArray{T}"/>.
        ///  The Array must have zero-based indexing. </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            CopyTo(0, array, arrayIndex, Count);
        }

        /// <summary>
        /// Copies a range of elements from the <see cref="BigArray{T}"/> to a compatible one-dimensional array,
        /// starting at the specified index of the target array.
        /// </summary>
        /// <param name="index">The zero-based index in the source <see cref="BigArray{T}"/> at which copying begins.</param>
        /// <param name="array">The one-dimensional Array that is the destination of the elements copied from <see cref="BigArray{T}"/>. 
        /// The Array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <param name="count">The number of elements to copy.</param>
        public void CopyTo(int index, T[] array, int arrayIndex, int count)
        {
            //If there is empty distributed array
            if (index == 0 && Count == 0)
                return;

            if (!this.IsValidIndex(index) || !array.IsValidRange(arrayIndex, count))
                throw new ArgumentOutOfRangeException();

            if (array == null)
                throw new ArgumentNullException("array");

            var enumerator = GetEnumerator();
            ((BigArrayEnumerator)enumerator).MoveToIndex(index);

            //Transfer data
            for (int i = arrayIndex; i < arrayIndex + count; i++)
            {
                array[i] = enumerator.Current;

                if (!enumerator.MoveNext())
                    break;
            }
        }

        /// <summary>
        /// Determines whether the <see cref="BigArray{T}"/> contains elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The Predicate{T} delegate that defines the conditions of the elements to search for.</param>
        /// <returns>True if the <see cref="BigArray{T}"/> contains one or more elements that match the conditions defined by the specified predicate; otherwise false.</returns>
        public bool Exists(Predicate<T> match)
        {
            //This value was approximately estimated by practical way
            const int borderCountToSwitchMode = 10000000;

            if (Count < borderCountToSwitchMode)
                return _blockCollection.Any(block => block.Exists(match));
            else
                return _blockCollection.AsParallel().Any(block => block.Exists(match));
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate,
        /// and returns the first occurrence within the entire <see cref="BigArray{T}"/>.
        /// </summary>
        /// <param name="match">The Predicate{T} delegate that defines the conditions of the element to search for.</param>
        /// <returns>The first element that matches the conditions defined by the specified predicate, if found;
        /// otherwise, the default value for type T. </returns>
        public T Find(Predicate<T> match)
        {
            if (match == null)
                throw new ArgumentNullException("match");

	        foreach (var block in _blockCollection)
	        {
		        int index = block.FindIndex(match);
		        if (index != -1)
			        return block[index];
	        }

	        return default(T);

	        //return this.FirstOrDefault(i => match(i));
        }

        /// <summary>
        /// Retrieves all the elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The Predicate{T} delegate that defines the conditions of the elements to search for.</param>
        /// <param name="isSaveOrder">If flag is true - return suitable elements in right order, otherwise - order doesn't matter.</param>
        /// <returns>A <see cref="BigArray{T}"/> containing all the elements that match the conditions defined by the specified predicate,
        /// if found; otherwise, an empty <see cref="BigArray{T}"/>.</returns>
        public BigArray<T> FindAll(Predicate<T> match, bool isSaveOrder = false)
        {
            if (match == null)
                throw new ArgumentNullException("match");

            var resultArray = new BigArray<T>();

            if (isSaveOrder)
            {
                foreach (var block in _blockCollection)
                {
                    var findData = block.FindAll(match);

                    if (findData.Count != 0)
                        resultArray.AddRange(findData);
                }
            }
            else
            {
                object locker = new object();

                Parallel.ForEach(_blockCollection, block =>
                {
                    var findData = block.FindAll(match);

                    lock (locker)
                    {
                        if (findData.Count != 0)
                            resultArray.AddRange(findData);
                    }
                });
            }

            return resultArray;
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate,
        /// and returns the zero-based index of the first occurrence within the entire <see cref="BigArray{T}"/>.
        /// </summary>
        /// <param name="match">The Predicate{T} delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by match, if found; otherwise, –1.</returns>
        public int FindIndex(Predicate<T> match)
        {
	        return FindIndex(0, Count, match);
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate,
        /// and returns the zero-based index of the first occurrence within the range of elements in the <see cref="BigArray{T}"/>
        /// that extends from the specified index to the last element.
        /// </summary>
        /// <param name="index">The zero-based starting index of the search.</param>
        /// <param name="match">The Predicate{T} delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by match, if found; otherwise, –1. </returns>
        public int FindIndex(int index, Predicate<T> match)
        {
            return FindIndex(index, Count - index, match);
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate,
        ///  and returns the zero-based index of the first occurrence within the range of elements in the <see cref="BigArray{T}"/>
        ///  that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="index">The zero-based starting index of the search.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The Predicate(T) delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by match,
        ///  if found; otherwise, –1. </returns>
        public int FindIndex(int index, int count, Predicate<T> match)
        {
            if (match == null)
                throw new ArgumentNullException("match");

            var multyblockRange = _arrayMap.MultyblockRange(new Range(index, count));

            int indexOfBlock = multyblockRange.IndexOfStartBlock;
            foreach (var blockRange in multyblockRange.Ranges)
            {
                int findIndexResult = _blockCollection[indexOfBlock++]
                    .FindIndex(blockRange.Subindex, blockRange.Count, match);

                if (findIndexResult != -1)
                    return blockRange.CommonStartIndex + findIndexResult;
            }

            //If there is no needed value
            return -1;
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate,
        ///  and returns the last occurrence within the entire <see cref="BigArray{T}"/>.
        /// </summary>
        /// <param name="match">The Predicate{T} delegate that defines the conditions of the element to search for.</param>
        /// <returns>The last element that matches the conditions defined by the specified predicate, if found; otherwise, the default value for type T.</returns>
        public T FindLast(Predicate<T> match)
        {
            var range = _arrayMap.ReverseMultyblockRange(new Range(Count - 1, Count));

            //Find it
            int indexOfBlock = range.IndexOfStartBlock;
            foreach (var blockRange in range.Ranges)
            {
                int findLastIndexResult = _blockCollection[indexOfBlock--]
                    .FindLastIndex(blockRange.Subindex, blockRange.Count, match);
                if (findLastIndexResult != -1)
                    return this[blockRange.CommonStartIndex + findLastIndexResult];
            }

            //If there is no such item
            return default(T);
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate,
        ///  and returns the zero-based index of the last occurrence within the entire <see cref="BigArray{T}"/>.
        /// </summary>
        /// <param name="match">The Predicate{T} delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the last occurrence of an element that matches the conditions defined by match, if found; otherwise, –1.</returns>
        public int FindLastIndex(Predicate<T> match)
        {
            int index = (Count == 0) ? 0 : Count - 1;
            return FindLastIndex(index, Count, match);
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last 
        /// occurrence within the range of elements in the <see cref="BigArray{T}"/> that extends from the first element to the specified index.
        /// </summary>
        /// <param name="index">The zero-based starting index of the backward search.</param>
        /// <param name="match">The Predicate{T} delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the last occurrence of an element that matches the conditions defined by match, if found; otherwise, –1.</returns>
        public int FindLastIndex(int index, Predicate<T> match)
        {
            return FindLastIndex(index, index + 1, match);
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the 
        /// last occurrence within the range of elements in the <see cref="BigArray{T}"/> that contains the specified number of elements and ends at the specified index.
        /// </summary>
        /// <param name="index">The zero-based starting index of the backward search.</param>
        /// <param name="count">The number of elements to search.</param>
        /// <param name="match">The Predicate{T} delegate that defines the conditions of the element to search for.</param>
        /// <returns></returns>
        public int FindLastIndex(int index, int count, Predicate<T> match)
        {
            if (match == null)
                throw new ArgumentNullException("match");

            var range = _arrayMap.ReverseMultyblockRange(new Range(index, count));

            //Find it
            int indexOfBlock = range.IndexOfStartBlock;
            foreach (var blockRange in range.Ranges)
            {
                int findLastIndexResult = _blockCollection[indexOfBlock--]
                    .FindLastIndex(blockRange.Subindex, blockRange.Count, match);

                if (findLastIndexResult != -1)
                    return blockRange.CommonStartIndex + findLastIndexResult;
            }

            //If there is no needed value
            return -1;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="BigArray{T}"/>.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            return new BigArrayEnumerator(this);
        }

        /// <summary>
        /// Creates a shallow copy of a range of elements in the source <see cref="BigArray{T}"/>.
        /// </summary>
        /// <param name="index">The zero-based <see cref="BigArray{T}"/> index at which the range starts.</param>
        /// <param name="count">The number of elements in the range.</param>
        /// <returns></returns>
        public BigArray<T> GetRange(int index, int count)
        {
            if (!this.IsValidRange(index, count))
                throw new ArgumentOutOfRangeException();

            var newArray = new BigArray<T>();

            if (count == 0)
                return newArray;

            var enumerator = GetEnumerator();
            ((BigArrayEnumerator)enumerator).MoveToIndex(index);
            bool isContinue = true;

            while (isContinue && count != 0)
            {
                newArray.Add(enumerator.Current);

                count--;
                isContinue = enumerator.MoveNext();
            }

            return newArray;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new BigArrayEnumerator(this);
        }

        /// <summary>
        /// If value conatins in <see cref="BigArray{T}"/> returns index of this value, otherwise return -1.
        /// </summary>
        /// <param name="item">Data, the location of which is necessary to calculate</param>
        /// <returns>The zero-based index of the first occurrence of item within the entire <see cref="BigArray{T}"/>, if found; otherwise, –1.</returns>
        public int IndexOf(T item)
        {
            return IndexOf(item, 0, Count);
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first
        /// occurrence within the range of elements in the <see cref="BigArray{T}"/> that extends from the 
        /// specified index to the last element.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="BigArray{T}"/>. The value can benull for reference types.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <returns>The zero-based index of the first occurrence of item within the range of elements 
        /// in the <see cref="BigArray{T}"/> that extends fromindex to the last element, if found; otherwise, –1.</returns>
        public int IndexOf(T item, int index)
        {
            return IndexOf(item, index, Count - index);
        }

        /// <summary>
        ///  Searches for the specified object and returns the zero-based index of the first occurrence
        ///  within the range of elements in the <see cref="BigArray{T}"/> that starts at the specified index
        ///  and contains the specified number of elements.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="BigArray{T}"/>. The value can benull for reference types.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list. </param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <returns>The zero-based index of the first occurrence of item within the range of elements
        ///  in the <see cref="BigArray{T}"/> that starts atindex and contains count number of elements, if found; otherwise, –1. </returns>
        public int IndexOf(T item, int index, int count)
        {
            if (!this.IsValidRange(index, count))
                throw new ArgumentOutOfRangeException();

            var multyblockRange = _arrayMap.MultyblockRange(new Range(index, count));

            int indexOfBlock = multyblockRange.IndexOfStartBlock;
            foreach (var blockRange in multyblockRange.Ranges)
            {
                int indexOfResult = _blockCollection[indexOfBlock++]
                    .IndexOf(item, blockRange.Subindex, blockRange.Count);

                if (indexOfResult != -1)
                    return blockRange.CommonStartIndex + indexOfResult;
            }

            //If there is no needed value
            return -1;
        }

        /// <summary>
        /// Inserts an element into the <see cref="BigArray{T}"/> at the specified index.
        /// </summary>
        /// <param name="index">Index of <see cref="BigArray{T}"/> where the value will be.</param>
        /// <param name="item">The data to be placed.</param>
        public void Insert(int index, T item)
        {
            if (index == Count)
            {
                Add(item);
                return;
            }

            var blockInfo = _arrayMap.BlockInfo(index);

            int blockSubindex = index - blockInfo.CommonStartIndex;
            int indexOfBlock = blockInfo.IndexOfBlock;
            var block = _blockCollection[blockInfo.IndexOfBlock];

            bool isMaxSize = (block.Count == _balancer.GetMaxBlockSize(indexOfBlock));
            bool isNeedToAddPreviosBlock = (blockSubindex == 0 && blockInfo.Count >= _balancer.GetDefaultBlockSize(indexOfBlock));

            if (isMaxSize)
            {
                _blockCollection.TryToDivideBlock(blockInfo.IndexOfBlock);
                _arrayMap.DataChanged(blockInfo.IndexOfBlock);
                Insert(index, item);
                return;
            }

            //Insertion
            if (!isNeedToAddPreviosBlock)
            {
                _blockCollection[blockInfo.IndexOfBlock].Insert(blockSubindex, item);
                _blockCollection.TryToDivideBlock(blockInfo.IndexOfBlock);
                _arrayMap.DataChanged(blockInfo.IndexOfBlock);
            }
            //Try to add to the previous block
            else
            {
                //If there is need - add new block
                bool isStartBlock = (blockInfo.IndexOfBlock == 0);
                bool isPrevBlockFull = false;

                if (!isStartBlock)
                    isPrevBlockFull = (_blockCollection[blockInfo.IndexOfBlock].Count == _balancer.GetMaxBlockSize(indexOfBlock));

                if (isStartBlock || isPrevBlockFull)
                {
                    _blockCollection.InsertNewBlock(blockInfo.IndexOfBlock);

                    blockInfo.IndexOfBlock++;
                }

                _blockCollection[blockInfo.IndexOfBlock - 1].Add(item);
                _arrayMap.DataChanged(blockInfo.IndexOfBlock - 1);
            }

            Count++;
        }

        /// <summary>
        /// Inserts the elements of a collection into the <see cref="BigArray{T}"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the new elements should be inserted. </param>
        /// <param name="collection">The collection whose elements should be inserted into the <see cref="BigArray{T}"/>.
        ///  The collection it self can't be null, but it can contains elements that are null, if type T is a reference type. </param>
        public void InsertRange(int index, ICollection<T> collection)
        {
            //Validity of index and count check in BlockInfo
            var blockInfo = new BlockInfo();

            if (index == 0 && Count == 0) // Empty array
            {
                _blockCollection.AddFirstBlockIfThereIsNeeded();
                blockInfo.IndexOfBlock = 0;
                blockInfo.CommonStartIndex = 0;
            }
            else if (index == Count) // Last position
            {
                blockInfo.IndexOfBlock = _blockCollection.Count - 1;
                blockInfo.CommonStartIndex = Count - _blockCollection[blockInfo.IndexOfBlock].Count;
            }
            else // Default case
            {
                blockInfo = _arrayMap.BlockInfo(index);
            }

            //Insert
            _blockCollection[blockInfo.IndexOfBlock].InsertRange(
                index - blockInfo.CommonStartIndex, collection);
            _blockCollection.TryToDivideBlock(blockInfo.IndexOfBlock);

            Count += collection.Count;
            _arrayMap.DataChanged(blockInfo.IndexOfBlock);
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the last occurrence within the entire <see cref="BigArray{T}"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="BigArray{T}"/>. The value can benull for reference types.</param>
        /// <returns>The zero-based index of the last occurrence of item within the entire the <see cref="BigArray{T}"/>, if found; otherwise, –1.</returns>
        public int LastIndexOf(T item)
        {
            int index = (Count == 0) ? 0 : Count - 1;
            return LastIndexOf(item, index, Count);
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the last occurrence
        /// within the range of elements in the <see cref="BigArray{T}"/> that extends from the first element to the specified index.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="BigArray{T}"/>. The value can benull for reference types.</param>
        /// <param name="index">The zero-based starting index of the backward search.</param>
        /// <returns>The zero-based index of the last occurrence of item within the range of 
        /// elements in the <see cref="BigArray{T}"/> that extends from the first element toindex, if found; otherwise, –1. </returns>
        public int LastIndexOf(T item, int index)
        {
            return LastIndexOf(item, index, index + 1);
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index
        ///  of the last occurrence within the range of elements in the <see cref="BigArray{T}"/> that contains the specified 
        /// number of elements and ends at the specified index.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="BigArray{T}"/>. The value can benull for reference types.</param>
        /// <param name="index">The zero-based starting index of the backward search. </param>
        /// <param name="count">The number of elements in the section to search. </param>
        /// <returns>The zero-based index of the last occurrence of item within the range of elements in the <see cref="BigArray{T}"/>
        ///  that containscount number of elements and ends at index, if found; otherwise, –1. </returns>
        public int LastIndexOf(T item, int index, int count)
        {
            var reverseMultyblockRange = _arrayMap.ReverseMultyblockRange(new Range(index, count));

            //Find it
            int indexOfBlock = reverseMultyblockRange.IndexOfStartBlock;
            foreach (var blockRange in reverseMultyblockRange.Ranges)
            {
                int lastIndexOfResult = _blockCollection[indexOfBlock--]
                    .LastIndexOf(item, blockRange.Subindex, blockRange.Count);
                if (lastIndexOfResult != -1)
                {
                    return blockRange.CommonStartIndex + lastIndexOfResult;
                }
            }

            //If there is no needed value
            return -1;
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="BigArray{T}"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="BigArray{T}"/>.
        /// The value can be null for reference types.</param>
        /// <returns>True if item is successfully removed; otherwise, false.
        /// This method also returns false if item was not found in the <see cref="BigArray{T}"/>.</returns>
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
                        _arrayMap.DataChangedAfterBlockRemoving(i);
                    }
                    else
                    {
                        _arrayMap.DataChanged(i); 
                    }

                    Count--;
                    return true;
                }
            }

            //If there is not value in this distributed array
            return false;
        }

        /// <summary>
        /// Removes the element at the specified index of the <see cref="BigArray{T}"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        public void RemoveAt(int index)
        {
            if (index == Count - 1)
            {
                RemoveLast();
                return;
            }

            //Validity of index check in BlockInfo
            var blockInfo = _arrayMap.BlockInfo(index);

            //Remove
            _blockCollection[blockInfo.IndexOfBlock].RemoveAt(index - blockInfo.CommonStartIndex);

            //If there is empty block, we will remove it
            if (_blockCollection[blockInfo.IndexOfBlock].Count == 0)
            {
                _blockCollection.RemoveAt(blockInfo.IndexOfBlock);
                _arrayMap.DataChangedAfterBlockRemoving(blockInfo.IndexOfBlock);
            }
            else
            {
                _arrayMap.DataChanged(blockInfo.IndexOfBlock);
            }

            Count--;
        }

        /// <summary>
        /// Remove last element of array.
        /// </summary>
        public void RemoveLast()
        {
            if (Count == 0)
                throw new InvalidOperationException("Can't remove element from empty collectioin!");

            int indexOfLastBlock = _blockCollection.Count - 1;
            var lastBlock = _blockCollection[indexOfLastBlock];

            lastBlock.RemoveAt(lastBlock.Count - 1);

            if (lastBlock.Count == 0)
            {
                _blockCollection.RemoveAt(indexOfLastBlock);
                _arrayMap.DataChangedAfterBlockRemoving(indexOfLastBlock);
            }
            else
            {
                _arrayMap.DataChanged(indexOfLastBlock);
            }

            Count--;
        }

        /// <summary>
        /// Removes a range of elements from the <see cref="BigArray{T}"/>.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range of elements to remove.</param>
        /// <param name="count">The number of elements to remove.</param>
        public void RemoveRange(int index, int count)
        {
            var multyblockRange = _arrayMap.MultyblockRange(new Range(index, count));

            int shift = 0;
            int indexOfBlock = multyblockRange.IndexOfStartBlock;

            // There is need to get all ranges before we start to remove,
            // otherwise we get invalid data.
            var blockRanges = new BlockRange[multyblockRange.Count];
            int i = 0;
            foreach (var blockRange in multyblockRange.Ranges)
            {
                blockRanges[i++] = blockRange;
            }

            //Calc
            foreach (var blockRange in blockRanges)
            {
                var block = _blockCollection[indexOfBlock - shift];

                if (blockRange.Count != 0)
                {
                    if (block.Count == blockRange.Count)
                    {
                        _blockCollection.RemoveAt(indexOfBlock - shift);
                        shift++;
                    }
                    else
                    {
                        block.RemoveRange(blockRange.Subindex, blockRange.Count);
                    }
                }

                indexOfBlock++;
            }

            _arrayMap.DataChangedAfterBlockRemoving(multyblockRange.IndexOfStartBlock);
            Count -= count;
        }

        /// <summary>
        /// Reverses the order of the elements in the entire <see cref="BigArray{T}"/>.
        /// </summary>
        public void Reverse()
        {
            Parallel.ForEach(_blockCollection, block =>
            {
                block.Reverse();
            });

            _blockCollection.Reverse();
        }

        /// <summary>
        /// Copies the elements of the <see cref="BigArray{T}"/> to a new array.
        /// </summary>
        /// <returns>An array containing copies of the elements of the <see cref="BigArray{T}"/>.</returns>
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
        /// <param name="index">The zero-based index of the element to get or set.</param>
        public T this[int index]
        {
            get
            {
                //Check for exceptions in BlockInfo()
                var blockInfo = _arrayMap.BlockInfo(index);
                return _blockCollection[blockInfo.IndexOfBlock][index - blockInfo.CommonStartIndex];
            }
            set
            {
                //Check for exceptions in BlockInfo()
                var blockInfo = _arrayMap.BlockInfo(index);
                _blockCollection[blockInfo.IndexOfBlock][index - blockInfo.CommonStartIndex] = value;
            }
        }

        /// <summary>
        /// Get the number of elements actually contained in the <see cref="BigArray{T}"/>.
        /// </summary>
        public int Count
        {
            get
            {
                return _count;
            }
            private set
            {
                Debug.Assert(value >= 0);
                _count = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="BigArray{T}"/> is read-only.
        /// </summary>
        public bool IsReadOnly { get; private set; }

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