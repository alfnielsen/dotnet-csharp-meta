using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace dotnet_csharp_meta;

public static class Query
{
    public static IEnumerable<TResult> OfType<TResult>(string programText, Func<TResult, bool> predicate)
        where TResult: BaseMethodDeclarationSyntax 
    => OfType(CSharpSyntaxTree.ParseText(programText), predicate);
    
    public static IEnumerable<TResult> OfType<TResult>(SyntaxTree tree, Func<TResult, bool> predicate)
        where TResult: BaseMethodDeclarationSyntax 
    => OfType(tree.GetCompilationUnitRoot(), predicate);
    
    public static IEnumerable<TResult> OfType<TResult>(CompilationUnitSyntax root, Func<TResult, bool> predicate)
        where TResult: BaseMethodDeclarationSyntax 
    {
        return root.DescendantNodes()
           .OfType<TResult>()
           .Where(predicate);
    }
    
    
}