namespace SuppaCompiler.CodeAnalysis.Binding
{
    internal enum BoundNodeKind
    {
        Invalid,
        LiteralExpression,
        UnaryExpression,
        BinaryExpression,
        VariableExpression,
        AssignmentExpression,
    }
}
