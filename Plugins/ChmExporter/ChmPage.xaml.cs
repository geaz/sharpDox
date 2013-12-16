using SharpDox.Sdk.UI;

namespace SharpDox.Plugins.Chm
{
    public partial class ChmPage : IPage
    {
        private readonly ChmConfig _chmConfig;

        public ChmPage(ChmStrings strings, ChmConfig chmConfig)
        {
            _chmConfig = chmConfig;

            Strings = strings;
            DataContext = chmConfig;

            InitializeComponent();
        }

        private void Reset_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _chmConfig.BackgroundColor = null;
            _chmConfig.TextColor = null;
            _chmConfig.LinkColor = null;
            _chmConfig.LinkHoverColor = null;
            _chmConfig.TableHeaderBackgroundColor = null;
            _chmConfig.TableHeaderBorderColor = null;
            _chmConfig.BreadCrumbBackgroundColor = null;
            _chmConfig.BreadCrumbBorderColor = null;
            _chmConfig.BreadCrumbLinkColor = null;
            _chmConfig.BreadCrumbLinkHoverColor = null;
            _chmConfig.SyntaxBoxBackgroundColor = null;
            _chmConfig.SyntaxBoxBorderColor = null;
            _chmConfig.SyntaxBoxTextColor = null;
        }

        public ChmStrings Strings { get; private set; }
        public new int Height { get { return int.Parse(mainGrid.Height.ToString()); } }
        public new int Width { get { return int.Parse(mainGrid.Width.ToString()); } }
        public string PageName { get { return "Chm"; } }
    }
}
