using System.IO;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.TypeSystem;

namespace SharpDox.Build.NRefactory.Loader
{
    internal class CSharpFile
    {
        public CSharpFile(CSharpProject project, string fileName)
        {          
            Project = project;
            FileName = fileName;
            OriginalText = File.ReadAllText(fileName, Encoding.Default);

            var p = new CSharpParser(project.CompilerSettings);
            SyntaxTree = p.Parse(OriginalText, fileName);

            UnresolvedTypeSystemForFile = SyntaxTree.ToTypeSystem();
            LinesOfCode = 1 + OriginalText.Count(c => c == '\n');
        }

        public CSharpProject Project {get; private set; }
        public string FileName { get; private set; }
        public string OriginalText { get; private set; }
        public SyntaxTree SyntaxTree { get; private set; }
        public CSharpUnresolvedFile UnresolvedTypeSystemForFile { get; private set; }
        public int LinesOfCode { get; private set; }
    }
}
