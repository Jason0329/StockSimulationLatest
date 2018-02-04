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
    public class Strate_2330_CountRaiseDays : IStrategy
    {
        public double Acc = 2;
        int CountRaiseDaysParameter = 3;
        int CountRaiseDays = 0;
        bool StartBuy = false;

        public Strate_2330_CountRaiseDays(Hashtable Setup)
        {
            try
            {

                Acc = int.Parse(Setup["StopEarn"].ToString());
                CountRaiseDaysParameter = (int)Acc;


            }
            catch (Exception ee)
            {

            }

        }

        public Strate_2330_CountRaiseDays(){}
        public bool BuyCondition(ref SimulationVariable simulationVariable, ref DataList dataList, ref BasicFinancialReportListModel financialdata, int j)
        {
            if (!simulationVariable.HasBuy && dataList.TechData[j].ReturnOnInvestment > 0)
                CountRaiseDays++;
            else if (dataList.TechData[j].ReturnOnInvestment != 0 || simulationVariable.HasBuy)
                CountRaiseDays = 0;

            if (CountRaiseDays == CountRaiseDaysParameter)
                StartBuy = true;

            if (StartBuy
                //|| dataList.ReturnValue("CountRaiseinDays-20", j) > 6
                )
            {
                StartBuy = false;
                return true;
            }

            return false;
        }

        public bool SellCondition(ref SimulationVariable simulationVariable, ref DataList dataList, ref BasicFinancialReportListModel financialdata, int j)
        {
            simulationVariable.MoveStopLossPercentage = Acc;
            if (
                // (dataList.ReturnValue("CountRaiseinDays-20", j) > 9 && simulationVariable.Accumulation > 1)
                //|| (dataList.ReturnValue("CountRaiseinDays-20", j) > 9 && simulationVariable.Accumulation < -2)
                 simulationVariable.Accumulation > 2
                || simulationVariable.Accumulation < -2
                )
                return true;
            return false;
        }
    }
}