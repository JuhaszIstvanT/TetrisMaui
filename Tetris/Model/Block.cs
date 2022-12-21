namespace Tetris.Model
{
    public abstract class Block
    {
        #region Fields
        private int rotationState;
        private Position offset;
        #endregion
        #region Properties
        public abstract Position[][] Tiles { get; }
        public abstract int Id { get; }
        public int RotationState { get { return rotationState; } set { rotationState = value; } }
        public Position Offset { get { return offset; } set { offset = value; } }
        #endregion

        #region Constructor
        public Block()
        {
            offset = new Position(0, 0);
        }
        #endregion

        #region Public methods
        public IEnumerable<Position> TilePositions()
        {
            foreach (Position p in Tiles[rotationState])
            {
                yield return new Position(p.X + offset.X, p.Y + offset.Y);
            }
        }
        public void RotateCW()
        {
            rotationState = (rotationState + 1) % Tiles.Length;
        }
        public void RotateCCW()
        {
            if (rotationState == 0)
            {
                rotationState = Tiles.Length - 1;
            }
            else
            {
                rotationState--;
            }
        }

        public void Move(int rows, int columns)
        {
            offset.X += rows;
            offset.Y += columns;
        }

        public void Reset()
        {
            rotationState = 0;
            offset.X = 0;
            offset.Y = 0;
        }
        #endregion

    }
}
