import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AccTrnCashbooksummaryComponent } from './Component/acc-trn-cashbooksummary/acc-trn-cashbooksummary.component';
import { AccTrnBankbooksummaryComponent } from './Component/acc-trn-bankbooksummary/acc-trn-bankbooksummary.component';
import { AccTrnBankbookComponent } from './Component/acc-trn-bankbook/acc-trn-bankbook.component';
import { AccMstBankmasterAddComponent } from './Component/acc-mst-bankmaster-add/acc-mst-bankmaster-add.component';
import { AccMstBankmasterComponent } from './Component/acc-mst-bankmaster/acc-mst-bankmaster.component';
import { AccTrnBankbookaddentryComponent } from './Component/acc-trn-bankbookaddentry/acc-trn-bankbookaddentry.component';
import { AccTrnCashbookSelectComponent } from './Component/acc-trn-cashbook-select/acc-trn-cashbook-select.component';
// import { AccTrnAddcashbookComponent } from './Component/acc-trn-addcashbook/acc-trn-addcashbook.component';
import { AccTrnCashbookeditComponent } from './Component/acc-trn-cashbookedit/acc-trn-cashbookedit.component';
import { AccMstBankmasterEditComponent } from './Component/acc-mst-bankmaster-edit/acc-mst-bankmaster-edit.component';
import { AccMstOpeningbalanceComponent } from './Component/acc-mst-openingbalance/acc-mst-openingbalance.component';
import { AccMstCreditcardmasterSummaryComponent } from './Component/acc-mst-creditcardmaster-summary/acc-mst-creditcardmaster-summary.component';
import { AccMstGlcodeSummaryComponent } from './Component/acc-mst-glcode-summary/acc-mst-glcode-summary.component';
import { AccTrnCreditcardbookSummaryComponent } from './Component/acc-trn-creditcardbook-summary/acc-trn-creditcardbook-summary.component';
import { AccTrnJournalentrySummaryComponent } from './Component/acc-trn-journalentry-summary/acc-trn-journalentry-summary.component';
import { AccTrnSundrysalesSummaryComponent } from './Component/acc-trn-sundrysales-summary/acc-trn-sundrysales-summary.component';
import { AccTrnGstmanagementSummaryComponent } from './Component/acc-trn-gstmanagement-summary/acc-trn-gstmanagement-summary.component';
import { AccRptIncomeandEpenditureReportComponent } from './Component/acc-rpt-incomeand-ependiture-report/acc-rpt-incomeand-ependiture-report.component';
import { AccTrnReceivableinvoiceSummaryComponent } from './Component/acc-trn-receivableinvoice-summary/acc-trn-receivableinvoice-summary.component';
import { AccTrnBankbookentryaddComponent } from './Component/acc-trn-bankbookentryadd/acc-trn-bankbookentryadd.component';
import { AccTrnCashbookentryaddComponent } from './Component/acc-trn-cashbookentryadd/acc-trn-cashbookentryadd.component';
import { AccTrnTaxmanagementComponent } from './Component/acc-trn-taxmanagement/acc-trn-taxmanagement.component';
import { AccTrnTaxfillingComponent } from './Component/acc-trn-taxfilling/acc-trn-taxfilling.component';
import { AccTrnTaxmanagementviewComponent } from './Component/acc-trn-taxmanagementview/acc-trn-taxmanagementview.component';
import { AccTrnJournalentryAddComponent } from './Component/acc-trn-journalentry-add/acc-trn-journalentry-add.component';
import { AccTrnFundtransfersummaryComponent } from './Component/acc-trn-fundtransfersummary/acc-trn-fundtransfersummary.component';
import { AccTrnFundtransferapprovalComponent } from './Component/acc-trn-fundtransferapproval/acc-trn-fundtransferapproval.component';
import { AccTrnGlcodedebitoraddComponent } from './Component/acc-trn-glcodedebitoradd/acc-trn-glcodedebitoradd.component';
import { AccTrnGlcodecreditoraddComponent } from './Component/acc-trn-glcodecreditoradd/acc-trn-glcodecreditoradd.component';
import { AccTrnGstinsummaryComponent } from './Component/acc-trn-gstinsummary/acc-trn-gstinsummary.component';
import { AccTrnGstoutsummaryComponent } from './Component/acc-trn-gstoutsummary/acc-trn-gstoutsummary.component';
import { AccTrnGstfillingsummaryComponent } from './Component/acc-trn-gstfillingsummary/acc-trn-gstfillingsummary.component';
import { AccRptDebtorreportComponent } from './Component/acc-rpt-debtorreport/acc-rpt-debtorreport.component';
import { AccRptDebtorreportviewComponent } from './Component/acc-rpt-debtorreportview/acc-rpt-debtorreportview.component';
import { AccRptCreditorreportComponent } from './Component/acc-rpt-creditorreport/acc-rpt-creditorreport.component';
import { AccRptCreditorreportviewComponent } from './Component/acc-rpt-creditorreportview/acc-rpt-creditorreportview.component';
import { AccRptLedgerreportComponent } from './Component/acc-rpt-ledgerreport/acc-rpt-ledgerreport.component';
import { AccMstChartofaccountchildviewComponent } from './Component/acc-mst-chartofaccountchildview/acc-mst-chartofaccountchildview.component';
import { AccMstChartofaccountincomeComponent } from './Component/acc-mst-chartofaccountincome/acc-mst-chartofaccountincome.component';
import { AccMstChartofaccountliabilityComponent } from './Component/acc-mst-chartofaccountliability/acc-mst-chartofaccountliability.component';
import { AccMstChartofaccountassetComponent } from './Component/acc-mst-chartofaccountasset/acc-mst-chartofaccountasset.component';
import { AccMstChartofaccountincomeviewComponent } from './Component/acc-mst-chartofaccountincomeview/acc-mst-chartofaccountincomeview.component';
import { AccMstChartofaccountliabilityviewComponent } from './Component/acc-mst-chartofaccountliabilityview/acc-mst-chartofaccountliabilityview.component';
import { AccMstChartofaccountassetviewComponent } from './Component/acc-mst-chartofaccountassetview/acc-mst-chartofaccountassetview.component';
import { AccTrnJournalentryEditComponent } from './Component/acc-trn-journalentry-edit/acc-trn-journalentry-edit.component';
import { AccRptOpenbalancereportComponent } from './Component/acc-rpt-openbalancereport/acc-rpt-openbalancereport.component';
import { AccRptProfileandlostreportComponent } from './Component/acc-rpt-profileandlostreport/acc-rpt-profileandlostreport.component';
import { AccRptTrialbalanceComponent } from './Component/acc-rpt-trialbalance/acc-rpt-trialbalance.component';
import { AccRptProfileandlossdetailsComponent } from './Component/acc-rpt-profileandlossdetails/acc-rpt-profileandlossdetails.component';
import { AccRptBalancesheeetreportComponent } from './Component/acc-rpt-balancesheeetreport/acc-rpt-balancesheeetreport.component';
import { AccTrnRecordExpenseComponent } from './Component/acc-trn-record-expense/acc-trn-record-expense.component';
import { AccTrnExpenseAddComponent } from './Component/acc-trn-expense-add/acc-trn-expense-add.component';
import { AccTrnMakePaymentComponent } from './Component/acc-trn-make-payment/acc-trn-make-payment.component';
import { AccRptDebtorDetailedreportComponent } from './Component/acc-rpt-debtor-detailedreport/acc-rpt-debtor-detailedreport.component';
import { AccTrnFinancedashboardComponent } from './Component/acc-trn-financedashboard/acc-trn-financedashboard.component';
import { AccMstChartofaccountsComponent } from './Component/acc-mst-chartofaccounts/acc-mst-chartofaccounts.component';
import { AccRptVendor360Component } from './Component/acc-rpt-vendor360/acc-rpt-vendor360.component';
import { AccRptCreditordetailedreportComponent } from './Component/acc-rpt-creditordetailedreport/acc-rpt-creditordetailedreport.component';
import { AccRptJournalentryBookComponent } from './Component/acc-rpt-journalentry-book/acc-rpt-journalentry-book.component';
import { SmrTrnSaleslegderComponent } from './Component/smr-trn-saleslegder/smr-trn-saleslegder.component';
import { PmrTrnPurchaselegderComponent } from './Component/pmr-trn-purchaselegder/pmr-trn-purchaselegder.component';
import { AccRptBalancesheetstatementComponent } from './Component/acc-rpt-balancesheetstatement/acc-rpt-balancesheetstatement.component';
import { AccRptIncomeandexpensestatementComponent } from './Component/acc-rpt-incomeandexpensestatement/acc-rpt-incomeandexpensestatement.component';
import { AccRptBalancesheetreportviewComponent } from './Component/acc-rpt-balancesheetreportview/acc-rpt-balancesheetreportview.component';
import { AccRptLedgerassetreportComponent } from './Component/acc-rpt-ledgerassetreport/acc-rpt-ledgerassetreport.component';
import { AccRptLedgerliabilityreportComponent } from './Component/acc-rpt-ledgerliabilityreport/acc-rpt-ledgerliabilityreport.component';
import { AccTrnSundryexpenseComponent } from './Component/acc-trn-sundryexpense/acc-trn-sundryexpense.component';
import { AccTrnAddsundryexpenseComponent } from './Component/acc-trn-addsundryexpense/acc-trn-addsundryexpense.component';
import { AccTrnEditsundryexpensesComponent } from './Component/acc-trn-editsundryexpenses/acc-trn-editsundryexpenses.component';
import { AccTrnViewsundryexpensesComponent } from './Component/acc-trn-viewsundryexpenses/acc-trn-viewsundryexpenses.component';
import { AccRptIncomeExpenseComponent } from './Component/acc-rpt-income-expense/acc-rpt-income-expense.component';
import { AccTrnPaymentSummaryComponent } from './Component/acc-trn-payment-summary/acc-trn-payment-summary.component';
import { AccTrnAddPaymentComponent } from './Component/acc-trn-add-payment/acc-trn-add-payment.component';
import { AccTrnPaymentaddconfirmComponent } from './Component/acc-trn-paymentaddconfirm/acc-trn-paymentaddconfirm.component';
import { AccTrnMultipleExpenses2singlepaymentComponent } from './Component/acc-trn-multiple-expenses2singlepayment/acc-trn-multiple-expenses2singlepayment.component';
import { AccTrnSundrypaymentViewComponent } from './Component/acc-trn-sundrypayment-view/acc-trn-sundrypayment-view.component';
import { AccTrnFinanceregulationComponent } from './Component/acc-trn-financeregulation/acc-trn-financeregulation.component';
import { AccRptLedgerincomereportComponent } from './Component/acc-rpt-ledgerincomereport/acc-rpt-ledgerincomereport.component';
import { AccRptLedgerexpensereportComponent } from './Component/acc-rpt-ledgerexpensereport/acc-rpt-ledgerexpensereport.component';
import { AccRptPurchaseledgerviewComponent } from './Component/acc-rpt-purchaseledgerview/acc-rpt-purchaseledgerview.component';


const routes: Routes = [
  { path: 'AccTrnCashbooksummary', component: AccTrnCashbooksummaryComponent },
  { path: 'AccTrnBankbooksummary', component: AccTrnBankbooksummaryComponent },
  { path: 'AccTrnBankbook/:bank_gid/:finyear_gid', component: AccTrnBankbookComponent },
  { path: 'AccMstBankMasterAdd', component: AccMstBankmasterAddComponent },
  { path: 'AccMstBankMasterSummary', component: AccMstBankmasterComponent },
  { path: 'AccTrnBankBookEntry/:bank_gid', component: AccTrnBankbookaddentryComponent },
  { path: 'AccTrnCashbookedit/:branch_gid', component: AccTrnCashbookeditComponent },
  // { path: 'AccTrnAddcashbook', component:AccTrnAddcashbookComponent}
  { path: 'AccTrnCashbookSelect/:branch_gid/:finyear_gid', component: AccTrnCashbookSelectComponent },
  { path: 'AccMstBankmasterEdit/:bank_gid', component: AccMstBankmasterEditComponent },
  { path: 'AccMstOpeningbalance', component: AccMstOpeningbalanceComponent },
  { path: 'AccMstCreditcardmasterSummary', component: AccMstCreditcardmasterSummaryComponent },
  { path: 'AccMstGlcodeSummary', component: AccMstGlcodeSummaryComponent },
  { path: 'AccTrnCreditcardbookSummary', component: AccTrnCreditcardbookSummaryComponent },
  { path: 'AccTrnJournalentrySummary', component: AccTrnJournalentrySummaryComponent },
  { path: 'AccTrnSundrysalesSummary', component: AccTrnSundrysalesSummaryComponent },
  { path: 'AccTrnGstmanagementSummary', component: AccTrnGstmanagementSummaryComponent },
  { path: 'AccRptIncomeandEpenditureReport', component: AccRptIncomeandEpenditureReportComponent },
  { path: 'AccTrnReceivableinvoiceSummary', component: AccTrnReceivableinvoiceSummaryComponent },
  // { path: 'AccTrnBankBookEntryAdd/:bank_gid/:finyear_gid', component: AccTrnBankbookentryaddComponent},
  { path: 'AccTrnBankBookEntryAdd/:bank_gid/:from_date/:to_date', component: AccTrnBankbookentryaddComponent },
  { path: 'AccTrnCashBookEntryAdd/:branch_gid/:finyear_gid', component: AccTrnCashbookentryaddComponent },
  { path: 'AccTrnTaxManagement', component: AccTrnTaxmanagementComponent },
  { path: 'AccTrnTaxFilling', component: AccTrnTaxfillingComponent },
  { path: 'AccTrnTaxManagementView/:taxfiling_gid', component: AccTrnTaxmanagementviewComponent },
  { path: 'AccTrnJournalentryAdd', component: AccTrnJournalentryAddComponent },
  { path: 'AccTrnFundTransferSummary', component: AccTrnFundtransfersummaryComponent },
  { path: 'AccTrnFundTransferApproval', component: AccTrnFundtransferapprovalComponent },
  { path: 'AccTrnGLCodeDebitorAdd', component: AccTrnGlcodedebitoraddComponent },
  { path: 'AccTrnGLCodeCreditorAdd', component: AccTrnGlcodecreditoraddComponent },
  { path: 'AccTrnGstInSummary/:lsmonth/:lsyear', component: AccTrnGstinsummaryComponent },
  { path: 'AccTrnGstOutSummary/:lsmonth/:lsyear', component: AccTrnGstoutsummaryComponent },
  { path: 'AccTrnGstFillingSummary/:lsmonth/:lsyear', component: AccTrnGstfillingsummaryComponent },
  { path: 'AccRptDebtorReportSummary', component: AccRptDebtorreportComponent },
  { path: 'AccRptDebtorReportView/:account_gid', component: AccRptDebtorreportviewComponent },
  { path: 'AccRptCreditorReport', component: AccRptCreditorreportComponent },
  { path: 'AccRptCreditorReportView/:account_gid', component: AccRptCreditorreportviewComponent },
  { path: 'AccRptLedgerReport', component: AccRptLedgerreportComponent },
  { path: 'AccMstChartofAccountChild/:account_gid/:account_code/:account_name', component: AccMstChartofaccountchildviewComponent },
  { path: 'AccMstChartofAccountIncome', component: AccMstChartofaccountincomeComponent },
  { path: 'AccMstChartofAccountLiability', component: AccMstChartofaccountliabilityComponent },
  { path: 'AccMstChartofAccountAsset', component: AccMstChartofaccountassetComponent },
  { path: 'AccMstChartofAccountIncomeView/:account_gid/:account_code/:account_name', component: AccMstChartofaccountincomeviewComponent },
  { path: 'AccMstChartofAccountLiabilityView/:account_gid/:account_code/:account_name', component: AccMstChartofaccountliabilityviewComponent },
  { path: 'AccMstChartofAccountAssetView/:account_gid/:account_code/:account_name', component: AccMstChartofaccountassetviewComponent },
  { path: 'AccTrnJournalEntryEdit/:journal_gid', component: AccTrnJournalentryEditComponent },
  { path: 'AccRptOpenBalanceReport', component: AccRptOpenbalancereportComponent },
  { path: 'AccRptProfitandLost', component: AccRptProfileandlostreportComponent },
  { path: 'AccRptProfitandLost/:lspage', component: AccRptProfileandlostreportComponent },
  { path: 'AccRptProfitandLostDetails/:account_gid/:lspage', component: AccRptProfileandlossdetailsComponent },
  { path: 'AccRptTrailBalance', component: AccRptTrialbalanceComponent },
  { path: 'AccRptBalanceSheeet', component: AccRptBalancesheeetreportComponent },
  { path: 'AccTrnRecordExpense', component: AccTrnRecordExpenseComponent },
  { path: 'AccTrnExpenseAdd', component: AccTrnExpenseAddComponent },
  { path: 'AccTrnMakePayment/:encryptedParam', component: AccTrnMakePaymentComponent },
  { path: 'Accmstchartofaccounts', component: AccMstChartofaccountsComponent },
  { path: 'AccRptVendor360/:vendor_gid', component: AccRptVendor360Component },
  { path: 'AccRptCreditordetailedreport/:account_gid', component: AccRptCreditordetailedreportComponent },
  { path: 'AccRptDetailedReport/:accountgid/:customergid', component: AccRptDebtorDetailedreportComponent },
  { path: 'AccTrnFinanceDashboard', component: AccTrnFinancedashboardComponent },
  { path: 'AccRptJournalEntryBook', component: AccRptJournalentryBookComponent },
  { path: 'SmrTrnSalesLegderFin', component: SmrTrnSaleslegderComponent },
  { path: 'PmrTrnPurchaseLegderFin', component: PmrTrnPurchaselegderComponent },
  { path: 'AccRptBalancesheetstatement', component: AccRptBalancesheetstatementComponent },
  { path: 'AccRptIncomeandexpensestatement', component: AccRptIncomeandexpensestatementComponent },
  { path: 'AccRptBalancesheetreportview/:account_gid/:finyear/:branch', component: AccRptBalancesheetreportviewComponent },
  { path: 'AccRptLedgerassetreport/:accountgid/:customergid', component: AccRptLedgerassetreportComponent },
  { path: 'AccRptLedgerliabityreport/:account_gid', component: AccRptLedgerliabilityreportComponent },
  { path: 'AccTrnSundryexpenseSummary', component: AccTrnSundryexpenseComponent },
  { path: 'AccTrnAddsundryexpense', component: AccTrnAddsundryexpenseComponent },
  { path: 'AccTrnEditsundryexpenses/:expense_gid', component: AccTrnEditsundryexpensesComponent },
  { path: 'AccTrnViewsundryexpenses/:expense_gid', component: AccTrnViewsundryexpensesComponent },
  { path: 'AccRptIncome&Expense', component: AccRptIncomeExpenseComponent },
  { path: 'AccTrnPaymentSummary', component: AccTrnPaymentSummaryComponent },
  { path: 'AccTrnAddPayment', component: AccTrnAddPaymentComponent },
  { path: 'AccTrnPaymentaddconfirm/:expense_gid/:vendor_gid', component: AccTrnPaymentaddconfirmComponent },
  { path: 'AccTrnMultipleExpense2singlepayment/:vendor_gid', component: AccTrnMultipleExpenses2singlepaymentComponent },
  { path: 'AccTrnPaymentview/:paymentgid', component: AccTrnSundrypaymentViewComponent },
  { path: 'AccTrnFinanceregulation', component: AccTrnFinanceregulationComponent },
  { path: 'AccRptLedgerincomereport/:account_gid', component: AccRptLedgerincomereportComponent},
  { path: 'AccRptLedgerexpensereport/:account_gid', component: AccRptLedgerexpensereportComponent},
  { path: 'AccRptPurchaseledgerview/:invoice_gid', component: AccRptPurchaseledgerviewComponent}
  
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class EmsFinanceRoutingModule { }
