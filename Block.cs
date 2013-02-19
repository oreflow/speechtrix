using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Speechtrix
{
    class Block
    {
        short id; //from 0-6
        private bool[,,] rotations
        {
            get {return rotations;}
            private set;
        }
             //each 4 rotations for a block

        public Block(short pId)
        {
            if (pId >= 0 && pId <= 6)
                id = pId;
            else
                id = 0;

            initRotations();
        }
        
        void initRotations()
        {
            switch (id)
            {
                case 0:
                    rotations = new bool[4, 4, 4]
                        {{{true,true,false,false},{true,true,false,false},{true,true,false,false},{true,true,false,false}},
                        {{true,true,true,false},{false,false,true,false},{false,false,false,false},{false,false,false,false}},
                        {{false,true,false,false},{false,true,false,false},{true,true,false,false},{false,false,false,false}},
                        {{true,false,false,false},{true,true,true,false},{false,false,false,false},{false,false,false,false}}};
                    break;
                case 1:
                    rotations = new bool[4, 4, 4]
                        {{{true,false,false,false},{true,false,false,false},{true,false,false,false},{true,false,false,false}},
                        {{true,true,true,true},{false,false,false,false},{false,false,false,false},{false,false,false,false}},
                        {{true,false,false,false},{true,false,false,false},{true,false,false,false},{true,false,false,false}},
                        {{true,true,true,true},{false,false,false,false},{false,false,false,false},{false,false,false,false}}};
                    break;
                case 2:
                    rotations = new bool[4, 4, 4]
                        {{{true,true,false,false},{true,true,false,false},{false,false,false,false},{false,false,false,false}},
                        {{true,true,false,false},{true,true,false,false},{false,false,false,false},{false,false,false,false}},
                        {{true,true,false,false},{true,true,false,false},{false,false,false,false},{false,false,false,false}},
                        {{true,true,false,false},{true,true,false,false},{false,false,false,false},{false,false,false,false}}};
                    break;
                case 3:
                    rotations = new bool[4, 4, 4]
                        {{{true,false,false,false},{true,true,false,false},{false,true,false,false},{false,false,false,false}}
                        ,{{false,true,true,false},{true,true,false,false},{false,false,false,false},{false,false,false,false}}
                        ,{{true,false,false,false},{true,true,false,false},{false,true,false,false},{false,false,false,false}}
                        ,{{false,true,true,false},{true,true,false,false},{false,false,false,false},{false,false,false,false}}};
                    break;
                case 4:
                    rotations = new bool[4, 4, 4]
                        {{{false,true,false,false},{true,true,false,false},{true,false,false,false},{false,false,false,false}},
                        {{true,true,false,false},{false,true,true,false},{false,false,false,false},{false,false,false,false}}
                        ,{{false,true,false,false},{true,true,false,false},{true,false,false,false},{false,false,false,false}}
                        ,{{true,true,false,false},{false,true,true,false},{false,false,false,false},{false,false,false,false}}};
                    break;

                case 5:
                    rotations = new bool[4, 4, 4]
                        {{{false,true,false,false},{true,true,false,false},{false,true,false,false},{false,false,false,false}}
                        ,{{false,true,false,false},{true,true,true,false},{false,false,false,false},{false,false,false,false}}
                        ,{{true,false,false,false},{true,true,false,false},{true,false,false,false},{false,false,false,false}}
                        ,{{true,true,true,false},{false,true,false,false},{false,false,false,false},{false,false,false,false}}};
                    break;
                case 6:
                    rotations = new bool[4, 4, 4]
                        {{{true,true,false,false},{false,true,false,false},{false,true,false,false},{false,false,false,false}}
                        ,{{false,false,true,false},{true,true,true,false},{false,false,false,false},{false,false,false,false}}
                        ,{{true,false,false,false},{true,false,false,false},{true,true,false,false},{false,false,false,false}}
                        ,{{true,true,true,false},{true,false,false,false},{false,false,false,false},{false,false,false,false}}};
                    break;
                default:
                    break;
            }           
        }
    }
}