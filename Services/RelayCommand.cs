using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace musicvisualizerWPF.Services
{
    // simplifies the creation of commands by encapsulating the logic in
    // a resuable class. Purpose of this class is to provide a concenient way
    // to bind commands from the viewModel to actions in the View
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;


        // Constructor takes two delegates: 'execute' (action to be performed) and
        // 'canExecute' (condition for whtere the command can execute)
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        // method is called to determine whether the command can execute in the current state.
        public bool CanExecute(object parameter)
        {
            bool result = _canExecute == null ? true : _canExecute(parameter);
            return result;
        }

        // method is called when the command is invoked. executes the action specified in _execute and
        // passes the provided parameter
        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        // part of the ICommand interface it allows the command to notify the system when the ability
        // to execute the command might have changed.
        // Wired to the 'CommandManager.RequerySuggested' event, which is a system event
        // that suggests when the command manager should reevaluate the ability to exeute commands.
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove {  CommandManager.RequerySuggested -= value;}
        }
    }
}
