using StockSimulationMVC.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StockSimulationMVC.Models;
using StockSimulationMVC.Simulation_SimulationStart;

namespace StockSimulationMVC.Strategy
{
    public class Strategy_Ben : IStrategy
    {
        public double Acc = 20;
        public bool BuyCondition(ref SimulationVariable simulationVariable, ref DataList dataList, ref BasicFinancialReportListModel financialdata, int j)
        {
            if (financialdata.RevenueInt - 2 < 0 || financialdata.BasicFinancialInt <= 0 || financialdata.BasicFinancialInt - 20 <= 0 || j < 30) return false;
            financialdata.InitialDate(dataList.TechData[j].Date);

            double? QOperatingIncomePercentage = 0;
            double? QNetIncomePercentage = 0;
            double? QNetIncomeOperatingIncomePercentage = 0;
            int FinIndex = financialdata.BasicFinancialInt;

            for (int i =0; i<20; i++)
            {
                if (i % 4 == 0 && i!=0)
                {
                    QNetIncomeOperatingIncomePercentage += QOperatingIncomePercentage / QNetIncomePercentage;
                    QOperatingIncomePercentage = 0;
                    QNetIncomePercentage = 0;
                }

                QOperatingIncomePercentage += financialdata.FinancialDataList[FinIndex - i].QOperatingIncomePercentage;
                QNetIncomePercentage += financialdata.FinancialDataList[FinIndex - i].QNetIncomePercentage;               
            }

            QNetIncomeOperatingIncomePercentage = QNetIncomeOperatingIncomePercentage / 5;


            if (financialdata.ComparerFinancial("QNetIncome", 0, 20, Yearly: true)
                && financialdata.ComparerFinancial("QOperatingIncomePercentage", 30, 20, Yearly: true)
                && financialdata.ComparerFinancial("QReturnonEquityPercentage_A", 10, 20, false, true)
                && financialdata.ComparerFinancial("QOperatingIncome", 0, 20, false, true)            
                && QNetIncomeOperatingIncomePercentage>0.75
                && financialdata.RevenueList[financialdata.RevenueInt].YoYPercentage_MonthlySale > 0
                && financialdata.RevenueList[financialdata.RevenueInt - 1].YoYPercentage_MonthlySale > 0

                 && dataList.CoditionSatified("BollingerBandsDown-20", "MoveAverageValue-1", j - 1)
                 && dataList.CoditionSatified("MoveAverageValue-1", "BollingerBandsDown-20", j)
                && dataList.TechData[j].CashYieldRate >= 4.5
                )
                return true;
            return false;
        }

        public bool SellCondition(ref SimulationVariable simulationVariable, ref DataList dataList, ref BasicFinancialReportListModel financialdata, int j)
        {

            simulationVariable.MoveStopLossPercentage = Acc;
            if (  simulationVariable.Accumulation > Acc
                //||dataList.TechData[j].CashYieldRate <= 2.5
                //|| (financialdata.RevenueList[financialdata.RevenueInt].YoYPercentage_MonthlySale < -10 )
                //|| simulationVariable.ConditionSatifiedMoveStopLoss("MoveStopLossPercentage")

                || simulationVariable.Accumulation < -Acc)
                return true;
            return false;
        }
    }
}