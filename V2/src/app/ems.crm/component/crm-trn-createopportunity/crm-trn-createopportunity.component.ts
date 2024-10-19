import { Component, OnInit, ElementRef, ViewChild, HostListener } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { Subscription, timer } from "rxjs";
import { map, share } from "rxjs/operators";
import { DatePipe } from '@angular/common';
import { NgxSpinnerService } from 'ngx-spinner';
import { saveAs } from 'file-saver';
import { Location } from '@angular/common';
import {
  CountryISO,
  SearchCountryField,

} from "ngx-intl-tel-input";

import { TabsModule } from 'ngx-bootstrap/tabs';
import { TabsetComponent } from 'ngx-bootstrap/tabs';

import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
@Component({
  selector: 'app-crm-trn-createopportunity',
  templateUrl: './crm-trn-createopportunity.component.html',
  styleUrls: ['./crm-trn-createopportunity.component.scss']
})
export class CrmTrnCreateopportunityComponent {
  // reactiveForm!: FormGroup;
  // constructor(private fb: FormBuilder, private route: ActivatedRoute, private router: Router, private service: SocketService, private ToastrService: ToastrService, private datePipe: DatePipe, private NgxSpinnerService: NgxSpinnerService, private location: Location) {

  // }
  // ngOnInit(): void {
  //   this.reactiveForm = new FormGroup({
  //     appointment_timing: new FormControl(''),
  //     leadname_gid: new FormControl(null),
  //     bussiness_verticle: new FormControl(null),
  //     remarks: new FormControl(null),
  //     lead_title: new FormControl(null),
  //   });
  // }
  // get appointment_timing() {
  //   return this.reactiveForm.get('appointment_timing')!;
  // }
  // get leadname_gid() {
  //   return this.reactiveForm.get('leadname_gid')!;
  // }
  // get bussiness_verticle() {
  //   return this.reactiveForm.get('bussiness_verticle')!;
  // }
  // get lead_title() {
  //   return this.reactiveForm.get('lead_title')!;
  // }
  // onback() {
    
  //     this.router.navigate(['/crm/CrmTrnAppointmentmanagement']);
    
  // }

}
