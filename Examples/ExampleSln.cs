using System.Reflection;
using dotnet_csharp_meta.Util;
using Microsoft.CodeAnalysis.MSBuild;

namespace dotnet_csharp_meta.Examples;

public static class ExampleSln
{
    public static async Task Run()
    {
        // get path to solution
        var slnPath = Meta.GetMetaSolutionPath();
        Console.WriteLine("SlnPath: "+slnPath);
        await ModelLoader.LoadSolution(slnPath);
    }
}