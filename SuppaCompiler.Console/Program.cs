using System;
using System.Collections.Generic;
using SuppaCompiler.CodeAnalysis;
using SuppaCompiler.CodeAnalysis.Syntax;
using System.Linq;
using SuppaCompiler.CodeAnalysis.Binding;

namespace SuppaCompiler.Console
{
    internal static class Program
    {
        private static void Main()
        {
            var scope = new Scope();

            while (true)
            {
                System.Console.Write("> ");
                var line = System.Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    return;

                if (line == "#reset")
                {
                    scope = new Scope();
                    continue;
                }

                if (line == "#cls")
                {
                    System.Console.Clear();
                    continue;
                }

                var compiler = new Compiler();
                var compilationResult = compiler.Compile(line, scope);

                if (!compilationResult.Diagnostics.Any())
                {
                    var evaluation = new Evaluator(compilationResult.BoundExpression, scope).Evaluate();
                    ShowEvaluatedValue(evaluation);
                }
                else
                {
                    ShowDiagnostics(compilationResult.Diagnostics);
                }

                scope = new Scope(scope);
            }
        }

        private static void ShowEvaluatedValue(object evaluation)
        {
            System.Console.ForegroundColor = ConsoleColor.Magenta;
            System.Console.WriteLine(evaluation);
            System.Console.ResetColor();
        }

        private static void ShowDiagnostics(IEnumerable<Diagnostic> diagnostics)
        {
            System.Console.ForegroundColor = ConsoleColor.Red;

            foreach (var diagnostic in diagnostics)
            {
                System.Console.WriteLine(diagnostic);
            }

            System.Console.ResetColor();
        }

        static void PrettyPrint(SyntaxNode node, string indent = "", bool isLast = true)
        {
            var marker = isLast ? "└──" : "├──";

            System.Console.Write(indent);
            System.Console.Write(marker);
            System.Console.Write(node.Kind);

            if (node is SyntaxToken t && t.Value != null)
            {
                System.Console.Write(" ");
                System.Console.Write(t.Value);
            }

            System.Console.WriteLine();

            indent += isLast ? "   " : "│  ";

            var lastChild = node.GetChildren().LastOrDefault();

            foreach (var child in node.GetChildren())
            {
                PrettyPrint(child, indent, child == lastChild);
            }
        }
    }
}
