using StockSimulationMVC.Core;
using StockSimulationMVC.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockSimulationMVC.Service
{
    public class OptimizeStock
    {

        public TransactionList OptimizeCompany(TransactionList AllCompanyTransaction)
        {
            List<int> Company;
            Company = new List<int>();
            var CoData = from _company in InitialData.InitialData_CompanyData
                         select _company.Company;
            Company = (List<int>)CoData.ToList();

            
            TransactionList ReturnTransactionList = new TransactionList();

            for (int i=0; i<Company.Count; i++)
            {
                var Data = from data in AllCompanyTransaction._TransactionList
                           where data.BuyDetail.Nubmer==Company[i].ToString()
                           select data;

                TransactionList transactionlist = new TransactionList();
                transactionlist._TransactionList = (List<Transaction>)Data.ToList();
                transactionlist.TransactionStatisticResult();

                System.Diagnostics.Debug.WriteLine(i + " " + transactionlist._TransactionList.Count +" " + transactionlist.WinRatio);

                if (transactionlist._TransactionList.Count>3 && transactionlist.WinRatio>80)
                    ReturnTransactionList.AddTransactionList(transactionlist._TransactionList);
            }


            return ReturnTransactionList;
        }

        public TransactionList OptimizeCompany(TransactionList AllCompanyTransaction,Hashtable parameter=null)
        {
            int TransactionCount; 
            double WinRatio;
            double AverageHoldDays; 

            try
            {
                TransactionCount = int.Parse(parameter["TransactionCount"].ToString());
                WinRatio = double.Parse(parameter["WinRatio"].ToString());
                AverageHoldDays= double.Parse(parameter["AverageHoldDays"].ToString());
            }
            catch(Exception e)
            {
                TransactionCount = 3;
                WinRatio = 70;
                AverageHoldDays = 3000;
            }


            List<int> Company;
            Company = new List<int>();
            var CoData = from _company in InitialData.InitialData_CompanyData
                         select _company.Company;
            Company = (List<int>)CoData.ToList();


            TransactionList ReturnTransactionList = new TransactionList();

            for (int i = 0; i < Company.Count; i++)
            {
                var Data = from data in AllCompanyTransaction._TransactionList
                           where data.BuyDetail.Nubmer == Company[i].ToString()
                           select data;

                TransactionList transactionlist = new TransactionList();
                transactionlist._TransactionList = (List<Transaction>)Data.ToList();
                transactionlist.TransactionStatisticResult();

                System.Diagnostics.Debug.WriteLine(i + " " + transactionlist._TransactionList.Count + " " + transactionlist.WinRatio);

                if (transactionlist._TransactionList.Count >= TransactionCount && transactionlist.WinRatio >= WinRatio 
                    && transactionlist.AverageHoldDays<=AverageHoldDays)
                    ReturnTransactionList.AddTransactionList(transactionlist._TransactionList);
            }


            return ReturnTransactionList;
        }

        public Hashtable OutputCompany(TransactionList CompanyData)
        {
            Hashtable Record = new Hashtable();
            foreach(var Data in CompanyData._TransactionList)
            {
                try
                {
                    Record.Add(Data.BuyDetail.Nubmer, Data.BuyDetail.Nubmer);
                }
                catch (Exception e)
                { }
            }

            return Record;
        }
    }
}