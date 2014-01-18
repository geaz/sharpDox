using System.Collections.Generic;

namespace SharpDox.GUI.ViewModels
{
    internal class ExporterListBoxItemViewModel
    {
        private readonly ICollection<string> _deactivatedExporters;

        public ExporterListBoxItemViewModel(string exporterName, string author, string description, ICollection<string> deactivatedExporters)
        {
            Name = exporterName;
            Author = author;
            Description = description;

            _deactivatedExporters = deactivatedExporters;
            IsChecked = !deactivatedExporters.Contains(Name);
        }

        public string Name { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }

        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (value != _isChecked)
                {
                    _isChecked = value;
                    if (_isChecked && _deactivatedExporters.Contains(Name))
                    {
                        _deactivatedExporters.Remove(Name);
                    }
                    else if (!_isChecked && !_deactivatedExporters.Contains(Name))
                    {
                        _deactivatedExporters.Add(Name);
                    }
                }
            }
        }
    }
}
