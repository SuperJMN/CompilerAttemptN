using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Superpower.Model;
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

    internal class DiagnosticBag : Collection<Diagnostic>
    {
        private void Report(TextSpan span, string message)
        {
            Add(new Diagnostic(span, message));
        }

        public void ReportUndefinedUnaryOperator(TextSpan span, string operatorStr, Type operandType)
        {
            Add(new Diagnostic(span, $"Cannot find operator {operatorStr} for type {operandType}"));
        }

        public void ReportUndefinedBinaryOperator(TextSpan span, string operatorStr, Type leftType, Type rightType)
        {
            Add(new Diagnostic(span, $"Cannot find operator {operatorStr} for type {leftType} and {rightType}"));
        }

        public void ReportUndefinedName(string name)
        {
            Add(new Diagnostic(new TextSpan(), $"Cannot find name {name}"));
        }
    }
}
