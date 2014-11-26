using System.Windows.Input;
using Chess.Game;
using Chess.Model;
using Command;

namespace Chess.ViewModels
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            SquarePressed = new SimpleCommand(executeSquarePressed, CanExecuteSquarePressed);

            GameEngine = new GameEngine();
        }

        #region commands

        public ICommand SquarePressed { get; private set; }

        public GameEngine GameEngine { get; private set; }

        private bool CanExecuteSquarePressed(object parameter)
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