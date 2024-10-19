import { Component, OnInit ,Input} from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ActivatedRoute,Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { SelectionModel } from '@angular/cdk/collections';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { NgbTimepickerModule, NgbTimeStruct } from '@ng-bootstrap/ng-bootstrap';
import { param } from 'jquery';
import { NgxSpinnerService } from 'ngx-spinner';

export class ICampaign {
  campaign_gid: string = "";
  campaign_list: string[] = [];
  assign_user: string = "";
  team_name: string = "";
  team_member: string = "";
  schedule_remarks: string = "";
  schedule_date: string = "";
  schedule_time: string = "";
  schedule_type: string = "";
}

interface ITransfer {
  team_name: string;
  team_member: string;
  schedule_date: string;
  schedule_time: string;
  schedule_type: string;
  schedule_remarks: string;

}

@Component({
  selector: 'app-crm-trn-teleteamview',
  templateUrl: './crm-trn-teleteamview.component.html',
  styleUrls: ['./crm-trn-teleteamview.component.scss']
})
export class CrmTrnTeleteamviewComponent {
  @Input() number: number = 1;
  marketingmanager_list: any;
  managerdetail_list: any;
  totallist: any[] = [];
  isCollapsed = false;
  public isOpen = true;
  assigned_leads: any;
  Lapsed_Leads: any;
  Longest_Leads: any;
  responsedata: any;
  internal_notes: any;
  leadbank_name: any;
 
  page: any='Total Lead';
  reactiveFormfollow!: FormGroup;
  //reactiveFormSchedule!: FormGroup;
  reactiveFormTransfer!: FormGroup;
  reactiveFormdrop!: FormGroup;
  ScheduleType = [

    { type: 'Meeting', },
    { type: 'Call', },

  ];
  transfer: ITransfer = {
    team_name: '',
    team_member: '',
    schedule_date: '',
    schedule_time: '',
    schedule_type: '',
    schedule_remarks: ''
  };

  countlist: any[] = [];
  CurObj: ICampaign = new ICampaign();
  selection = new SelectionModel<ICampaign>(true, []);
  pick: Array<any> = [];
  assign_user: any;
  parameterValue1: any;
  employee_list: any;
  teamname_list: any;
  mailaddress: any;
  mail_id: any;
  isSubmitting: any;
  schedulesummary_list1: any[] = [];
  teamdetails: any[] = [];
  totaltilecountdetails: any[] = [];
  totallapsedlongest: any[] = [];
  ///////
  leadchartcount: any = {};
  saleschartcountlist: any[] = [];
  saleschartcount: any = {};
  quotationcountlist: any[] = [];
  quotationchartcount: any = {};
  enquirychartcountlist: any[] = [];
  enquirychartcount: any = {};
  response_data: any;
  teamactivitysummary_list: any[] = [];
  selectedChartType: any;
  chartscountsummary_list: any[] = [];
  Performencechart_list: any[] = [];
  WebsiteUserchart: any = {};
  WebsiteSessionchart: any = {};
  parameter1: any;
  chartcountlist: any[] = [];
  months: any;
  new_leads: any;
  pending_calls: any;
  follow_up: any;
  prospect: any;
  campaign_gid: any;
  campaign_name: any;
  team_prefix: any;
  showOptionsDivId: any;
  reactiveFormappointment!:FormGroup;  
  Getbussinessverticledropdown_list: any[] = [];



  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService,
    public service: SocketService,  private router: ActivatedRoute, private route: Router,  private NgxSpinnerService: NgxSpinnerService) {

  }
  ngOnInit(): void {
    // this.GetMarketingManagerSummary();
    // this.GetMarketingManagerTotalSummary();
    // this.Getteamcount();
    this.GetTotaltilecount();
    // this.GetTotallapsedlongest();
    // this.Getleadchartcount();
    // this.Getteamactivitysummary();
    const campaign_gid = this.router.snapshot.paramMap.get('encryptedParam');
    this.campaign_gid = campaign_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.campaign_gid, secretKey).toString(enc.Utf8);
      this.GetTelecallerManagerTotalSummary(deencryptedParam);
    this.GetNewleadchartcount();
    this.GetTeamPerformencechart();
    this.GetTelecallerManagerSummary();

    this.reactiveFormTransfer = new FormGroup({
      team_name: new FormControl(this.transfer.team_name, [
        Validators.required,
      ]),
      team_member: new FormControl(this.transfer.team_member, [
        Validators.required,
      ]),
      leadbank_gid: new FormControl(''),
      lead2campaign_gid: new FormControl(''),
      assignedto_gid: new FormControl(''),
      leadbank_name: new FormControl(''),
    });

    this.reactiveFormfollow = new FormGroup({
      schedule_date: new FormControl(this.transfer.schedule_date, [
        Validators.required,
      ]),
      schedule_time: new FormControl(this.transfer.schedule_time, [
        Validators.required,
      ]),
      schedule_type: new FormControl(this.transfer.schedule_type, [
        Validators.required,
      ]),
      schedule_remarks: new FormControl(''),
      ScheduleRemarks1: new FormControl(''),
      leadbank_gid: new FormControl(''),
      lead2campaign_gid: new FormControl(''),
      assignedto_gid: new FormControl(''),
      leadbank_name: new FormControl(''),
    });
    this.reactiveFormdrop = new FormGroup({
      drop_remarks: new FormControl('',[Validators.required,]),
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

    var api1 = 'TelecallerManager/GetTelecallerCallerTeamlist'
    this.service.get(api1).subscribe((result: any) => {
      this.teamname_list = result.telecallerteam_list;
      //console.log(this.branch_list)
    });
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
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
 getleadbankgid(parameter: string) {
     
    this.reactiveFormappointment.get("leadbank_gid")?.setValue(parameter);
    var api = 'AppointmentManagement/Getbussinessverticledropdown';
    this.service.get(api).subscribe((result: any) => {
      this.Getbussinessverticledropdown_list = result.Getbussinessverticledropdown_list;
    });
  }
  
  
  onclose() {
  
    this.reactiveFormappointment.reset();
  }
  onaddopportunity() {
    if (this.reactiveFormappointment.value.lead_title != null &&
      this.reactiveFormappointment.value.appointment_timing != null) {
      this.NgxSpinnerService.show();
      var url = 'Mycalls/PostAppointmentmycalls'
      this.service.post(url, this.reactiveFormappointment.value).subscribe((result: any) => {
        if (result.status == false) {
          this.ToastrService.warning(result.message);
        }
        else {
          this.ToastrService.success(result.message);
        }
        this.GetTotaltilecount();
      });
      this.NgxSpinnerService.hide();
    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !!');
    }
  }
  teamname() {
    let team_gid = this.reactiveFormTransfer.get("team_name")?.value;
    let param = {
      team_gid: team_gid
    }
    var url = 'TelecallerManager/GetTelecallerCallerEmployeelist'
    this.service.getparams(url, param).subscribe((result: any) => {
      this.employee_list = result.teleemployee_list;
    });
  }
  Onteamview(campaign_gid:any){
    const secretKey = 'storyboarderp';
    const param = (campaign_gid);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.route.navigate(['/crm/CrmTrnTeleteamview', encryptedParam])
    const deencryptedParam = AES.decrypt(encryptedParam, secretKey).toString(enc.Utf8);
    this.GetTelecallerManagerTotalSummary(deencryptedParam);
  }
  
  //Tiles count//
  GetMarketingManagerSummary() {
    var url = ''
    this.service.get(url).subscribe((result: any) => {
      $('#countlist').DataTable().destroy();
      this.responsedata = result;
      this.countlist = this.responsedata.marketingmanager_lists;
      //console.log(this.entity_list)
      setTimeout(() => {
        $('#countlist').DataTable();
      }, 1);
    });
  }
  //Get Total tile count
  GetTotaltilecount() {
    
    var url = 'TelecallerManager/Getteamcount'
    this.service.get(url).subscribe((result: any) => {
      
      this.responsedata = result;
      console.log(this.responsedata,"check")

      this.totaltilecountdetails = this.responsedata.teletotaltilecount_lists;
      //console.log(this.entity_list)
      // setTimeout(() => {
      //   $('#totaltilecountdetails').DataTable();
      // }, 1);
    });
   
  }
  //Team count
  Getteamcount() {
    var url = ''
    this.service.get(url).subscribe((result: any) => {
      $('#teamdetails').DataTable().destroy();
      this.responsedata = result;
      this.teamdetails = this.responsedata.teamdetails;
      //console.log(this.entity_list)
      setTimeout(() => {
        $('#teamdetails').DataTable();
      }, 1);
    });
  }


  //Get Total lapsed longest count
  // GetTotallapsedlongest() {
  //   var url = ''
  //   this.service.get(url).subscribe((result: any) => {
  //     $('#totaltilecountdetails').DataTable().destroy();
  //     this.responsedata = result;
  //     this.totallapsedlongest = this.responsedata.totallapsedlongest;
  //     //console.log(this.entity_list)
  //     // setTimeout(() => {
  //     //   $('#totaltilecountdetails').DataTable();
  //     // }, 1);

  //     console.log(this.totallapsedlongest)
  //   });
  // }

  //Get Marketing Manager Total summary//
  GetTelecallerManagerTotalSummary(deencryptedParam:any) {
    this.NgxSpinnerService.show();
    let param = {
      campaign_gid : deencryptedParam
    }
    var url = 'TelecallerManager/GetTelecallerTeamViewSummary'
    this.service.getparams(url,param).subscribe((result: any) => {
      $('#totallist').DataTable().destroy();
      this.responsedata = result;
      this.totallist = this.responsedata.telecallermanager_totallists;
      this.campaign_name = this.totallist[0].campaign_title;
      this.team_prefix = this.totallist[0].team_prefix;
      this.NgxSpinnerService.hide();
      //console.log(this.entity_list)
      setTimeout(() => {
        $('#totallist').DataTable();
      }, 1);
    });
  }

  //360 view
  Onopen(param1: any, param2: any, param3: any,param4:any) {
    debugger
    this.mail_id = param3.split('/');
    this.mailaddress = this.mail_id[2]
    localStorage.setItem('mailaddress', this.mailaddress)
    const secretKey = 'storyboarderp';
    const lspage1 = "MM-teleteamview";
    const lspage = AES.encrypt(lspage1, secretKey).toString();
    const leadbank_gid = AES.encrypt(param1, secretKey).toString();
    const lead2campaign_gid = AES.encrypt(param2, secretKey).toString();
    const appointment_gid = AES.encrypt(param4, secretKey).toString();
    this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,appointment_gid, lspage]);
  }

  // ondetail(campaign_gid: any) {
  //   var url = 'MarketingManager/GetManagerSummaryDetailTable'
  //   let param = {
  //     campaign_gid: campaign_gid
  //   }
  //   this.service.getparams(url, param).subscribe((result: any) => {
  //     $('#managerdetail_list').DataTable().destroy();
  //     // this.responsedata=result;
  //     this.managerdetail_list = result.managerDetailTable_lists;
  //     console.log('details', this.managerdetail_list)
  //     setTimeout(() => {
  //       $('#managerdetail_list').DataTable();
  //     }, 1);

  //   });
  // }

  popmodal(parameter: string, parameter1: string) {
    this.internal_notes = parameter; // Access parameter directly
    this.leadbank_name = parameter1;
  }

  onedit(params: any) {
    const secretKey = 'storyboarderp';
    // const param = (params);
    const campaign_gid = AES.encrypt(params.campaign_gid, secretKey).toString();
    const assign_to = AES.encrypt(params.assign_to, secretKey).toString();
    const stage1 = "Total";
    const stage = AES.encrypt(stage1, secretKey).toString();
    this.route.navigate(['/crm/CrmTrnCampaignmanager', campaign_gid, assign_to, stage])
  }
  //Schedule & Transfer//
  get team_name() {
    return this.reactiveFormTransfer.get('team_name')!;
  }
  get team_member() {
    return this.reactiveFormTransfer.get('team_member')!;
  }
  get assignedto_gid() {
    return this.reactiveFormTransfer.get('assignedto_gid')!;
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
  get drop_remarks() {
    return this.reactiveFormdrop.get('drop_remarks')!;
  }

  //**schedule log popup**//
  openModallog3(parameter: string) {
    debugger
    this.parameterValue1 = parameter
    this.reactiveFormfollow.get("leadbank_gid")?.setValue(this.parameterValue1.leadbank_gid);
    this.reactiveFormfollow.get("lead2campaign_gid")?.setValue(this.parameterValue1.lead2campaign_gid);
    this.reactiveFormfollow.get("assignedto_gid")?.setValue(this.parameterValue1.assignedto_gid);
    this.reactiveFormfollow.get("leadbank_name")?.setValue(this.parameterValue1.leadbank_name);
    this.leadbank_name = this.parameterValue1.leadbank_name;
    this.Getshedulesummary(this.parameterValue1.leadbank_gid);

  }
  //**Transfer log popup**//
  openModallog4(parameter: string) {
    debugger
    this.parameterValue1 = parameter
    this.reactiveFormTransfer.get("leadbank_gid")?.setValue(this.parameterValue1.leadbank_gid);
    this.reactiveFormTransfer.get("lead2campaign_gid")?.setValue(this.parameterValue1.lead2campaign_gid);
    this.reactiveFormTransfer.get("assignedto_gid")?.setValue(this.parameterValue1.assignedto_gid);
    this.reactiveFormTransfer.get("leadbank_name")?.setValue(this.parameterValue1.leadbank_name);
    this.leadbank_name = this.parameterValue1.leadbank_name;
  }

  //Schedule submit//
  onsubmitschedule() {
    debugger
    this.NgxSpinnerService.show();
    console.log(this.reactiveFormfollow.value);
    if (this.reactiveFormfollow) {
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
          this.reactiveFormfollow.get("schedule_time")?.setValue(null);
          // window.location.reload()
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
    else {
      console.log("Form is not valid");
      return;
    }

  }
  // Transfer submit
  OnTransfer() {
    this.NgxSpinnerService.show();
debugger
    console.log(this.reactiveFormTransfer.value);
    const url1 = 'TelecallerManager/PostTeleMoveToTransfer';

    this.service.post(url1, this.reactiveFormTransfer.value).pipe().subscribe((result: any) => {
      window.scrollTo({
        top: 0,
      });

      if (result.status == false) {
        this.ToastrService.warning('Error While Transferring Lead');
      } else {
        this.ToastrService.success('Lead Transferred Successfully');
      }

      this.NgxSpinnerService.hide();

      // Delayed the reload by 2 seconds 
      setTimeout(() => {
        window.location.reload();
      }, 2000);
    });
  }


  //Delete or Move to Drop//
  openModallog5(gid: string){
    this.parameter1 = gid
   }
 
   OnBin() {
     this.reactiveFormdrop.value.leadbank_gid = this.parameter1;
     this.NgxSpinnerService.show();
     var url1 = 'TelecallerManager/TeleLeadMoveToBin'
     this.service.post(url1, this.reactiveFormdrop.value).subscribe((result: any) => {
       if (result.status == false) {
         this.ToastrService.warning('Error While Lead Moved to MyBin')
         this.NgxSpinnerService.hide();
       }
       else {
         this.ToastrService.success('Lead Moved to MyBin Successfully')
         this.NgxSpinnerService.hide();
         window.location.reload();
       }
     });
   }

  oncloseschedule() {
    this.reactiveFormfollow.reset();
  }
  onclosetransfer() {
    this.reactiveFormTransfer.reset();
  }
  onclosedrop() {
    this.reactiveFormdrop.reset();
  }

  //Schedule summary 
  Getshedulesummary(leadbank_gid: any) {
    var url = 'TelecallerManager/GetSchedulelogsummary'
    let param = {
      leadbank_gid: leadbank_gid
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

  GetNewleadchartcount() {

    var url = 'TelecallerManager/GetNewleadchartcount'
    this.service.get(url).subscribe((result: any) => {
      this.response_data = result;
      this.chartcountlist = this.response_data.telechartscount_list;
      this.months = this.chartcountlist.map((entry: { months: any }) => entry.months),
      this.new_leads = this.chartcountlist.map((entry: { new_leads: any }) => entry.new_leads)
      this.pending_calls = this.chartcountlist.map((entry: { pending_calls: any }) => entry.pending_calls)
      this.follow_up = this.chartcountlist.map((entry: { follow_up: any }) => entry.follow_up),
      this.prospect = this.chartcountlist.map((entry: { prospect: any }) => entry.prospect)

      // Initialize chart options
      this.leadchartcount = {
        chart: {
          type: 'bar',
          height: 360, // Adjust the height of the chart as needed
          width: '100%',
          background: 'White',
          foreColor: '#0F0F0F',
          toolbar: {
            show: false, // Set to false to hide the toolbar/menu icon
          },
        },
        colors: ['#8062D6', '#FFD54F', '#66BB6A', '#EF5350', '#0F0F0F'], // Use a set of colors for better combinations
        plotOptions: {
          bar: {
            horizontal: false,
            columnWidth: '1%', // Adjust the width of the bars
            borderRadius: 0, // Add some border radius for a more modern look
          },
        },
        dataLabels: {
          enabled: false, // Disable data labels for a cleaner look
        },
        stroke: {
          show: true,
          width: 2,
          colors: ['transparent'],
        },
        xaxis: {
          categories: this.months,
          labels: {
            style: {
              fontWeight: 'bold',
              fontSize: '14px',
              //colors: ['#FF5733', '#33FF57', '#5733FF', '#FFFF33'], // Set different colors for each label
            },
          },
        },
        yaxis: {
          title: {
            text: 'Weeks',
            style: {
              fontWeight: 'bold',
              fontSize: '14px',
              color: '#0F0F0F', // Set a different color for the y-axis title
            },
          },
        },
        series: [
          {
            name: 'Awareness',
            data: this.new_leads,
            color: '#CE7F92', //#1a70d9
          },
          {
            name: 'Pending Calls',
            data: this.pending_calls,
            color: '#747C8C', //#1a70d9
          },
          {
            name: 'Interest',
            data: this.follow_up,
            color: '#667967', //#1a70d9
          },
          {
            name: 'Consideration',
            data: this.prospect,
            color: '#B4584B', //#1a70d9
          },
        ],
        legend: {
          position: "top",
          offsetY:5
        }
      };


    })
  }
//   GetPendingCallsleadchartcount() {
// debugger
//     var url = 'TelecallerManager/GetPendingCallsleadchartcount'
//     this.service.get(url).subscribe((result: any) => {
//       this.response_data = result;
//       debugger
//       this.quotationcountlist = this.response_data.telechartscount_list;
//       const data = this.quotationcountlist.map((entry: { quotationmonthcount: any; }) => entry.quotationmonthcount);
//       const categories = this.quotationcountlist.map((entry: { quotationmonth: any; }) => entry.quotationmonth);
// debugger
//       // Initialize chart options
//       this.quotationchartcount = {
//         chart: {
//           type: 'bar',
//           height: 360, // Adjust the height of the chart as needed
//           // width: 600,
//           background: 'White',
//           foreColor: '#0F0F0F',
//           toolbar: {
//             show: false, // Set to false to hide the toolbar/menu icon
//           },
//         },
//         colors: ['#8062D6', '#FFD54F', '#66BB6A', '#EF5350', '#0F0F0F'], // Use a set of colors for better combinations
//         plotOptions: {
//           bar: {
//             horizontal: false,
//             columnWidth: '1%', // Adjust the width of the bars
//             borderRadius: 0, // Add some border radius for a more modern look
//           },
//         },
//         dataLabels: {
//           enabled: false, // Disable data labels for a cleaner look
//         },
//         stroke: {
//           show: true,
//           width: 2,
//           colors: ['transparent'],
//         },
//         xaxis: {
//           categories: categories,
//           labels: {
//             style: {
//               fontWeight: 'bold',
//               fontSize: '14px',
//               //colors: ['#FF5733', '#33FF57', '#5733FF', '#FFFF33'], // Set different colors for each label
//             },
//           },
//         },
//         yaxis: {
//           title: {
//             text: 'Weeks',
//             style: {
//               fontWeight: 'bold',
//               fontSize: '14px',
//               color: '#0F0F0F', // Set a different color for the y-axis title
//             },
//           },
//         },
//         series: [
//           {
//             name: 'Quotation',
//             data: data,
//             color: '#096e36', //#1a70d9
//           },
//         ],
//       };


//     })
//   }
//   GetFollowUpleadchartcount() {
// debugger
//     var url = 'TelecallerManager/GetFollowUpleadchartcount'
//     this.service.get(url).subscribe((result: any) => {
//       this.response_data = result;
//       debugger
//       this.enquirychartcountlist = this.response_data.telechartscount_list;
//       const data = this.enquirychartcountlist.map((entry: { enquirymonthcount: any; }) => entry.enquirymonthcount);
//       const categories = this.enquirychartcountlist.map((entry: { enquirymonth: any; }) => entry.enquirymonth);

//       // Initialize chart options
//       this.enquirychartcount = {
//         chart: {
//           type: 'bar',
//           height: 360, // Adjust the height of the chart as needed
//           // width: 600,
//           background: 'White',
//           foreColor: '#0F0F0F',
//           toolbar: {
//             show: false, // Set to false to hide the toolbar/menu icon
//           },
//         },
//         colors: ['#8062D6', '#FFD54F', '#66BB6A', '#EF5350', '#0F0F0F'], // Use a set of colors for better combinations
//         plotOptions: {
//           bar: {
//             horizontal: false,
//             columnWidth: '1%', // Adjust the width of the bars
//             borderRadius: 0, // Add some border radius for a more modern look
//           },
//         },
//         dataLabels: {
//           enabled: false, // Disable data labels for a cleaner look
//         },
//         stroke: {
//           show: true,
//           width: 2,
//           colors: ['transparent'],
//         },
//         xaxis: {
//           categories: categories,
//           labels: {
//             style: {
//               fontWeight: 'bold',
//               fontSize: '14px',
//               //colors: ['#FF5733', '#33FF57', '#5733FF', '#FFFF33'], // Set different colors for each label
//             },
//           },
//         },
//         yaxis: {
//           title: {
//             text: 'Weeks',
//             style: {
//               fontWeight: 'bold',
//               fontSize: '14px',
//               color: '#0F0F0F', // Set a different color for the y-axis title
//             },
//           },
//         },
//         series: [
//           {
//             name: 'Enquire',
//             data: data,
//             color: '#5d1d82', //#1a70d9
//           },
//         ],
//       };


//     })
//   }
//   GetProspectleadchartcount() {

//     var url = 'TelecallerManager/GetProspectleadchartcount'
//     this.service.get(url).subscribe((result: any) => {
//       this.response_data = result;
//       this.saleschartcountlist = this.response_data.telechartscount_list;
//       const data = this.saleschartcountlist.map((entry: { salesmonthcount: any; }) => entry.salesmonthcount);
//       const categories = this.saleschartcountlist.map((entry: { salesmonth: any; }) => entry.salesmonth);

//       // Initialize chart options
//       this.saleschartcount = {
//         chart: {
//           type: 'bar',
//           height: 360, // Adjust the height of the chart as needed
//           // width: 600,
//           background: 'White',
//           foreColor: '#0F0F0F',
//           toolbar: {
//             show: false, // Set to false to hide the toolbar/menu icon
//           },
//         },
//         colors: ['#8062D6', '#FFD54F', '#66BB6A', '#EF5350', '#0F0F0F'], // Use a set of colors for better combinations
//         plotOptions: {
//           bar: {
//             horizontal: false,
//             columnWidth: '1%', // Adjust the width of the bars
//             borderRadius: 0, // Add some border radius for a more modern look
//           },
//         },
//         dataLabels: {
//           enabled: false, // Disable data labels for a cleaner look
//         },
//         stroke: {
//           show: true,
//           width: 2,
//           colors: ['transparent'],
//         },
//         xaxis: {
//           categories: categories,
//           labels: {
//             style: {
//               fontWeight: 'bold',
//               fontSize: '14px',
//               //colors: ['#FF5733', '#33FF57', '#5733FF', '#FFFF33'], // Set different colors for each label
//             },
//           },
//         },
//         yaxis: {
//           title: {
//             text: 'Weeks',
//             style: {
//               fontWeight: 'bold',
//               fontSize: '14px',
//               color: '#0F0F0F', // Set a different color for the y-axis title
//             },
//           },
//         },
//         series: [
//           {
//             name: 'Sales',
//             data: data,
//             color: '#91346c', //#1a70d9
//           },
//         ],
//       };


//     })
//   }
  Getteamactivitysummary() {
    var url = 'TelecallerManager/teamactivitysummary'
    this.service.get(url).subscribe((result: any) => {
      $('#chartscountsummary_list').DataTable().destroy();
      this.responsedata = result;
      this.chartscountsummary_list = this.responsedata.chartscount_list;
      //let lblamountseperator1 =(parseInt( this.chartscountsummary_list[].quoteorder_amount.replace(/[^\d]+/gi, '')) || 0).toLocaleString('en-IN')
      //console.log(lblamountseperator1)
      setTimeout(() => {
        $('#chartscountsummary_list').DataTable();
      }, 1);
    });
    
  }
  changeSummary(data:string):void {
    this.page = data;
  }

  GetTelecallerManagerSummary() {
    var url = 'TelecallerManager/GetTelecallerManagerSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#countlist').DataTable().destroy();
      this.responsedata = result;
      this.countlist = this.responsedata.telecallermanager_lists;
      //console.log(this.entity_list)
      setTimeout(() => {
        $('#countlist').DataTable();
      }, 1);
    });
  }

  GetTeamPerformencechart() {
    this.NgxSpinnerService.show();
    var url = 'TelecallerManager/GetTeamPerformencechart'
    this.service.get(url).subscribe((result: any) => {

      this.response_data = result;

      this.Performencechart_list = this.response_data.Performencechart_list;
      // this.monthlySalesData = result || [];

      // Transform data for chart
      const categories = this.Performencechart_list.map((entry: { call_response: any; }) => entry.call_response);
      const filteredDatacity = categories.slice(0, 20);
      const data = this.Performencechart_list.map((entry: { callcount: any; }) => entry.callcount);
      const filteredData = data.slice(0, 20);
      // console.log(categories)
      // console.log(data)
      // Initialize chart options
      this.WebsiteUserchart = {
        chart: {
          type: 'bar',
          height: 300, // Adjust the height of the chart as needed
          width: '100%',
          background: 'White',
          foreColor: '#0F0F0F',
          toolbar: {
            show: false, // Set to false to hide the toolbar/menu icon
          },
        },
        colors: ['#8062D6', '#FFD54F', '#66BB6A', '#50efcf'], // Use a set of colors for better combinations
        plotOptions: {
          bar: {
            horizontal: true,
            columnWidth: '5%', // Adjust the width of the bars
            borderRadius: 0, // Add some border radius for a more modern look
          },
        },
        dataLabels: {
          enabled: false, // Disable data labels for a cleaner look
        },
        stroke: {
          show: true,
          width: 2,
          colors: ['transparent'],
        },
        xaxis: {
          categories: filteredDatacity,
          labels: {
            style: {
              fontWeight: 'bold',
              fontSize: '14px',
              //colors: ['#FF5733', '#33FF57', '#5733FF', '#FFFF33'], // Set different colors for each label
            },
          },
        },
        yaxis: {
          title: {
            text: 'Weeks',
            style: {
              fontWeight: 'bold',
              fontSize: '14px',
              color: '#0F0F0F', // Set a different color for the y-axis title
            },
          },
        },
        series: [
          {
            name: 'Users',
            data: filteredData,
            color: '#74d9f7',
            
          },
        ],
      };
    })
    this.NgxSpinnerService.hide();
  }
  getColor(buttonNumber: number): any {
    if ((buttonNumber === 2) && buttonNumber === this.number) {
      return {'background': 'linear-gradient(to left, #ffffff, #ffdfdf)'};
    } else if ((buttonNumber === 1) && buttonNumber === this.number) {
      return {'background': 'linear-gradient(to left, #ffffff, #bbebff)'};;
    } else if ((buttonNumber === 3) && buttonNumber === this.number) {
      return {'background': 'linear-gradient(to left, #ffffff, #d1fbd1)'};;
    }else if ((buttonNumber === 4) && buttonNumber === this.number) {
      return {'background': 'linear-gradient(to left, #ffffff, #f7c2db)'};;
    }else if ((buttonNumber === 5) && buttonNumber === this.number) {
      return {'background': 'linear-gradient(to left, #ffffff, #f6ccf6)'};;
    }else if ((buttonNumber === 6) && buttonNumber === this.number) {
      return {'background': 'linear-gradient(to left, #ffffff, #9ee6dc)'};;
    }else if ((buttonNumber === 7) && buttonNumber === this.number) {
      return {'background': 'linear-gradient(to left, #ffa5a5, #ffa5a5)'};;
    // }else {
      return 'gray';
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

