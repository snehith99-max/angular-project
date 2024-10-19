import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './ems.utilities/auth/Guard/auth.guard';
const routes: Routes = [
  {
    path: 'auth',
    loadChildren: () =>
      import(/* webpackChunkName: "utilities-module" */'./ems.utilities/ems.utilities.module').then((m) => m.EmsUtilitiesModule),
  },
  {
    path: 'system',canActivate: [AuthGuard], 
    loadChildren: () =>
      import(/* webpackChunkName: "system-module" */'./ems.system/ems.system.module').then((m) => m.EmsSystemModule),
  },
  {
    path: 'hrm',canActivate: [AuthGuard], 
    loadChildren: () =>
      import(/* webpackChunkName: "hrm-module" */'./ems.hrm/ems.hrm.module').then((m) => m.EmsHrmModule),
  },
  {
    path: 'payroll',canActivate: [AuthGuard], 
    loadChildren: () =>
      import(/* webpackChunkName: "payroll-module" */'./ems.payroll/ems.payroll.module').then((m) => m.EmsPayrollModule),
  },
  {
    path: 'rsk',canActivate: [AuthGuard], 
    loadChildren: () =>
      import(/* webpackChunkName: "rsk-module" */'./ems.rsk/ems.rsk.module').then((m) => m.EmsRskModule),
  },
  {
    path: 'crm',canActivate: [AuthGuard], 
    loadChildren: () =>
      import(/* webpackChunkName: "crm-module" */'./ems.crm/ems.crm.module').then((m) => m.EmsCrmModule),
  },
  {
    path: 'pmr',canActivate: [AuthGuard], 
    loadChildren: () =>
      import(/* webpackChunkName: "pmr-module" */'./ems.pmr/ems.pmr.module').then((m) => m.EmsPmrModule),
  },
  {
    path: 'sbc',canActivate: [AuthGuard], 
    loadChildren: () =>
      import(/* webpackChunkName: "subscription-module" */'./ems.subscription/ems.subscription.module').then((m) => m.EmsSubscriptionModule),
  },
  {
    path: 'ims',canActivate: [AuthGuard], 
    loadChildren: () =>
      import(/* webpackChunkName: "inventory-module" */'./ems.inventory/ems.inventory.module').then((m) => m.EmsInventoryModule),
  },
  {
    path: 'finance',canActivate: [AuthGuard], 
    loadChildren: () =>
      import(/* webpackChunkName: "finance-module" */'./ems.finance/ems.finance.module').then((m) => m.EmsFinanceModule),
  },
  {
    path: 'einvoice',canActivate: [AuthGuard], 
    loadChildren: () =>
      import(/* webpackChunkName: "einvoice-module" */'./ems.einvoice/ems.einvoice.module').then((m) => m.EmsEinvoiceModule),
  },
  {
    path: 'smr',canActivate: [AuthGuard], 
    loadChildren: () =>
      import(/* webpackChunkName: "sales-module" */'./ems.sales/ems.sales.module').then((m) => m.EmsSalesModule),
  },
  {
    path: 'asset',canActivate: [AuthGuard], 
    loadChildren: () =>
      import(/* webpackChunkName: "asset-module" */'./ems.asset/ems.asset.module').then((m) => m.EmsAssetModule),
  },
  {
    path: 'payable',canActivate: [AuthGuard], 
    loadChildren: () =>
      import(/* webpackChunkName: "payable-module" */'./ems.payable/ems.payable.module').then((m) => m.EmsPayableModule),
  },
  {
    path: 'payroll',canActivate: [AuthGuard],
    loadChildren: () =>
      import(/* webpackChunkName: "payroll-module" */'./ems.payroll/ems.payroll.module').then((m) => m.EmsPayrollModule),
  },
  {
    path: 'outlet',canActivate: [AuthGuard],
    loadChildren: () =>
      import(/* webpackChunkName: "outlet-module" */'./ems.outlet/ems.outlet.module').then((m) => m.EmsOutletModule),
  },
  {
    path: 'legal',canActivate: [AuthGuard],
    loadChildren: () =>
      import(/* webpackChunkName: "law-module" */'./ems.law/ems.law.module').then((m) => m.EmsLawModule),
  },
  {
    path: 'ITS',canActivate: [AuthGuard], 
    loadChildren: () =>
      import(/* webpackChunkName: "task-module" */'./ems.task/ems.task.module').then((m) => m.EmsTaskModule),
  },
  {
    path: 'wos',canActivate: [AuthGuard], 
    loadChildren: () =>
      import(/* webpackChunkName: "wos-module" */'./ems.wos/ems.wos.module').then((m) => m.EmsWosModule),
  },
  {
    path: '**',
    redirectTo: '/auth/login'
  },
  {
    path: '**',
    redirectTo: '/auth/vcxcontrollerlogin'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {useHash:true})],
  exports: [RouterModule]
})
export class AppRoutingModule { }
