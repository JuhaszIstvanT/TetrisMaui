using Tetris.Persistence;
using Tetris.Maui.ViewModel;
using Tetris.Model;

namespace Tetris.Maui;

public partial class App : Application
{
    /// <summary>
    /// Erre az útvonalra mentjük a félbehagyott játékokat
    /// </summary>
    private const string SuspendedGameSavePath = "SuspendedGame";

    private readonly AppShell _appShell;
	private readonly ITetrisDataAccess _tetrisDataAccess;
	private readonly TetrisGameModel _tetrisGameModel;
	private readonly IStore _tetrisStore;
	private readonly TetrisViewModel _tetrisViewModel;
	public App()
	{
		InitializeComponent();

		_tetrisStore = new TetrisStore();
		_tetrisDataAccess = new TetrisFileDataAccess(FileSystem.AppDataDirectory);

		_tetrisGameModel = new TetrisGameModel(_tetrisDataAccess);
		_tetrisViewModel = new TetrisViewModel(_tetrisGameModel);

		_appShell = new AppShell(_tetrisStore, _tetrisDataAccess, _tetrisGameModel, _tetrisViewModel)
		{
			BindingContext = _tetrisViewModel
		};

		MainPage = _appShell;
	}

    protected override Window CreateWindow(IActivationState? activationState)
    {
        Window window = base.CreateWindow(activationState);

        window.Created += (s, e) =>
        {
			_tetrisGameModel.NewGame();
            _appShell.StartTimer();
        };

        return window;
    }
}
