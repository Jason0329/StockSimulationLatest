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
    public class Strategy___2330_CountDropDays : IStrategy
    {
        int CountRaiseDays = 0;
        public double Acc = 5;
        public double StopEarn = 5;
        int CountRaiseDaysParameter = 3;
        bool StartBuy = false;
        Decimal ReferencePrice = 0;
        public double StopLossAndRaise = 2;
        public Strategy___2330_CountDropDays(Hashtable Setup)
        {
            try
            {
                CountRaiseDaysParameter = int.Parse(Setup["CountRaiseDaysParameter"].ToString());
                Acc = int.Parse(Setup["StopEarn"].ToString());
                StopLossAndRaise = int.Parse(Setup["StopLossAndRaise"].ToString());
            }
            catch (Exception ee)
            {
                CountRaiseDaysParameter = 3;
            }

        }
       
        public bool BuyCondition(ref SimulationVariable simulationVariable, ref DataList dataList, ref BasicFinancialReportListModel financialdata, int j)
        {
            if (
                dataList.ReturnValue("CountDropinDays-20", j) > 9

                )
                return true;

            return false;
        }

        public bool SellCondition(ref SimulationVariable simulationVariable, ref DataList dataList, ref BasicFinancialReportListModel financialdata, int j)
        {
            simulationVariable.MoveStopLossPercentage = Acc;
            if (
                (dataList.ReturnValue("CountRaiseinDays-20", j) > 9 &&  simulationVariable.Accumulation > 1)
                || (dataList.ReturnValue("CountRaiseinDays-20", j) > 9 && simulationVariable.Accumulation < -2)
                || simulationVariable.Accumulation >4
                )
                return true;
            return false;
        }
    }
}