using System;
using System.Collections.Generic;
using SuppaCompiler.CodeAnalysis.Syntax;

namespace SuppaCompiler.CodeAnalysis.Binding
{
    internal sealed class Binder
    {
        private readonly IDictionary<string, Symbol> variables;
        public readonly DiagnosticBag Diagnostics = new DiagnosticBag();

        public Binder(IDictionary<string, Symbol> variables)
        {
            this.variables = variables;
        }

        public BoundExpression BindExpression(ExpressionSyntax syntax)
        {
            switch (syntax.Kind)
            {
                case SyntaxKind.LiteralExpression:
                    return BindLiteralExpression((LiteralExpressionSyntax) syntax);
                case SyntaxKind.UnaryExpression:
                    return BindUnaryExpression((UnaryExpressionSyntax) syntax);
                case SyntaxKind.BinaryExpression:
                    return BindBinaryExpression((BinaryExpressionSyntax) syntax);
                case SyntaxKind.NameExpression:
                    return BindNameExpression((NameExpressionSyntax)syntax);
                case SyntaxKind.AssigmentExpression:
                    return BindAssigmentExpression((AssignmentExpressionSyntax)syntax);
                case SyntaxKind.Invalid:
                    return new InvalidBoundExpression();

                default:
                    throw new Exception($"Unexpected syntax {syntax.Kind}");
            }
        }

        private BoundExpression BindAssigmentExpression(AssignmentExpressionSyntax syntax)
        {
            var name = syntax.IdentifierToken;
            var boundExpresssion = BindExpression(syntax.Expression);

            variables[name.Identifier] = new Symbol
            {
                Type = boundExpresssion.Type,
                Value = null,
            };
            
            return new BoundAssignementExpression(name, boundExpresssion);
        }

        private BoundExpression BindNameExpression(NameExpressionSyntax syntax)
        {
            var name = syntax.Identifier;
            if (variables.TryGetValue(name, out var value))
            {
                var type = typeof(int);
                return new BoundVariableExpression(name, type);
            }

            Diagnostics.ReportUndefinedName(name);
            return new BoundLiteralExpression(0);
        }

        private BoundExpression BindLiteralExpression(LiteralExpressionSyntax syntax)
        {
            var value = syntax.Value ?? 0;
            return new BoundLiteralExpression(value);
        }

        private BoundExpression BindUnaryExpression(UnaryExpressionSyntax syntax)
        {
            var boundOperand = BindExpression(syntax.Operand);
            var boundOperator = BoundUnaryOperator.Bind(syntax.OperatorToken.Kind, boundOperand.Type);

            if (boundOperator == null)
            {
                Diagnostics.ReportUndefinedUnaryOperator(syntax.OperatorToken.InnerToken.Span, syntax.OperatorToken.Span.ToStringValue(), boundOperand.Type);
                return boundOperand;
            }

            return new BoundUnaryExpression(boundOperator, boundOperand);
        }

        private BoundExpression BindBinaryExpression(BinaryExpressionSyntax syntax)
        {
            var boundLeft = BindExpression(syntax.Left);
            var boundRight = BindExpression(syntax.Right);
            var boundOperator = BoundBinaryOperator.Bind(syntax.OperatorToken.Kind, boundLeft.Type, boundRight.Type);

            if (boundOperator == null)
            {
                Diagnostics.ReportUndefinedBinaryOperator(syntax.OperatorToken.InnerToken.Span, syntax.OperatorToken.InnerToken.ToStringValue(), boundLeft.Type, boundRight.Type);
                return boundLeft;
            }

            return new BoundBinaryExpression(boundLeft, boundOperator, boundRight);
        }
    }
}
