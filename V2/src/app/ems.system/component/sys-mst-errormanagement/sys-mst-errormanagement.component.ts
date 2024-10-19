import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
interface Ierror{
  error_code: string;
  error_message: string;
  error_type:string;
  ref_no:string;
  error_gid: string;
}

@Component({
  selector: 'app-sys-mst-errormanagement',
  templateUrl: './sys-mst-errormanagement.component.html',
  styleUrls: ['./sys-mst-errormanagement.component.scss'],
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
export class SysMstErrormanagementComponent {
  showOptionsDivId: any;
  reactiveForm!: FormGroup;
  reactiveaddForm!: FormGroup;
  reactiveeditForm!: FormGroup;
  responsedata: any;
  parameterValue: any;
  parameterValue1: any;
  error_list: any[] = [];
  Error!: Ierror;
  constructor(private SocketService: SocketService, private NgxSpinnerService: NgxSpinnerService, public service: SocketService, private ToastrService: ToastrService, private FormBuilder: FormBuilder) {
    this.Error = {} as Ierror;

    this.reactiveaddForm = new FormGroup({
      ref_no: new FormControl(''),     
      error_code: new FormControl('', [Validators.required, Validators.pattern(/^\S.*$/)]),
      error_message: new FormControl('', [Validators.required,Validators.pattern(/^\S.*$/)]),
      error_type: new FormControl('', [Validators.required, Validators.pattern(/^\S.*$/)]),
      error_gid:new FormControl(''),
    })

   this.reactiveeditForm = new FormGroup({
    ref_no: new FormControl(''),     
    error_code: new FormControl(''),
    error_message: new FormControl('', [Validators.required,Validators.pattern(/^\S.*$/)]),
    error_type: new FormControl('', [Validators.required, Validators.pattern(/^\S.*$/)]),
    error_gid:new FormControl(''),
  })
}

  ngOnInit(): void {

    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
 this.errorsummary();
  }
  errorsummary(){
    var url = 'SysMsterrormanager/GeterrorSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#error_list').DataTable().destroy();
    this.responsedata = result;
      this.error_list = this.responsedata.errorcount;
      setTimeout(() => {
        $('#error_list').DataTable();
      }, 1);
    });
  }


  onsubmit() {
    var params = {
      error_gid: this.reactiveaddForm.value.ref_no,
      error_code: this.reactiveaddForm.value.error_code,
      error_message: this.reactiveaddForm.value.error_message,
      error_type: this.reactiveaddForm.value.error_type  
    }
    var url = 'SysMsterrormanager/Adderrorsubmit'
    this.NgxSpinnerService.show();
    this.service.postparams(url, params).subscribe((result: any) => {   
      if (result.status == false) {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message)
        this.reactiveForm.reset();
        this.NgxSpinnerService.hide();
        this.errorsummary();
      }
      else {
        this.reactiveaddForm.reset();
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
        this.errorsummary();

      }
    });
   
    // setTimeout(function () {
    //   window.location.reload();
    // }, 2000);
  }

  onclose(){
    this.reactiveaddForm.reset();
  }

  get error_code(){
    return this.reactiveaddForm.get('error_code')!;
  }
  get error_message(){
    return this.reactiveaddForm.get('error_message')!;
  }
  get error_type(){
    return this.reactiveaddForm.get('error_type')!;
  }
  get ref_no(){
    return this.reactiveaddForm.get('ref_no')!;
  }

  

  openModaledit(parameter: string) {
    this.parameterValue1 = parameter   
    this.reactiveeditForm.get("ref_no")?.setValue(this.parameterValue1.error_gid);
    this.reactiveeditForm.get("error_code")?.setValue(this.parameterValue1.error_code );
    this.reactiveeditForm.get("error_message")?.setValue(this.parameterValue1.error_message);
    this.reactiveeditForm.get("error_type")?.setValue(this.parameterValue1.error_type );
    this.reactiveeditForm.get("error_gid")?.setValue(this.parameterValue1.error_gid);
  }
  
  update(){
    var url = 'SysMsterrormanager/getUpdatederrormanage';
    this.NgxSpinnerService.show();
    this.SocketService.post(url, this.reactiveeditForm.value).subscribe((result:any) => {
      this.NgxSpinnerService.hide();
      if (result.status == false) {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide();
        this.errorsummary();

      }
      else {        
        this.reactiveeditForm.reset();
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
        this.errorsummary();

      }
    });
    // setTimeout(function () {
    //   window.location.reload();
    // }, 2000);  
  }
  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }
}
