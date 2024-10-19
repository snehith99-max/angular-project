import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AngularEditorModule } from '@kolkov/angular-editor';

import { ReactiveFormsModule } from '@angular/forms';
import { EmsSubscriptionRoutingModule } from './ems.subscription-routing.module';
import { SbcTrnSubscriptionsummaryComponent } from './sbc-trn-subscriptionsummary/sbc-trn-subscriptionsummary.component';
import { SbcMstServersummaryComponent } from './sbc-mst-serversummary/sbc-mst-serversummary.component';
import { SbcMstConsumersummaryComponent } from './sbc-mst-consumersummary/sbc-mst-consumersummary.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { SbcMstScriptmanagementComponent } from './sbc-mst-scriptmanagement/sbc-mst-scriptmanagement.component';
import { SbcMstDynamicdbcreationComponent } from './sbc-mst-dynamicdbcreation/sbc-mst-dynamicdbcreation.component';
import { SbcMstScriptmanagementviewComponent } from './sbc-mst-scriptmanagementview/sbc-mst-scriptmanagementview.component';
import { SbcMstScriptmanagementexceptionviewComponent } from './sbc-mst-scriptmanagementexceptionview/sbc-mst-scriptmanagementexceptionview.component';
import { SbcMstProductmoduleComponent } from './sbc-mst-productmodule/sbc-mst-productmodule.component';
import { SbcMstDynamicdbexceptionerrorviewComponent } from './sbc-mst-dynamicdbexceptionerrorview/sbc-mst-dynamicdbexceptionerrorview.component';
import { SbcMstDashboardComponent } from './sbc-mst-dashboard/sbc-mst-dashboard.component';
import { NgApexchartsModule } from 'ng-apexcharts';
import { SbcMstHelpandsupportComponent } from './sbc-mst-helpandsupport/sbc-mst-helpandsupport.component';
@NgModule({
  declarations: [
    SbcTrnSubscriptionsummaryComponent,
    SbcMstServersummaryComponent,
    SbcMstConsumersummaryComponent,
    SbcMstScriptmanagementComponent,
    SbcMstDynamicdbcreationComponent,
    SbcMstScriptmanagementviewComponent,
    SbcMstScriptmanagementexceptionviewComponent,
    SbcMstProductmoduleComponent,
    SbcMstDynamicdbexceptionerrorviewComponent,
    SbcMstDashboardComponent,
    SbcMstHelpandsupportComponent,
  ],
  imports: [
    CommonModule,
    EmsSubscriptionRoutingModule,NgApexchartsModule,
    ReactiveFormsModule,NgSelectModule,AngularEditorModule
  ]
})
export class EmsSubscriptionModule { }
