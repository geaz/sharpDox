using System;
using System.ComponentModel;
using SharpDox.Sdk.Local;
using System.Collections.ObjectModel;

namespace SharpDox.Sdk.Config
{
    /// <default>
    ///     <summary>
    ///     All core configuration items of sharpDox.
    ///     </summary>
    /// </default>
    /// <de>
    ///     <summary>
    ///     Alle Basis-Einstellungen für sharpDox.
    ///     </summary>
    /// </de>
    public class SharpDoxConfig : IConfigSection
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isSaved;
        private string _author;
        private string _configFileName;
        private string _description;
        private string _inputPath;
        private string _lastBuild;
        private string _logoPath;
        private string _outputPath;
        private string _docLanguage;
        private string _pathToConfig;
        private string _projectName;
        private string _versionNumber;
        private ObservableCollection<string> _excludedIdentifiers;
        private ObservableCollection<string> _deactivatedExporters;

        private readonly SharpDoxStrings _strings;

        public SharpDoxConfig(SharpDoxStrings strings)
        {
            _strings = strings;
        }

        /// <default>
        ///     <summary>
        ///     Returns whether the actual configuration is saved or not.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert einen Wert der angibt, ob die Konfiguration gespeichert ist.
        ///     </summary>
        /// </de>
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

        /// <default>
        ///     <summary>
        ///     Returns the name of the configuration file.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert den Namen der Konfigurationsdatei.
        ///     </summary>
        /// </de>
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

        /// <default>
        ///     <summary>
        ///     Return the path to the actual configuration.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert den Pfad zur aktuellen Konfigutaionsdatei.
        ///     </summary>
        /// </de>
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

        /// <default>
        ///     <summary>
        ///     Returns the date of the last documentation build.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert das Datum des letzten Bauvorgangs.
        ///     </summary>
        /// </de>
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

        /// <default>
        ///     <summary>
        ///     Returns the project name.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert den Namen des Projekts.
        ///     </summary>
        /// </de>
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

        /// <default>
        ///     <summary>
        ///     Returns the version number.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert die Versionsnummer.
        ///     </summary>
        /// </de>
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

        /// <default>
        ///     <summary>
        ///     Returns the author.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert den Autoren.
        ///     </summary>
        /// </de>
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

        /// <default>
        ///     <summary>
        ///     Returns the description.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert die Beschreibung.
        ///     </summary>
        /// </de>
        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        /// <default>
        ///     <summary>
        ///     Returns the path to the logo.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert den Pfad zum Logo.
        ///     </summary>
        /// </de>
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

        /// <default>
        ///     <summary>
        ///     Returns the input path.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert den Eingabepfad.
        ///     </summary>
        /// </de>
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

        /// <default>
        ///     <summary>
        ///     Returns all exluded namespaces.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert alle ausgeschlossenen Namensräume.
        ///     </summary>
        /// </de>
        public ObservableCollection<string> ExcludedIdentifiers
        {
            get { return _excludedIdentifiers ?? (_excludedIdentifiers = new ObservableCollection<string>()); }
            set
            {
                _excludedIdentifiers = value; 
                if(_excludedIdentifiers != null) _excludedIdentifiers.CollectionChanged += (s, a) => OnPropertyChanged("ExcludedIdentifiers");
                OnPropertyChanged("ExcludedIdentifiers");
            }
        }

        /// <default>
        ///     <summary>
        ///     Returns all deactivated exporters.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert alle deaktivierten Exporter.
        ///     </summary>
        /// </de>
        public ObservableCollection<string> DeactivatedExporters
        {
            get { return _deactivatedExporters ?? (_excludedIdentifiers = new ObservableCollection<string>()); }
            set
            {
                _deactivatedExporters = value;
                if (_deactivatedExporters != null) _deactivatedExporters.CollectionChanged += (s, a) => OnPropertyChanged("DeactivatedExporters");
                OnPropertyChanged("DeactivatedExporters");
            }
        }

        /// <default>
        ///     <summary>
        ///     Return the output path.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert den Ausgabepfad.
        ///     </summary>
        /// </de>
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

        /// <default>
        ///     <summary>
        ///     Returns the default documentation language.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert die standard Dokumentationssprache.
        ///     </summary>
        /// </de>
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

        /// <default>
        ///     <summary>
        ///     Returns the guid of the core configuration section.
        ///     </summary>
        /// </default>
        /// <de>
        ///     <summary>
        ///     Liefert die GUID der sharpDox Konfiguration.
        ///     </summary>
        /// </de>
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