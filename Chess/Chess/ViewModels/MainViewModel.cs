using System.Windows;
using System.Windows.Input;
using Chess.Game;
using Chess.Model;
using Utilities;

namespace Chess.ViewModels
{
    internal class MainViewModel
    {
        public MainViewModel()
        {
            SquarePressed = new SimpleCommand(executeSquarePressed, canExecuteSquarePressed);
            NewGame = new SimpleCommand(executeNewGame, canExecuteNewGame);
            Close = new SimpleCommand(executeClose, canExecuteClose);

            GameEngine = new GameEngine();
        }

        public GameEngine GameEngine { get; private set; }

        public ICommand SquarePressed { get; private set; }
        public ICommand NewGame { get; private set; }
        public ICommand Close { get; private set; }

        #region commands

        private void executeClose(object parameter)
        {
            Application.Current.Shutdown();
        }

        private bool canExecuteClose(object parameter)
        {
            return true;
        }

        private bool canExecuteNewGame(object parameter)
        {
            return true;
        }

        private void executeNewGame(object parameter)
        {
            GameEngine.NewGame();
        }

        private bool canExecuteSquarePressed(object parameter)
        {
            return true;
        }

        private void executeSquarePressed(object parameter)
        {
            var square = parameter as Square;

            GameEngine.HandleInput(square);
        }
    }

    #endregion
}