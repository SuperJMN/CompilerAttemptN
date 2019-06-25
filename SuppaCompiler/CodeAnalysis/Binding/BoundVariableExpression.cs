using System;

namespace SuppaCompiler.CodeAnalysis.Binding
{
    internal class BoundVariableExpression : BoundExpression
    {
        public string Name { get; }
        public override Type Type { get; }

        public BoundVariableExpression(string name, Type type)
        {
            Name = name;
            Type = type;
        }

        public override BoundNodeKind Kind => BoundNodeKind.VariableExpression;
    }
}