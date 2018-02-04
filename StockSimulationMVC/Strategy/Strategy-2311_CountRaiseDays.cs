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
    public class Strategy_2311_CountRaiseDays : IStrategy
    {
        public double Acc = 5;
        public double StopLossAndRaise=2;
        public Strategy_2311_CountRaiseDays(Hashtable Setup)
        {
            try
            {
                StopLossAndRaise = int.Parse(Setup["StopLossAndRaise"].ToString());
                Acc = int.Parse(Setup["StopEarn"].ToString());
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
            if (
                dataList.ReturnValue("CountRaiseinDays-20", j) > 12

                )
                return true;

            return false;
        }

        public bool SellCondition(
            ref SimulationVariable simulationVariable,
            ref DataList dataList,
            ref BasicFinancialReportListModel financialdata, int j)
        {
            simulationVariable.MoveStopLossPercentage = Acc;
            if (simulationVariable.Accumulation < -1
                //(dataList.ReturnValue("CountRaiseinDays-20", j) > 9 && simulationVariable.Accumulation > 1)
                //|| (dataList.ReturnValue("CountRaiseinDays-20", j) > 9 && simulationVariable.Accumulation < -2)
                || simulationVariable.Accumulation > 1
                )
                return true;
            return false;
        }
    }
}