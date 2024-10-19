
import { Component, ElementRef, OnDestroy, OnInit, Renderer2 } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute, Route } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { Subscription, map, share, timer } from 'rxjs';
@Component({
  selector: 'app-otl-trn-toutletmanagerview',
  templateUrl: './otl-trn-toutletmanagerview.component.html',
  styleUrls: ['./otl-trn-toutletmanagerview.component.scss']
})
export class OtlTrnToutletmanagerviewComponent {
  revenue_list: any[] = []
  expense_list: any[] = []
  responsedata: any;
  daytracker_gid:any;
  tracker_gid:any;
  daytracker_list:any;
  edittracker_list:any;
  edittrackerexpence_list:any;
  edittrackerrevenue_list:any;
  otpverification_list:any;
  parameterValue1: any;
  constructor(private renderer: Renderer2, 
    private el: ElementRef, 
    public service: SocketService,
    private route: Router, 
    private router: ActivatedRoute, 
    private ToastrService: ToastrService,
    private NgxSpinnerService: NgxSpinnerService)  {
     }
     ngOnInit(): void {
        const daytracker_gid = this.router.snapshot.paramMap.get('daytracker_gid');
        this.daytracker_gid = daytracker_gid;
        const secretKey = 'storyboarderp';
        const deencryptedParam = AES.decrypt(this.daytracker_gid, secretKey).toString(enc.Utf8);
        console.log(deencryptedParam+"daytracker_gid");
        this.GetDaytrackerview(deencryptedParam);
     }
GetDaytrackerview(daytracker_gid:any){
  debugger
  var url = 'DayTracker/GetRevenueEditsummary';
  this.NgxSpinnerService.show();
  this.daytracker_gid = daytracker_gid;
  var params = {
    daytracker_gid: daytracker_gid
  };
  this.service.getparams(url,params).subscribe((result: any) => {
    this.responsedata = result;
    debugger;
    this.revenue_list = this.responsedata.revenue_list;
    console.log(this.revenue_list);
  });
  var url = 'DayTracker/GetExpenseEditSummary'
  this.service.getparams(url,params).subscribe((result: any) => {
    this.responsedata = result;
    this.expense_list = this.responsedata.expense_list;
    console.log(this.expense_list)
  });
  var url = 'DayTracker/Getviewdaytracker'
  this.service.getparams(url,params).subscribe((result: any) => {
    this.responsedata = result;
    this.daytracker_list = this.responsedata.daytrackersummary_list;
    console.log(this.daytracker_list)
  });
  var url = 'DayTracker/Getedittrackersummary'
  this.service.getparams(url,params).subscribe((result: any) => {
    this.responsedata = result;
    this.edittracker_list = this.responsedata.edittracker_list;
    console.log(this.edittracker_list)
  });
  this.NgxSpinnerService.hide();
}

Viewedittracker(trackerhis_gid:string){
  let param = {
    trackerhis_gid: trackerhis_gid
  }
this.NgxSpinnerService.show();
  var url = 'DayTracker/Getedittrackerdtl'
  this.service.getparams(url,param).subscribe((result: any) => {
    this.responsedata = result;
    this.edittrackerrevenue_list = this.responsedata.edittrackerrevenue_list;
    console.log(this.edittracker_list)
  });
  var url = 'DayTracker/Getedittrackerdtl1'
  this.service.getparams(url,param).subscribe((result: any) => {
    this.responsedata = result;
    this.edittrackerexpence_list = this.responsedata.edittrackerexpence_list;
    console.log(this.edittracker_list)
  })
  this.NgxSpinnerService.hide();
}

Approvaltracker(parameter:any){
  debugger
  this.parameterValue1 = parameter
  var params={
  daytracker_gid:this.parameterValue1,
  }
  var url = 'OutletManager/GetmanagerApproval'
  this.service.getparams(url,params).subscribe((result: any) => {
    this.otpverification_list = result.managerApproval_list
  });

}
Approve(parameter:any){
  debugger
  this.parameterValue1 = parameter
  var params={
  daytracker_gid:this.parameterValue1,
  edit_status:"Approved",
  }
  this.NgxSpinnerService.show()
  var url1 = 'OutletManager/PostManagerupdate'
  this.service.postparams(url1, params).subscribe((result: any) => {
    this.responsedata=result;
    if(result.status ==false){
      window.scrollTo({
        top: 0, 
      });
      this.ToastrService.warning(result.message)
      this.GetDaytrackerview(this.parameterValue1);
      this.NgxSpinnerService.hide()
    }
    else{
      window.scrollTo({
        top: 0, 
      });
      this.ToastrService.success(result.message)
      this.GetDaytrackerview(this.parameterValue1);
      this.NgxSpinnerService.hide()
    }
});
}
Reject(parameter:any){
  debugger
  this.parameterValue1 = parameter
  var params={
  daytracker_gid:this.parameterValue1,
  edit_status:"Rejected",
  }
  this.NgxSpinnerService.show()
  var url1 = 'OutletManager/PostManagerupdate'
  this.service.postparams(url1, params).subscribe((result: any) => {
    this.responsedata=result;
    if(result.status == false){

        window.scrollTo({
          top: 0, 
        });
      this.ToastrService.warning(result.message)
      this.GetDaytrackerview(this.parameterValue1);
      this.NgxSpinnerService.hide()
    }
    else{
      window.scrollTo({
        top: 0, 
      });
      this.ToastrService.success(result.message)
      this.GetDaytrackerview(this.parameterValue1);
      this.NgxSpinnerService.hide()
    }
});
}
}
