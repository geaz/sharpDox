using SharpDox.GUI.ViewModels;
using SharpDox.Sdk.Config;
using SharpDox.Sdk.Exporter;
using SharpDox.Sdk.UI;

namespace SharpDox.GUI.Pages
{
    public partial class ExporterSelection : IPage
    {
        public ExporterSelection(SDGuiStrings strings, SharpDoxConfig sharpdoxConfig, IExporter[] allExporters)
        {
            Strings = strings;

            DataContext = new ExporterViewModel(sharpdoxConfig, allExporters);
            InitializeComponent();
        }

        public SDGuiStrings Strings { get; private set; }
        public string PageName { get { return Strings.ExporterSelection; } }
        public new int Width { get { return int.Parse(mainGrid.Width.ToString()); } }
        public new int Height { get { return int.Parse(mainGrid.Height.ToString()); } }
    }
}
