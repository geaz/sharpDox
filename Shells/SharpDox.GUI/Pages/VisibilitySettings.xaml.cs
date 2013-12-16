using SharpDox.GUI.ViewModels;
using SharpDox.Sdk.Build;
using SharpDox.Sdk.Config;
using SharpDox.Sdk.Local;
using SharpDox.Sdk.UI;

namespace SharpDox.GUI.Pages
{
    public partial class VisibilitySettings : IPage
    {
        public VisibilitySettings(SDGuiStrings strings, SharpDoxConfig sharpdoxConfig, IBuildController buildController, IBuildMessenger buildMessenger)
        {
            Strings = strings;

            DataContext = new VisibilityViewModel(sharpdoxConfig, buildController, buildMessenger);
            InitializeComponent();
        }

        public SDGuiStrings Strings { get; private set; }
        public string PageName { get { return Strings.VisibilitySettings; } }
        public new int Width { get { return int.Parse(mainGrid.Width.ToString()); } }
        public new int Height { get { return int.Parse(mainGrid.Height.ToString()); } }
    }
}
