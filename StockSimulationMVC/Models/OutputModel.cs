using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockSimulationMVC.Models
{
    public class OutputModel
    {
        public double Evaluation_1Year { get; set; }
        public double Evaluation_2Year { get; set; }
        public double Evaluation_3Year { get; set; }
        public double Evaluation_4Year { get; set; }
        public double Evaluation_5Year { get; set; }
        public double operationIncomePercentageCount { get; set; }
        public double netIncomePercentageCount { get; set; }
        public double EPSCount { get; set; }
        public double EPSQoQ { get; set; }
        public double EPSYoY { get; set; }
        public double EPSAccumulationYoY { get; set; }

    }
}