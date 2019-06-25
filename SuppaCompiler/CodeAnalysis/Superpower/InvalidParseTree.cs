using System.Collections.Generic;
using SuppaCompiler.CodeAnalysis.Syntax;

namespace SuppaCompiler.CodeAnalysis.Superpower
{
    public class InvalidParseTree : ExpressionSyntax
    {
        public override SyntaxKind Kind => SyntaxKind.Invalid;
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield break;
        }
    }
}