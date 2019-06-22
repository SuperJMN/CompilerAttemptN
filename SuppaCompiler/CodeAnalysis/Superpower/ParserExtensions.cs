using Superpower.Model;
using SuppaCompiler.CodeAnalysis.Syntax;

namespace SuppaCompiler.CodeAnalysis.Superpower
{
    public static class ParserExtensions 
    {
        public static SyntaxToken ToToken(this Token<SyntaxKind> token)
        {
            return ToSyntaxToken(token, null);
        }

        public static SyntaxToken ToSyntaxToken(this Token<SyntaxKind> token, object value)
        {
            return new SyntaxToken(token, value);
        }

        public static ExpressionSyntax ToBinary(SyntaxToken op, ExpressionSyntax left, ExpressionSyntax right)
        {
            return new BinaryExpressionSyntax(left, op, right);
        }

        public static ExpressionSyntax ToLiteral(this SyntaxToken toSyntaxToken, object value)
        {
            return new LiteralExpressionSyntax(toSyntaxToken, value);
        }
    }
}