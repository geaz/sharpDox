using System.IO;

namespace SharpDox.Core
{
    public class SDPath
    {
        public SDPath()
        {
            
        }

        public SDPath(string fullPath, string relativePath)
            : this()
        {
            FullPath = fullPath;
            RelativePath = relativePath;
        }

        public string RelativePath { get; private set; }

        public string FullPath { get; private set; }

        public string ResolvePath(string currentDirectory, bool checkForExistence = false)
        {
            if (!string.IsNullOrWhiteSpace(RelativePath))
            {
                var relativePathToResolve = Path.Combine(currentDirectory, RelativePath);
                var relativeResolvedPath = Path.GetFullPath(relativePathToResolve);

                if (checkForExistence)
                {
                    if (!Directory.Exists(relativeResolvedPath) && !File.Exists(relativeResolvedPath))
                    {
                        relativeResolvedPath = null;
                    }
                }

                if (relativeResolvedPath != null)
                {
                    return relativeResolvedPath;
                }
            }

            return FullPath;
        }

        public void UpdatePath(string fullPath, string basePath)
        {
            FullPath = fullPath;
            RelativePath = PathHelper.GetRelativePath(fullPath, basePath);
        }
    }
}
