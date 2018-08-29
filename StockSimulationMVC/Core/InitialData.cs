using Autofac;
using StockSimulationMVC.Interface;
using StockSimulationMVC.Models;
using StockSimulationMVC.ObjectContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace StockSimulationMVC.Core
{
    public static class InitialData
    {
        public static List<BasicFinancialDataModel> InitialData_BasicFinancialData;
        public static List<BasicFinancialContainParentDataModel> InitialData_BasicFinancialDataContainParent;
        public static List<MonthRevenueModel> InitialData_MonthRevenueData;
        public static List<TechnologicalDataModel> InitialData_TechnologicalData;
        public static List<CompanyModel> InitialData_CompanyData;
        public static int StartYear=2016;
        public static int EndYear = 2020;

        public static void Initial()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<GenericRepository<BasicFinancialDataModel>>().As<IRepository<BasicFinancialDataModel>>()
                .InstancePerLifetimeScope();
            builder.RegisterType<GenericRepository<MonthRevenueModel>>().As<IRepository<MonthRevenueModel>>()
               .InstancePerLifetimeScope();
            builder.RegisterType<GenericRepository<TechnologicalDataModel>>().As<IRepository<TechnologicalDataModel>>()
               .InstancePerLifetimeScope();
            builder.RegisterType<GenericRepository<CompanyModel>>().As<IRepository<CompanyModel>>()
               .InstancePerLifetimeScope();
            builder.RegisterType<GenericRepository<BasicFinancialContainParentDataModel>>().As<IRepository<BasicFinancialContainParentDataModel>>()
                .InstancePerLifetimeScope();

            var container = builder.Build();

            container.Resolve<IRepository<BasicFinancialContainParentDataModel>>(new TypedParameter(typeof(DbContext), new DataObjectContext())).SetYearRange(StartYear, EndYear);
            InitialData_BasicFinancialDataContainParent = container.Resolve<IRepository<BasicFinancialContainParentDataModel>>(new TypedParameter(typeof(DbContext), new DataObjectContext())).GetAll().ToList();


            container.Resolve<IRepository<BasicFinancialDataModel>>(new TypedParameter(typeof(DbContext), new DataObjectContext())).SetYearRange(StartYear, EndYear);
            InitialData_BasicFinancialData = container.Resolve<IRepository<BasicFinancialDataModel>>(new TypedParameter(typeof(DbContext), new DataObjectContext())).GetAll().ToList();

            container.Resolve<IRepository<MonthRevenueModel>>(new TypedParameter(typeof(DbContext), new DataObjectContext())).SetYearRange(StartYear, EndYear);
            InitialData_MonthRevenueData = container.Resolve<IRepository<MonthRevenueModel>>(new TypedParameter(typeof(DbContext), new DataObjectContext())).GetAllMonthRevenue().ToList();

            container.Resolve<IRepository<TechnologicalDataModel>>(new TypedParameter(typeof(DbContext), new DataObjectContext())).SetYearRange(StartYear, EndYear);
            InitialData_TechnologicalData = container.Resolve<IRepository<TechnologicalDataModel>>(new TypedParameter(typeof(DbContext), new DataObjectContext())).GetAllTech().ToList();
            InitialData_CompanyData = container.Resolve<IRepository<CompanyModel>>(new TypedParameter(typeof(DbContext), new DataObjectContext())).GetAll().ToList();

        }

        public static void SetYear(int StartYear , int EndYear)
        {
            InitialData.StartYear = StartYear;
            InitialData.EndYear = EndYear;
        }
    }
}
