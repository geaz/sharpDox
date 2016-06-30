using System;
using EnvDTE;
using EnvDTE80;

namespace SharpDox.Vsix.Controllers
{
    internal class OutputController
    {
        private const string PaneName = "sharpDox";

        private readonly Window _vsOutputWindow;
        private readonly OutputWindowPane _outputPane;

        public OutputController(DTE2 dte)
        {
            _vsOutputWindow = dte.Windows.Item(Constants.vsWindowKindOutput);
            var outputWindow = (OutputWindow)_vsOutputWindow.Object;

            foreach (OutputWindowPane pane in outputWindow.OutputWindowPanes)
            {
                if (pane.Name == PaneName)
                {
                    _outputPane = pane;
                    break;
                }
            }
            if(_outputPane == null) _outputPane = outputWindow.OutputWindowPanes.Add(PaneName);
        }

        public void WriteOutputLine(string message)
        {
            _vsOutputWindow.Activate();
            _outputPane.OutputString($"{message}{Environment.NewLine}");
        }

        public void WriteOutput(string message)
        {
            _vsOutputWindow.Activate();
            _outputPane.OutputString(message);
        }
    }
}
