using System.Collections.Generic;
using SuppaCompiler.CodeAnalysis.Binding;

namespace SuppaCompiler.CodeAnalysis
{
    public class CompileResult
    {
        public BoundExpression BoundExpression { get; }
        public IEnumerable<Diagnostic> Diagnostics { get; }

        public CompileResult(BoundExpression boundExpression) : this(boundExpression, new List<Diagnostic>())
        {
            BoundExpression = boundExpression;
        }

        public CompileResult(BoundExpression boundExpression, IEnumerable<Diagnostic> diagnostics)
        {
            BoundExpression = boundExpression;
            Diagnostics = diagnostics;
        }

        public CompileResult(IEnumerable<Diagnostic> diagnostics) : this(new InvalidBoundExpression(), diagnostics)
        {
        }
    }
}