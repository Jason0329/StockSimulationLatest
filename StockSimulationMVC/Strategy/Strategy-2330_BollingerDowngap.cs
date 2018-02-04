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
    public class Strategy_2330_BollingerDowngap : IStrategy
    {
        public double Acc = 3;

        public Strategy_2330_BollingerDowngap(Hashtable Setup)
        {
            try
            {
                
                Acc = int.Parse(Setup["StopEarn"].ToString());
            }
            catch (Exception ee)
            {
               
            }

        }
        public bool BuyCondition(ref SimulationVariable simulationVariable, ref DataList dataList, ref BasicFinancialReportListModel financialdata, int j)
        {
            if (
             dataList.CoditionSatified("MoveAverageValue-1", "BollingerBandsDown-20", j)
             && dataList.CoditionSatified("BollingerBandsDown-20", "MoveAverageValue-1", j - 1)
            )
                return true;
            return false;
        }
        public bool SellCondition(ref SimulationVariable simulationVariable, ref DataList dataList, ref BasicFinancialReportListModel financialdata, int j)
        {
            simulationVariable.MoveStopLossPercentage = 5;
            if ((dataList.ReturnValue("CountRaiseinDays-20", j) > 9 && simulationVariable.Accumulation > 1)
                || (dataList.ReturnValue("CountRaiseinDays-20", j) > 9 && simulationVariable.Accumulation < -2)
                || simulationVariable.Accumulation > 4
                || simulationVariable.Accumulation < -4
                )
                return true;
            return false;
        }
    }
}