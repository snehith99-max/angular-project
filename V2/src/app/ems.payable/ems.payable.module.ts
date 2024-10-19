import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgSelectModule } from '@ng-select/ng-select';
import { PmrTrnInvoiceAccountingaddconfirmComponent } from './Component/pmr-trn-invoice-accountingaddconfirm/pmr-trn-invoice-accountingaddconfirm.component';
import { PmrTrnInvoiceAddselectComponent } from './Component/pmr-trn-invoice-addselect/pmr-trn-invoice-addselect.component';
import { PmrTrnInvoiceSummaryComponent } from './Component/pmr-trn-invoice-summary/pmr-trn-invoice-summary.component';
import { EmsPayableRoutingModule } from './ems.payable-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PmrTrnOpeninginvoiceComponent } from './Component/pmr-trn-openinginvoice/pmr-trn-openinginvoice.component';
import { PmrTrnOpeninginvoiceSummaryComponent } from './Component/pmr-trn-openinginvoice-summary/pmr-trn-openinginvoice-summary.component';
import { PmrTrnInvoiceviewComponent } from './Component/pmr-trn-invoiceview/pmr-trn-invoiceview.component';
import { PblTrnPaymentsummaryComponent } from './Component/pbl-trn-paymentsummary/pbl-trn-paymentsummary.component';
import { PblTrnPaymentaddproceedComponent } from './Component/pbl-trn-paymentaddproceed/pbl-trn-paymentaddproceed.component';
import { PblTrnMultipleinvoice2singlepaymentComponent } from './Component/pbl-trn-multipleinvoice2singlepayment/pbl-trn-multipleinvoice2singlepayment.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { PblTrnPaymentcancelComponent } from './Component/pbl-trn-paymentcancel/pbl-trn-paymentcancel.component';
import { PblTrnPaymentviewComponent } from './Component/pbl-trn-paymentview/pbl-trn-paymentview.component';
import { PblTrnInvoiceaddselectgrndetailsComponent } from './Component/pbl-trn-invoiceaddselectgrndetails/pbl-trn-invoiceaddselectgrndetails.component';
import { PblTrnDirectinvoiceAddComponent } from './Component/pbl-trn-directinvoice-add/pbl-trn-directinvoice-add.component';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { PblTrnDirectinvoiceEditComponent } from './Component/pbl-trn-directinvoice-edit/pbl-trn-directinvoice-edit.component';
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


@NgModule({
  declarations: [
    PmrTrnInvoiceSummaryComponent,
    PblTrnPaymentviewComponent,
    PmrTrnInvoiceAddselectComponent,
    PmrTrnInvoiceAccountingaddconfirmComponent,
    PmrTrnOpeninginvoiceComponent,
    PmrTrnOpeninginvoiceSummaryComponent,
    PmrTrnInvoiceviewComponent,
    PblTrnPaymentsummaryComponent,
    PblTrnPaymentaddproceedComponent,
    PblTrnMultipleinvoice2singlepaymentComponent,
    PblTrnPaymentcancelComponent,
    PblTrnInvoiceaddselectgrndetailsComponent,
    PblTrnDirectinvoiceAddComponent,
    PblTrnDirectinvoiceEditComponent,
    PblTrnInvoicepricechangeComponent,PblTrnInvoiceApprovalAddComponent
    ,PblTrnInvoiceApprovalComponent, PblTrnDebitnoteSummaryComponent,
     PblTrnRaisedebitnoteSelectComponent,
     PblTrnRaisedebitnoteAddComponent,
     PblTrnStockreturnDebitnoteComponent,
     PblTrnDebitnoteViewComponent,
     PblTrnPaymentaddconfirmComponent,
     PmrTrnLedgerInvoiceviewComponent,
     PblRptAgeingReportComponent,
     PblTrnServiceInvoiceAddComponent
    

  ],
  imports: [
    CommonModule,
    EmsPayableRoutingModule,
    FormsModule, ReactiveFormsModule,
    NgSelectModule, AngularEditorModule, NgbModule
  ]
})
export class EmsPayableModule { }
