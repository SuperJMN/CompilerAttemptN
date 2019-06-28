using System.Collections.Generic;
using SuppaCompiler.CodeAnalysis;
using Xunit;

namespace SuppaCompiler.Tests
{
    public class Class1
    {
        [Fact]
        public void Test()
        {
            var sut = new Compiler();
            var compilationResult = sut.Compile("2+2", new Dictionary<string, Symbol>());
        }
    }
}