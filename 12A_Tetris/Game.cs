using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _12A_Tetris
{
    public class Game
    {
        private Block currentBlock;

        public Block CurrentBlock
        {
            get { return currentBlock; }
            set 
            { 
                currentBlock = value;
                currentBlock.Reset(); 

                for (int i = 0; i < 2; i++)
                {
                    currentBlock.Move(1, 0);

                    if (!BlockFits())
                    {
                        currentBlock.Move(-1, 0);
                    }
                }
            }
        }

        public GameGrid Grid { get; }
        public BlockQueue BlockQueue { get; }
        public int GameSpeed { get; private set; }
        public bool GameOver { get; private set; }
        public bool Paused { get; set; }
        public int Score { get; private set; }
        public int Level { get;  set; }
        public string Name { get; set; }

        public Game()
        {
            Grid = new GameGrid(22, 10);
            BlockQueue = new BlockQueue();
            CurrentBlock = BlockQueue.GetNextBlock();
            Paused = true;
        }

        private bool BlockFits()
        {
            foreach (var p in CurrentBlock.GetTilePositions())
            {
                if(!Grid.IsEmpty(p.Row, p.Column))
                {
                    return false;
                }
            }
            return true;
        }

        public void RotateBlockClockwise()
        {
            CurrentBlock.RotateClockwise();
            if (!BlockFits())
            {
                CurrentBlock.RotateCounterClockwise();
            }
        }

        public void RotateBlockCounterClockwise()
        {
            CurrentBlock.RotateCounterClockwise();
            if (!BlockFits())
            {
                CurrentBlock.RotateClockwise();
            }
        }

        public void MoveBlockLeft()
        {
            CurrentBlock.Move(0, -1);
            if (!BlockFits())
            {
                CurrentBlock.Move(0, 1);
            }
        }

        public void MoveBlockRight()
        {
            CurrentBlock.Move(0, 1);
            if (!BlockFits())
            {
                CurrentBlock.Move(0, -1);
            }
        }

        private bool IsGameOver()
        {
            return !(Grid.IsRowEmpty(0) && Grid.IsRowEmpty(1));
        }

        private void PlaceBlock()
        {
            foreach (var p in CurrentBlock.GetTilePositions())
            {
                Grid[p.Row, p.Column] = CurrentBlock.Id;
            }

            Score += Grid.ClearFullRows();

            if (IsGameOver())
            {
                GameOver = true;
            }
            else
            {
                CurrentBlock = BlockQueue.GetNextBlock();
            }
        }

        public void MoveBlockDown()
        {
            CurrentBlock.Move(1, 0);
            if (!BlockFits())
            {
                CurrentBlock.Move(-1, 0);
                PlaceBlock();
            }
        }

        private int TileDropDistance(Position p)
        {
            int drop = 0;

            while(Grid.IsEmpty(p.Row + drop + 1, p.Column))
            {
                drop++;
            }

            return drop;
        }

        public int BlockDropDistance()
        {
            return CurrentBlock.GetTilePositions().Min(p => TileDropDistance(p));
        }

        public void DropBlock()
        {
            CurrentBlock.Move(BlockDropDistance(), 0);
            PlaceBlock();
        }

        public void SetGameLvl(int level)
        {
            Level = level;
            GameSpeed = 1000 - (level - 1) * 200;
        }
    }
}
