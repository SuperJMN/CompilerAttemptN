﻿using System.Collections.Generic;
using System.Linq;
using SuppaCompiler.CodeAnalysis.Binding;
using SuppaCompiler.CodeAnalysis.Syntax;

namespace SuppaCompiler.CodeAnalysis
{
    public class Compilation
    {
        public SyntaxTree Syntax { get; }

        public Compilation(SyntaxTree syntax)
        {
            Syntax = syntax;
        }

        public EvaluationResult Evaluate()
        {
            var binder = new Binder();
            var boundExpression = binder.BindExpression(Syntax.Root);
            var evaluator = new Evaluator(boundExpression);

            var diagnostics = binder.Diagnostics;
            if (diagnostics.Any())
            {
                return new EvaluationResult(diagnostics, null);
            }

            var value = evaluator.Evaluate();
            return new EvaluationResult(diagnostics, value);
        }
    }
}