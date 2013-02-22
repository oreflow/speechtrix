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
        public short bxs;
        public short bys;
        private short _state;
        public short state
        {
            set
            {
                _state = value;
                bxs = Blocks.sizes[0][nr, _state];
                bys = Blocks.sizes[1][nr, _state];
            }
            get
            {
                return _state;
            }
        }
        public Color color {
            get { return bl.color; }
            set { }
        }
        public bool movable { get; set; }


        static Blocks blocks = new Blocks();
        Block bl = blocks.getBlock(0);

        public bool[,] getRotation()
        {
            return Blocks.getRotations(nr, state);
        }
    }
}
