
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
    public class ZhaoFinancialSelectStockStrategy : IStrategy
    {
        readonly double EPSParameter = 0.08;
        readonly double ValuationLatestParameter = 1;
        readonly double ValuationParameter = 1;

        int valuationParameter = 5;
       
        double operationIncomePercentageSet = 2;
        double netIncomePercentageSet = 3;
        public ZhaoFinancialSelectStockStrategy(Hashtable Setup)
        {
            if(Setup["ValuationParameter"] != null)
            {
                valuationParameter = int.Parse(Setup["ValuationParameter"].ToString());
            }

            if (Setup["operationIncomePercentageSet"] != null)
            {
                operationIncomePercentageSet = double.Parse(Setup["operationIncomePercentageSet"].ToString());
            }

            if (Setup["netIncomePercentageSet"] != null)
            {
                netIncomePercentageSet = double.Parse(Setup["netIncomePercentageSet"].ToString());
            }

            try
            {
               
            }
            catch (Exception ee)
            {
               
            }

        }
        public bool BuyCondition(ref SimulationVariable simulationVariable, ref DataList dataList, ref BasicFinancialReportListModel financialdata, int j)
        {
            //return true;
            if (financialdata.FinancialDataList.Count == 0)
            {
                return false;
            }

            if(financialdata.FinancialDataList[financialdata.BasicFinancialInt].EarningPerShare_Consol == null 
                || financialdata.FinancialDataList[financialdata.BasicFinancialInt].BPS_A__Consol == null
                || financialdata.FinancialDataList[financialdata.BasicFinancialInt].CashFlow_Operating_Consol == null)
            {
                return false;
            }


            double? EPS = financialdata.FinancialDataList[financialdata.BasicFinancialInt].EarningPerShare_Consol;
            double? BPS = financialdata.FinancialDataList[financialdata.BasicFinancialInt].BPS_A__Consol;
            double? OperationCashFlow = financialdata.FinancialDataList[financialdata.BasicFinancialInt].CashFlow_Operating_Consol;
            double? OperationIncomePercentage = financialdata.FinancialDataList[financialdata.BasicFinancialInt].OperatingIncome0_Consol;
            double? NetIncomePercentage = financialdata.FinancialDataList[financialdata.BasicFinancialInt].NetIncome0_Consol;





            if (ValuationConditionSatisfied(ref financialdata, valuationParameter) && OperationCashFlow > 0
                //&& dataList.TechData[j].CashYieldRate >= 4

               && FinancialPublished(ref dataList , j)
               )
            {
                return true;
            }


            return false;
        }

        public bool SellCondition(ref SimulationVariable simulationVariable, ref DataList dataList, ref BasicFinancialReportListModel financialdata, int j)
        {
            return true;
            //if (j < 0 || dataList.TechData.Count - j <= 0 || financialdata.RevenueInt - 1 < 0 || financialdata.RevenueInt >= financialdata.RevenueList.Count)
            //    return false;

            if (
                dataList.TechData[j].Date.Month ==6 || dataList.TechData[j].Date.Month==9 || dataList.TechData[j].Date.Month==12 || dataList.TechData[j].Date.Month==4

                )
                return true;

            return false;
        }

        public bool FinancialPublished(ref DataList dataList , int j)
        {
            if((dataList.TechData[j].Date.Month ==5 && dataList.TechData[j].Date.Day == 15) ||
                (dataList.TechData[j].Date.Month == 8 && dataList.TechData[j].Date.Day == 15)||
                (dataList.TechData[j].Date.Month == 11 && dataList.TechData[j].Date.Day == 15)||
                (dataList.TechData[j].Date.Month == 3 && dataList.TechData[j].Date.Day == 31)
                )
            {
                return true;
            }

            return false;
        }
        bool ValuationConditionSatisfied(ref BasicFinancialReportListModel financialdata, int countRefYear)
        {
            bool condiotionsatisfied = true;
            if (LatestYearValuation(ref financialdata) > BeforeYearValuation(ref financialdata , 1) * ValuationLatestParameter)
            {

                if(countRefYear ==4)
                {
                    if (BeforeYearValuation(ref financialdata, 2) > BeforeYearValuation(ref financialdata, 3) * ValuationParameter
                      && BeforeYearValuation(ref financialdata, 3) > BeforeYearValuation(ref financialdata, 4) * ValuationParameter)
                    {
                        return true;
                    }

                    if (BeforeYearValuation(ref financialdata, 1) > BeforeYearValuation(ref financialdata, 2) * ValuationParameter
                    && BeforeYearValuation(ref financialdata, 3) > BeforeYearValuation(ref financialdata, 4) * ValuationParameter)
                    {
                        return true;
                    }

                }
                
                for (int i = 1; i <= countRefYear -1 ; i++)
                {
                    // if it is not satfified condition . test every year
                    if (BeforeYearValuation(ref financialdata, i) < BeforeYearValuation(ref financialdata, i+1) * ValuationParameter)
                    {
                        condiotionsatisfied = false;
                        break;
                    }
                }

            }
            else
            {
                condiotionsatisfied = false;
            }

            return condiotionsatisfied;
        }

        double LatestYearValuation(ref BasicFinancialReportListModel financialdata)
        {
            double returnValuation = 0;

            if (financialdata.FinancialDataList[financialdata.BasicFinancialInt].Date.Year <= 2007)
            {
                if(financialdata.BasicFinancialInt >= 1)
                {
                    returnValuation = Valuation(financialdata.FinancialDataList[financialdata.BasicFinancialInt]) 
                                    + Valuation(financialdata.FinancialDataList[financialdata.BasicFinancialInt - 1]);
                }
            }
            else
            {
                if (financialdata.BasicFinancialInt >= 3)
                {
                    returnValuation = Valuation(financialdata.FinancialDataList[financialdata.BasicFinancialInt])
                                    + Valuation(financialdata.FinancialDataList[financialdata.BasicFinancialInt - 1])
                                    + Valuation(financialdata.FinancialDataList[financialdata.BasicFinancialInt - 2])
                                    + Valuation(financialdata.FinancialDataList[financialdata.BasicFinancialInt - 3]);
                }
            }

            return returnValuation;
        }

        double BeforeYearValuation(ref BasicFinancialReportListModel financialdata , int previousYear)
        {
            
            int referenceYear = financialdata.FinancialDataList[financialdata.BasicFinancialInt].Date.Year - previousYear;
            double returnValuation = 0;
            int countRef = (referenceYear >= 2008) ? 4 : 2;
            int count = 0; 

            for(int i = financialdata.BasicFinancialInt; i >=0; i-- )
            {
                if(financialdata.FinancialDataList[i].Date.Year == referenceYear)
                {
                    returnValuation += Valuation(financialdata.FinancialDataList[i]);
                    count++;
                }

                if(count == countRef)
                {
                    break;
                }
            }

            return returnValuation;
        }


        double Valuation(BasicFinancialContainParentDataModel seasonFinancial)
        {
            if(seasonFinancial.EarningPerShare_Consol ==null || seasonFinancial.BPS_A__Consol == null)
            {
                return 0;
            }

            double EPS = (double)seasonFinancial.EarningPerShare_Consol;
            double BPS = (double)seasonFinancial.BPS_A__Consol;

            return EPS / EPSParameter + BPS;
        }

    }
}