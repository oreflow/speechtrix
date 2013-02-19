using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Speechtrix.DataModel
{
    class Block
    {
        //klass Block: short ID, bool[4][4][4], för varje block 4 rotationer med 16 rutor
        short ID; //from 0-6
        bool[][][] rotations; //each 4 rotations for a block


    }
}
