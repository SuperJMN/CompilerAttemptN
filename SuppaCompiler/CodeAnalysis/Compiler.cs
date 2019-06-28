using System.Collections.Generic;
using System.Linq;
using SuppaCompiler.CodeAnalysis.Binding;
using SuppaCompiler.CodeAnalysis.Superpower;

namespace SuppaCompiler.CodeAnalysis
{
    public class Compiler
    {
        public CompileResult Compile(string source, Scope symbols)
        {
            var parser = new Parser();
            var parseResult = parser.Parse(source);
            var binder = new Binder(symbols);

            if (!parseResult.Diagnostics.Any())
            {
                var bindResult =  binder.Bind(parseResult.Syntax);
                if (!bindResult.Diagnostics.Any())
                {
                    return new CompileResult(bindResult.BoundExpression);
                }

                return new CompileResult(bindResult.Diagnostics);
            }

            return new CompileResult(parseResult.Diagnostics);
        }
    }
}