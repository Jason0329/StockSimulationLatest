using Autofac;
using StockSimulationMVC.Core;
using StockSimulationMVC.Interface;
using StockSimulationMVC.Models;
using StockSimulationMVC.ObjectContext;
using StockSimulationMVC.Simulation_SimulationStart;
using StockSimulationMVC.Strategy;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace StockSimulationMVC
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DataObjectContext ObjectContext = new DataObjectContext();
            ObjectContext.Install();

            InitialData.SetYear(2016, 2020);
            InitialData.Initial();
            //Strategy_Jason1 Strategy = new Strategy_Jason1();
            //SimulationStart Start = new SimulationStart(Strategy);
            //Start.Run();


            //LineGraph line = new LineGraph();
            //line.AddLineGraphDictionary("MoveAverageValue",3);
            //line.AddLineGraphDictionary("MaxValue", 3);
            //line.AddLineGraphDictionary("MinValue", 3);
            //line.AddLineGraphDictionary("Acculation", 3);

            Database.SetInitializer<DataObjectContext>(new DropCreateDatabaseIfModelChanges<DataObjectContext>());

            var builder = new ContainerBuilder();
            
            builder.RegisterType<GenericRepository<BasicFinancialContainParentDataModel>>().As<IRepository<BasicFinancialContainParentDataModel>>()
                .InstancePerLifetimeScope();

            //var container = builder.Build();
            
            //BasicFinancialContainParentDataModel tt = new BasicFinancialContainParentDataModel();
            //tt.ID = 5;
            //tt.Company = 324;
            //tt.CompanyName = "51423";
            ////tt.ProfitAfterTax = 325423;
            //tt.Date = DateTime.Now;

            //container.Resolve<IRepository<BasicFinancialContainParentDataModel>>(new TypedParameter(typeof(DbContext), new DataObjectContext())).Create(tt);



            //InitialData.Initial();

            //List<BasicFinancialDataModel> aa = InitialData.InitialData_BasicFinancialData;
            //var bb = InitialData.InitialData_BasicFinancialData.OrderBy(m => m.Date).Select(m => m.ID == 1).ToList() ;
            ////List<BasicFinancialDataModel> cc = (List<BasicFinancialDataModel>)bb;

            ////builder.RegisterType<GenericRepository<BasicFinancialDataModel>>().As<IRepository<BasicFinancialDataModel>>()
            ////    .InstancePerLifetimeScope();

            ////var container = builder.Build();

            ////BasicFinancialDataModel tt = new BasicFinancialDataModel();
            ////tt.ID = 1;
            ////tt.LongTermLiabilities = 324;
            ////tt.OthersLiabilities = 51423;
            ////tt.ProfitAfterTax = 325423;
            ////tt.Date = DateTime.Now;
            ////List<BasicFinancialDataModel> aa;
            ////container.Resolve<IRepository<BasicFinancialDataModel>>(new TypedParameter(typeof(DbContext), new DataObjectContext())).Create(tt);

            //var builder1 = new ContainerBuilder();



        }
    }
}
