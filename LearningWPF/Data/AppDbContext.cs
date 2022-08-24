using System.Collections.Generic;
using System.Text;
using CommonLibrary.Validation;
using LearningWPF.Common;
using LearningWPF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LearningWPF.Data
{
    internal class AppDbContext : DbContext
    {
        public AppDbContext() : base()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) return;

            // Application.Properties Property, visit: https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.properties?view=windowsdesktop-6.0
            string connectionString = AppSettings.Instance.ConnectionString;

            /* Previously, using System.Windows.Application.Properties. v
               Visit: https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.properties?view=windowsdesktop-6.0
            connectionString = Application.Current.Properties["DefaultConnectionSting"].ToString();
            */

            /* Required to use Migrations
            if (string.IsNullOrWhiteSpace(connectionString))
                connectionString =
                    "Server=localhost\\DEV2019;Database=learningWPF;Trusted_Connection=True;MultipleActiveResultSets=true";
            */
            
            optionsBuilder.UseSqlServer(connectionString);
        }

        public List<ValidationMessage> CreateValidationMessages(DbUpdateException ex)
        {
            // Retrieve the error messages from EF Core
            var sb = new StringBuilder();
            foreach (EntityEntry entityEntry in ex.Entries)
            {
                sb.AppendLine(
                    $"Entity of type {entityEntry.Entity.GetType().Name} in state {entityEntry.State} could not be updated.");
            }

            return new List<ValidationMessage>
            {
                new()
                {
                    Message = $"DbUpdateException error details - {ex?.InnerException?.InnerException?.Message}",
                    PropertyName = sb.ToString()
                }
            };
        }

        /*
         In Entity Framework Core, the DbSet represents the set of entities. 
         In a database, a group of similar entities is called an Entity Set (DbSet<TEntity> are called entity sets).
         The DbSet enables to perform CRUD operations on the entity set.
        */
        // Entity sets
        public virtual DbSet<UserModel> Users { get; set; }
    }
}
