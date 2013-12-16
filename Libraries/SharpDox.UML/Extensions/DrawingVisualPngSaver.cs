using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SharpDox.UML
{
    internal static class DrawingVisualPngSaver
    {
        public static void SaveAsPng(this DrawingVisual visual, string outputFilepath)
        {
            RenderTargetBitmap target = null;

            if (visual.ContentBounds.Height > 4000 || visual.ContentBounds.Width > 4000)
            {
                var scale = 0.99;
                var correctScaleFound = false;
                while (!correctScaleFound)
                {
                    if (visual.ContentBounds.Height*scale < 4000 && visual.ContentBounds.Width*scale < 4000)
                    {
                        correctScaleFound = true;
                    }
                    else
                    {
                        scale -= 0.01;
                    }
                }
                visual.Transform = new ScaleTransform(scale, scale);
                target = new RenderTargetBitmap(
                    (int)(visual.ContentBounds.Width * scale), 
                    (int)(visual.ContentBounds.Height * scale),
                    96, 96, PixelFormats.Pbgra32);
            }
            else
            {
                target = new RenderTargetBitmap(
                    (int) visual.ContentBounds.Width, 
                    (int) visual.ContentBounds.Height,
                    96, 96, PixelFormats.Pbgra32);
            }

            target.Render(visual);

            var encoder = new PngBitmapEncoder();
            var outputFrame = BitmapFrame.Create(target);
            encoder.Frames.Add(outputFrame);

            using (var file = File.OpenWrite(outputFilepath))
            {
                encoder.Save(file);
            }
        }
    }
}
