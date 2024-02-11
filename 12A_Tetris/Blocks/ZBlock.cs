using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _12A_Tetris.Blocks
{
    public class ZBlock : Block
    {
        private Position[][] tiles = new Position[][]
        {
            new Position[]
            {
                new Position(0, 0),
                new Position(0, 1),
                new Position(1, 1),
                new Position(1, 2)
            },
            new Position[]
            {
                new Position(0, 2),
                new Position(1, 1),
                new Position(1, 2),
                new Position(2, 1)
            },
            new Position[]
            {
                new Position(1, 0),
                new Position(1, 1),
                new Position(2, 1),
                new Position(2, 2)
            },
            new Position[]
            {
                new Position(0, 1),
                new Position(1, 0),
                new Position(1, 1),
                new Position(2, 0)
            }
        };

        public override int Id
        {
            get { return 7; }
        }
        protected override Position StartOffset
        {
            get { return new Position(0, 3); }
        }
        protected override Position[][] Tiles
        {
            get { return tiles; }
        }
    }
}
