using System;
using System.Collections.ObjectModel;
using Superpower.Model;
using SuppaCompiler.CodeAnalysis.Syntax;

namespace SuppaCompiler.CodeAnalysis.Binding
{
    internal sealed class Binder
    {
        public readonly DiagnosticBag Diagnostics = new DiagnosticBag();


        public BoundExpression BindExpression(ExpressionSyntax syntax)
        {
            switch (syntax)
            {
                case LiteralExpressionSyntax e:
                    return BindLiteralExpression(e);
                case UnaryExpressionSyntax e:
                    return BindUnaryExpression(e);
                case BinaryExpressionSyntax e:
                    return BindBinaryExpression(e);
                default:
                    throw new Exception($"Unexpected syntax {syntax.Kind}");
            }
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
    }
}
