using Superpower.Model;

namespace SuppaCompiler.CodeAnalysis.Binding
{
    public class Diagnostic
    {
        public TextSpan Span { get; }
        public string Message { get; }

        public Diagnostic(TextSpan span, string message)
        {
            Span = span;
            Message = message;
        }

        public override string ToString()
        {
            return $"{Span.Position}: {Message}";
        }
    }
}