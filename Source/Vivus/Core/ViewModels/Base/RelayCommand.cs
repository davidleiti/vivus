namespace Vivus.Core.ViewModels
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// Represents a basic command that runs an action.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private Action action;

        /// <summary>
        /// The event thats fired when the <see cref="CanExecute(object)"/> value has changed.
        /// </summary>
        public event EventHandler CanExecuteChanged = (sender, e) => { };

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class with the specified value.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        public RelayCommand(Action action)
        {
            this.action = action;
        }

        /// <summary>
        /// A relay command can always be executed.
        /// </summary>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Executes the action of the command.
        /// </summary>
        public void Execute(object parameter)
        {
            action();
        }
    }

    /// <summary>
    /// Represents a basic command that takes a parameter and runs an action.
    /// </summary>
    public class RelayCommand<T> : ICommand where T : class
    {
        private Action<T> action;

        /// <summary>
        /// The event thats fired when the <see cref="CanExecute(object)"/> value has changed.
        /// </summary>
        public event EventHandler CanExecuteChanged = (sender, e) => { };

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class with the specified value.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        public RelayCommand(Action<T> action)
        {
            this.action = action;
        }

        /// <summary>
        /// A relay command can always be executed.
        /// </summary>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Executes the parameterized action of the command.
        /// </summary>
        /// <param name="parameter">The parameter of the command.</param>
        public void Execute(object parameter)
        {
            if (!(parameter is T))
                throw new InvalidCastException("Parameter type is not valid.");

            action(parameter as T);
        }
    }
}
