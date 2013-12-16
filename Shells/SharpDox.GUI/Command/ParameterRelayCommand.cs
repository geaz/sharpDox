using System;
using System.Windows.Input;

namespace SharpDox.GUI.Command
{
    internal class ParameterRelayCommand<T> : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly Action<T> _handler;
        private bool _isEnabled;

        public ParameterRelayCommand(Action<T> handler, bool isEnabled)
        {
            _handler = handler;
            _isEnabled = isEnabled;
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (value != _isEnabled)
                {
                    _isEnabled = value;
                    if (CanExecuteChanged != null)
                    {
                        CanExecuteChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        public bool CanExecute(object parameter)
        {
            return IsEnabled;
        }

        public void Execute(object parameter)
        {
            _handler((T)parameter);
        }
    }
}
