using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Speechtrix
{
    public class Block
    {
        Color[] cola = new Color[7]
            {Color.FromArgb(226, 0, 127), Color.FromArgb(255, 0, 0), Color.FromArgb(0, 255, 0), 
                Color.FromArgb(12, 0, 247), Color.FromArgb(255, 240, 0), Color.FromArgb(122, 78, 156),
                Color.FromArgb(45, 237, 120)};


		public short id { get; set; } //from 0-6
		private bool[][,] rotations; //each 4 rotations for a block
        public Color color { get; set; }

        public Block(short pId)
        {
            if (pId >= 0 && pId <= 6)
                id = pId;
            else
                id = 0;

            initColor();
            initRotations();
        }

        void initColor()
        {
            color = cola[id];
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