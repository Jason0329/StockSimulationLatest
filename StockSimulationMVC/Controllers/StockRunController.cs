﻿using StockSimulationMVC.Core;
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

namespace StockSimulationMVC.Controllers
{
    public class StockRunController : Controller
    {
        // GET: StockRun
        public ActionResult Index()
        {
            Strategy_Jason1 Strategy = new Strategy_Jason1();
            SimulationStart Start = new SimulationStart(Strategy);
            TransactionList Data = Start.Run();
            Data._TransactionList.Sort();
            Data.TransactionStatisticResult();

            Session["DataResult"] = Data;

            return View(Data);

        }

        public ActionResult Optimize()
        {
            List<TransactionList> OptimizeList = new List<TransactionList>();
            
            OptimizeStock _OptimizeStock = new OptimizeStock();

            string GetParametersQuery = Request.Url.Query;
            Hashtable GetParameters = new Hashtable();

            foreach(var data in GetParametersQuery.Trim('?').Split('&'))
            {
                GetParameters.Add(data.Split('=')[0], data.Split('=')[1]);
            }

            Strategy_Jason1 Strategy = new Strategy_Jason1();

            for (int i=3; i<=10; i++)
            {
                Strategy.Acc = i;
                SimulationStart Start = new SimulationStart(Strategy);
                TransactionList Data = Start.Run();
                //Data._TransactionList.Sort();
               
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
    }
}