import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgApexchartsModule } from 'ng-apexcharts';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { EmsInventoryRoutingModule } from './ems.inventory-routing.module';
import { ImsTrnOpeningstockSummaryComponent } from './Component/ims-trn-openingstock-summary/ims-trn-openingstock-summary.component';
import { ImsTrnIssuematerialSummaryComponent } from './Component/ims-trn-issuematerial-summary/ims-trn-issuematerial-summary.component';
import { ImsTrnStockadjustmentSummaryComponent } from './Component/ims-trn-stockadjustment-summary/ims-trn-stockadjustment-summary.component';
import { ImsTrnDeliveryorderComponent } from './Component/ims-trn-deliveryorder/ims-trn-deliveryorder.component';
import { ImsTrnAddDeliveryorderComponent } from './Component/ims-trn-add-deliveryorder/ims-trn-add-deliveryorder.component';
import { ImsTrnRaiseDeliveryorderComponent } from './Component/ims-trn-raise-deliveryorder/ims-trn-raise-deliveryorder.component';
import { ImsTrnOpeningstockAddComponent } from './Component/ims-trn-openingstock-add/ims-trn-openingstock-add.component';
import { ImsTrnOpeningstockEditComponent } from './Component/ims-trn-openingstock-edit/ims-trn-openingstock-edit.component';
import { ImsTrnDeliveryAcknowledgementComponent } from './Component/ims-trn-delivery-acknowledgement/ims-trn-delivery-acknowledgement.component';
import { ImsTrnDeliveryacknowlegdementAddComponent } from './Component/ims-trn-deliveryacknowlegdement-add/ims-trn-deliveryacknowlegdement-add.component';
import { ImsTrnDeliveryacknowledgementUpdateComponent } from './Component/ims-trn-deliveryacknowledgement-update/ims-trn-deliveryacknowledgement-update.component';
import { ImsTrnOpendcsummaryComponent } from './Component/ims-trn-opendcsummary/ims-trn-opendcsummary.component';
import { ImsTrnOpendcAddselectComponent } from './Component/ims-trn-opendc-addselect/ims-trn-opendc-addselect.component';
import { ImsTrnOpendcaddselectUpdateComponent } from './Component/ims-trn-opendcaddselect-update/ims-trn-opendcaddselect-update.component';
import { ImsRptStockreportComponent } from './Component/ims-rpt-stockreport/ims-rpt-stockreport.component';
import { ImsTrnDespatchViewComponent } from './Component/ims-trn-despatch-view/ims-trn-despatch-view.component';
import { ImsTrnDeliveryorderViewComponent } from './Component/ims-trn-deliveryorder-view/ims-trn-deliveryorder-view.component';
import { ImsTrnStocksummaryComponent } from './Component/ims-trn-stocksummary/ims-trn-stocksummary.component';
import { ImsTrnAddstockComponent } from './Component/ims-trn-addstock/ims-trn-addstock.component';
import { ImsTrnStockamendComponent } from './Component/ims-trn-stockamend/ims-trn-stockamend.component';
import { ImsTrnAddamendstockComponent } from './Component/ims-trn-addamendstock/ims-trn-addamendstock.component';
import { ImsTrnStockdamageComponent } from './Component/ims-trn-stockdamage/ims-trn-stockdamage.component';
import { ImsTrnAddDamagedstockComponent } from './Component/ims-trn-add-damagedstock/ims-trn-add-damagedstock.component';
import { ImsTrnDirectissuematerialComponent } from './Component/ims-trn-directissuematerial/ims-trn-directissuematerial.component';
import { ImsTrnMaterialindentSummaryComponent } from './Component/ims-trn-materialindent-summary/ims-trn-materialindent-summary.component';
import { ImsTrnMaterialindentAddComponent } from './Component/ims-trn-materialindent-add/ims-trn-materialindent-add.component';
import { ImsTrnIssuematerialViewComponent } from './Component/ims-trn-issuematerial-view/ims-trn-issuematerial-view.component';
import { ImsTrnMaterialindentIssueComponent } from './Component/ims-trn-materialindent-issue/ims-trn-materialindent-issue.component';
import { ImsTrnMaterialindentViewComponent } from './Component/ims-trn-materialindent-view/ims-trn-materialindent-view.component';
import { ImsMstLocationComponent } from './Component/ims-mst-location/ims-mst-location.component';
import { ImsMstAssignproductComponent } from './Component/ims-mst-assignproduct/ims-mst-assignproduct.component';
import { ImsTrnWarrantytrackerComponent } from './Component/ims-trn-warrantytracker/ims-trn-warrantytracker.component';
import { ImsTrnStockstatementComponent } from './Component/ims-trn-stockstatement/ims-trn-stockstatement.component';
import { ImsTrnOpendcComponent } from './Component/ims-trn-opendc/ims-trn-opendc.component';
import { ImsTrnOpendccreateComponent } from './Component/ims-trn-opendccreate/ims-trn-opendccreate.component';
import { ImsTrnViewdcComponent } from './Component/ims-trn-viewdc/ims-trn-viewdc.component';
import { ImsTrnPurchasereturnSummaryComponent } from './Component/ims-trn-purchasereturn-summary/ims-trn-purchasereturn-summary.component';
import { ImsTrnPurchasereturnViewComponent } from './Component/ims-trn-purchasereturn-view/ims-trn-purchasereturn-view.component';
import { ImsTrnPurchasereturnAddselectComponent } from './Component/ims-trn-purchasereturn-addselect/ims-trn-purchasereturn-addselect.component';
import { ImsTrnPurchasereturnAddComponent } from './Component/ims-trn-purchasereturn-add/ims-trn-purchasereturn-add.component';
import { ImsTrnSalesreturnSummaryComponent } from './Component/ims-trn-salesreturn-summary/ims-trn-salesreturn-summary.component';
import { ImsTrnSalesreturnViewComponent } from './Component/ims-trn-salesreturn-view/ims-trn-salesreturn-view.component';
import { ImsTrnSalesreturnAddComponent } from './Component/ims-trn-salesreturn-add/ims-trn-salesreturn-add.component';
import { ImsTrnSalesreturnAddselectComponent } from './Component/ims-trn-salesreturn-addselect/ims-trn-salesreturn-addselect.component';
import { ImsTrnProductSplitComponent } from './Component/ims-trn-product-split/ims-trn-product-split.component';
import { ImsTrnStockstatementViewComponent } from './Component/ims-trn-stockstatement-view/ims-trn-stockstatement-view.component';
import { ImsMstProductSummaryComponent } from './Component/ims-mst-product-summary/ims-mst-product-summary.component';
import { ImsMstProductAddComponent } from './Component/ims-mst-product-add/ims-mst-product-add.component';
import { ImsMstProductViewComponent } from './Component/ims-mst-product-view/ims-mst-product-view.component';
import { ImsMstProductEditComponent } from './Component/ims-mst-product-edit/ims-mst-product-edit.component';
import { ImsTrnStocktransferSummaryComponent } from './Component/ims-trn-stocktransfer-summary/ims-trn-stocktransfer-summary.component';
import { ImsTrnStocktransferBranchComponent } from './Component/ims-trn-stocktransfer-branch/ims-trn-stocktransfer-branch.component';
import { ImsTrnStocktransferBranchwiseComponent } from './Component/ims-trn-stocktransfer-branchwise/ims-trn-stocktransfer-branchwise.component';
import { ImsTrnStocktrnasferbranchViewComponent } from './Component/ims-trn-stocktrnasferbranch-view/ims-trn-stocktrnasferbranch-view.component';
import { ImsTrnStocktransferLocationComponent } from './Component/ims-trn-stocktransfer-location/ims-trn-stocktransfer-location.component';
import { ImsTrnPurchasehistoryComponent } from './Component/ims-trn-purchasehistory/ims-trn-purchasehistory.component';
import { ImsTrnPurchaseHistoryviewComponent } from './Component/ims-trn-purchase-historyview/ims-trn-purchase-historyview.component';
import { ImsTrnSalesHistoryviewComponent } from './Component/ims-trn-sales-historyview/ims-trn-sales-historyview.component';
import { ImsTrnSalesHistoryComponent } from './Component/ims-trn-sales-history/ims-trn-sales-history.component';
import { ImsTrnStocktransferacknowledgementSummaryComponent } from './Component/ims-trn-stocktransferacknowledgement-summary/ims-trn-stocktransferacknowledgement-summary.component';
import { ImsTrnStocktransferacknowledgementViewComponent } from './Component/ims-trn-stocktransferacknowledgement-view/ims-trn-stocktransferacknowledgement-view.component';
import { ImsTrnStocktransferlocationViewComponent } from './Component/ims-trn-stocktransferlocation-view/ims-trn-stocktransferlocation-view.component';
import { ImsRptStocktransferReportComponent } from './Component/ims-rpt-stocktransfer-report/ims-rpt-stocktransfer-report.component';
import { ImsTrnStocktransferapprovalSummaryComponent } from './Component/ims-trn-stocktransferapproval-summary/ims-trn-stocktransferapproval-summary.component';
import { ImsTrnStocktransferapprovalviewComponent } from './Component/ims-trn-stocktransferapprovalview/ims-trn-stocktransferapprovalview.component';
import { ImsTrnStockconsumptionreportComponent } from './Component/ims-trn-stockconsumptionreport/ims-trn-stockconsumptionreport.component';
import { ImsRptStockagereportComponent } from './Component/ims-rpt-stockagereport/ims-rpt-stockagereport.component';
import { ImsRptMaterialtrackerReportComponent } from './Component/ims-rpt-materialtracker-report/ims-rpt-materialtracker-report.component';
import { ImsRptProductissueReportComponent } from './Component/ims-rpt-productissue-report/ims-rpt-productissue-report.component';
import { ImsRptStockstatusReportComponent } from './Component/ims-rpt-stockstatus-report/ims-rpt-stockstatus-report.component';
import { ImsRptClosingstockReportComponent } from './Component/ims-rpt-closingstock-report/ims-rpt-closingstock-report.component';
import { ImsRptMaterialissueReportComponent } from './Component/ims-rpt-materialissue-report/ims-rpt-materialissue-report.component';
import { ImsTrnIssueComponent } from './Component/ims-trn-issue/ims-trn-issue.component';
import { ImsRptHighcostComponent } from './Component/ims-rpt-highcost/ims-rpt-highcost.component';
import { ImsTrnIssuematerialComponent } from './Component/ims-trn-issuematerial/ims-trn-issuematerial.component';
import { ImsRptGrnreportComponent } from './Component/ims-rpt-grnreport/ims-rpt-grnreport.component'
import { ImsRptGrndetailreportComponent } from './Component/ims-rpt-grndetailreport/ims-rpt-grndetailreport.component';
import { ImsTrnStatustrackComponent } from './Component/ims-trn-statustrack/ims-trn-statustrack.component';
import { ImsTrnReorderlevelComponent } from './Component/ims-trn-reorderlevel/ims-trn-reorderlevel.component';
import { ImsTrnRolsettingsComponent } from './Component/ims-trn-rolsettings/ims-trn-rolsettings.component';
import { ImsTrnMIapprovalComponent } from './Component/ims-trn-miapproval/ims-trn-miapproval.component';
import { ImsTrnMIapprovalreviewComponent } from './Component/ims-trn-miapprovalreview/ims-trn-miapprovalreview.component';
import { ImsTrnPendingmaterialissueComponent } from './Component/ims-trn-pendingmaterialissue/ims-trn-pendingmaterialissue.component';
import { ImsTrnRaisematerialindentComponent } from './Component/ims-trn-raisematerialindent/ims-trn-raisematerialindent.component';
import { ImsTrnMrpriceassignSummaryComponent } from './Component/ims-trn-mrpriceassign-summary/ims-trn-mrpriceassign-summary.component';
import { ImsTrnMrpriceassignViewComponent } from './Component/ims-trn-mrpriceassign-view/ims-trn-mrpriceassign-view.component';
import { ImsTrnRequestedissueComponent } from './Component/ims-trn-requestedissue/ims-trn-requestedissue.component';
import { ImsMstReorderleveladdComponent } from './Component/ims-mst-reorderleveladd/ims-mst-reorderleveladd.component';
import { ImsTrnReorderleveleditComponent } from './Component/ims-trn-reorderleveledit/ims-trn-reorderleveledit.component';
import { ImsTrnStockregularizationComponent } from './Component/ims-trn-stockregularization/ims-trn-stockregularization.component';
import { ImsTrnDeliveryviewComponent } from './Component/ims-trn-deliveryview/ims-trn-deliveryview.component';
import { ImsRptStockmovementComponent } from './Component/ims-rpt-stockmovement/ims-rpt-stockmovement.component';
import { ImsTrnStorerequisitionComponent } from './Component/ims-trn-storerequisition/ims-trn-storerequisition.component';
import { ImsTrnStorerequisitionaddComponent } from './Component/ims-trn-storerequisitionadd/ims-trn-storerequisitionadd.component';
import { ImsTrnStorerequisitionviewComponent } from './Component/ims-trn-storerequisitionview/ims-trn-storerequisitionview.component';
import { ImsRptMovementviewComponent } from './Component/ims-rpt-movementview/ims-rpt-movementview.component';


@NgModule({
  declarations: [
    ImsTrnOpeningstockSummaryComponent,
    ImsTrnIssuematerialSummaryComponent,
    ImsTrnStockadjustmentSummaryComponent,
    ImsTrnDeliveryorderComponent,
    ImsTrnAddDeliveryorderComponent,
    ImsTrnRaiseDeliveryorderComponent,
    ImsTrnOpeningstockAddComponent,
    ImsTrnOpeningstockAddComponent,
    ImsTrnOpeningstockAddComponent,
    ImsTrnOpeningstockEditComponent,
    ImsTrnDeliveryAcknowledgementComponent,
    ImsTrnDeliveryacknowlegdementAddComponent,
    ImsTrnDeliveryacknowledgementUpdateComponent,
    ImsTrnOpendcsummaryComponent,
    ImsTrnOpendcAddselectComponent,
    ImsTrnOpendcaddselectUpdateComponent,
    ImsRptStockreportComponent,
     ImsTrnDespatchViewComponent,
      ImsTrnDeliveryorderViewComponent, 
      ImsTrnAddstockComponent,
      ImsTrnStocksummaryComponent,
      ImsTrnStockamendComponent,
      ImsTrnAddamendstockComponent,
      ImsTrnStockdamageComponent,
      ImsTrnAddDamagedstockComponent,
      ImsTrnDirectissuematerialComponent,
      ImsTrnMaterialindentSummaryComponent,
      ImsTrnMaterialindentAddComponent,
      ImsTrnIssuematerialViewComponent,
      ImsTrnMaterialindentIssueComponent,
      ImsTrnMaterialindentViewComponent,
      ImsMstLocationComponent,
      ImsMstAssignproductComponent,
      ImsTrnWarrantytrackerComponent,
      ImsTrnStockstatementComponent,
      ImsTrnOpendcComponent,
      ImsTrnOpendccreateComponent,
      ImsTrnViewdcComponent,
      ImsTrnPurchasereturnSummaryComponent,
      ImsTrnPurchasereturnViewComponent,
      ImsTrnPurchasereturnAddselectComponent,
      ImsTrnPurchasereturnAddComponent,
      ImsTrnSalesreturnSummaryComponent,
      ImsTrnSalesreturnViewComponent,
      ImsTrnSalesreturnAddComponent,
      ImsTrnSalesreturnAddselectComponent,
      ImsTrnProductSplitComponent,
      ImsTrnStockstatementViewComponent,
      ImsTrnWarrantytrackerComponent,
      ImsMstProductSummaryComponent,
      ImsMstProductAddComponent,
      ImsMstProductViewComponent,
      ImsMstProductEditComponent,
      ImsTrnStocktransferSummaryComponent,
      ImsTrnStocktransferBranchComponent,
      ImsTrnStocktransferBranchwiseComponent,
      ImsTrnStocktrnasferbranchViewComponent,
      ImsTrnStocktransferLocationComponent,
      ImsTrnPurchasehistoryComponent,
      ImsTrnPurchaseHistoryviewComponent,
      ImsTrnSalesHistoryviewComponent,
      ImsTrnSalesHistoryComponent,
      ImsTrnStocktransferlocationViewComponent,
      ImsRptStocktransferReportComponent,
      ImsTrnStocktransferapprovalSummaryComponent,
      ImsTrnStocktransferapprovalviewComponent,
      ImsTrnStocktransferacknowledgementSummaryComponent,
      ImsTrnStocktransferacknowledgementViewComponent,
      ImsTrnStockconsumptionreportComponent,
      ImsRptStockagereportComponent,
      ImsRptMaterialtrackerReportComponent,
      ImsRptProductissueReportComponent,
      ImsRptStockstatusReportComponent,
      ImsRptClosingstockReportComponent,
      ImsRptMaterialissueReportComponent,
      ImsTrnIssueComponent,
      ImsRptHighcostComponent,
      ImsTrnIssuematerialComponent,
      ImsRptGrnreportComponent,
      ImsRptGrndetailreportComponent,
      ImsTrnStatustrackComponent,
      ImsTrnReorderlevelComponent,
      ImsTrnRolsettingsComponent,
      ImsTrnMIapprovalComponent,
      ImsTrnMIapprovalreviewComponent,
      ImsTrnPendingmaterialissueComponent,
      ImsTrnRaisematerialindentComponent,
      ImsTrnMrpriceassignViewComponent,
      ImsTrnMrpriceassignSummaryComponent,
      ImsTrnRequestedissueComponent,
      ImsMstReorderleveladdComponent,
      ImsTrnReorderleveleditComponent,
      ImsTrnStockregularizationComponent,
      ImsTrnDeliveryviewComponent,
      ImsRptStockmovementComponent,
      ImsTrnStorerequisitionComponent,
      ImsTrnStorerequisitionaddComponent,
      ImsTrnStorerequisitionviewComponent,
      ImsRptMovementviewComponent,
  ],
      
  imports: [
    CommonModule,
    EmsInventoryRoutingModule,NgApexchartsModule,
    AngularEditorModule,NgSelectModule,
    ReactiveFormsModule,FormsModule,
    NgbModule
  ]
})
export class EmsInventoryModule { }
