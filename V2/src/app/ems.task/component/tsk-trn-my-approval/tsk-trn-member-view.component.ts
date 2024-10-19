import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from 'src/environments/environment';
import { Location } from '@angular/common';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';

@Component({
  selector: 'app-tsk-trn-member-view',
  templateUrl: './tsk-trn-member-view.component.html',
  styles: [`  .bg-gray {
    background-color: rgb(234 237 240) !important;
  }
  @keyframes rtol_move {
    0% {
      transform: translateX(50);
    }
   
    100%{
      transform: translateX(-40%);
    }
  }
  .scroll-container {
    display: flex;
    animation: rtol_move 5s linear infinite;
  }
  .done{
    border-radius: 1.475rem;
  }
  `
  ]
})
export class TskTrnMemberViewComponent {
  task_gid: any;
  Estimated: any
  task_status_list: any
  task_details: any;
  subtask: any;
  status: any;
  Task_status = [
    { status_name: 'In Progress', status_gid: 'TSTS_001' },
    { status_name: 'Hold', status_gid: 'TSTS_002' },
    { status_name: 'In Testing', status_gid: 'TSTS_003' },
    { status_name: 'Completed In Live', status_gid: 'TSTS_004' },

  ];
  hold_status = [
    { status_name: 'Revoke', status_gid: 'TSTS_001' },
  ];
  test_status = [
    { status_name: 'Re-Testing', status_gid: 'TSTS_001' },
    { status_name: 'Completed In Live', status_gid: 'TSTS_002' },
  ];
  progress_status = [
    { status_name: 'Hold', status_gid: 'TSTS_002' },
    { status_name: 'In Testing', status_gid: 'TSTS_003' },
    { status_name: 'Completed In Live', status_gid: 'TSTS_003' },

  ];
  txtremarks: any
  remarks: boolean = false;
  txtdescription: any
  description: boolean = false
  doc_summary: any;
  task_status: any;
  estimtedhrs: boolean = false;
  taskupdated_by: any;
  taskForm!: FormGroup | any;
  taskupdated_date: any;
  hold_date: any;
  hold_by: any;
  txtholdremarks: any;
  days_since_creation: any;
  completed_date: any;
  completed_by: any;
  testing_date: any;
  testing_by: any;
  completedlive_list: any;
  assigned_list: any;
  statuslog_list: any;
  transfer_list: any;
  txthrs: undefined;
  txtreason: any;
  actualdevelopment_hrs: any
  development: boolean = false
  completed: boolean = false;
  actualcompleted_hrs: any
  development_hrs: any;
  completed_hrs: any;
  estimated_hours: any;
  txtreason_revoke: any;
  txttotal_hrs:any
  txtdeployment: undefined;
  constructor(public FormBuilder: FormBuilder, private NgxSpinnerService: NgxSpinnerService, private location: Location, public router: Router, private ActivatedRoute: ActivatedRoute, private SocketService: SocketService, private ToastrService: ToastrService) {
    this.createform()

  }
  createform() {
    this.taskForm = this.FormBuilder.group({
      txttask_status: ['', Validators.required],
      txtdeployment: ['', Validators.required],
      txttask_remarks: ['', [Validators.required, Validators.pattern(/^(\S+\s*)*(?!\s).*$/)]],
      txttotal_hrs: ['', [Validators.required, Validators.pattern(/^(\S+\s*)*(?!\s).*$/)]],
      txtactualdevelopment_hrs: ['',[Validators.required, Validators.pattern(/^(\S+\s*)*(?!\s).*$/)]],
      txtactualcompleted_hrs: ['',[Validators.required, Validators.pattern(/^(\S+\s*)*(?!\s).*$/)]]
    });
  }
  ngOnInit(): void {
    debugger
    this.ActivatedRoute.queryParams.subscribe(params => {
      const urlparams = params['hash'];
      if (urlparams) {
        const decryptedParam = AES.decrypt(urlparams, environment.secretKey).toString(enc.Utf8);
        const paramvalues = decryptedParam.split('&');
        this.task_gid = paramvalues[0];
      }
    });
    this.NgxSpinnerService.show();
    var params = {
      task_gid: this.task_gid,
    };
    var url = 'TskTrnTaskManagement/taskedit';
    this.SocketService.getparams(url, params).subscribe((result: any) => {
      this.task_details = result;
      this.status = result.task_status
      this.subtask=result.subtask || []
      this.taskupdated_by = result.taskupdated_by
      this.taskupdated_date = result.taskupdated_date
      this.hold_by = result.hold_by
      this.hold_date = result.hold_date
      this.txtholdremarks = result.txtremarks
      this.doc_summary = result.documentdata_list
      this.completed_date = result.completed_date;
      this.completed_by = result.completed_by;
      this.testing_date = result.testing_date;
      this.testing_by = result.testing_by;
      this.assigned_list = result.assigned_list
      this.estimated_hours=result.estimated_hours
      this.statuslog_list = result.statuslog_list
      this.completedlive_list = result.completedlive_list
      this.transfer_list = result.transfer_list
      this.development_hrs = result.actualdevelopment_hrs
      this.completed_hrs = result.actualcompleted_hrs
      this.NgxSpinnerService.hide();
    });
    const options: Options = {
      dateFormat: 'd-m-Y',
      minDate:'today',    
    };
    flatpickr('.date-picker', options); 
    this.isFormValid()
  }
  viewFile(path: string, name: string) {
    debugger;
    const lowerCaseFileName = name.toLowerCase();
    if (!(lowerCaseFileName.endsWith('.pdf') ||
      lowerCaseFileName.endsWith('.jpg') ||
      lowerCaseFileName.endsWith('.jpeg') ||
      lowerCaseFileName.endsWith('.png') ||
      lowerCaseFileName.endsWith('.txt') ||
      lowerCaseFileName.endsWith('.bmp'))) {
      window.scrollTo({
        top: 0,
      });
      this.ToastrService.warning('File Format Not Supported');
    }
    else {
      var params = {
        file_path: path,
        file_name: name
      }
      this.SocketService.downloadFile(params).subscribe((data: any) => {
        if (data != null) {
          this.SocketService.fileviewer(data);
        }
      });
    }
  }
  // downloadFiles(path:string, file_name:string){
  //   var params = {
  //     file_path : path,
  //     file_name : file_name
  //   }
  //   this.SocketService.downloadFile(params).subscribe((data:any) => {
  //     if(data != null){
  //       this.SocketService.filedownload(data);
  //     }
  //   });
  // }
  downloadFiles(document_upload: string, document_title: string): void {

    const image = document_upload.split('.net/');
    const page = image[0];
    const url = page.split('?');
    const imageurl = url[0];
    const parts = imageurl.split('.');
    const extension = parts.pop();

    this.SocketService.downloadfile(imageurl, document_title + '.' + extension).subscribe(
      (data: any) => {
        if (data != null) {
          this.SocketService.filedownload1(data);
        } else {
          this.ToastrService.warning('Error in file download');
        }
      },
    );
  }
  changestatus() {
    debugger

    if (this.task_status.status_name === 'Hold' || this.task_status.status_name === 'Revoke' || this.task_status.status_name ==='Re-Testing') {
      this.taskForm.get('txttask_remarks').setValidators([Validators.required, Validators.pattern(/^(\S+\s*)*(?!\s).*$/)]);
      this.taskForm.get('txttask_remarks').updateValueAndValidity();
      this.taskForm.get('txtactualdevelopment_hrs').reset();
      this.taskForm.get('txtactualdevelopment_hrs').clearValidators();
      this.taskForm.get('txtactualdevelopment_hrs').updateValueAndValidity();
      this.taskForm.get('txtactualcompleted_hrs').reset();
      this.taskForm.get('txtactualcompleted_hrs').clearValidators();
      this.taskForm.get('txtactualcompleted_hrs').updateValueAndValidity();
      this.taskForm.get('txttotal_hrs').reset();
      this.taskForm.get('txttotal_hrs').clearValidators();
      this.taskForm.get('txttotal_hrs').updateValueAndValidity();
      this.taskForm.get('txtdeployment').reset();
      this.taskForm.get('txtdeployment').clearValidators();
      this.taskForm.get('txtdeployment').updateValueAndValidity();
      this.remarks = true;
      this.estimtedhrs = false;
      this.development = false
      this.completed = false
      this.actualcompleted_hrs = null
      this.actualdevelopment_hrs = null
    }
    else if (this.task_status.status_name === 'In Testing') {
      this.taskForm.get('txtactualdevelopment_hrs').setValidators([Validators.required, Validators.pattern(/^(\S+\s*)*(?!\s).*$/)]);
      this.taskForm.get('txtactualdevelopment_hrs').updateValueAndValidity();
      this.taskForm.get('txtactualcompleted_hrs').reset();
      this.taskForm.get('txtactualcompleted_hrs').clearValidators();
      this.taskForm.get('txtactualcompleted_hrs').updateValueAndValidity();
      this.taskForm.get('txttask_remarks').reset();
      this.taskForm.get('txttask_remarks').clearValidators();
      this.taskForm.get('txttask_remarks').updateValueAndValidity();
      this.remarks = false;
      this.estimtedhrs = false;
      this.development = true
      this.completed = false
      this.txtremarks = null
      this.actualcompleted_hrs = null
      const options: Options = {
        dateFormat: 'Y-m-d',    
      };
      flatpickr('.date-picker', options); 
    }
    else if (this.task_status.status_name === 'Completed In Live') {
      this.taskForm.get('txtactualdevelopment_hrs').setValidators([Validators.required, Validators.pattern(/^(\S+\s*)*(?!\s).*$/)]);
      this.taskForm.get('txtactualdevelopment_hrs').updateValueAndValidity();
      this.taskForm.get('txtactualcompleted_hrs').setValidators([Validators.required, Validators.pattern(/^(\S+\s*)*(?!\s).*$/)]);
      this.taskForm.get('txtactualcompleted_hrs').updateValueAndValidity();
      this.taskForm.get('txttotal_hrs').setValidators([Validators.required, Validators.pattern(/^(\S+\s*)*(?!\s).*$/)]);
      this.taskForm.get('txttotal_hrs').updateValueAndValidity();
      this.taskForm.get('txtdeployment').setValidators([Validators.required]);
      this.taskForm.get('txtdeployment').updateValueAndValidity();
      this.taskForm.get('txttask_remarks').reset();
      this.taskForm.get('txttask_remarks').clearValidators();
      this.taskForm.get('txttask_remarks').updateValueAndValidity();
      this.remarks = false;
      this.estimtedhrs = false;
      this.development = false
      this.completed = true
      this.actualdevelopment_hrs = null
this.txtremarks=null
    }
    else {
      this.remarks = false;
      this.development = false
      this.estimtedhrs = true;
      this.completed = false
      this.actualcompleted_hrs = null
      this.actualdevelopment_hrs = null
      this.txtremarks = null

    }
    // if (this.task_status.status_name === 'Hold') {
    //   this.remarks = true;
    //   this.estimtedhrs = false;
    // } else if (this.task_status.status_name === 'In Progress') {
    //   this.remarks = false;
    //   this.estimtedhrs = true;
    // } else if (this.task_status.status_name === 'Re Open') {
    //   this.remarks = false;
    //   this.estimtedhrs = true;
    // } else if (this.task_status.status_name === 'Completed') {
    //   this.remarks = true;
    //   this.estimtedhrs = false;
    // } else if (this.task_status.status_name === 'Testing') {
    //   this.remarks = true;
    //   this.estimtedhrs = false;
    // } else if (this.task_status.status_name === 'Completed In Live') {
    //   this.remarks = true;
    //   this.estimtedhrs = false;
    // }
  }

  isFormValid() {
    if (this.remarks) {
      return this.txtremarks.trim().length > 0;
    }
    return true;
  }
  hold() {
    this.remarks = true
    this.description = false
    this.txtdescription = null
    this.txtremarks = null
  }
  pending() {
    this.remarks = false
    this.description = false
    this.txtdescription = null
  }
  complete() {
    this.remarks = false
    this.description = true
    this.txtremarks = null

  }
  backbutton() {
    this.location.back()
  }

  submittask() {
    debugger
    var param = {
      // employee_status: this.task_status.status_name === 'Revoke' ? 'Pending' : this.task_status.status_name,
      employee_status : this.task_status.status_name,
      task_gid: this.task_gid,
      remarks: (this.txtremarks == undefined) ? "" : this.txtremarks,
      total_hrs: (this.txttotal_hrs == undefined) ? "" : this.txttotal_hrs,
      actualdevelopment_hrs: (this.actualdevelopment_hrs == undefined) ? "" : this.actualdevelopment_hrs,
      actualcompleted_hrs: (this.actualcompleted_hrs == undefined) ? "" : this.actualcompleted_hrs,
      deployment_date: (this.txtdeployment == undefined) ? "" : this.txtdeployment
    }
    this.NgxSpinnerService.show();

    var url = 'TskTrnTaskManagement/UpdateTask';
    this.SocketService.post(url, param).subscribe((result: any) => {
      if (result.status == true) {
        this.ToastrService.success(result.message);
        this.location.back();
        this.NgxSpinnerService.hide();

      } else {
        this.ToastrService.warning(result.message);
        this.location.back();
        this.NgxSpinnerService.hide();

      }
    });
  }


  submit() {
    debugger
    let description = this.txtdescription !== undefined ? this.txtdescription : this.txtremarks;
    var param = {
      task_status: this.status,
      task_gid: this.task_gid,
      remarks: description

    }
    this.NgxSpinnerService.show();

    var url = 'TskTrnTaskManagement/Updatestatus';
    this.SocketService.post(url, param).subscribe((result: any) => {
      if (result.status == true) {
        this.ToastrService.success(result.message);
        this.location.back();
        this.NgxSpinnerService.hide();

      } else {
        this.ToastrService.warning(result.message);
        this.location.back();
        this.NgxSpinnerService.hide();

      }
    });
  }
  remarksdetails(data: any) {
    this.txtreason = data.allocationhold_reason
    this.txtreason_revoke=data.revoke_reason
  }

  clear() {
    this.task_status = null
    this.txtremarks = null
    this.actualcompleted_hrs = null
    this.actualdevelopment_hrs = null
  }
}
