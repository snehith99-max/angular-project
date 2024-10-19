import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule} from '@angular/forms'
import { EmsEinvoiceRoutingModule } from './ems.einvoice-routing.module';
import { CrmMstProductComponent } from './componet/crm-mst-product/crm-mst-product.component';
import { CrmMstProductAddComponent } from './componet/crm-mst-product-add/crm-mst-product-add.component';
import { CrmMstProductEditComponent } from './componet/crm-mst-product-edit/crm-mst-product-edit.component';
import { DashboardComponent } from './componet/dashboard/dashboard.component';
import { NgApexchartsModule } from 'ng-apexcharts';
import { BodyComponent } from './componet/body/body.component';
import { SidenavComponent } from './componet/sidenav/sidenav.component';
import { CrmMstCustomerAddComponent } from './componet/crm-mst-customeradd/crm-mst-customeradd.component';
import { CrmMstCustomerSummaryComponent } from './componet/crm-mst-customersummary/crm-mst-customersummary.component';
import { RblTrnTinvoiceComponent } from './componet/rbl-trn-tinvoice/rbl-trn-tinvoice.component';
import { DataTablesModule } from 'angular-datatables';
import { RblTrninvoiceaddComponent } from './componet/rbl-trn-invoiceadd/rbl-trn-invoiceadd.component';
import { CrmMstProductViewComponent } from './componet/crm-mst-product-view/crm-mst-product-view.component';
import { CrmMstCustomereditComponent } from './componet/crm-mst-customeredit/crm-mst-customeredit.component';
import { CrmMstCustomerviewComponent } from './componet/crm-mst-customerview/crm-mst-customerview.component';
import { SysMstBranchComponent } from './componet/sys-mst-branch/sys-mst-branch.component';
import { RblTrnEinvoiceComponent } from './componet/rbl-trn-einvoice/rbl-trn-einvoice.component';
import { RblTrnInvoiceeditComponent } from './componet/rbl-trn-invoiceedit/rbl-trn-invoiceedit.component';
import { EinvoicedashboardComponent } from './componet/einvoicedashboard/einvoicedashboard.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { RblTrnProformaInvoiceComponent } from './componet/rbl-trn-proforma-invoice/rbl-trn-proforma-invoice.component';
import { RblTrnProformaInvoiceAddComponent } from './componet/rbl-trn-proforma-invoice-add/rbl-trn-proforma-invoice-add.component';
import { RblTrnProformaInvoiceConfirmComponent } from './componet/rbl-trn-proforma-invoice-confirm/rbl-trn-proforma-invoice-confirm.component';
import { RblMstPaymentsummaryComponent } from './componet/rbl-mst-paymentsummary/rbl-mst-paymentsummary.component';
import { RblMstReceiptaddComponent } from './componet/rbl-mst-receiptadd/rbl-mst-receiptadd.component';
import { RblMstMakereceiptComponent } from './componet/rbl-mst-makereceipt/rbl-mst-makereceipt.component';
import { RblTrnProformaInvoiceAdvanceReceiptComponent } from './componet/rbl-trn-proforma-invoice-advance-receipt/rbl-trn-proforma-invoice-advance-receipt.component';
import { RblTrnProformainvoicemailComponent } from './componet/rbl-trn-proformainvoicemail/rbl-trn-proformainvoicemail.component';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { RblTrnInvoiceviewComponent } from './componet/rbl-trn-invoiceview/rbl-trn-invoiceview.component';
import { RblTrnSalesinvoicesummaryComponent } from './componet/rbl-trn-salesinvoicesummary/rbl-trn-salesinvoicesummary.component';
import { RblTrnInvoiceaccountingaddconfirmComponent } from './componet/rbl-trn-invoiceaccountingaddconfirm/rbl-trn-invoiceaccountingaddconfirm.component';
import { RblTrnInvoiceaddBobaComponent } from './componet/rbl-trn-invoiceadd-boba/rbl-trn-invoiceadd-boba.component';
import { RblTrnInvoiceeditBobaComponent } from './componet/rbl-trn-invoiceedit-boba/rbl-trn-invoiceedit-boba.component';
import { RblTrnInvoicesummaryBobaComponent } from './componet/rbl-trn-invoicesummary-boba/rbl-trn-invoicesummary-boba.component';
import { RblTrnInvoiceviewBobaComponent } from './componet/rbl-trn-invoiceview-boba/rbl-trn-invoiceview-boba.component';
import { RblTrnBobateainvoiceaddComponent } from './componet/rbl-trn-bobateainvoiceadd/rbl-trn-bobateainvoiceadd.component';
import { RblTrnEwaysummaryComponent } from './componet/rbl-trn-ewaysummary/rbl-trn-ewaysummary.component';
import { RblTrnEwayinvoicesummaryComponent } from './componet/rbl-trn-ewayinvoicesummary/rbl-trn-ewayinvoicesummary.component';
import { RblTrnEwayAddComponent } from './componet/rbl-trn-eway-add/rbl-trn-eway-add.component';
import { RblTrnProformaInvoiceEditComponent } from './componet/rbl-trn-proforma-invoice-edit/rbl-trn-proforma-invoice-edit.component';
import { RblTrnProformainvoiceViewComponent } from './componet/rbl-trn-proformainvoice-view/rbl-trn-proformainvoice-view.component';
import { RblTrnProformainvoiceconfirmnewComponent } from './componet/rbl-trn-proformainvoiceconfirmnew/rbl-trn-proformainvoiceconfirmnew.component';

@NgModule({
  declarations: [
    CrmMstProductComponent,
    RblTrnSalesinvoicesummaryComponent,
    EinvoicedashboardComponent,
    CrmMstProductAddComponent,
    CrmMstProductEditComponent,
    DashboardComponent,
    RblTrnEinvoiceComponent,
    BodyComponent,
    SidenavComponent,
    CrmMstCustomerAddComponent,
    CrmMstCustomerSummaryComponent,
    RblTrnTinvoiceComponent,
    RblTrninvoiceaddComponent,
    CrmMstProductViewComponent,
    CrmMstCustomereditComponent,
    SysMstBranchComponent,
    CrmMstCustomerviewComponent,
    RblTrnInvoiceeditComponent,
    RblTrnProformaInvoiceComponent,
    RblTrnProformaInvoiceAddComponent,
    RblTrnProformaInvoiceConfirmComponent,
    RblMstPaymentsummaryComponent,
    RblMstReceiptaddComponent,
    RblMstMakereceiptComponent,
    RblTrnEinvoiceComponent,
    RblTrnProformaInvoiceAdvanceReceiptComponent,
    RblTrnProformainvoicemailComponent,
    RblTrnInvoiceaccountingaddconfirmComponent,
    RblTrnInvoiceviewComponent,
    RblTrnInvoiceaddBobaComponent,
    RblTrnInvoiceeditBobaComponent,
    RblTrnInvoicesummaryBobaComponent,
    RblTrnInvoiceviewBobaComponent,
    RblTrnBobateainvoiceaddComponent,
    RblTrnEwaysummaryComponent,
    RblTrnEwayinvoicesummaryComponent,
    RblTrnEwayAddComponent,RblTrnProformaInvoiceEditComponent, RblTrnProformainvoiceViewComponent, RblTrnProformainvoiceconfirmnewComponent
  ],

  imports: [
    CommonModule,
    EmsEinvoiceRoutingModule,
    NgApexchartsModule,
    FormsModule,
    DataTablesModule,
    NgSelectModule,
    ReactiveFormsModule,
    AngularEditorModule,
  ]
})

export class EmsEinvoiceModule { }
