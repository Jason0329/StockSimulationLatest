using StockSimulationMVC.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StockSimulationMVC.Models;
using StockSimulationMVC.Simulation_SimulationStart;
using System.Collections;

namespace StockSimulationMVC.Strategy
{
    public class Strategy_Jason1 : IStrategy //2 3 3 0
    {
        int CountDropDays = 0;
        public double Acc = 5;
        public double StopEarn = 5;
        int CountDropDaysParameter = 3;
        bool StartBuy = false;
        Decimal ReferencePrice = 0;


        public Strategy_Jason1(Hashtable Setup)
        {
            try
            {
                CountDropDaysParameter = int.Parse(Setup["CountDropDaysParameter"].ToString());
                StopEarn= int.Parse(Setup["StopEarn"].ToString());
            }
            catch(Exception ee)
            {
                CountDropDaysParameter = 3;
            }

        }

        public Strategy_Jason1()
        { }

        public bool BuyCondition(ref SimulationVariable simulationVariable, ref DataList dataList, ref BasicFinancialReportListModel financialdata, int j)
        {
            if (!simulationVariable.HasBuy && dataList.TechData[j].ReturnOnInvestment < 0)
                CountDropDays++;
            else if(dataList.TechData[j].ReturnOnInvestment!=0 || simulationVariable.HasBuy)
                CountDropDays = 0;

            bool fin = financialdata.ComparerFinancial("QEarningPerShare", 1, 2);
            bool fin1 = financialdata.ComparerMonthlyRevenue("MoMPercentage_MonthlySale", 10,1,false);
            if (CountDropDays == CountDropDaysParameter
                &&  double.Parse(dataList.TechData[j].Volume.ToString())  > 200 //成交額大於二千萬
                )

            {
                return true;
                StartBuy = true;
                ReferencePrice = dataList.TechData[j].ClosePrice;
            }

            if (StartBuy

               
                && ReferencePrice * 0.99m > dataList.TechData[j].ClosePrice
                )
            {
                StartBuy = false;
                return true;
            }

            if (StartBuy


               //&& dataList.CoditionSatified("BollingerBandsDown-20", "MoveAverageValue-1", j)
               && ReferencePrice * 1.05m < dataList.TechData[j].ClosePrice
               )
            {
                StartBuy = false; 
            }

            return false;
        }

        public bool SellCondition(ref SimulationVariable simulationVariable, ref DataList dataList, ref BasicFinancialReportListModel financialdata, int j)
        {
            if ( simulationVariable.Accumulation > 5
                 || simulationVariable.Accumulation < -5)
                return true;

            return false;
        }
    }
}