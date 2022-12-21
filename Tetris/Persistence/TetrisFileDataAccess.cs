using Tetris.Model;

namespace Tetris.Persistence
{
    public class TetrisFileDataAccess : ITetrisDataAccess
    {
        private String? _basePath = String.Empty;

        public TetrisFileDataAccess(String? basePath = null)
        {
            _basePath = basePath;
        }

        public (TetrisTable, int, int) Load(String path, BlockQueue bq)
        {
            if (!String.IsNullOrEmpty(_basePath))
                path = Path.Combine(_basePath, path);

            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    String line = reader.ReadLine() ?? String.Empty;
                    String[] numbers = line.Split(' ');
                    Int32 tableWidth = Int32.Parse(numbers[0]);
                    Int32 tableHeight = Int32.Parse(numbers[1]);
                    TetrisTable table = new TetrisTable(tableHeight, tableWidth);

                    for (Int32 i = 0; i < tableHeight; i++)
                    {
                        line = reader.ReadLine() ?? String.Empty;
                        numbers = line.Split(' ');

                        for (Int32 j = 0; j < tableWidth; j++)
                        {
                            table[i, j] = Int32.Parse(numbers[j]);
                        }
                    }

                    line = reader.ReadLine() ?? String.Empty;
                    numbers = line.Split(' ');
                    int id = Int32.Parse(numbers[0]);
                    int rs = Int32.Parse(numbers[1]);
                    int offsetX = Int32.Parse(numbers[2]);
                    int offsetY = Int32.Parse(numbers[3]);
                    int time = Int32.Parse(numbers[4]);
                    bq.blocks[id - 1].RotationState = rs;
                    bq.blocks[id - 1].Offset.X = offsetX;
                    bq.blocks[id - 1].Offset.Y = offsetY;

                    return (table, id, time);
                }
            }
            catch
            {
                throw new TetrisDataException();
            }
        }
        public TetrisTable LoadGame(String path)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    String line = reader.ReadLine() ?? String.Empty;
                    String[] numbers = line.Split(' ');
                    Int32 tableWidth = Int32.Parse(numbers[0]);
                    Int32 tableHeight = Int32.Parse(numbers[1]);
                    TetrisTable table = new TetrisTable(tableHeight, tableWidth);

                    for (Int32 i = 0; i < tableHeight; i++)
                    {
                        line = reader.ReadLine() ?? String.Empty;
                        numbers = line.Split(' ');

                        for (Int32 j = 0; j < tableWidth; j++)
                        {
                            table[i, j] = Int32.Parse(numbers[j]);
                        }
                    }

                    line = reader.ReadLine() ?? String.Empty;
                    numbers = line.Split(' ');
                    int id = Int32.Parse(numbers[0]);
                    int rs = Int32.Parse(numbers[1]);
                    int offsetX = Int32.Parse(numbers[2]);
                    int offsetY = Int32.Parse(numbers[3]);

                    return table;
                }
            }
            catch
            {
                throw new TetrisDataException();
            }
        }
        public void Save(String path, TetrisTable table, Block block, int time)
        {
            if (!String.IsNullOrEmpty(_basePath))
                path = Path.Combine(_basePath, path);

            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    writer.Write(table.Columns);
                    writer.WriteLine(" " + table.Rows);
                    for (Int32 i = 0; i < table.Rows; i++)
                    {
                        for (Int32 j = 0; j < table.Columns; j++)
                        {
                            writer.Write(table[i, j] + " ");
                        }
                        writer.WriteLine();
                    }

                    writer.WriteLine(block.Id + " " + block.RotationState + " " + block.Offset.X + " " + block.Offset.Y + " " + time);
                }
            }
            catch
            {
                throw new TetrisDataException();
            }
        }
    }
}
