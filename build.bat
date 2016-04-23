call nuget.exe restore src

set msBuildDir="C:\Program Files (x86)\MSBuild\14.0\Bin"
call %msBuildDir%\msbuild "msbuild.config" /p:Configuration=Build