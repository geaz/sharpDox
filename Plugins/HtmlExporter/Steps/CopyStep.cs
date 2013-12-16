using System;
using System.Collections.Generic;
using System.IO;

namespace SharpDox.Plugins.Html.Steps
{
    public class CopyStep : Step
    {
        public override void ProcessStep(HtmlExporter htmlExporter)
        {
            htmlExporter.ExecuteOnStepProgress(40);

            CopyAssets(htmlExporter, Path.Combine(Path.GetDirectoryName(this.GetType().Assembly.Location), "assets"), htmlExporter.OutputPath);
            CopyImages(htmlExporter, htmlExporter.Repository.Images, htmlExporter.OutputPath);
            CopyImage(htmlExporter, htmlExporter.Repository.ProjectInfo.LogoPath, htmlExporter.OutputPath);

            htmlExporter.CurrentStep = new CreateHtmlStep();
        }

        private void CopyAssets(HtmlExporter htmlExporter, string input, string output)
        {
            foreach (var dir in Directory.EnumerateDirectories(input))
            {
                if (!Directory.Exists(Path.Combine(output, Path.GetFileName(dir))))
                {
                    Directory.CreateDirectory(Path.Combine(output, Path.GetFileName(dir)));
                }
                var files = Directory.EnumerateFiles(dir);
                foreach (var file in files)
                {
                    htmlExporter.ExecuteOnStepMessage(string.Format(htmlExporter.HtmlStrings.CopyingFile, Path.GetFileName(file)));
                    File.Copy(file, Path.Combine(output, Path.GetFileName(dir), Path.GetFileName(file)), true);
                }

                CopyAssets(htmlExporter, dir, Path.Combine(output, Path.GetFileName(dir)));
            }
        }

        private void CopyImages(HtmlExporter htmlExporter, IEnumerable<string> images, string outputPath)
        {
            foreach (var image in images)
            {
                CopyImage(htmlExporter, image, Path.Combine(outputPath, "images"));
            }
        }

        private void CopyImage(HtmlExporter htmlExporter, string imagePath, string outputPath)
        {
            if (!String.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
            {
                htmlExporter.ExecuteOnStepMessage(string.Format(htmlExporter.HtmlStrings.CopyingFile, Path.GetFileName(imagePath)));
                File.Copy(imagePath, Path.Combine(outputPath, Path.GetFileName(imagePath)), true);
            }
        }
    }
}
