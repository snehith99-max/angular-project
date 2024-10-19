import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES } from 'crypto-js';
import { Router } from '@angular/router';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import flatpickr from 'flatpickr';

@Component({
  selector: 'app-crm-trn-myappointmentnew',
  templateUrl: './crm-trn-myappointmentnew.component.html',
  styleUrls: ['./crm-trn-myappointmentnew.component.scss']
})
export class CrmTrnMyappointmentnewComponent {
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
  New_count: any;
  prospect_count: any;
  potentials_count: any;
  drop_count: any;
  closed_count: any;
  showOptionsDivId: any;
  reactiveFormfollow!: FormGroup;
  parameterValue1:any;
  schedulesummary_list1: any[] = [];

  constructor( public service: SocketService,private NgxSpinnerService: NgxSpinnerService,private ToastrService: ToastrService,
    private route: Router){

  }
  ngOnInit(){
    this.reactiveFormfollow = new FormGroup({
      schedule_date: new FormControl(''),
      schedule_time: new FormControl(''),
      schedule_type: new FormControl(''),
      schedule_remarks: new FormControl(''),
      ScheduleRemarks1: new FormControl(''),
      leadbank_gid: new FormControl(''),
      lead2campaign_gid: new FormControl(''),
      assignedto_gid: new FormControl(''),
      appointment_gid: new FormControl(''),
    
    });
    this.GetMyAppointmentTilesCount();
    this.GetAppointmentSummary();
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
  GetAppointmentSummary(){
    this.NgxSpinnerService.show();
    var url = 'MyAppointment/GetNewappointmentSummary'
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
  Onopen(param1: any,param2:any){
    const secretKey = 'storyboarderp';
    const lspage1 = "My-Appointment-New";
    const lspage = AES.encrypt(lspage1, secretKey).toString();
    const leadbank_gid = AES.encrypt(param1, secretKey).toString();
    const appointment_gid = AES.encrypt(param2, secretKey).toString();
    this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, appointment_gid, lspage]);
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
  toggleOptions(appointment_gid: any) {
    if (this.showOptionsDivId === appointment_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = appointment_gid;
    }
  }
  openModallog3(parameter: string) {
    this.parameterValue1 = parameter
    this.reactiveFormfollow.get("leadbank_gid")?.setValue(this.parameterValue1.leadbank_gid);
    this.reactiveFormfollow.get("appointment_gid")?.setValue(this.parameterValue1.appointment_gid);
    this.reactiveFormfollow.get("assignedto_gid")?.setValue(this.parameterValue1.assignedto_gid);
    this.reactiveFormfollow.get("leadbank_name")?.setValue(this.parameterValue1.leadbank_name);
    this.leadbank_name = this.parameterValue1.leadbank_name;
    this.Getshedulesummary(this.parameterValue1.appointment_gid);

  }
  get schedule_type() {
    return this.reactiveFormfollow.get('schedule_type')!;
  }
  get schedule_date() {
    return this.reactiveFormfollow.get('schedule_date')!;
  }
  get schedule_time() {
    return this.reactiveFormfollow.get('schedule_time')!;
  }
  Getshedulesummary(appointment_gid: any) {
    var url = 'MarketingManager/GetSchedulelogsummary'
    let param = {
      appointment_gid: appointment_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      // this.responsedata=result;
      this.schedulesummary_list1 = result.schedulesummary_list1;
      console.log(this.schedulesummary_list1)
      // console.log(this.callresponse_list[0].branch_gid)
      this.reactiveFormfollow.get("log_details")?.setValue(this.schedulesummary_list1[0].log_details);
      this.reactiveFormfollow.get("log_legend")?.setValue(this.schedulesummary_list1[0].log_legend);

      console.log(this.reactiveFormfollow.value);

    });
  }
  oncloseschedule() {
    this.reactiveFormfollow.reset();
  }
  onsubmitschedule() {
    this.NgxSpinnerService.show();
    console.log(this.reactiveFormfollow.value);
    if (this.reactiveFormfollow.value.schedule_date != null || this.reactiveFormfollow.value.schedule_date != '') {
      var url1 = 'MarketingManager/PostManagerSchedule'
      this.service.post(url1, this.reactiveFormfollow.value).subscribe((result: any) => {
        console.log(this.reactiveFormfollow.value);
        if (result.status == false) {
          //window.location.reload()
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.warning(result.message)
          this.NgxSpinnerService.hide();
          this.reactiveFormfollow.reset();
        }
        else {
          this.reactiveFormfollow.get("schedule_date")?.setValue(null);
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.success(result.message)
          this.NgxSpinnerService.hide();
          this.reactiveFormfollow.reset();
        }
        this.reactiveFormfollow.reset();
      });
    }
    else{
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ');
    }
    window.location.reload();

  }
}
