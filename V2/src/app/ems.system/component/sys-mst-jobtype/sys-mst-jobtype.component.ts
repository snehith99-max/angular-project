import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { ApexChart, ApexNonAxisChartSeries, ApexResponsive } from 'ng-apexcharts';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-sys-mst-jobtype',
  templateUrl: './sys-mst-jobtype.component.html',
  styleUrls: ['./sys-mst-jobtype.component.scss']
})
export class SysMstJobtypeComponent {
 jobtype_list: any[] = [];
 JobType: FormGroup | any;
 parameterValue1: any;
 parameterValue: any;
 responsedata: any;
 showOptionsDivId: any;
 


 constructor(private SocketService: SocketService, private NgxSpinnerService: NgxSpinnerService, public service: SocketService, private ToastrService: ToastrService, private FormBuilder: FormBuilder) {
  this.JobType = new FormGroup({
  Jobtype_Code: new FormControl('', [Validators.required]),
  Jobtype_Name: new FormControl('', [Validators.required]),
  jobtype_gid: new FormControl(''),

  })
}
ngOnInit(): void{
  this.jobtypesummary()
}
  jobtypesummary() {
    var api = 'Jobtype/GetJobtypeSummary';
    this.service.get(api).subscribe((result: any) => {
      this.jobtype_list = result.JobtypeLists;
      setTimeout(() => {
        $('#jobtype_list').DataTable();
        }, );
    });
  }
  addJobtype() {
    var url = 'Jobtype/PostJobtype';
    this.SocketService.postparams(url, this.JobType.value).subscribe((result: any) => {
      if (result.status != false) {
        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message);
        this.JobType.reset();

        this.jobtypesummary()
      }
      else {
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide();
        this.JobType.reset();
      }
    });
    setTimeout(function () {
      window.location.reload();
    }, 2000);
  }
  openModaledit(parameter: string) {
    debugger;
    this.parameterValue1 = parameter
  
    this.JobType.get("Jobtype_Code")?.setValue(this.parameterValue1.JobType_Code);
    this.JobType.get("Jobtype_Name")?.setValue(this.parameterValue1.JobType_Name);
    this.JobType.get("jobtype_gid")?.setValue(this.parameterValue1.Jobtype_gid );
  }
  updateJobtype(){
    this.NgxSpinnerService.show();
    var url = 'Jobtype/getUpdatedJobtype';
    this.SocketService.post(url, this.JobType.value).subscribe((result:any) => {
      if(result.status == true){
        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message);
        this.JobType.reset();
      }
      else {
            
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide();
        this.JobType.reset();
  
        
      }
      this.ngOnInit();
    });
    setTimeout(function () {
      window.location.reload();
    }, 2000);
      
      
  
  }
  ondelete(){
  this.NgxSpinnerService.show();
  var url = 'Jobtype/DeleteJobtype';
  let params = {
  Jobtype_gid: this.parameterValue
  }
  this.SocketService.getparams(url, params).subscribe((result:any) => {
  if(result.status == true){
  this.NgxSpinnerService.hide();
  this.ToastrService.success(result.message);
  this.ngOnInit();
  }
 else {
  this.ToastrService.warning(result.message);
  this.NgxSpinnerService.hide();
  this.ngOnInit();
  }  
});
setTimeout(function () {
  window.location.reload();
}, 2000);
}
  deletemodal(parameter: string) {
    this.parameterValue = parameter
  }
  
  close() {
    this.JobType.reset();
  }

}

