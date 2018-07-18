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
    public class CountSerialDrop_Strategy_3679:IStrategy
    {
        int CountDropDays = 0;
        public double Acc = 5;
        public double StopEarn = 5;
        int CountDropDaysParameter = 0;
        bool StartBuy = false;
        Decimal ReferencePrice = 0;


        public CountSerialDrop_Strategy_3679(Hashtable Setup)
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

        public CountSerialDrop_Strategy_3679() { }



        public bool BuyCondition(ref SimulationVariable simulationVariable, ref DataList dataList, ref BasicFinancialReportListModel financialdata, int j)
        {
            if (!simulationVariable.HasBuy && dataList.TechData[j].ReturnOnInvestment < 0)
                CountDropDays++;
            else if (dataList.TechData[j].ReturnOnInvestment != 0 || simulationVariable.HasBuy)
                CountDropDays = 0;


            if (CountDropDays == CountDropDaysParameter)
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

            if (simulationVariable.Accumulation > 3
                || simulationVariable.Accumulation < -3 || simulationVariable.MaxRatio > 4
                || (simulationVariable.HaveStockDayContainHoliday > 10 && simulationVariable.Accumulation > 1)
                 )
                return true;

            return false;
        }
    }
}