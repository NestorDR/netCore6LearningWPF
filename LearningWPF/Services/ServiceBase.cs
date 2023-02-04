using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
// App modules
using CommonLibrary;
using CommonLibrary.Troubles;
using LearningWPF.Data;
using LearningWPF.Models;

namespace LearningWPF.Services
{
    internal class ServiceBase : IDisposable
    {
        #region Constructor

        public ServiceBase() : this(null)
        {
        }

        public ServiceBase(AppDbContext? db)
        {
            _db = db;
        }

        #endregion Constructor

        #region Properties

        private AppDbContext? _db;
        public AppDbContext Db => _db ??= new AppDbContext();

        public List<TroubleMessage> TroubleMessages = new();

        public virtual int TopRecords => 1000;

        #endregion Properties

        #region Methods

        public virtual IEnumerable<TEntity> Get<TEntity>(int? count = null) where TEntity : CommonBase, IEntityInterface
        {
            try
            {
                return Db.Set<TEntity>().Take(count ?? TopRecords).AsEnumerable();
            }
            catch (SqlException ex)
            {
                TroubleMessages = AppDbContext.CreateTroubleMessages(ex);
            }

            return new List<TEntity>();
        }

        public virtual TEntity? GetById<TEntity>(int? id = null) where TEntity : CommonBase, IEntityInterface
        {
            try
            {
                return Db.Set<TEntity>().Find(id);
            }
            catch (SqlException ex)
            {
                TroubleMessages = AppDbContext.CreateTroubleMessages(ex);
            }

            return null;
        }

        public virtual bool Save<TEntity>(List<TEntity> list, bool isAddMode) where TEntity : CommonBase, IEntityInterface
        {
            bool result = false;

            try
            {
                foreach (TEntity entity in list)
                {
                    if (entity.Id == 0 || isAddMode)
                    {
                        // Add new entity to EF entities collection - Insert
                        Db.Set<TEntity>().Add(entity);
                        CreateLoggingData(entity);
                    }
                    else
                    {
                        // Update
                        Db.Attach(entity).State = EntityState.Modified;
                        UpdateLoggingData(entity);
                    }
                }

                Db.SaveChanges();

                result = true;
            }
            catch (DbUpdateException ex)
            {
                TroubleMessages = AppDbContext.CreateTroubleMessages(ex);
            }
            catch (SqlException ex)
            {
                TroubleMessages = AppDbContext.CreateTroubleMessages(ex);
            }

            return result;
        }

        /// <summary>
        /// Create or Update a generic entity in the database
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity">Entity to save</param>
        /// <param name="isAddMode">True when create/insert, False otherwise</param>
        /// <returns></returns>
        public virtual bool Save<TEntity>(TEntity entity, bool isAddMode) where TEntity : CommonBase, IEntityInterface
        {
            bool result = false;

            try
            {
                if (isAddMode)
                {
                    // Add new entity to EF entities collection - Insert
                    Db.Set<TEntity>().Add(entity);
                    CreateLoggingData(entity);
                }
                else
                {
                    // Update
                    Db.Attach(entity).State = EntityState.Modified;
                    UpdateLoggingData(entity);
                }

                Db.SaveChanges();

                result = true;
            }
            catch (DbUpdateException ex)
            {
                TroubleMessages = AppDbContext.CreateTroubleMessages(ex);
            }

            return result;
        }

        public static void CreateLoggingData<TEntity>(TEntity entity) where TEntity : IEntityInterface
        {
            entity.CreatedBy = Thread.CurrentPrincipal?.Identity?.Name;
            entity.CreatedOn = DateTime.Now;
        }

        public static void UpdateLoggingData<TEntity>(TEntity entity) where TEntity : IEntityInterface
        {
            entity.UpdatedBy = Thread.CurrentPrincipal?.Identity?.Name;
            entity.UpdatedOn = DateTime.Now;
        }

        /// <summary>
        /// Delete a generic entity in the database
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity">Entity to delete</param>
        /// <returns>True if the delete was successful, otherwise false</returns>
        public virtual bool Delete<TEntity>(TEntity entity) where TEntity : CommonBase, IEntityInterface
        {
            bool result = false;

            try
            {
                // Get the entity saved in the database
                TEntity? savedEntity = Db.Set<TEntity>().Find(entity.Id);

                if (savedEntity != null)
                {
                    // Remove entity from EF collection
                    Db.Set<TEntity>().Remove(savedEntity);

                    // Save changes to database
                    Db.SaveChanges();
                }

                result = true;
            }
            catch (DbUpdateException ex)
            {
                TroubleMessages = AppDbContext.CreateTroubleMessages(ex);
            }

            return result;
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            _db?.Dispose();
            _db = null;
        }

        #endregion
    }
}
