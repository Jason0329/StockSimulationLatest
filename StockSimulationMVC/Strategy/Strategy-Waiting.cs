using StockSimulationMVC.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StockSimulationMVC.Models;
using StockSimulationMVC.Simulation_SimulationStart;

namespace StockSimulationMVC.Strategy
{
    public class Strategy_Waiting : IStrategy
    {
        public double Acc = 20;
        public bool BuyCondition(ref SimulationVariable simulationVariable, ref DataList dataList, ref BasicFinancialReportListModel financialdata, int j)
        {
            if (financialdata.RevenueInt - 2 < 0 || financialdata.BasicFinancialInt <= 0 || financialdata.BasicFinancialInt - 20 <= 0 ||j<30) return false;

            financialdata.InitialDate(dataList.TechData[j].Date);

            double? CFC_Yearly = 0;
            double? CFC_5Year = 0;
            double? QCashFlowFromOperatingAction = 0;
            double? QNetIncome = 0;
            double? LongTermLiability = 0;
            double? TotalLiability = 0;

            double? QCashFlow_QNetIncome = 0;
            int count = 0;
            int countY = 1;

            for (int i = financialdata.BasicFinancialInt; i > financialdata.BasicFinancialInt - 20; i--)
            {
                
                QCashFlowFromOperatingAction += financialdata.FinancialDataList[i].QCashFlowFromOperatingAction;
                QNetIncome += financialdata.FinancialDataList[i].QNetIncome;

                countY++;
                if(countY%4==0)
                {
                    QCashFlow_QNetIncome += (QCashFlowFromOperatingAction/ QNetIncome);
                    QCashFlowFromOperatingAction = 0;
                    QNetIncome = 0;
                }

                if (count < 4)
                {
                    CFC_Yearly += financialdata.FinancialDataList[i].QCashFlowFromOperatingAction - financialdata.FinancialDataList[i].QCashFlowfromInvestmentAction;
                    LongTermLiability += financialdata.FinancialDataList[i].QLong_TermLiabilities;
                    TotalLiability += financialdata.FinancialDataList[i].QTotalLiabilities;
                    count++;
                }
                CFC_5Year += financialdata.FinancialDataList[i].QCashFlowFromOperatingAction - financialdata.FinancialDataList[i].QCashFlowfromInvestmentAction;
            }

            QCashFlow_QNetIncome = QCashFlow_QNetIncome / 5;

            if (dataList.TechData[j].CashYieldRate >= 5 && 
                financialdata.RevenueList[financialdata.RevenueInt].YoYPercentage_MonthlySale > 0 &&
                financialdata.RevenueList[financialdata.RevenueInt - 1].YoYPercentage_MonthlySale > 0 &&

            

                 financialdata.FinancialDataList[financialdata.BasicFinancialInt].QLong_TermLiabilities / financialdata.FinancialDataList[financialdata.BasicFinancialInt].QTotalLiabilities < 0.3 &&
                 //LongTermLiability/TotalLiability<0.3&&

                 QCashFlowFromOperatingAction / QNetIncome >= 1 &&
                 //QCashFlow_QNetIncome>=1&&



                 financialdata.ComparerFinancial("QReturnonEquityPercentage_A", 20, 4, false, true) &&
                 financialdata.ComparerFinancial("QReturnonEquityPercentage_A", 15, 20, false, true) &&

                 CFC_5Year > 0 &&
                 CFC_Yearly > 0 &&

                 financialdata.ComparerFinancial("QNetIncomePercentage", 10, 20, Yearly: true) && //稅後淨利率
                 financialdata.ComparerFinancial("QNetIncomePercentage", 10, 4, Yearly: true)
                 //financialdata.ComparerFinancial("QNetIncomePercentage", 20, 4, true, Yearly: true)

                 //&& dataList.CoditionSatifiedIsBiggerValue( "MoveAverageValue-30",j ,500)
                 && dataList.CoditionSatified("BollingerBandsDown-20", "MoveAverageValue-1", j - 1)
                 && dataList.CoditionSatified("MoveAverageValue-1", "BollingerBandsDown-20", j)
)// && financialdata.ComparerFinancial("QCashFlowPerShare",3,4))
                return true;
            return false;
        }
        public bool SellCondition(ref SimulationVariable simulationVariable, ref DataList dataList, ref BasicFinancialReportListModel financialdata, int j)
        {
            simulationVariable.MoveStopLossPercentage = Acc;
            if (dataList.TechData[j].CashYieldRate <= 2.5
                || financialdata.RevenueList[financialdata.RevenueInt].YoYPercentage_MonthlySale < -20
                || simulationVariable.Accumulation > 50 
                //|| simulationVariable.Accumulation < -Acc
                ||(financialdata.RevenueList[financialdata.RevenueInt ].YoYPercentage_MonthlySale< -10&& financialdata.RevenueList[financialdata.RevenueInt - 1].YoYPercentage_MonthlySale<-10)
                //|| financialdata.RevenueList[financialdata.RevenueInt ].YoYPercentage_MonthlySale<-10
                //|| dataList.CoditionSatified("MoveAverageValue-1", "MoveAverageValue-10", j,false)//&& dataList.CoditionSatified("BollingerBandsDown-5", "MoveAverageValue-1", j - 1,false)
                || simulationVariable.ConditionSatifiedMoveStopLoss("MoveStopLossPercentage")
                //|| ( dataList.CoditionSatified("MoveAverageValue-60", "MoveAverageValue-1", j )
                // && dataList.CoditionSatified("MoveAverageValue-1", "MoveAverageValue-60", j -1))
                )//simulationVariable.ConditionSatifiedMoveStopLoss("MoveStopLossPercentage"))// || dataList.CoditionSatified("MinValue-1", "MinValue-10", j))
                return true;
            return false;
        }
    }
}