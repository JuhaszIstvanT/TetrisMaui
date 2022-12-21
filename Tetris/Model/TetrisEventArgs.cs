using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Model
{
    public class TetrisEventArgs : EventArgs
    {
        #region Fields
        private int _gameTime;
        private bool _isGameOver;
        #endregion

        #region Properties
        public int GameTime { get { return _gameTime; } }
        public bool IsGameOver { get { return _isGameOver; } }
        #endregion

        #region Constructor
        public TetrisEventArgs(int gameTime, bool isGameOver)
        {
            _gameTime = gameTime;
            _isGameOver = isGameOver;
        }
        #endregion
    }
}
