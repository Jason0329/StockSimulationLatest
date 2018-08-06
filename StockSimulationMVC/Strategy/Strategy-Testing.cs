using StockSimulationMVC.Interface;
using StockSimulationMVC.Models;
using StockSimulationMVC.Simulation_SimulationStart;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockSimulationMVC.Strategy
{
    public class Strategy_Testing : IStrategy
    {
        int CountDropDays = 0;
        public double Acc = 5;
        public double StopEarn = 5;
        public double StopLoss = 3;
        public double Var = 1;
        public int Company = 0;
        public bool CheckCompany = false;
        int CountDropDaysParameter = 4;
        bool StartBuy = false;
        Decimal ReferencePrice = 0;


        public Strategy_Testing(Hashtable Setup)
        {
            try
            {
                //CountDropDaysParameter = int.Parse(Setup["CountDropDaysParameter"].ToString());
                //Acc = int.Parse(Setup["StopEarn"].ToString());
                Var = double.Parse(Setup["Var"].ToString());
                Company = int.Parse(Setup["Company"].ToString());
            }
            catch (Exception ee)
            {
                CountDropDaysParameter = 4;
            }

        }

        public Strategy_Testing() { }



        public bool BuyCondition(ref SimulationVariable simulationVariable, ref DataList dataList, ref BasicFinancialReportListModel financialdata, int j)
        {
            if(j-2<0) return false;
            if (!simulationVariable.HasBuy && dataList.TechData[j].ReturnOnInvestment < 0)
                CountDropDays++;
            else if (dataList.TechData[j].ReturnOnInvestment != 0 || simulationVariable.HasBuy)
                CountDropDays = 0;

            if(Company!=0)
            {
                CheckCompany = true;
            }

            if (//CountDropDays==3
                //dataList.TechData[j-1].ReturnOnInvestment > Var && dataList.TechData[j-1].ReturnOnInvestment<Var+1
                dataList.TechData[j - 1].ClosePrice > dataList.TechData[j - 1].OpenPrice
                && dataList.TechData[j ].OpenPrice < dataList.TechData[j - 1].HighestPrice
                && dataList.TechData[j].OpenPrice > dataList.TechData[j - 1].LowestPrice


                //&& dataList.TechData[j].OpenPrice > dataList.TechData[j-1].ClosePrice
                //    && dataList.TechData[j-1].OpenPrice < dataList.TechData[j - 2].HighestPrice
                //((CheckCompany && Company == dataList.TechData[j].Company ) || !CheckCompany)
                //&&dataList.ReturnValue("MoveAverageValue-20", j - 1)  < dataList.ReturnValue("MoveAverageValue-1", j - 1) * Var
                //&& dataList.ReturnValue("MoveAverageValue-20", j) > dataList.ReturnValue("MoveAverageValue-1", j) * Var
                && dataList.ReturnValue("MinValue-20", j) > 200)//CountDropDays == CountDropDaysParameter )
                
            {
                CountDropDays = 0;
                return true;   
            }

            return false;
        }

        public bool SellCondition(ref SimulationVariable simulationVariable, ref DataList dataList, ref BasicFinancialReportListModel financialdata, int j)
        {


            //if ((simulationVariable.Accumulation > Acc)//&& dataList.ReturnValue("MoveAverageValue-20", j) < dataList.ReturnValue("MoveAverageValue-1", j) * 1.1)
            //    || (simulationVariable.Accumulation < -StopLoss)// //|| simulationVariable.MaxRatio > 5
            //                                             //|| (simulationVariable.HaveStockDayContainHoliday > 20 && simulationVariable.Accumulation > 1)
            //     )
                return true;

            return false;
        }
    }
}