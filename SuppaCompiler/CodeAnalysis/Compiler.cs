using System.Collections.Generic;
using System.Linq;
using SuppaCompiler.CodeAnalysis.Binding;
using SuppaCompiler.CodeAnalysis.Superpower;

namespace SuppaCompiler.CodeAnalysis
{
    public class Compiler
    {
        public CompileResult Compile(string source, Dictionary<string, Symbol> symbols)
        {
            var parser = new Parser();
            var parseResult = parser.Parse(source);
            var binder = new Binder(symbols);

            if (!parseResult.Diagnostics.Any())
            {
                var boundExpression =  binder.BindExpression(parseResult.Syntax);
                return new CompileResult(boundExpression);
            }

            return new CompileResult(parseResult.Diagnostics);
        }
    }
}