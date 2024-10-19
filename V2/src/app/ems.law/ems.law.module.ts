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
import { EmsLawRoutingModule } from './ems.law-routing.module';
import { LawMstInstituteaddComponent } from './Component/law-mst-instituteadd/law-mst-instituteadd.component';
import { LawMstCasetypeComponent } from './Component/law-mst-casetype/law-mst-casetype.component';
import { LglMstCasemanagementAddComponent } from './Component/lgl-mst-casemanagement-add/lgl-mst-casemanagement-add.component';
import { LglMstCasemanagementComponent } from './Component/lgl-mst-casemanagement/lgl-mst-casemanagement.component';
import { LglMstCasemanagementViewComponent } from './Component/lgl-mst-casemanagement-view/lgl-mst-casemanagement-view.component';
import { LawMstInstituteComponent } from './Component/law-mst-institute/law-mst-institute.component';
import { LawMstArbitrationtypeComponent } from './Component/law-mst-arbitrationtype/law-mst-arbitrationtype.component';
import { LawTrnArbitrationComponent } from './Component/law-trn-arbitration/law-trn-arbitration.component';
import { LawTrnArbitrationaddComponent } from './Component/law-trn-arbitrationadd/law-trn-arbitrationadd.component';
import { LglMstCasemanagementUploadComponent } from './Component/lgl-mst-casemanagement-upload/lgl-mst-casemanagement-upload.component';
import { LglInstCasemanagementSummaryComponent } from './Component/lgl-inst-casemanagement-summary/lgl-inst-casemanagement-summary.component';
import { LglInstCasemanagementViewComponent } from './Component/lgl-inst-casemanagement-view/lgl-inst-casemanagement-view.component';
import { LglInstCasemanagementUploadComponent } from './Component/lgl-inst-casemanagement-upload/lgl-inst-casemanagement-upload.component';
import { LawMstInstituteeditComponent } from './Component/law-mst-instituteedit/law-mst-instituteedit.component';
import { LawMstInstituteviewComponent } from './Component/law-mst-instituteview/law-mst-instituteview.component';
import { LglDashboardComponent } from './Component/lgl-dashboard/lgl-dashboard.component';
import { LawMstCasestageComponent } from './Component/law-mst-casestage/law-mst-casestage.component';

@NgModule({
  declarations: [
    LawMstInstituteComponent,
    LawMstArbitrationtypeComponent,
    LawTrnArbitrationComponent,
    LawTrnArbitrationaddComponent,
    LawMstInstituteaddComponent,
    LawMstCasetypeComponent,
    LglMstCasemanagementAddComponent,
    LglMstCasemanagementComponent,
    LglMstCasemanagementAddComponent,
    LglMstCasemanagementViewComponent,
    LawMstInstituteComponent,
    LawMstArbitrationtypeComponent,
    LawTrnArbitrationComponent,
    LawTrnArbitrationaddComponent,
    LglMstCasemanagementUploadComponent,
    LglMstCasemanagementAddComponent,
    LglInstCasemanagementSummaryComponent,
    LglInstCasemanagementViewComponent,
    LglInstCasemanagementUploadComponent,
    LawMstInstituteeditComponent,
    LawMstInstituteviewComponent,
    LglDashboardComponent,
    LawMstCasestageComponent,
          
  ],
  imports: [
    CommonModule,
    EmsLawRoutingModule,
    FormsModule, ReactiveFormsModule,EmsUtilitiesModule,
    NgApexchartsModule,DataTablesModule,
    NgSelectModule,AngularEditorModule,
    TabsModule,NgxIntlTelInputModule,
  ]
})
export class EmsLawModule { }
