import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Error401Component } from './auth/Error/components/error401/error401.component';
import { Error404Component } from './auth/Error/components/error404/error404.component';
import { Error500Component } from './auth/Error/components/error500/error500.component';
import { LoginComponent } from './auth/login/login.component';
import { BobaloginComponent } from './auth/bobalogin/bobalogin.component';
import { InstituteloginComponent } from './auth/institutelogin/institutelogin.component';
import { WelcomePageComponent } from './auth/welcome-page/welcome-page.component';
import { PaymentsComponent } from './payments/payments.component';
import { VcxcontrollerloginComponent } from './auth/vcxcontrollerlogin/vcxcontrollerlogin.component';
import { HomePageComponent } from './auth/home-page/home-page.component';


const routes: Routes = [
  {
    path:'login', component: LoginComponent
  },
  {
    path:'401', component: Error401Component
  },
  {
    path:'404', component: Error404Component
  },
  {
    path:'500', component: Error500Component
  },
  {
    path:'', redirectTo : '/auth/login', pathMatch:'full'
  },
  {
    path:'bobalogin', component: BobaloginComponent
  },
  {
    path:'institutelogin', component: InstituteloginComponent
  },
  {
    path:'WelcomePage', component: WelcomePageComponent
  },
  {
    path:'payments', component: PaymentsComponent
  },
  {
    path:'vcxcontrollerlogin', component: VcxcontrollerloginComponent
  
  },
  {
    path:'HomePage', component: HomePageComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmsUtilitiesRoutingModule { }
