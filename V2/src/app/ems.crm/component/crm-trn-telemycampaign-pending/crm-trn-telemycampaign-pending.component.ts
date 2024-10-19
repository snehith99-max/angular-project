import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { ToastrService } from 'ngx-toastr';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgSelectModule } from '@ng-select/ng-select';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AES } from 'crypto-js';
import { DatePipe } from '@angular/common';
import { Subscription, Observable, timer } from 'rxjs';
import { map, share } from "rxjs/operators";
import flatpickr from 'flatpickr';
import { NgxSpinnerService } from 'ngx-spinner';
interface IMycalls {
  campaign_title: string;
  leadbank_name: string;
  contact_details: string;
  regionname: string;
  remarks: string;
  lead_notes: string;
  schedule_type: string;
  schedule: string;
  schedule_remarks: string;
  details: string;
  dialed_number: string;
  call_response: string;
  prosperctive_percentage: string;
  product_name: string;
  productgroup_name: string;
  call_feedback: string;
  productgroup_gid: string;
  product_gid: string;
  schedule_date: string;
  Select_Response: string;
  schedule_time: string;
  leadbank_gid: string;
  lead2campaign_gid: string;

  dialed_name:string;
}
@Component({
  selector: 'app-crm-trn-telemycampaign-pending',
  templateUrl: './crm-trn-telemycampaign-pending.component.html',
  styleUrls: ['./crm-trn-telemycampaign-pending.component.scss']
})
export class CrmTrnTelemycampaignPendingComponent {
  GetCallLogLead_list: any;
  GetLeadNoteDetails_list: any;
  notes_count: any;
  leadbank_gid: any;
  remarks: any;
  parameterValue: any;
  responsedata: any;
  parameterValue1: any;
  new_list: any[] = [];
  new_pending_list: any[] = [];
  followup_list: any[] = [];
  closed_list: any[] = [];
  drop_list: any[] = [];
  product_list: any[] = [];
  product_group_list: any[] = [];
  mycalls!: IMycalls;
  reactiveForm!: FormGroup;
  reactiveFormfollow!: FormGroup;
  /////
  page: any = 'schedule';
  mycallstilescount_list: any[] = [];
  upcomingschedule_count:any;
  schedule_count: any;
  newleads_count: any;
  followup_count: any;
  prospect_count: any;
  drop_count: any;
  pending_count: any;
  alllead_count: any;
  callresponse_list: any[] = [];
  person_name: any;
  schedulesummary_list:any[]=[];
  chart:any={};
  toDate: any;
  intervalId: any;
  time = new Date();
  rxTime = new Date();
  currentDayName: any;
  subscription!: Subscription;
  leadbank_name:any;
  internal_notes:any;
  reactiveFormappointment!: FormGroup;
  Getbussinessverticledropdown_list: any[] = [];
  showOptionsDivId: any;
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, private route: Router, public service: SocketService,private datePipe: DatePipe,private NgxSpinnerService: NgxSpinnerService) {
    const today = new Date();
    this.currentDayName = today.toLocaleDateString('en-US', { weekday: 'long' });
    this.mycalls = {} as IMycalls;
  }
  ngOnInit(): void {
    // this.GetNewSummary();
    this.GetPendingSummary();
    this.Getmycallstilescount();
    // this.GetFollowupSummary();
    // this.GetClosedSummary();
    // this.GetDropSummary();
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
    this.reactiveForm = new FormGroup({

      dialed_number: new FormControl(this.mycalls.dialed_number, [
        Validators.required,
        Validators.maxLength(15),
      ]),
      leadbank_gid: new FormControl(''),
      lead2campaign_gid: new FormControl(''),

      call_response: new FormControl(this.mycalls.call_response, [
        Validators.required,
        // Validators.pattern( '^[a-zA-Z]+$')
      ]),
      prosperctive_percentage: new FormControl(null),
      product_name: new FormControl(null),
      productgroup_name: new FormControl(null),
      call_feedback: new FormControl(''),
      productgroup_gid: new FormControl(''),
      dialed_name: new FormControl(this.mycalls.dialed_name, [
        Validators.required,
        Validators.maxLength(64),
      ]),
    }

    );

    this.reactiveFormfollow = new FormGroup({
      schedule_date: new FormControl(this.mycalls.schedule_date, [
        Validators.required,

      ]),
      schedule_time: new FormControl(this.mycalls.schedule_time, [
        Validators.required,

      ]),

      schedule_type: new FormControl(this.mycalls.schedule_type, [
        Validators.required,

      ]),
      schedule_remarks: new FormControl(''),


      leadbank_gid: new FormControl(''),
      lead2campaign_gid: new FormControl(''),
    });

    var api = 'Mycalls/GetProductdropdown'
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.product_list = this.responsedata.product_list3;
    });

    // var api6 = 'Mycalls/GetProductGroupdropdown'
    // this.service.get(api6).subscribe((result: any) => {
    // this.responsedata=result;
    //   this.product_group_list = this.responsedata.product_group_list1;
    // });
    var api8 = 'Mycalls/GetMycallsresponsedropdown';
    this.service.get(api8).subscribe((result:any) => {
      this.responsedata = result;
      this.callresponse_list = this.responsedata.mycallsresponse_list; 
      });
      var api = 'AppointmentManagement/Getbussinessverticledropdown';
      this.service.get(api).subscribe((result: any) => {
        this.Getbussinessverticledropdown_list = result.Getbussinessverticledropdown_list;
      });

      this.reactiveFormappointment = new FormGroup({
        appointment_timing: new FormControl(''),
        bussiness_verticle: new FormControl(null),
        lead_title: new FormControl(null),
        leadbank_gid: new FormControl(''),
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
  viewcalllog(parameter: string, parameter1: string, parameter2: string) {
     //this.reactiveForm.get("leadbank_gid")?.setValue(parameter);
     // this.reactiveForm.get("dialed_name")?.setValue(parameter1);
     // this.reactiveForm.get("dialed_number")?.setValue(parameter2);
     this.leadbank_name =parameter1;
     this.leadbank_gid = parameter;
     if (parameter != null) {
       this.NgxSpinnerService.show();
       let param = {
         leadbank_gid: this.leadbank_gid
       }
       var url = 'Mycalls/GetCallLogLead'
       this.service.getparams(url, param).subscribe((result: any) => {
         $('#GetCallLogLead_list').DataTable().destroy();
         this.responsedata = result;
         this.GetCallLogLead_list = this.responsedata.GetCallLogLead_list;
         this.NgxSpinnerService.hide();
         //console.log(this.entity_list)
         setTimeout(() => {
           $('#GetCallLogLead_list').DataTable();
         }, 1);
       })
     }
     else
     {
       this.GetCallLogLead_list =null;
     }
 
   }
  GetPendingSummary() {
    var api = 'Mycalls/GetPendingSummary'
    this.service.get(api).subscribe((result: any) => {
      $('#new_pending_list').DataTable().destroy();
      this.responsedata = result;
      this.new_pending_list = this.responsedata.new_pending_list;
      // //(this.entity_list)
      setTimeout(() => {
        $('#new_pending_list').DataTable();
      }, 1);
    });
  }

  getcall(mobile: any, leadbank_name: any) {
   var url = 'clicktocall/customercall'
    let params = {
      phone_number: mobile,
      user_name: leadbank_name
    }
    this.service.postparams(url, params).subscribe((result: any) => {

    });
  }
  get dialed_name() {
    return this.reactiveForm.get('dialed_name')!;
  }
  get dialed_number() {
    return this.reactiveForm.get('dialed_number')!;
  }
  get call_response() {
    return this.reactiveForm.get('call_response')!;
  }
  get prosperctive_percentage() {
    return this.reactiveForm.get('prosperctive_percentage')!;
  }
  get product_name() {
    return this.reactiveForm.get('product_name')!;
  }
  get productgroup_name() {
    return this.reactiveForm.get('productgroup_name')!;
  }
  get call_feedback() {
    return this.reactiveForm.get('call_feedback')!;
  }
  get schedule_date() {
    return this.reactiveFormfollow.get('schedule_date')!;
  }
  get schedule_time() {
    return this.reactiveFormfollow.get('schedule_time')!;
  }
  get schedule_type() {
    return this.reactiveFormfollow.get('schedule_type')!;
  }
  get schedule_remarks() {
    return this.reactiveFormfollow.get('schedule_remarks')!;
  }
  get appointment_timing() {
    return this.reactiveFormappointment.get('appointment_timing')!;
  }
  get bussiness_verticle() {
    return this.reactiveFormappointment.get('bussiness_verticle')!;
  }
  get lead_title() {
    return this.reactiveFormappointment.get('lead_title')!;
  }

  // public onsubmit(): void {
  //   if (this.reactiveForm.value.dialed_number != null && this.reactiveForm.value.call_response != null && this.reactiveForm.value.prosperctive_percentage != null) {

  //     for (const control of Object.keys(this.reactiveForm.controls)) {
  //       this.reactiveForm.controls[control].markAsTouched();
  //     }
  //      //(this.reactiveForm.value);
  //     var url = 'MyCalls/PostNewlog'
  //     this.service.post(url, this.reactiveForm.value).pipe().subscribe((result: any) => {

  //       if (result.status == false) {
  //         window.location.reload()
  //         this.ToastrService.warning(result.message)
  //         this.GetNewSummary();
  //         this.reactiveForm.reset();
  //       }
  //       else {
  //         this.reactiveForm.get("dialed_number")?.setValue(null);
  //         this.reactiveForm.get("call_response")?.setValue(null);
  //         this.reactiveForm.get("prosperctive_percentage")?.setValue(null);
  //         window.location.reload()
  //         this.ToastrService.success(result.message)
  //         this.GetNewSummary();
  //         this.reactiveForm.reset();
  //       }
  //       this.GetNewSummary();
  //       this.reactiveForm.reset();
  //     });

  //   }
  //   else {
  //     this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
  //   }
  // }

  public onsubmit2(): void {
    if (this.reactiveForm.value.dialed_number != null && this.reactiveForm.value.call_response != null && this.reactiveForm.value.prosperctive_percentage != null) {

      for (const control of Object.keys(this.reactiveForm.controls)) {
        this.reactiveForm.controls[control].markAsTouched();
      }
       //(this.reactiveForm.value);
      var url = 'MyCalls/PostNewlog'
      this.service.post(url, this.reactiveForm.value).pipe().subscribe((result: any) => {

        if (result.status == false) {
          window.location.reload()
          this.ToastrService.warning(result.message)
          this.GetPendingSummary();
          this.reactiveForm.reset();
        }
        else {
          this.reactiveForm.get("dialed_number")?.setValue(null);
          this.reactiveForm.get("call_response")?.setValue(null);
          this.reactiveForm.get("prosperctive_percentage")?.setValue(null);
          window.location.reload()
          this.ToastrService.success(result.message)
          this.GetPendingSummary();
          this.reactiveForm.reset();
        }
        this.GetPendingSummary();
        this.reactiveForm.reset();
      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }
  // public onsubmit3(): void {
  //   if (this.reactiveForm.value.dialed_number != null && this.reactiveForm.value.call_response != null && this.reactiveForm.value.prosperctive_percentage != null) {

  //     for (const control of Object.keys(this.reactiveForm.controls)) {
  //       this.reactiveForm.controls[control].markAsTouched();
  //     }
  //      //(this.reactiveForm.value);
  //     var url = 'MyCalls/PostFollowuplog'
  //     this.service.post(url, this.reactiveForm.value).pipe().subscribe((result: any) => {

  //       if (result.status == false) {
  //         window.location.reload()
  //         this.ToastrService.warning(result.message)
  //         this.GetFollowupSummary();
  //         this.reactiveForm.reset();
  //       }
  //       else {
  //         this.reactiveForm.get("dialed_number")?.setValue(null);
  //         this.reactiveForm.get("call_response")?.setValue(null);
  //         this.reactiveForm.get("prosperctive_percentage")?.setValue(null);
  //         window.location.reload()
  //         this.ToastrService.success(result.message)
  //         this.GetFollowupSummary();
  //         this.reactiveForm.reset();
  //       }
  //       this.GetFollowupSummary();
  //       this.reactiveForm.reset();
  //     });

  //   }
  //   else {
  //     this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
  //   }
  // }


  public schedule(): void {
    if (this.reactiveFormfollow.value.schedule_date != null && this.reactiveFormfollow.value.schedule_time != null) {

      for (const control of Object.keys(this.reactiveFormfollow.controls)) {
        this.reactiveFormfollow.controls[control].markAsTouched();
      }
      this.reactiveFormfollow.value;
      var url = 'MyCalls/PostFollowschedulelog'
      this.service.post(url, this.reactiveFormfollow.value).subscribe((result: any) => {

         //(this.reactiveFormfollow.value);

        if (result.status == false) {
          window.location.reload()
          this.ToastrService.warning(result.message)
          this.GetPendingSummary();
          this.reactiveFormfollow.reset();
        }
        else {
          this.reactiveFormfollow.get("schedule_date")?.setValue(null);
          this.reactiveFormfollow.get("schedule_time")?.setValue(null);
          window.location.reload()
          this.ToastrService.success(result.message)
          this.GetPendingSummary();
          this.reactiveFormfollow.reset();
        }
        this.GetPendingSummary();
        this.reactiveFormfollow.reset();
        window.location.reload()
      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }

  }


  onclose() {
    this.reactiveForm.reset();
    this.reactiveFormfollow.reset();
  }
  openModal1(parameter: string,parameter1: string,parameter2: string) {
    this.reactiveForm.get("leadbank_gid")?.setValue(parameter);
    this.reactiveForm.get("dialed_name")?.setValue(parameter1);
    this.reactiveForm.get("dialed_number")?.setValue(parameter2);
  
  }
  openModal2(parameter: string) {
    this.reactiveForm.get("leadbank_gid")?.setValue(parameter);
  }
  openModal3(parameter: string, parameter1: string) {
    this.reactiveFormfollow.get("leadbank_gid")?.setValue(parameter);
    this.reactiveFormfollow.get("lead2campaign_gid")?.setValue(parameter1);
  }
  openModal4(parameter: string) {
    this.reactiveForm.get("leadbank_gid")?.setValue(parameter);

  }
  // popmodal(parameter: string, parameter1: string) {
  //   this.internal_notes = parameter; // Access parameter directly
  //   this.leadbank_name = parameter1;
  // }
  popmodal(parameter: string, parameter1: string, parameter2: string, parameter3: string) {
    this.remarks = parameter; // Access parameter directly
    this.leadbank_name = parameter1;
    this.leadbank_gid = parameter2;
    this.notes_count = parameter3;
    if (this.notes_count != 0 || this.notes_count != "0") {
      this.NgxSpinnerService.show();
      let param = {
        leadbank_gid: this.leadbank_gid
      }
      var url = 'TelecallerManager/GetLeadNoteDetails'
      this.service.getparams(url, param).subscribe((result: any) => {
        $('#GetLeadNoteDetails_list').DataTable().destroy();
        this.responsedata = result;
        this.GetLeadNoteDetails_list = this.responsedata.GetLeadNoteDetails_list;
        this.NgxSpinnerService.hide();
        //console.log(this.entity_list)
        setTimeout(() => {
          $('#GetLeadNoteDetails_list').DataTable();
        }, 1);
      })
    }
    else
    {
      this.GetLeadNoteDetails_list =null;
    }
  }
  productname() {

    let product_gid = this.reactiveForm.get("product_name")?.value;



    let params = {

      product_gid: product_gid

    }

    var url = 'MyCalls/GetProductGroupdropdown'



    this.service.getparams(url, params).subscribe((result: any) => {

      this.responsedata = result;



      this.product_group_list = this.responsedata.product_group_list1;



    });

  }

  Onopen(param1: any, param2: any) {
    const secretKey = 'storyboarderp';
    const lspage1 = "My-teleNewpending";
    const lspage = AES.encrypt(lspage1, secretKey).toString();
     //(param1);
     //(param2);
    const leadbank_gid = AES.encrypt(param1, secretKey).toString();
    const appointment_gid = AES.encrypt('', secretKey).toString();
    const lead2campaign_gid = AES.encrypt(param2, secretKey).toString();
    this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,appointment_gid, lspage]);
  }

  openModallog3(parameter: string) {
    this.parameterValue1 = parameter
     
    this.reactiveFormfollow.get("leadbank_gid")?.setValue(this.parameterValue1.leadbank_gid);
    this.reactiveFormfollow.get("lead2campaign_gid")?.setValue(this.parameterValue1.lead2campaign_gid);
    this.person_name=this.parameterValue1.leadbank_name;
     //(this.person_name)
  
    this.Getshedulesummary()
  }

  changeSummary(data: string): void {

    this.page = data;
  }
  Getmycallstilescount(){
   
    var url = 'MyCalls/GetMycallstilescount'
    this.service.get(url).subscribe((result: any) => {
     this.responsedata=result;
     this.mycallstilescount_list = this.responsedata.mycallstilescount_list;
     this.schedule_count = this.mycallstilescount_list[0].schedule_count;
     this.newleads_count = this.mycallstilescount_list[0].newleads_count;
     this.followup_count = this.mycallstilescount_list[0].followup_count;
     this.prospect_count = this.mycallstilescount_list[0].prospect_count;
     this.drop_count = this.mycallstilescount_list[0].drop_count;
     this.pending_count = this.mycallstilescount_list[0].pending_count;
     this.alllead_count = this.mycallstilescount_list[0].alllead_count;
     this.upcomingschedule_count = this.mycallstilescount_list[0].upcomingschedule_count;
     const alllead_count = this.mycallstilescount_list.map((entry: { alllead_count: number; }) => entry.alllead_count);
 
     this.chart = {
      series: [alllead_count],
      chart: {
        height: 100,
        type: 'donut',
      },
      plotOptions: {
        pie: {
          donut: {
            labels: {
              show: true,
              name: {
                show: false
              },
             
            }
          }
        }
      },
      colors: [
        '#37647D',
        '#CE634B',
        '#418B7C',
        '#DDBA6A',
        '#80A473',
        '#DB9B4D',
      ],
      labels: ['All'],
      dataLabels: {
        enabled: false,
      },
      legend: {
        position: "top"
      },
    };
    //  this.chart = {
    //   series: [schedule_count,newleads_count,pending_count,followup_count,],
    //   chart: {
    //     type: "donut"
    //   },
    //   labels: ["Schedule", "New", "Pending", "Followup", "Prospect","Drop","All"],
    //   responsive: [
    //     {
    //       breakpoint: 480,
    //       options: {
    //         chart: {
    //           width: 200,height:100
    //         },
    //         legend: {
    //           position: "top"
    //         }
    //       }
    //     }
    //   ]
    // };
  


    });
  }
  Getshedulesummary() {
    var url = 'MyLead/GetSchedulelogsummary'
    let param = {
      leadbank_gid: this.parameterValue1.leadbank_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      // this.responsedata=result;
      this.schedulesummary_list = result.schedulesummary_list;
      // //(this.schedulesummary_list)
     //  //(this.callresponse_list[0].branch_gid)
      this.reactiveFormfollow.get("log_details")?.setValue(this.schedulesummary_list[0].log_details);
      this.reactiveFormfollow.get("log_legend")?.setValue(this.schedulesummary_list[0].log_legend);
    
      // //(this.reactiveFormfollow.value);
  
    });
  }
  onadd() {
    const secretKey = 'storyboarderp';
    const lspage1 = 'Mycalls-pending';
    const lspage = AES.encrypt(lspage1, secretKey).toString();
    this.route.navigate(['/crm/CrmTrnMycallsaddleads', lspage]);
  }
  getleadbankgid(parameter: string){
    debugger
 this.reactiveFormappointment.get("leadbank_gid")?.setValue(parameter);
  }
  onaddopportunity(){
    if(this.reactiveFormappointment.value.lead_title!=null && 
      this.reactiveFormappointment.value.appointment_timing != null){
        this.NgxSpinnerService.show();
        var url='Mycalls/PostAppointmentmycalls'
        this.service.post(url,this.reactiveFormappointment.value).subscribe((result: any) => {
          if(result.status == false){
            this.ToastrService.warning(result.message);
          }
          else{
            this.ToastrService.success(result.message);
          }
          this.GetPendingSummary();
          this.Getmycallstilescount();
        });
        this.NgxSpinnerService.hide();
    }
    else{
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !!');
    }
  }

  toggleOptions(leadbank_gid: any) {
    if (this.showOptionsDivId === leadbank_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = leadbank_gid;
    }
  }
}
