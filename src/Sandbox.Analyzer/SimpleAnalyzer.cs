using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace Sandbox.Analyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SimpleAnalyzer : DiagnosticAnalyzer
    {
        static readonly DiagnosticDescriptor Diag1 = new("S1", "My title", "My msg", "Usage",
            DiagnosticSeverity.Warning, isEnabledByDefault: true);
        
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Diag1);

        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

            context.RegisterSymbolAction(analysisContext =>
            {
                var methodSymbol = (IMethodSymbol)analysisContext.Symbol;

                var attributes = methodSymbol.GetAttributes();

                if (attributes.IsEmpty)
                {
                    return;
                }

                foreach (var attribute in attributes)
                {
                    if (attribute.AttributeClass?.Name != "FooAttribute")
                    {
                        continue;
                    }
                    
                    if (attribute.ApplicationSyntaxReference == null)
                    {
                        continue;
                    }
                        
                    var location = Location.Create(attribute.ApplicationSyntaxReference.SyntaxTree, attribute.ApplicationSyntaxReference.Span);
                    analysisContext.ReportDiagnostic(Diagnostic.Create(Diag1, location, attribute.AttributeClass.Name));
                }

            }, SymbolKind.Method);
        }
    }
}