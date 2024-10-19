import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DataTablesModule } from 'angular-datatables';
import { EmsPmrRoutingModule } from './ems.pmr-routing.module';
import { NgApexchartsModule } from 'ng-apexcharts';
import { PmrTrnVendorregisterSummaryComponent } from './Component/pmr-trn-vendorregister-summary/pmr-trn-vendorregister-summary.component';
import { PmrMstProductunitComponent } from './Component/pmr-mst-productunit/pmr-mst-productunit.component';
import { PmrMstCurrencySummaryComponent } from './Component/pmr-mst-currency-summary/pmr-mst-currency-summary.component';
import { PmrMstProductgroupComponent } from './Component/pmr-mst-productgroup/pmr-mst-productgroup.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PmrTrnVendorregisterAddComponent } from './Component/pmr-trn-vendorregister-add/pmr-trn-vendorregister-add.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { PmrMstProductAddComponent } from './Component/pmr-mst-product-add/pmr-mst-product-add.component';
import { PmrMstProductSummaryComponent } from './Component/pmr-mst-product-summary/pmr-mst-product-summary.component';
import { PmrMstTaxSummaryComponent } from './Component/pmr-mst-tax-summary/pmr-mst-tax-summary.component';
import { EmsUtilitiesModule } from '../ems.utilities/ems.utilities.module';
import { PmrTrnGrninwardAddComponent } from './Component/pmr-trn-grninward-add/pmr-trn-grninward-add.component';
import { PmrTrnGrninwardaddComponent } from './Component/pmr-trn-grninwardadd/pmr-trn-grninwardadd.component';
import { PmrTrnGrninwardComponent } from './Component/pmr-trn-grninward/pmr-trn-grninward.component';
import { PmrTrnPurchaseorderSummaryComponent } from './Component/pmr-trn-purchaseorder-summary/pmr-trn-purchaseorder-summary.component';
import { PmrMstTermsconditionssummaryComponent } from './Component/pmr-mst-termsconditionssummary/pmr-mst-termsconditionssummary.component';
import { PmrMstTermsconditionsaddComponent } from './Component/pmr-mst-termsconditionsadd/pmr-mst-termsconditionsadd.component';
import { PmrTrnVendorregisterEditComponent } from './Component/pmr-trn-vendorregister-edit/pmr-trn-vendorregister-edit.component';
import { PmrTrnVendorregisterViewComponent } from './Component/pmr-trn-vendorregister-view/pmr-trn-vendorregister-view.component';
import { PmrTrnPurchaseorderViewComponent } from './Component/pmr-trn-purchaseorder-view/pmr-trn-purchaseorder-view.component';
import { PmrRptPurchaseorderdetailedreportComponent } from './Component/pmr-rpt-purchaseorderdetailedreport/pmr-rpt-purchaseorderdetailedreport.component';
import { PmrMstProductViewComponent } from './Component/pmr-mst-product-view/pmr-mst-product-view.component';
import { PmrMstProductEditComponent } from './Component/pmr-mst-product-edit/pmr-mst-product-edit.component';
import { PmrMstVendorAdditionalinformationComponent } from './Component/pmr-mst-vendor-additionalinformation/pmr-mst-vendor-additionalinformation.component';
import { PmrMstVendorregisterdocumentComponent } from './Component/pmr-mst-vendorregisterdocument/pmr-mst-vendorregisterdocument.component';
import { PmrMstVendorregisterimportexcelComponent } from './Component/pmr-mst-vendorregisterimportexcel/pmr-mst-vendorregisterimportexcel.component';
import { PmrTrnGrninwardViewComponent } from './Component/pmr-trn-grninward-view/pmr-trn-grninward-view.component';
import { PmrTrnGrncheckerComponent } from './Component/pmr-trn-grnchecker/pmr-trn-grnchecker.component';
import { PmrTrnGrnqccheckerComponent } from './Component/pmr-trn-grnqcchecker/pmr-trn-grnqcchecker.component';
import { PmrTrnRaiseEnquiryComponent } from './Component/pmr-trn-raise-enquiry/pmr-trn-raise-enquiry.component';
import { PmrTrnRaiseEnquiryaddComponent } from './Component/pmr-trn-raise-enquiryadd/pmr-trn-raise-enquiryadd.component';
import { PmrTrnVendorenquiryViewComponent } from './Component/pmr-trn-vendorenquiry-view/pmr-trn-vendorenquiry-view.component';
import { PmrTrnPurchaseordermailComponent } from './Component/pmr-trn-purchaseordermail/pmr-trn-purchaseordermail.component';
import { PmrTrnPurchaseRequisitionComponent } from './Component/pmr-trn-purchase-requisition/pmr-trn-purchase-requisition.component';
import { PmrTrnRaiseRequisitionComponent } from './Component/pmr-trn-raise-requisition/pmr-trn-raise-requisition.component';
import { PmrTrnPurchaseQuotationComponent } from './Component/pmr-trn-purchase-quotation/pmr-trn-purchase-quotation.component';
import { PmrTrnPurchasequotaionSummaryComponent } from './Component/pmr-trn-purchasequotaion-summary/pmr-trn-purchasequotaion-summary.component';
import { PmrDashboardComponent } from './Component/pmr-dashboard/pmr-dashboard.component';
import { NgxIntlTelInputModule } from 'ngx-intl-tel-input';
import { PmrTrnEnquiryAddSelectComponent } from './Component/pmr-trn-enquiry-add-select/pmr-trn-enquiry-add-select.component';
import { PmrTrnRequestForQuoteSummaryComponent } from './Component/pmr-trn-request-for-quote-summary/pmr-trn-request-for-quote-summary.component';
import { PmrTrnEnquiryaddProceedComponent } from './Component/pmr-trn-enquiryadd-proceed/pmr-trn-enquiryadd-proceed.component';
import { PmrRptVendorledgerReportComponent } from './Component/pmr-rpt-vendorledger-report/pmr-rpt-vendorledger-report.component';
import { PmrRptVendorledgerreportComponent } from './Component/pmr-rpt-vendorledgerreport/pmr-rpt-vendorledgerreport.component';
import { PmrRptOverallreportComponent } from './Component/pmr-rpt-overallreport/pmr-rpt-overallreport.component';
import { PmrTrnPurchaserequisitionViewComponent } from './Component/pmr-trn-purchaserequisition-view/pmr-trn-purchaserequisition-view.component';
import { PmrTrnEnquiryAddConfirmComponent } from './Component/pmr-trn-enquiry-add-confirm/pmr-trn-enquiry-add-confirm.component';
import { PmrMstTermsandconditionEditComponent } from './Component/pmr-mst-termsandcondition-edit/pmr-mst-termsandcondition-edit.component';
import { PmrTrnRequestForQuoteViewComponent } from './Component/pmr-trn-request-for-quote-view/pmr-trn-request-for-quote-view.component';
import { PmrMstConfigurationComponent } from './Component/pmr-mst-configuration/pmr-mst-configuration.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { PmrMstMaptax2productComponent } from './Component/pmr-mst-maptax2product/pmr-mst-maptax2product.component';
import { PmrMstTaxsegmentComponent } from './Component/pmr-mst-taxsegment/pmr-mst-taxsegment.component';
import { PmrMstTaxsegmentWithstateVendorComponent } from './Component/pmr-mst-taxsegment-withstate-vendor/pmr-mst-taxsegment-withstate-vendor.component';
import { PmrMstTaxsegmentInterstateVendorComponent } from './Component/pmr-mst-taxsegment-interstate-vendor/pmr-mst-taxsegment-interstate-vendor.component';
import { PmrMstTaxsegmentOverseasVendorComponent } from './Component/pmr-mst-taxsegment-overseas-vendor/pmr-mst-taxsegment-overseas-vendor.component';
import { PmrMstTaxsegmentOthersVendorComponent } from './Component/pmr-mst-taxsegment-others-vendor/pmr-mst-taxsegment-others-vendor.component';
import { PmrMstTaxsegmentUnassignVendorComponent } from './Component/pmr-mst-taxsegment-unassign-vendor/pmr-mst-taxsegment-unassign-vendor.component';
import { PmrMstTaxsegmentTotalVendorComponent } from './Component/pmr-mst-taxsegment-total-vendor/pmr-mst-taxsegment-total-vendor.component';
import { PmrTrnRaiseEnquiryaddNewComponent } from './Component/pmr-trn-raise-enquiryadd-new/pmr-trn-raise-enquiryadd-new.component';
import { VendorAssignDualList1Component } from './Component/pmr-mst-taxsegment/vendor-assign-dual-list1/vendor-assign-dual-list1.component';
import { PmrTrnPurchaseorderAddselectComponent } from './Component/pmr-trn-purchaseorder-addselect/pmr-trn-purchaseorder-addselect.component';
import { PmrTrnPurchaseorderAddconfirmComponent } from './Component/pmr-trn-purchaseorder-addconfirm/pmr-trn-purchaseorder-addconfirm.component';
import { PmrRptPurchaserequisitionReportComponent } from './Component/pmr-rpt-purchaserequisition-report/pmr-rpt-purchaserequisition-report.component';
import { PmrTrnDirectpoAddLatestComponent } from './Component/pmr-trn-directpo-add-latest/pmr-trn-directpo-add-latest.component';
import { PmrTrnTaxsegment2assignvendorComponent } from './Component/pmr-trn-taxsegment2assignvendor/pmr-trn-taxsegment2assignvendor.component';
import { PmrTrnTaxsegment2unassignvendorComponent } from './Component/pmr-trn-taxsegment2unassignvendor/pmr-trn-taxsegment2unassignvendor.component';
import { PmrMstTaxUnMap2ProductComponent } from './Component/pmr-mst-tax-un-map2-product/pmr-mst-tax-un-map2-product.component';
import { PmrTrnPurchaseorderEditComponent } from './Component/pmr-trn-purchaseorder-edit/pmr-trn-purchaseorder-edit.component';
import { PmrTrnRatecontractComponent } from './Component/pmr-trn-ratecontract/pmr-trn-ratecontract.component';
import { PmrTrnRCproductassignComponent } from './Component/pmr-trn-rcproductassign/pmr-trn-rcproductassign.component';
import { PmrTrnRCproductremoveComponent } from './Component/pmr-trn-rcproductremove/pmr-trn-rcproductremove.component';
import { PmrTrnRCproductamendComponent } from './Component/pmr-trn-rcproductamend/pmr-trn-rcproductamend.component';
import { PmrTrnOperationpoapprovalComponent } from './Component/pmr-trn-operationpoapproval/pmr-trn-operationpoapproval.component';
import { PmrTrnFinancepoapprovalComponent } from './Component/pmr-trn-financepoapproval/pmr-trn-financepoapproval.component';
import { PmrTrnOperationpoapprovalviewComponent } from './Component/pmr-trn-operationpoapprovalview/pmr-trn-operationpoapprovalview.component';
import { PmrTrnFinancepoapprovalviewComponent } from './Component/pmr-trn-financepoapprovalview/pmr-trn-financepoapprovalview.component';
import { PmrTrnContractpoComponent } from './Component/pmr-trn-contractpo/pmr-trn-contractpo.component';
import { PmrTrnContractvendorComponent } from './Component/pmr-trn-contractvendor/pmr-trn-contractvendor.component';
import { PmrTrnCreatecontractComponent } from './Component/pmr-trn-createcontract/pmr-trn-createcontract.component';

// import { PmrTrnInvoiceaddconfirmComponent } from './Component/pmr-trn-invoiceaddconfirm/pmr-trn-invoiceaddconfirm.component'
import { PmrRptOutstandingamountreportSummaryComponent } from './Component/pmr-rpt-outstandingamountreport-summary/pmr-rpt-outstandingamountreport-summary.component';
import { PmrMstPurchasetypeComponent } from './Component/pmr-mst-purchasetype/pmr-mst-purchasetype.component';
import { PmrTrnPurchaseledgerComponent } from './Component/pmr-trn-purchaseledger/pmr-trn-purchaseledger.component';
import { PmrTrnVendor360Component } from './Component/pmr-trn-vendor360/pmr-trn-vendor360.component';
import { PmrRptPurchaseorderReportComponent } from './Component/pmr-rpt-purchaseorder-report/pmr-rpt-purchaseorder-report.component';
import { PmrRptPaymentorderReportComponent } from './Component/pmr-rpt-paymentorder-report/pmr-rpt-paymentorder-report.component';
import { PmrRptPurchaseInvoiceReportComponent } from './Component/pmr-rpt-purchase-invoice-report/pmr-rpt-purchase-invoice-report.component';
import { PmrRptVendor360Component } from './Component/pmr-rpt-vendor360/pmr-rpt-vendor360.component';
import { PmrRptVendorreportViewComponent } from './Component/pmr-rpt-vendorreport-view/pmr-rpt-vendorreport-view.component';
import { PmrTrnPurchaseagreementComponent } from './Component/pmr-trn-purchaseagreement/pmr-trn-purchaseagreement.component';
import { PmrTrnRasieAgreementorderComponent } from './Component/pmr-trn-rasie-agreementorder/pmr-trn-rasie-agreementorder.component';
import { PmrTrnPurchaseagreementviewComponent } from './Component/pmr-trn-purchaseagreementview/pmr-trn-purchaseagreementview.component';
import { PmrTrnAgreementtoInvoiceComponent } from './Component/pmr-trn-agreementto-invoice/pmr-trn-agreementto-invoice.component';
import { PmrTrnAgreementtoinvoicetagComponent } from './Component/pmr-trn-agreementtoinvoicetag/pmr-trn-agreementtoinvoicetag.component';
import { PmrTrnEditAgreementorderComponent } from './Component/pmr-trn-edit-agreementorder/pmr-trn-edit-agreementorder.component';
import { PmrRptAgreementreportComponent } from './Component/pmr-rpt-agreementreport/pmr-rpt-agreementreport.component';

@NgModule({
  declarations: [
    PmrMstTaxSummaryComponent,
    PmrMstCurrencySummaryComponent,
    PmrMstProductSummaryComponent,
    PmrMstProductAddComponent,
    PmrMstProductgroupComponent,
    PmrMstProductunitComponent,
    PmrTrnVendorregisterSummaryComponent,
    PmrTrnVendorregisterAddComponent,
    PmrTrnGrninwardAddComponent,
    PmrTrnGrninwardaddComponent,
    PmrTrnAgreementtoInvoiceComponent,
    PmrTrnGrninwardComponent,
    PmrTrnPurchaseorderSummaryComponent,
    PmrTrnPurchaseordermailComponent,
    PmrMstTermsconditionssummaryComponent,
    PmrMstTermsconditionsaddComponent,
    PmrTrnVendorregisterEditComponent,
    PmrTrnVendorregisterViewComponent,
    PmrRptPurchaseorderdetailedreportComponent,
    PmrTrnPurchaseorderViewComponent,
    PmrMstProductViewComponent,
    PmrMstProductEditComponent,
    PmrMstVendorAdditionalinformationComponent,
    PmrMstVendorregisterdocumentComponent,
    PmrMstVendorregisterimportexcelComponent,
    PmrTrnGrninwardViewComponent,
    PmrTrnGrncheckerComponent,
    PmrTrnGrnqccheckerComponent,
    PmrTrnRaiseEnquiryComponent,
    PmrTrnRaiseEnquiryaddComponent,
    PmrTrnVendorenquiryViewComponent,
    PmrTrnPurchaseRequisitionComponent,
    PmrTrnRaiseRequisitionComponent,
    PmrTrnPurchaseQuotationComponent,
    PmrTrnPurchasequotaionSummaryComponent,PmrDashboardComponent,
    PmrTrnEnquiryAddSelectComponent,
    PmrTrnRequestForQuoteSummaryComponent,
    PmrTrnEnquiryaddProceedComponent,
    PmrRptVendorledgerReportComponent,
    PmrRptVendorledgerreportComponent,
    PmrRptOverallreportComponent,
    PmrTrnPurchaserequisitionViewComponent,
    PmrTrnEnquiryAddConfirmComponent,
    PmrMstTermsandconditionEditComponent,
     PmrTrnRequestForQuoteViewComponent,
     PmrMstConfigurationComponent,
         PmrMstMaptax2productComponent, PmrMstTaxsegmentComponent, 
         PmrMstTaxsegmentWithstateVendorComponent,
          PmrMstTaxsegmentInterstateVendorComponent,
           PmrMstTaxsegmentOverseasVendorComponent,
            PmrMstTaxsegmentOthersVendorComponent, 
            PmrMstTaxsegmentUnassignVendorComponent, 
            PmrMstTaxsegmentTotalVendorComponent,
             PmrTrnRaiseEnquiryaddNewComponent,VendorAssignDualList1Component,
             PmrTrnPurchaseorderAddselectComponent,PmrTrnPurchaseorderAddconfirmComponent, PmrRptPurchaserequisitionReportComponent,
             PmrTrnDirectpoAddLatestComponent,
             PmrTrnTaxsegment2assignvendorComponent,
             PmrTrnTaxsegment2unassignvendorComponent,
             PmrMstTaxUnMap2ProductComponent,
             PmrTrnPurchaseorderEditComponent,
             PmrTrnRatecontractComponent,
             PmrTrnRCproductassignComponent,
             PmrTrnRCproductremoveComponent,
             PmrTrnRCproductamendComponent,
             PmrTrnOperationpoapprovalComponent,
             PmrTrnFinancepoapprovalComponent,
             PmrTrnOperationpoapprovalviewComponent,
             PmrTrnFinancepoapprovalviewComponent,
             PmrTrnContractpoComponent,
             PmrTrnContractvendorComponent,
             PmrTrnCreatecontractComponent,PmrRptOutstandingamountreportSummaryComponent, PmrMstPurchasetypeComponent, PmrTrnPurchaseledgerComponent, PmrTrnVendor360Component,
              PmrRptPurchaseorderReportComponent,
              PmrRptPaymentorderReportComponent,
              PmrRptPurchaseInvoiceReportComponent,
              PmrRptVendor360Component,
              PmrRptVendorreportViewComponent,
              PmrTrnPurchaseagreementComponent,
              PmrTrnRasieAgreementorderComponent,
      
              PmrTrnPurchaseagreementviewComponent,
              PmrTrnAgreementtoinvoicetagComponent  ,
              PmrTrnEditAgreementorderComponent,
              PmrRptAgreementreportComponent           
            //  PmrTrnInvoiceaddconfirmComponent,
  ],

  imports: [
     CommonModule,
     EmsPmrRoutingModule,
     FormsModule, ReactiveFormsModule,EmsUtilitiesModule,
     FormsModule,NgApexchartsModule,ReactiveFormsModule,DataTablesModule,
     NgSelectModule,AngularEditorModule,
     NgxIntlTelInputModule,
     NgbModule
   ]
})
export class EmsPmrModule { }
