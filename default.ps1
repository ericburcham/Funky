# Shared variables --------------------------------------------------------------------------------
$msbuildExe = Get-Item "C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe"
$nugetExe = Get-Item ".\Source\.nuget\NuGet.exe"
$packageDirectory = ".\Packages"
$reflectionsProject = ".\Source\Funky\Funky.csproj"
$solutionFile = ".\Source\Funky.sln"



# Standard PowerShell Functions -------------------------------------------------------------------
function Create-Directory([string]$path) {
    New-Item -ItemType Directory -Force -Path $path
}

function Delete-Directory([string]$path) {
    if (Test-Path $path)
    {
        Remove-Item $path -Force -Recurse
    }
}

function Invoke-Compile {    
    [CmdletBinding()]  
    param(  
        [Parameter(Position=0,Mandatory=1)] [string]$slnPath = $null,
        [Parameter(Position=1,Mandatory=0)] [string]$configuration = "Debug",
        [Parameter(Position=2,Mandatory=0)] [string]$platform = "x64"
    )
    
    Write-Host "Running Build for solution @" $slnPath -ForegroundColor Cyan
    $config = "Configuration=" + $configuration + ";Platform="+ $platform
    exec { & $msbuildExe $slnPath /m /nologo /p:$($config) /p:SignAssembly=true /p:AssemblyOriginatorKeyFile=../StrongNameKey.snk /t:build /v:m }
}

function Test-ReparsePoint([string]$path) {
  $file = Get-Item $path -Force -ea 0
  return [bool]($file.Attributes -band [IO.FileAttributes]::ReparsePoint)
}



# Psake tasks -------------------------------------------------------------------------------------
task default -depends CleanAll, RestorePackages, BuildDebug, BuildRelease, Pack

task ? -description "Writes task documentation to the console." {
    WriteDocumentation
}

task BuildAllConfigurations -description "Builds all valid solution configurations." -depends BuildDebug, BuildRelease

task BuildAllConfigurationsWithPrerequisites -description "Builds all valid solution configurations.  Runs prerequisites first." -depends CleanAll, RestorePackages, BuildDebug, BuildRelease

task BuildDebug -description "Builds Funky.sln in the debug configuration." {
    Invoke-Compile $SolutionFile "Debug" "Any CPU"
}

task BuildDebugWithPrerequisites -description "Builds Funky.sln in the debug configuration.  Runs prerequisite steps first." -depends CleanAll, RestorePackages, BuildDebug

task BuildRelease -description "Builds Funky.sln in the release configuration." {
    Invoke-Compile $SolutionFile "Release" "Any CPU"
}

task BuildReleaseWithPrerequisites -description "Builds Funky.sln in the debug configuration.  Runs prerequisite steps first." -depends CleanAll, RestorePackages, BuildRelease

task CleanAll -description "Runs 'git clean -xdf.'  Prompts first if untracked files are found." {
    $gitStatus = (@(git status --porcelain) | Out-String)

    IF ($gitStatus.Contains("??"))
    {
        Write-Host "About to delete any untracked files.  Press 'Y' to continue or any other key to cancel." -ForegroundColor "yellow"
        $continue = $host.UI.RawUI.ReadKey().Character
        IF ($continue -ne "Y" -and $continue -ne "y")
        {
            Write-Error "CleanAll canceled."
        }
    }

    # test to see if packages is a link or a real directory
    if (Test-ReparsePoint("packages/")) {
        # packages is a NTFS Reparse point
        # delete everything in packages but 
        Echo "Packages is a link. Will keep it but delete its content."
        Remove-Item .\packages\* -Force -Recurse
        git clean -xdf -e "*.suo" -e "packages/"
    } else {
        git clean -xdf -e "*.suo" 
    }
}

task Pack -description "Packs Funky as a nuget package." {
    Delete-Directory $packageDirectory
    Create-Directory $packageDirectory
    exec { & $nugetExe pack $reflectionsProject -OutputDirectory $packageDirectory -Prop Configuration=Release -Symbols }
}

task PackWithPrerequisites -description "Packs Funky as a nuget package.  Runs prerequisites first." -depends CleanAll, RestorePackages, BuildDebug, BuildRelease, Pack

task Push -description "Pushes Funky to nuget.org." {
    $packages = Get-ChildItem $packageDirectory\Funky.*.nupkg
    foreach ($package in $packages)
    {
        exec { & $nugetExe push $package.FullName }
    }
}

task PushWithPrerequisites -depends CleanAll, RestorePackages, BuildDebug, BuildRelease, Pack, Push -description "Pushes Relfections to nuget.org.  Runs prerequisites first."

task RestorePackages -description "Restores all nuget packages in the solution." {
    exec { & $nugetExe restore $solutionFile }
}

task Start:VisualStudio -description "Opens Funky.sln in Visual Studio" {
    Invoke-Item .\Source\Funky.sln
}