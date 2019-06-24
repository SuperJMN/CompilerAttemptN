using System;

namespace SuppaCompiler.CodeAnalysis
{
    public class Symbol
    {
        public Type Type { get; set; }
        public object Value { get; set; }
    }
}