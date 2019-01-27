//////////////////////////////////////////////////////////////////////
// Command Examples
//////////////////////////////////////////////////////////////////////

// Example
// PS C:\projects\LoadTesting> .\build\build.ps1 -Verbosity:Normal -Target:Create-Packages -ScriptArgs:@('--MsBuildConfiguration=Release','--EnablePrecompilation=true','--MsBuildVerbosity=normal','-PackageVersion="1-integration"')

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var msBuildConfiguration = Argument("msBuildConfiguration", "Debug");
var msBuildVerbosity = Argument("msBuildVerbosity", Verbosity.Minimal);
var packageVersion = Argument("packageVersion", "");
var shouldRunASPNETPrecompilation = Argument("enablePrecompilation", false);
var layerToDeploy = Argument("layer","");
var lastDeploymentGitCommitID = Argument("lastDeploymentGitCommitID","");
var nUNitIsTeamCity = Argument("nUNitIsTeamCity",false);


//////////////////////////////////////////////////////////////////////
// TOOLS / ADDINS
//////////////////////////////////////////////////////////////////////

//#addin "Cake.PowerShell&version=0.4.5"
// Nunit.ConsoleRunner has a nasty bug https://github.com/nunit/nunit-console/issues/370
// Once 3.9.0 is released, use that version instead
//#tool nuget:?package=NUnit.ConsoleRunner&version=3.9.0
//#tool nuget:?package=JetBrains.dotCover.CommandLineTools&version=2018.2.3

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////
var buildFailedMessage = "Build Failed!";
var solutionFileName = "../WordConverterAPI.sln";
var msBuildToolVersion = MSBuildToolVersion.VS2017;
var nugetPackageVerbosity = "quiet";
var nugetPath = Context.Tools.Resolve("nuget.exe");

// Define artefact directories.
var deploymentDir = MakeAbsolute(File("../_Deployment")).FullPath;
var outputDir = deploymentDir + "./Output";
var outputPackagesDir = deploymentDir + "/Packages/";
var baseDictory = "../../";

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////
Task("Build-Dev-Machine")
    .IsDependentOn("Before-Build-Dev-Machine")
    .IsDependentOn("Build-Solution")
    .IsDependentOn("Copy-Website-Files");

Task("Before-Build-Dev-Machine")
    .Does(()=>{
        RunTarget("Clean-Deployment-Folders");
    });

Task("Build-Check")
    .IsDependentOn("Before-Build-Check")
    .IsDependentOn("Clean-Deployment-Folders")
    .IsDependentOn("Clean-Solution")
    .IsDependentOn("Build-Solution");

	Task("Copy-Website-Files")
    .Does(()=>{
       
    });
	

Task("Before-Build-Check")
    .Does(()=>{
        packageVersion = "1.0.0";
        lastDeploymentGitCommitID="79b8e7340e230b2779619a080b34e23700347d8c";
    });

Task("Clean-Solution")
    .Does(() =>
    {
        var settings = new MSBuildSettings
        {
            NodeReuse = false,
            MaxCpuCount = 0,
            Verbosity = msBuildVerbosity
        };
        settings = settings.WithTarget("Clean");
        MSBuild(solutionFileName, settings);
    });


public void RemoveDirectory(string dirPath){
    if(DirectoryExists(dirPath)){
        //remove it
        CleanDirectory(dirPath);
    }
}


Task("Build-Project")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(()=> {
        if (string.IsNullOrWhiteSpace(layerToDeploy))
        {
            Error("Please add at least one of following parameters: -project/-layer");
            throw new Exception(buildFailedMessage);
        }
        var parsedSolution = ParseSolution(solutionFileName);
        var allProjects = parsedSolution.Projects;
        var projectsByLayer = string.IsNullOrWhiteSpace(layerToDeploy) ? allProjects : allProjects.Where(x => x.Path.ToString().Contains("/"+layerToDeploy+"/"));
        var errors = new List<string>();
        foreach(var project in projectsByLayer)
        {
            Information("Deploying "+ project.Name);
            var settings = new MSBuildSettings
            {
                Configuration = msBuildConfiguration,
                NodeReuse = false,
                MaxCpuCount = 0,
                Verbosity = msBuildVerbosity,
                ToolVersion = msBuildToolVersion
            };
            try
            {
                MSBuild(project.Path, settings);
            }
            catch(Exception)
            {
              errors.Add(project.Path.ToString());
            }
        }
        var success = projectsByLayer.Count() - errors.Count();
        Information("Build finished ["+projectsByLayer.Count()+"] found, [" + success + "] succeed!"  );
        errors.ForEach(x => {Error("Build Failed ["+x+"]");});
      });

Task("Build-Solution")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(()=>
    {
        var settings = new MSBuildSettings
        {
            Configuration = msBuildConfiguration,
            NodeReuse = false,
            MaxCpuCount = 0,
            ToolVersion = MSBuildToolVersion.VS2017,
            Verbosity = msBuildVerbosity
        };
        settings.Properties["PrecompileASPNET"] = new List<string>{shouldRunASPNETPrecompilation.ToString()};

        MSBuild(solutionFileName,settings);

    });

Task("Restore-NuGet-Packages")
    .Does(() =>
    {
        var settings = new NuGetRestoreSettings
        {
            Verbosity = NuGetVerbosity.Detailed
        };
        
        NuGetRestore(solutionFileName, settings);   
    });

Task("Create-Packages")
    .IsDependentOn("Before-Create-Packages")
    .IsDependentOn("Clean-Deployment-Folders")
    .IsDependentOn("Build-Solution")
    .IsDependentOn("Create-Deployment-Artifacts");

Task("Before-Create-Packages")
    .Does(()=>
    {
        if(string.IsNullOrWhiteSpace(packageVersion))
            throw new Exception("missing package version parameter");
    });

Task("Create-Deployment-Artifacts")
    .Does(()=>
    {
        Information("Deployment directory path: "+outputDir);

        var actions = new List<Action>();
        actions.Add(()=>{
            var exitCode = StartProcess(nugetPath, new ProcessSettings {
            Arguments = new ProcessArgumentBuilder()
                .Append("pack ./nuspec/WordConverterWeb.nuspec -Version "+packageVersion+" -Verbosity "+nugetPackageVerbosity+" -BasePath "+baseDictory+" -OutputDirectory "+outputPackagesDir+" -NoPackageAnalysis -properties Configuration="+msBuildConfiguration+"")
            });

            if(exitCode != 0)
                throw new Exception(buildFailedMessage);
        });

        var projectsToBuild = new String[]{
            "../WordConverterAPI/WordConverterAPI.csproj"
        };
        actions.Add(()=>{
            foreach(var project in projectsToBuild)
            {
                var settings = new MSBuildSettings
                {
                    Configuration = msBuildConfiguration,
                    NodeReuse = false,
                    MaxCpuCount = 0,
                    Verbosity = msBuildVerbosity,
                    ToolVersion = msBuildToolVersion
                };
                settings.Properties["RunOctopack"] = new List<string>{"true"};
                settings.Properties["OctoPackPackageVersion"] = new List<string>{packageVersion};
                settings.Properties["OctoPackPublishPackageToFileShare"] = new List<string>{outputPackagesDir};
                settings.Properties["OctoPackNuGetExePath"] = new List<string>{nugetPath.FullPath}; // performance improvement - use lastest nuget.exe
				
                MSBuild(project, settings);
            }
        });

        DoInParallel(actions);
    });

public void DoInParallel(
    List<Action> actions,
    int maxDegreeOfParallelism = -1,
    System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
{
    var options = new ParallelOptions {
        MaxDegreeOfParallelism = maxDegreeOfParallelism,
        CancellationToken = cancellationToken
    };

    Parallel.Invoke(options, actions.ToArray());
}

Task("Clean-Deployment-Folders")
    .Does(() =>
    {
        try
        {
            RunTarget("_Clean-Deployment-Folders");
        }
        catch
        {
            Information("Failed cleaning deployment folders, retrying one more time..");
            RunTarget("_Clean-Deployment-Folders");
        }
    });
Task("_Clean-Deployment-Folders")
    .Does(()=>{

        EnsureDirectoryExists(deploymentDir);
        CleanDirectory(deploymentDir);
    });

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Build-Dev-Machine");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
