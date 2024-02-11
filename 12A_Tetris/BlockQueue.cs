using _12A_Tetris.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _12A_Tetris
{
    public class BlockQueue
    {
        private Block[] blocks = new Block[]
        {
            new IBlock(),
            new JBlock(),
            new LBlock(),
            new OBlock(),
            new SBlock(),
            new TBlock(),
            new ZBlock()
        };

        private Random random = new Random();

        public Block[] blocksQueue = new Block[3];

        public BlockQueue()
        {
            for (int i = 0; i < 3; i++)
            {
                blocksQueue[i] = NextBlock(i);
            }
        }
        public Block NextBlock(int i)
        {
            Block nextBlock = blocks[random.Next(blocks.Length)];

            if(i != 0)
            {
                while (blocksQueue.Length > 0 && nextBlock.Equals(blocksQueue[i - 1]))
                {
                    nextBlock = blocks[random.Next(blocks.Length)];
                }
            }

            return nextBlock;
        }

        public Block GetNextBlock()
        {
            Block nextBlock = blocksQueue[0];
            for (int i = 0; i < 2; i++)
            {
                blocksQueue[i] = blocksQueue[i + 1];
            }
            blocksQueue[2] = NextBlock(2);
            return nextBlock;
        }
    }
}
