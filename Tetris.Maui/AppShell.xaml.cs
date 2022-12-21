using Tetris.Persistence;
using Tetris.Model;
using Tetris.Maui.View;
using Tetris.Maui.ViewModel;

namespace Tetris.Maui;

public partial class AppShell : Shell
{
	private ITetrisDataAccess _tetrisDataAccess;
	private readonly TetrisGameModel _tetrisGameModel;
	private readonly TetrisViewModel _tetrisViewModel;

	private readonly IDispatcherTimer _timer;

    private readonly IStore _store;
	public AppShell(IStore tetrisStore, ITetrisDataAccess tetrisDataAccess, TetrisGameModel tetrisGameModel, TetrisViewModel tetrisViewModel)
	{
		InitializeComponent();

		_store = tetrisStore;
		_tetrisDataAccess = tetrisDataAccess;
		_tetrisGameModel = tetrisGameModel;
		_tetrisViewModel = tetrisViewModel;
        _tetrisViewModel.NewGame += TetrisViewModel_NewGame;

        _timer = Dispatcher.CreateTimer();
		_timer.Interval = TimeSpan.FromSeconds(1);
		_timer.Tick += (_, _) => _tetrisGameModel.AdvanceTime();
        _timer.Tick += (_, _) => _tetrisGameModel.MoveBlockDown();
		_timer.Start();

    }

    internal void StartTimer() => _timer.Start();
    internal void StopTimer() => _timer.Stop();

	private void TetrisViewModel_NewGame(object? sender, EventArgs e)
	{
		_tetrisGameModel.GameSize = GameSize.Medium;
		_tetrisGameModel.NewGame();

		//StartTimer();
	}
}
