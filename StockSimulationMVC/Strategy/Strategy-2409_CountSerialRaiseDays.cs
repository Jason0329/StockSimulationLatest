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
    public class Strategy_2409_CountSerialDays : IStrategy
    {
        int CountRaiseDays = 0;
        public double Acc = 5;
        public double StopEarn = 5;
        int CountRaiseDaysParameter = 4;
        bool StartBuy = false;
        Decimal ReferencePrice = 0;
        public double StopLossAndRaise = 2;

        public Strategy_2409_CountSerialDays(Hashtable Setup)
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
        public Strategy_2409_CountSerialDays() { }
        public bool BuyCondition(ref SimulationVariable simulationVariable, ref DataList dataList, ref BasicFinancialReportListModel financialdata, int j)
        {
            if (!simulationVariable.HasBuy && dataList.TechData[j].ReturnOnInvestment > 0)
                CountRaiseDays++;
            else if (dataList.TechData[j].ReturnOnInvestment != 0 || simulationVariable.HasBuy)
                CountRaiseDays = 0;


            if (CountRaiseDays == 3
                //|| dataList.ReturnValue("CountDropinDays-20", j) > 9
                )

            {
                //CountRaiseDays = 0;
                return true;
            }

            return false;
        }
        public bool SellCondition(ref SimulationVariable simulationVariable, ref DataList dataList, ref BasicFinancialReportListModel financialdata, int j)
        {
            if (simulationVariable.HasBuy && dataList.TechData[j].ReturnOnInvestment > 0)
                CountRaiseDays++;
            else if (dataList.TechData[j].ReturnOnInvestment != 0 || simulationVariable.HasBuy)
                CountRaiseDays = 0;

      
            if (
                simulationVariable.Accumulation > 3
                || simulationVariable.Accumulation < -4
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