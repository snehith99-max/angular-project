import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { EmsFinanceRoutingModule } from './ems.finance-routing.module';
import { AccTrnCashbooksummaryComponent } from './Component/acc-trn-cashbooksummary/acc-trn-cashbooksummary.component';
import { AccTrnBankbooksummaryComponent } from './Component/acc-trn-bankbooksummary/acc-trn-bankbooksummary.component';
import { AccTrnBankbookComponent } from './Component/acc-trn-bankbook/acc-trn-bankbook.component';
import { AccMstBankmasterComponent } from './Component/acc-mst-bankmaster/acc-mst-bankmaster.component';
import { AccMstBankmasterAddComponent } from './Component/acc-mst-bankmaster-add/acc-mst-bankmaster-add.component';
import { AccTrnBankbookaddentryComponent } from './Component/acc-trn-bankbookaddentry/acc-trn-bankbookaddentry.component';
import { AccTrnCashbookSelectComponent } from './Component/acc-trn-cashbook-select/acc-trn-cashbook-select.component';
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
import { AccTrnJournalentryAddComponent } from './Component/acc-trn-journalentry-add/acc-trn-journalentry-add.component';
import { AccTrnTaxmanagementviewComponent } from './Component/acc-trn-taxmanagementview/acc-trn-taxmanagementview.component';
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
import { AccMstChartofaccountassetviewComponent } from './Component/acc-mst-chartofaccountassetview/acc-mst-chartofaccountassetview.component';
import { AccMstChartofaccountliabilityviewComponent } from './Component/acc-mst-chartofaccountliabilityview/acc-mst-chartofaccountliabilityview.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AccTrnJournalentryEditComponent } from './Component/acc-trn-journalentry-edit/acc-trn-journalentry-edit.component';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgApexchartsModule } from 'ng-apexcharts';
import { AccRptOpenbalancereportComponent } from './Component/acc-rpt-openbalancereport/acc-rpt-openbalancereport.component';
import { AccRptProfileandlostreportComponent } from './Component/acc-rpt-profileandlostreport/acc-rpt-profileandlostreport.component';
import { AccRptTrialbalanceComponent } from './Component/acc-rpt-trialbalance/acc-rpt-trialbalance.component';
import { AccRptProfileandlossdetailsComponent } from './Component/acc-rpt-profileandlossdetails/acc-rpt-profileandlossdetails.component';
import { AccRptBalancesheeetreportComponent } from './Component/acc-rpt-balancesheeetreport/acc-rpt-balancesheeetreport.component';
import { AccTrnRecordExpenseComponent } from './Component/acc-trn-record-expense/acc-trn-record-expense.component';
import { AccTrnExpenseAddComponent } from './Component/acc-trn-expense-add/acc-trn-expense-add.component';
import { AccTrnMakePaymentComponent } from './Component/acc-trn-make-payment/acc-trn-make-payment.component';
import { AccTrnFinancedashboardComponent } from './Component/acc-trn-financedashboard/acc-trn-financedashboard.component';
import { AccMstChartofaccountsComponent } from './Component/acc-mst-chartofaccounts/acc-mst-chartofaccounts.component';
import { AccRptVendor360Component } from './Component/acc-rpt-vendor360/acc-rpt-vendor360.component';
import { AccRptCreditordetailedreportComponent } from './Component/acc-rpt-creditordetailedreport/acc-rpt-creditordetailedreport.component';
import { AccRptDebtorDetailedreportComponent } from './Component/acc-rpt-debtor-detailedreport/acc-rpt-debtor-detailedreport.component';
import { AccRptJournalentryBookComponent } from './Component/acc-rpt-journalentry-book/acc-rpt-journalentry-book.component';
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
import { SmrTrnSaleslegderComponent } from './Component/smr-trn-saleslegder/smr-trn-saleslegder.component';
import { PmrTrnPurchaselegderComponent } from './Component/pmr-trn-purchaselegder/pmr-trn-purchaselegder.component';
import { AccTrnSundrypaymentViewComponent } from './Component/acc-trn-sundrypayment-view/acc-trn-sundrypayment-view.component';
import { AccTrnFinanceregulationComponent } from './Component/acc-trn-financeregulation/acc-trn-financeregulation.component';
import { AccRptLedgerincomereportComponent } from './Component/acc-rpt-ledgerincomereport/acc-rpt-ledgerincomereport.component';
import { AccRptLedgerexpensereportComponent } from './Component/acc-rpt-ledgerexpensereport/acc-rpt-ledgerexpensereport.component';
import { AccRptPurchaseledgerviewComponent } from './Component/acc-rpt-purchaseledgerview/acc-rpt-purchaseledgerview.component';

@NgModule({
  declarations: [
    AccTrnCashbooksummaryComponent,
    AccTrnBankbooksummaryComponent,
    AccTrnBankbookComponent, AccMstBankmasterComponent, AccMstBankmasterAddComponent, AccTrnBankbookaddentryComponent,
    AccTrnCashbookSelectComponent, AccTrnCashbookeditComponent,
    AccMstBankmasterEditComponent,
    AccMstOpeningbalanceComponent,
    AccMstCreditcardmasterSummaryComponent,
    AccMstGlcodeSummaryComponent,
    AccTrnCreditcardbookSummaryComponent,
    AccTrnJournalentrySummaryComponent,
    AccTrnSundrysalesSummaryComponent,
    AccTrnGstmanagementSummaryComponent,
    AccRptIncomeandEpenditureReportComponent,
    AccTrnReceivableinvoiceSummaryComponent,
    AccTrnBankbookentryaddComponent,
    AccTrnCashbookentryaddComponent,
    AccTrnTaxmanagementComponent,
    AccTrnTaxfillingComponent,
    AccTrnTaxmanagementviewComponent,
    AccTrnJournalentryAddComponent,
    AccTrnFundtransfersummaryComponent,
    AccTrnFundtransferapprovalComponent,
    AccTrnGlcodedebitoraddComponent,
    AccTrnGlcodecreditoraddComponent,
    AccTrnGstinsummaryComponent,
    AccTrnGstoutsummaryComponent,
    AccTrnGstfillingsummaryComponent,
    AccRptDebtorreportComponent,
    AccRptDebtorreportviewComponent,
    AccRptCreditorreportComponent,
    AccRptCreditorreportviewComponent,
    AccRptLedgerreportComponent,
    AccMstChartofaccountchildviewComponent,
    AccMstChartofaccountincomeComponent,
    AccMstChartofaccountliabilityComponent,
    AccMstChartofaccountassetComponent,
    AccMstChartofaccountincomeviewComponent,
    AccMstChartofaccountassetviewComponent,
    AccMstChartofaccountliabilityviewComponent,
    AccTrnJournalentryEditComponent,
    AccRptOpenbalancereportComponent,
    AccRptProfileandlostreportComponent,
    AccRptTrialbalanceComponent,
    AccRptProfileandlostreportComponent,
    AccRptProfileandlossdetailsComponent,
    AccRptBalancesheeetreportComponent,
    AccTrnRecordExpenseComponent,
    AccTrnExpenseAddComponent,
    AccTrnMakePaymentComponent,
    AccTrnFinancedashboardComponent,
    AccMstChartofaccountsComponent,
    AccRptCreditordetailedreportComponent,
    AccRptVendor360Component,
    AccRptDebtorDetailedreportComponent,
    AccRptJournalentryBookComponent,
    AccRptBalancesheetstatementComponent,
    AccRptIncomeandexpensestatementComponent,
    AccRptBalancesheetreportviewComponent,
    AccRptLedgerassetreportComponent,
AccRptIncomeExpenseComponent,
AccRptLedgerliabilityreportComponent,
    AccTrnViewsundryexpensesComponent,
    AccTrnEditsundryexpensesComponent,
    AccTrnAddsundryexpenseComponent,
    AccTrnSundryexpenseComponent,
    AccTrnPaymentSummaryComponent,
    AccTrnAddPaymentComponent,
    AccTrnPaymentaddconfirmComponent,
    AccTrnMultipleExpenses2singlepaymentComponent,
    SmrTrnSaleslegderComponent,
    AccTrnSundrypaymentViewComponent,
    PmrTrnPurchaselegderComponent,
    AccTrnFinanceregulationComponent,
    AccRptLedgerincomereportComponent,
    AccRptLedgerexpensereportComponent,
    AccRptPurchaseledgerviewComponent,
  ],
  imports: [
    CommonModule,
    EmsFinanceRoutingModule, NgSelectModule,
    FormsModule, ReactiveFormsModule,
    NgbModule,
    PaginationModule.forRoot(),
    NgApexchartsModule,
  ],
})
export class EmsFinanceModule { }
