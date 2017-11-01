using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace StockSimulationMVC.Core
{
    public abstract class LineGraph
    {
        List<double> SelectedData;//要畫線圖的股票技術分析資料
        Dictionary<string, List<double>> LineGraphDictionarny;



        public LineGraph()
        {
            SelectedData = new List<double>();
            LineGraphDictionarny = new Dictionary<string, List<double>>();
        }

        public void LineGraphData<T>(ref List<T> Data, string SelectDataName)
        {
            SelectedData.Clear();

            for (int i = 0; i < Data.Count; i++)
            {
                PropertyInfo DataProperty = Data[i].GetType().GetProperty(SelectDataName);

                var DataTemp = DataProperty.GetValue(Data[i]).ToString();

                SelectedData.Add(double.Parse(DataTemp));
            }
        }

        public bool CoditionSatified(string StrategyName1 , string StrategyName2 ,int DaysCounter,  int Times=1)//StrategyName1 is bigger than StrategyNames2
        {
            bool _IsConditionSatified = LineGraphDictionarny[StrategyName1][DaysCounter] > Times * LineGraphDictionarny[StrategyName2][DaysCounter];

            return  _IsConditionSatified ;
        }

        public bool CoditionSatifiedIsBiggerValue(string StrategyName1, int DaysCounter, double Value)//StrategyName1 is bigger than StrategyNames2
        {
            bool _IsConditionSatified = LineGraphDictionarny[StrategyName1][DaysCounter] > Value;

            return _IsConditionSatified;
        }

        public void AddLineGraphDictionary(string StrategyName , int Days , double BollingerParameter = 2.0 )
        {
            MethodInfo method = this.GetType().GetMethod(StrategyName);

            switch(StrategyName)
            {
                case "BollingerBandsDown":
                case "BollingerBandsUp":
                    var BollingerLine = method.Invoke(this, new object[] { Days, BollingerParameter });
                    LineGraphDictionarny.Add(StrategyName + "-" + Days, (List<double>)BollingerLine);
                    break;

                default:
                    var DefaultLine = method.Invoke(this, new object[] { Days });
                    LineGraphDictionarny.Add(StrategyName + "-" + Days, (List<double>)DefaultLine);
                    break;
            }
  

        }
       
        public List<double> MoveAverageValue(int AverageDays)//移動平均數
        {
            double sum = 0;
            List<double> MoveAverageData = new List<double>();

            for(int i=0; i<AverageDays; i++)
            {
                MoveAverageData.Add(0);
            }

            for (int i = AverageDays; i < SelectedData.Count; i++)
            {

                for (int j = 0; j < AverageDays; j++)
                {
                    sum += SelectedData[i - j];
                }

                MoveAverageData.Add(sum / AverageDays);
                sum = 0;
            }



            return MoveAverageData;
        }
        public List<double> MaxValue(int MaxDays)//新高
        {
            double Max = 0;
            List<double> MaxData = new List<double>();

            for (int i = 0; i < MaxDays; i++)
            {
                MaxData.Add(0);
            }

            for (int i = MaxDays; i < SelectedData.Count; i++)
            {


                for (int j = 0; j < MaxDays; j++)
                {
                    if (SelectedData[i - j] > Max)
                        Max = SelectedData[i - j];
                }

                MaxData.Add(Max);
                Max = 0;
            }
            return MaxData;
        }
        public List<double> MinValue(int MinDays)//新低
        {
            double Min = int.MaxValue;
            List<double> MinData = new List<double>();

            for (int i = 0; i < MinDays; i++)
            {
                MinData.Add(0);
            }

            for (int i = MinDays; i < SelectedData.Count; i++)
            {

                for (int j = 0; j < MinDays; j++)
                {
                    if (SelectedData[i - j] < Min)
                        Min = SelectedData[i - j];
                }

                MinData.Add(Min);
                Min = int.MaxValue;
            }

            return MinData;
        }
        public List<double> Acculation(int AcculationDays)//多日累積
        {
            double Acc = 0;
            List<double> Accul = new List<double>();

            for (int i = 0; i < AcculationDays; i++)
            {
                Accul.Add(0);
            }

            for (int i = AcculationDays; i < SelectedData.Count; i++)
            {
                for (int j = 0; j < AcculationDays; j++)
                {
                    Acc += SelectedData[i - j];
                }

                Accul.Add(Acc);
                Acc = 0;
            }

            return Accul;
        }
        public List<double> BollingerBandsUp(int Days, double BollingerParameter = 2.0)
        {
            List<double> StandardDeviationData = StandardDeviation(Days);
            List<double> MoveAverageData = MoveAverageValue(Days);
            List<double> BollingerBandsUpBand = new List<double>();

            for (int i = 0; i < Days; i++)
            {
                BollingerBandsUpBand.Add(0);
            }

            for (int i = Days; i < SelectedData.Count; i++)
            {
                BollingerBandsUpBand.Add(MoveAverageData[i] + BollingerParameter * StandardDeviationData[i]);
            }

            return BollingerBandsUpBand;
        }
        public List<double> BollingerBandsDown(int Days , double BollingerParameter = 2.0)
        {
            List<double> StandardDeviationData = StandardDeviation(Days);
            List<double> MoveAverageData = MoveAverageValue(Days);
            List<double> BollingerBandsDownBand = new List<double>();

            for (int i = 0; i < Days; i++)
            {
                BollingerBandsDownBand.Add(0);
            }

            for (int i = Days; i < SelectedData.Count; i++)
            {
                BollingerBandsDownBand.Add(MoveAverageData[i] - BollingerParameter * StandardDeviationData[i]);
            }

            return BollingerBandsDownBand;
        }
        private List<double> StandardDeviation(int Days)
        {
            double SquareSum = 0;
            double Standard_Deviation = 0;

            List<double> AverageValue = MoveAverageValue(Days);
            List<double> StandardDeviationData = new List<double>();

            for (int i = 0; i < Days; i++)
            {
                StandardDeviationData.Add(0);
            }

            for (int i = Days; i < SelectedData.Count; i++)
            {

                for (int j = 0; j < Days; j++)
                {
                    SquareSum += SelectedData[i - j] * SelectedData[i - j];
                }

                SquareSum = SquareSum / Days;

                Standard_Deviation = Math.Sqrt((SquareSum - (AverageValue[i] * AverageValue[i])));

                StandardDeviationData.Add(Standard_Deviation);
                
                SquareSum = 0;
            }



            return StandardDeviationData;
        }
       
    }
}