using Superpower;
using Superpower.Parsers;
using SuppaCompiler.CodeAnalysis.Syntax;

namespace SuppaCompiler.CodeAnalysis.Superpower
{
    public static class Tokenizer
    {
        public static Tokenizer<SyntaxKind> Create()
        {
            return new global::Superpower.Tokenizers.TokenizerBuilder<SyntaxKind>()
                .Ignore(Span.WhiteSpace)
                .Match(Numerics.Natural, SyntaxKind.NumberToken)
                .Match(Span.EqualTo("!="), SyntaxKind.BangEqualsToken)
                .Match(Span.EqualTo("=="), SyntaxKind.EqualsEqualsToken)
                .Match(Span.EqualTo("&&"), SyntaxKind.AmpersandAmpersandToken)
                .Match(Span.EqualTo("||"), SyntaxKind.PipePipeToken)
                .Match(Span.EqualTo("true"), SyntaxKind.TrueKeyword)
                .Match(Span.EqualTo("false"), SyntaxKind.FalseKeyword)
                .Match(Character.EqualTo('!'), SyntaxKind.BangToken)
                .Match(Character.EqualTo('+'), SyntaxKind.PlusToken)
                .Match(Character.EqualTo('-'), SyntaxKind.MinusToken)
                .Match(Character.EqualTo('*'), SyntaxKind.StarToken)
                .Match(Character.EqualTo('/'), SyntaxKind.SlashToken)
                .Match(Character.EqualTo('('), SyntaxKind.OpenParenthesisToken)
                .Match(Character.EqualTo(')'), SyntaxKind.CloseParenthesisToken)
                .Match(Span.Regex("\\w*"), SyntaxKind.IdentifierToken)
                .Build();
        }
    }
}
