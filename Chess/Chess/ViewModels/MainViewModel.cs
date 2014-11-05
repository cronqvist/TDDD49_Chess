using Chess.Game;
using Chess.Model;
using Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Chess.ViewModels
{
    public class MainViewModel
    {
        public ICommand SquarePressed { get; private set; }

        public GameEngine GameEngine { get; private set; }

        public MainViewModel()
        {
            SquarePressed = new SimpleCommand(ExecuteSquarePressed, CanExecuteSquarePressed);

            GameEngine = new GameEngine();
        }

        #region commands

        private bool CanExecuteSquarePressed(object parameter)
        {
            return true;
        }

        private void ExecuteSquarePressed(object parameter)
        {
            Square square = parameter as Square;

            GameEngine.HandleInput(square);
        }
    }

    #endregion
}
