using System.Collections.Generic;
using Superpower;
using Superpower.Model;
using Superpower.Parsers;
using SuppaCompiler.Console.CodeAnalysis.Syntax;
using SyntaxTokenParser = Superpower.TokenListParser<SuppaCompiler.Console.CodeAnalysis.Syntax.SyntaxKind, SuppaCompiler.Console.CodeAnalysis.Syntax.SyntaxToken>;
using Parser = Superpower.TokenListParser<SuppaCompiler.Console.CodeAnalysis.Syntax.SyntaxKind, SuppaCompiler.Console.CodeAnalysis.Syntax.ExpressionSyntax>;

namespace SuppaCompiler.Console.CodeAnalysis.Superpower
{
    public static class Parsers
    {
        private static readonly TokenListParser<SyntaxKind, ExpressionSyntax> Number = Token.EqualTo(SyntaxKind.NumberToken)
            .Select(x => (ExpressionSyntax) new LiteralExpressionSyntax(ToSyntaxToken(x), Numerics.IntegerInt32(x.Span).Value));

        private static readonly SyntaxTokenParser Addition = Token.EqualTo(SyntaxKind.PlusToken).Select(ToSyntaxToken);
        private static readonly SyntaxTokenParser Subtraction = Token.EqualTo(SyntaxKind.MinusToken).Select(ToSyntaxToken);
        private static readonly SyntaxTokenParser Multiplication = Token.EqualTo(SyntaxKind.StarToken).Select(ToSyntaxToken);
        private static readonly SyntaxTokenParser Division = Token.EqualTo(SyntaxKind.SlashToken).Select(ToSyntaxToken);

        private static readonly Parser ParenthesizedExpresion = Parse.Ref(() => Expression.Between(Token.EqualTo(SyntaxKind.OpenParenthesisToken), Token.EqualTo(SyntaxKind.CloseParenthesisToken)));

        private static readonly Parser Operand = ParenthesizedExpresion.Or(Number);
        private static readonly Parser Multiplicand = Parse.Chain(Multiplication.Or(Division), Operand, ToBinary);
        private static readonly Parser Addend = Parse.Chain(Addition.Or(Subtraction), Multiplicand, ToBinary);
        private static readonly Parser Expression = from unaryOp in Subtraction.Or(Addition).OptionalOrDefault()
            from expr in Addend
            select unaryOp != null ? new UnaryExpressionSyntax(unaryOp, expr) : expr;

        public static readonly TokenListParser<SyntaxKind, SyntaxTree> Tree = Expression.Select(x => new SyntaxTree(new List<string>(), x, null));

        private static SyntaxToken ToSyntaxToken(Token<SyntaxKind> token)
        {
            return new SyntaxToken(token.Kind, token.Position.Absolute, token.Span.Source, null);
        }

        private static ExpressionSyntax ToBinary(SyntaxToken op, ExpressionSyntax left, ExpressionSyntax right)
        {
            return new BinaryExpressionSyntax(left, op, right);
        }
    }
}