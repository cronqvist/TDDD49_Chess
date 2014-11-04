using System;
using System.Windows.Input;

/// http://stackoverflow.com/questions/1468791/wpf-icommand-mvvm-implementation

namespace Command
{
    /// <summary>
    /// SimpleCommand used for abstraction to avoid massive amounts of command binding.
    /// </summary>
    /// <remarks>
    /// Idea from Marlon "Marlon" Grech:
    /// http://marlongrech.wordpress.com/2008/11/26/avoiding-commandbinding-in-the-xaml-code-behind-files/
    /// Author(s): Mattias Cronqvist, Fredrik Präntare
    /// </remarks>
    public class SimpleCommand : ICommand
    {
        /// <summary>
        /// The command that is executed.
        /// </summary>
        /// <param name="parameter"></param>
        public delegate void ICommandOnExecute(object parameter);
        
        /// <summary>
        /// Command that checks wether this command is executeable.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public delegate bool ICommandOnCanExecute(object parameter);

        private ICommandOnExecute _execute;
        private ICommandOnCanExecute _canExecute;

        /// <summary>
        /// Constructor that creates a simplecomand. 
        /// </summary>
        /// <param name="onExecuteMethod">Is executed when whatever the command is binded to is executed.</param>
        /// <param name="onCanExecuteMethod">Checks wether whatever the command is binded to is able to execute.</param>
        public SimpleCommand(ICommandOnExecute onExecuteMethod, ICommandOnCanExecute onCanExecuteMethod)
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
