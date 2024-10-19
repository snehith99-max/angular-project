import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from 'src/environments/environment';
import { Location } from '@angular/common';
@Component({
  selector: 'app-tsk-mst-task-status-view',
  templateUrl: './tsk-mst-task-status-view.component.html',
  styles: [`.done{
    border-radius: 1.475rem;
  }
  .bg-gray {
    background-color: rgb(234 237 240) !important;
  }`
  ]
})
export class TskMstTaskStatusViewComponent {
  task_gid: any;
  task_details: any;
  subtask: any;
  txtreason:any
  status: any;
  txtremarks:any
  remarks: boolean=false;
  txtdescription:any
  description:boolean=false
  doc_summary: any;
  assigned_list: any;
  statuslog_list: any;
  completedlive_list: any;
  transfer_list: any;
  development_hrs: any;
  completed_hrs: any;
  constructor(private NgxSpinnerService:NgxSpinnerService,private location: Location,public router:Router,private ActivatedRoute: ActivatedRoute,private SocketService: SocketService,private ToastrService: ToastrService) {}

  ngOnInit(): void 
  {debugger
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
    this.SocketService.getparams(url,params).subscribe((result: any) => { 
      this.task_details  = result; 
      this.status=result.task_status
      // this.subtask=result.sub_task
      this.doc_summary=result.documentdata_list
      this.assigned_list=result.assigned_list
      this.statuslog_list=result.statuslog_list
      this.subtask = result.subtask || [];
            this.completedlive_list=result.completedlive_list
      this.transfer_list=result.transfer_list
      this.development_hrs=result.actualdevelopment_hrs
      this.completed_hrs=result.actualcompleted_hrs
      this.NgxSpinnerService.hide();
    });
  }
  viewFile(path:string, name:string){
    debugger;
    const lowerCaseFileName = name.toLowerCase();
    if (!(lowerCaseFileName.endsWith('.pdf') ||
      lowerCaseFileName.endsWith('.jpg') ||
      lowerCaseFileName.endsWith('.jpeg') ||
      lowerCaseFileName.endsWith('.png') ||
      lowerCaseFileName.endsWith('.txt')||
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
        var url = 'TskTrnTaskManagement/DownloadDocument';
        this.SocketService.post(url, params).subscribe((result: any) => {        
            if (result != null) {
            this.SocketService.fileviewer(result);
          }
        });
    }
  }

  downloadFiles(path:string, file_name:string) {debugger
    var params = {
      file_path : path,
      file_name : file_name
    }
    var url = 'TskTrnTaskManagement/DownloadDocument';
    this.SocketService.post(url, params).subscribe((result: any) => {
    // this.SocketService.downloadFile(params).subscribe((data:any) => {
      if(result != null){
        this.SocketService.filedownload1(result);
      }
    });
  }
  
  hold(){
    this.remarks=true
    this.description=false
    this.txtdescription=null
    this.txtremarks=null
  }
  pending(){
    this.remarks=false
    this.description=false
this.txtdescription=null
  }
  complete(){
    this.remarks=false
this.description=true
this.txtremarks=null

  }
  backbutton(){
    this.location.back()
  }
  submit(){debugger
    let description = this.txtdescription !== undefined ? this.txtdescription : this.txtremarks;
    var param={
      task_status:this.status,
      task_gid:this.task_gid,
      remarks:description
     
    }
    this.NgxSpinnerService.show();

    var url = 'TskMstCustomer/Updatestatus';
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
  remarksdetails(data:any){
this.txtreason = data.task_remarks
  }
}
