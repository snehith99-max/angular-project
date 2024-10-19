import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LayoutComponent } from './layout.component';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { BodyComponent } from './components/body/body.component';
import { SidenavComponent } from './components/sidenav/sidenav.component';
import { SublevelMenuComponent } from './components/sidenav/sublevel-menu.component';
import { RouterModule } from '@angular/router';
import { NotesComponent } from './components/notes/notes.component';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { CalendlyComponent } from './components/calendly/calendly.component';
import { MeetingsPanelComponent } from './components/meetings-panel/meetings-panel.component';
import { NgxSpinnerModule } from 'ngx-spinner';


@NgModule({
  declarations: [
    LayoutComponent,
    HeaderComponent,
    FooterComponent,
    BodyComponent,
    SidenavComponent,
    SublevelMenuComponent,
    NotesComponent,
    CalendlyComponent,
    MeetingsPanelComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    FormsModule
  ],
  exports:[
    HeaderComponent,
    LayoutComponent
  ]
})
export class LayoutModule { }
