using System.Collections.Generic;

namespace SuppaCompiler.Console.CodeAnalysis.Syntax
{
    internal sealed class Lexer
    {
        private readonly string text;
        private int position;
        private List<string> diagnostics = new List<string>();

        public Lexer(string text)
        {
            this.text = text;
        }

        public IEnumerable<string> Diagnostics => diagnostics;

        private char Current => Peek(0);

        private char Lookahead => Peek(1);

        private char Peek(int offset)
        {
            var index = position + offset;

            if (index >= text.Length)
                return '\0';

            return text[index];
        }

        private void Next()
        {
            position++;
        }

        public SyntaxToken Lex()
        {
            if (position >= text.Length)
                return new SyntaxToken(SyntaxKind.EndOfFileToken, position, "\0", null);

            if (char.IsDigit(Current))
            {
                var start = position;

                while (char.IsDigit(Current))
                    Next();

                var length = position - start;
                var text = this.text.Substring(start, length);
                if (!int.TryParse(text, out var value))
                    diagnostics.Add($"The number {this.text} isn't valid Int32.");

                return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
            }

            if (char.IsWhiteSpace(Current))
            {
                var start = position;

                while (char.IsWhiteSpace(Current))
                    Next();

                var length = position - start;
                var text = this.text.Substring(start, length);
                return new SyntaxToken(SyntaxKind.WhitespaceToken, start, text, null);
            }

            if (char.IsLetter(Current))
            {
                var start = position;

                while (char.IsLetter(Current))
                    Next();

                var length = position - start;
                var text = this.text.Substring(start, length);
                var kind = SyntaxFacts.GetKeywordKind(text);
                return new SyntaxToken(kind, start, text, null);
            }

            switch (Current)
            {
                case '+':
                    return new SyntaxToken(SyntaxKind.PlusToken, position++, "+", null);
                case '-':
                    return new SyntaxToken(SyntaxKind.MinusToken, position++, "-", null);
                case '*':
                    return new SyntaxToken(SyntaxKind.StarToken, position++, "*", null);
                case '/':
                    return new SyntaxToken(SyntaxKind.SlashToken, position++, "/", null);
                case '(':
                    return new SyntaxToken(SyntaxKind.OpenParenthesisToken, position++, "(", null);
                case ')':
                    return new SyntaxToken(SyntaxKind.CloseParenthesisToken, position++, ")", null);
                case '&':
                    if (Lookahead == '&')
                        return new SyntaxToken(SyntaxKind.AmpersandAmpersandToken, position += 2, "&&", null);
                    break;
                case '|':
                    if (Lookahead == '|')
                        return new SyntaxToken(SyntaxKind.PipePipeToken, position += 2, "||", null);
                    break;
                case '=':
                    if (Lookahead == '=')
                        return new SyntaxToken(SyntaxKind.EqualsEqualsToken, position += 2, "==", null);
                    break;
                case '!':
                    if (Lookahead == '=')
                        return new SyntaxToken(SyntaxKind.BangEqualsToken, position += 2, "!=", null);
                    else
                        return new SyntaxToken(SyntaxKind.BangToken, position++, "!", null);
            }

            diagnostics.Add($"ERROR: bad character input: '{Current}'");
            return new SyntaxToken(SyntaxKind.BadToken, position++, text.Substring(position - 1, 1), null);
        }
    }
}