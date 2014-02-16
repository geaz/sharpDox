using System;
using System.Linq;
using System.Windows;
using SharpDox.GUI.Controls.ConfigGrid;
using SharpDox.GUI.ViewModels;
using SharpDox.Local;
using SharpDox.Sdk.Config;
using SharpDox.Sdk.Exporter;
using SharpDox.Sdk.UI;
using SharpDox.Sdk.Build;

namespace SharpDox.GUI
{
    public partial class Shell : IShell
    {
        public event Action OnClose;

        public Shell(IConfigController configController, IExporter[] allExporters, LocalController localController, IBuildController buildController)
        {
            var guiStrings = localController.GetLocalStrings<SDGuiStrings>();
            DataContext = new ShellViewModel(guiStrings, configController, buildController, ExecuteOnClose);
            Strings = guiStrings;

            InitializeComponent();

            svBody.Content = new ConfigGridControl(configController, allExporters, localController, buildController);

            MouseLeftButtonDown += (s, a) => DragMove();
        }

        public void Start(string[] args)
        {
            new Application();
            ShowDialog();
        }

        private void ExecuteOnClose()
        {
            if (OnClose != null) OnClose();
            Close();
        }

        public SDGuiStrings Strings { get; private set; }
        public bool IsGui { get { return true; } }
    }
}