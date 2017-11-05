using StockSimulationMVC.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StockSimulationMVC.Models;
using StockSimulationMVC.Simulation_SimulationStart;

namespace StockSimulationMVC.Strategy
{
    public class Strategy___TechBigVolume : IStrategy
    {
        public double Acc = 10;
        public bool BuyCondition(ref SimulationVariable simulationVariable, ref DataList dataList, ref BasicFinancialReportListModel financialdata, int j)
        {
            if (dataList.CoditionSatified("MoveAverageValue-1", "MoveAverageValue-6", j, Times: 2)
                && dataList.TechData[j].ReturnOnInvestment > 5
                && dataList.TechData[j].ReturnOnInvestment < 6)
                return true;

            return false;
        }

        public bool SellCondition(ref SimulationVariable simulationVariable, ref DataList dataList, ref BasicFinancialReportListModel financialdata, int j)
        {
            if (simulationVariable.Accumulation > Acc
                || simulationVariable.Accumulation < -Acc)
                return true;
            return false;
        }
    }
}