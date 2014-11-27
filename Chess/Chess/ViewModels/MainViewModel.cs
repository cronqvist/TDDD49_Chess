using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;
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