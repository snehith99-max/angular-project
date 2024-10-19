import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { Router } from '@angular/router';
import { AES } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import { SelectionModel } from '@angular/cdk/collections';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { NgbTimepickerModule, NgbTimeStruct } from '@ng-bootstrap/ng-bootstrap';
import { param } from 'jquery';
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
  selector: 'app-crm-trn-upcomingmarketing',
  templateUrl: './crm-trn-upcomingmarketing.component.html',
  styleUrls: ['./crm-trn-upcomingmarketing.component.scss']
})
export class CrmTrnUpcomingmarketingComponent {
  teamdetails: any[] = [];
  upcoming: any;
  countlist: any[] = [];
  schedulesummary_list1: any[] = [];
  responsedata: any;
  leadbank_list: any;


  schedule_remarks: string | undefined;
  leadbank_name: string | undefined;

  totallist: any[] = [];
  isCollapsed = false;
  public isOpen = true;
  assigned_leads: any;
  Lapsed_Leads: any;
  Longest_Leads: any;
  internal_notes: any;
  reactiveFormfollow!: FormGroup;
  //reactiveFormSchedule!: FormGroup;
  reactiveFormTransfer!: FormGroup;
  reactiveFormdrop!: FormGroup;

  selectedItems: string[] = [];
  assignlist: string[] = [];
  ScheduleType = [

    { type: 'Meeting', },

  ];

  transfer: ITransfer = {
    team_name: '',
    team_member: '',
    schedule_date: '',
    schedule_time: '',
    schedule_type: '',
    schedule_remarks: ''
  };
  CurObj: ICampaign = new ICampaign();
  selection = new SelectionModel<ICampaign>(true, []);
  pick: Array<any> = [];
  assign_user: any;
  parameterValue1: any;
  employee_list: any;
  teamname_list: any;
  totaltilecountdetails: any[] = [];
  totallapsedlongest: any[] = [];
  ///
  chartscountlist: any[] = [];
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
  parameter1: any;
  Months: any;
  new_leads: any;
  potentialscount: any;
  prospectcount: any;
  orderscount: any;
  showOptionsDivId: any;
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService, private route: Router,
    private NgxSpinnerService: NgxSpinnerService) {

  }
  ngOnInit(): void {
    this.GetUpcomingManagerSummary();
    this.GetMarketingManagerSummary();
    this.Getteamcount();
    this.GetTotaltilecount();
    this.GetTotallapsedlongest();
    this.Getleadchartcount();
    this.Getteamactivitysummary();
    this.reactiveFormTransfer = new FormGroup({
      team_name: new FormControl(this.transfer.team_name, [
        Validators.required,
      ]),
      team_member: new FormControl(this.transfer.team_member, [
        Validators.required,
      ]),
      leadbank_gid: new FormControl(''),
      appointment_gid: new FormControl(''),
      assignedto_gid: new FormControl(''),
    });

    this.reactiveFormfollow = new FormGroup({
      schedule_date: new FormControl(this.transfer.schedule_date, [
        Validators.required,
      ]),
      schedule_time: new FormControl(this.transfer.schedule_time, [
        Validators.required,
      ]),
      schedule_type: new FormControl(null, Validators.required,),
      schedule_remarks: new FormControl(''),
      ScheduleRemarks1: new FormControl(''),
      leadbank_gid: new FormControl(''),
      lead2campaign_gid: new FormControl(''),
      assignedto_gid: new FormControl(''),
     
    });
    this.reactiveFormdrop = new FormGroup({
      drop_remarks: new FormControl('',[Validators.required,]),
    });

    var api1 = 'MarketingManager/GetTeamNamedropdown'
    this.service.get(api1).subscribe((result: any) => {
      this.teamname_list = result.GetTeamNamedropdown;
      //console.log(this.branch_list)
    });

    const options: Options = {
      // enableTime: true,
      dateFormat: 'Y-m-d',

    };
    flatpickr('.date-picker', options);

    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
  }
  teamname() {
    let team_gid = this.reactiveFormTransfer.get("team_name")?.value;
    let param = {
      team_gid: team_gid
    }
    var url = 'MarketingManager/GetTeamEmployeedropdown'
    this.service.getparams(url, param).subscribe((result: any) => {
      this.employee_list = result.GetTeamEmployeedropdown;
    });
  }
  //teamcount//
  Getteamcount() {
    var url = 'MarketingManager/Getteamcount'
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

  //Get Total tile count
  GetTotaltilecount() {
    var url = 'MarketingManager/GetTotaltilecount'
    this.service.get(url).subscribe((result: any) => {
      $('#totaltilecountdetails').DataTable().destroy();
      this.responsedata = result;
      this.totaltilecountdetails = this.responsedata.totaltilecount_lists;
      //console.log(this.entity_list)
      setTimeout(() => {
        $('#totaltilecountdetails').DataTable();
      }, 1);
    });
  }
  Onteamview(campaign_gid:any){
    const secretKey = 'storyboarderp';
    const param = (campaign_gid);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.route.navigate(['/crm/CrmTrnTteamleadsview', encryptedParam])
  }
  //Get Total lapsed longest count
  GetTotallapsedlongest() {
    var url = 'MarketingManager/Totallapsedlongest'
    this.service.get(url).subscribe((result: any) => {
      $('#totaltilecountdetails').DataTable().destroy();
      this.responsedata = result;
      this.totallapsedlongest = this.responsedata.totallapsedlongest;
      //console.log(this.entity_list)
      setTimeout(() => {
        $('#totaltilecountdetails').DataTable();
      }, 1);
    });
  }

  //// Summary Grid//////
  GetUpcomingManagerSummary() {
    this.NgxSpinnerService.show();
    var url = 'Marketingmanager/GetUpcomingManagerSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#upcoming').DataTable().destroy();
      this.responsedata = result;
      this.upcoming = this.responsedata.upcoming;
      this.NgxSpinnerService.hide();
      //console.log(this.source_list)
      setTimeout(() => {
        $('#upcoming').DataTable();
      }, 1);
    });
  }
  popmodal(parameter: string, parameter1: string) {

    this.schedule_remarks = parameter;
    this.leadbank_name = parameter1;
  }
  //360//
  Onopen(param1: any, param2: any) {
    const secretKey = 'storyboarderp';
    const lspage1 = "MM-Upcoming";
    const lspage = AES.encrypt(lspage1, secretKey).toString();
    console.log(param1);
    console.log(param2);
    const leadbank_gid = AES.encrypt(param1, secretKey).toString();
    const appointment_gid = AES.encrypt(param2, secretKey).toString();
    this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,appointment_gid, lspage]);
  }
  //Tiles count//
  GetMarketingManagerSummary() {
    var url = 'MarketingManager/GetMarketingManagerSummary'
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
  //Schedule & Transfer//
  get team_name() {
    return this.reactiveFormTransfer.get('team_name')!;
  }
  get team_member() {
    return this.reactiveFormTransfer.get('team_member')!;
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
    this.parameterValue1 = parameter
    this.reactiveFormTransfer.get("leadbank_gid")?.setValue(this.parameterValue1.leadbank_gid);
    this.reactiveFormTransfer.get("appointment_gid")?.setValue(this.parameterValue1.appointment_gid);
    this.reactiveFormTransfer.get("assignedto_gid")?.setValue(this.parameterValue1.assignedto_gid);
    this.reactiveFormTransfer.get("leadbank_name")?.setValue(this.parameterValue1.leadbank_name);
    this.leadbank_name = this.parameterValue1.leadbank_name;
  }


  //Schedule submit//
  onsubmitschedule() {
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
        window.location.reload()
        this.reactiveFormfollow.reset();
      });

    }

    else {
      window.location.reload()
      console.log("Form is not valid");
      return;
    }

  }

  //Schedule summary 
  Getshedulesummary(leadbank_gid: any) {
    var url = 'MarketingManager/GetSchedulelogsummary'
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

  // Transfer submit
  OnTransfer() {
    this.NgxSpinnerService.show();

    console.log(this.reactiveFormTransfer.value);
    const url1 = 'MarketingManager/PostMoveToTransfer';

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

      // Delay the reload by 2 seconds (adjust the time as needed)
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
    var url1 = 'MarketingManager/GetCampaignMoveToBin'
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
  //Ends here
  oncloseschedule() {
    this.reactiveFormfollow.reset();
  }
  onclosetransfer() {
    this.reactiveFormTransfer.reset();
  }

  Getleadchartcount() {
    var url = 'MarketingManager/leadchartcount'
    this.service.get(url).subscribe((result: any) => {
      this.response_data = result;
      this.chartscountlist = this.response_data.chartscount_list;
      this.Months = this.chartscountlist.map((entry: { Months: any }) => entry.Months),
      this.new_leads = this.chartscountlist.map((entry: { new_leads: any }) => entry.new_leads)
      this.potentialscount = this.chartscountlist.map((entry: { potentialscount: any }) => entry.potentialscount)
      this.prospectcount = this.chartscountlist.map((entry: { prospectcount: any }) => entry.prospectcount),
      this.orderscount = this.chartscountlist.map((entry: { orderscount: any }) => entry.orderscount)
      // Initialize chart options
      this.leadchartcount = {
        chart: {
          type: 'bar',
          height: 360, // Adjust the height of the chart as needed
          // width: 600,
          background: 'White',
          foreColor: '#0F0F0F',
          fontFamily: 'inherit',
          toolbar: {
            show: false, // Set to false to hide the toolbar/menu icon
          },
        },
        colors: ['#8062D6', '#FFD54F', '#66BB6A', '#EF5350', '#0F0F0F'], // Use a set of colors for better combinations
        plotOptions: {
          bar: {
            horizontal: false,
            columnWidth: '50%', // Adjust the width of the bars
            borderRadius: 0, // Add some border radius for a more modern look
          },
        },
        dataLabels: {
          enabled: false, // Disable data labels for a cleaner look
        },
        xaxis: {
          categories: this.Months,
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
            style: {
              fontWeight: 'bold',
              fontSize: '14px',
              color: '#0F0F0F', // Set a different color for the y-axis title
            },
          },
        },
        series: [
          {
            name: 'Appointments',
            data: this.new_leads,
            color: '#CE7F92', //#1a70d9
          },
          {
            name: 'Enquiries',
            data: this.prospectcount,
            color: '#747C8C', //#1a70d9
          },
          {
            name: 'Quotations',
            data: this.potentialscount,
            color: '#667967', //#1a70d9
          },
          {
            name: 'Sales',
            data: this.orderscount,
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
  // Getquotationchartcount() {

  //   var url = 'MarketingManager/quotationchartcount'
  //   this.service.get(url).subscribe((result: any) => {
  //     this.response_data = result;
  //     this.quotationcountlist = this.response_data.chartscount_list;
  //     const data = this.quotationcountlist.map((entry: { potentialscount: any; }) => entry.potentialscount);
  //     const categories = this.quotationcountlist.map((entry: { potentialsmonth: any; }) => entry.potentialsmonth);

  //     // Initialize chart options
  //     this.quotationchartcount = {
  //       chart: {
  //         type: 'bar',
  //         height: 360, // Adjust the height of the chart as needed
  //         // width: 600,
  //         background: 'White',
  //         foreColor: '#0F0F0F',
  //         toolbar: {
  //           show: false, // Set to false to hide the toolbar/menu icon
  //         },
  //       },
  //       colors: ['#8062D6', '#FFD54F', '#66BB6A', '#EF5350', '#0F0F0F'], // Use a set of colors for better combinations
  //       plotOptions: {
  //         bar: {
  //           horizontal: false,
  //           columnWidth: '1%', // Adjust the width of the bars
  //           borderRadius: 0, // Add some border radius for a more modern look
  //         },
  //       },
  //       dataLabels: {
  //         enabled: false, // Disable data labels for a cleaner look
  //       },
  //       stroke: {
  //         show: true,
  //         width: 2,
  //         colors: ['transparent'],
  //       },
  //       xaxis: {
  //         categories: categories,
  //         labels: {
  //           style: {
  //             fontWeight: 'bold',
  //             fontSize: '14px',
  //             //colors: ['#FF5733', '#33FF57', '#5733FF', '#FFFF33'], // Set different colors for each label
  //           },
  //         },
  //       },
  //       yaxis: {
  //         title: {
  //           text: 'Weeks',
  //           style: {
  //             fontWeight: 'bold',
  //             fontSize: '14px',
  //             color: '#0F0F0F', // Set a different color for the y-axis title
  //           },
  //         },
  //       },
  //       series: [
  //         {
  //           name: 'Quotation',
  //           data: data,
  //           color: '#096e36', //#1a70d9
  //         },
  //       ],
  //     };


  //   })
  // }
  // Getenquirychartcount() {
  //   debugger
  //   var url = 'MarketingManager/enquirychartcount'
  //   this.service.get(url).subscribe((result: any) => {
  //     this.response_data = result;
  //     this.enquirychartcountlist = this.response_data.chartscount_list;
  //     const data = this.enquirychartcountlist.map((entry: { prospectcount: any; }) => entry.prospectcount);
  //     const categories = this.enquirychartcountlist.map((entry: { prospectmonth: any; }) => entry.prospectmonth);

  //     // Initialize chart options
  //     this.enquirychartcount = {
  //       chart: {
  //         type: 'bar',
  //         height: 360, // Adjust the height of the chart as needed
  //         // width: 600,
  //         background: 'White',
  //         foreColor: '#0F0F0F',
  //         toolbar: {
  //           show: false, // Set to false to hide the toolbar/menu icon
  //         },
  //       },
  //       colors: ['#8062D6', '#FFD54F', '#66BB6A', '#EF5350', '#0F0F0F'], // Use a set of colors for better combinations
  //       plotOptions: {
  //         bar: {
  //           horizontal: false,
  //           columnWidth: '1%', // Adjust the width of the bars
  //           borderRadius: 0, // Add some border radius for a more modern look
  //         },
  //       },
  //       dataLabels: {
  //         enabled: false, // Disable data labels for a cleaner look
  //       },
  //       stroke: {
  //         show: true,
  //         width: 2,
  //         colors: ['transparent'],
  //       },
  //       xaxis: {
  //         categories: categories,
  //         labels: {
  //           style: {
  //             fontWeight: 'bold',
  //             fontSize: '14px',
  //             //colors: ['#FF5733', '#33FF57', '#5733FF', '#FFFF33'], // Set different colors for each label
  //           },
  //         },
  //       },
  //       yaxis: {
  //         title: {
  //           text: 'Weeks',
  //           style: {
  //             fontWeight: 'bold',
  //             fontSize: '14px',
  //             color: '#0F0F0F', // Set a different color for the y-axis title
  //           },
  //         },
  //       },
  //       series: [
  //         {
  //           name: 'Enquire',
  //           data: data,
  //           color: '#5d1d82', //#1a70d9
  //         },
  //       ],
  //     };


  //   })
  // }
  // Getsaleschartcountt() {

  //   var url = 'MarketingManager/saleschartcount'
  //   this.service.get(url).subscribe((result: any) => {
  //     this.response_data = result;
  //     this.saleschartcountlist = this.response_data.chartscount_list;
  //     const data = this.saleschartcountlist.map((entry: { orderscount: any; }) => entry.orderscount);
  //     const categories = this.saleschartcountlist.map((entry: { ordersmonth: any; }) => entry.ordersmonth);

  //     // Initialize chart options
  //     this.saleschartcount = {
  //       chart: {
  //         type: 'bar',
  //         height: 360, // Adjust the height of the chart as needed
  //         // width: 600,
  //         background: 'White',
  //         foreColor: '#0F0F0F',
  //         toolbar: {
  //           show: false, // Set to false to hide the toolbar/menu icon
  //         },
  //       },
  //       colors: ['#8062D6', '#FFD54F', '#66BB6A', '#EF5350', '#0F0F0F'], // Use a set of colors for better combinations
  //       plotOptions: {
  //         bar: {
  //           horizontal: false,
  //           columnWidth: '1%', // Adjust the width of the bars
  //           borderRadius: 0, // Add some border radius for a more modern look
  //         },
  //       },
  //       dataLabels: {
  //         enabled: false, // Disable data labels for a cleaner look
  //       },
  //       stroke: {
  //         show: true,
  //         width: 2,
  //         colors: ['transparent'],
  //       },
  //       xaxis: {
  //         categories: categories,
  //         labels: {
  //           style: {
  //             fontWeight: 'bold',
  //             fontSize: '14px',
  //             //colors: ['#FF5733', '#33FF57', '#5733FF', '#FFFF33'], // Set different colors for each label
  //           },
  //         },
  //       },
  //       yaxis: {
  //         title: {
  //           text: 'Weeks',
  //           style: {
  //             fontWeight: 'bold',
  //             fontSize: '14px',
  //             color: '#0F0F0F', // Set a different color for the y-axis title
  //           },
  //         },
  //       },
  //       series: [
  //         {
  //           name: 'Sales',
  //           data: data,
  //           color: '#91346c', //#1a70d9
  //         },
  //       ],
  //     };


  //   })
  // }
  Getteamactivitysummary() {
    var url = 'MarketingManager/teamactivitysummary'
    this.service.get(url).subscribe((result: any) => {
      $('#chartscountsummary_list').DataTable().destroy();
      this.responsedata = result;
      this.chartscountsummary_list = this.responsedata.chartscount_list;
      //let lblamountseperator1 =(parseInt( this.chartscountsummary_list[].quoteorder_amount.replace(/[^\d]+/gi, '')) || 0).toLocaleString('en-IN')
      //console.log(lblamountseperator1)
      // setTimeout(() => {
      //   $('#chartscountsummary_list').DataTable();
      // }, 1);
    });

  }
  // updateChart(chartType: string) {
  //   this.selectedChartType = chartType;
  //   switch (chartType) {
  //     case 'lead':
  //       this.Getleadchartcount();
  //       break;
  //     case 'enquiry':
  //       this.Getenquirychartcount();
  //       break;
  //     case 'quote':
  //       this.Getquotationchartcount();
  //       break;
  //     case 'salesorder':
  //       this.Getsaleschartcountt();
  //       break;
  //     default:
  //       this.Getleadchartcount();
  //       break;
  //   }
  // }

  toggleOptions(appointment_gid: any) {
    if (this.showOptionsDivId === appointment_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = appointment_gid;
    }
  }
}
