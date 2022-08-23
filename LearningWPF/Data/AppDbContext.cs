using System.Collections.Generic;
using System.Net.Mime;
using CommonLibrary.Validation;
using LearningWPF.Models;
using Microsoft.EntityFrameworkCore;

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

            optionsBuilder
                .UseSqlServer("Server=localhost\\DEV2019;Database=learningWPF;Trusted_Connection=True;MultipleActiveResultSets=true");
        }

        /*
        public List<ValidationMessage> CreateValidationMessages(DbEntityValidationException ex)
        {
            // Retrieve the error messages from EF
            return ex.EntityValidationErrors
                .SelectMany(x => x.ValidationErrors)
                .Select(error => new ValidationMessage 
                    { 
                        Message = error.ErrorMessage, 
                        PropertyName = error.PropertyName

                    }).ToList();
        }
        */

        /*
         In Entity Framework Core, the DbSet represents the set of entities. 
         In a database, a group of similar entities is called an Entity Set (DbSet<TEntity> are called entity sets).
         The DbSet enables to perform CRUD operations on the entity set.
        */
        // Entity sets
        public virtual DbSet<UserModel> Users { get; set; }
    }
}