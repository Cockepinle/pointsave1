using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace POINT.ViewModel.Helpers
{
    internal class BindableCommand : ICommand
    {
        private Action<object> _execute;
        private Func<object, bool> _canExecute;


        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        //Вызывается при изменении условий

        public BindableCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        //Конструктор, который помогает передавать комаду

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }
        //Определяет, может ли команда выполняться


        public void Execute(object parameter)
        {
            _execute(parameter);
        }
        //Выполняет логику программы
    }

}

