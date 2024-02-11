using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _12A_Tetris
{
    public abstract class Block
    {
        protected abstract Position[][] Tiles { get; }
        protected abstract Position StartOffset { get; }
        public abstract int Id { get; }

        private int rotationState;
        private Position offset;

        public Block()
        {
            offset = new Position(StartOffset.Row, StartOffset.Column);
        }

        public IEnumerable<Position> GetTilePositions()
        {
            return Tiles[rotationState].Select(p => new Position(p.Row + offset.Row, p.Column + offset.Column));
        }

        public void RotateClockwise()
        {
            rotationState = (rotationState + 1) % Tiles.Length;
        }

        public void RotateCounterClockwise()
        {
            rotationState = (rotationState + Tiles.Length - 1) % Tiles.Length;
        }

        public void Move(int row, int column)
        {
            offset = new Position(offset.Row + row, offset.Column + column);
        }

        public void Reset()
        {
            offset = new Position(StartOffset.Row, StartOffset.Column);
            rotationState = 0;
        }
    }
}
