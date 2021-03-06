﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockSimulationMVC.Models
{
    public class BasicFinancialDataModel
    {
        public double ID { get; set; }
        public int Company { get; set; }
        public string CompanyName { get; set; }
        public DateTime Date { get; set; }
        public double? Quarter { get; set; }
        public double? QCashonHand_CashEquivalent { get; set; }
        public double? QFinancialAssetsatFairValueThroughProfitorLoss_Curr { get; set; }
        public double? QAvailable_for_saleFinancialAssets_Current { get; set; }
        public double? QHeolding_To_MaturityFinancialAssets_Current { get; set; }
        public double? QHedgingDerivativeFinancialAssets_Current { get; set; }
        public double? QFinancialAssetsCarriedatCost_Current { get; set; }
        public double? QBondInvestmentsonNotActiveMarket_Current { get; set; }
        public double? QAccounts_NotesReceivable_Trade { get; set; }
        public double? QOtherReceivable { get; set; }
        public double? QInventories { get; set; }
        public double? QPrepaidExpenses_Advance { get; set; }
        public double? QOtherCurrentAssets { get; set; }
        public double? QTotalCurrentAssets { get; set; }
        public double? QTotalFixedAssets { get; set; }
        public double? QTotalOtherAssets { get; set; }
        public double? QTotalAssets { get; set; }
        //public double? QShort_termBorrowing { get; set; }
        //public double? QBillsIssued { get; set; }
        //public double? QFinancialLiabilitiesatFairValueThroughPL_Non_Current { get; set; }
        public double? QPreferredStockLiabilities_Current { get; set; }
        public double? QAP_NP { get; set; }
        public double? QCurrentPortionofLong_TermDebt { get; set; }
        public double? QTotalCurrentLiabilities { get; set; }
        public double? QLong_TermLiabilities { get; set; }

        public double? QTotalOtherLong_TermLiabilities { get; set; }
        public double? QTotalLiabilities { get; set; }
        //public double? QCommonStocks { get; set; }
        //public double? QPreferredStocks { get; set; }
        //public double? QProceedsofNewSharesIssued { get; set; }
        //public double? QReserveforCapitalIncrease { get; set; }
        //public double? QCapitalReserve { get; set; }
        //public double? QLegalReserve { get; set; }
        public double? QAppropriatedRetainedEarnings { get; set; }
        public double? QUn_appropriatedRetained { get; set; }
        public double? QAdjustmentforFXTrade { get; set; }
        public double? QTotalStockholdersEquity { get; set; }
        public double? QTotalLiabilities_Equity { get; set; }
        public double? QNetSales { get; set; }
        public double? QCostofGoodsSold { get; set; }
        public double? QGrossProfit { get; set; }
        public double? QOperatingExpenses { get; set; }
        public double? QOperatingIncome { get; set; }
        public double? QInterestIncome { get; set; }
        public double? QTotalNon_Operating { get; set; }
        public double? QInterestExpenses { get; set; }
        public double? QTotalNon_operatingExpenses { get; set; }
        public double? QPre_TaxIncome { get; set; }
        public double? QIncomeTaxExpense { get; set; }
        public double? QOrdinaryIncome { get; set; }
        public double? QNetIncome { get; set; }
        public double? QEarningPerShare { get; set; }
        public double? QCashDividendPerShare { get; set; }
        public double? QStockDividendPerShare_Earning { get; set; }
        public double? QStockDividendPerShare_Capital { get; set; }
        public double? QCashFlowFromOperatingAction { get; set; }
        public double? QCashFlowfromInvestmentAction { get; set; }
        //public double? QCashFlowfromFinancingAction { get; set; }
        public double? QInfluencefromExchange { get; set; }
        public double? QChangeinCashFlow { get; set; }
        //public double? QDividendPaid { get; set; }
        //public double? QLoansDiscounted_BillsPurchased_net { get; set; }
        //public double? QDepositsfromMutualLoansAccounts { get; set; }
        //public double? QInvestListedStock_Curr { get; set; }
        //public double? QInvestListedStock_NonCurr { get; set; }
        //public double? QBorrowingfromNon_FinancialInstitutions { get; set; }
        //public double? QCurrentOfL_TDebt_ExceptBond_CB { get; set; }
        public double? QCurrentOfL_TDebt_CB_Bond { get; set; }
        public double? QLong_TermBorrowings { get; set; }
        public double? QBonds_Convertible { get; set; }
        public double? QInstallmentPayable { get; set; }
        public double? QOtherLong_TermBorrowing { get; set; }
        public double? QPreferredStocksLiabilities { get; set; }
        public double? QTotalDebt { get; set; }
        public double? QReturnOnTotalAssetsPercentage_C { get; set; }
        public double? QReturnonTotalAssetsPercentage_A { get; set; }
        public double? QReturnonTotalAssetsPercentage_B__EDITDA { get; set; }
        public double? QReturnonEquityPercentage_A { get; set; }
        public double? QReturnonEquityPercentage_B { get; set; }
        public double? QGrossMarginPercentage { get; set; }
        public double? QRealizedGrossProfitPercentage { get; set; }
        public double? QOperatingIncomePercentage { get; set; }
        public double? QPre_TaxIncomePercentage { get; set; }
        public double? QNetIncomePercentage { get; set; }
        public double? QNumbersofEmployee { get; set; }
        public double? QOperatingExpPercentage { get; set; }
        public double? QCashFlowfmOperatingCurrentLiabilitiesPercentage { get; set; }
        public double? QBookValuePerShare_B_ { get; set; }
        public double? QEPS_NetIncome_ExcludeDisposalGL_ { get; set; }
        public double? QCashFlowPerShare { get; set; }
        public double? QSalesPerShare { get; set; }
        public double? QOperatingIncomePerShare { get; set; }
        public double? QPre_TaxIncomePerShare { get; set; }
        public double? QSalesGrowthRate { get; set; }
        public double? QGrossMarginGrowthRate { get; set; }
        public double? QRealizedGrossProfitGrowthRate { get; set; }
        public double? QOperatingIncomeGrowthRate { get; set; }
        public double? QPre_TaxIncomeGrowthRate { get; set; }
        public double? QNetIncomeGrowthRate { get; set; }
        public double? QTotalAssetsGrowthRate { get; set; }
        public double? QTotalEquityGrowthRate { get; set; }
        public double? QReturnonTotalAssetsGrowthPercentage { get; set; }
        public double? QRevenueGrowthRate_Quarterly { get; set; }
        public double? QOperatingIncomeGrowthRate_Quarterly { get; set; }
        public double? QNetIncomeGrowthRate_Quarterly { get; set; }
        public double? QCurrentRatio { get; set; }
        public double? QLiabilitiesEquityRatio { get; set; }
        public double? QLiabilitiesPercentage { get; set; }
        public double? QEquityTotalAssets { get; set; }
        public double? QInterest_BearingDebtShareholdersEquity { get; set; }
        //public double? QTimesInterestEarned { get; set; }
        //public double? QOperatingIncomePerShares { get; set; }
        //public double? QPre_TaxIncomeCapital { get; set; }
        public double? QTotalAssetTurnover { get; set; }
        public double? QAccountsReceivablesTurnover { get; set; }
        public double? QDaysReceivablesOutstanding { get; set; }
        public double? QInventoryTurnover { get; set; }
        public double? QDaysInventoryOutstanding { get; set; }
        public double? QFixedAssetTurnover { get; set; }
        public double? QEquityTurnover { get; set; }
        //public double? QDaysPayablesOutstanding { get; set; }
        //public double? QNetOperatingCycle { get; set; }
        //public double? QSalesPerEmployee { get; set; }
        //public double? QOperationIncomePerEmployee { get; set; }
        //public double? QFixedAssetsPerEmployee { get; set; }
        public double? QPE { get; set; }
        public double? QPB { get; set; }
        public double? QPSR { get; set; }
        public double? QDividendYield { get; set; }
        public double? QCashDividendYield { get; set; }
        public double? QBookValuePerShare_F_ { get; set; }
        public double? MarketCap { get; set; }


    }
}