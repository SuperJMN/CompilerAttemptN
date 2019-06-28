using SuppaCompiler.CodeAnalysis;
using SuppaCompiler.CodeAnalysis.Binding;
using Xunit;

namespace SuppaCompiler.Tests
{
    public class CompilationTests
    {
        [Fact]
        public void Test()
        {
            var sut = new Compiler();
            var compilationResult = sut.Compile("2+2", new Scope());
        }
    }
}