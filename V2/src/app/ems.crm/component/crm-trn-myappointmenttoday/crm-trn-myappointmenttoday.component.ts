import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { map, share } from "rxjs/operators";
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Subscription, Observable, timer } from 'rxjs';
import { DatePipe } from '@angular/common';
import { AES } from 'crypto-js';
import flatpickr from 'flatpickr';
import { Router } from '@angular/router';
@Component({
  selector: 'app-crm-trn-myappointmenttoday',
  templateUrl: './crm-trn-myappointmenttoday.component.html',
  styleUrls: ['./crm-trn-myappointmenttoday.component.scss'],
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
export class CrmTrnMyappointmenttodayComponent {
  responsedata: any;
  getmyappointmenttilescount_lists: any;
  total_count: any;
  assigned_count: any;
  unassigned_count: any;
  completed_count: any;
  today_count: any;
  upcoming_count: any;
  expired_count: any;
  reactiveFormclose!: FormGroup;
  reactiveFormPostponed!: FormGroup;
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
  New_count: any;
  prospect_count: any;
  potentials_count: any;
  drop_count: any;
  closed_count: any;
  showOptionsDivId: any;
  appointment_gid:any;
  Opportunitylog_gid:any;
  constructor( public service: SocketService,private NgxSpinnerService: NgxSpinnerService,private ToastrService: ToastrService,private datePipe: DatePipe,
    private route: Router){

  }
  ngOnInit(){
    this.reactiveFormclose = new FormGroup({
      schedule_remarks: new FormControl(''),
      Opportunitylog_gid: new FormControl(''),
      appointment_gid: new FormControl(''),
    });
    this.reactiveFormPostponed = new FormGroup({

      postponed_date: new FormControl(''),
      meeting_time_postponed: new FormControl(''),
      schedule_remarks: new FormControl(''),
      Opportunitylog_gid: new FormControl(''),
      appointment_gid: new FormControl(''),
    });
    this.GetMyAppointmentTilesCount();
    this.GetTodayAppointmentSummary();
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
    const today = new Date();
    const minDate = today;
    const Options = {
      enableTime: true,
      dateFormat: 'Y-m-d H:i:S',
      minDate: minDate,
      defaultDate: today,
      minuteIncrement: 1
    };
    flatpickr('.date-picker', Options);
    
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
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
      this.New_count = this.getmyappointmenttilescount_lists[0].New_count;
      this.prospect_count = this.getmyappointmenttilescount_lists[0].prospect_count;
      this.potentials_count = this.getmyappointmenttilescount_lists[0].potentials_count;
      this.drop_count = this.getmyappointmenttilescount_lists[0].drop_count;
      this.closed_count = this.getmyappointmenttilescount_lists[0].closed_count;
      

    });
  }
  GetTodayAppointmentSummary(){
    this.NgxSpinnerService.show();
    var url = 'MyAppointment/GetTodayappointmentSummary'
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
  Onopen(param1: any,param2: any){
    const secretKey = 'storyboarderp';
    const lspage1 = "My-Appointment-Today";
    const lspage = AES.encrypt(lspage1, secretKey).toString();
    const leadbank_gid = AES.encrypt(param1, secretKey).toString();
    const appointment_gid = AES.encrypt(param2, secretKey).toString();
    this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, appointment_gid, lspage]);
  }
   //*******close textfields******//
   get schedule_remarks1() {
    return this.reactiveFormclose.get('schedule_remarks')!;
  }
  //*****postponed textfields******//
  get postponed_date() {
    return this.reactiveFormPostponed.get('postponed_date')!;
  }
  get meeting_time_postponed() {
    return this.reactiveFormPostponed.get('meeting_time_postponed')!;
  }
  Onlogmodal(param1: any,param2: any,param3:any,param4:any){
  
    let param = {
      leadbank_gid: param1
    }
      this.leadbank_name=param2
      this.Opportunitylog_gid = param3;
      this.appointment_gid = param4;
      this.reactiveFormPostponed.get("Opportunitylog_gid")?.setValue(this.Opportunitylog_gid);
      this.reactiveFormPostponed.get("appointment_gid")?.setValue(this.appointment_gid);
      this.reactiveFormclose.get("Opportunitylog_gid")?.setValue(this.Opportunitylog_gid);
      this.reactiveFormclose.get("appointment_gid")?.setValue(this.appointment_gid);
      var url='MyAppointment/GetMyAppointmentlogsummary'
      this.service.getparams(url,param).subscribe((result: any) =>{
        this.responsedata = result;
        this.getmyappointmentlog_lists = this.responsedata.getmyappointmentlog_lists;
      });
  }
  onsubmitPostponed(){
    var url='MyAppointment/Poststatuspostpone'
    this.service.post(url,this.reactiveFormPostponed.value).subscribe((result: any) => {
      if(result.status == false){
        this.ToastrService.warning(result.message);
      }
      else{
        this.ToastrService.success(result.message);
      }
         this.GetMyAppointmentTilesCount();
    this.GetTodayAppointmentSummary();
    });
  }
  onsubmitClose(){
    var url='MyAppointment/Postsstatusclose'
    this.service.post(url,this.reactiveFormclose.value).subscribe((result: any) => {
      if(result.status == false){
        this.ToastrService.warning(result.message);
      }
      else{
        this.ToastrService.success(result.message);
      }
         this.GetMyAppointmentTilesCount();
    this.GetTodayAppointmentSummary();
    });
  }
  onclose() {
  }
  toggleOptions(appointment_gid: any) {
    if (this.showOptionsDivId === appointment_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = appointment_gid;
    }
  }
}
