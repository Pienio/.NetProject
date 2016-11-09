using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Visits
{
    class Command : ICommand
    {
        private readonly Func<object, bool> _canExecute;
        private readonly Action<object> _execute;

        private bool couldExecute = false;

        public Command(Action<object> execute, Func<object, bool> canExecute = null)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
            _execute = execute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            bool res = _canExecute == null ? true : _canExecute(parameter);
            //if (res != couldExecute && CanExecuteChanged != null)
            //    CanExecuteChanged(this, EventArgs.Empty);
            return res;
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
