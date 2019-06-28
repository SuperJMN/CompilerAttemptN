using System.Collections.Generic;
using System.Linq;
using SuppaCompiler.CodeAnalysis.Binding;
using SuppaCompiler.CodeAnalysis.Syntax;

namespace SuppaCompiler.CodeAnalysis
{
    public class Compilation
    {
        public SyntaxTree Syntax { get; }

        public Compilation(SyntaxTree syntax)
        {
            Syntax = syntax;
        }

        public EvaluationResult Evaluate(Scope scope)
        {
            var binder = new Binder(scope);
            var boundExpression = binder.BindExpression(Syntax.Root);
            var evaluator = new Evaluator(boundExpression, scope);

            var diagnostics = Syntax.Diagnostics.Concat(binder.Diagnostics).ToArray();
            if (diagnostics.Any())
            {
                return new EvaluationResult(diagnostics, null);
            }

            var value = evaluator.Evaluate();
            return new EvaluationResult(diagnostics, value);
        }
    }
}