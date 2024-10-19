import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EmsUtilitiesRoutingModule } from './ems.utilities-routing.module';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { LoginComponent } from './auth/login/login.component';
import { Error404Component } from './auth/Error/components/error404/error404.component';
import { Error401Component } from './auth/Error/components/error401/error401.component';
import { Error500Component } from './auth/Error/components/error500/error500.component';
import { BobaloginComponent } from './auth/bobalogin/bobalogin.component';
import { InstituteloginComponent } from './auth/institutelogin/institutelogin.component';
import { WelcomePageComponent } from './auth/welcome-page/welcome-page.component';
import { PaymentsComponent } from './payments/payments.component';
import { VcxcontrollerloginComponent } from './auth/vcxcontrollerlogin/vcxcontrollerlogin.component';
import { HomePageComponent } from './auth/home-page/home-page.component';





@NgModule({
  declarations: [
    LoginComponent,
    Error404Component,
    Error401Component,
    Error500Component,
    BobaloginComponent,
    InstituteloginComponent,
    WelcomePageComponent,
    PaymentsComponent,
    VcxcontrollerloginComponent,
    HomePageComponent
  ],
  imports: [
    CommonModule,
    EmsUtilitiesRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    
  ]
})
export class EmsUtilitiesModule { }
