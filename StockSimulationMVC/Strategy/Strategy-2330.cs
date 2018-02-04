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
    public class Strategy_2330 : IStrategy //2 3 3 0
    {
        int CountDropDays = 0;
        public double Acc = 5;
        public double StopEarn = 5;
        int CountDropDaysParameter = 4;
        bool StartBuy = false;
        Decimal ReferencePrice = 0;


        public Strategy_2330(Hashtable Setup)
        {
            try
            {
                CountDropDaysParameter = int.Parse(Setup["CountDropDaysParameter"].ToString());
                Acc = int.Parse(Setup["StopEarn"].ToString());
            }
            catch(Exception ee)
            {
                CountDropDaysParameter = 3;
            }

        }

        public Strategy_2330() { }



        public bool BuyCondition(ref SimulationVariable simulationVariable, ref DataList dataList, ref BasicFinancialReportListModel financialdata, int j)
        {
            if (!simulationVariable.HasBuy && dataList.TechData[j].ReturnOnInvestment < 0)
                CountDropDays++;
            else if(dataList.TechData[j].ReturnOnInvestment!=0 || simulationVariable.HasBuy)
                CountDropDays = 0;

            if (StartBuy)
            {
                StartBuy = false;
                return true;
            }


            if (CountDropDays == CountDropDaysParameter
               //|| dataList.ReturnValue("CountDropinDays-20", j) > 9
                )

            {
                CountDropDays = 0;
                return true;
                StartBuy = true;
                ReferencePrice = dataList.TechData[j].ClosePrice;
            }

            return false;
        }

        public bool SellCondition(ref SimulationVariable simulationVariable, ref DataList dataList, ref BasicFinancialReportListModel financialdata, int j)
        {
            if (
                //simulationVariable.Accumulation > Acc && dataList.TechData[j].ReturnOnInvestment < 5
                // || simulationVariable.Accumulation < -Acc
                (dataList.ReturnValue("CountRaiseinDays-20", j) > 9 && simulationVariable.Accumulation > 1)
                //||(dataList.ReturnValue("CountRaiseinDays-20", j) > 9 && simulationVariable.Accumulation < -2)
                || (dataList.ReturnValue("CountRaiseinDays-20", j) > 9 && simulationVariable.Accumulation < -2)
                || simulationVariable.Accumulation > 4
                || simulationVariable.Accumulation < -4
                 )
                return true;

            return false;
        }
    }
}