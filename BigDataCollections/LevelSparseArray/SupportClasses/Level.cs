using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BigDataCollections.LevelSparseArray.SupportClasses
{
    class Level<T>
    {
        //API
        public Level(int degree)
        {
            Degree = degree;
            _data = new T[Degree];
        }

        //Data
        public T this[int index]
        {
            get
            {
                return _data[index];
            }
            set
            {
                _data[index] = value;
            }
        }
        public int Degree
        {
            get
            {
                return _degree;
            }
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                _degree = value;
            }
        }
        private int _degree;
        private readonly T[] _data;
    }
}
