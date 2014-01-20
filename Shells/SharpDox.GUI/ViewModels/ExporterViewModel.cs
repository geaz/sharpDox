using System;
using System.Collections.ObjectModel;
using System.Net.Mime;
using System.Windows;
using System.Windows.Threading;
using SharpDox.Sdk.Config;
using SharpDox.Sdk.Exporter;

namespace SharpDox.GUI.ViewModels
{
    internal class ExporterViewModel : ViewModelBase
    {
        private readonly SharpDoxConfig _sharpDoxConfig;
        private readonly IExporter[] _allExporter;

        public ExporterViewModel(SharpDoxConfig sharpDoxConfig, IExporter[] allExporters)
        {
            _sharpDoxConfig = sharpDoxConfig;
            _allExporter = allExporters;

            RunRefresh();
            sharpDoxConfig.PropertyChanged += (s, a) => RunRefresh();
        }

        private void RunRefresh()
        {
            if (Application.Current != null)
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(RefreshListBox));
            }
            else
            {
                RefreshListBox();
            }
        }

        private void RefreshListBox()
        {
            ExporterList.Clear();
            foreach (var exporter in _allExporter)
            {
                var item = new ExporterListBoxItemViewModel(exporter.ExporterName, exporter.Author,
                    exporter.Description, _sharpDoxConfig.DeactivatedExporters);
                ExporterList.Add(item);
            }
        }

        private ObservableCollection<ExporterListBoxItemViewModel> _exporterList = new ObservableCollection<ExporterListBoxItemViewModel>();
        public ObservableCollection<ExporterListBoxItemViewModel> ExporterList
        {
            get { return _exporterList; }
            set { _exporterList = value; OnPropertyChanged("ExporterList"); }
        }
    }
}
