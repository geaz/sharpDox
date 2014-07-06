using System.Windows;
using System.Windows.Input;
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

            MouseLeftButtonDown += OnMouseDown;
            MouseLeftButtonUp += OnMouseUp;
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // this prevents win7 aerosnap
                if (ResizeMode != System.Windows.ResizeMode.NoResize)
                {
                    ResizeMode = System.Windows.ResizeMode.NoResize;
                    UpdateLayout();
                }

                DragMove();
            }
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (ResizeMode == System.Windows.ResizeMode.NoResize)
            {
                // restore resize grips
                ResizeMode = System.Windows.ResizeMode.CanResizeWithGrip;
                UpdateLayout();
            }
        }

        public SDGuiStrings Strings { get; private set; }
    }
}