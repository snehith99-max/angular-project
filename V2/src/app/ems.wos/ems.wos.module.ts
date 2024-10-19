import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { EmsUtilitiesModule } from '../ems.utilities/ems.utilities.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxPaginationModule } from 'ngx-pagination';
import { DataTablesModule } from 'angular-datatables';
import { NgSelectModule } from '@ng-select/ng-select';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { MatTabsModule } from '@angular/material/tabs';
import { EmsWosRoutingModule } from './ems.wos-routing.module';
import { WosChatWhatsAppComponent } from './component/wos-chat-whats-app/wos-chat-whats-app.component';
import { NgxIntlTelInputModule } from 'ngx-intl-tel-input';


@NgModule({
  declarations: [
    WosChatWhatsAppComponent
  ],
  imports: [
    CommonModule,
    EmsWosRoutingModule, ReactiveFormsModule,
    DataTablesModule, TabsModule.forRoot(), NgbModule, FormsModule,
    NgxPaginationModule, EmsUtilitiesModule, NgSelectModule, MatTabsModule,NgxIntlTelInputModule
  ]
})
export class EmsWosModule { }
