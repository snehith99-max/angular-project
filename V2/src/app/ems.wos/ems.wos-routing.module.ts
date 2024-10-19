import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { WosChatWhatsAppComponent } from './component/wos-chat-whats-app/wos-chat-whats-app.component';
const routes: Routes = [
  { path: 'WosChatWhatsApp', component: WosChatWhatsAppComponent },


];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmsWosRoutingModule { }
