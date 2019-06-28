using System;
using System.Collections.ObjectModel;
using Superpower.Model;

namespace SuppaCompiler.CodeAnalysis.Binding
{
    internal class DiagnosticBag : Collection<Diagnostic>
    {
        private void Report(TextSpan span, string message)
        {
            Add(new Diagnostic(span, message));
        }

        public void ReportUndefinedUnaryOperator(TextSpan span, string operatorStr, Type operandType)
        {
            Add(new Diagnostic(span, $"Cannot find operator '{operatorStr}' for type '{operandType}'"));
        }

        public void ReportUndefinedBinaryOperator(TextSpan span, string operatorStr, Type leftType, Type rightType)
        {
            Add(new Diagnostic(span, $"Cannot find operator {operatorStr} for type '{leftType}' and '{rightType}'"));
        }

        public void ReportUndefinedName(TextSpan span, string name)
        {
            Add(new Diagnostic(span, $"Cannot find name '{name}'"));
        }
    }
}