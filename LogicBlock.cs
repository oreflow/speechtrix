using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Speechtrix
{
    public class LogicBlock
    {
        public short nr, x, y, bxs, bys, rot;
        public Color color;
        public bool[,] blo = new bool[4, 4]; // unnecessary to double store? could just make it to a wrapper

        public LogicBlock()
        {
            blo = new bool[4, 4];
        }
    }
}
