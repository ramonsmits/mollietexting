properties {
	$TargetFramework = "net-4.0"
	$buildConfiguration = "Release"
}

$baseDir  = resolve-path .
$binariesDir = "$buildBase\binaries"
$srcDir = "$baseDir\src"
$buildBase = "$baseDir\build"
$outDir =  "$buildBase\output"
$artifactsDir = "$buildBase\artifacts"
$toolsDir = "$baseDir\tools"
$zipExec = "$toolsDir\zip\7za.exe"
$script:architecture = "x86"
$script:msBuildTargetFramework = ""	
$script:msBuild = ""
$script:isEnvironmentInitialized = $false

include $toolsDir\psake\buildutils.ps1

task default -depends Release

task Release -depends Compile, ZipOutput

task Compile -depends Clean, InitEnvironment{ 

	$solutions = dir "$srcDir\*.sln"
	$solutions | % {
		$solutionFile = $_.FullName
		exec { &$script:msBuild $solutionFile /m /p:OutDir="$buildBase\bin\" /p:Configuration=$buildConfiguration }
	}
	
	$assemblies = @()
	$assemblies +=	dir $buildBase\bin\MollieTexting.dll
	$assemblies  +=  dir $buildBase\bin\MollieTexting*.dll -Exclude MollieTexting.dll, **Tests.dll
	
	Copy-Item $buildBase\bin\MollieTexting.dll $binariesDir -Force;
	Copy-Item $buildBase\bin\MollieTexting.pdb $binariesDir -Force;
}

task InitEnvironment -description "Initializes the environment for build" {

	if($script:isEnvironmentInitialized -ne $true){
		if ($TargetFramework -eq "net-4.0"){
			$netfxInstallroot ="" 
			$netfxInstallroot =	Get-RegistryValue 'HKLM:\SOFTWARE\Microsoft\.NETFramework\' 'InstallRoot' 
			
			$netfxCurrent = $netfxInstallroot + "v4.0.30319"
			
			$script:msBuild = $netfxCurrent + "\msbuild.exe"
			
			echo ".Net 4.0 build requested - $script:msBuild" 

			$script:msBuildTargetFramework ="/p:TargetFrameworkVersion=v4.0 /ToolsVersion:4.0"
			
			$script:isEnvironmentInitialized = $true
		}
	}
	$binariesExists = Test-Path $binariesDir;
	if($binariesExists -eq $false){	
		Create-Directory $binariesDir
		echo "created binaries"
	}
}

task Clean -description "Cleans the evironment for the build" {

	if(Test-Path $buildBase){
		Delete-Directory $buildBase
	}
	
	if(Test-Path $artifactsDir){
		Delete-Directory $artifactsDir
	}
	
	if(Test-Path $binariesDir){
		Delete-Directory $binariesDir
	}
}

task ZipOutput -description "Ziping artifacts directory for releasing"  {
	
	echo "Cleaning the Release Artifacts before ziping"

	if((Test-Path -Path $artifactsDir) -eq $true)
	{
		Delete-Directory $artifactsDir
	}
	
    Create-Directory $artifactsDir
	
	$archive = "$artifactsDir\MollieTexting.zip"
	echo "Ziping artifacts directory for releasing"
	exec { &$zipExec a -tzip $archive $binariesDir\** }
}
