using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;

namespace dotnet_csharp_meta;

public static class ModelLoader
{
    public static async Task<Solution> LoadSolution(string solutionPath)
    {
        MSBuildLocator.RegisterDefaults();
        var msWorkspace = MSBuildWorkspace.Create();
        var solution = await msWorkspace.OpenSolutionAsync(solutionPath);
        return solution;
    }
    
    public static async Task<Workspace> Workspace(string solutionPath)
    {
        var solution = await LoadSolution(solutionPath);
        return solution.Workspace;
    }
    
    public static async Task<IEnumerable<Project>> Projects(string solutionPath)
    {
        var solution = await LoadSolution(solutionPath);
        return solution.Projects;
    }

    public static async Task<IEnumerable<Document>> Documents(string solutionPath, string? projectName = null)
    {
        var projects = await Projects(solutionPath);
        if (projectName is not null)
        {
            projects = projects.Where(p => p.Name == projectName);
        }
        return projects.SelectMany(p => p.Documents);
    }
    
}