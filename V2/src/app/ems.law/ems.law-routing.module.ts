
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LawMstInstituteComponent } from './Component/law-mst-institute/law-mst-institute.component';
import { LawMstArbitrationtypeComponent } from './Component/law-mst-arbitrationtype/law-mst-arbitrationtype.component';
import { LawTrnArbitrationComponent } from './Component/law-trn-arbitration/law-trn-arbitration.component';
import { LawTrnArbitrationaddComponent } from './Component/law-trn-arbitrationadd/law-trn-arbitrationadd.component';
import { LawMstInstituteaddComponent } from './Component/law-mst-instituteadd/law-mst-instituteadd.component';
import { LawMstCasetypeComponent } from './Component/law-mst-casetype/law-mst-casetype.component';
import { LglMstCasemanagementComponent } from './Component/lgl-mst-casemanagement/lgl-mst-casemanagement.component';
import { LglMstCasemanagementAddComponent } from './Component/lgl-mst-casemanagement-add/lgl-mst-casemanagement-add.component';
import { LglMstCasemanagementViewComponent } from './Component/lgl-mst-casemanagement-view/lgl-mst-casemanagement-view.component';
import { LglMstCasemanagementUploadComponent } from './Component/lgl-mst-casemanagement-upload/lgl-mst-casemanagement-upload.component';
import { LglInstCasemanagementSummaryComponent } from './Component/lgl-inst-casemanagement-summary/lgl-inst-casemanagement-summary.component';
import { LglInstCasemanagementViewComponent } from './Component/lgl-inst-casemanagement-view/lgl-inst-casemanagement-view.component';
import { LglInstCasemanagementUploadComponent } from './Component/lgl-inst-casemanagement-upload/lgl-inst-casemanagement-upload.component';
import { LawMstInstituteeditComponent } from './Component/law-mst-instituteedit/law-mst-instituteedit.component';
import { LawMstInstituteviewComponent } from './Component/law-mst-instituteview/law-mst-instituteview.component';
import { LglDashboardComponent } from './Component/lgl-dashboard/lgl-dashboard.component';
import { LawMstCasestageComponent } from './Component/law-mst-casestage/law-mst-casestage.component';


const routes: Routes = [

    { path: 'LglMstInstitute', component: LawMstInstituteComponent },
    { path: 'LglstArbitrationtype', component: LawMstArbitrationtypeComponent },
    { path: 'LglTrnArbitration', component: LawTrnArbitrationComponent },
    { path: 'LglTrnArbitrationadd', component: LawTrnArbitrationaddComponent },
    { path: 'LglMstInstituteadd', component: LawMstInstituteaddComponent },
    { path: 'LglMstInstituteedit/:institute_gid', component: LawMstInstituteeditComponent },
    { path: 'LglMstInstituteview/:institute_gid', component: LawMstInstituteviewComponent },
    { path: 'LglMstCasetype', component: LawMstCasetypeComponent },
    { path: 'CaseManagementAdd', component: LglMstCasemanagementAddComponent },
    
    { path: 'CaseManagementUpload/:case_gid1', component: LglMstCasemanagementUploadComponent },
    { path: 'CaseManagement', component: LglMstCasemanagementComponent },
  { path: 'CaseManagementAdd', component: LglMstCasemanagementAddComponent },
  { path: 'CaseManagementView/:case_gid1', component: LglMstCasemanagementViewComponent },

  // ------------------------instutite route-----------------------------------------//
  { path: 'LglIstCaseManagement', component: LglInstCasemanagementSummaryComponent },
  { path: 'LglIstCaseManagement-View/:case_gid1', component: LglInstCasemanagementViewComponent },
  { path: 'LglIstCaseManagement-Upload/:case_gid1', component: LglInstCasemanagementUploadComponent },
  { path: 'LglDashboard', component: LglDashboardComponent },
  { path: 'LglMstCaseStage', component: LawMstCasestageComponent },
];

 
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmsLawRoutingModule { }
