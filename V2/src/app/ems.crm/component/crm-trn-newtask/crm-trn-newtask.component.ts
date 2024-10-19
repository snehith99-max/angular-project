import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { ToastrService } from 'ngx-toastr';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { TabsetComponent } from 'ngx-bootstrap/tabs';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AES } from 'crypto-js';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { Subscription, Observable, timer } from 'rxjs';
import { DatePipe } from '@angular/common';
import { map, share } from "rxjs/operators";

interface IMyleads {

  campaign_title: string;
  leadbank_name: string;
  contact_details: string;
  region_name: string;
  leadstage_name: string;
  remarks: string;
  lead_notes: string;
  details: string;
  customer_type:string;
  leadbank_gid: string;
  leadbank_gid1: string;
  lead2campaign_gid: string;
  customer_address: string;
  schedule_type: string;
  schedule: string;
  ScheduleRemarks1: string;
  schedule_status: string;
  meeting_time: string;
  date_of_demo: string;
  contact_person: string;
  alternate_person: string;
  location: string;
  prosperctive_percentage: string;
  // schedule_remarks: string;
  postponed_date: string;
  meeting_time_postponed: string;
  drop_reason: string;
  log_gid: string;
  date_of_demo_online: string;
  meeting_time_online: string;
  contact_person_online: string;
  technical_assist: string;
  prosperctive_percentage_online: string;
  productgroup_name: string;
  productgroup_name1: string;
  product_name: string;
  product_name1:string;
  demo_remarks: string;
  product_gid: string;
  product_gid1:string;
  meeting_remarks_offline: string;
  productgroup_name_offline: string;
  product_name_offline: string;
  prosperctive_percentage_offline: string;
  schedule_type_offline: string;
  location_offline: string;
  visited_by: string;
  contact_person_offline: string;
  meeting_time_offline: string;
  date_of_visit_offline: string;
  regionname: string;
  dialed_number: string;
  call_response: string;
  call_feedback: string;
  productgroup_gid: string;
  productgroup_gid1: string;
  schedule_date: string;
  Select_Response: string;
  schedule_time: string;
  call_count: string;
  call_remarks: string;
}

@Component({
  selector: 'app-crm-trn-newtask',
  templateUrl: './crm-trn-newtask.component.html',
  styleUrls: ['./crm-trn-newtask.component.scss']
})

export class CrmTrnNewtaskComponent {
  parameterValue: any;
  responsedata: any;
  parameterValue1: any;
  myleadslist: any[] = [];
  inprogresslist: any[] = [];
  potentiallist: any[] = [];
  customerlist: any[] = [];
  alllist: any[] = [];
  droplist: any[] = [];
  today_list: any[] = [];
  new_list: any[] = [];
  upcoming_list: any[] = [];
  product_group_list: any[] = [];
  product_list: any[] = [];
  myleads!: IMyleads;
  MyLeadsCount_List: any[] = [];
  callresponse_list:any[] = [];
  schedulesummary_list: any[] = [];

  reactiveForm!: FormGroup;
  reactiveForm1!: FormGroup;
  reactiveForm2!: FormGroup;
  reactiveFormfollow!: FormGroup;
  reactiveFormPostponed!: FormGroup;
  reactiveFormDrop!: FormGroup;
  reactiveFormInprogress!: FormGroup;
  reactiveFormCustomer!: FormGroup;
  ScheduleType = [
  
    { type: 'Meeting', },
  
  ];
  movetodrop: any;
  leadbank_gid: any;
  lead2campaign_gid: any;
  internal_notes: any;
  ScheduleRemarks1:any;
  schedule_remarks:any;
  leadbank_name:any;
  formSubmitted: boolean = false;
  //////Date//////
  time = new Date();
  rxTime = new Date();
  currentDayName: any;
  subscription!: Subscription;
  toDate: any;
  intervalId: any;

  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService,
    private route: Router, public service: SocketService,private datePipe: DatePipe) {
      const today = new Date();
      this.currentDayName = today.toLocaleDateString('en-US', { weekday: 'long' });
      this.myleads = {} as IMyleads;
  }
  ngOnInit(): void {

    const options: Options = {
      // enableTime: true,
      dateFormat: 'Y-m-d',

    };
    this.GetNewSummary();
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
    this.reactiveForm2 = new FormGroup({

      dialed_number: new FormControl(this.myleads.dialed_number, [
        Validators.required,
        Validators.maxLength(10),
      ]),
      leadbank_gid: new FormControl(''),
      lead2campaign_gid: new FormControl(''),

      call_response: new FormControl(this.myleads.call_response, [
        Validators.required,
        // Validators.pattern( '^[a-zA-Z]+$')
      ]),
      leadbank_name: new FormControl(this.myleads.leadbank_name, [
        Validators.required,
        // Validators.pattern( '^[a-zA-Z]+$')
      ]),
      prosperctive_percentage: new FormControl(null),
      product_name: new FormControl(null),
      productgroup_name: new FormControl(null),
      call_feedback: new FormControl(''),
      productgroup_gid: new FormControl(''),

    }
    );

    this.reactiveFormfollow = new FormGroup({
      schedule_date: new FormControl(this.myleads.schedule_date, [
        Validators.required,
      ]),
      schedule_time: new FormControl(this.myleads.schedule_time, [
        Validators.required,
      ]),
      schedule_type: new FormControl(this.myleads.schedule_type, [
        Validators.required,
      ]),
      schedule_remarks: new FormControl(''),
      ScheduleRemarks1: new FormControl(''),
      leadbank_gid: new FormControl(''),
      lead2campaign_gid: new FormControl(''),
      log_details : new FormControl(''),
      log_legend : new FormControl(''),
    });
    this.reactiveForm1 = new FormGroup({
      leadbank_gid: new FormControl(''),
      lead2campaign_gid: new FormControl(''),
    });

    this.reactiveForm = new FormGroup({
      date_of_demo: new FormControl(this.myleads.date_of_demo, [
        Validators.required,
      ]),
      meeting_time: new FormControl(this.myleads.meeting_time, [
        Validators.required,
      ]),
      contact_person: new FormControl(this.myleads.contact_person, [
        Validators.required,
      ]),
      alternate_person: new FormControl(this.myleads.alternate_person, [
        Validators.required,
      ]),
      location: new FormControl(this.myleads.location, [
        Validators.required,
      ]),
      prosperctive_percentage: new FormControl(""),
      
      leadbank_gid: new FormControl(''),
      log_gid: new FormControl(''),
      schedulelog_gid: new FormControl(''),
    });


    // var api = 'MyLead/GetProductdropdown'
    // this.service.get(api).subscribe((result: any) => {
    //   this.responsedata = result;
    //   this.product_list = this.responsedata.product_list32;
    // });

    var api6 = 'MyLead/GetProductGroupdropdown'
    this.service.get(api6).subscribe((result: any) => {
      this.responsedata = result;
      this.product_group_list = this.responsedata.product_group_list12;
    });

    var api7 = 'MyLead/GetMyLeadsCount';
    this.service.get(api7).subscribe((result:any) => {
    this.responsedata = result;
    this.MyLeadsCount_List = this.responsedata.getMyLeadsCount_List; 
    console.log(this.MyLeadsCount_List,'testdata');
    }); 

    var api8 = 'MyLead/Getcallresponsedropdown';
    this.service.get(api8).subscribe((result:any) => {
      this.responsedata = result;
      this.callresponse_list = this.responsedata.callresponse_list; 
      console.log(this.MyLeadsCount_List,'testdata');
      });

      flatpickr('.date-picker', options);
  }

  onedit(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    console.log(param);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    console.log(encryptedParam);
    this.route.navigate(['/crm/CrmTrnAddtocustomer', encryptedParam])

  }

  //360//
Onopen(param1: any, param2: any) {
  const secretKey = 'storyboarderp';
  const lspage1 = "My-New";
  const lspage = AES.encrypt(lspage1, secretKey).toString();
  console.log(param1);
  console.log(param2);
  const leadbank_gid = AES.encrypt(param1, secretKey).toString();
  const lead2campaign_gid = AES.encrypt(param2, secretKey).toString();
  this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, lead2campaign_gid, lspage]);
}

  //*******close textfields******//
  get date_of_demo() {
    return this.reactiveForm.get('date_of_demo')!;
  }
  get meeting_time() {
    return this.reactiveForm.get('meeting_time')!;
  }
  get contact_person() {
    return this.reactiveForm.get('contact_person')!;
  }
  get alternate_person() {
    return this.reactiveForm.get('alternate_person')!;
  }
  get location() {
    return this.reactiveForm.get('location')!;
  }
  get prosperctive_percentage() {
    return this.reactiveForm.get('prosperctive_percentage')!;
  }
  // get schedule_remarks() {
  //   return this.reactiveForm.get('schedule_remarks')!;
  // }

  //*******Calllog & schedulelog*********//
  get dialed_number() {
    return this.reactiveForm2.get('dialed_number')!;
  }
  get call_response() {
    return this.reactiveForm2.get('call_response')!;
  }
  // get prosperctive_percentage() {
  //   return this.reactiveForm2.get('prosperctive_percentage')!;
  // }
  get product_name() {
    return this.reactiveForm2.get('product_name')!;
  }
  get productgroup_name() {
    return this.reactiveForm2.get('productgroup_name')!;
  }
  get call_feedback() {
    return this.reactiveForm2.get('call_feedback')!;
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
  // get  schedule_remarks() {
  //   return this.reactiveFormfollow.get('schedule_remarks')!;
  // }
  

//Remarks popup//
popmodal(parameter: string, parameter1: string) {
  this.internal_notes = parameter
  this.leadbank_name = parameter1;
} 

  //*New tab call log popup*//
  openModallog1(parameter: string) {
    this.parameterValue1 = parameter
    this.reactiveForm2.get("leadbank_gid")?.setValue(this.parameterValue1.leadbank_gid);
    this.reactiveForm2.get("leadbank_name")?.setValue(this.parameterValue1.leadbank_name);
    this.leadbank_name = this.parameterValue1.leadbank_name;
    this.reactiveForm2.get("dialed_number")?.setValue(this.parameterValue1.dialed_number);
  }
  
  //**schedule log popup**//
  openModallog3(parameter: string) {
    this.parameterValue1 = parameter
    this.reactiveFormfollow.get("leadbank_gid")?.setValue(this.parameterValue1.leadbank_gid);
    this.reactiveFormfollow.get("lead2campaign_gid")?.setValue(this.parameterValue1.lead2campaign_gid);
    this.reactiveFormfollow.get("leadbank_name")?.setValue(this.parameterValue1.leadbank_name);
    this.leadbank_name = this.parameterValue1.leadbank_name;
    this.Getshedulesummary(this.parameterValue1.leadbank_gid);
  }
 
  // //Add Lead page Routing//
  // onadd() {
  //   const secretKey = 'storyboarderp'
  //   const lspage1 = 'MyLeadsNewTasks';
  //   const lspage = AES.encrypt(lspage1, secretKey).toString();
  //   this.route.navigate(['/crm/CrmTrnLeadmasteradd', lspage]);

  // }
  productname() {
    let productgroup_gid = this.reactiveForm2.get("productgroup_name")?.value;
    let params = {
      productgroup_gid: productgroup_gid
    }
    var url = 'MyLead/GetProductdropdown'
    this.service.getparams(url, params).subscribe((result: any) => {
      this.responsedata = result;
      this.product_list = this.responsedata.product_list32;
    });
  }
 

  //**New**//
  GetNewSummary() {
    var url = 'MyLead/GetNewSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#new_list').DataTable().destroy();
      this.responsedata = result;
      this.new_list = this.responsedata.newSummary_list;
      //console.log(this.entity_list)
      setTimeout(() => {
        $('#new_list').DataTable();
      }, 1);
    });
  }

  onclose() {
    this.reactiveForm.reset();
    this.reactiveFormPostponed.reset();
    this.reactiveFormDrop.reset();
    this.reactiveForm2.reset();
    this.reactiveFormfollow.reset();
  }
  
  //********Post New Log submit **********//
  public onsubmitlog1(): void {
    if (this.reactiveForm2.value.dialed_number != null && this.reactiveForm2.value.call_response != null &&
      this.reactiveForm2.value.prosperctive_percentage != null) {

      for (const control of Object.keys(this.reactiveForm2.controls)) {
        this.reactiveForm2.controls[control].markAsTouched();
      }
      console.log(this.reactiveForm2.value);
      var url = 'MyLead/PostNewlog'
      this.service.post(url, this.reactiveForm2.value).pipe().subscribe((result: any) => {

        if (result.status == false) {
          window.location.reload()
          this.ToastrService.warning(result.message)
          this.GetNewSummary();
          this.reactiveForm2.reset();
        }
        else {
          this.reactiveForm2.get("dialed_number")?.setValue(null);
          this.reactiveForm2.get("call_response")?.setValue(null);
          this.reactiveForm2.get("prosperctive_percentage")?.setValue(null);
          window.location.reload()
          this.ToastrService.success(result.message)
          this.GetNewSummary();
          this.reactiveForm2.reset();
        }
        this.GetNewSummary();
        this.reactiveForm2.reset();
      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }
 
  public schedule(): void {
    if (this.reactiveFormfollow.value.schedule_date != null && this.reactiveFormfollow.value.schedule_time != null) {

      for (const control of Object.keys(this.reactiveFormfollow.controls)) {
        this.reactiveFormfollow.controls[control].markAsTouched();
      }
      this.reactiveFormfollow.value;
      var url = 'MyLead/PostNewschedulelog'
      this.service.post(url, this.reactiveFormfollow.value).subscribe((result: any) => {

        console.log(this.reactiveFormfollow.value);

        if (result.status == false) {
          this.ToastrService.warning(result.message)
          this.ToastrService.warning("false")
          window.location.reload()

          this.GetNewSummary();
          this.reactiveFormfollow.reset();
        }
        else {
          this.reactiveFormfollow.get("schedule_date")?.setValue(null);
          this.reactiveFormfollow.get("schedule_time")?.setValue(null);
          this.ToastrService.success(result.message)
          window.location.reload()
          this.GetNewSummary();
          this.reactiveFormfollow.reset();
        }
        this.GetNewSummary();
        this.reactiveFormfollow.reset();
      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }

  }
  onsubmitClose1() {
    if (!this.formSubmitted) {
      // Disable the submit button after 1 click
      this.formSubmitted = true;
    }
  }
 

//Schedule summary 
Getshedulesummary(leadbank_gid: any) {
  var url = 'MyLead/GetSchedulelogsummary'
  let param = {
    leadbank_gid: leadbank_gid
  }
  this.service.getparams(url, param).subscribe((result: any) => {
    // this.responsedata=result;
    this.schedulesummary_list = result.schedulesummary_list;
    console.log(this.schedulesummary_list)
   // console.log(this.callresponse_list[0].branch_gid)
    this.reactiveFormfollow.get("log_details")?.setValue(this.schedulesummary_list[0].log_details);
    this.reactiveFormfollow.get("log_legend")?.setValue(this.schedulesummary_list[0].log_legend);
  
    console.log(this.reactiveFormfollow.value);

  });
}
    ////Add Lead page Routing////
    onadd() {
      const secretKey = 'storyboarderp'
      const lspage1 = 'MyLead-newleads';
      const lspage = AES.encrypt(lspage1, secretKey).toString();
      this.route.navigate(['/crm/CrmTrnMyleadsaddlead', lspage]);
  
    }
}
