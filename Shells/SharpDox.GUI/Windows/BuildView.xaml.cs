using System.Windows;
using SharpDox.GUI.ViewModels;
using SharpDox.Sdk.Build;

namespace SharpDox.GUI.Windows
{
    public partial class BuildView : Window
    {
        public BuildView(SDGuiStrings strings, IBuildMessenger buildMessenger)
        {
            Strings = strings;

            DataContext = new BuildViewModel(buildMessenger, Hide);
            InitializeComponent();

            MouseLeftButtonDown += (s, a) => DragMove();
        }

        public SDGuiStrings Strings { get; private set; }
    }
}