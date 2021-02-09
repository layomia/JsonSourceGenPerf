REM Rebuild System.Text.Json ref project.
dotnet restore D:\repos\dotnet_runtimelab_2\src\libraries\System.Text.Json\ref\System.Text.Json.csproj
dotnet msbuild D:\repos\dotnet_runtimelab_2\src\libraries\System.Text.Json\ref\System.Text.Json.csproj /t:Rebuild /p:TargetFramework=netcoreapp3.0 /p:Configuration=Release

REM Rebuild System.Text.Json src project.
dotnet restore D:\repos\dotnet_runtimelab_2\src\libraries\System.Text.Json\src\System.Text.Json.csproj
dotnet msbuild D:\repos\dotnet_runtimelab_2\src\libraries\System.Text.Json\src\System.Text.Json.csproj /t:Rebuild /p:TargetFramework=netcoreapp3.0 /p:Configuration=Release

REM Rebuild System.Text.Json.SourceGeneration project.
dotnet msbuild D:\repos\dotnet_runtimelab_2\src\libraries\System.Text.Json\System.Text.Json.SourceGeneration\System.Text.Json.SourceGeneration.csproj /p:BuildTargetFramework=netstandard2.0 /p:Configuration=Debug

REM Copy System.Text.Json.dll (implementation dll) to .nuget package cache.
robocopy D:\repos\dotnet_runtimelab_2\artifacts\bin\System.Text.Json\netcoreapp3.0-Release\ C:\Users\laakinri.NORTHAMERICA\.nuget\packages\system.text.json\6.0.0-preview.2.21102.3\lib\netcoreapp3.0\ System.Text.Json.dll /is /it

REM Copy System.Text.Json.SourceGeneration.dll to .nuget package cache.
robocopy D:\repos\dotnet_runtimelab_2\artifacts\bin\System.Text.Json.SourceGeneration\netstandard2.0-Debug\ C:\Users\laakinri.NORTHAMERICA\.nuget\packages\system.text.json\6.0.0-preview.2.21102.3\analyzers\dotnet\cs System.Text.Json.SourceGeneration.dll /is /it