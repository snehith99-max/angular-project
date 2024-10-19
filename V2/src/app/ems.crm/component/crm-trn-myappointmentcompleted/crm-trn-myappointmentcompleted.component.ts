import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { map, share } from "rxjs/operators";
import { Router } from '@angular/router';
import { Subscription, Observable, timer } from 'rxjs';
import { DatePipe } from '@angular/common';
import { AES } from 'crypto-js';
@Component({
  selector: 'app-crm-trn-myappointmentcompleted',
  templateUrl: './crm-trn-myappointmentcompleted.component.html',
  styleUrls: ['./crm-trn-myappointmentcompleted.component.scss']
})
export class CrmTrnMyappointmentcompletedComponent {
  responsedata: any;
  getmyappointmenttilescount_lists: any;
  total_count: any;
  assigned_count: any;
  unassigned_count: any;
  completed_count: any;
  today_count: any;
  upcoming_count: any;
  expired_count: any;  
  leadbank_name: any;
  leadbank_gid: any;
  gettotalappointmentsummary_lists: any[] = [];
  getmyappointmentlog_lists: any[] = [];
  toDate: any;
  intervalId: any;
  time = new Date();
  rxTime = new Date();
  currentDayName: any;
  subscription!: Subscription;
  constructor( public service: SocketService,private NgxSpinnerService: NgxSpinnerService,private datePipe: DatePipe,
    private route: Router){

  }
  ngOnInit(){
    this.GetMyAppointmentTilesCount();
    this.GetCompletedAppointmentSummary();
    this.toDate = this.datePipe.transform(new Date(), 'dd-MM-yyyy');
    this.intervalId = setInterval(() => {
      this.time = new Date();
    }, 1000);

    this.subscription = timer(0, 1000)
    .pipe(
      map(() => new Date()),
      share()
    )
    .subscribe(time => {
      let hour = this.rxTime.getHours();
      let minuts = this.rxTime.getMinutes();
      let seconds = this.rxTime.getSeconds();
      //let a = time.toLocaleString('en-US', { hour: 'numeric', hour12: true });
      let NewTime = hour + ":" + minuts + ":" + seconds
      // console.log(NewTime);
      this.rxTime = time;
    });
  }
  GetMyAppointmentTilesCount() {
    var url = 'MyAppointment/GetMyAppointmentTilesCount'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.getmyappointmenttilescount_lists = this.responsedata.getmyappointmenttilescount_lists;
      this.total_count = this.getmyappointmenttilescount_lists[0].total_count;
      this.assigned_count = this.getmyappointmenttilescount_lists[0].assigned_count;
      this.unassigned_count = this.getmyappointmenttilescount_lists[0].unassigned_count;
      this.completed_count = this.getmyappointmenttilescount_lists[0].completed_count;
      this.upcoming_count = this.getmyappointmenttilescount_lists[0].upcoming_count;
      this.expired_count = this.getmyappointmenttilescount_lists[0].expired_count;
      this.today_count = this.getmyappointmenttilescount_lists[0].today_count;
      

    });
  }
  GetCompletedAppointmentSummary(){
    this.NgxSpinnerService.show();
    var url = 'MyAppointment/GetCompletedappointmentSummary'
     this.service.get(url).subscribe((result: any) => {
       $('#gettotalappointmentsummary_lists').DataTable().destroy();
       this.responsedata = result;
       this.gettotalappointmentsummary_lists = this.responsedata.gettotalappointmentsummary_lists;
       setTimeout(() => {
         $('#gettotalappointmentsummary_lists').DataTable();
       }, 1);
       this.NgxSpinnerService.hide();
   
   
     });
  }
  Onopen(param1: any){
    const secretKey = 'storyboarderp';
    const lspage1 = "My-Appointment-Complete";
    const lspage = AES.encrypt(lspage1, secretKey).toString();
    const leadbank_gid = AES.encrypt(param1, secretKey).toString();
    this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, lspage]);
  }
  Onlogmodal(param1: any,param2: any){
  
    let param = {
      leadbank_gid: param1
    }
      this.leadbank_name=param2
      var url='MyAppointment/GetMyAppointmentlogsummary'
      this.service.getparams(url,param).subscribe((result: any) =>{
        this.responsedata = result;
        this.getmyappointmentlog_lists = this.responsedata.getmyappointmentlog_lists;
      });
  }
}
