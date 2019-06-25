using System.Collections.Generic;
using System.Linq;
using SuppaCompiler.CodeAnalysis.Binding;

namespace SuppaCompiler.CodeAnalysis.Syntax
{
    public sealed class SyntaxTree
    {
        public SyntaxTree(IEnumerable<Diagnostic> diagnostics, ExpressionSyntax root)
        {
            Diagnostics = diagnostics.ToArray();
            Root = root;
        }

        public SyntaxTree(ExpressionSyntax root) : this(new List<Diagnostic>(), root)
        {
        }

        public IReadOnlyList<Diagnostic> Diagnostics { get; }
        public ExpressionSyntax Root { get; }
    }
}