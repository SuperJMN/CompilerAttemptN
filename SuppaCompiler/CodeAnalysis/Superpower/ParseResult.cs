using System.Collections.Generic;
using SuppaCompiler.CodeAnalysis.Binding;
using SuppaCompiler.CodeAnalysis.Syntax;

namespace SuppaCompiler.CodeAnalysis.Superpower
{
    public class ParseResult
    {
        public SyntaxNode Syntax { get; }
        public IEnumerable<Diagnostic> Diagnostics { get; }

        public ParseResult(SyntaxNode syntax, IEnumerable<Diagnostic> diagnostics)
        {
            Syntax = syntax;
            Diagnostics = diagnostics;
        }

        public ParseResult(SyntaxNode syntax) : this(syntax, new List<Diagnostic>())
        {
            Syntax = syntax;
        }
    }
}