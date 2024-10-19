import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SbcTrnSubscriptionsummaryComponent } from './sbc-trn-subscriptionsummary/sbc-trn-subscriptionsummary.component';
import { SbcMstServersummaryComponent } from './sbc-mst-serversummary/sbc-mst-serversummary.component';
import { SbcMstConsumersummaryComponent } from './sbc-mst-consumersummary/sbc-mst-consumersummary.component';
import { SbcMstScriptmanagementComponent } from './sbc-mst-scriptmanagement/sbc-mst-scriptmanagement.component';
import { SbcMstDynamicdbcreationComponent } from './sbc-mst-dynamicdbcreation/sbc-mst-dynamicdbcreation.component';
import { SbcMstScriptmanagementviewComponent } from './sbc-mst-scriptmanagementview/sbc-mst-scriptmanagementview.component';
import { SbcMstScriptmanagementexceptionviewComponent } from './sbc-mst-scriptmanagementexceptionview/sbc-mst-scriptmanagementexceptionview.component';
import { SbcMstProductmoduleComponent } from './sbc-mst-productmodule/sbc-mst-productmodule.component';
import { SbcMstDynamicdbexceptionerrorviewComponent } from './sbc-mst-dynamicdbexceptionerrorview/sbc-mst-dynamicdbexceptionerrorview.component';
import { SbcMstDashboardComponent } from './sbc-mst-dashboard/sbc-mst-dashboard.component';
import { SbcMstHelpandsupportComponent } from './sbc-mst-helpandsupport/sbc-mst-helpandsupport.component';

const routes: Routes = [
  { path: 'SbcTrnSubscriptionsummary', component: SbcTrnSubscriptionsummaryComponent },
  { path: 'SbcMstServerSummary', component: SbcMstServersummaryComponent },
  { path: 'SbcMstConsumerSummary', component: SbcMstConsumersummaryComponent },
  { path: 'SbcMstScriptManagement', component:  SbcMstScriptmanagementComponent},
  { path: 'SbcMstDynamicdbcreation', component:  SbcMstDynamicdbcreationComponent},
  { path: 'SbcMstScriptmanagementview/:dbscriptmanagementdocument_gid', component:  SbcMstScriptmanagementviewComponent},
  { path: 'SbcMstScriptmanagementexceptionview/:dbscriptmanagementdocument_gid', component:  SbcMstScriptmanagementexceptionviewComponent},
  { path: 'SbcMstProductmodule', component:  SbcMstProductmoduleComponent},
  { path: 'SbcMstDynamicdbexceptionerrorview/:dynamicdbscriptmanagement_gid', component:  SbcMstDynamicdbexceptionerrorviewComponent},
  { path: 'SbcMstDashboard', component:  SbcMstDashboardComponent},
  { path: 'SbcMstHelpandsupport', component:  SbcMstHelpandsupportComponent},





];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmsSubscriptionRoutingModule { }
