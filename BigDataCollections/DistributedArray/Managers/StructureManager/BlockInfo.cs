using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BigDataCollections.DistributedArray.Managers.StructureManager
{
    struct BlockInfo
    {
        public int Compare(int index)
        {
            if (index < StartIndex)
            {
                return -1;
            }
            if (index >= StartIndex + Count)
            {
                return 1;
            }
            return 0;
        }
        public int IndexOfBlock;
        public int StartIndex;
        public int Count;
    }
}
