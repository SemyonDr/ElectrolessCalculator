using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ElectrolessCalculator.ViewModel
{
    public class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public bool CanExecute(object parameter)
        {
            if (canExecute == null)
                return true;

            return this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }

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

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
