using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Speechtrix
{
    public class Block
    {
		public short id { get; set; } //from 0-6
		private bool[][,] rotations; //each 4 rotations for a block

        public Block(short pId)
        {
            if (pId >= 0 && pId <= 6)
                id = pId;
            else
                id = 0;

            initRotations();
        }

		public bool[,] getRot(short rot)
		{
			return rotations[rot];
	
		}
        void initRotations()
        {
            switch (id)
            {
                case 0:
                    rotations = new bool[4][,]{ //0
			            new bool[4,4]{{true,true,true,false},{true,false,false,false},{false,false,false,false},{false,false,false,false}},
			            new bool[4,4]{{true,false,false,false},{true,false,false,false},{true,true,false,false},{false,false,false,false}},
			            new bool[4,4]{{false,false,true,false},{true,true,true,false},{false,false,false,false},{false,false,false,false}},
			            new bool[4,4]{{true,true,false,false},{false,true,false,false},{false,true,false,false},{false,false,false,false}}};
                    break;
                case 1:
                    rotations = new bool[4][,]{//1
			            new bool[4,4]{{true,true,true,true},{false,false,false,false},{false,false,false,false},{false,false,false,false}},
			            new bool[4,4]{{true,false,false,false},{true,false,false,false},{true,false,false,false},{true,false,false,false}},
			            new bool[4,4]{{true,true,true,true},{false,false,false,false},{false,false,false,false},{false,false,false,false}},
			            new bool[4,4]{{true,false,false,false},{true,false,false,false},{true,false,false,false},{true,false,false,false}}};
                    break;
                case 2:
                    rotations = new bool[4][,]{//2
                        new bool[4,4]{{true,true,false,false},{true,true,false,false},{false,false,false,false},{false,false,false,false}},
                        new bool[4,4]{{true,true,false,false},{true,true,false,false},{false,false,false,false},{false,false,false,false}},
                        new bool[4,4]{{true,true,false,false},{true,true,false,false},{false,false,false,false},{false,false,false,false}},
                        new bool[4,4]{{true,true,false,false},{true,true,false,false},{false,false,false,false},{false,false,false,false}}};
                    break;
                case 3:
                    rotations = new bool[4][,]{//3
			            new bool[4,4]{{true,true,false,false},{false,true,true,false},{false,false,false,false},{false,false,false,false}},
			            new bool[4,4]{{false,true,false,false},{true,true,false,false},{true,false,false,false},{false,false,false,false}},
			            new bool[4,4]{{true,true,false,false},{false,true,true,false},{false,false,false,false},{false,false,false,false}},
			            new bool[4,4]{{false,true,false,false},{true,true,false,false},{true,false,false,false},{false,false,false,false}}};
                    break;
                case 4:
                    rotations = new bool[4][,]{//4
			            new bool[4,4]{{false,true,true,false},{true,true,false,false},{false,false,false,false},{false,false,false,false}},
			            new bool[4,4]{{true,false,false,false},{true,true,false,false},{false,true,false,false},{false,false,false,false}},
			            new bool[4,4]{{false,true,true,false},{true,true,false,false},{false,false,false,false},{false,false,false,false}},
			            new bool[4,4]{{true,false,false,false},{true,true,false,false},{false,true,false,false},{false,false,false,false}}};
                    break;
                case 5:
                    rotations = new bool[4][,]{//5
			            new bool[4,4]{{false,true,false,false},{true,true,true,false},{false,false,false,false},{false,false,false,false}},
			            new bool[4,4]{{false,true,false,false},{true,true,false,false},{false,true,false,false},{false,false,false,false}},
			            new bool[4,4]{{true,true,true,false},{false,true,false,false},{false,false,false,false},{false,false,false,false}},
			            new bool[4,4]{{true,false,false,false},{true,true,false,false},{true,false,false,false},{false,false,false,false}}};
                    break;
                case 6:
                    rotations = new bool[4][,]{//6
			            new bool[4,4]{{true,false,false,false},{true,true,true,false},{false,false,false,false},{false,false,false,false}},
			            new bool[4,4]{{false,true,false,false},{false,true,false,false},{true,true,false,false},{false,false,false,false}},
			            new bool[4,4]{{true,true,true,false},{false,false,true,false},{false,false,false,false},{false,false,false,false}},
			            new bool[4,4]{{true,true,false,false},{true,false,false,false},{true,false,false,false},{false,false,false,false}}};
                    break;
                default:
                    break;
            }           
        }
    }
}