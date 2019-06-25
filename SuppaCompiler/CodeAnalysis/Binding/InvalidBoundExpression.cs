using System;

namespace SuppaCompiler.CodeAnalysis.Binding
{
    internal class InvalidBoundExpression : BoundExpression
    {
        public override BoundNodeKind Kind => BoundNodeKind.Invalid;
        public override Type Type => typeof(object);
    }
}