import { Router } from '@angular/router';
import { Component, OnDestroy, HostListener, ElementRef } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { ToastrService } from 'ngx-toastr';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { TabsetComponent } from 'ngx-bootstrap/tabs';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AES } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import {
  CountryISO,
  SearchCountryField,

} from "ngx-intl-tel-input";

@Component({
  selector: 'app-crm-trn-tmycallslogs',
  templateUrl: './crm-trn-tmycallslogs.component.html',
  styleUrls: ['./crm-trn-tmycallslogs.component.scss']
})
export class CrmTrnTmycallslogsComponent {
  calllog_report: any[] = [];
  responsedata: any;
  recording_path: any;

  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, private el: ElementRef,
    private route: Router, public service: SocketService, private NgxSpinnerService: NgxSpinnerService) {    
   
  }
  ngOnInit(): void {
    this.Getcalllogreport();
  }
  Getcalllogreport() {
    this.NgxSpinnerService.show();
    var url = 'clicktocall/callSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#calllog_report').DataTable().destroy();
      this.responsedata = result;
      this.calllog_report = this.responsedata.calllog_report;
      setTimeout(() => {
        $('#calllog_report').DataTable();
      }, 1);
      this.NgxSpinnerService.hide();
  
    });
  }
  getaudio(uniqueid: any) {
    this.NgxSpinnerService.show();
    var url = 'clicktocall/Getaudioplay'
    let param = {
      uniqueid: uniqueid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.recording_path = this.responsedata.recording_path;
      this.NgxSpinnerService.hide();
    });
  }
}
