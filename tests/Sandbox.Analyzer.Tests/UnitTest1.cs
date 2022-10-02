using Xunit;
using AnalyzerTest = Microsoft.CodeAnalysis.CSharp.Testing.CSharpAnalyzerTest<Sandbox.Analyzer.SimpleAnalyzer, Microsoft.CodeAnalysis.Testing.Verifiers.XUnitVerifier>;
using Verify = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<Sandbox.Analyzer.SimpleAnalyzer>;
using System.Threading.Tasks;

namespace Sandbox.Analyzer.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            string testCode = @"
using System;

namespace Sandbox.PlaygroundLibrary
{
    [System.AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class FooAttribute : Attribute
    {
        public FooAttribute(string name) => Name = name;
        public string Name { get; }
    }

    public class MethodAttributesExample
    {
        [Foo(""a"")]
        [return: Foo(""b"")]
        [Foo(""c"")]
        public string Run(string myQueueItem) => $""gh {myQueueItem}"";
    }
}";
            var test = new AnalyzerTest
            {
                TestCode = testCode
            };

            test.ExpectedDiagnostics.Add(Verify.Diagnostic().WithSeverity(Microsoft.CodeAnalysis.DiagnosticSeverity.Warning)
                .WithSpan(15, 10, 15, 18).WithArguments("FooAttribute"));

            test.ExpectedDiagnostics.Add(Verify.Diagnostic().WithSeverity(Microsoft.CodeAnalysis.DiagnosticSeverity.Warning)
                .WithSpan(17, 10, 17, 18).WithArguments("FooAttribute"));

            await test.RunAsync();
        }
    }
}