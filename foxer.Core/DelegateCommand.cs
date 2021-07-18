using System;
using System.Windows.Input;

namespace foxer.Core
{
    public class DelegateCommand : ICommand
    {
        private readonly Action _action;
        private bool _enabled;

        public event EventHandler CanExecuteChanged;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                if (_enabled != value)
                {
                    _enabled = value;
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public DelegateCommand(Action action, bool canExecute)
        {
            _action = action;
            _enabled = canExecute;
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            return _enabled;
        }

        public void Execute(object parameter)
        {
            if (_enabled)
            {
                _action?.Invoke();
            }
        }
    }


    internal class DelegateCommand<T> : ICommand
    {
        private readonly Action<T> _action;
        private bool _enabled;

        public event EventHandler CanExecuteChanged;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                if (_enabled != value)
                {
                    _enabled = value;
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public DelegateCommand(Action<T> action, bool canExecute)
        {
            _action = action;
            _enabled = canExecute;
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            return _enabled;
        }

        public void Execute(object parameter)
        {
            if (_enabled && parameter is T)
            {
                _action?.Invoke((T)parameter);
            }
        }
    }
}
