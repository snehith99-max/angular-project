import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { Router } from '@angular/router';
import { AES } from 'crypto-js';
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
  selector: 'app-crm-trn-telecampaignmanagersummary-scheduled',
  templateUrl: './crm-trn-telecampaignmanagersummary-scheduled.component.html',
  styleUrls: ['./crm-trn-telecampaignmanagersummary-scheduled.component.scss']
})
export class CrmTrnTelecampaignmanagersummaryScheduledComponent {
  responsedata: any;
  totallist: any;
  internal_notes: any;
  leadbank_name: any;
  mail_id: any;
  mailaddress: any;
  parameterValue1: any;
  reactiveFormfollow!: FormGroup;
  reactiveFormTransfer!: FormGroup;
  reactiveFormdrop!: FormGroup;
  schedulesummary_list1: any;
  employee_list: any;
  teamname_list: any;
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
  leadchartcountlist: any[] = [];
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
  page: any='Total Lead';
  countlist: any[] = [];
   totaltilecountdetails: any[] = [];
   parameter1: any;
   chartcountlist: any[] = [];
   months: any;
   new_leads: any;
   pending_calls: any;
   follow_up: any;
   prospect: any;
   showOptionsDivId: any;
   chartflag:boolean=false;
   chartflag1:boolean=false;
   
  GetCallLogLead_list:any;
  leadbank_gid:any;
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService,
    public service: SocketService, private route: Router, private NgxSpinnerService: NgxSpinnerService) {

  }

  ngOnInit(): void {
    this.GetTelecallerManagerTotalSummary();
    this.GetTotaltilecount();
    this.GetNewleadchartcount();
    this.GetTeamPerformencechart();
    this.GetTelecallerManagerSummary();
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
    this.reactiveFormdrop = new FormGroup({
      drop_remarks: new FormControl('',[Validators.required,]),
    });
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


  GetTelecallerManagerTotalSummary() {
    this.NgxSpinnerService.show();
    var url = 'TelecallerManager/GetTelecallerManagerScheduledSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#totallist').DataTable().destroy();
      this.responsedata = result;
      this.totallist = this.responsedata.telecallermanager_totallists;
      this.NgxSpinnerService.hide();
      //console.log(this.entity_list)
      setTimeout(() => {
        $('#totallist').DataTable();
      }, 1);
    });
  }
  popmodal(parameter: string, parameter1: string) {
    this.internal_notes = parameter; // Access parameter directly
    this.leadbank_name = parameter1;
  }

  Onopen(param1: any, param2: any, param3: any,param4:any) {
    this.mail_id = param3.split('/');
    this.mailaddress = this.mail_id[2]
    localStorage.setItem('mailaddress', this.mailaddress)
    const secretKey = 'storyboarderp';
    const lspage1 = "MM-teleScheduled";
    const lspage = AES.encrypt(lspage1, secretKey).toString();
    const leadbank_gid = AES.encrypt(param1, secretKey).toString();
    const lead2campaign_gid = AES.encrypt(param2, secretKey).toString();
    const appointment_gid = AES.encrypt(param4, secretKey).toString();
    this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,appointment_gid, lspage]);
  }

  openModallog3(parameter: string) {
    this.parameterValue1 = parameter
    this.reactiveFormfollow.get("leadbank_gid")?.setValue(this.parameterValue1.leadbank_gid);
    this.reactiveFormfollow.get("lead2campaign_gid")?.setValue(this.parameterValue1.lead2campaign_gid);
    this.reactiveFormfollow.get("assignedto_gid")?.setValue(this.parameterValue1.assignedto_gid);
    this.reactiveFormfollow.get("leadbank_name")?.setValue(this.parameterValue1.leadbank_name);
    this.leadbank_name = this.parameterValue1.leadbank_name;
    this.Getshedulesummary(this.parameterValue1.leadbank_gid);

  }

  openModallog4(parameter: string) {
    this.parameterValue1 = parameter
    this.reactiveFormTransfer.get("leadbank_gid")?.setValue(this.parameterValue1.leadbank_gid);
    this.reactiveFormTransfer.get("lead2campaign_gid")?.setValue(this.parameterValue1.lead2campaign_gid);
    this.reactiveFormTransfer.get("assignedto_gid")?.setValue(this.parameterValue1.assignedto_gid);
    this.reactiveFormTransfer.get("leadbank_name")?.setValue(this.parameterValue1.leadbank_name);
    this.leadbank_name = this.parameterValue1.leadbank_name;
  }

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
  onclosedrop() {
    this.reactiveFormdrop.reset();
  }

  oncloseschedule() {
    this.reactiveFormfollow.reset();
  }
  onclosetransfer() {
    this.reactiveFormTransfer.reset();
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

  //Schedule submit//
  onsubmitschedule() {
    debugger
    this.NgxSpinnerService.show();
    console.log(this.reactiveFormfollow.value);
    if (this.reactiveFormfollow) {
      var url1 = 'MyCalls/PostFollowschedulelog'
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
  Onteamview(campaign_gid:any){
    const secretKey = 'storyboarderp';
    const param = (campaign_gid);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.route.navigate(['/crm/CrmTrnTeleteamview', encryptedParam])
  }
  //T
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

      if (this.chartcountlist.length > 0) {
        this.chartflag = true;
      } 
      // Initialize chart options
      this.leadchartcount = {
        chart: {
          type: 'bar',
          height: 360, // Adjust the height of the chart as needed
          // width: 600,
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
      
	   if (this.Performencechart_list.length > 0) {
      this.chartflag1 = true;
    } 
      this.WebsiteUserchart = {
        chart: {
          type: 'bar',
          height: 300, // Adjust the height of the chart as needed
          //  width: 600,
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
  changeSummary(data:string):void {
    this.page = data;
  }
  toggleOptions(leadbank_gid: any) {
    if (this.showOptionsDivId === leadbank_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = leadbank_gid;
    }
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
}
