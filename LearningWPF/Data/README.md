# Data Description

## Packages installed
Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.Extensions.Configuration.Json
Microsoft.EntityFrameworkCore.Tools

## Sources
[EF Core getting Started with WPF](https://docs.microsoft.com/en-us/ef/core/get-started/wpf)

[Entity Framework 6 vs Entity Framework Core - subtle differences?](https://stackoverflow.com/questions/61153920/entity-framework-6-vs-entity-framework-core-subtle-differences#61154482)

[Add WPF Local SQL Database to Application](https://codedocu.com/Net-Framework/WPF/Code-Samples/Add-WPF-Local-SQL-Database-to-Application-and-Connect-Data?2140): insert a Service-based Database element.

[Multi-Tenanted EF Core Migration Deployment](https://chadgolden.com/blog/multi-tenanted-entity-framework-core-migration-deployment)

## Configuration

### appsettings.json
[.NET now uses the appsettings.json file for app settings](https://docs.microsoft.com/en-us/dotnet/core/porting/upgrade-assistant-wpf-framework#modernize-appsettingsjson)

[JSON configuration provider](https://docs.microsoft.com/en-us/dotnet/core/extensions/configuration-providers#json-configuration-provider)

[Can not add appsettings.json inside WPF project .Net Core](https://stackoverflow.com/questions/59909207/cannot-add-appsettings-json-inside-wpf-project-net-core-3-0#59909447)

Add the following NuGet package
  Microsoft.Extensions.Configuration.Json

Create and add appsettings.json manually and set property (F4 shortcut) "Copy to Output Directory": "Copy if newer".

Idem if you add appsettings.Development.json

[Group appsettings.*.json](https://stackoverflow.com/questions/50970954/is-there-a-dependentupon-option-in-a-net-core-app#50971055)
```
  <ItemGroup>
    <None Update="appsettings.json">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
    ...
    <None Update="appsettings.*.json">
      <DependentUpon>appsettings.json</DependentUpon>
    </None>
  </ItemGroup>
```

## Dependency injection

## Database
Add the following NuGet packages
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.SqlServer

### Inheriting from DBContext

[Can not add appsettings.json inside WPF project .Net Core](https://stackoverflow.com/questions/59909207/cannot-add-appsettings-json-inside-wpf-project-net-core-3-0#67338758)

[Multi-Tenanted EF Core Migration Deployment](https://chadgolden.com/blog/multi-tenanted-entity-framework-core-migration-deployment)

### Migrations Tools
To create migrations, apply migrations, and generate code for a model based on an existing database use [Package Manager Console tools](https://docs.microsoft.com/en-us/ef/core/cli/powershell).
- Microsoft.EntityFrameworkCore.Tools
