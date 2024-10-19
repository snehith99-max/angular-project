import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DecimalPipe } from '@angular/common';
import { FilterPipe } from '../Service/filter';
import { NgSelectModule } from '@ng-select/ng-select';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { EmsSalesRoutingModule } from './ems.sales-routing.module';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgxIntlTelInputModule } from 'ngx-intl-tel-input';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { SmrMstProductgroupComponent } from './Component/smr-mst-productgroup/smr-mst-productgroup.component';
import { SmrMstTaxsummaryComponent } from './Component/smr-mst-taxsummary/smr-mst-taxsummary.component';
import { SmrMstCurrencySummaryComponent } from './Component/smr-mst-currency-summary/smr-mst-currency-summary.component';
import { SmrMstProductunitsSummaryComponent } from './Component/smr-mst-productunits-summary/smr-mst-productunits-summary.component';
import { SmrMstProductSummaryComponent } from './Component/smr-mst-product-summary/smr-mst-product-summary.component';
import { SmrTrnQuotationSummaryComponent } from './Component/smr-trn-quotation-summary/smr-trn-quotation-summary.component';
import { SmrTrnSalesorderSummaryComponent } from './Component/smr-trn-salesorder-summary/smr-trn-salesorder-summary.component';
import { SmrRptSalesorderReportComponent } from './Component/smr-rpt-salesorder-report/smr-rpt-salesorder-report.component';
import { SmrMstProductaddComponent } from './Component/smr-mst-productadd/smr-mst-productadd.component';
import { SmrMstProducteditComponent } from './Component/smr-mst-productedit/smr-mst-productedit.component';
import { SmrMstProductviewComponent } from './Component/smr-mst-productview/smr-mst-productview.component';
import { SmrTrnCustomerenquiryeditComponent } from './Component/smr-trn-customerenquiryedit/smr-trn-customerenquiryedit.component';
import { SmrTrnCustomerSummaryComponent } from './Component/smr-trn-customer-summary/smr-trn-customer-summary.component';
import { SmrTrnCustomeraddComponent } from './Component/smr-trn-customeradd/smr-trn-customeradd.component';
import { SmrTrnRaiseproposalComponent } from './Component/smr-trn-raiseproposal/smr-trn-raiseproposal.component';
import { SmrTrnQuotationaddComponent } from './Component/smr-trn-quotationadd/smr-trn-quotationadd.component';
import { SmrTrnMyenquiryComponent } from './Component/smr-trn-myenquiry/smr-trn-myenquiry.component';
import { SmrTrnAllComponent } from './Component/smr-trn-all/smr-trn-all.component';
import { SmrTrnNewComponent } from './Component/smr-trn-new/smr-trn-new.component';
import { SmrTrnPotentialComponent } from './Component/smr-trn-potential/smr-trn-potential.component';
import { SmrTrnProspectComponent } from './Component/smr-trn-prospect/smr-trn-prospect.component';
import { SmrTrnDropComponent } from './Component/smr-trn-drop/smr-trn-drop.component';
import { SmrTrnCompletedComponent } from './Component/smr-trn-completed/smr-trn-completed.component';
import { SmrMstPricesegmentComponent } from './Component/smr-mst-pricesegment/smr-mst-pricesegment.component';
import { SmrMstProductAssignComponent } from './Component/smr-mst-product-assign/smr-mst-product-assign.component';
import { SmrTrnRaisesalesorderComponent } from './Component/smr-trn-raisesalesorder/smr-trn-raisesalesorder.component';
import { SmrTrnAdddeliveryorderComponent } from './Component/smr-trn-adddeliveryorder/smr-trn-adddeliveryorder.component';
import { SmrTrnRaisedeliveryorderComponent } from './Component/smr-trn-raisedeliveryorder/smr-trn-raisedeliveryorder.component';
import { SmrTrnDeliveryorderSummaryComponent } from './Component/smr-trn-deliveryorder-summary/smr-trn-deliveryorder-summary.component';
import { SmrTrnRaisequoteComponent } from './Component/smr-trn-raisequote/smr-trn-raisequote.component';
import { SmrtrnquotetoorderComponent } from './Component/smrtrnquotetoorder/smrtrnquotetoorder.component';
import { SmrMstSalesteamSummaryComponent } from './Component/smr-mst-salesteam-summary/smr-mst-salesteam-summary.component';
import { DualListComponent } from './Component/smr-mst-pricesegment/dual-list/dual-list.component';
import { SmrRptTodaySalesreportComponent } from './Component/smr-rpt-today-salesreport/smr-rpt-today-salesreport.component';
import { SmrRptSalesreportComponent } from './Component/smr-rpt-salesreport/smr-rpt-salesreport.component';
import { SmrTrnCustomerCorporateComponent } from './Component/smr-trn-customer-corporate/smr-trn-customer-corporate.component';
import { SmrTrnCustomerDistributorComponent } from './Component/smr-trn-customer-distributor/smr-trn-customer-distributor.component';
import { SmrRptTodayPaymentreportComponent } from './Component/smr-rpt-today-paymentreport/smr-rpt-today-paymentreport.component';
import { SmrRptTodayInvoicereportComponent } from './Component/smr-rpt-today-invoicereport/smr-rpt-today-invoicereport.component';
import { SmrRptTodayDeliveryreportComponent } from './Component/smr-rpt-today-deliveryreport/smr-rpt-today-deliveryreport.component';
import { SmrTrnCustomerRetailerComponent } from './Component/smr-trn-customer-retailer/smr-trn-customer-retailer.component';
import { SmrRptOrderreportComponent } from './Component/smr-rpt-orderreport/smr-rpt-orderreport.component';
import { SmrRptEnquiryreportComponent } from './Component/smr-rpt-enquiryreport/smr-rpt-enquiryreport.component';
import { NgApexchartsModule } from 'ng-apexcharts';
import { SmrDashboardComponent } from './Component/smr-dashboard/smr-dashboard.component';
import { SrmTrnCustomerviewComponent } from './Component/srm-trn-customerview/srm-trn-customerview.component';
import { SmrTrnCustomerenquirySummaryComponent } from './Component/smr-trn-customerenquiry-summary/smr-trn-customerenquiry-summary.component';
import { SmrTrnSalesorderviewComponent } from './Component/smr-trn-salesorderview/smr-trn-salesorderview.component';
import { SrmTrnNewquotationviewComponent } from './Component/srm-trn-newquotationview/srm-trn-newquotationview.component';
import { SmrRptQuotationreportComponent } from './Component/smr-rpt-quotationreport/smr-rpt-quotationreport.component';
import { SmrTrnCustomerProductPriceComponent } from './Component/smr-trn-customer-product-price/smr-trn-customer-product-price.component';
import { SmtMstCustomerEditComponent } from './Component/smt-mst-customer-edit/smt-mst-customer-edit.component';
import { SmrTrnSalesorderamendComponent } from './Component/smr-trn-salesorderamend/smr-trn-salesorderamend.component';
import { SmrTrnQuotationmailComponent } from './Component/smr-trn-quotationmail/smr-trn-quotationmail.component';
import { SmrTrnAmendQuotationComponent } from './Component/smr-trn-amend-quotation/smr-trn-amend-quotation.component';
import { SmrTrnQuotationHistoryComponent } from './Component/smr-trn-quotation-history/smr-trn-quotation-history.component';
import { SmrTrnCustomerraiseenquiryComponent } from './Component/smr-trn-customerraiseenquiry/smr-trn-customerraiseenquiry.component';
import { SalesTeamDualListComponent } from './Component/smr-mst-salesteam-summary/dual-list/dual-list.component';
import { SalesteamManagerListComponent } from './Component/smr-mst-salesteam-summary/salesteam-manager-list/salesteam-manager-list.component';
import { SmrTrnCustomerbranchComponent } from './Component/smr-trn-customerbranch/smr-trn-customerbranch.component';
import { SmrTrnSalesteampotentialsComponent } from './Component/smr-trn-salesteampotentials/smr-trn-salesteampotentials.component';
import { SmrTrnSalesteamprospectComponent } from './Component/smr-trn-salesteamprospect/smr-trn-salesteamprospect.component';
import { SmrTrnSalesManagerSummaryComponent } from './Component/smr-trn-sales-manager-summary/smr-trn-sales-manager-summary.component';
import { SmrTrnSalesteamCompleteComponent } from './Component/smr-trn-salesteam-complete/smr-trn-salesteam-complete.component';
import { SmrTrnSalesteamDropComponent } from './Component/smr-trn-salesteam-drop/smr-trn-salesteam-drop.component';
import { SmrRptCustomerledgerreportComponent } from './Component/smr-rpt-customerledgerreport/smr-rpt-customerledgerreport.component';
import { SmrRptSalesorderDetailedreportComponent } from './Component/smr-rpt-salesorder-detailedreport/smr-rpt-salesorder-detailedreport.component';
import { SmrTrnCustomerCallComponent } from './Component/smr-trn-customer-call/smr-trn-customer-call.component';
import { SmrRptCustomerledgerdetailComponent } from './Component/smr-rpt-customerledgerdetail/smr-rpt-customerledgerdetail.component';
import { SmrRptCustomerledgerinvoiceComponent } from './Component/smr-rpt-customerledgerinvoice/smr-rpt-customerledgerinvoice.component';
import { SmrRptCustomerledgerpaymentComponent } from './Component/smr-rpt-customerledgerpayment/smr-rpt-customerledgerpayment.component';
import { SmrRptCustomerledgeroutstandingreportComponent } from './Component/smr-rpt-customerledgeroutstandingreport/smr-rpt-customerledgeroutstandingreport.component';
import { SmrTrnSales360Component } from './Component/smr-trn-sales360/smr-trn-sales360.component';
import { SmrTrnCommissionSettingComponent } from './Component/smr-trn-commission-setting/smr-trn-commission-setting.component';
import { SmrTrnCommissionPayoutComponent } from './Component/smr-trn-commission-payout/smr-trn-commission-payout.component';
import { SmrTrnCommissionPayoutAddComponent } from './Component/smr-trn-commission-payout-add/smr-trn-commission-payout-add.component';
import { SmrRptCommissionpayoutReportComponent } from './Component/smr-rpt-commissionpayout-report/smr-rpt-commissionpayout-report.component';
import { SmrRptTeamwiseReportComponent } from './Component/smr-rpt-teamwise-report/smr-rpt-teamwise-report.component';
import { SmrRptemployeewiseReportComponent } from './Component/smr-rptemployeewise-report/smr-rptemployeewise-report.component';
import { SmrMstConfigurationComponent } from './Component/smr-mst-configuration/smr-mst-configuration.component';
import { SmrTrnEnquiryfrom360Component } from './Component/smr-trn-enquiryfrom360/smr-trn-enquiryfrom360.component';
import { SmrTrnQuotationfrom360Component } from './Component/smr-trn-quotationfrom360/smr-trn-quotationfrom360.component';
import { SmrTrnSalesordereditComponent } from './Component/smr-trn-salesorderedit/smr-trn-salesorderedit.component';

import { SmrTrnOrderfrom360Component } from './Component/smr-trn-orderfrom360/smr-trn-orderfrom360.component';
import { SmrTrnInvoiceAdd360Component } from './Component/smr-trn-invoice-add360/smr-trn-invoice-add360.component';
import { SmrMstTaxsegmentComponent } from './Component/smr-mst-taxsegment/smr-mst-taxsegment.component';
import { SmrMstMaptaxsegment2productComponent } from './Component/smr-mst-maptaxsegment2product/smr-mst-maptaxsegment2product.component';
import { SmrTrnTenquiryviewComponent } from './Component/smr-trn-tenquiryview/smr-trn-tenquiryview.component';
import { CustomerAssignDualListComponent } from './Component/smr-mst-taxsegment/customer-assign-dual-list/customer-assign-dual-list.component';
import { VendorAssignDualListComponent } from './Component/smr-mst-taxsegment/vendor-assign-dual-list/vendor-assign-dual-list.component';
import { SmrTrnSalesorderhistoryComponent } from './Component/smr-trn-salesorderhistory/smr-trn-salesorderhistory.component';
import { SmrMstMaptax2productComponent } from './Component/smr-mst-maptax2product/smr-mst-maptax2product.component';
import { SmrRptInvoicereportComponent } from './Component/smr-rpt-invoicereport/smr-rpt-invoicereport.component';
import { SmrTrnInvoiceaddComponent } from './Component/smr-trn-invoiceadd/smr-trn-invoiceadd.component';
import { SmrTrnInvoiceraiseComponent } from './Component/smr-trn-invoiceraise/smr-trn-invoiceraise.component';
import { SmrTrnInvoiceeditComponent } from './Component/smr-trn-invoiceedit/smr-trn-invoiceedit.component';
import { SmrMstTaxsegmenttotalcustomersComponent } from './Component/smr-mst-taxsegmenttotalcustomers/smr-mst-taxsegmenttotalcustomers.component';
import { SmrMstTaxsegmentothersegmentsComponent } from './Component/smr-mst-taxsegmentothersegments/smr-mst-taxsegmentothersegments.component';
import { SmrMstTaxsegmentoverseascustomerComponent } from './Component/smr-mst-taxsegmentoverseascustomer/smr-mst-taxsegmentoverseascustomer.component';
import { SmrMstTaxsegmentunassigncustomerComponent } from './Component/smr-mst-taxsegmentunassigncustomer/smr-mst-taxsegmentunassigncustomer.component';
import { SmrMstTaxsegmentwithinstatecustomerComponent } from './Component/smr-mst-taxsegmentwithinstatecustomer/smr-mst-taxsegmentwithinstatecustomer.component';
import { SmrMstTaxsegmentinterstatecustomerComponent } from './Component/smr-mst-taxsegmentinterstatecustomer/smr-mst-taxsegmentinterstatecustomer.component';
import { TableModule } from 'primeng/table';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { SmrTrnQuotationaddNewComponent } from './Component/smr-trn-quotationadd-new/smr-trn-quotationadd-new.component';
import{RblTrnDirectinvoiceComponent} from './Component/rbl-trn-directinvoice/rbl-trn-directinvoice.component';
import { SmrTrnQuotationviewNewComponent } from './Component/smr-trn-quotationview-new/smr-trn-quotationview-new.component';
import { SmrTrnRaiseSalesOrderComponent } from './Component/smr-trn-raise-sales-order/smr-trn-raise-sales-order.component';
import { SmrTrnSalesorderviewNewComponent } from './Component/smr-trn-salesorderview-new/smr-trn-salesorderview-new.component';
import { SmrTrnQuotationfrom360NewComponent } from './Component/smr-trn-quotationfrom360-new/smr-trn-quotationfrom360-new.component';
import { SmrTrnOrderfrom360NewComponent } from './Component/smr-trn-orderfrom360-new/smr-trn-orderfrom360-new.component';
import { SmrTrnSalesorderapprovalComponent } from './Component/smr-trn-salesorderapproval/smr-trn-salesorderapproval.component';
import { SmrTrnInvoicemailComponent } from './Component/smr-trn-invoicemail/smr-trn-invoicemail.component';
import { SmrTrnInvoiceaccountingaddconfirmComponent } from './Component/smr-trn-invoiceaccountingaddconfirm/smr-trn-invoiceaccountingaddconfirm.component';
import { SmrMstAssigncustomerComponent } from './Component/smr-mst-assigncustomer/smr-mst-assigncustomer.component';
import { SmrMstUnassigntax2productComponent } from './Component/smr-mst-unassigntax2product/smr-mst-unassigntax2product.component';
import { SmrMstPriceassigncustomerComponent } from './Component/smr-mst-priceassigncustomer/smr-mst-priceassigncustomer.component';
import { SmrMstPriceunassigncustomerComponent } from './Component/smr-mst-priceunassigncustomer/smr-mst-priceunassigncustomer.component';
import { SmrMstPriceunassignproductComponent } from './Component/smr-mst-priceunassignproduct/smr-mst-priceunassignproduct.component';
import { SmrTrnReceiptsummaryComponent } from './Component/smr-trn-receiptsummary/smr-trn-receiptsummary.component';
import { SmrTrnAddreceiptComponent } from './Component/smr-trn-addreceipt/smr-trn-addreceipt.component';
import { SmrTrnInvoiceviewComponent } from './Component/smr-trn-invoiceview/smr-trn-invoiceview.component';
import { SmrMstCustomereportalpreviewmailComponent } from './Component/smr-mst-customereportalpreviewmail/smr-mst-customereportalpreviewmail.component';
import { RblTrnOrdertoinvoiceComponent } from './Component/rbl-trn-ordertoinvoice/rbl-trn-ordertoinvoice.component';
import { SmrTrnInvoiceReceiptComponent } from './Component/smr-trn-invoice-receipt/smr-trn-invoice-receipt.component';
import { SmrTrnRaisesalesorder2invoiceComponent } from './Component/smr-trn-raisesalesorder2invoice/smr-trn-raisesalesorder2invoice.component';
import { SmrTrnShopifyordersComponent } from './Component/smr-trn-shopifyorders/smr-trn-shopifyorders.component';
import { SmrMstWhatsappproductsummaryComponent } from './Component/smr-mst-whatsappproductsummary/smr-mst-whatsappproductsummary.component';
import { SmrMstWhatsappproducteditComponent } from './Component/smr-mst-whatsappproductedit/smr-mst-whatsappproductedit.component';
 import { SmrTrnShopifyordersviewComponent } from './Component/smr-trn-shopifyordersview/smr-trn-shopifyordersview.component';
 import { SmrTrnShopifyproductComponent } from './Component/smr-trn-shopifyproduct/smr-trn-shopifyproduct.component';
import { SmrTrnShopifycustomerComponent } from './Component/smr-trn-shopifycustomer/smr-trn-shopifycustomer.component';
import { SmrTrnShopifypaymentComponent } from './Component/smr-trn-shopifypayment/smr-trn-shopifypayment.component';
import { SmrTrnShopifyinventoryComponent } from './Component/smr-trn-shopifyinventory/smr-trn-shopifyinventory.component';
import { SmrTrnPortalordersCustomerComponent } from './Component/smr-trn-portalorders-customer/smr-trn-portalorders-customer.component';
import { SmrTrnPortalordersCustomerApprovalComponent } from './Component/smr-trn-portalorders-customer-approval/smr-trn-portalorders-customer-approval.component';
import { SmrTrnQuoteeditnewComponent } from './Component/smr-trn-quoteeditnew/smr-trn-quoteeditnew.component';
import { SmrTrnTcreditnnoteSummaryComponent } from './Component/smr-trn-tcreditnnote-summary/smr-trn-tcreditnnote-summary.component';
import { SmrTrnTcreditnoteaddproceedComponent } from './Component/smr-trn-tcreditnoteaddproceed/smr-trn-tcreditnoteaddproceed.component';
import { SmrTrnTcreditnoteaddselectComponent } from './Component/smr-trn-tcreditnoteaddselect/smr-trn-tcreditnoteaddselect.component';
import { SmrTrnTcreditnoteviewComponent } from './Component/smr-trn-tcreditnoteview/smr-trn-tcreditnoteview.component';
import { SmrTrnTcreditnotestockreturnComponent } from './Component/smr-trn-tcreditnotestockreturn/smr-trn-tcreditnotestockreturn.component';
 import { SmrTrnRenewalmanagersummaryComponent } from './Component/smr-trn-renewalmanagersummary/smr-trn-renewalmanagersummary.component';
import { SmrTrnRenewalteamsummaryComponent } from './Component/smr-trn-renewalteamsummary/smr-trn-renewalteamsummary.component';
import { RenewalDualListComponent } from './Component/smr-trn-renewalteamsummary/renewal-dual-list/renewal-dual-list.component';
import { RenewalManagerListComponent } from './Component/smr-trn-renewalteamsummary/renewal-manager-list/renewal-manager-list.component';
import { SmrTrnSalesInvoiceSummaryComponent } from './Component/smr-trn-sales-invoice-summary/smr-trn-sales-invoice-summary.component';
import { SmrTrnEinvoiceComponent } from './Component/smr-trn-einvoice/smr-trn-einvoice.component';
import { SmrMstWhatsappproductpricemanagementComponent } from './Component/smr-mst-whatsappproductpricemanagement/smr-mst-whatsappproductpricemanagement.component';
import { SmrMstWaassignproductComponent } from './Component/smr-mst-waassignproduct/smr-mst-waassignproduct.component';
import { SmrMstWaproductpriceupdateComponent } from './Component/smr-mst-waproductpriceupdate/smr-mst-waproductpriceupdate.component';
import { SmrMstBranchwhatsappproductsummaryComponent } from './Component/smr-mst-branchwhatsappproductsummary/smr-mst-branchwhatsappproductsummary.component';
import { SmrTrnRenevalsummaryComponent } from './Component/smr-trn-renevalsummary/smr-trn-renevalsummary.component';
import { SmrTrnRenewalsalesorderselectComponent } from './Component/smr-trn-renewalsalesorderselect/smr-trn-renewalsalesorderselect.component';
import { SmrTrnRenewalsummaryviewComponent } from './Component/smr-trn-renewalsummaryview/smr-trn-renewalsummaryview.component';
import { SmrTrnRenewalInvoiceComponent } from './Component/smr-trn-renewal-invoice/smr-trn-renewal-invoice.component';
import { SmrTrnRenewaladdComponent } from './Component/smr-trn-renewaladd/smr-trn-renewaladd.component';
import { SmrRptAgeingreportComponent } from './Component/smr-rpt-ageingreport/smr-rpt-ageingreport.component';
import { SmrTrnRenewaltoraisesoComponent } from './Component/smr-trn-renewaltoraiseso/smr-trn-renewaltoraiseso.component';
import { SmrRptOuststandingamountReportComponent } from './Component/smr-rpt-ouststandingamount-report/smr-rpt-ouststandingamount-report.component';
import { SmrRptSalesinvoicereportComponent } from './Component/smr-rpt-salesinvoicereport/smr-rpt-salesinvoicereport.component';
import { SmrMstSalestypeComponent } from './Component/smr-mst-salestype/smr-mst-salestype.component';
import { SmrTrnReceiptapprovalComponent } from './Component/smr-trn-receiptapproval/smr-trn-receiptapproval.component';
import { SmrRptRenewalreportComponent } from './Component/smr-rpt-renewalreport/smr-rpt-renewalreport.component';
import { SmrTrnSalesledgerComponent } from './Component/smr-trn-salesledger/smr-trn-salesledger.component';
import { SmrTrnCustomer360Component } from './Component/smr-trn-customer360/smr-trn-customer360.component';
import { SmrTrnSalesledgerInvoiceviewComponent } from './Component/smr-trn-salesledger-invoiceview/smr-trn-salesledger-invoiceview.component';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { SmrTrnRenewalassignComponent } from './Component/smr-trn-renewalassign/smr-trn-renewalassign.component';
import { SmrTrnRenewalemployeeComponent } from './Component/smr-trn-renewalemployee/smr-trn-renewalemployee.component';
import { SmrTrnEnquiry360Component } from './Component/smr-trn-enquiry360/smr-trn-enquiry360.component';
import { SmrBobateaDashboardComponent } from './Component/smr-bobatea-dashboard/smr-bobatea-dashboard.component';
import { RblTrnDirectinvoice360Component } from './Component/rbl-trn-directinvoice360/rbl-trn-directinvoice360.component';
import { SmrTrnRaisesalesordernew360Component } from './Component/smr-trn-raisesalesordernew360/smr-trn-raisesalesordernew360.component';
import { SmrTrnQuotationaddNew360Component } from './Component/smr-trn-quotationadd-new360/smr-trn-quotationadd-new360.component';
import { SmrMstProducthsncodeComponent } from './Component/smr-mst-producthsncode/smr-mst-producthsncode.component';
import { SmrMstSequencecustomizerComponent } from './Component/smr-mst-sequencecustomizer/smr-mst-sequencecustomizer.component';
import { SmrMstSequencecodeeditComponent } from './Component/smr-mst-sequencecodeedit/smr-mst-sequencecodeedit.component';
import { SmrTrnRenewals360Component } from './Component/smr-trn-renewals360/smr-trn-renewals360.component';
import { SmrTrnLeadtocustomerComponent } from './Component/smr-trn-leadtocustomer/smr-trn-leadtocustomer.component';
import { SmrTrnSalesAll360Component } from './Component/smr-trn-sales-all360/smr-trn-sales-all360.component';
import { SmrTrnRaiseAgreementComponent } from './Component/smr-trn-raise-agreement/smr-trn-raise-agreement.component';
import { SmrTrnAssignrenewalagreementComponent } from './Component/smr-trn-assignrenewalagreement/smr-trn-assignrenewalagreement.component';
import { SmrTrnRenewaltoInvoiceComponent } from './Component/smr-trn-renewalto-invoice/smr-trn-renewalto-invoice.component';
import { SmrRptSales360Component } from './Component/smr-rpt-sales360/smr-rpt-sales360.component';
import { SmrRptCustomerreportComponent } from './Component/smr-rpt-customerreport/smr-rpt-customerreport.component';
import { SmrRptCustomerreportViewComponent } from './Component/smr-rpt-customerreport-view/smr-rpt-customerreport-view.component';
import { SmrRptSalesreportviewComponent } from './Component/smr-rpt-salesreportview/smr-rpt-salesreportview.component';
import { SmrTrnRenewalEditComponent } from './Component/smr-trn-renewal-edit/smr-trn-renewal-edit.component';
import { SmrRptReceiptreportComponent } from './Component/smr-rpt-receiptreport/smr-rpt-receiptreport.component';
import { SmrTrnAgreementtoinvoicetagComponent } from './Component/smr-trn-agreementtoinvoicetag/smr-trn-agreementtoinvoicetag.component';
import { SmrTrnReceiptviewComponent } from './Component/smr-trn-receiptview/smr-trn-receiptview.component';
import { SmrRptProductsellingreportComponent } from './Component/smr-rpt-productsellingreport/smr-rpt-productsellingreport.component';
import { SmrRptProductgroupreportComponent } from './Component/smr-rpt-productgroupreport/smr-rpt-productgroupreport.component';
import { SmrRptProductconsumptioneportsComponent } from './Component/smr-rpt-productconsumptioneports/smr-rpt-productconsumptioneports.component';
import { SmrTrnTproformainvoiceComponent } from './Component/smr-trn-tproformainvoice/smr-trn-tproformainvoice.component';
import { SmrTrnRaiseorder2ProformainvoiceComponent } from './Component/smr-trn-raiseorder2-proformainvoice/smr-trn-raiseorder2-proformainvoice.component';
import { SmrTrnOrdertoproformainvoiceComponent } from './Component/smr-trn-ordertoproformainvoice/smr-trn-ordertoproformainvoice.component';
import { SmrTrnEditproformainvoiceComponent } from './Component/smr-trn-editproformainvoice/smr-trn-editproformainvoice.component';
import { SmrTrnProformainvoiceviewComponent } from './Component/smr-trn-proformainvoiceview/smr-trn-proformainvoiceview.component';
import { RblTrnDeliverytoinvoiceComponent } from './Component/rbl-trn-deliverytoinvoice/rbl-trn-deliverytoinvoice.component';
import { SmrTrnRaisePurchaseorderComponent } from './Component/smr-trn-raise-purchaseorder/smr-trn-raise-purchaseorder.component';
@NgModule({
  declarations: [
    SmrRptOrderreportComponent,
    SmrMstProductgroupComponent,
    SmrMstTaxsummaryComponent,
    SmrMstCurrencySummaryComponent,
    SmrMstProductunitsSummaryComponent,
    SmrMstProductSummaryComponent,
    SmrTrnQuotationSummaryComponent,
    SmrTrnSalesorderSummaryComponent,
    SmrRptSalesorderReportComponent,
    SmrTrnCustomerenquirySummaryComponent,
    SmrMstProductaddComponent,
    SmrMstProducteditComponent,
    SmrMstProductviewComponent,
    SmrRptEnquiryreportComponent,
    SmrTrnCustomerenquiryeditComponent,
    SmrTrnCustomerSummaryComponent,
    SmrTrnCustomeraddComponent,
    SmrTrnRaiseproposalComponent,
    SmrTrnQuotationaddComponent,
    SmrMstPricesegmentComponent,
    SmrMstProductAssignComponent,
    SmrTrnRaisesalesorderComponent,
    SmrTrnMyenquiryComponent,
    SmrTrnSalesordereditComponent,
    SmrTrnAllComponent,
    SmrTrnNewComponent,
    SmrTrnPotentialComponent,
    SmrTrnProspectComponent,
    SmrTrnDropComponent,
    SmrMstWhatsappproductsummaryComponent,
    SmrMstWhatsappproducteditComponent,
    SmrTrnCompletedComponent,
    SmrTrnAdddeliveryorderComponent,
    SmrTrnRaisedeliveryorderComponent,
    SmrTrnDeliveryorderSummaryComponent,
    SmrTrnRaisequoteComponent,
    SmrtrnquotetoorderComponent,
    SmrMstSalesteamSummaryComponent, DualListComponent,
    SmrRptTodaySalesreportComponent,
    SmrRptSalesreportComponent,
    SmrTrnCustomerCorporateComponent,
    SmrTrnCustomerDistributorComponent,
    SmrRptTodayPaymentreportComponent,
    SmrRptTodayInvoicereportComponent,
    SmrRptTodayDeliveryreportComponent,
    SmrTrnCustomerRetailerComponent,
    SmrDashboardComponent,
    SrmTrnCustomerviewComponent,
    SmrTrnSalesorderviewComponent,
    SrmTrnNewquotationviewComponent,
    SmrRptQuotationreportComponent,
    SmrTrnCustomerProductPriceComponent,
    SmtMstCustomerEditComponent,
    SmrTrnSalesorderamendComponent,
    SmrTrnQuotationmailComponent,
    SmrTrnAmendQuotationComponent,
    SmrTrnInvoiceReceiptComponent,
    SmrTrnQuotationHistoryComponent,
    SmrTrnCustomerraiseenquiryComponent,
    SalesTeamDualListComponent,
    SalesteamManagerListComponent,
    SmrTrnCustomerbranchComponent,
    SmrTrnSalesteampotentialsComponent,
    SmrTrnSalesteamprospectComponent,
    SmrTrnSalesManagerSummaryComponent,
    SmrTrnSalesteamCompleteComponent,
    SmrTrnSalesteamDropComponent,
    SmrMstCustomereportalpreviewmailComponent,
    SmrRptCustomerledgerreportComponent,
    SmrRptSalesorderDetailedreportComponent,
    SmrTrnCustomerCallComponent,
    SmrRptCustomerledgerdetailComponent,
    SmrRptCustomerledgerinvoiceComponent,
    SmrRptCustomerledgerpaymentComponent,
    SmrRptCustomerledgeroutstandingreportComponent,
    SmrTrnSales360Component,
    SmrTrnCommissionSettingComponent,
    SmrTrnCommissionPayoutComponent,
    SmrTrnCommissionPayoutAddComponent,
    SmrRptCommissionpayoutReportComponent,
    SmrRptTeamwiseReportComponent,
    SmrRptemployeewiseReportComponent,
    SmrMstConfigurationComponent,
    SmrTrnEnquiryfrom360Component,
    SmrTrnQuotationfrom360Component,
    SmrTrnOrderfrom360Component,
    SmrTrnInvoiceAdd360Component,SmrMstTaxsegmentComponent, SmrMstMaptaxsegment2productComponent,
    SmrTrnTenquiryviewComponent,CustomerAssignDualListComponent,VendorAssignDualListComponent, SmrTrnSalesorderhistoryComponent, SmrMstMaptax2productComponent, SmrRptInvoicereportComponent, SmrTrnInvoiceaddComponent, SmrTrnInvoiceraiseComponent, SmrTrnInvoiceeditComponent, 
     SmrMstTaxsegmenttotalcustomersComponent,
     SmrMstTaxsegmentothersegmentsComponent,
     SmrMstTaxsegmentoverseascustomerComponent,
     SmrMstTaxsegmentunassigncustomerComponent,
     SmrMstTaxsegmentwithinstatecustomerComponent,
     SmrMstTaxsegmentinterstatecustomerComponent,
     SmrTrnQuotationaddNewComponent,
     RblTrnDirectinvoiceComponent,
     SmrTrnQuotationviewNewComponent, 
     SmrTrnRaiseSalesOrderComponent, 
     SmrTrnSalesorderviewNewComponent,
     SmrTrnQuotationfrom360NewComponent,
     SmrTrnOrderfrom360NewComponent,
     SmrTrnInvoiceviewComponent,
     FilterPipe,SmrTrnSalesorderapprovalComponent, 
     SmrTrnInvoicemailComponent, 
     SmrTrnInvoiceaccountingaddconfirmComponent,
      SmrMstAssigncustomerComponent,
       SmrMstUnassigntax2productComponent, SmrMstPriceassigncustomerComponent,
        SmrMstPriceunassigncustomerComponent, SmrMstPriceunassignproductComponent,
         SmrTrnReceiptsummaryComponent, SmrTrnAddreceiptComponent,
         RblTrnOrdertoinvoiceComponent,
         SmrTrnInvoiceReceiptComponent,
         SmrTrnRaisesalesorder2invoiceComponent,
         SmrTrnShopifyordersComponent,
         SmrTrnShopifyordersviewComponent,
         SmrTrnShopifyproductComponent,
         SmrTrnShopifycustomerComponent,
         SmrTrnShopifypaymentComponent,
         SmrTrnShopifyinventoryComponent,
         SmrTrnPortalordersCustomerComponent,
         SmrTrnPortalordersCustomerApprovalComponent,
         SmrTrnQuoteeditnewComponent,
         SmrTrnTcreditnnoteSummaryComponent,
         SmrTrnTcreditnoteaddproceedComponent,
         SmrTrnTcreditnoteaddselectComponent,
         SmrTrnTcreditnoteviewComponent,
         SmrTrnTcreditnotestockreturnComponent,
        SmrTrnRenewalmanagersummaryComponent,
         SmrTrnRenewalteamsummaryComponent ,
         RenewalDualListComponent,
         RenewalManagerListComponent   ,
         SmrTrnEinvoiceComponent,SmrTrnSalesInvoiceSummaryComponent,
         SmrMstWhatsappproductpricemanagementComponent,
         SmrMstWaassignproductComponent,
         SmrMstWaproductpriceupdateComponent,
         SmrMstBranchwhatsappproductsummaryComponent,
         SmrTrnRenevalsummaryComponent,
         SmrTrnRenewalsalesorderselectComponent,
         SmrTrnRenewalsummaryviewComponent, 
         SmrTrnRenewalInvoiceComponent,
         SmrTrnRenewaladdComponent,
         SmrRptAgeingreportComponent,
         SmrTrnRenewaltoraisesoComponent,
         SmrRptOuststandingamountReportComponent,
         SmrRptSalesinvoicereportComponent,
         SmrMstSalestypeComponent,
         SmrTrnReceiptapprovalComponent,
         SmrRptRenewalreportComponent,
         SmrTrnSalesledgerComponent,
         SmrTrnCustomer360Component,
         SmrTrnSalesledgerInvoiceviewComponent,
         SmrTrnRenewalassignComponent,
         SmrTrnRenewalemployeeComponent,
         SmrTrnQuotationaddNew360Component,
         SmrTrnRaisesalesordernew360Component,
         RblTrnDirectinvoice360Component,
         SmrBobateaDashboardComponent,
	       SmrTrnEnquiry360Component	,
         SmrMstProducthsncodeComponent,
         SmrMstSequencecustomizerComponent,
         SmrMstSequencecodeeditComponent,
         SmrTrnRenewals360Component,
         SmrTrnLeadtocustomerComponent,
         SmrTrnSalesAll360Component,
         SmrTrnRaiseAgreementComponent,
         SmrTrnAssignrenewalagreementComponent,
         SmrTrnRenewaltoInvoiceComponent,SmrRptSales360Component,SmrTrnRenewalEditComponent,
          SmrRptCustomerreportComponent, SmrRptCustomerreportViewComponent, SmrRptSalesreportviewComponent, SmrRptReceiptreportComponent, SmrTrnAgreementtoinvoicetagComponent, SmrTrnReceiptviewComponent, SmrRptProductsellingreportComponent,
		SmrTrnTproformainvoiceComponent, SmrTrnRaiseorder2ProformainvoiceComponent, SmrTrnOrdertoproformainvoiceComponent, SmrTrnEditproformainvoiceComponent, SmrTrnProformainvoiceviewComponent
    ,SmrRptProductgroupreportComponent,SmrRptProductconsumptioneportsComponent, RblTrnDeliverytoinvoiceComponent, SmrTrnRaisePurchaseorderComponent

  ],

  imports: [
    CommonModule,
    EmsSalesRoutingModule,
    NgApexchartsModule,
    PaginationModule.forRoot(),
    FormsModule, ReactiveFormsModule,
    NgSelectModule,
    TabsModule, AngularEditorModule,NgxIntlTelInputModule, DecimalPipe, TableModule, NgbModule  
  ]
})

export class EmsSalesModule { }
