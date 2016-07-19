using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using SharpDox.Vsix.Commands;
using SharpDox.Vsix.Tools;

namespace SharpDox.Vsix
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(VsixPackage.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideToolWindow(typeof(ConfigWindow))]
    public sealed class VsixPackage : Package
    {
        public const string PackageGuidString = "c163b90a-5213-4848-ba51-26fa88309897";
        
        public VsixPackage()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.
        }
        
        protected override void Initialize()
        {
            BuildCommand.Initialize(this);
            base.Initialize();
            ConfigWindowCommand.Initialize(this);
        }
    }
}
