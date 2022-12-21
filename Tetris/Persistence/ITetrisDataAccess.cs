using Tetris.Model;

namespace Tetris.Persistence
{
    public interface ITetrisDataAccess
    {
        (TetrisTable, int, int) Load(String path, BlockQueue bq);
        TetrisTable LoadGame(String path);
        void Save(String path, TetrisTable table, Block block, int time);
    }
}
