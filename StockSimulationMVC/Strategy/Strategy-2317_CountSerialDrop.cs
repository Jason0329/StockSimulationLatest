﻿using StockSimulationMVC.Interface;
using StockSimulationMVC.Models;
using StockSimulationMVC.Simulation_SimulationStart;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockSimulationMVC.Strategy
{
    public class Strategy_2317_CountSErialDrop:IStrategy
    {
        int CountDropDays = 0;
        public double Acc = 5;
        public double StopEarn = 5;
        int CountDropDaysParameter = 4;
        bool StartBuy = false;
        Decimal ReferencePrice = 0;
        public double StopLossAndRaise = 2;

        public Strategy_2317_CountSErialDrop(Hashtable Setup)
        {
            try
            {
                CountDropDaysParameter = int.Parse(Setup["CountDropDaysParameter"].ToString());
                Acc = int.Parse(Setup["StopEarn"].ToString());
                StopLossAndRaise = int.Parse(Setup["StopLossAndRaise"].ToString());
            }
            catch (Exception ee)
            {
                CountDropDaysParameter = 3;
            }

        }
        public Strategy_2317_CountSErialDrop() { }
        public bool BuyCondition(ref SimulationVariable simulationVariable, ref DataList dataList, ref BasicFinancialReportListModel financialdata, int j)
        {
            if (!simulationVariable.HasBuy && dataList.TechData[j].ReturnOnInvestment < 0)
                CountDropDays++;
            else if (dataList.TechData[j].ReturnOnInvestment != 0 || simulationVariable.HasBuy)
                CountDropDays = 0;


            if (CountDropDays == CountDropDaysParameter
                //|| dataList.ReturnValue("CountDropinDays-20", j) > 9
                )

            {
                //CountDropDays = 0;
                return true;
            }

            return false;
        }
        public bool SellCondition(ref SimulationVariable simulationVariable, ref DataList dataList, ref BasicFinancialReportListModel financialdata, int j)
        {
            if (simulationVariable.HasBuy && dataList.TechData[j].ReturnOnInvestment > 0)
                CountDropDays++;
            else if (dataList.TechData[j].ReturnOnInvestment != 0 || simulationVariable.HasBuy)
                CountDropDays = 0;


            if (
                (simulationVariable.Accumulation > StopLossAndRaise)// && dataList.TechData[j].ReturnOnInvestment < 4)
                || simulationVariable.Accumulation < -StopLossAndRaise
                || CountDropDays==5
                 )
            {
                CountDropDays = 0;
                return true;
            }

            return false;
        }
    }
}