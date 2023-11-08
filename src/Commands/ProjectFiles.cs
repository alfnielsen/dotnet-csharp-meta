using Microsoft.CodeAnalysis;

namespace dotnet_csharp_meta.Commands;

public static class ProjectFiles
{
    public static string FindSolution(string rootPath)
    {
        var slnFiles = Directory.GetFiles(rootPath, "*.sln", SearchOption.AllDirectories);
        switch (slnFiles.Length)
        {
            case 0:
                return "Error: No solution files found.";
            case > 1:
                return "Error: Multiple solution files found.";
            default:
                return slnFiles[0];
        }
    }
    
    public static string[] FindSolutions(string rootPath)
    {
        return Directory.GetFiles(rootPath, "*.sln", SearchOption.AllDirectories);
    }
    
    
    public static async Task<IEnumerable<Project>> FindProjects(string solutionPath, string? projectName = null)
    {
        var solution = FindSolution(solutionPath);
        var projects = await ModelLoader.Projects(solution);
        if (projectName is not null)
        {
            projects = projects.Where(p => p.Name == projectName);
        }

        return projects;
    }
    
    public static async Task<IEnumerable<Document>> FindDocuments(string solutionPath, string? projectName = null)
    {
        var projects = await FindProjects(solutionPath, projectName);
        return projects.SelectMany(p => p.Documents);
    }
    
    
}