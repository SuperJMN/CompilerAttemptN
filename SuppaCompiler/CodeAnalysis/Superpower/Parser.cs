using Superpower;
using Superpower.Model;
using SuppaCompiler.CodeAnalysis.Binding;

namespace SuppaCompiler.CodeAnalysis.Superpower
{
    public class Parser
    {
        public ParseResult Parse(string line)
        {
            try
            {
                var expr = Parsers.Expression.Parse(Tokenizer.Create().Tokenize(line));
                return new ParseResult(expr);
            }
            catch (ParseException e)
            {
                var textSpan = new TextSpan(line, e.ErrorPosition, 1);
                var diagnostic = new Diagnostic(textSpan, e.Message);
                return new ParseResult(new InvalidParseTree(), new [] { diagnostic });
            }
        }
    }
}