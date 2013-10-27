using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

namespace BigDataCollections.DistributedArray.SupportClasses
{
    struct BlockInformation
    {
        public BlockInformation(int indexOfBlock, int blockStartIndex)
        {
            IndexOfBlock = indexOfBlock;
            BlockStartIndex = blockStartIndex;
        }
        public int IndexOfBlock;
        public int BlockStartIndex;
    }
}
