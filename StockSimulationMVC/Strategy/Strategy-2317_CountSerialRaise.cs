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
    public class Strategy_2317_CountSerialRaise : IStrategy
    {
        public double Acc = 5;
        public double StopLossAndRaise = 2;
        private int CountRaiseDaysParameter;
        private int CountRaiseDays = 0;

        public Strategy_2317_CountSerialRaise(Hashtable Setup)
        {
            try
            {
                CountRaiseDaysParameter = int.Parse(Setup["CountRaiseDaysParameter"].ToString());
                Acc = int.Parse(Setup["StopEarn"].ToString());
                StopLossAndRaise = int.Parse(Setup["StopLossAndRaise"].ToString());
            }
            catch (Exception ee)
            {

            }

        }
        public bool BuyCondition(
            ref SimulationVariable simulationVariable,
            ref DataList dataList,
            ref BasicFinancialReportListModel financialdata, int j)
        {

            if (!simulationVariable.HasBuy && dataList.TechData[j].ReturnOnInvestment > 0)
                CountRaiseDays++;
            else if (dataList.TechData[j].ReturnOnInvestment != 0 || simulationVariable.HasBuy)
                CountRaiseDays = 0;


            if (
                CountRaiseDays== 4
                )
                return true;

            return false;
        }

        public bool SellCondition(
            ref SimulationVariable simulationVariable,
            ref DataList dataList,
            ref BasicFinancialReportListModel financialdata, int j)
        {

            if (simulationVariable.HasBuy && dataList.TechData[j].ReturnOnInvestment > 0)
                CountRaiseDays++;
            else if (dataList.TechData[j].ReturnOnInvestment != 0 || simulationVariable.HasBuy)
                CountRaiseDays = 0;

            simulationVariable.MoveStopLossPercentage = Acc;
            if (
                (simulationVariable.Accumulation > 6)// && dataList.TechData[j].ReturnOnInvestment < 4)
                || simulationVariable.Accumulation < -5
                || CountRaiseDays == 5
                )
            {
                CountRaiseDays = 0;
                return true;
            }
            return false;
        }
    }
}