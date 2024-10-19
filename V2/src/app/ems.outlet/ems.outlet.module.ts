import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EmsUtilitiesModule } from '../ems.utilities/ems.utilities.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgApexchartsModule } from 'ng-apexcharts';
import { NgxIntlTelInputModule } from 'ngx-intl-tel-input';
import { DataTablesModule } from 'angular-datatables';
import { NgSelectModule } from '@ng-select/ng-select';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { EmsOutletRoutingModule } from './ems.outlet-routing.module';
import { OtlMstRevenuecategorysummaryComponent } from './Component/otl-mst-revenuecategorysummary/otl-mst-revenuecategorysummary.component';
import { OtlMstExpensecategorysummaryComponent } from './Component/otl-mst-expensecategorysummary/otl-mst-expensecategorysummary.component';
import { OtlMstDaytrackersummaryComponent } from './Component/otl-mst-daytrackersummary/otl-mst-daytrackersummary.component';
import { OtlMstDaytrackeraddComponent } from './Component/otl-mst-daytrackeradd/otl-mst-daytrackeradd.component';
import { OtlTrnOutletmanageComponent } from './Component/otl-trn-outletmanage/otl-trn-outletmanage.component';
import { DualListComponent } from './Component/otl-trn-outletmanage/dual-list/dual-list.component';
import { OutletManagerListComponent } from './Component/otl-trn-outletmanage/outlet-manager-list/outlet-manager-list.component';
import { OtlMstDaytrackereditComponent } from './Component/otl-mst-daytrackeredit/otl-mst-daytrackeredit.component';
import { OtlMstDaytrackerviewComponent } from './Component/otl-mst-daytrackerview/otl-mst-daytrackerview.component';
import { OtlRptOutletdaytrackerreportComponent } from './Component/otl-rpt-outletdaytrackerreport/otl-rpt-outletdaytrackerreport.component';
import { OtlTrnToutletmanagerComponent } from './Component/otl-trn-toutletmanager/otl-trn-toutletmanager.component';
import { OtlTrnToutletmanagerviewComponent } from './Component/otl-trn-toutletmanagerview/otl-trn-toutletmanagerview.component';
import { OtlTrnMaterialindentComponent } from './Component/otl-trn-materialindent/otl-trn-materialindent.component';
import { OtlTrnPurchaseindentComponent } from './Component/otl-trn-purchaseindent/otl-trn-purchaseindent.component';
import { OtlTrnRaisematerialindentComponent } from './Component/otl-trn-raisematerialindent/otl-trn-raisematerialindent.component';
import { OtlTrnMaterialindentViewComponent } from './Component/otl-trn-materialindent-view/otl-trn-materialindent-view.component';
import { OtlMstDeliverycostmappingComponent } from './Component/otl-mst-deliverycostmapping/otl-mst-deliverycostmapping.component';
import { OtlMstPincodeComponent } from './Component/otl-mst-pincode/otl-mst-pincode.component';
import { OtlMstUserviewComponent } from './Component/otl-mst-userview/otl-mst-userview.component';
import { OtlMstUsereditComponent } from './Component/otl-mst-useredit/otl-mst-useredit.component';
import { OtlMstUsereaddComponent } from './Component/otl-mst-usereadd/otl-mst-usereadd.component';
import { OtlMstUsermanagementComponent } from './Component/otl-mst-usermanagement/otl-mst-usermanagement.component';
import { OtlMstProductComponent } from './Component/otl-mst-product/otl-mst-product.component';
import { OtlMstProductEditComponent } from './Component/otl-mst-product-edit/otl-mst-product-edit.component';
import { OtlMstProductAddComponent } from './Component/otl-mst-product-add/otl-mst-product-add.component';
import { OtlMstBranchComponent } from './Component/otl-mst-branch/otl-mst-branch.component';
import { OtlMstAmendproductComponent } from './Component/otl-mst-amendproduct/otl-mst-amendproduct.component';
import { OtlMstRemovepincodeComponent } from './Component/otl-mst-removepincode/otl-mst-removepincode.component';
import { OtlMstAssignpincodeComponent } from './Component/otl-mst-assignpincode/otl-mst-assignpincode.component';
import { OtlTrnChangepriceComponent } from './Component/otl-trn-changeprice/otl-trn-changeprice.component';
import { OtlMstAssignProductComponent } from './Component/otl-mst-assign-product/otl-mst-assign-product.component';
import { OtlTrnUnassignProductComponent } from './Component/otl-trn-unassign-product/otl-trn-unassign-product.component';
import { OtlMstProductViewComponent } from './Component/otl-mst-product-view/otl-mst-product-view.component';
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






@NgModule({
  declarations: [
    OtlMstRevenuecategorysummaryComponent,
    OtlMstExpensecategorysummaryComponent,
    OtlMstDaytrackersummaryComponent,
    OtlMstDaytrackeraddComponent,
    OtlTrnOutletmanageComponent,
    OtlTrnUnassignProductComponent,
    OtlMstProductViewComponent,
    DualListComponent,
    OtlMstAssignProductComponent,
    OutletManagerListComponent,
    OtlMstDaytrackereditComponent,
    OtlMstDaytrackerviewComponent,
    OtlRptOutletdaytrackerreportComponent,
    OtlTrnToutletmanagerComponent,
    OtlTrnToutletmanagerviewComponent,
    OtlTrnMaterialindentComponent,
    OtlTrnPurchaseindentComponent,
    OtlTrnRaisematerialindentComponent,
    OtlTrnPurchaseindentComponent,
    OtlTrnMaterialindentViewComponent,
    OtlMstDeliverycostmappingComponent,
    OtlMstPincodeComponent,
    OtlMstUserviewComponent,
    OtlMstUsereditComponent,
    OtlMstUsereaddComponent,
    OtlMstUsermanagementComponent,
    OtlMstProductComponent,
    OtlMstProductEditComponent,
    OtlMstProductAddComponent,
    OtlMstBranchComponent,
    OtlMstAmendproductComponent,
    OtlMstRemovepincodeComponent,
    OtlMstAssignpincodeComponent,
    OtlTrnChangepriceComponent,
    OtlMstDeliverypincodeAssignComponent,
    OtlTrnWhatsapporderComponent,
    OtlTrnWhatsapporderviewComponent,
   OtlTrnOutletdirectpoComponent,
   OtlTrnOutletmanageAssignComponent,
   OtlTrnOutletmanageUnassignComponent,
   OtlTrnOutletmanageEmployeeassignComponent,
   OtlTrnOutletmanageEmployeeunassignComponent,
   OtlTrnTradedaytrackerComponent,
   OtlTrnTraisepurchaseindentComponent,
   OtlTrnPurchaseindentviewComponent,


  ],
  imports: [
    CommonModule,
    EmsOutletRoutingModule,
    FormsModule, ReactiveFormsModule,EmsUtilitiesModule,
    NgApexchartsModule,DataTablesModule,
    NgSelectModule,
    AngularEditorModule,
    TabsModule,
    NgxIntlTelInputModule,
    
  ]
})
export class EmsOutletModule { }
