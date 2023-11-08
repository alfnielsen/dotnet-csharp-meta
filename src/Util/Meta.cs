using System.Reflection;

namespace dotnet_csharp_meta.Util;

public static class Meta
{
    public static string GetMetaSolutionRoot()
    {
        var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var slnRoot = Path.GetFullPath(Path.Combine(basePath, "../../../"));
        return slnRoot;
    }
    public static string GetMetaSolutionPath()
    {
        var slnPath = Path.GetFullPath(Path.Combine( GetMetaSolutionRoot(), "dotnet-csharp-meta.sln"));
        return slnPath;
    }
    
}