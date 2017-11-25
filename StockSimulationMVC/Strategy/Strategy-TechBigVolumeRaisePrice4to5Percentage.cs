using StockSimulationMVC.Interface;
using StockSimulationMVC.Models;
using StockSimulationMVC.Simulation_SimulationStart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockSimulationMVC.Strategy
{
    public class Strategy_TechBigVolumeRaisePrice4to5Percentage:IStrategy
    {
        public double Acc = 7;
        public bool BuyCondition(ref SimulationVariable simulationVariable, ref DataList dataList, ref BasicFinancialReportListModel financialdata, int j)
        {
            if (dataList.CoditionSatified("MoveAverageValue-1", "MoveAverageValue-30", j, Times: 2)
                && dataList.CoditionSatified("MoveAverageValue-1", "MoveAverageValue-30", j, 3, false)
                && dataList.TechData[j].ReturnOnInvestment >= 5
                && dataList.TechData[j].ReturnOnInvestment <= 6
                 //&& dataList.TechData[j].ClosePrice >= 20
                 //&& dataList.TechData[j].Date.Month != 3
                 //&& dataList.TechData[j].Date.Month != 4

                 //&& dataList.TechData[j].Date.Month != 6

                 && dataList.TechData[j].Date.Month == Acc


                //&& (dataList.TechData[j].ClosePrice>=40 || dataList.TechData[j].ClosePrice <= 20)
                && dataList.CoditionSatifiedIsBiggerValue("MoveAverageValue-30", j, 1400)
                && !dataList.CoditionSatifiedIsBiggerValue("MoveAverageValue-30", j, 7400)
                )
                return true;

            return false;
        }

        public bool SellCondition(ref SimulationVariable simulationVariable, ref DataList dataList, ref BasicFinancialReportListModel financialdata, int j)
        {
            simulationVariable.MoveStopLossPercentage = Acc;
            if ((simulationVariable.Accumulation > 7 && dataList.TechData[j].ReturnOnInvestment < 2)
                || simulationVariable.Accumulation < -7
                || (simulationVariable.HaveStockDay > 16 && simulationVariable.Accumulation < -2)
                || (simulationVariable.HaveStockDay > 15 && simulationVariable.Accumulation > 1 && dataList.TechData[j].ReturnOnInvestment < 2)
                )
                return true;
            return false;
        }
    }
}