using System;
using Bigio.BigArray.Interfaces;
using Bigio.BigArray.Support_Classes.Balancer;
using Bigio.BigArray.Support_Classes.BlockCollection;

namespace Bigio.BigArray
{
    public class BigArrayConfiguration<T>
    {
        /// <summary>
        /// Object to manage block size logic
        /// </summary>
        public IBalancer Balancer { get; set; }

        /// <summary>
        /// Collection for storage blocks of <see cref="BigArray"/>. You can
        /// defint you own collection for it to controll it. For example you can send BigArray{Block{T}}
        /// and have second level distribution.
        /// </summary>
        public IBigList<Block<T>> BlockCollection { get; set; }

        /// <summary>
        /// Create new instance of <see cref="BigArrayConfiguration{T}"/> with specified <paramref name="balancer"/>
        /// and <paramref name="blockCollection"/>.
        /// </summary>
        /// <param name="balancer">Object to manage block size logic</param>
        /// <param name="blockCollection"> Collection for storage blocks of <see cref="BigArray"/>. You can
        /// defint you own collection for it to controll it. For example you can send BigArray{Block{T}}
        /// and have second level distribution.</param>
        public BigArrayConfiguration(IBalancer balancer, IBigList<Block<T>> blockCollection)
        {
            if (balancer == null)
                throw new ArgumentNullException("balancer", "Configuration must contains balancer!");

            if (blockCollection == null)
                throw new ArgumentNullException("balancer", "Configuration must contains block collection!");

            Balancer = balancer;
            BlockCollection = blockCollection;
        }

        /// <summary>
        /// Create new instance of <see cref="BigArrayConfiguration{T}"/> with specified <paramref name="balancer"/>
        /// and default block collection.
        /// </summary>
        /// <param name="balancer">Object to manage block size logic</param>
        public BigArrayConfiguration(IBalancer balancer) : this(balancer, DefaultConfiguration.BlockCollection)
        {
            
        }

        /// <summary>
        /// Create new instance of <see cref="BigArrayConfiguration{T}"/> with specified <paramref name="blockCollection"/>
        /// and default balancer.
        /// </summary>
        /// <param name="blockCollection"> Collection for storage blocks of <see cref="BigArray"/>. You can
        /// defint you own collection for it to controll it. For example you can send BigArray{Block{T}}
        /// and have second level distribution.</param>
        public BigArrayConfiguration(IBigList<Block<T>> blockCollection) : this(DefaultConfiguration.Balancer, blockCollection)
        {
            
        }

        /// <summary>
        /// Get configuration with default block collection and balancer.
        /// </summary>
        public static BigArrayConfiguration<T> DefaultConfiguration
        {
            get
            {
                return new BigArrayConfiguration<T>(new FixedBalancer(), new InternalBlockList<Block<T>>());
            }
        }
    }
}