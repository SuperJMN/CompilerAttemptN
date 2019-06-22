using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SuppaCompiler.CodeAnalysis.Binding;

namespace SuppaCompiler.CodeAnalysis
{
    public sealed class EvaluationResult
    {
        public object Value { get; }

        public EvaluationResult(IEnumerable<Diagnostic> diagnostics, object value)
        {
            Diagnostics = diagnostics.ToList().AsReadOnly();
            Value = value;
        }

        public ReadOnlyCollection<Diagnostic> Diagnostics { get; }
    }
}