using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Maui.ViewModel
{
    public class TetrisField : ViewModelBase
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }
        public int X { get; set; }

        public int Y { get; set; }

        public int Number { get; set; }

    }
}
