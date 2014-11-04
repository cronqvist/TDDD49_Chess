using Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Chess.ViewModels
{
    public class MainViewModel
    {
        public ICommand PressedButton { get; private set; }

        public MainViewModel()
        {
            PressedButton = new SimpleCommand(ExecutePressedButton, CanExecutePressedButton);
        }

        private bool CanExecutePressedButton(object parameter)
        {
            return true;
        }

        private void ExecutePressedButton(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
