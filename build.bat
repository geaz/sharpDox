call nuget.exe restore src

set msBuildDir="C:\Program Files (x86)\Microsoft Visual Studio\2017\BuildTools\MSBuild\15.0\Bin"
call %msBuildDir%\msbuild "msbuild.config"