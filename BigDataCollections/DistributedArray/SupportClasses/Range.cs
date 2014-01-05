using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BigDataCollections.DistributedArray.SupportClasses
{
    struct Range
    {
        //API
        public Range(int index, int count)
        {
            Index = index;
            Count = count;
        }
        public int Index;
        public int Count;
    }
}
