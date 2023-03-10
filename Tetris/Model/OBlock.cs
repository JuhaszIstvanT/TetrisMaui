using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Model
{
    public class OBlock : Block
    {
        public override int Id => 4;
        public override Position[][] Tiles => new Position[][]
        {
            new Position[] { new(0,0), new(0,1), new(1,0), new(1,1) },
            new Position[] { new(0,0), new(0,1), new(1,0), new(1,1) },
            new Position[] { new(0,0), new(0,1), new(1,0), new(1,1) },
            new Position[] { new(0,0), new(0,1), new(1,0), new(1,1) }
        };
    }
}
