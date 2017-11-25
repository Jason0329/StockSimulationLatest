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
        Hashtable LineGraphTable;
        Dictionary<string, List<double>> LineGraphDictionarny;



        public LineGraph()
        {
            LineGraphDictionarny = new Dictionary<string, List<double>>();
            LineGraphTable = new Hashtable();
        }

        public void LineGraphData<T>(ref List<T> Data, string SelectDataName)
        {
            SelectedData = new List<double>();

            for (int i = 0; i < Data.Count; i++)
            {
                PropertyInfo DataProperty = Data[i].GetType().GetProperty(SelectDataName);

                var DataTemp = DataProperty.GetValue(Data[i]).ToString();

                SelectedData.Add(double.Parse(DataTemp));
                
            }

            LineGraphTable.Add(SelectDataName, SelectedData);
        }

        public bool CoditionSatified(string StrategyName1 , string StrategyName2 ,int DaysCounter,  double Times=1 , bool IsBigger = true)//StrategyName1 is bigger than StrategyNames2
        {
            bool _IsConditionSatified = LineGraphDictionarny[StrategyName1][DaysCounter] > Times * LineGraphDictionarny[StrategyName2][DaysCounter];

            if(!IsBigger)
            {
                _IsConditionSatified = !_IsConditionSatified;
            }

            return  _IsConditionSatified ;
        }

        public double ReturnValue(string StrategyName1, int DaysCounter)
        {
            return LineGraphDictionarny[StrategyName1][DaysCounter];
        }

        public bool CoditionSatifiedIsBiggerValue(string StrategyName1, int DaysCounter, double Value)//StrategyName1 is bigger than StrategyNames2
        {
            bool _IsConditionSatified = LineGraphDictionarny[StrategyName1][DaysCounter] > Value;

            return _IsConditionSatified;
        }

        public void AddLineGraphDictionary(string StrategyName , int Days , string DataType="ClosePrice" , double BollingerParameter = 2.0 )
        {
            MethodInfo method = this.GetType().GetMethod(StrategyName);

            switch(StrategyName)
            {
                case "BollingerBandsDown":
                case "BollingerBandsUp":
                    var BollingerLine = method.Invoke(this, new object[] { Days, DataType, BollingerParameter });
                    LineGraphDictionarny.Add(StrategyName + "-" + Days, (List<double>)BollingerLine);
                    break;

                default:
                    var DefaultLine = method.Invoke(this, new object[] { Days, DataType });
                    LineGraphDictionarny.Add(StrategyName + "-" + Days, (List<double>)DefaultLine);
                    break;
            }
  

        }
       /*
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

            for (int i = Days; i < MoveAverageData.Count; i++)
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

            for (int i = Days; i < MoveAverageData.Count; i++)
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
        */
        public List<double> MoveAverageValue(int AverageDays, string DataType)//移動平均數
        {
            double sum = 0;
            List<double> MoveAverageData = new List<double>();
            List<double> Data = (List<double>)LineGraphTable[DataType];

            for (int i = 0; i < AverageDays; i++)
            {
                MoveAverageData.Add(0);
            }

            for (int i = AverageDays; i < Data.Count; i++)
            {

                for (int j = 0; j < AverageDays; j++)
                {
                    sum += Data[i - j];
                }

                MoveAverageData.Add(sum / AverageDays);
                sum = 0;
            }



            return MoveAverageData;
        }
        public List<double> MaxValue(int MaxDays, string DataType)//新高
        {
            double Max = 0;
            List<double> MaxData = new List<double>();
            List<double> Data = (List<double>)LineGraphTable[DataType];

            for (int i = 0; i < MaxDays; i++)
            {
                MaxData.Add(0);
            }

            for (int i = MaxDays; i < Data.Count; i++)
            {


                for (int j = 0; j < MaxDays; j++)
                {
                    if (Data[i - j] > Max)
                        Max = Data[i - j];
                }

                MaxData.Add(Max);
                Max = 0;
            }
            return MaxData;
        }
        public List<double> MinValue(int MinDays, string DataType)//新低
        {
            double Min = int.MaxValue;
            List<double> MinData = new List<double>();
            List<double> Data = (List<double>)LineGraphTable[DataType];

            for (int i = 0; i < MinDays; i++)
            {
                MinData.Add(0);
            }

            for (int i = MinDays; i < Data.Count; i++)
            {

                for (int j = 0; j < MinDays; j++)
                {
                    if (Data[i - j] < Min)
                        Min = Data[i - j];
                }

                MinData.Add(Min);
                Min = int.MaxValue;
            }

            return MinData;
        }
        public List<double> Acculation(int AcculationDays, string DataType)//多日累積
        {
            double Acc = 0;
            List<double> Accul = new List<double>();
            List<double> Data = (List<double>)LineGraphTable[DataType];

            for (int i = 0; i < AcculationDays; i++)
            {
                Accul.Add(0);
            }

            for (int i = AcculationDays; i < Data.Count; i++)
            {
                for (int j = 0; j < AcculationDays; j++)
                {
                    Acc += Data[i - j];
                }

                Accul.Add(Acc);
                Acc = 0;
            }

            return Accul;
        }
        public List<double> BollingerBandsUp(int Days, string DataType, double BollingerParameter = 2.0)
        {
            List<double> StandardDeviationData = StandardDeviation(Days, DataType);
            List<double> MoveAverageData = MoveAverageValue(Days, DataType);
            List<double> BollingerBandsUpBand = new List<double>();


            for (int i = 0; i < Days; i++)
            {
                BollingerBandsUpBand.Add(0);
            }

            for (int i = Days; i < MoveAverageData.Count; i++)
            {
                BollingerBandsUpBand.Add(MoveAverageData[i] + BollingerParameter * StandardDeviationData[i]);
            }

            return BollingerBandsUpBand;
        }
        public List<double> BollingerBandsDown(int Days, string DataType, double BollingerParameter = 2.0)
        {
            List<double> StandardDeviationData = StandardDeviation(Days, DataType);
            List<double> MoveAverageData = MoveAverageValue(Days, DataType);
            List<double> BollingerBandsDownBand = new List<double>();

            for (int i = 0; i < Days; i++)
            {
                BollingerBandsDownBand.Add(0);
            }

            for (int i = Days; i < MoveAverageData.Count; i++)
            {
                BollingerBandsDownBand.Add(MoveAverageData[i] - BollingerParameter * StandardDeviationData[i]);
            }

            return BollingerBandsDownBand;
        }
        private List<double> StandardDeviation(int Days, string DataType)
        {
            double SquareSum = 0;
            double Standard_Deviation = 0;

            List<double> AverageValue = MoveAverageValue(Days, DataType);
            List<double> StandardDeviationData = new List<double>();
            List<double> Data = (List<double>)LineGraphTable[DataType];

            for (int i = 0; i < Days; i++)
            {
                StandardDeviationData.Add(0);
            }

            for (int i = Days; i < Data.Count; i++)
            {

                for (int j = 0; j < Days; j++)
                {
                    SquareSum += Data[i - j] * Data[i - j];
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