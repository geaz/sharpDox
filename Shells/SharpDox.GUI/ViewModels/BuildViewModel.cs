using System;
using System.Text;
using SharpDox.GUI.Command;
using SharpDox.Sdk.Build;

namespace SharpDox.GUI.ViewModels
{
    internal class BuildViewModel : ViewModelBase
    {
        private StringBuilder _outputMessage;
        private bool _stepMessageOccured;

        private readonly Action _onCloseHandle;

        public BuildViewModel(IBuildMessenger buildMessenger, Action onCloseHandle)
        {
            _onCloseHandle = onCloseHandle;
            _outputMessage = new StringBuilder();

            buildMessenger.OnBuildProgress += (i) => { if (i == 0) _outputMessage = new StringBuilder(); };
            buildMessenger.OnBuildMessage += UpdateTextBox;
        }

        void UpdateTextBox(string message)
        {
            _outputMessage.AppendLine(message);
            Text = _outputMessage.ToString();
        }

        private string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged("Text");
            }
        }

        private RelayCommand _closeCommand;
        public RelayCommand CloseCommand
        {
            get
            {
                return _closeCommand ?? new RelayCommand(() => _onCloseHandle(), true);
            }
            set
            {
                _closeCommand = value;
                OnPropertyChanged("CloseCommand");
            }
        }
    }
}
