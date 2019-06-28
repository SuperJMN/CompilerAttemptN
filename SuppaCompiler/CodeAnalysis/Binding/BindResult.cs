using System.Collections.Generic;

namespace SuppaCompiler.CodeAnalysis.Binding
{
    public class BindResult
    {
        public BindResult(BoundExpression boundExpression) : this(boundExpression, new List<Diagnostic>())
        {
        }

        public BindResult(IEnumerable<Diagnostic> diagnostics) : this(new InvalidBoundExpression(), diagnostics)
        {
        }

        private BindResult(BoundExpression boundExpression, IEnumerable<Diagnostic> diagnostics)
        {
            BoundExpression = boundExpression;
            Diagnostics = diagnostics;
        }

        public BoundExpression BoundExpression { get; }
        public IEnumerable<Diagnostic> Diagnostics { get; }
    }
}