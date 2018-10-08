using StockSimulationMVC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace StockSimulationMVC.Models
{
    public class BasicFinancialReportListModel
    {
        public List<BasicFinancialContainParentDataModel> FinancialDataList = new List<BasicFinancialContainParentDataModel>();
        public List<MonthRevenueModel> RevenueList = new List<MonthRevenueModel>();
        public int RevenueInt = -1;
        public int BasicFinancialInt = -1;

        #region Initial 
        public void Initial(int Company)
        {
            var FinancialData = from CompanyData in InitialData.InitialData_BasicFinancialDataContainParent
                                where CompanyData.Company == Company
                                orderby CompanyData.Date ascending
                                select CompanyData;

            FinancialDataList = FinancialData.ToList();

            //var MonthlyData = from CompanyData in InitialData.InitialData_MonthRevenueData
            //                    where CompanyData.Company == Company
            //                    orderby CompanyData.Date ascending
            //                    select CompanyData;
            //RevenueList = MonthlyData.ToList();

        }
                //public void Initial(string startDate, string EndDate, int company, bool IsOTC = false)
                //{
                //    DataList.Clear();
                //    RevenueList.Clear();
                //    List<string[]> temp = new List<string[]>();
                //    string command = "";

                //    #region 看使否為上櫃股是資料
                //    if (IsOTC)
                //    {
                //        command = @"SELECT [company],[Column1],[Column16],[Column17],[Column18],[Column19],[Column76],[Column77],[Column95],[Column101],[Column75],[Column65],[Column15]
                //                        FROM [OverTheCounter].[dbo].[standard_analysis] where company =" + company + @"and [Column1] between '" + startDate + @"' and 
                //                        '" + EndDate + "' order by Column1";
                //    }
                //    else
                //    {
                //        command = @"SELECT [company],[Column1],[Column16],[Column17],[Column18],[Column19],[Column76],[Column77],[Column95],[Column101],[Column75],[Column65],[Column15]
                //                        FROM [StockDatabase].[dbo].[standard_analysis] where company =" + company + @"and [Column1] between '" + startDate + @"' and 
                //                        '" + EndDate + "' order by Column1";
                //    }

                //    gd.GetData(command, ref temp);

                //    for (int i = 0; i < temp.Count; i++)
                //    {
                //        DataList.Add(new BasicData(temp[i]));
                //    }


                //    if (IsOTC)
                //    {
                //        command = @"SELECT [company],[Column1],[Column2],[Column4],[Column5] FROM  [OverTheCounter].[dbo].[_monthRevenue] where company=" + company +
                //            @" and [Column1] between '" + startDate + @"' and  '" + EndDate + @"' order by [Column1]";

                //    }
                //    else
                //    {
                //        command = @"SELECT [company],[Column1],[Column2],[Column4],[Column5] FROM [StockDatabase].[dbo].[_monthRevenue] where company=" + company +
                //            @" and [Column1] between '" + startDate + @"' and  '" + EndDate + @"' order by [Column1]";
                //    }
                //    #endregion

                //    temp = new List<string[]>();

                //    gd.GetData(command, ref temp);

                //    for (int i = 0; i < temp.Count; i++)
                //    {
                //        RevenueList.Add(new MonthRevenue(temp[i]));
                //    }
                //}
        #endregion

        #region 每季財報
        public void InitialDate(DateTime dt)
        {
            BasicFinancialInt = 0;

            #region 第一季
            if (dt.Year >= 2008 && dt.Year <= 2012 && dt.Month > 4 && dt.Month < 9)
            {
                for (int i = 0; i < FinancialDataList.Count; i++)
                {
                    if (FinancialDataList[i].Date.Year == dt.Year && FinancialDataList[i].Date.Month == 3)
                    {
                        BasicFinancialInt = i;
                        break;
                    }

                }
            }
            else if (dt.Year > 2012 && (dt.Month > 4 && dt.Month < 8 || dt.Month == 8 && dt.Day <= 15))
            {
                for (int i = 0; i < FinancialDataList.Count; i++)
                {
                    if (FinancialDataList[i].Date.Year == dt.Year && FinancialDataList[i].Date.Month == 3)
                    {
                        BasicFinancialInt = i;
                        break;
                    }
                }
            }


            #endregion
            #region 第二季
            if (dt.Year <= 2012 && dt.Year >= 2008 && dt.Month > 8 && dt.Month < 11)
            {
                for (int i = 0; i < FinancialDataList.Count; i++)
                {
                    if (FinancialDataList[i].Date.Year == dt.Year && FinancialDataList[i].Date.Month == 6)
                    {
                        BasicFinancialInt = i;
                        break;
                    }
                }
            }
            else if (dt.Year > 2012 && (dt.Month > 8 && dt.Month < 11 ||
                (dt.Month == 8 && dt.Day > 15)
                || (dt.Month == 11 && dt.Day <= 15)))
            {
                for (int i = 0; i < FinancialDataList.Count; i++)
                {
                    if (FinancialDataList[i].Date.Year == dt.Year && FinancialDataList[i].Date.Month == 6)
                    {
                        BasicFinancialInt = i;
                        break;
                    }
                }
            }
            else if (dt.Year <= 2007 && dt.Year >= 2002 || (dt.Year == 2008 && dt.Month < 6))
            {
                if (dt.Month > 8)
                {
                    for (int i = 0; i < FinancialDataList.Count; i++)
                    {
                        if (FinancialDataList[i].Date.Year == dt.Year && FinancialDataList[i].Date.Month == 6)
                        {
                            BasicFinancialInt = i;
                            break;
                        }
                    }
                }
                else if (dt.Month <= 3)
                {
                    for (int i = 0; i < FinancialDataList.Count; i++)
                    {
                        if (FinancialDataList[i].Date.Year == dt.Year - 1 && FinancialDataList[i].Date.Month == 6)
                        {
                            BasicFinancialInt = i;
                            break;
                        }
                    }
                }
            }
            #endregion
            #region 第三季
            if (dt.Month < 4 || dt.Month == 12 || dt.Month == 11)
            {

                if (dt.Year > 2012 && ((dt.Month == 11 && dt.Day >= 15) || dt.Month == 12 || dt.Month < 4))
                {
                    for (int i = 0; i < FinancialDataList.Count; i++)
                    {
                        if (FinancialDataList[i].Date.Year == dt.Year && FinancialDataList[i].Date.Month == 9)
                        {
                            BasicFinancialInt = i;
                            break;
                        }
                        else if (dt.Month <= 3)
                        {
                            if (FinancialDataList[i].Date.Year == dt.Year - 1 && FinancialDataList[i].Date.Month == 9)
                            {
                                BasicFinancialInt = i;
                                break;
                            }
                        }
                    }
                }
                else if (dt.Year <= 2012 && dt.Year >= 2008)
                {
                    for (int i = 0; i < FinancialDataList.Count; i++)
                    {
                        if (dt.Month >= 11)
                        {
                            if (FinancialDataList[i].Date.Year == dt.Year && FinancialDataList[i].Date.Month == 9)
                            {
                                BasicFinancialInt = i;
                                break;
                            }
                        }
                        else if (dt.Month <= 3)
                        {
                            if (FinancialDataList[i].Date.Year == dt.Year - 1 && FinancialDataList[i].Date.Month == 9)
                            {
                                BasicFinancialInt = i;
                                break;
                            }
                        }
                    }
                }

            }
            #endregion
            #region 第四季
            if (dt.Year <= 2012 && dt.Year >= 2008)
            {
                if (dt.Month == 4)
                {
                    for (int i = 0; i < FinancialDataList.Count; i++)
                    {
                        if (FinancialDataList[i].Date.Year == dt.Year - 1 && FinancialDataList[i].Date.Month == 12)
                        {
                            BasicFinancialInt = i;
                            break;
                        }
                    }
                }
            }
            else if (dt.Year > 2012)
            {
                if (dt.Month == 4 || (dt.Month == 5 && dt.Day < 15))
                {
                    for (int i = 0; i < FinancialDataList.Count; i++)
                    {
                        if (FinancialDataList[i].Date.Year == dt.Year - 1 && FinancialDataList[i].Date.Month == 12)
                        {
                            BasicFinancialInt = i;
                            break;
                        }
                    }
                }
            }
            else if (dt.Year <= 2007 && dt.Year >= 2002)
            {
                if (dt.Month >= 4)
                {
                    for (int i = 0; i < FinancialDataList.Count; i++)
                    {
                        if (FinancialDataList[i].Date.Year == dt.Year - 1 && FinancialDataList[i].Date.Month == 12)
                        {
                            BasicFinancialInt = i;
                            break;
                        }
                    }
                }
                else if (dt.Month <= 8)
                {
                    for (int i = 0; i < FinancialDataList.Count; i++)
                    {
                        if (FinancialDataList[i].Date.Year == dt.Year && FinancialDataList[i].Date.Month == 12)
                        {
                            BasicFinancialInt = i;
                            break;
                        }
                    }
                }
            }
            else if (dt.Year <= 2001)
            {
                if (dt.Month < 4)
                {
                    for (int i = 0; i < FinancialDataList.Count; i++)
                    {
                        if (FinancialDataList[i].Date.Year == dt.Year - 1 && FinancialDataList[i].Date.Month == 12)
                        {
                            BasicFinancialInt = i;
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < FinancialDataList.Count; i++)
                    {
                        if (FinancialDataList[i].Date.Year == dt.Year && FinancialDataList[i].Date.Month == 12)
                        {
                            BasicFinancialInt = i;
                            break;
                        }
                    }
                }
            }
            #endregion
            #region 每月營收
            InitialRevenue(dt);
            #endregion

            //Console.WriteLine(DataList[BasicFinancialInt].Date.Year+" "+DataList[BasicFinancialInt].Date.Month);
            //Console.WriteLine(RevenueList[RevenueInt].Date);
        }
        #endregion

        #region 每月營收
        void InitialRevenue(DateTime dt)
        {
            RevenueInt = 0;
            if (dt.Day >= 10)
            {


                for (int i = 0; i < RevenueList.Count; i++)
                {
                    if (RevenueList[i].Date.Month.CompareTo(dt.Month - 1) == 0
                        && RevenueList[i].Date.Year.CompareTo(dt.Year) == 0
                        && (dt.Month != 1))
                    {
                        RevenueInt = i;
                        break;
                    }
                    else if (dt.Month == 1 && RevenueList[i].Date.Year.CompareTo(dt.Year - 1) == 0
                        && (RevenueList[i].Date.Month.CompareTo(12) == 0))
                    {
                        RevenueInt = i;
                        break;
                    }
                }

            }
            else if (dt.Day < 10)
            {
                if (dt.Month != 1)
                    InitialRevenue(new DateTime(dt.Year, dt.Month - 1, 15));
                else
                    InitialRevenue(new DateTime(dt.Year - 1, 12, 15));
            }
        }
        #endregion

        public bool ComparerFinancial(string CompareName, double CompareValue, int CompareSeasens , bool CompareValueIsBigger=false , bool Yearly = false)
        {
            if (BasicFinancialInt - CompareSeasens < 0 || BasicFinancialInt > FinancialDataList.Count) return false;

            double AverageValues = 0;

            for (int i = BasicFinancialInt; i > BasicFinancialInt - CompareSeasens; i--)
            {
                PropertyInfo property = FinancialDataList[i].GetType().GetProperty(CompareName);
                var Value = property.GetValue(FinancialDataList[i]);

                if (Value == null) return false;

                AverageValues += (double)Value;
            }

            if (!Yearly)
            {
                AverageValues = AverageValues / CompareSeasens;
            }
            else
            {
                AverageValues = AverageValues / CompareSeasens * 4;
            }

            if(CompareValueIsBigger)
            {
                return CompareValue > AverageValues;
            }
            else
            {
                return CompareValue < AverageValues;
            }
           
        }
        public bool ComparerMonthlyRevenue(string CompareName, double CompareValue, int CompareSeasens, bool CompareValueIsBigger = false)
        {
            if (RevenueInt < 0 || RevenueInt - CompareSeasens <= 0 || RevenueList.Count == 0 || RevenueInt > RevenueList.Count) return false;
            double AverageValues = 0;
    

            for (int i = RevenueInt; i > RevenueInt - CompareSeasens; i--)
            {
                PropertyInfo property = RevenueList[RevenueInt].GetType().GetProperty(CompareName);
                var Value = property.GetValue(RevenueList[RevenueInt]);

                if (Value == null)
                    return false;

                AverageValues += (double)Value;
            }

            AverageValues = AverageValues / CompareSeasens;

            if (CompareValueIsBigger)
            {
                return CompareValue > AverageValues;
            }
            else
            {
                return CompareValue < AverageValues;
            }

        }
    }
}