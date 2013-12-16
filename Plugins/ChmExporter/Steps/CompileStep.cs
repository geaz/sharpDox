using System.IO;
using System.Diagnostics;

namespace SharpDox.Plugins.Chm.Steps
{
    internal class CompileStep : Step
    {
        public override void ProcessStep(ChmExporter chmExporter)
        {
            chmExporter.ExecuteOnStepProgress(70);
            chmExporter.ExecuteOnStepMessage(chmExporter.ChmStrings.Compiling);

            var p = new Process();
            p.StartInfo.FileName = Path.Combine(chmExporter.TmpPath, "hhc.exe");
            p.StartInfo.Arguments = "\"" + Path.Combine(chmExporter.TmpPath, chmExporter.Repository.ProjectInfo.ProjectName.Replace(" ", "") + ".hhp") + "\"";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();

            p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            chmExporter.CurrentStep = new SaveAndCleanStep();
        }
    }
}
