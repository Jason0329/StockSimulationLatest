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
        int CountDropDaysParameter = 4;
        bool StartBuy = false;
        Decimal ReferencePrice = 0;


        public Strategy_Testing(Hashtable Setup)
        {
            try
            {
                CountDropDaysParameter = int.Parse(Setup["CountDropDaysParameter"].ToString());
                Acc = int.Parse(Setup["StopEarn"].ToString());
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

            if (dataList.ReturnValue("MoveAverageValue-20", j-1) < dataList.ReturnValue("MoveAverageValue-1", j-1) * 1.05
                &&dataList.ReturnValue("MoveAverageValue-20", j) > dataList.ReturnValue("MoveAverageValue-1", j) * 1.05
                && dataList.ReturnValue("MoveAverageValue-30", j) > 200)//CountDropDays == CountDropDaysParameter )
            {
                CountDropDays = 0;
                return true;   
            }

            return false;
        }

        public bool SellCondition(ref SimulationVariable simulationVariable, ref DataList dataList, ref BasicFinancialReportListModel financialdata, int j)
        {
            //if (dataList.TechData[j].ReturnOnInvestment > 5)
            //    return false;

            if ((simulationVariable.Accumulation > 2 )//&& dataList.ReturnValue("MoveAverageValue-20", j) < dataList.ReturnValue("MoveAverageValue-1", j) * 1.1)
                || (simulationVariable.Accumulation < -2 )// //|| simulationVariable.MaxRatio > 5
                                                                                                                                                                 //|| (simulationVariable.HaveStockDayContainHoliday > 20 && simulationVariable.Accumulation > 1)
                 )
                return true;

            return false;
        }
    }
}