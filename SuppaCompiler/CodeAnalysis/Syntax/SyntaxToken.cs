using System.Collections.Generic;
using System.Linq;
using Superpower.Model;

namespace SuppaCompiler.CodeAnalysis.Syntax
{
    public sealed class SyntaxToken : SyntaxNode
    {
        public SyntaxToken(Token<SyntaxKind> innerToken, object value)
        {
            InnerToken = innerToken;
            Value = value;
        }

        public override SyntaxKind Kind => InnerToken.Kind;
        public int Position { get; }
        public Token<SyntaxKind> InnerToken { get; }
        public object Value { get; }
        public TextSpan Span { get; set; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            return Enumerable.Empty<SyntaxNode>();
        }
    }
}