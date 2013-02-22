using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Speechtrix
{
	class Blocks
	{
        public static Block[] allblocks = new Block[7] {new Block(0), new Block(1),
                new Block(2), new Block(3), new Block(4), new Block(5), new Block(6)};
        
        public static short[][,] sizes = new short[2][,]
        {
            new short[7,4]{{2,3,2,3},{1,4,1,4},{2,2,2,2},{2,3,2,3},{2,3,2,3},{2,3,2,3},{2,3,2,3}},
            new short[7,4]{{3,2,3,2},{4,1,4,1},{2,2,2,2},{3,2,3,2},{3,2,3,2},{3,2,3,2},{3,2,3,2}}
        };


        public Block getBlock(int nr)
        {
            return allblocks[nr];
        }

		public static bool[,] getRotations(short blockID, short rotation)
		{
			return allblocks[blockID].getRot(rotation);
		}
	}
}
