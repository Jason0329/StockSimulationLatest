﻿using StockSimulationMVC.Core;
using StockSimulationMVC.Interface;
using StockSimulationMVC.Models;
using StockSimulationMVC.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace StockSimulationMVC.Simulation_SimulationStart
{
    public class SimulationStart
    {
        readonly int SimualationDays = 1000;
        List<int> Company;//還未初始化
        
        TransactionList Transaction_List;
        private IStrategy _strategy;
        ////StreamWriter sw = new StreamWriter(@"C:\Users\user\Desktop\Dataselected_company.txt");
        
        public SimulationStart(IStrategy strategy , string UrlQuery = "Database" )
        {
            Company = new List<int>();
            
           if (UrlQuery.Contains("File"))
            {
                StreamReader sr = new StreamReader(@"C:\Users\user\Desktop\Data\FileCompanyData.csv");
                string[] data = sr.ReadLine().Trim(',').Split(',');

                foreach (var company in data)
                {
                    Company.Add(int.Parse(company));
                }
                sr.Close();
            }
            else 
            {
                var CoData = from _company in InitialData.InitialData_CompanyData
                                 //where _company.ID >2200 && _company.ID <2340
                             select _company.Company;
                Company = (List<int>)CoData.ToList();
            }
            





                Transaction_List = new TransactionList();
            _strategy = strategy;
            
            
        }
      
        public TransactionList Run()
        {
            //之後平行化處理看看
            for(int i=0; i< Company.Count; i++)
            {               
                SimulationVariable _SimulationVariable = new SimulationVariable();
                Transaction transaction = new Transaction() ;
                DataList DataList = new DataList(Company[i]);
                BasicFinancialReportListModel BasicFinancialReportData = new BasicFinancialReportListModel();

                //////////////////
                DataList.LineGraphData(ref DataList.TechData, "ClosePrice");
                DataList.LineGraphData(ref DataList.TechData, "Volume");
                DataList.AddLineGraphDictionary("MoveAverageValue", 5);
                DataList.AddLineGraphDictionary("MoveAverageValue", 10);
                DataList.AddLineGraphDictionary("MoveAverageValue", 20);
                DataList.AddLineGraphDictionary("MoveAverageValue", 60);
                DataList.AddLineGraphDictionary("MinValue", 1);
                DataList.AddLineGraphDictionary("MinValue", 20, "Volume");
                DataList.AddLineGraphDictionary("BollingerBandsDown", 20, "ClosePrice", 2.4);
                DataList.AddLineGraphDictionary("BollingerBandsUp", 20, "ClosePrice", 2.1);
                DataList.AddLineGraphDictionary("MoveAverageValue", 1);

                DataList.AddLineGraphDictionary("MoveAverageValue", 30, "Volume");
                DataList.AddLineGraphDictionary("MoveAverageValue", 6, "Volume");



                if (DataList.TechData.Count == 0) continue;

                //BasicFinancialReportData.Initial(DataList.TechData[0].Company);
                //////////////////
                SortedDictionary<Decimal, int> Data1 = new SortedDictionary<decimal, int>();
                SortedDictionary<Decimal, int> Data2 = new SortedDictionary<decimal, int>();
                SortedDictionary<Decimal, int> Data3 = new SortedDictionary<decimal, int>();
                ////Decimal AverageMonthPrice = 0;
                ////Decimal CountDay = 0;



                for (int j =1; j < DataList.TechData.Count; j++)
                {
                    #region 
                    //////////////////
                    BasicFinancialReportData.InitialDate(DataList.TechData[j].Date);
                    //////////////////


                    if (_SimulationVariable.HasBuy)
                    {
                        _SimulationVariable.CountDays(DataList.TechData[j]);// put TechData Return on Investment
                    }

                    #region 看買賣條件
                    BuySell_Condition(ref _SimulationVariable, ref DataList, ref BasicFinancialReportData, j);
                    #endregion

                    if (_SimulationVariable.CanBuy && !_SimulationVariable.HasBuy)
                    {
                        transaction = new Transaction();
                        transaction.Buy(DataList.TechData[j].Company.ToString(), DataList.TechData[j].CompanyName,
                            DataList.TechData[j].ClosePrice, DataList.TechData[j].Date);

                        _SimulationVariable.HasBuy = true;
                        _SimulationVariable.Buy = DataList.TechData[j];

                    }

                    if (_SimulationVariable.HasBuy && !_SimulationVariable.CanBuy)
                    {
                        transaction.Sell(DataList.TechData[j].Company.ToString(), DataList.TechData[j].CompanyName,
                            DataList.TechData[j].ClosePrice, DataList.TechData[j].Date);

                        Transaction_List._TransactionList.Add(transaction);

                        _SimulationVariable.HasBuy = false;
                        _SimulationVariable.Initial();
                    }
                    #endregion

                    ////if (DataList.TechData[1].Date.Year != 2014 || DataList.TechData[1].Date.Month != 3
                    ////    || DataList.TechData[1].Volume < 300) break;

                    ////try
                    ////{

                    ////    if ((DataList.TechData[j].Date.Month == 6 && DataList.TechData[j - 1].Date.Month == 5))
                    ////    {
                    ////        if (DataList.TechData[j].Date.Year == 2014)
                    ////            Data1.Add(AverageMonthPrice / CountDay, 1);
                    ////        if (DataList.TechData[j].Date.Year == 2015)
                    ////            Data2.Add(AverageMonthPrice / CountDay, 1);
                    ////        if (DataList.TechData[j].Date.Year == 2016)
                    ////            Data3.Add(AverageMonthPrice / CountDay, 1);
                    ////        AverageMonthPrice = 0;
                    ////        CountDay = 0;
                    ////    }
                    ////    if ((DataList.TechData[j].Date.Month == 9 && DataList.TechData[j - 1].Date.Month ==8))
                    ////    {
                    ////        if (DataList.TechData[j].Date.Year == 2014)
                    ////            Data1.Add(AverageMonthPrice / CountDay, 2);
                    ////        if (DataList.TechData[j].Date.Year == 2015)
                    ////            Data2.Add(AverageMonthPrice / CountDay, 2);
                    ////        if (DataList.TechData[j].Date.Year == 2016)
                    ////            Data3.Add(AverageMonthPrice / CountDay, 2);
                    ////        AverageMonthPrice = 0;
                    ////        CountDay = 0;
                    ////    }
                    ////    if ((DataList.TechData[j].Date.Month == 12 && DataList.TechData[j - 1].Date.Month == 11))
                    ////    {
                    ////        if (DataList.TechData[j].Date.Year == 2014)
                    ////            Data1.Add(AverageMonthPrice / CountDay, 3);
                    ////        if (DataList.TechData[j].Date.Year == 2015)
                    ////            Data2.Add(AverageMonthPrice / CountDay, 3);
                    ////        if (DataList.TechData[j].Date.Year == 2016)
                    ////            Data3.Add(AverageMonthPrice / CountDay, 3);

                    ////        AverageMonthPrice = 0;
                    ////        CountDay = 0;
                    ////    }
                    ////    if ((DataList.TechData[j].Date.Month == 3 && DataList.TechData[j - 1].Date.Month == 2))
                    ////    {
                    ////        if (DataList.TechData[j].Date.Year == 2015)
                    ////            Data1.Add(AverageMonthPrice / CountDay, 4);
                    ////        if (DataList.TechData[j].Date.Year == 2016)
                    ////            Data2.Add(AverageMonthPrice / CountDay, 4);
                    ////        if (DataList.TechData[j].Date.Year == 2017)
                    ////            Data3.Add(AverageMonthPrice / CountDay, 4);

                    ////        AverageMonthPrice = 0;
                    ////        CountDay = 0;
                    ////    }
                    ////}
                    ////catch(Exception e)
                    ////{
                    ////    break;
                    ////}

                    ////AverageMonthPrice += DataList.TechData[j].ClosePrice;
                    ////CountDay++;

                }

                ////if (Data1.Count != 4 || Data2.Count != 4 || Data3.Count != 4) continue;
                ////bool flag = true;
                //////for (int ii= 0 ;ii < Data1.Count; ii++)
                //////{
                //////    try
                //////    {
                //////        var item1 = Data1.ElementAt(ii);
                //////        var item2 = Data2.ElementAt(ii);
                //////        var item3 = Data3.ElementAt(ii);
                //////        if (item1.Value != item2.Value || item2.Value != item3.Value)
                //////        {
                //////            flag = false;
                //////        }
                //////        if (Data1.Count != 4 || Data2.Count != 4 || Data3.Count != 4)
                //////            flag = false;


                //////    }
                //////    catch(Exception ee)
                //////    {
                //////        flag = false;
                //////    }
                //////}
                ////if (Data1.ElementAt(0).Value != Data2.ElementAt(0).Value
                ////    || Data2.ElementAt(3).Value!= Data1.ElementAt(3).Value)
                ////    flag = false;
                ////if (Data2.ElementAt(0).Value != Data3.ElementAt(0).Value
                ////    || Data2.ElementAt(3).Value != Data3.ElementAt(3).Value)
                ////    flag = false;

                ////if (Data1.ElementAt(0).Key / Data2.ElementAt(0).Key > 1.1m)
                ////    flag = false;

                ////if (Data1.ElementAt(0).Key * 1.1m > Data1.ElementAt(3).Key)
                ////    flag = false;
                ////if (Data2.ElementAt(0).Key * 1.1m > Data2.ElementAt(3).Key)
                ////    flag = false;
                ////if (Data3.ElementAt(0).Key * 1.1m > Data3.ElementAt(3).Key)
                ////    flag = false;

                ////if (flag)
                ////{
                ////    sw.Write(Company[i] + ",");
                ////}


                if (_SimulationVariable.HasBuy) //期末賣出
                {
                    transaction.Sell(DataList.TechData[DataList.TechData.Count - 1].Company.ToString(), DataList.TechData[DataList.TechData.Count - 1].CompanyName,
                        DataList.TechData[DataList.TechData.Count - 1].ClosePrice, DataList.TechData[DataList.TechData.Count - 1].Date);
                    //期末賣出的不做計算
                    //Transaction_List._TransactionList.Add(transaction);

                    _SimulationVariable.HasBuy = false;
                    _SimulationVariable.Initial();
                }
            }
            //sw.Close();
            return Transaction_List;
        }

        private void BuySell_Condition(ref SimulationVariable simulationVariable, ref DataList dataList,ref BasicFinancialReportListModel BasicFinancialData, int j)
        {
            if (!simulationVariable.HasBuy && _strategy.BuyCondition(ref simulationVariable, ref dataList, ref BasicFinancialData, j))
            {
                simulationVariable.CanBuy = true;
            }

            if (simulationVariable.HasBuy && _strategy.SellCondition(ref simulationVariable, ref dataList, ref BasicFinancialData, j))
            {
                simulationVariable.CanBuy = false;
            }
            
        }
    }
}