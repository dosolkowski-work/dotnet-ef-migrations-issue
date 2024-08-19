Steps to reproduce:

1. Clone the repository
2. Run `dotnet tool restore` to install the EF Core CLI
3. Run `dotnet build` to ensure the project builds
4. Run `dotnet ef migrations add TestAnnotation` and observe the generated .cs file has no code in it at all
