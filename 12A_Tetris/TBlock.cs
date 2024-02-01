using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _12A_Tetris
{
    public class TBlock : Block
    {
        private Position[][] tiles = new Position[][]
        {
            new Position[]
            {
                new Position(0, 1),
                new Position(1, 0),
                new Position(1, 1),
                new Position(1, 2)
            },
            new Position[]
            {
                new Position(0, 1),
                new Position(1, 2),
                new Position(1, 1),
                new Position(2, 1)
            },
            new Position[]
            {
                new Position(2, 1),
                new Position(1, 0),
                new Position(1, 1),
                new Position(1, 2)
            },
            new Position[]
            {
                new Position(0, 1),
                new Position(1, 1),
                new Position(2, 1),
                new Position(1, 0)
            }
        };

        public override int Id
        {
            get { return 1; }
        }
        protected override Position StartOffset
        {
            get { return new Position(-1, 3); }
        }
        protected override Position[][] Tiles
        {
            get { return tiles; }
        }
    }
}
