using Superpower;
using Superpower.Parsers;
using SuppaCompiler.Console.CodeAnalysis.Syntax;

namespace SuppaCompiler.Console.CodeAnalysis.Superpower
{
    public static class Tokenizer
    {
        public static Tokenizer<SyntaxKind> Create()
        {
            return new global::Superpower.Tokenizers.TokenizerBuilder<SyntaxKind>()
                .Ignore(Span.WhiteSpace)
                .Match(Numerics.Natural, SyntaxKind.NumberToken)
                .Match(Character.EqualTo('+'), SyntaxKind.PlusToken)
                .Match(Character.EqualTo('-'), SyntaxKind.MinusToken)
                .Match(Character.EqualTo('*'), SyntaxKind.StarToken)
                .Match(Character.EqualTo('/'), SyntaxKind.SlashToken)
                .Match(Character.EqualTo('('), SyntaxKind.OpenParenthesisToken)
                .Match(Character.EqualTo(')'), SyntaxKind.CloseParenthesisToken)
                .Build();
        }
    }
}
