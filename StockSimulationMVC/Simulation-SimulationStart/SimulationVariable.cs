using StockSimulationMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace StockSimulationMVC.Simulation_SimulationStart
{
    public class SimulationVariable
    {
        public bool CanBuy { get; set; }
        public bool HasBuy { get; set; }
        public bool CanSell { get; set; }
        public bool HasSell { get; set; }
        public TechnologicalDataModel Buy { get; set; }
        TechnologicalDataModel CurrentlyTechData { get; set; }
        public decimal MoveStopLoss { get; set; }
        public double? MoveStopLossPercentage { get; set; }
        decimal StopLossCalPrice { get; set; }
        double? StopLossCalPricePercentage { get; set; }
        public int HaveStockDayContainHoliday { get; set; }
        public int HaveStockDay { get; set; }
        public double Accumulation { get; set; }
        public double AccumulationRise { get; set; }
        public double AccumulationDrop { get; set; }
        public bool PreSell { get; set; }
        public double startToCountDropAfterPreSell { get; set; }
        public int CountDropDay { get; set; }
        public int Circumstance { get; set; }
        public decimal MaxRatio { get; set; }
        //public double MoveStopLossPercnetage { get; set; }
        //public double MoveStopLoss { get; set; }
        public Guid Number { get; }

        public SimulationVariable()
        {
            Number = Guid.NewGuid();
            Initial();
        }
        public void Initial()
        {
            CountDropDay = 0;
            CanBuy = false;
            CanSell = false;
            HasBuy = false;
            HasSell = true;
            PreSell = false;
            HaveStockDay = 0;
            Accumulation = 0;
            AccumulationDrop = 0;
            AccumulationRise = 0;
            startToCountDropAfterPreSell = 0;
            MaxRatio = 0;

        }

        public bool ConditionSatified(double CompareValue, string CompareProperty, bool IsbiggerOrEqual = true)
        {
            PropertyInfo property = this.GetType().GetProperty(CompareProperty);
            double Value = Convert.ToDouble( property.GetValue(this));
            double IsBiggerThanZero = Value - CompareValue;

            bool _IsConditionSatified = IsBiggerThanZero >= 0.0;

            return IsbiggerOrEqual ? _IsConditionSatified : !_IsConditionSatified;
        }

       
        public bool ConditionSatifiedMoveStopLoss(string StopLossType)
        {
            switch(StopLossType)
            {
                case "MoveStopLoss":
                    if (CurrentlyTechData.ClosePrice < StopLossCalPrice - MoveStopLoss)
                        return true;
                    break;
                case "MoveStopLossPercentage":
                    if (Accumulation < (double)(StopLossCalPricePercentage - MoveStopLossPercentage))
                        return true;
                    break;
            }
            
            return false;
        }

        public void CountDays(TechnologicalDataModel TechData)//double UnrealizedGains , decimal ClosePrice)
        {
            HaveStockDay++;
            HaveStockDayContainHoliday = (TechData.Date-Buy.Date).Days;

            MaxRatio = (TechData.HighestPrice - Buy.ClosePrice) / Buy.ClosePrice * 100;

            Accumulation = (double)((TechData.ClosePrice - Buy.ClosePrice) / Buy.ClosePrice * 100);// UnrealizedGains;
            CurrentlyTechData = TechData;


            if ((double)TechData.ReturnOnInvestment > 0)
                AccumulationRise += (double)TechData.ReturnOnInvestment;
            else if ((double)TechData.ReturnOnInvestment < 0)
                AccumulationDrop += (double)TechData.ReturnOnInvestment;

            if (HaveStockDay == 1)
            {
                StopLossCalPrice = Buy.ClosePrice;
                StopLossCalPricePercentage = 0;
            }
            if (TechData.ClosePrice > StopLossCalPrice)
            {
                StopLossCalPrice = TechData.ClosePrice;
            }
            if(Accumulation > StopLossCalPricePercentage)
            {
                StopLossCalPricePercentage = (double)Accumulation;
            }
                
        }
    }
}