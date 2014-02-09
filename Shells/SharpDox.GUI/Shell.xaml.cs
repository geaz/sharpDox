using System;
using System.Windows;
using SharpDox.GUI.Controls.ConfigGrid;
using SharpDox.GUI.ViewModels;
using SharpDox.Local;
using SharpDox.Sdk.Config;
using SharpDox.Sdk.Exporter;
using SharpDox.Sdk.UI;

namespace SharpDox.GUI
{
    public partial class Shell : IShell
    {
        public event Action OnClose;

        public Shell(SDGuiStrings strings, ICoreConfigSection sharpDoxConfig, IExporter[] allExporters, IConfigSection[] configSections, IConfigController configController, LocalController localController)
        {
            DataContext = new ShellViewModel(strings, configController, configSections, sharpDoxConfig, ExecuteOnClose);
            Strings = strings;

            InitializeComponent();

            svBody.Content = new ConfigGridControl(configSections, allExporters, localController);

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