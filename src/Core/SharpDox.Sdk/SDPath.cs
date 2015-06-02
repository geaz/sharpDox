using System;
using System.IO;

namespace SharpDox.Sdk
{
    public class SDPath
    {
        public SDPath()
        {
            
        }

        public SDPath(string fullPath, string relativePath = null)
            : this()
        {
            FullPath = fullPath;
            RelativePath = relativePath;
        }

        public string RelativePath { get; private set; }

        public string FullPath { get; private set; }

        public string ResolvePath()
        {
            return ResolvePath(Environment.CurrentDirectory);
        }

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

        public void UpdatePath()
        {
            UpdatePath(FullPath, Environment.CurrentDirectory);
        }

        public void UpdatePath(string fullPath, string basePath)
        {
            FullPath = fullPath;
            RelativePath = PathHelper.GetRelativePath(fullPath, basePath);
        }

        public static implicit operator string (SDPath path)
        {
            return path.ResolvePath();
        }

        public static implicit operator SDPath (string fullPath)
        {
            return new SDPath(fullPath);
        }
    }
}
