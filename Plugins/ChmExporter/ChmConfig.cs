using System.ComponentModel;
using System;
using SharpDox.Sdk.Config;

namespace SharpDox.Plugins.Chm
{
    public class ChmConfig : IConfigSection
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _backgroundColor;
        private string _textColor;
        private string _linkColor;
        private string _linkHoverColor;
        private string _tableHeaderBackgroundColor;
        private string _tableHeaderBorderColor;
        private string _breadCrumbBorderColor;
        private string _breadCrumLinkColor;
        private string _breadCrumLinkHoverColor;
        private string _syntaxBoxBackgroundColor;
        private string _syntaxBoxBorderColor;
        private string _syntaxBoxTextColor;
        private string _breadCrumBackgroundColor;

        public string BackgroundColor
        {
            get { return _backgroundColor ?? "#FFFFFF"; }
            set
            {
                _backgroundColor = value;
                OnPropertyChanged("BackgroundColor");
            }
        }

        public string TextColor
        {
            get { return _textColor ?? "#636363"; }
            set
            {
                _textColor = value;
                OnPropertyChanged("TextColor");
            }
        }

        public string LinkColor
        {
            get { return _linkColor ?? "#1382CE"; }
            set
            {
                _linkColor = value;
                OnPropertyChanged("LinkColor");
            }
        }

        public string LinkHoverColor
        {
            get { return _linkHoverColor ?? "#F58026"; }
            set
            {
                _linkHoverColor = value;
                OnPropertyChanged("LinkHoverColor");
            }
        }

        public string TableHeaderBackgroundColor
        {
            get { return _tableHeaderBackgroundColor ?? "#F6F5F1"; }
            set
            {
                _tableHeaderBackgroundColor = value;
                OnPropertyChanged("TableHeaderBackgroundColor");
            }
        }

        public string TableHeaderBorderColor
        {
            get { return _tableHeaderBorderColor ?? "#C2C2C2"; }
            set
            {
                _tableHeaderBorderColor = value;
                OnPropertyChanged("TableHeaderBorderColor");
            }
        }

        public string BreadCrumbBackgroundColor
        {
            get { return _breadCrumBackgroundColor ?? "#EEEEEE"; }
            set
            {
                _breadCrumBackgroundColor = value;
                OnPropertyChanged("BreadCrumBackgroundColor");
            }
        }

        public string BreadCrumbBorderColor
        {
            get { return _breadCrumbBorderColor ?? "#B4A9AC"; }
            set
            {
                _breadCrumbBorderColor = value;
                OnPropertyChanged("BreadCrumBorderColor");
            }
        }

        public string BreadCrumbLinkColor
        {
            get { return _breadCrumLinkColor ?? "#8C7F83"; }
            set
            {
                _breadCrumLinkColor = value;
                OnPropertyChanged("BreadCrumLinkColor");
            }
        }

        public string BreadCrumbLinkHoverColor
        {
            get { return _breadCrumLinkHoverColor ?? "#574C4F"; }
            set
            {
                _breadCrumLinkHoverColor = value;
                OnPropertyChanged("BreadCrumLinkHoverColor");
            }
        }

        public string SyntaxBoxBackgroundColor
        {
            get { return _syntaxBoxBackgroundColor ?? "#EEEEEE"; }
            set
            {
                _syntaxBoxBackgroundColor = value;
                OnPropertyChanged("SyntaxBoxBackgroundColor");
            }
        }

        public string SyntaxBoxBorderColor
        {
            get { return _syntaxBoxBorderColor ?? "#CCCCCC"; }
            set
            {
                _syntaxBoxBorderColor = value;
                OnPropertyChanged("SyntaxBoxBorderColor");
            }
        }

        public string SyntaxBoxTextColor
        {
            get { return _syntaxBoxTextColor ?? "#636363"; }
            set
            {
                _syntaxBoxTextColor = value;
                OnPropertyChanged("SyntaxBoxTextColor");
            }
        }

        public Guid Guid { get { return new Guid("36db802f-af3f-456d-910f-e2f9255e8150"); } }         
    }
}
