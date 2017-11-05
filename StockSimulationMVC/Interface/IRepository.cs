using StockSimulationMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockSimulationMVC.Interface
{
    public interface IRepository<TEntity> : IDisposable
      where TEntity : class
    {
        void Create(TEntity instance);

        void Update(TEntity instance);

        void Delete(TEntity instance);

        TEntity Get(int primaryID);

        IQueryable<TEntity> GetAll();

        void SaveChanges();


        IQueryable<TechnologicalDataModel> GetAllTech();
        IQueryable<MonthRevenueModel> GetAllMonthRevenue();
        IQueryable<BasicFinancialDataModel> GetAllBasic();

        void SetYearRange(int StartYear, int EndYear);

    }
}