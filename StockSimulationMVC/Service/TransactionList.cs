using StockSimulationMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockSimulationMVC.Service
{
    public class TransactionList
    {
        public List<Transaction> _TransactionList;
        public double Win=0;
        public double Loss = 0;
        public double WinRatio = 0;
        public double AverageHoldDays = 0;
        public decimal ExpectedRateOfReturn = 0;
        public decimal MaxRateOfReturn = 0;
        public decimal MinRateOfReturn = 0;
        public decimal StandardDeviationRateOfReturn = 0;
        public decimal TotalReturnRate = 0;

        public TransactionList()
        {
            _TransactionList = new List<Transaction>();
        }

        public void TransactionStatisticResult()
        {
            if (_TransactionList.Count == 0) return;

            for(int i=0; i< _TransactionList.Count;i++)
            {
                if (_TransactionList[i].Result.RateOfReturn > 0.5m)
                    Win++;
                else
                    Loss++;

                AverageHoldDays += _TransactionList[i].Result.HoldDays;
                ExpectedRateOfReturn += _TransactionList[i].Result.RateOfReturn;

                if (_TransactionList[i].Result.RateOfReturn > MaxRateOfReturn)
                    MaxRateOfReturn = _TransactionList[i].Result.RateOfReturn;

                if (_TransactionList[i].Result.RateOfReturn < MinRateOfReturn)
                    MinRateOfReturn = _TransactionList[i].Result.RateOfReturn;

                StandardDeviationRateOfReturn += _TransactionList[i].Result.RateOfReturn * _TransactionList[i].Result.RateOfReturn;
            }

            WinRatio = Math.Round(Win / (Win + Loss) * 100, 2);
            AverageHoldDays = Math.Round(AverageHoldDays / _TransactionList.Count , 2);
            ExpectedRateOfReturn = Math.Round(ExpectedRateOfReturn / _TransactionList.Count, 2);

            StandardDeviationRateOfReturn = Math.Round(StandardDeviationRateOfReturn / _TransactionList.Count, 2);
            StandardDeviationRateOfReturn = (decimal)Math.Sqrt((double)Math.Abs(StandardDeviationRateOfReturn - ExpectedRateOfReturn * ExpectedRateOfReturn));
            TotalReturnRate = (ExpectedRateOfReturn - (Decimal)0.5) * (Decimal)(Win + Loss);
        }

        public void AddTransactionList(List<Transaction> transactiondata)
        {
            foreach(var transaction in transactiondata)
            {
                _TransactionList.Add(transaction);
            }
        }
    }
}