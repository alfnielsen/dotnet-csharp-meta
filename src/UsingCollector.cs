using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace dotnet_csharp_meta;

public class UsingCollector : CSharpSyntaxWalker
{
    public List<UsingDirectiveSyntax> Usings { get; } = new();
    
    public override void VisitUsingDirective(UsingDirectiveSyntax node)
    {
        Console.WriteLine($"\tVisitUsingDirective called with {node.Name}.");
        if (node.Name is null)  return;
        if (node.Name.ToString() == "System")  return;
        if (node.Name.ToString().StartsWith("System.")) return;
        Console.WriteLine($"\t\tSuccess. Adding {node.Name}.");
        Usings.Add(node);
    }
}