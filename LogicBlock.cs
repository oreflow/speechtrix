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
        public short nr
        {
            set { bl = blocks.getBlock(value); }
            get { return bl.id; }
        }
        public short x { get; set; }
        public short y { get; set; }
        public short bxs { get; set; }
        public short bys { get; set; }
        public short state { get; set; }
        public Color color { get; set; }
        
        static Blocks blocks = new Blocks();
        Block bl = blocks.getBlock(0);

        public bool[,] getRotation()
        {
            return Blocks.getRotations(nr, state);
        }
    }
}
