import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TskMstTaskCreationComponent } from './component/tsk-mst-task-creation/tsk-mst-task-creation.component';
import { TskMstTaskAddComponent } from './component/tsk-mst-task-creation/tsk-mst-task-add.component';
import { TskMstCustomerComponent } from './component/tsk-mst-customer/tsk-mst-customer.component';
import { TskMstTaskTeamComponent } from './component/tsk-mst-task-team/tsk-mst-task-team.component';
import { TskMstTaskStatusViewComponent } from './component/tsk-mst-task-creation/tsk-mst-task-status-view.component';
import { TskTrnManagerComponent } from './component/tsk-trn-manager/tsk-trn-manager.component';
import { TskTrnMyApprovalComponent } from './component/tsk-trn-my-approval/tsk-trn-my-approval.component';
import { TskTrnMemberViewComponent } from './component/tsk-trn-my-approval/tsk-trn-member-view.component';
import { TskMstMandatorysummaryComponent } from './component/tsk-mst-task-creation/tsk-mst-mandatorysummary.component';
import { TskMstNicetohaveSummaryComponent } from './component/tsk-mst-task-creation/tsk-mst-nicetohave-summary.component';
import { TskMstNonMandatorysummaryComponent } from './component/tsk-mst-task-creation/tsk-mst-non-mandatorysummary.component';
import { TskTrnAssignedSummaryComponent } from './component/tsk-trn-manager/tsk-trn-assigned-summary.component';
import { TskTrnCompletedSummaryComponent } from './component/tsk-trn-manager/tsk-trn-completed-summary.component';
import { TskTrnHoldSummaryComponent } from './component/tsk-trn-my-approval/tsk-trn-hold-summary.component'; 
import { TskTrnLiveSummaryComponent } from './component/tsk-trn-my-approval/tsk-trn-live-summary.component';
import { TskTrnProgressSummaryComponent } from './component/tsk-trn-my-approval/tsk-trn-progress-summary.component';
import { TskTrnTestingSummaryComponent } from './component/tsk-trn-my-approval/tsk-trn-testing-summary.component';
import { TskTrnManagerHoldSummaryComponent } from './component/tsk-trn-manager/tsk-trn-manager-hold-summary.component';
import { TskTrnManagerTestSummaryComponent } from './component/tsk-trn-manager/tsk-trn-manager-test-summary.component';
import { TskMstCompletedComponent } from './component/tsk-mst-task-creation/tsk-mst-completed.component';
import { TskTrnTaskSheetComponent } from './component/tsk-trn-my-approval/tsk-trn-task-sheet/tsk-trn-task-sheet.component';
import { TskTrnTaskSheetDashboardComponent } from './component/tsk-trn-task-sheet-dashboard/tsk-trn-task-sheet-dashboard.component';
import { TskTrnManagerViewComponent } from './component/tsk-trn-manager/tsk-trn-manager-view.component';
import { TskMstDashboardComponent } from './component/tsk-mst-dashboard/tsk-mst-dashboard.component';
import { TskMstDeploymentTrackerComponent } from './component/tsk-mst-deployment-tracker/tsk-mst-deployment-tracker.component';
import { TskMstDeployAddComponent } from './component/tsk-mst-deployment-tracker/tsk-mst-deploy-add/tsk-mst-deploy-add.component';
import { TskMstDeployViewComponent } from './component/tsk-mst-deployment-tracker/tsk-mst-deploy-view/tsk-mst-deploy-view.component';
import { TskMstDeployEditComponent } from './component/tsk-mst-deployment-tracker/tsk-mst-deploy-edit/tsk-mst-deploy-edit.component';
const routes: Routes = [
  { path: 'ItsMstTaskCreation',component:TskMstTaskCreationComponent},
  { path: 'ItsMstTaskAdd',component:TskMstTaskAddComponent},
  { path: 'ItsMstCustomer',component:TskMstCustomerComponent},
  { path: 'ItsMstTaskTeam',component:TskMstTaskTeamComponent},
  { path: 'ItsMstTaskStatusView',component:TskMstTaskStatusViewComponent},
  { path: 'ItsTrnManager',component:TskTrnManagerComponent},
  { path: 'ItsTrnMyApproval',component:TskTrnMyApprovalComponent},
  { path: 'ItsTrnMemberView',component:TskTrnMemberViewComponent},
  { path: 'ItsMstNonMandatorysummary',component:TskMstNonMandatorysummaryComponent},
  { path: 'ItsMstNicetohaveSummary',component:TskMstNicetohaveSummaryComponent},
  { path: 'ItsMstMandatorysummary',component:TskMstMandatorysummaryComponent},
  { path: 'ItsTrnCompletedSummary',component:TskTrnCompletedSummaryComponent},
  { path: 'ItsTrnAssignedSummary',component:TskTrnAssignedSummaryComponent},
  { path: 'ItsTrnTestingSummary',component:TskTrnTestingSummaryComponent},
  { path: 'ItsTrnLiveSummary',component:TskTrnLiveSummaryComponent},
  { path: 'ItsTrnProgressSummary',component:TskTrnProgressSummaryComponent},
  { path: 'ItsTrnHoldSummary',component:TskTrnHoldSummaryComponent},
  { path: 'ItsTrnManagerHoldSummary',component:TskTrnManagerHoldSummaryComponent},
  { path: 'ItsTrnManagerTestSummary',component:TskTrnManagerTestSummaryComponent},
  { path: 'ItsMstCompleted',component:TskMstCompletedComponent},
  { path: 'ItsTrnTaskSheet',component:TskTrnTaskSheetComponent},
  { path: 'ItsTrnTaskSheetDashboard',component:TskTrnTaskSheetDashboardComponent},
  { path: 'ItsTrnManagerView',component:TskTrnManagerViewComponent},
  { path: 'ItsMstDashboard',component:TskMstDashboardComponent},
  { path: 'ItsMstDeploymentTracker',component:TskMstDeploymentTrackerComponent},
  { path: 'IskMstDeployAdd',component:TskMstDeployAddComponent},
  { path: 'IskMstDeployView',component:TskMstDeployViewComponent},
  { path: 'IskMstDeployEdit',component:TskMstDeployEditComponent},
];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmsTaskRoutingModule { }
