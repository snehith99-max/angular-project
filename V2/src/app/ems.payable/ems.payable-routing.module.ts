import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PmrTrnInvoiceSummaryComponent } from './Component/pmr-trn-invoice-summary/pmr-trn-invoice-summary.component';
import { PmrTrnInvoiceAccountingaddconfirmComponent } from './Component/pmr-trn-invoice-accountingaddconfirm/pmr-trn-invoice-accountingaddconfirm.component';
import { PmrTrnInvoiceAddselectComponent } from './Component/pmr-trn-invoice-addselect/pmr-trn-invoice-addselect.component';
import { PmrTrnOpeninginvoiceComponent } from './Component/pmr-trn-openinginvoice/pmr-trn-openinginvoice.component';
import { PmrTrnOpeninginvoiceSummaryComponent } from './Component/pmr-trn-openinginvoice-summary/pmr-trn-openinginvoice-summary.component';
import { PmrTrnInvoiceviewComponent } from './Component/pmr-trn-invoiceview/pmr-trn-invoiceview.component';
import { PblTrnPaymentsummaryComponent } from './Component/pbl-trn-paymentsummary/pbl-trn-paymentsummary.component';
import { PblTrnPaymentaddproceedComponent } from './Component/pbl-trn-paymentaddproceed/pbl-trn-paymentaddproceed.component';
import { PblTrnMultipleinvoice2singlepaymentComponent } from './Component/pbl-trn-multipleinvoice2singlepayment/pbl-trn-multipleinvoice2singlepayment.component';
import { PblTrnDirectinvoiceEditComponent } from './Component/pbl-trn-directinvoice-edit/pbl-trn-directinvoice-edit.component';
import { PblTrnPaymentcancelComponent } from './Component/pbl-trn-paymentcancel/pbl-trn-paymentcancel.component';
import { PblTrnPaymentviewComponent } from './Component/pbl-trn-paymentview/pbl-trn-paymentview.component';
import { PblTrnInvoiceaddselectgrndetailsComponent } from './Component/pbl-trn-invoiceaddselectgrndetails/pbl-trn-invoiceaddselectgrndetails.component';
import { PblTrnDirectinvoiceAddComponent } from './Component/pbl-trn-directinvoice-add/pbl-trn-directinvoice-add.component';
import { PblTrnInvoicepricechangeComponent } from './Component/pbl-trn-invoicepricechange/pbl-trn-invoicepricechange.component';
import { PblTrnInvoiceApprovalComponent } from './Component/pbl-trn-invoice-approval/pbl-trn-invoice-approval.component';
import { PblTrnInvoiceApprovalAddComponent } from './Component/pbl-trn-invoice-approval-add/pbl-trn-invoice-approval-add.component';
import { PblTrnDebitnoteSummaryComponent } from './Component/pbl-trn-debitnote-summary/pbl-trn-debitnote-summary.component';
import { PblTrnRaisedebitnoteSelectComponent } from './Component/pbl-trn-raisedebitnote-select/pbl-trn-raisedebitnote-select.component';
import { PblTrnRaisedebitnoteAddComponent } from './Component/pbl-trn-raisedebitnote-add/pbl-trn-raisedebitnote-add.component';
import { PblTrnStockreturnDebitnoteComponent } from './Component/pbl-trn-stockreturn-debitnote/pbl-trn-stockreturn-debitnote.component';
import { PblTrnDebitnoteViewComponent } from './Component/pbl-trn-debitnote-view/pbl-trn-debitnote-view.component';
import { PblTrnPaymentaddconfirmComponent } from './Component/pbl-trn-paymentaddconfirm/pbl-trn-paymentaddconfirm.component';
import { PmrTrnLedgerInvoiceviewComponent } from './Component/pmr-trn-ledger-invoiceview/pmr-trn-ledger-invoiceview.component';
import { PblRptAgeingReportComponent } from './Component/pbl-rpt-ageing-report/pbl-rpt-ageing-report.component';
import { PblTrnServiceInvoiceAddComponent } from './Component/pbl-trn-service-invoice-add/pbl-trn-service-invoice-add.component';

const routes: Routes = [
  { path: 'PmrTrnInvoice', component: PmrTrnInvoiceSummaryComponent },
  { path: 'PmrTrnInvoiceAccountingaddconfirm/:vendor_gid', component: PmrTrnInvoiceAccountingaddconfirmComponent },
  { path: 'PmrTrnInvoiceAddselect', component: PmrTrnInvoiceAddselectComponent },
  { path: 'PmrTrnOpeningInvoice', component: PmrTrnOpeninginvoiceComponent },
  { path: 'PmrTrnOpeninginvoiceSummary', component: PmrTrnOpeninginvoiceSummaryComponent },
  { path: 'PmrTrnInvoiceview/:invoice_gid', component: PmrTrnInvoiceviewComponent },
  { path: 'PblTrnPaymentsummary', component: PblTrnPaymentsummaryComponent },
  { path: 'PblTrnPaymentAddProceed', component: PblTrnPaymentaddproceedComponent },
  { path: 'PmrTrnInvoiceEdit/:invoice_gid', component: PblTrnDirectinvoiceEditComponent },
  { path: 'PblTrnMultipleinvoice2singlepayment/:vendor_gid', component: PblTrnMultipleinvoice2singlepaymentComponent },
  { path: 'PblTrnDirectInvoiceAdd', component: PblTrnDirectinvoiceAddComponent },
  { path: 'PblTrnDirectInvoice', component: PblTrnDirectinvoiceAddComponent },
  { path: 'PblTrnPaymentCancel/:payment_gid', component: PblTrnPaymentcancelComponent },
  { path: 'PblTrnPaymentview/:payment_gid', component: PblTrnPaymentviewComponent },
  { path: 'PblTrnInvoiceaddselectgrndetailsComponent/:purchaseorder_gid1/:vendor_gid2/:grn_gid3', component: PblTrnInvoiceaddselectgrndetailsComponent },
  { path: 'PblTrnInvoicepricechange/:purchaseorder_gid', component: PblTrnInvoicepricechangeComponent },
  { path: 'PblTrnInvoiceApproval', component: PblTrnInvoiceApprovalComponent },
  { path: 'PblTrnInvoiceApprovalAdd/:purchaseorder_gid', component: PblTrnInvoiceApprovalAddComponent },
  { path: 'PblTrnDebitNote', component: PblTrnDebitnoteSummaryComponent },
  { path: 'PblTrnRaiseDebitNote', component: PblTrnRaisedebitnoteSelectComponent },
  { path: 'PblTrnRaiseDebitNoteSelect/:invoicegid', component: PblTrnRaisedebitnoteAddComponent },
  { path: 'PblTrnStockReturn/:invoicegid/:debitnotegid', component: PblTrnStockreturnDebitnoteComponent },
  { path: 'PblTrnDebitNoteView/:invoicegid', component: PblTrnDebitnoteViewComponent },
  { path: 'PblTrnPaymentaddconfirm/:invoice_gid/:vendor_gid', component: PblTrnPaymentaddconfirmComponent },
  { path: 'PmrTrnLedgerInvoiceView/:invoice_gid/:lspage', component: PmrTrnLedgerInvoiceviewComponent },
  { path: 'PblRptAgeingReport', component: PblRptAgeingReportComponent },
  { path: 'PblTrnServiceInvoiceAdd/:purchaseorder_gid', component: PblTrnServiceInvoiceAddComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmsPayableRoutingModule { }
