# Create the learningWPF database by implementing Migrations

In VS 2022 follow this menu sequence

    > Tools
      > NuGet Package Manager 
        > Package Manager Console

Then in console to create c# database schema type

    `add-migration CreateDatabase`

where `CreateDatabase` is the migration name

Then push the migrations to database typing

    `update-database`

## Migrations Tools
To create migrations, apply migrations, and generate code for a model based on an existing database use [Package Manager Console tools](https://docs.microsoft.com/en-us/ef/core/cli/powershell).
- Microsoft.EntityFrameworkCore.Tools

If you get the

> **Warning**
> Both Entity Framework Core and Entity Framework 6 are installed

and you want to just use Entity Framework Core, you can run `Remove-Module EntityFramework6`

# Using T-Script
For the creation of the database and the tables, a Transact SQL script is attached:

    `create_database_and_tables.sql`

open it and run it in your MS SQL Server Management Studio
