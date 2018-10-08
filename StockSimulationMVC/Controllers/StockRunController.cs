using StockSimulationMVC.Core;
using StockSimulationMVC.Interface;
using StockSimulationMVC.Models;
using StockSimulationMVC.Service;
using StockSimulationMVC.Simulation_SimulationStart;
using StockSimulationMVC.Simulation_Test;
using StockSimulationMVC.Strategy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace StockSimulationMVC.Controllers
{
    public class StockRunController : Controller
    {
        // GET: StockRun
        public ActionResult Index()
        {
            string GetParametersQuery = Request.Url.Query;
            Hashtable GetParameters = new Hashtable();

            foreach (var reqdata in GetParametersQuery.Trim('?').Split('&'))
            {
                try
                {
                    GetParameters.Add(reqdata.Split('=')[0], reqdata.Split('=')[1]);
                }
                catch (Exception e)
                { }
            }

            string UrlQuery = Request.Url.Query;


            ZhaoFinancialSelectStockStrategy Strategy = new ZhaoFinancialSelectStockStrategy(GetParameters);
            SimulationStart Start = new SimulationStart(Strategy, UrlQuery);
            TransactionList Data = Start.Run();
            Data._TransactionList.Sort();
            Data.TransactionStatisticResult();

            Session["DataResult"] = Data;

            return View(Data);

        }

        public ActionResult Optimize()
        {
            List<TransactionList> OptimizeList = new List<TransactionList>();
            string UrlQuery = Request.Url.Query;

            OptimizeStock _OptimizeStock = new OptimizeStock();

            string GetParametersQuery = Request.Url.Query;
            Hashtable GetParameters = new Hashtable();

            foreach(var data in GetParametersQuery.Trim('?').Split('&'))
            {
                GetParameters.Add(data.Split('=')[0], data.Split('=')[1]);
            }

            Strategy_2330 Strategy = new Strategy_2330(GetParameters);

            for (int i=1; i<=19; i+=1)
            {
                Strategy.Acc = i;
                SimulationStart Start = new SimulationStart(Strategy, UrlQuery);
                TransactionList Data = Start.Run();
                Data._TransactionList.Sort();
               
                TransactionList OptimizeData = _OptimizeStock.OptimizeCompany(Data,GetParameters);
                OptimizeData.TransactionStatisticResult();
                OptimizeList.Add(OptimizeData);
            }

            Session["ResultStore"] = OptimizeList;
           
            return View("~/Views/Optimize/OptimizeList.cshtml", OptimizeList);

        }

        //public ActionResult SimTransaction()
        //{
        //    TransactionList data = (TransactionList)Session["DataResult"];
        //    SimulateTransaction simulate = new SimulateTransaction(ref data._TransactionList);
        //    simulate.Run();

        //    return View("~/Views/StockRun/SimTransaction.cshtml", simulate.CapitalSimList);
        //}

        public ActionResult SimTransaction(int id)
        {
            string GetParametersQuery = Request.Url.Query;
            Hashtable GetParameters = new Hashtable();

            foreach (var reqdata in GetParametersQuery.Trim('?').Split('&'))
            {
                GetParameters.Add(reqdata.Split('=')[0], reqdata.Split('=')[1]);
            }

            id--;
            List<TransactionList> OptimizeList = (List<TransactionList>)Session["ResultStore"];
            TransactionList data = OptimizeList[id];
            SimulateTransaction simulate = new SimulateTransaction(ref data._TransactionList, double.Parse(GetParameters["InitialCapital"].ToString()));
            simulate.BuyFreq = int.Parse(GetParameters["BuyFreq"].ToString());
            simulate.Run();

            return View("~/Views/StockRun/SimTransaction.cshtml", simulate.CapitalSimList);
        }

        public ActionResult Detail(int id)
        {
            id--;
            List<TransactionList> OptimizeList = (List<TransactionList>)Session["ResultStore"];

            return View("~/Views/StockRun/Detail.cshtml",OptimizeList[id]);
        }

        public ActionResult Output(int id)
        {
            id--;
            List<TransactionList> OptimizeList = (List<TransactionList>)Session["ResultStore"];

            StreamWriter sw = new StreamWriter(@"C:\Users\user\Documents\visual studio 2017\Projects\StockSimulationMVC\StockSimulationMVC\Strategy-" + id+".csv");
            Hashtable Record = new Hashtable();
            
            foreach(var data in OptimizeList[id]._TransactionList)
            {
                try
                {
                    Record.Add(data.BuyDetail.Nubmer, data.BuyDetail.Nubmer);
                    sw.Write(data.BuyDetail.Nubmer + ",");
                }
                catch(Exception e)
                { }

            }

            sw.Close();
            sw.Dispose();

            return View("~/Views/StockRun/Detail.cshtml", OptimizeList[id]);
        }

        public ActionResult AutoOptimizeStrategyAndCompany()
        {
            List<TransactionList> OptimizeList = new List<TransactionList>();

            OptimizeStock _OptimizeStock = new OptimizeStock();

            string GetParametersQuery = Request.Url.Query;
            Hashtable GetParameters = new Hashtable();

            foreach (var data in GetParametersQuery.Trim('?').Split('&'))
            {
                GetParameters.Add(data.Split('=')[0], data.Split('=')[1]);
            }

            ZhaoFinancialSelectStockStrategy Strategy = new ZhaoFinancialSelectStockStrategy(GetParameters);

            for (int i = 0; i < 1; i++)
            {
                InitialData.SetYear(2016 - i, 2019 - i);
                InitialData.Initial();
                SimulationStart Start = new SimulationStart(Strategy);
                TransactionList Data = Start.Run();
                //Data._TransactionList.Sort();

                TransactionList OptimizeData = _OptimizeStock.OptimizeCompany(Data, GetParameters);
                OptimizeData.TransactionStatisticResult();
                OptimizeList.Add(OptimizeData);
            }

            Hashtable CandidateCompany = _OptimizeStock.OutputCompany(OptimizeList[0]);
            Hashtable StoreCompany = _OptimizeStock.OutputCompany(OptimizeList[0]);

          



            for (int i = 1; i < OptimizeList.Count; i++)
            {
                Hashtable FilterCompany = _OptimizeStock.OutputCompany(OptimizeList[i]);

                foreach (var check_company in CandidateCompany.Values)
                {
                   
                    if (!FilterCompany.ContainsValue(check_company))
                    {
                        StoreCompany.Remove(check_company);
                    }
                }
            }

            StreamWriter sw = new StreamWriter(@"C:\Users\user\Desktop\Data\FileCompanyData.csv");
            foreach (var company in StoreCompany.Values)
            {
                sw.Write("m.Company==" + company + "||");
            }
            sw.Close();


            return View("~/Views/Optimize/CompanyList.cshtml", StoreCompany);
        }
    }
}