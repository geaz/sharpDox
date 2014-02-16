using System;
using System.Windows;
using SharpDox.GUI.Controls.ConfigGrid;
using SharpDox.GUI.ViewModels;
using SharpDox.Local;
using SharpDox.Sdk.Config;
using SharpDox.Sdk.Exporter;
using SharpDox.Sdk.UI;
using SharpDox.Sdk.Build;

namespace SharpDox.GUI.Windows
{
    public partial class VisibilityEditorView : Window
    {
        public VisibilityEditorView(SDGuiStrings strings, ICoreConfigSection sharpdoxConfig, IBuildController buildController)
        {
            Strings = strings;

            DataContext = new VisibilityViewModel(sharpdoxConfig, buildController, Hide);
            InitializeComponent();

            MouseLeftButtonDown += (s, a) => DragMove();
        }

        public SDGuiStrings Strings { get; private set; }
    }
}