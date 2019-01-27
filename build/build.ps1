
[CmdletBinding()]
Param(
    [string]$Script = "build.cake",
    [string]$Target = "Default",
    [ValidateSet("Release", "Debug")]
    [string]$Configuration = "Release",
    [ValidateSet("Quiet", "Minimal", "Normal", "Verbose", "Diagnostic")]
    [string]$Verbosity = "Verbose",
    [switch]$SkipToolPackageRestore,
    [Parameter(Position=0,Mandatory=$false,ValueFromRemainingArguments=$true)]
    [string[]]$ScriptArgs
)

$sourceNugetExe = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"
$NUGET_EXE = Join-Path ".\tools" "nuget.exe"

if (!(Test-Path $NUGET_EXE))
{
    Invoke-WebRequest $sourceNugetExe -OutFile $NUGET_EXE
    Set-Alias nuget $NUGET_EXE -Scope Global -Verbose
}

Write-Host "$NUGET_EXE"

$TOOLS_DIR = Convert-Path ".\Tools"

$NuGetOutput = Invoke-Expression "$NUGET_EXE install -ExcludeVersion -OutputDirectory $TOOLS_DIR"


$CAKE_EXE = Join-Path $TOOLS_DIR "Cake/Cake.exe"
$Script = Join-Path "build" $Script

Write-Host "$Script"
# Start Cake
Write-Host "Running build script..."
Write-Host "Cake exe: $CAKE_EXE"
Invoke-Expression "& `"$CAKE_EXE`" `"$Script`" -target=`"$Target`" -configuration=`"$Configuration`" -verbosity=`"$Verbosity`" $ScriptArgs"
exit $LASTEXITCODE

