
using StockSimulationMVC.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StockSimulationMVC.Models;
using StockSimulationMVC.Simulation_SimulationStart;
using System.Collections;
using System.Reflection;
using StockSimulationMVC.Core;

namespace StockSimulationMVC.Strategy
{
    public class ZhaoFinancialSelectStockStrategy : IStrategy
    {
        readonly double EPSParameter = 0.08;
        readonly double ValuationLatestParameter = 1;
        readonly double ValuationParameter = 1;

        int valuationParameter = 3;
       
        double operationIncomePercentageCountSet = 0;
        double netIncomePercentageCountSet = 0;
        double EPSCountSet = 0;
        double EPSSet = -200;
        bool OperationCashFlowSet = false;
        double EPSQoQSet = -200;
        double EPSYoYSet = -200;
        double EPSAccumulationYoYSet = -200;
        public ZhaoFinancialSelectStockStrategy(Hashtable Setup)
        {
            if(Setup["ValuationParameter"] != null)
            {
                valuationParameter = int.Parse(Setup["ValuationParameter"].ToString());
            }

            if (Setup["operationIncomePercentageCountSet"] != null)
            {
                operationIncomePercentageCountSet = double.Parse(Setup["operationIncomePercentageCountSet"].ToString());
            }

            if (Setup["netIncomePercentageCountSet"] != null)
            {
                netIncomePercentageCountSet = double.Parse(Setup["netIncomePercentageCountSet"].ToString());
            }

            if (Setup["EPSSet"] != null)
            {
                EPSSet = double.Parse(Setup["EPSSet"].ToString());
            }

            if (Setup["EPSCountSet"] != null)
            {
                EPSCountSet = double.Parse(Setup["EPSCountSet"].ToString());
            }

            if (Setup["EPSQoQSet"] != null)
            {
                EPSQoQSet = double.Parse(Setup["EPSQoQSet"].ToString());
            }

            if (Setup["EPSYoYSet"] != null)
            {
                EPSYoYSet = double.Parse(Setup["EPSYoYSet"].ToString());
            }

            if (Setup["EPSAccumulationYoYSet"] != null)
            {
                EPSAccumulationYoYSet = double.Parse(Setup["EPSAccumulationYoYSet"].ToString());
            }

            if (Setup["OperationCashFlowSet"] != null)
            {
                OperationCashFlowSet = bool.Parse(Setup["OperationCashFlowSet"].ToString());
            }


        }
        public bool BuyCondition(ref SimulationVariable simulationVariable, ref DataList dataList, ref BasicFinancialReportListModel financialdata, int j)
        {
            InitialData.OutputData = new OutputModel();

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


            double? BPS = financialdata.FinancialDataList[financialdata.BasicFinancialInt].BPS_A__Consol;
            double? OperationCashFlow = financialdata.FinancialDataList[financialdata.BasicFinancialInt].CashFlow_Operating_Consol;

            double? NetIncomePercentageCount = CountKeepingRaise(ref financialdata , "NetIncome0_Consol");
            double? OperationIncomePercentageCount = CountKeepingRaise(ref financialdata, "OperatingIncome0_Consol");
            double? EPS = financialdata.FinancialDataList[financialdata.BasicFinancialInt].EarningPerShare_Consol;
            double? EPSCount = CountKeepingRaise(ref financialdata, "EarningPerShare_Consol");
            double? EPSQoQ = QoQ(ref financialdata, "EarningPerShare_Consol");
            double? EPSYoY = YoY(ref financialdata, "EarningPerShare_Consol");
            double? EPSAccumulationYoY = YoY(ref financialdata, "EarningPerShare_Consol");

            if(NetIncomePercentageCount !=null)
            {
                InitialData.OutputData.netIncomePercentageCount = (double)NetIncomePercentageCount;
            }

            if (OperationIncomePercentageCount != null)
            {
                InitialData.OutputData.operationIncomePercentageCount = (double)OperationIncomePercentageCount;
            }

            if (EPSCount != null)
            {
                InitialData.OutputData.EPSCount = (double)EPSCount;
            }

            if (EPSQoQ != null)
            {
                InitialData.OutputData.EPSQoQ = System.Math.Round((double)EPSQoQ, 2);
            }

            if (EPSYoY != null)
            {
                InitialData.OutputData.EPSYoY = System.Math.Round((double)EPSYoY, 2);
            }

            if (EPSAccumulationYoY != null)
            {
                InitialData.OutputData.EPSAccumulationYoY = System.Math.Round((double)EPSAccumulationYoY, 2);
            }

            if (EPS != null)
            {
                InitialData.OutputData.EPS = System.Math.Round((double)EPS, 2);
            }
           
            
            
            
            
           
            

            if (ValuationConditionSatisfied(ref financialdata, valuationParameter) && OperationCashFlow > 0
                && NetIncomePercentageCount >= netIncomePercentageCountSet
                && OperationIncomePercentageCount >= operationIncomePercentageCountSet
                && EPS >= EPSSet
                && EPSCount >= EPSCountSet
                && EPSQoQ >= EPSQoQSet
                && EPSYoY >= EPSYoYSet
                && EPSAccumulationYoY >= EPSAccumulationYoYSet
                //&& FinancialPublished(ref dataList , j)
                && j > 240
                && ((OperationCashFlowSet && OperationCashFlow > 0) || !OperationCashFlowSet)
                && (dataList.ReturnValue("MaxValue-240", j - 1)) < (double)dataList.TechData[j].ClosePrice
               )
            {
                return true;
            }


            return false;
        }

        public bool SellCondition(ref SimulationVariable simulationVariable, ref DataList dataList, ref BasicFinancialReportListModel financialdata, int j)
        {
            //return true;
            //if (j < 0 || dataList.TechData.Count - j <= 0 || financialdata.RevenueInt - 1 < 0 || financialdata.RevenueInt >= financialdata.RevenueList.Count)
            //    return false;

            if (
                //dataList.TechData[j].Date.Month ==6 || dataList.TechData[j].Date.Month==9 || dataList.TechData[j].Date.Month==12 || dataList.TechData[j].Date.Month==4
                simulationVariable.Accumulation > 20 || simulationVariable.Accumulation < -20
                )
                return true;

            return false;
        }

        public bool FinancialPublished(ref DataList dataList , int j)
        {
            if(
                //(dataList.TechData[j].Date.Month ==5 && dataList.TechData[j].Date.Day == 15) ||
                //(dataList.TechData[j].Date.Month == 8 && dataList.TechData[j].Date.Day == 15)||
                //(dataList.TechData[j].Date.Month == 11 && dataList.TechData[j].Date.Day == 15)||
                //(dataList.TechData[j].Date.Month == 4 && dataList.TechData[j].Date.Day == 1)
                (dataList.TechData[j].Date.Month == 1 && dataList.TechData[j].Date.Day == 5)
                )
            {
                return true;
            }

            return false;
        }
        bool ValuationConditionSatisfied(ref BasicFinancialReportListModel financialdata, int countRefYear)
        {
            UpdateOutputData(ref financialdata);

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

        void UpdateOutputData(ref BasicFinancialReportListModel financialdata)
        {
            double latestYearValuation = LatestYearValuation(ref financialdata);
            double[] BeforeYearValuationArray = new double[4];
            BeforeYearValuationArray[0] = BeforeYearValuation(ref financialdata, 1);
            BeforeYearValuationArray[1] = BeforeYearValuation(ref financialdata, 2);
            BeforeYearValuationArray[2] = BeforeYearValuation(ref financialdata, 3);
            BeforeYearValuationArray[3] = BeforeYearValuation(ref financialdata, 4);

            InitialData.OutputData.Evaluation_1Year = System.Math.Round(latestYearValuation,2);
            InitialData.OutputData.Evaluation_2Year = System.Math.Round(BeforeYearValuationArray[0],2);
            InitialData.OutputData.Evaluation_3Year = System.Math.Round(BeforeYearValuationArray[1],2);
            InitialData.OutputData.Evaluation_4Year = System.Math.Round(BeforeYearValuationArray[2],2);
            InitialData.OutputData.Evaluation_5Year = System.Math.Round(BeforeYearValuationArray[3],2);

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

        int CountKeepingRaise(ref BasicFinancialReportListModel financialdata , string field )
        {
            int count = 1;
            int counting = financialdata.BasicFinancialInt;

            for (int i = financialdata.BasicFinancialInt; i > 0; i--)
            {
                PropertyInfo propertyNext = financialdata.FinancialDataList[i].GetType().GetProperty(field);
                double? valueNext = (double?)propertyNext.GetValue(financialdata.FinancialDataList[i]);

                PropertyInfo propertyPre = financialdata.FinancialDataList[i - 1].GetType().GetProperty(field);
                double? valuePre = (double?)propertyNext.GetValue(financialdata.FinancialDataList[i -1]);

                if(valueNext > valuePre)
                {
                    count++;
                }
                else
                {
                    break;
                }
            }


            return count;
        }

        double? QoQ(ref BasicFinancialReportListModel financialdata , string field)
        {
            int counting = financialdata.BasicFinancialInt;

            if (counting < 1 )
            {
                return -1000;
            }

            PropertyInfo propertyNext = financialdata.FinancialDataList[counting].GetType().GetProperty(field);
            double? valueNext = (double?)propertyNext.GetValue(financialdata.FinancialDataList[counting]);

            PropertyInfo propertyPre = financialdata.FinancialDataList[counting - 1].GetType().GetProperty(field);
            double? valuePre = (double?)propertyNext.GetValue(financialdata.FinancialDataList[counting - 1]);

            double? qoq = ((valueNext / valuePre) - 1) * 100 ;

            return qoq;
        }
        double? YoY(ref BasicFinancialReportListModel financialdata, string field)
        {
            int counting = financialdata.BasicFinancialInt;

            if (counting < 4)
            {
                return -1000;
            }

            PropertyInfo propertyNext = financialdata.FinancialDataList[counting].GetType().GetProperty(field);
            double? valueNext = (double?)propertyNext.GetValue(financialdata.FinancialDataList[counting]);

            PropertyInfo propertyPre = financialdata.FinancialDataList[counting - 4].GetType().GetProperty(field);
            double? valuePre = (double?)propertyNext.GetValue(financialdata.FinancialDataList[counting - 4]);

            double? yoy = ((valueNext / valuePre) - 1) * 100;

            return yoy;
        }
        double? AccumulationYoY(ref BasicFinancialReportListModel financialdata, string field)
        {
            int counting = financialdata.BasicFinancialInt;

            if (counting < 8)
            {
                return -1000;
            }

            double? valueNext = 0;
            double? valuePre = 0;

            for(int i = 0; i<4; i++)
            {
                PropertyInfo propertyNext = financialdata.FinancialDataList[counting - i].GetType().GetProperty(field);
                valueNext += (double?)propertyNext.GetValue(financialdata.FinancialDataList[counting - i]);
            }

            for (int i = 4; i < 8; i++)
            {
                PropertyInfo propertyPre = financialdata.FinancialDataList[counting - i].GetType().GetProperty(field);
                valuePre += (double?)propertyPre.GetValue(financialdata.FinancialDataList[counting - i]);
            }

            double? yoy = ((valueNext / valuePre) - 1) * 100;

            return yoy;
        }

    }
}