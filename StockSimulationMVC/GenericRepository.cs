using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Validation;
using StockSimulationMVC.Interface;
using StockSimulationMVC.Models;

namespace StockSimulationMVC
{
    public class GenericRepository<TEntity> : IRepository<TEntity>
       where TEntity : class
    {
        #region Fields

        private DbContext _context
        {
            get;
            set;
        }
        private IDbSet<TEntity> _entities;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">Object context</param>
        public GenericRepository(DbContext context)
        {
            this._context = context;
        }

        //public GenericRepository()
        //{
        //    this._context = new TechnologicalDataObjectContext();
        //}

        //public GenericRepository(TechnologicalDataModel tt)
        //{
        //    this._context = new TechnologicalDataObjectContext();
        //}



        #endregion

        #region Utilities

        /// <summary>
        /// Get full error
        /// </summary>
        /// <param name="exc">Exception</param>
        /// <returns>Error</returns>
        protected string GetFullErrorText(DbEntityValidationException exc)
        {
            var msg = string.Empty;
            foreach (var validationErrors in exc.EntityValidationErrors)
                foreach (var error in validationErrors.ValidationErrors)
                    msg += string.Format("Property: {0} Error: {1}", error.PropertyName, error.ErrorMessage) + Environment.NewLine;
            return msg;
        }

        #endregion
        public virtual void Create(TEntity instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance is null-Create");
            }
            else
            {
                this._context.Set<TEntity>().Add(instance);
                this.SaveChanges();
            }
        }

        public virtual void Delete(TEntity instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance is null-Delete");
            }
            else
            {
                this._context.Entry(instance).State = EntityState.Deleted;
                this.SaveChanges();
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual TEntity Get(int primaryID)
        {
            return this._entities.Find(primaryID);
        }

        public IQueryable<TEntity> GetAll()
        {

            return this._context.Set<TEntity>().AsQueryable();
        }

        public IQueryable<TechnologicalDataModel> GetAllTech()
        {

            return this._context.Set<TechnologicalDataModel>().Where(m=>  m.Date.Year > 2013).AsQueryable();
        }

        public IQueryable<BasicFinancialDataModel> GetAllBasic()
        {

            return this._context.Set<BasicFinancialDataModel>().Where(m =>  m.Date.Year > 2007).AsQueryable();
        }

        public IQueryable<MonthRevenueModel> GetAllMonthRevenue()
        {

            return this._context.Set<MonthRevenueModel>().Where(m =>  m.Date.Year > 2007).AsQueryable();
        }

        public void SaveChanges()
        {
            this._context.SaveChanges();
        }

        public void Update(TEntity instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance is null-Update");
            }
            else
            {
                this._context.Entry(instance).State = EntityState.Modified;
                this.SaveChanges();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._context != null)
                {
                    this._context.Dispose();
                    this._context = null;
                }
            }
        }
    }

}