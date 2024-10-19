import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ImsTrnOpeningstockSummaryComponent } from './Component/ims-trn-openingstock-summary/ims-trn-openingstock-summary.component';
import { ImsTrnIssuematerialSummaryComponent } from './Component/ims-trn-issuematerial-summary/ims-trn-issuematerial-summary.component';
import { ImsTrnStockadjustmentSummaryComponent } from './Component/ims-trn-stockadjustment-summary/ims-trn-stockadjustment-summary.component';
import { ImsTrnDeliveryorderComponent } from './Component/ims-trn-deliveryorder/ims-trn-deliveryorder.component';
import { ImsTrnRaiseDeliveryorderComponent } from './Component/ims-trn-raise-deliveryorder/ims-trn-raise-deliveryorder.component';
import { ImsTrnAddDeliveryorderComponent } from './Component/ims-trn-add-deliveryorder/ims-trn-add-deliveryorder.component';
import { ImsTrnOpeningstockAddComponent } from './Component/ims-trn-openingstock-add/ims-trn-openingstock-add.component';
import { ImsTrnDeliveryAcknowledgementComponent } from './Component/ims-trn-delivery-acknowledgement/ims-trn-delivery-acknowledgement.component';
import { ImsTrnDeliveryacknowlegdementAddComponent } from './Component/ims-trn-deliveryacknowlegdement-add/ims-trn-deliveryacknowlegdement-add.component';
import { ImsTrnOpeningstockEditComponent } from './Component/ims-trn-openingstock-edit/ims-trn-openingstock-edit.component';
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
import { ImsTrnPurchasereturnSummaryComponent } from './Component/ims-trn-purchasereturn-summary/ims-trn-purchasereturn-summary.component';
import { ImsTrnPurchasereturnViewComponent } from './Component/ims-trn-purchasereturn-view/ims-trn-purchasereturn-view.component';
import { ImsTrnPurchasereturnAddselectComponent } from './Component/ims-trn-purchasereturn-addselect/ims-trn-purchasereturn-addselect.component';
import { ImsTrnPurchasereturnAddComponent } from './Component/ims-trn-purchasereturn-add/ims-trn-purchasereturn-add.component';
import { ImsTrnOpendcComponent } from './Component/ims-trn-opendc/ims-trn-opendc.component';
import { ImsTrnOpendccreateComponent } from './Component/ims-trn-opendccreate/ims-trn-opendccreate.component';
import { ImsTrnViewdcComponent } from './Component/ims-trn-viewdc/ims-trn-viewdc.component';
import { ImsTrnStockstatementComponent } from './Component/ims-trn-stockstatement/ims-trn-stockstatement.component';
import { ImsTrnSalesreturnSummaryComponent } from './Component/ims-trn-salesreturn-summary/ims-trn-salesreturn-summary.component';
import { ImsTrnSalesreturnViewComponent } from './Component/ims-trn-salesreturn-view/ims-trn-salesreturn-view.component';
import { ImsTrnSalesreturnAddComponent } from './Component/ims-trn-salesreturn-add/ims-trn-salesreturn-add.component';
import { ImsTrnSalesreturnAddselectComponent } from './Component/ims-trn-salesreturn-addselect/ims-trn-salesreturn-addselect.component';
import { ImsTrnProductSplitComponent } from './Component/ims-trn-product-split/ims-trn-product-split.component';


import { ImsTrnStockstatementViewComponent } from './Component/ims-trn-stockstatement-view/ims-trn-stockstatement-view.component';
import { ImsTrnWarrantytrackerComponent } from './Component/ims-trn-warrantytracker/ims-trn-warrantytracker.component';
import { ImsMstProductSummaryComponent } from './Component/ims-mst-product-summary/ims-mst-product-summary.component';
import { ImsMstProductAddComponent } from './Component/ims-mst-product-add/ims-mst-product-add.component';
import { ImsMstProductViewComponent } from './Component/ims-mst-product-view/ims-mst-product-view.component';
import { ImsMstProductEditComponent } from './Component/ims-mst-product-edit/ims-mst-product-edit.component';
import { ImsTrnStocktransferSummaryComponent } from './Component/ims-trn-stocktransfer-summary/ims-trn-stocktransfer-summary.component';
import { ImsTrnStocktransferBranchComponent } from './Component/ims-trn-stocktransfer-branch/ims-trn-stocktransfer-branch.component';
import { ImsTrnStocktransferBranchwiseComponent } from './Component/ims-trn-stocktransfer-branchwise/ims-trn-stocktransfer-branchwise.component';
import { ImsTrnStocktrnasferbranchViewComponent } from './Component/ims-trn-stocktrnasferbranch-view/ims-trn-stocktrnasferbranch-view.component';
import { ImsTrnStocktransferLocationComponent } from './Component/ims-trn-stocktransfer-location/ims-trn-stocktransfer-location.component';
import { ImsTrnStocktransferlocationViewComponent } from './Component/ims-trn-stocktransferlocation-view/ims-trn-stocktransferlocation-view.component';
import { ImsTrnPurchasehistoryComponent } from './Component/ims-trn-purchasehistory/ims-trn-purchasehistory.component';
import { ImsTrnSalesHistoryComponent } from './Component/ims-trn-sales-history/ims-trn-sales-history.component';
import { ImsTrnPurchaseHistoryviewComponent } from './Component/ims-trn-purchase-historyview/ims-trn-purchase-historyview.component';
import { ImsTrnSalesHistoryviewComponent } from './Component/ims-trn-sales-historyview/ims-trn-sales-historyview.component';
import { ImsTrnStocktransferacknowledgementSummaryComponent } from './Component/ims-trn-stocktransferacknowledgement-summary/ims-trn-stocktransferacknowledgement-summary.component';
import { ImsTrnStocktransferacknowledgementViewComponent } from './Component/ims-trn-stocktransferacknowledgement-view/ims-trn-stocktransferacknowledgement-view.component';
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
const routes: Routes = [
  { path: 'ImsTrnOpeningstockSummary', component: ImsTrnOpeningstockSummaryComponent },
  { path: 'ImsTrnIssuematerialSummary', component: ImsTrnIssuematerialSummaryComponent },
  { path: 'ImsTrnStockadjustmentSummary', component: ImsTrnStockadjustmentSummaryComponent },
  { path: 'ImsTrnRaiseDeliveryorder/:salesorder_gid', component: ImsTrnRaiseDeliveryorderComponent },
  { path: 'ImsTrnDeliveryorder', component: ImsTrnDeliveryorderComponent },
  { path: 'ImsTrnAddDeliveryorder', component: ImsTrnAddDeliveryorderComponent },
  { path: 'ImsTrnOpeningstockAdd', component: ImsTrnOpeningstockAddComponent },
  { path: 'ImsTrnOpeningstockEdit/:stock_gid', component: ImsTrnOpeningstockEditComponent },
  { path: 'ImsTrnDeliveryOrderAcknowlegement', component: ImsTrnDeliveryAcknowledgementComponent },
  { path: 'ImsTrnDeliveryAcknowledgemantadd', component: ImsTrnDeliveryacknowlegdementAddComponent },
  { path: 'ImsTrnDeliveryacknowledgementUpdate/:directorder_gid', component: ImsTrnDeliveryacknowledgementUpdateComponent },
  { path: 'ImsTrnOpendcsummary', component: ImsTrnOpendcsummaryComponent },
  { path: 'ImsTrnOpendcAddselect', component: ImsTrnOpendcAddselectComponent },
  { path: 'ImsTrnOpendcaddselectUpdate/:salesorder_gid', component: ImsTrnOpendcaddselectUpdateComponent },
  { path: 'ImsRptStockreport', component: ImsRptStockreportComponent },
  { path: 'ImsTrnDespatchView/:salesorder_gid', component: ImsTrnDespatchViewComponent },
  { path: 'ImsTrnDeliveryorderView/:directorder_gid', component: ImsTrnDeliveryorderViewComponent },
  { path: 'ImsTrnStocksummary', component: ImsTrnStocksummaryComponent },
  { path: 'ImsTrnAddstock', component: ImsTrnAddstockComponent },
  { path: 'ImsTrnStockamend/:product_gid/:uom_gid/:branch_gid/:stock_gid', component: ImsTrnStockamendComponent },
  { path: 'ImsTrnAddamendstock/:stock_gid', component: ImsTrnAddamendstockComponent },
  { path: 'ImsTrnStockdamage/:product_gid/:uom_gid/:branch_gid/:stock_gid', component: ImsTrnStockdamageComponent },
  { path: 'ImsTrnAddDamagedstock/:stock_gid', component: ImsTrnAddDamagedstockComponent },
  { path: 'ImsTrnDirectissuematerial', component: ImsTrnDirectissuematerialComponent },
  { path: 'ImsTrnMaterialindent', component: ImsTrnMaterialindentSummaryComponent },
  { path: 'ImsTrnMaterialindentAdd', component: ImsTrnMaterialindentAddComponent },
  { path: 'ImsTrnIssueMaterialView/:materialissued_gid/:lspage', component: ImsTrnIssuematerialViewComponent },
  { path: 'ImsTrnMaterialindentIssue/:materialrequisition_gid', component: ImsTrnMaterialindentIssueComponent },
  { path: 'ImsTrnMaterialIndentView/:materialrequisition_gid', component: ImsTrnMaterialindentViewComponent },
  { path: 'ImsMstLocation', component: ImsMstLocationComponent },
  { path: 'ImsMstAssignproduct', component: ImsMstAssignproductComponent },
  { path: 'ImsTrnStockstatement', component: ImsTrnStockstatementComponent },
  { path: 'ImsTrnPurchaseReturns', component: ImsTrnPurchasereturnSummaryComponent },
  { path: 'ImsTrnPurchaseReturnView/:purchasereturngid', component: ImsTrnPurchasereturnViewComponent },
  { path: 'ImsTrnPurchaseReturnAddSelect', component: ImsTrnPurchasereturnAddselectComponent },
  { path: 'ImsTrnPurchaseReturnAdd/:grngid/:vendorgid/:lspage', component: ImsTrnPurchasereturnAddComponent },
  { path: 'ImsTrnOpendc', component: ImsTrnOpendcComponent },
  { path: 'ImsTrnOpendccreate', component: ImsTrnOpendccreateComponent },
  { path: 'ImsTrnViewdc/:directorder_gid', component: ImsTrnViewdcComponent },
  { path: 'ImsTrnSalesReturnSummary', component: ImsTrnSalesreturnSummaryComponent },
  { path: 'ImsTrnSalesReturnView/:salesreturngid', component: ImsTrnSalesreturnViewComponent },
  { path: 'ImsTrnSalesReturnAdd', component: ImsTrnSalesreturnAddComponent },
  { path: 'ImsTrnSalesReturnAddSelect/:directordergid', component: ImsTrnSalesreturnAddselectComponent },
  { path: 'ImsTrnStockStatementView/:productgid/:branchgid', component: ImsTrnStockstatementViewComponent },
  { path: 'ImsTrnProductSplit/:product_gid', component: ImsTrnProductSplitComponent },
  { path: 'ImsTrnExpiretracker', component: ImsTrnWarrantytrackerComponent },
  { path:'ImsMstProductSummary',component:ImsMstProductSummaryComponent},
  { path:'ImsMstProductAdd',component:ImsMstProductAddComponent},
  { path:'ImsMstProductView/:product_gid',component:ImsMstProductViewComponent},
  { path:'ImsMstProductEdit/:product_gid',component:ImsMstProductEditComponent},
  { path:'ImsTrnStockTransfer',component:ImsTrnStocktransferSummaryComponent},
  { path:'ImsTrnStockTransferBranch',component:ImsTrnStocktransferBranchComponent},
  { path:'ImsTrnStockTransferBranchWise/:stock_gid',component:ImsTrnStocktransferBranchwiseComponent},
  { path:'ImsTrnStockTransferBranchView/:stocktransfer_gid',component:ImsTrnStocktrnasferbranchViewComponent},
  { path:'ImsTrnStockTransferLocation',component:ImsTrnStocktransferLocationComponent},
  { path:'ImsTrnStockTransferLocationView/:stocktransfer_gid',component:ImsTrnStocktransferlocationViewComponent},
  { path: 'ImsTrnPurchasehistory/:vendorgid/:productgid', component: ImsTrnPurchasehistoryComponent },
  { path: 'ImsTrnSalesHistory/:customergid/:productgid', component: ImsTrnSalesHistoryComponent },
  { path:'ImsTrnPurchaseHistoryview/:purchaseorder_gid',component:ImsTrnPurchaseHistoryviewComponent},
  { path:'ImsTrnSalesHistoryview/:invoice_gid',component:ImsTrnSalesHistoryviewComponent},
  { path:'ImsTrnStockTransferAcknowledgementSummary',component:ImsTrnStocktransferacknowledgementSummaryComponent},
  { path:'ImsTrnStockTransferAcknowledgementView/:stocktransfer_gid',component:ImsTrnStocktransferacknowledgementViewComponent},
  { path:'ImsRptStockTransferReport',component:ImsRptStocktransferReportComponent},
  { path:'ImsTrnStockTransferApprovalSummary',component:ImsTrnStocktransferapprovalSummaryComponent},
  { path:'ImsTrnStockTransferApprovalView/:stocktransfer_gid',component:ImsTrnStocktransferapprovalviewComponent},
  { path:'ImsTrnStockConsumptionReport',component:ImsTrnStockconsumptionreportComponent},
  { path:'ImsRptStockAgeReport',component:ImsRptStockagereportComponent},
  { path:'ImsRptMaterialTracker',component:ImsRptMaterialtrackerReportComponent},
  { path:'ImsRptProductIssueReport',component:ImsRptProductissueReportComponent},
  { path:'ImsRptStockStatusReport',component:ImsRptStockstatusReportComponent},
  { path:'ImsRptClosingStockReport',component:ImsRptClosingstockReportComponent},
  { path:'ImsRptMaterialIssueReport',component:ImsRptMaterialissueReportComponent},
  { path:'ImsTrnIssue',component:ImsTrnIssueComponent},
  { path:'ImsRptHighcost',component:ImsRptHighcostComponent},
  { path:'ImsTrnIssuematerial/:materialrequisition_gid/:lspage',component:ImsTrnIssuematerialComponent},
  { path:'ImsRptGRNReport',component:ImsRptGrnreportComponent},
  { path:'ImsRptGRNDetailReport',component:ImsRptGrndetailreportComponent},
  { path:'ImsTrnStatustrack/:materialrequisition_gid',component:ImsTrnStatustrackComponent},
  { path:'ImsTrnReorderlevel',component:ImsTrnReorderlevelComponent},
  { path:'ImsTrnRolsettings',component:ImsTrnRolsettingsComponent},
  { path:'ImsTrnMIapproval',component:ImsTrnMIapprovalComponent},
  { path:'ImsTrnMIapprovalreview/:materialrequisition_gid',component:ImsTrnMIapprovalreviewComponent},
  { path:'ImsTrnPendingMaterialIssue',component:ImsTrnPendingmaterialissueComponent},
  { path:'ImsTrnRaiseMaterialIndent/:materialrequisition_gid',component:ImsTrnRaisematerialindentComponent},
  { path: 'ImsTrnIndentPriceEstimation', component: ImsTrnMrpriceassignSummaryComponent},
  { path: 'ImsTrnIndentPriceEstimationView/:materialrequisition_gid', component: ImsTrnMrpriceassignViewComponent},
  { path: 'ImsTrnRequestedissue/:materialrequisition_gid', component: ImsTrnRequestedissueComponent},
  { path: 'ImsMstReorderLevelAdd', component: ImsMstReorderleveladdComponent},
  { path: 'ImsTrnReorderLevelEdit/:rol_gid', component: ImsTrnReorderleveleditComponent },
  { path: 'ImsTrnStockRegularization', component: ImsTrnStockregularizationComponent },
  { path: 'ImsTrnDeliveryview/:directorder_gid', component: ImsTrnDeliveryviewComponent },
  { path: 'ImsRptStockmovement', component: ImsRptStockmovementComponent },
  { path: 'ImsTrnStorerequisition', component: ImsTrnStorerequisitionComponent },
  { path: 'ImsTrnStorerequisitionadd', component: ImsTrnStorerequisitionaddComponent },
  { path: 'ImsTrnStorerequisitionview/:storerequisition_gid', component: ImsTrnStorerequisitionviewComponent },
  { path: 'ImsRptMovementview/:productgid/:branchgid', component:ImsRptMovementviewComponent},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmsInventoryRoutingModule { }
