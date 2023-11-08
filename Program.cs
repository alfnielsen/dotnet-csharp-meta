using dotnet_csharp_meta.Commands;
using dotnet_csharp_meta.Examples;
using dotnet_csharp_meta.Util;

var noColor = HasDashedArg("plain");
var json = HasDashedArg("json");
var debug = HasDashedArg("debug");
var help = HasDashedArg("help","h","?");
var interactive = HasDashedArg("start");
var command = args.Length > 0 ? args[0] : null;
var commandArgs = 
    args.Length > 1 
        ? args[1..].Where(a => !a.StartsWith("-")).ToArray() 
        : Array.Empty<string>();

if (help)
{
    Note("Dotnet-C#-Meta");
    Write("");
    Note("Usage: ./dotnet-csharp-meta <Command> [..Arguments]");
    Write("Options:");
    Note("--plain (Remove color from output)");
    Note("--json (Output in JSON format - no-color implied)");
    Note("--debug (Write debug information)");
    Note("--start (Start interactive command)");
    Write("");
    Write("Commands:");
    Note("FindSolution <RootPath> (Error if not found or multiple found)");
    Note("FindSolutions <RootPath>");
    Note("FindDocuments <RootPath for Solution>");
    return;
}

if (!interactive && (args.Length <= 0 || command is null || command.StartsWith("-")))
{
    Error("No Command passed in.");
    Note("Help: -h | -? | --help");
    return;
}

if (interactive)
{
    Note("Select command:");
    Write("1. FindSolution");
    Write("2. FindSolutions");
    Write("3. FindProjects");
    Write("4. FindDocuments");
    Write("0. Exit");
    Note("Write <Num> followed by <arguments> to run command (space separated)");
    Console.Write("> ");
    var input = Console.ReadLine();
    if (input is null)
    {
        Error("No input");
        return;
    }
    commandArgs = input.Split(" ").Skip(1).ToArray();
    var commandNum = input.Split(" ").First();
    switch (commandNum)
    {
        case "0":
            return;
        case "1":
            command = "FindSolution";
            break;
        case "2":
            command = "FindSolutions";
            break;
        case "3":
            command = "FindProjects";
            break;
        case "4":
            command = "FindDocuments";
            break;
    }
    Write("Selected: " + command);
    
}

switch (command)
{
    // Run Example1 if there is an argument passed in to the program "Example1"
    case "Example1":
        Example1.Run();
        return;
    // Run ExampleSln if there is an argument passed in to the program "ExampleSln"
    case "ExampleSln":
        await ExampleSln.Run();
        return;
    // Run FindSolution if there is an argument passed in to the program "FindSolution"
    case "FindSolution":
    {
        var slnRoot = commandArgs.Length > 0 ? commandArgs[0] : Meta.GetMetaSolutionRoot();
        if (debug)
        {
            Note("Looking: " + slnRoot);
        }
        var solutionPath = ProjectFiles.FindSolution(slnRoot);
        // if (json) // same as not having json!
        Write(solutionPath);
        return;
    }
    case "FindSolutions":
    {
        var slnRoot = commandArgs.Length > 0 ? commandArgs[0] : Meta.GetMetaSolutionRoot();
        if (debug)
        {
            Note("Looking: " + slnRoot);
        }
        var solutionPaths = ProjectFiles.FindSolutions(slnRoot);
        if (json)
        {
            Write("[" + string.Join(",\n", solutionPaths.Select(p => $"\"{p}\"")) +"]");
            return;
        }
        Write(string.Join("\n", solutionPaths));
        return;
    }
    case "FindProjects":
    {
        var slnRoot = commandArgs.Length > 0 ? commandArgs[0] : Meta.GetMetaSolutionRoot();
        if (debug)
        {
            Note("Looking: " + slnRoot);
        }
        var projects = await ProjectFiles.FindProjects(slnRoot);
        if (json)
        {
            Write("[" + string.Join(",", projects.Select(p => $"\"{p.Name}\"")) +"]");
            return;
        }
        Write(string.Join("\n", projects.Select(x => x.Name)));
        return;
    }
    case "FindDocuments":
    {
        var slnRoot = commandArgs.Length > 0 ? commandArgs[0] : Meta.GetMetaSolutionRoot();
        if (debug)
        {
            Note("Looking: " + slnRoot);
        }
        var documents = await ProjectFiles.FindDocuments(slnRoot);
        if (json)
        {
            Write("[" + string.Join(",", documents.Select(p => $"\"{p.Name}\"")) +"]");
            return;
        }
        Write(string.Join("\n", documents.Select(x=>x.Name)));
        return;
    }
}

return;

void Write(string message) => Console.WriteLine(message);

void Note(string message)
{
    if (noColor)
    {
        Write(message);
        return;
    }
    
    var words = message.Split(" ");
    var firstWord = words[0];
    if (firstWord == "Error:")
    {
        Error(message);
        return;
    }
    Write("\x1B[33m" + firstWord + "\x1B[0m " + string.Join(" ", words[1..]));

}

void Error(string message)
{
    if (noColor)
    {
        Write("Error: " + message);
        return;
    }
    Write("\x1B[31mError:\x1B[0m " + message);
}

bool HasArg(params string[] arg) => args.Any(a => arg.Contains(a, StringComparer.OrdinalIgnoreCase));
bool HasDashedArg(params string[] arg)
{
    var dashedArgs = arg.SelectMany(a => new[] {"-" + a, "--" + a}).ToArray();
    return args.Any(a => dashedArgs.Contains(a, StringComparer.OrdinalIgnoreCase));
}

