namespace Tetris.Persistence
{
    public class TetrisTable
    {
        #region Fields
        private int[,] _table;
        #endregion

        #region Properties
        public int Rows { get { return _table.GetLength(0); } }
        public int Columns { get { return _table.GetLength(1); } }
        public int this[int r, int c] { get { return _table[r, c]; } set { _table[r, c] = value; } }
        #endregion

        #region Constructor
        public TetrisTable()
        {
            _table = new int[16, 8];
        }
        public TetrisTable(int row, int col)
        {
            _table = new int[row, col];
        }
        #endregion

        #region Public methods
        public bool IsInside(int r, int c)
        {
            return r >= 0 && r < Rows && c >= 0 && c < Columns;
        }
        public bool IsEmpty(int r, int c)
        {
            return IsInside(r, c) && _table[r, c] == 0;
        }

        public bool IsRowFull(int r)
        {
            for (int c = 0; c < Columns; ++c)
            {
                if (_table[r, c] == 0)
                {
                    return false;
                }
            }
            return true;
        }
        public bool IsRowEmpty(int r)
        {
            for (int c = 0; c < Columns; ++c)
            {
                if (_table[r, c] != 0)
                {
                    return false;
                }
            }
            return true;
        }

        public int ClearFullRows()
        {
            int cleared = 0;
            for (int r = Rows - 1; r >= 0; r--)
            {
                if (IsRowFull(r))
                {
                    ClearRow(r);
                    cleared++;
                }
                else if (cleared > 0)
                {
                    MoveRowDown(r, cleared);
                }
            }
            return cleared;
        }

        #endregion

        #region Private methods
        private void ClearRow(int r)
        {
            for (int c = 0; c < Columns; ++c)
            {
                _table[r, c] = 0;
            }
        }

        private void MoveRowDown(int r, int numRows)
        {
            for (int c = 0; c < Columns; ++c)
            {
                _table[r + numRows, c] = _table[r, c];
                _table[r, c] = 0;
            }
        }
        #endregion


    }
}
