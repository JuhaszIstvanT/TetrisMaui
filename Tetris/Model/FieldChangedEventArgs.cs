using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Model
{
    public class FieldChangedEventArgs : EventArgs
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public FieldChangedEventArgs(int x, int y, int id)
        {
            Id = id;
            X = x;
            Y = y;
        }
    }
}
