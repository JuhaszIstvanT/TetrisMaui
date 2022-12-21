using System.Numerics;
using Tetris.Persistence;

namespace Tetris.Model
{
    public enum GameSize { Small, Medium, Big }
    public class TetrisGameModel
    {
        #region Size constants
        private const int GameSizeSmall = 4;
        private const int GameSizeMedium = 8;
        private const int GameSizeBig = 12;
        #endregion

        #region Fields
        private ITetrisDataAccess _dataAccess;
        private TetrisTable _table;
        private Block _currentBlock;
        private BlockQueue _blockQueue;
        private GameSize _gameSize;
        private int _gameTime;
        #endregion

        #region Properties
        public int GameTime { get { return _gameTime; } }
        public TetrisTable Table { get { return _table; } }
        public GameSize GameSize { get { return _gameSize; } set { _gameSize = value; } }
        public Block CurrentBlock
        {
            get => _currentBlock;
            private set
            {
                _currentBlock = value;
                _currentBlock.Reset();

            }
        }
        public bool IsGameOver { get { return !(_table.IsRowEmpty(0) && _table.IsRowEmpty(1)); } }
        public BlockQueue BlockQueue { get { return _blockQueue; } }

        #endregion

        #region Events
        public event EventHandler<TetrisEventArgs>? GameAdvanced;

        public event EventHandler<TetrisEventArgs>? GameOver;
        public event EventHandler<TetrisEventArgs> GameCreated;
        public event EventHandler<FieldChangedEventArgs> FieldChanged;
        #endregion

        #region Constructor
        public TetrisGameModel(ITetrisDataAccess dataAccess)
        {
            _blockQueue = new BlockQueue();
            CurrentBlock = BlockQueue.GetAndUpdate();
            _dataAccess = dataAccess;
            _table = new TetrisTable();
            _gameSize = GameSize.Medium;
            GenerateFields();
        }
        #endregion

        #region Public game methods
        public void NewGame()
        {
            _table = new TetrisTable(16, (int)GameSize * 4 + 4);
            CurrentBlock = BlockQueue.GetAndUpdate();
            _gameTime = 0;
            GenerateFields();
            OnGameCreated();
        }

        public void AdvanceTime()
        {
            _gameTime++;
            OnGameAdvanced();
        }

        public void RotateBlockCW()
        {
            foreach (Position p in CurrentBlock.TilePositions())
            {
                OnFieldChanged(p.X, p.Y, 0);
            }
            CurrentBlock.RotateCW();

            if (!BlockFits())
            {
                CurrentBlock.RotateCCW();
            }
            foreach (Position p in CurrentBlock.TilePositions())
            {
                OnFieldChanged(p.X, p.Y, CurrentBlock.Id);
            }
        }

        public void MoveBlockLeft()
        {
            foreach (Position p in CurrentBlock.TilePositions())
            {
                OnFieldChanged(p.X, p.Y, 0);
            }
            CurrentBlock.Move(0, -1);

            if (!BlockFits())
            {
                CurrentBlock.Move(0, 1);
            }
            foreach (Position p in CurrentBlock.TilePositions())
            {
                OnFieldChanged(p.X, p.Y, CurrentBlock.Id);
            }
        }

        public void MoveBlockRight()
        {
            foreach (Position p in CurrentBlock.TilePositions())
            {
                OnFieldChanged(p.X, p.Y, 0);
            }
            CurrentBlock.Move(0, 1);

            if (!BlockFits())
            {
                CurrentBlock.Move(0, -1);
            }
            foreach (Position p in CurrentBlock.TilePositions())
            {
                OnFieldChanged(p.X, p.Y, CurrentBlock.Id);
            }
        }

        public void MoveBlockDown()
        {
            foreach (Position p in CurrentBlock.TilePositions())
            {
                OnFieldChanged(p.X, p.Y, 0);
            }
            CurrentBlock.Move(1, 0);

            if (!BlockFits())
            {
                CurrentBlock.Move(-1, 0);
                PlaceBlock();
            }
            foreach (Position p in CurrentBlock.TilePositions())
            {
                OnFieldChanged(p.X, p.Y, CurrentBlock.Id);
            }
        }

        public void LoadGame(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            var names = _dataAccess.Load(path, _blockQueue);
            _table = names.Item1;
            int _id = names.Item2;
            _gameTime = names.Item3;
            _currentBlock = _blockQueue.blocks[_id - 1];
            _gameSize = (GameSize)(_table.Columns / 4 - 1);

            OnGameCreated();

            for (int i = 0; i < 16; ++i)
            {
                for (int j = 0; j < (int)GameSize * 4 + 4; ++j)
                {
                    OnFieldChanged(i, j, _table[i, j]);
                }
            }
            foreach (Position p in CurrentBlock.TilePositions())
            {
                OnFieldChanged(p.X, p.Y, CurrentBlock.Id);
            }
            OnGameAdvanced();
        }

        public void LoadGameModel(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            TetrisTable _table = _dataAccess.LoadGame(path);
            _gameSize = (GameSize)(_table.Columns / 4 - 1);

        }
        public void SaveGame(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");
            _dataAccess.Save(path, _table, _currentBlock, _gameTime);
        }
        #endregion

        #region Private game methods
        private void GenerateFields()
        {
            for (int i = 0; i < 16; ++i)
            {
                for (int j = 0; j < (int)GameSize * 4 + 4; ++j)
                {
                    _table[i, j] = 0;
                }
            }
        }
        private void PlaceBlock()
        {
            foreach (Position p in CurrentBlock.TilePositions())
            {
                _table[p.X, p.Y] = CurrentBlock.Id;
                OnFieldChanged(p.X, p.Y, CurrentBlock.Id);
            }

            _table.ClearFullRows();

            for (int i = 0; i < 16; ++i)
            {
                for (int j = 0; j < (int)GameSize * 4 + 4; ++j)
                {
                    OnFieldChanged(i, j, _table[i, j]);
                }
            }

            if (IsGameOver)
            {
                OnGameOver(true);
            }
            else
            {
                CurrentBlock = BlockQueue.GetAndUpdate();
            }




        }
        private bool BlockFits()
        {
            foreach (Position p in CurrentBlock.TilePositions())
            {
                if (!_table.IsEmpty(p.X, p.Y))
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region Private event methods
        private void OnGameAdvanced()
        {
            GameAdvanced?.Invoke(this, new TetrisEventArgs(_gameTime, false));
        }
        private void OnGameOver(bool isGameOver)
        {
            GameOver?.Invoke(this, new TetrisEventArgs(_gameTime, isGameOver));
        }
        private void OnGameCreated()
        {
            GameCreated?.Invoke(this, new TetrisEventArgs(_gameTime, false));
        }
        private void OnFieldChanged(int x, int y, int id)
        {
            FieldChanged?.Invoke(this, new FieldChangedEventArgs(x, y, id));
        }
        #endregion
    }
}
