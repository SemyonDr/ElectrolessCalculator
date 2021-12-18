using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ElectrolessCalculator.ViewModel
{
    /// <summary>
    /// Simple WPF command interface realisation.
    /// Wrapper for method for WPF binding.
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region EVENTS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void OnCanExecuteChanged() {
            CommandManager.InvalidateRequerySuggested();
        }

        #endregion

        #region DELEGATES
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        private Action<object> execute;
        private Func<object, bool> canExecute;

        #endregion

        #region CONSTRUCTORS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Creates command that have canExecute check.
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute) {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Creates command that always can be executed.
        /// </summary>
        /// <param name="execute"></param>
        public RelayCommand(Action<object> execute) {
            this.execute = execute;
        }
        #endregion

        #region METHODS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter) {
            this.execute(parameter);
        }

        /// <summary>
        /// Returns if command can be executed.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter) {
            if (canExecute == null)
                return true;

            return this.canExecute(parameter);
        }

        #endregion
    }
}
