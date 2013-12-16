using System;
using System.ComponentModel;
using SharpDox.Sdk.Config;

namespace SharpDox.Config.Tests
{
    public class TestConfig : IConfigSection
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _testVar1;
        private string _testVar2;
        private string _testVar3;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public string TestVar1
        {
            get { return _testVar1; }
            set
            {
                _testVar1 = value;
                OnPropertyChanged("TestVar1");
            }
        }

        public string TestVar2
        {
            get { return _testVar2; }
            set
            {
                _testVar2 = value;
                OnPropertyChanged("TestVar2");
            }
        }

        public string TestVar3
        {
            get { return _testVar3; }
            set
            {
                _testVar3 = value;
                OnPropertyChanged("TestVar3");
            }
        }

        public Guid Guid
        {
            get { return new Guid("6bc727ba-1391-4e9f-95b1-6c800eec9799"); }
        }
    }
}