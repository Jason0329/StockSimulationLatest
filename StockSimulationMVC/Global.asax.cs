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


            Database.SetInitializer<DataObjectContext>(new DropCreateDatabaseIfModelChanges<DataObjectContext>());

            var builder = new ContainerBuilder();
            
            builder.RegisterType<GenericRepository<BasicFinancialContainParentDataModel>>().As<IRepository<BasicFinancialContainParentDataModel>>()
                .InstancePerLifetimeScope();





        }
    }
}
