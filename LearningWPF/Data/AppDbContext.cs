using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
// --- App modules ---
using CommonLibrary.Troubles;
using LearningWPF.Common;
using LearningWPF.Models;
using CommonLibrary.Exceptions;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace LearningWPF.Data
{
    internal class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) return;

            string connectionString = AppSettings.Instance.ConnectionString;

            /* Previously, using System.Windows.Application.Properties.
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

        /// <summary>
        /// Generates <see cref="TroubleMessage"/> list for the exception caught
        /// </summary>
        public static List<TroubleMessage> CreateTroubleMessages(DbUpdateException ex)
        {
            ExceptionManager.LogToDebug(ex);

            List<TroubleMessage> troubleMessages = new();

            StringBuilder sb = new();

            // Get exception type
            string exceptionType = (ex.InnerException ?? ex).GetType().Name;
            string text = "Details";
            sb.AppendLine($"{exceptionType} {text}");

            // Retrieve the exception messages from database engine
            string? innerDetail = ex.InnerException?.InnerException?.Message;
            if (!string.IsNullOrWhiteSpace(innerDetail)) sb.AppendLine($"\n{innerDetail}");

            // Add exception to the trouble list
            troubleMessages.Add(new TroubleMessage { PropertyName = exceptionType, Message = sb.ToString() });

            // Get the message suffix for the exception from EF Core
            text = "{0} {1} could not be registered in the database.";

            // Retrieve the exception messages from EF Core and add them to the trouble list
            troubleMessages.AddRange(ex.Entries.Select(entityEntry => new TroubleMessage
            {
                PropertyName = entityEntry.Entity.GetType().Name,
                Message = string.Format(text, entityEntry.State, entityEntry.Entity.GetType().Name)
            }));

            return troubleMessages;
        }

        /// <summary>
        /// Generates <see cref="TroubleMessage"/> list for the exception caught
        /// </summary>
        public static List<TroubleMessage> CreateTroubleMessages(SqlException ex)
        {
            ExceptionManager.LogToDebug(ex);

            StringBuilder sb = new();

            // Get exception type
            string exceptionType = (ex.InnerException ?? ex).GetType().Name;
            sb.AppendLine($"{exceptionType} Details");

            sb.AppendLine($"\n{ex.Message}");

            // Retrieve the exception message from database engine and add it to the trouble list
            return new List<TroubleMessage>
            {
                new()
                {
                    PropertyName = exceptionType,
                    Message = sb.ToString(),
                }
            };
        }

        /*
         In Entity Framework Core, the DbSet represents the set of entities. 
         In a database, a group of similar entities is called an Entity Set (DbSet<TEntity> are called entity sets).
         The DbSet enables to perform CRUD operations on the entity set.
        */

        #region Entity sets

        public virtual DbSet<UserModel>? Users { get; set; }

        #endregion Entity sets
    }
}
