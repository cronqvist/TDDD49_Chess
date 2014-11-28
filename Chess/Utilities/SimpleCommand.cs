using System;
using System.Windows.Input;

namespace Utilities
{
    public class SimpleCommand : ICommand
    {
        /// <summary>
        /// The command that is executed.
        /// </summary>
        /// <param name="parameter"></param>
        public delegate void CommandOnExecute(object parameter);

        /// <summary>
        /// Command that checks wether this command is executeable.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public delegate bool CommandOnCanExecute(object parameter);

        private readonly CommandOnExecute _execute;
        private readonly CommandOnCanExecute _canExecute;

        /// <summary>
        /// Constructor that creates a simplecomand. 
        /// </summary>
        /// <param name="onExecuteMethod">Is executed when whatever the command is binded to is executed.</param>
        /// <param name="onCanExecuteMethod">Checks wether whatever the command is binded to is able to execute.</param>
        public SimpleCommand(CommandOnExecute onExecuteMethod, CommandOnCanExecute onCanExecuteMethod)
        {
            _execute = onExecuteMethod;
            _canExecute = onCanExecuteMethod;
        }

        #region ICommand Members

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute.Invoke(parameter);
        }

        public void Execute(object parameter)
        {
            _execute.Invoke(parameter);
        }

        #endregion
    }
}
