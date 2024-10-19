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
  selector: 'app-tsk-mst-deploy-view',
  templateUrl: './tsk-mst-deploy-view.component.html',
  styleUrls: ['./tsk-mst-deploy-view.component.scss']
})
export class TskMstDeployViewComponent {
  deploy_details: any;
  scriptattach_file: any;
  deploydependcy_module: any;
  deploy_module: any;
  constructor(private NgxSpinnerService:NgxSpinnerService,private location: Location,public router:Router,private ActivatedRoute: ActivatedRoute,private SocketService: SocketService,private ToastrService: ToastrService) {}
  deployment_trackergid:any
  ngOnInit(): void 
  {debugger
    this.ActivatedRoute.queryParams.subscribe(params => {
      const urlparams = params['hash'];  
      if (urlparams) {
        const decryptedParam = AES.decrypt(urlparams, environment.secretKey).toString(enc.Utf8);
        const paramvalues = decryptedParam.split('&');
        this.deployment_trackergid = paramvalues[0];
      }
    });
    this.NgxSpinnerService.show();
    var params = {
      deployment_trackergid: this.deployment_trackergid,
    }; 
    var url = 'TskTrnTaskManagement/deployview';
    this.SocketService.getparams(url,params).subscribe((result: any) => { 
      this.deploy_details  = result; 
      this.deploy_module=result.deploy_module
      this.deploydependcy_module = result.deploydependcy_module || [];
      this.scriptattach_file = result.scriptattach_file || [];
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
  backbutton(){
    this.location.back()
  }
}
