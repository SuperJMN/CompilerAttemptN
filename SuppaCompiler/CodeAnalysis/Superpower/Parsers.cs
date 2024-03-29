﻿using System.Collections.Generic;
using Superpower;
using Superpower.Parsers;
using SuppaCompiler.CodeAnalysis.Syntax;
using TokenParser = Superpower.TokenListParser<SuppaCompiler.CodeAnalysis.Syntax.SyntaxKind, SuppaCompiler.CodeAnalysis.Syntax.SyntaxToken>;
using ExpressionParser = Superpower.TokenListParser<SuppaCompiler.CodeAnalysis.Syntax.SyntaxKind, SuppaCompiler.CodeAnalysis.Syntax.ExpressionSyntax>;

namespace SuppaCompiler.CodeAnalysis.Superpower
{
    public static class Parsers
    {
        private static readonly TokenParser Addition = Token.EqualTo(SyntaxKind.PlusToken).Select(ParserExtensions.ToSyntaxToken);
        private static readonly TokenParser Subtraction = Token.EqualTo(SyntaxKind.MinusToken).Select(ParserExtensions.ToSyntaxToken);
        private static readonly TokenParser Multiplication = Token.EqualTo(SyntaxKind.StarToken).Select(ParserExtensions.ToSyntaxToken);
        private static readonly TokenParser Division = Token.EqualTo(SyntaxKind.SlashToken).Select(ParserExtensions.ToSyntaxToken);
        private static readonly TokenParser And = Token.EqualTo(SyntaxKind.AmpersandAmpersandToken).Select(ParserExtensions.ToSyntaxToken);
        private static readonly TokenParser Or = Token.EqualTo(SyntaxKind.PipePipeToken).Select(ParserExtensions.ToSyntaxToken);
        private static readonly TokenParser Equal = Token.EqualTo(SyntaxKind.EqualsEqualsToken).Select(ParserExtensions.ToSyntaxToken);
        private static readonly TokenParser Assign = Token.EqualTo(SyntaxKind.EqualsToken).Select(ParserExtensions.ToSyntaxToken);
        private static readonly TokenParser NotEqual = Token.EqualTo(SyntaxKind.BangEqualsToken).Select(ParserExtensions.ToSyntaxToken);
        private static readonly TokenParser Negation = Token.EqualTo(SyntaxKind.BangToken).Select(ParserExtensions.ToSyntaxToken);

        private static readonly ExpressionParser Identifier = Token.EqualTo(SyntaxKind.IdentifierToken).Select(x => (ExpressionSyntax)new NameExpressionSyntax(x.ToSyntaxToken()));
        private static readonly ExpressionParser True = Token.EqualTo(SyntaxKind.TrueKeyword).Select(x => x.ToSyntaxToken().ToLiteral(x.Kind == SyntaxKind.TrueKeyword));
        private static readonly ExpressionParser False = Token.EqualTo(SyntaxKind.FalseKeyword).Select(x => x.ToSyntaxToken().ToLiteral(x.Kind == SyntaxKind.TrueKeyword));
        private static readonly ExpressionParser BooleanValue = True.Or(False);
        private static readonly ExpressionParser Number = Token.EqualTo(SyntaxKind.NumberToken)
            .Select(x =>
            {
                var value = Numerics.IntegerInt32(x.Span).Value;
                return x.ToSyntaxToken(value).ToLiteral(value);
            });

        private static readonly ExpressionParser Item = BooleanValue.Or(Number).Or(Identifier);
        
        private static readonly ExpressionParser ParenthesizedExpresion = Parse.Ref(() => Expression.Between(Token.EqualTo(SyntaxKind.OpenParenthesisToken), Token.EqualTo(SyntaxKind.CloseParenthesisToken)));
        private static readonly ExpressionParser Factor = ParenthesizedExpresion.Or(Item);
        private static readonly ExpressionParser Operand =
            (from op in Subtraction.Or(Addition).Or(Negation)
                from factor in Factor
                select (ExpressionSyntax)new UnaryExpressionSyntax(op, factor)).Or(Factor).Named("expression");

        private static readonly ExpressionParser Multiplicand = Parse.Chain(Multiplication.Or(Division), Operand, ParserExtensions.ToBinary);
        private static readonly ExpressionParser Addend = Parse.Chain(Addition.Or(Subtraction), Multiplicand, ParserExtensions.ToBinary);
        private static readonly ExpressionParser Disjunction = Parse.Chain(And.Or(Or), Addend, ParserExtensions.ToBinary);
        private static readonly ExpressionParser Equality = Parse.Chain(Equal.Or(NotEqual), Disjunction, ParserExtensions.ToBinary);

        private static readonly ExpressionParser Assignment = Parse.ChainRight(Assign, Equality,
            (op, left, right) => new AssignmentExpressionSyntax((NameExpressionSyntax) left, op, right));

        public static readonly ExpressionParser Expression = Assignment;
    }
}