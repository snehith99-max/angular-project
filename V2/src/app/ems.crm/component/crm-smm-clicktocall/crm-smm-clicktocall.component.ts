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
interface Iclicktocalls {
  individual_gid: string;
  phone_number: string;
  user_name: string;
  remarks: string;
}
interface Iaddleads {
  user_name: string;
  phone_number: string;
  customertype_edit: string;
}
@Component({
  selector: 'app-crm-smm-clicktocall',
  templateUrl: './crm-smm-clicktocall.component.html',
  styleUrls: ['./crm-smm-clicktocall.component.scss'],
  styles: [`
  table thead th, 
  .table tbody td { 
   position: relative; 
  z-index: 0;
  } 
  .table thead th:last-child, 
  
  .table tbody td:last-child { 
   position: sticky; 
  
  right: 0; 
   z-index: 0; 
  
  } 
  .table td:last-child, 
  
  .table th:last-child { 
  
  padding-right: 50px; 
  
  } 
  .table.table-striped tbody tr:nth-child(odd) td:last-child { 
  
   background-color: #ffffff; 
    
    } 
    .table.table-striped tbody tr:nth-child(even) td:last-child { 
     background-color: #f2fafd; 
  
  } 
  `]
})
export class CrmSmmClicktocallComponent {
  responsedata: any;
  calllog_report: any[] = [];
  individualcalllog_report: any[] = [];
  recording_path: any;
  windowInterval: any;
  phone_number: string = "";
  reactiveFormContactEdit!: FormGroup;
  answered: any;
  agent_missed: any;
  customer_missed: any;
  individual_gid: any;
  total_count: any;
  SearchCountryField = SearchCountryField;
  CountryISO = CountryISO;
  preferredCountries: CountryISO[] = [
    CountryISO.India,
    CountryISO.India
  ];
  customertype_list: any[] = [];
  reactiveForm!: FormGroup;
  parameterValue: any;
  parameterValue1:any;
  clicktocalls!: Iclicktocalls;
  addleads!: Iaddleads;
  showOptionsDivId: any;

  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, private el: ElementRef,
    private route: Router, public service: SocketService, private NgxSpinnerService: NgxSpinnerService) {    
   this.clicktocalls = {} as Iclicktocalls;
    this.addleads = {} as Iaddleads;
  }
  ngOnInit(): void {

    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    this.Getcalllogreport();
    var api3 = 'Leadbank/GetCustomerTypeSummary'
    this.service.get(api3).subscribe((result: any) => {
      this.responsedata = result;
      this.customertype_list = result.customertype_list1;
    });

    var api1 = 'clicktocall/updatelead'
    this.service.get(api1).subscribe((result: any) => {
      this.responsedata = result;
    });
    this.reactiveFormContactEdit = new FormGroup({
      user_name: new FormControl(this.addleads.user_name, [
        Validators.required

      ]),
      phone_number: new FormControl(this.addleads.phone_number, [
      ]),
      customertype_edit: new FormControl(this.addleads.customertype_edit, [
        Validators.required

      ]),
    });
    this.reactiveForm = new FormGroup({
      individual_gid: new FormControl(this.clicktocalls.individual_gid, [
      ]),
      // user_name: new FormControl(this.clicktocalls.user_name, [
      //   Validators.required
      // ]),
      remarks: new FormControl(this.clicktocalls.remarks, [
      ]),
      phone_number: new FormControl(this.clicktocalls.phone_number, [
      ]),
    });

  }
  
  onInputChange(event: any): void {
    this.phone_number = event.target.value.replace(/\D/g, '');
  }

  clicktocall(phone_number: any) {
    this.NgxSpinnerService.show();
    var url = 'clicktocall/customercall'
    let param = {
      phone_number: phone_number
    }
    this.service.postparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        this.NgxSpinnerService.hide();

        this.ToastrService.warning(result.message)

      }
      else {
        this.NgxSpinnerService.hide();
        this.ToastrService.success('Connecting Your Call !! ')
        this.phone_number = "";

      }

    });
  }

  get user_name() {
    return this.reactiveFormContactEdit.get('user_name')!;
  }
  get customertype_edit() {
    return this.reactiveFormContactEdit.get('customertype_edit')!;
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

  getcall(parameter: string) {
    this.phone_number = parameter

  } 
  toggleOptions(individual_gid: any) {
    if (this.showOptionsDivId === individual_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = individual_gid;
    }
  }
  connectcall() {
    let params = {
      phone_number: this.phone_number,
    }
    this.NgxSpinnerService.show();
    var url = 'clicktocall/customercall'
    this.service.postparams(url, params).subscribe((result: any) => {
      if (result.status == false) {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message)

      }
      else {
        this.NgxSpinnerService.hide();
        this.ToastrService.success('Connecting Your Call!! ')

      }
    });
  }
  getlogreport(phone_number: any) {
    this.NgxSpinnerService.show();
    var url = 'clicktocall/Getlogreport'
    let params = {
      phone_number: phone_number,
    }
    this.service.getparams(url, params).subscribe((result: any) => {
      if (result != null) {
        this.individualcalllog_report = result.calllog_report;
        this.phone_number = result.phone_number
        this.NgxSpinnerService.hide();
        setTimeout(() => {
          $('#individualcalllog_report').DataTable();
        }, 1000);
      }
      else {
        clearInterval(this.windowInterval)
      }
    });
  }
  public onupdatecontact(): void {
    this.NgxSpinnerService.show();
    if (this.reactiveFormContactEdit.value.customertype_edit != null && this.reactiveFormContactEdit.value.customertype_edit != "" && this.reactiveFormContactEdit.value.user_name != null) {
      for (const control of Object.keys(this.reactiveFormContactEdit.controls)) {
        this.reactiveFormContactEdit.controls[control].markAsTouched();
      }
      this.reactiveFormContactEdit.value;
      var url = 'clicktocall/Postaddleads'
      this.service.postparams(url, this.reactiveFormContactEdit.value).pipe().subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.warning(result.message)
          this.reactiveFormContactEdit.reset();
          this.Getcalllogreport();
          this.NgxSpinnerService.hide();
        }
        else {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.success(result.message)
          this.reactiveFormContactEdit.reset();
          this.Getcalllogreport();
          this.NgxSpinnerService.hide();
        }



      });

    }
  }
  onclose() {
    this.reactiveFormContactEdit.reset();
    this.reactiveForm.reset();
    this.phone_number="";

  }

  closecall(){
    this.phone_number = "";

  }
  notesupdate(parameter: string) {
    this.parameterValue = parameter
    this.reactiveForm.get("individual_gid")?.setValue(this.parameterValue.individual_gid);
    this.reactiveForm.get("phone_number")?.setValue(this.parameterValue.phone_number);
    this.reactiveForm.get("user_name")?.setValue(this.parameterValue.user_name);
    this.reactiveForm.get("remarks")?.setValue(this.parameterValue.remarks);

  }
  addlead(parameter: string) {
    this.parameterValue1 = parameter
    this.reactiveFormContactEdit.get("user_name")?.setValue(this.parameterValue1.user_name);
    this.reactiveFormContactEdit.get("phone_number")?.setValue(this.parameterValue1.phone_number);
    this.reactiveFormContactEdit.get("customertype_edit")?.setValue(this.parameterValue1.customertype_edit);

  }
  onupdate() {

    this.reactiveForm.value;
    var url = 'clicktocall/UpdatedRemarks';
    this.service.postparams(url, this.reactiveForm.value).pipe().subscribe((result: any) => {
      this.responsedata = result;
      if (result.status == false) {
        this.Getcalllogreport();
        this.ToastrService.warning(result.message);
        this.reactiveForm.reset();

      } else {
        this.Getcalllogreport();
        this.ToastrService.success(result.message);
        this.reactiveForm.reset();
      }
    });


  }
}
