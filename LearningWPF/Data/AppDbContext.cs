using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
// --- App modules ---
using CommonLibrary.Validation;
using LearningWPF.Common;
using LearningWPF.Models;

namespace LearningWPF.Data
{
    internal class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) return;

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
            sb.AppendLine($"{ex.GetType().Name} error details - {ex.InnerException?.InnerException?.Message}.");
            
            foreach (EntityEntry entityEntry in ex.Entries)
            {
                sb.AppendLine(
                    $"Entity of type {entityEntry.Entity.GetType().Name} in state {entityEntry.State} could not be updated.");
            }

            return new List<ValidationMessage>
            {
                new()
                {
                    PropertyName = ex.GetType().Name,
                    Message = sb.ToString(),
                }
            };
        }

        /*
         In Entity Framework Core, the DbSet represents the set of entities. 
         In a database, a group of similar entities is called an Entity Set (DbSet<TEntity> are called entity sets).
         The DbSet enables to perform CRUD operations on the entity set.
        */
        // Entity sets
        public virtual DbSet<UserModel>? Users { get; set; }
    }
}
