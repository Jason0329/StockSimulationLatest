using StockSimulationMVC.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StockSimulationMVC.Models;
using StockSimulationMVC.Simulation_SimulationStart;

namespace StockSimulationMVC.Strategy
{
    public class TeacherStrategy_Stock_0056 : IStrategy
    {
        public double Acc = 10;
        int CountDropDays = 0;
        int CountDropDaysParameter = 3;
        public bool BuyCondition(ref SimulationVariable simulationVariable, ref DataList dataList, ref BasicFinancialReportListModel financialdata, int j)
        {
            if (!simulationVariable.HasBuy && dataList.TechData[j].ReturnOnInvestment < 0)
                CountDropDays++;
            else if (dataList.TechData[j].ReturnOnInvestment != 0 || simulationVariable.HasBuy)
                CountDropDays = 0;

            if (
                dataList.ReturnValue("MinValue-10", j) == double.Parse(dataList.TechData[j].Volume.ToString())
               && ( dataList.TechData[j].Date.Month == 7 || dataList.TechData[j].Date.Month == 8)
               && dataList.CoditionSatified("BollingerBandsDown-20", "MoveAverageValue-1",j))//&& dataList.CoditionSatified("BollingerBandsDown-5", "MoveAverageValue-1", j) && financialdata.ComparerFinancial("QCashFlowPerShare",3,4))
                return true;

            return false;
        }

        public bool SellCondition(ref SimulationVariable simulationVariable, ref DataList dataList, ref BasicFinancialReportListModel financialdata, int j)
        {
            bool fin = financialdata.ComparerFinancial("QEarningPerShare", 0, 1, false);
            if (
               (simulationVariable.Accumulation > Acc
                || simulationVariable.Accumulation < -Acc)
                && (dataList.TechData[j].Date.Month == 1 || dataList.TechData[j].Date.Month == 2 || dataList.TechData[j].Date.Month == 3))
              return true;

            return false;
        }
    }
}