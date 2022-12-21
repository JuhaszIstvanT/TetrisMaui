using System.Collections.ObjectModel;
using Tetris.Model;

namespace Tetris.Maui.ViewModel
{
    public class TetrisViewModel : ViewModelBase
    {
        #region Fields
        private TetrisGameModel _model;
        #endregion

        #region Properties
        public DelegateCommand NewGameCommand { get; private set; }

        public RowDefinitionCollection RowDefinitions
        {
            get => new RowDefinitionCollection(Enumerable.Repeat(new RowDefinition(GridLength.Star), 16).ToArray());
        }
        public ColumnDefinitionCollection ColumnDefinitions
        {
            get => new ColumnDefinitionCollection(Enumerable.Repeat(new ColumnDefinition(GridLength.Star), 8).ToArray());
        }
        public ObservableCollection<TetrisField> Fields { get; set; }
        #endregion

        #region Events
        public event EventHandler? NewGame;
        #endregion
        #region Constructors
        public TetrisViewModel(TetrisGameModel model)
        {
            _model = model;
            _model.GameCreated += new EventHandler<TetrisEventArgs>(Model_GameCreated);
            _model.FieldChanged += new EventHandler<FieldChangedEventArgs>(Model_FieldChanged);

            NewGameCommand = new DelegateCommand(param => OnNewGame());


            Fields = new ObservableCollection<TetrisField>();

            //RefreshTable();

        }
        #endregion

        #region Private methods
        private void RefreshTable()
        {
            Fields = null;
            Fields = new ObservableCollection<TetrisField>();
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Fields.Add(new TetrisField
                    {
                        Id = _model.Table[i, j],
                        X = i,
                        Y = j,
                        Number = i * 8 + j,
                    });
                }
            }
            int aaa = 0;
        }
        #endregion

        #region Game event handlers
        private void Model_GameAdvanced(object? sender, TetrisEventArgs e)
        {
            //OnPropertyChanged(nameof(GameTime));
        }
        private void Model_GameCreated(object? sender, TetrisEventArgs e)
        {
            RefreshTable();
        }

        private void Model_FieldChanged(object? sender, FieldChangedEventArgs e)
        {
            Fields.First(field => field.X == e.X && field.Y == e.Y).Id = e.Id;
        }

        #endregion

        #region Event methods
        private void OnNewGame()
        {
            NewGame?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
