using Microsoft.CodeAnalysis.CSharp;

namespace dotnet_csharp_meta;

public static class SyntaxWalker
{
    public static void Walk(string programText)
    {
        var tree = CSharpSyntaxTree.ParseText(programText);
        var root = tree.GetCompilationUnitRoot();
        
        var collector = new UsingCollector();
        collector.Visit(root);
        foreach (var directive in collector.Usings)
        {
            Console.WriteLine(directive.Name);
        }
    }
    
}