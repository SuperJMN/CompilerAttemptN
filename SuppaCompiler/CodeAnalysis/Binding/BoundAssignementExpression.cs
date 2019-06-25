using System;
using SuppaCompiler.CodeAnalysis.Syntax;

namespace SuppaCompiler.CodeAnalysis.Binding
{
    internal class BoundAssignementExpression : BoundExpression
    {
        public NameExpressionSyntax Name { get; }
        public BoundExpression Expression { get; }

        public BoundAssignementExpression(NameExpressionSyntax name, BoundExpression expression)
        {
            Name = name;
            Expression = expression;
        }

        public override BoundNodeKind Kind => BoundNodeKind.AssignmentExpression;
        public override Type Type => Expression.Type;
    }
}