using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using SharpDox.Sdk.Config;
using SharpDox.Sdk.Config.Attributes;

namespace SharpDox.Core.Config
{
    [Name(typeof (CoreStrings), "ConfigTitle")]
    public class CoreConfigSection : ICoreConfigSection
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isSaved;
        private string _author;
        private string _configFileName;
        private string _projectUrl;
        private string _authorUrl;
        private string _inputPath;
        private string _lastBuild;
        private string _logoPath;
        private string _outputPath;
        private string _docLanguage;
        private string _pathToConfig;
        private string _projectName;
        private string _versionNumber;
        private ObservableCollection<string> _excludedIdentifiers;
        private ObservableCollection<string> _activatedExporters;

        private readonly CoreStrings _strings;

        public CoreConfigSection(CoreStrings strings)
        {
            _strings = strings;
        }

        public bool IsSaved
        {
            get { return _isSaved; }
            set
            {
                if (_isSaved != value)
                {
                    _isSaved = value;
                    OnPropertyChanged("IsSaved");
                }
            }
        }

        public string ConfigFileName
        {
            get { return string.IsNullOrEmpty(_configFileName) ? _strings.NewConfig : _configFileName; }
            set
            {
                if (_configFileName != value)
                {
                    _configFileName = value;
                    OnPropertyChanged("ConfigFileName");
                }
            }
        }

        public string PathToConfig
        {
            get { return _pathToConfig; }
            set
            {
                if (_pathToConfig != value)
                {
                    _pathToConfig = value;
                    OnPropertyChanged("PathToConfig");
                }
            }
        }

        public string LastBuild
        {
            get { return String.IsNullOrEmpty(_lastBuild) ? _strings.Never : _lastBuild; }
            set
            {
                if (_lastBuild != value)
                {
                    _lastBuild = value;
                    OnPropertyChanged("LastBuild");
                }
            }
        }

        [Required]
        [Name(typeof(CoreStrings), "ProjectName")]
        public string ProjectName
        {
            get { return _projectName; }
            set
            {
                if (_projectName != value)
                {
                    _projectName = value;
                    OnPropertyChanged("ProjectName");
                }
            }
        }

        [Name(typeof(CoreStrings), "ProjectUrl")]
        public string ProjectUrl
        {
            get { return _projectUrl; }
            set
            {
                if (_projectUrl != value)
                {
                    _projectUrl = value;
                    OnPropertyChanged("ProjectUrl");
                }
            }
        }

        [Name(typeof(CoreStrings), "Author")]
        public string Author
        {
            get { return _author; }
            set
            {
                if (_author != value)
                {
                    _author = value;
                    OnPropertyChanged("Author");
                }
            }
        }

        [Name(typeof(CoreStrings), "AuthorUrl")]
        public string AuthorUrl
        {
            get { return _authorUrl; }
            set
            {
                if (_authorUrl != value)
                {
                    _authorUrl = value;
                    OnPropertyChanged("AuthorUrl");
                }
            }
        }

        [Name(typeof(CoreStrings), "VersionNumber")]
        public string VersionNumber
        {
            get { return _versionNumber; }
            set
            {
                if (_versionNumber != value)
                {
                    _versionNumber = value;
                    OnPropertyChanged("VersionNumber");
                }
            }
        }

        [ConfigEditor(EditorType.Filepicker, "Image File(.png; .jpg; .bmp)|*.png; *.jpg; *.bmp")]
        [Name(typeof(CoreStrings), "LogoPath")]
        public string LogoPath
        {
            get { return _logoPath; }
            set
            {
                if (_logoPath != value)
                {
                    _logoPath = value;
                    OnPropertyChanged("LogoPath");
                }
            }
        }

        [Required]
        [ConfigEditor(EditorType.Filepicker, "Solution/Project (*.sln; *.csproj)|*.sln; *.csproj")]
        [Name(typeof(CoreStrings), "InputPath")]
        public string InputPath
        {
            get { return _inputPath; }
            set
            {
                if (_inputPath != value)
                {
                    _inputPath = value;
                    OnPropertyChanged("InputPath");
                }
            }
        }

        [Required]
        [ConfigEditor(EditorType.Folderpicker)]
        [Name(typeof(CoreStrings), "OutputPath")]
        public string OutputPath
        {
            get { return _outputPath; }
            set
            {
                if (_outputPath != value)
                {
                    _outputPath = value;
                    OnPropertyChanged("OutputPath");
                }
            }
        }

        [Required]
        [ConfigEditor(EditorType.ComboBox, typeof(LanguageList))]
        [Name(typeof(CoreStrings), "DocLanguage")]
        public string DocLanguage
        {
            get { return _docLanguage; }
            set
            {
                if (_docLanguage != value)
                {
                    _docLanguage = value;
                    OnPropertyChanged("DocLanguage");
                }
            }
        }

        [Name(typeof(CoreStrings), "ExcludedIdentifiers")]
        public ObservableCollection<string> ExcludedIdentifiers
        {
            get { return _excludedIdentifiers ?? (_excludedIdentifiers = new ObservableCollection<string>()); }
            set
            {
                _excludedIdentifiers = value;
                if (_excludedIdentifiers != null)
                    _excludedIdentifiers.CollectionChanged += (s, a) => OnPropertyChanged("ExcludedIdentifiers");
                OnPropertyChanged("ExcludedIdentifiers");
            }
        }

        [ConfigEditor(EditorType.CheckBoxList)]
        [Name(typeof(CoreStrings), "Exporters")]
        public ObservableCollection<string> ActivatedExporters
        {
            get { return _activatedExporters ?? (_activatedExporters = new ObservableCollection<string>()); }
            set
            {
                _activatedExporters = value;
                if (_activatedExporters != null)
                    _activatedExporters.CollectionChanged += (s, a) => OnPropertyChanged("ActivatedExporters");
                OnPropertyChanged("ActivatedExporters");
            }
        }

        public Guid Guid
        {
            get { return new Guid("FEACBCE2-8290-4D90-BB05-373B9D7DBBFC"); }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}