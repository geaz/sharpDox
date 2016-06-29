using System;
using System.ComponentModel.Design;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace SharpDox.Vsix
{
    internal sealed class BuildCommand
    {
        public const int CommandId = 0x0100;
        public const int CommandId2 = 0x0110;

        public static readonly Guid CommandSet = new Guid("27c4f19a-58b2-463b-ae61-caeab6d13a03");
        
        private readonly Package _package;
        
        private BuildCommand(Package package)
        {
            if (package == null) throw new ArgumentNullException(nameof(package));
            _package = package;

            var commandService = ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandId = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(MenuItemCallback, menuCommandId);
                commandService.AddCommand(menuItem);

                menuCommandId = new CommandID(CommandSet, CommandId2);
                menuItem = new MenuCommand(MenuItemCallback, menuCommandId);
                commandService.AddCommand(menuItem);
            }
        }
        
        public static void Initialize(Package package)
        {
            Instance = new BuildCommand(package);
        }
        
        private void MenuItemCallback(object sender, EventArgs e)
        {
            var dte = ServiceProvider.GetService(typeof(SDTE)) as DTE2;
            var outputController = new OutputController(dte);



            outputController.WriteOutput(!string.IsNullOrEmpty(dte.Solution.FileName) ? dte.Solution.FileName : "NO SOLUTION");
        }

        public static BuildCommand Instance { get; private set; }

        private IServiceProvider ServiceProvider => _package;
    }
}
