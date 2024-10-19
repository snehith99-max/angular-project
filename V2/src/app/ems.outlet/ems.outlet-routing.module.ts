import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OtlMstRevenuecategorysummaryComponent } from './Component/otl-mst-revenuecategorysummary/otl-mst-revenuecategorysummary.component';
import { OtlMstExpensecategorysummaryComponent } from './Component/otl-mst-expensecategorysummary/otl-mst-expensecategorysummary.component';
import { OtlMstDaytrackersummaryComponent } from './Component/otl-mst-daytrackersummary/otl-mst-daytrackersummary.component';
import { OtlMstDaytrackeraddComponent } from './Component/otl-mst-daytrackeradd/otl-mst-daytrackeradd.component';
import { OtlTrnOutletmanageComponent } from './Component/otl-trn-outletmanage/otl-trn-outletmanage.component';
import { OtlMstDaytrackereditComponent } from './Component/otl-mst-daytrackeredit/otl-mst-daytrackeredit.component';
import { OtlMstDaytrackerviewComponent } from './Component/otl-mst-daytrackerview/otl-mst-daytrackerview.component';
import { OtlRptOutletdaytrackerreportComponent } from './Component/otl-rpt-outletdaytrackerreport/otl-rpt-outletdaytrackerreport.component';
import { OtlTrnToutletmanagerComponent } from './Component/otl-trn-toutletmanager/otl-trn-toutletmanager.component';
import { OtlTrnToutletmanagerviewComponent } from './Component/otl-trn-toutletmanagerview/otl-trn-toutletmanagerview.component';
import { OtlTrnMaterialindentComponent } from './Component/otl-trn-materialindent/otl-trn-materialindent.component';
import { OtlTrnPurchaseindentComponent } from './Component/otl-trn-purchaseindent/otl-trn-purchaseindent.component';
import { OtlTrnRaisematerialindentComponent } from './Component/otl-trn-raisematerialindent/otl-trn-raisematerialindent.component';
import { OtlTrnMaterialindentViewComponent } from './Component/otl-trn-materialindent-view/otl-trn-materialindent-view.component';
import { OtlMstProductAddComponent } from './Component/otl-mst-product-add/otl-mst-product-add.component';
import { OtlMstPincodeComponent } from './Component/otl-mst-pincode/otl-mst-pincode.component';
import { OtlMstUsermanagementComponent } from './Component/otl-mst-usermanagement/otl-mst-usermanagement.component';
import { OtlMstUsereaddComponent } from './Component/otl-mst-usereadd/otl-mst-usereadd.component';
import { OtlMstUsereditComponent } from './Component/otl-mst-useredit/otl-mst-useredit.component';
import { OtlMstUserviewComponent } from './Component/otl-mst-userview/otl-mst-userview.component';
import { OtlMstDeliverycostmappingComponent } from './Component/otl-mst-deliverycostmapping/otl-mst-deliverycostmapping.component';
import { OtlMstProductComponent } from './Component/otl-mst-product/otl-mst-product.component';
import { OtlMstProductEditComponent } from './Component/otl-mst-product-edit/otl-mst-product-edit.component';
import { OtlMstProductViewComponent } from './Component/otl-mst-product-view/otl-mst-product-view.component';
import { OtlMstBranchComponent } from './Component/otl-mst-branch/otl-mst-branch.component';
import { OtlMstAssignProductComponent } from './Component/otl-mst-assign-product/otl-mst-assign-product.component';
import { OtlMstAmendproductComponent } from './Component/otl-mst-amendproduct/otl-mst-amendproduct.component';
import { OtlMstAssignpincodeComponent } from './Component/otl-mst-assignpincode/otl-mst-assignpincode.component';
import { OtlTrnChangepriceComponent } from './Component/otl-trn-changeprice/otl-trn-changeprice.component';
import { OtlTrnUnassignProductComponent } from './Component/otl-trn-unassign-product/otl-trn-unassign-product.component';
import { OtlMstRemovepincodeComponent } from './Component/otl-mst-removepincode/otl-mst-removepincode.component';
import { OtlMstDeliverypincodeAssignComponent } from './Component/otl-mst-deliverypincode-assign/otl-mst-deliverypincode-assign.component';
import { OtlTrnWhatsapporderComponent } from './Component/otl-trn-whatsapporder/otl-trn-whatsapporder.component';
import { OtlTrnWhatsapporderviewComponent } from './Component/otl-trn-whatsapporderview/otl-trn-whatsapporderview.component';
import { OtlTrnOutletdirectpoComponent } from './Component/otl-trn-outletdirectpo/otl-trn-outletdirectpo.component';
import { OtlTrnOutletmanageAssignComponent } from './Component/otl-trn-outletmanage-assign/otl-trn-outletmanage-assign.component';
import { OtlTrnOutletmanageUnassignComponent } from './Component/otl-trn-outletmanage-unassign/otl-trn-outletmanage-unassign.component';
import { OtlTrnOutletmanageEmployeeassignComponent } from './Component/otl-trn-outletmanage-employeeassign/otl-trn-outletmanage-employeeassign.component';
import { OtlTrnOutletmanageEmployeeunassignComponent } from './Component/otl-trn-outletmanage-employeeunassign/otl-trn-outletmanage-employeeunassign.component';
import { OtlTrnTradedaytrackerComponent } from './Component/otl-trn-tradedaytracker/otl-trn-tradedaytracker.component';
import { OtlTrnTraisepurchaseindentComponent } from './Component/otl-trn-traisepurchaseindent/otl-trn-traisepurchaseindent.component';
import { OtlTrnPurchaseindentviewComponent } from './Component/otl-trn-purchaseindentview/otl-trn-purchaseindentview.component';

const routes: Routes = [
  { path: 'otlmstrevenuecategory', component: OtlMstRevenuecategorysummaryComponent },
  { path: 'otlmstexpensecategory', component: OtlMstExpensecategorysummaryComponent },
  { path: 'otlmstdaytrackersummary', component: OtlMstDaytrackersummaryComponent },
  { path: 'otlmstdaytrackeradd', component: OtlMstDaytrackeraddComponent },
  { path: 'otltrnoutletmanage', component: OtlTrnOutletmanageComponent },
  { path: 'otlmstdaytrackeredit/:daytracker_gid', component: OtlMstDaytrackereditComponent },
  { path: 'otlmstdaytrackerview/:daytracker_gid', component: OtlMstDaytrackerviewComponent },
  { path: 'otlrptoutletdaytrackerreport', component: OtlRptOutletdaytrackerreportComponent },
  { path: 'otltrnoutletmanager', component: OtlTrnToutletmanagerComponent },
  { path: 'otltrnoutletmanagerview/:daytracker_gid', component: OtlTrnToutletmanagerviewComponent },
  { path: 'OtlTrnMaterialindent', component: OtlTrnMaterialindentComponent },
  { path: 'OtlTrnPurchaseindent', component: OtlTrnPurchaseindentComponent },
  { path: 'OtlTrnMaterialIndentView/:materialrequisition_gid', component: OtlTrnMaterialindentViewComponent },
  { path: 'OtlMstUser', component: OtlMstUsermanagementComponent },
  { path: 'OtlMstUserAdd', component: OtlMstUsereaddComponent },
  { path: 'OtlMstUserEdit/:employee_gid', component: OtlMstUsereditComponent },
  { path: 'OtlMstUserView/:employee_gid', component: OtlMstUserviewComponent },
  { path: 'OtlMstPincode', component: OtlMstPincodeComponent },
  { path: 'OtlMstDeliveryCostMapping', component: OtlMstDeliverycostmappingComponent },
  { path: 'OtlMstProduct', component: OtlMstProductComponent },
  { path: 'OtlMstProductAdd', component: OtlMstProductAddComponent },
  { path: 'OtlMstProductEdit/:product_gid', component: OtlMstProductEditComponent },
  { path: 'OtlMstProductview/:product_gid', component: OtlMstProductViewComponent },
  { path: 'OtlMstUser', component: OtlMstUsermanagementComponent },
  { path: 'OtlMstUserAdd', component: OtlMstUsereaddComponent },
  { path: 'OtlMstUserEdit/:employee_gid', component: OtlMstUsereditComponent },
  { path: 'OtlMstUserView/:employee_gid', component: OtlMstUserviewComponent },
  { path: 'OtlMstPincode', component: OtlMstPincodeComponent },
  { path: 'OtlMstDeliveryCostMapping', component: OtlMstDeliverycostmappingComponent },
  { path: 'OtlMstPriceManagement', component: OtlMstBranchComponent },
  { path: 'OtlMstAssignProduct/:campaign_gid', component: OtlMstAssignProductComponent },
  { path: 'OtlMstAmendProduct/:branch_gid1', component: OtlMstAmendproductComponent },
  { path: 'OtlTrnUnassignProduct/:campaign_gid', component: OtlTrnUnassignProductComponent },
  { path: 'OtlTrnChangeprice/:campaign_gid', component: OtlTrnChangepriceComponent },
  { path: 'OtlMstAssignPincode/:branch_gid1', component: OtlMstAssignpincodeComponent },
  { path: 'OtlMstRemovePincode/:branch_gid1', component: OtlMstRemovepincodeComponent },
    { path: 'OtlMstDeliveryAssignPincode/:deliverycost_id1', component: OtlMstDeliverypincodeAssignComponent },
    { path: 'OtlTrnWhatsAppOrderSummary', component: OtlTrnWhatsapporderComponent },
    { path: 'OtlTrnWhatsapporderview/:kot_gid', component: OtlTrnWhatsapporderviewComponent },
    { path: 'OtlTrnOutletdirectpo', component: OtlTrnOutletdirectpoComponent },
    { path: 'OtlTrnRaisematerialindent', component: OtlTrnRaisematerialindentComponent },
    { path: 'OtlTrn', component: OtlTrnRaisematerialindentComponent },
    { path: 'OtlTrnOutletManageAssign/:campaign_gid', component: OtlTrnOutletmanageAssignComponent },
    { path: 'OtlTrnOutletManageUnassign/:campaign_gid', component:OtlTrnOutletmanageUnassignComponent },
    { path: 'OtlTrnOutletManageEmployeeAssign/:campaign_gid', component:OtlTrnOutletmanageEmployeeassignComponent },
    { path: 'OtlTrnOutletManageEmployeeUnassign/:campaign_gid', component:OtlTrnOutletmanageEmployeeunassignComponent},
    { path: 'OtlTrnTradedaytracker/:balance_date', component:OtlTrnTradedaytrackerComponent},
    { path:'OtlTrnTraisepurchaseindent',component:OtlTrnTraisepurchaseindentComponent},
    { path:'OtlTrnPurchaseindentview/:purchaserequisition_gid',component:OtlTrnPurchaseindentviewComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmsOutletRoutingModule { }
