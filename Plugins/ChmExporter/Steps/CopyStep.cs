using System.IO;
using System.Collections.Generic;

namespace SharpDox.Plugins.Chm.Steps
{
    internal class PreBuildStep : Step
    {
        public override void ProcessStep(ChmExporter chmExporter)
        {
            chmExporter.ExecuteOnStepProgress(0);
            chmExporter.ExecuteOnStepMessage(chmExporter.ChmStrings.Start);

            var contentPath = Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), "content");
            CopyContentToTmp(chmExporter.Repository.Images, chmExporter.TmpPath, contentPath);

            chmExporter.CurrentStep = new TemplateStep();
        }

        private void CopyContentToTmp(IEnumerable<string> images, string tmpFilepath, string contentPath)
        {
            Directory.CreateDirectory(tmpFilepath);
            Directory.CreateDirectory(Path.Combine(tmpFilepath, "diagrams"));
            CopyDirectoryRecursive(contentPath, tmpFilepath);
            CopyImages(images, tmpFilepath);
        }

        private void CopyDirectoryRecursive(string dirToCopy, string copyToLocation)
        {
            var files = Directory.EnumerateFiles(dirToCopy);
            foreach (var file in files)
            {
                File.Copy(file, Path.Combine(copyToLocation, Path.GetFileName(file)), true);
            }

            foreach (var dir in Directory.EnumerateDirectories(dirToCopy))
            {
                if (!Directory.Exists(Path.Combine(copyToLocation, Path.GetFileName(dir))))
                {
                    Directory.CreateDirectory(Path.Combine(copyToLocation, Path.GetFileName(dir)));
                }

                CopyDirectoryRecursive(dir, Path.Combine(copyToLocation, Path.GetFileName(dir)));
            }
        }

        private void CopyImages(IEnumerable<string> images, string tmpFilepath)
        {
            foreach (var image in images)
            {
                File.Copy(image, Path.Combine(tmpFilepath, Path.GetFileName(image)), true);
            }
        }
    }
}
