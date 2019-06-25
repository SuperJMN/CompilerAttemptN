using Superpower;
using Superpower.Model;
using SuppaCompiler.CodeAnalysis.Binding;
using SuppaCompiler.CodeAnalysis.Syntax;

namespace SuppaCompiler.CodeAnalysis.Superpower
{
    public static class Parser
    {
        public static SyntaxTree Parse(string line)
        {
            try
            {
                var expr = Parsers.Expression.Parse(Tokenizer.Create().Tokenize(line));
                return new SyntaxTree(expr);
            }
            catch (ParseException e)
            {
                var textSpan = new TextSpan(line, e.ErrorPosition, 1);
                var diagnostic = new Diagnostic(textSpan, e.Message);
                return new SyntaxTree(new [] { diagnostic }, new InvalidParseTree());
            }
        }
    }
}