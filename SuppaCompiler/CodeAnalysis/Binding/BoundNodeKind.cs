namespace SuppaCompiler.CodeAnalysis.Binding
{
    public enum BoundNodeKind
    {
        Invalid,
        LiteralExpression,
        UnaryExpression,
        BinaryExpression,
        VariableExpression,
        AssignmentExpression,
    }
}
