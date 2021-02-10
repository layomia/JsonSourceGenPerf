@RD /S /Q .\bin .\obj

dotnet publish -c Release /bl

dotnet serve --directory .\bin\Release\net6.0\publish\wwwroot