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
        int StartYear = 2016;
        int EndYear = 3014;
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

        public void SetYearRange(int StartYear,int EndYear)
        {
            this.StartYear = StartYear;
            this.EndYear = EndYear;
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
            DateTime StartDateTime = new DateTime(StartYear-1 , 12, 1);
            DateTime EndDateTime = new DateTime(EndYear, 12, 31);
            return this._context.Set<TechnologicalDataModel>().Where(m => m.Date>= StartDateTime && m.Date <= EndDateTime
          // &&(m.Company == 2884 || m.Company == 2344 || m.Company == 2104 || m.Company == 2327 || m.Company == 2377 || m.Company == 2352 || m.Company == 2492 || m.Company == 2323 || m.Company == 2474 || m.Company == 2360 || m.Company == 2009 || m.Company == 2472 || m.Company == 2884 || m.Company == 2408 || m.Company == 2428 || m.Company == 2478 || m.Company == 2301 || m.Company == 2477 || m.Company == 2421 || m.Company == 2881 || m.Company == 2882 || m.Company == 2027 || m.Company == 2014 || m.Company == 2340 || m.Company == 2488 || m.Company == 2351 || m.Company == 2397 || m.Company == 2305 || m.Company == 2345 )
                 && m.Company > 1000 && m.Company < 4000
               //&& (m.Company == 4755 || m.Company == 2327 )
              //|| m.Company == 2327 || m.Company == 2428
              // || m.Company == 8021 || m.Company == 6176 || m.Company == 2634 || m.Company == 2340
              // || m.Company == 1210 || m.Company == 1201 || m.Company == 2388 || m.Company == 2477
              // || m.Company == 2856 || m.Company == 2472
              //)//|| m.Company == 8404 || m.Company == 1539)
               ).AsQueryable();

        }

        public IQueryable<TechnologicalDataModel> GetTech9999()
        {
            DateTime StartDateTime = new DateTime(StartYear - 1, 12, 1);
            DateTime EndDateTime = new DateTime(EndYear, 12, 31);
            return this._context.Set<TechnologicalDataModel>().Where(m => m.Date >= StartDateTime && m.Date <= EndDateTime
               && (m.Company == 2330)
               ).AsQueryable();

        }

        public IQueryable<BasicFinancialContainParentDataModel> GetAllBasic()
        {

            return this._context.Set<BasicFinancialContainParentDataModel>().Where(m => m.Date.Year >=( StartYear - 7) && m.Date.Year <= EndYear
            // && m.Company == 2327
             && m.Company > 2000 && m.Company < 3000
             //  && (m.Company == 4755 || m.Company == 2327 
             //|| m.Company == 2327 || m.Company == 2428
             // || m.Company == 8021 || m.Company == 6176 || m.Company == 2634 || m.Company == 2340
             // || m.Company == 1210 || m.Company == 1201 || m.Company == 2388 || m.Company == 2477
             // || m.Company == 2856 || m.Company == 2472
             //)//|| m.Company == 8404 || m.Company == 1539)
             ).AsQueryable();
        }

        public IQueryable<MonthRevenueModel> GetAllMonthRevenue()
        {

            return this._context.Set<MonthRevenueModel>().Where(m => m.Date.Year >= StartYear && m.Date.Year <= EndYear).AsQueryable();
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