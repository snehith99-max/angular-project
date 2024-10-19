import { Component, OnInit, ViewChild } from "@angular/core";
import { ChartComponent } from "ng-apexcharts";
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { Subscription, Observable, timer } from 'rxjs';
import { first } from 'rxjs/operators';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { DatePipe } from '@angular/common';
import { map, share } from "rxjs/operators";
import { Pipe } from "@angular/core";
import {
  ApexNonAxisChartSeries,
  ApexResponsive,
  ApexChart
} from "ng-apexcharts";
import { SharedService } from "src/app/layout/services/shared.service";
export type ChartOptions1 = {
  series: ApexNonAxisChartSeries;
  chart: ApexChart;
  responsive: ApexResponsive[];
  labels: any;
};

export type ChartOptions3 = {
  series: ApexNonAxisChartSeries;
  chart: ApexChart;
  responsive: ApexResponsive[];
  labels: any;
};

@Component({
  selector: 'app-crm-dashboard',
  templateUrl: './crm-dashboard.component.html',
  styleUrls: ['./crm-dashboard.component.scss']
})

export class CrmDashboardComponent implements OnInit {
  //chartOptions: any = {};

  chartOptions1: any = {};
  chartOptions2: any = {};
  chartOptions3: any = {};
  leadchart: any = {};
  socialmedialeadcount: any = {};
  response_data: any;

  DashboardCount_List: any;
  LeadBankCountList: any;
  DashboardQuotationAmt_List: any;
  getleadbasedonemployeeList: any;
  noquotation: any;

  year: any;

  noquotation_status: any;
  mycalls_count: any;

  show = true;

  menu!: any[];
  submenu!: any[];
  selectedIndex: number = 0;
  menu_name!: any;
  menu_index!: number;
  noleadstatus: any;
  nomonthlyleadstatus: any;

  shopifyproductcount: any;
  shopifycustomercount: any;
  shopifyordercount: any;
  product_count: any;
  order_count: any;
  shopifystorename: any;
  store_name: any;
  contactcount_list: any;
  contact_count: any;
  messagecount_list: any;
  message_count: any;
  messageincoming_list: any;
  incoming_count: any;
  messageoutgoing_list: any;
  sent_count: any;
  emailstatus_list: any;
  deliverytotal_count: any;
  opentotal_count: any;
  clicktotal_count: any;
  sentmailcount_list: any;
  mail_sent: any;
  customertotalcount_list: any;
  customer_assigncount: any;
  customerassignedcount_list: any;
  unassign_count: any;
  customerunassignedcount_list: any;
  time = new Date();
  rxTime = new Date();
  intervalId: any;
  subscription!: Subscription;
  currentDayName: string;
  fromDate: any; toDate: any;
  // emptyFlag: boolean=false;
  emptyFlag1: boolean = false;
  series_Value: any;
  labels_value: any;
  series2: any;
  data: any;


  nodatastatus: any;
  emptyFlag3: boolean = false;
  shopifyproduct_counts: any;
  shopifyproducts_list: any[] = [];
  QuotationAmountchart: any = {};
  quotation_month: any;
  total_amount: any;
  responsedata: any;
  MyLeadsCount_List: any[] = [];
  corporate_count: any;
  retailer_counts: any;
  distributor_count: any;
  //////////////////
  crmtilescount_list: any[] = [];
  total_count: any;
  customer_count: any;
  ytd_year: any;
  ytd_count: any;
  mtd_month: any;
  mtd_count: any;
  crmleadchart: any = {};
  totalperformance_list: any[] = [];
  campaignsentchart_list: any[] = [];
  lead_month: any;
  lead_count: any;
  crmregionchart: any = {};
  region_name: any;
  region_count: any;
  source_count: any;
  source_name: any;
  customer_count1: any;
  customer_month: any;
  customer_type: any;
  customertype_count: any;
  months: any;
  mailsent_count: any;
  whatsappsent_count: any;
  crmsourcechart: any = {};
  crmcustomerchart: any = {};
  crmcustomertypechart: any = {};
  crmcampaignsentchart: any = {};
  selectedChartType: any;
  //////////overall/////////
  overall_list: any[] = [];
  leadstagechart: any = {};
  prospects: any;
  months_detail: any;
  potential: any;
  minsoft:any;
  drop_leads: any;
  customer_month1: any;
  customer: any;
  corporate_label: any;
  distributor_label: any;
  retailer_label: any;
  flag: boolean = false;
  campaignflag1: boolean = false;
  overallchartflag: boolean = false;
  leadbankflag: boolean = false;
  regionchart_list: any = {};
  sourcechart_list: any = {};
  chartOptiokk: any = {};
  regionflag: boolean = false;
  sourceflag: boolean = false;
  appointment_leadcount:any;
  appointment_month:any;


  constructor(private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private service: SocketService,
    private ToastrService: ToastrService,
    public sharedservice: SharedService,
    private datePipe: DatePipe) {
    const today = new Date();
    this.currentDayName = today.toLocaleDateString('en-US', { weekday: 'long' });
  }

  change_menu_tab(n: number): void {
    this.submenu = this.menu[n].sub1menu;
    this.selectedIndex = n;
  }

  ngOnInit(): void {
    this.Getcrmtilescount();
    this.Getcrmleadchart();
    this.Getcrmregionchart();
    this.Getcrmsourcechart();
    this.Getprospectchart();
    var url = 'SmrMstProduct/MintsoftProductDetailsAsync'
    this.service.get(url).subscribe((result: any) => {
      this.minsoft = result;
    });

    var api7 = 'MyLead/GetMyLeadsCount';
    this.service.get(api7).subscribe((result: any) => {
      this.responsedata = result;
      this.MyLeadsCount_List = this.responsedata.getMyLeadsCount_List;
    });


    let user_gid = localStorage.getItem('user_gid');
    let param = {
      user_gid: user_gid
    }
    var url = 'User/Dashboardprivilegelevel';
    this.service.getparams(url, param).subscribe((result: any) => {
      for (let i = 0; i < result.menu_list.length; i++) {
        if (result.menu_list[i].text == 'Marketing') {
          this.menu_index = i;
        }
      }
      this.menu = result.menu_list[this.menu_index].submenu;
      //console.log(this.menu,'test menu');
      this.submenu = this.menu[0].sub1menu;
      //console.log(this.submenu,'test submenu');
      // this.change_menu_tab(this.menu_index);
    });


    // var api = 'CrmDashboard/GetappointmentCount'
    // this.service.get(api).subscribe((result: any) => {
    //   this.responsedata = result;
    //   this.LeadBankCountList = this.responsedata.appointmentcount_list;
    //   console.log('waldmklsa',  this.LeadBankCountList )
    //   if (this.LeadBankCountList.length > 0) {
    //     this.leadbankflag = true;
    //   } 
    //   this.appointment_month = this.LeadBankCountList.map((entry: { appointment_month: any }) => entry.appointment_month),
    // this.appointment_leadcount = this.LeadBankCountList.map((entry: { appointment_leadcount: any }) => entry.appointment_leadcount)
    // // Initialize chart options
    // this.leadchart = {
    //   chart: {
    //     type: 'bar',
    //     height: 300,
    //     width: '100%',
    //     background: 'White',
    //     foreColor: '#0F0F0F',
    //     fontFamily: 'inherit',
    //     toolbar: {
    //       show: false,
    //     },
    //   },
    //   colors: ['#7FC7D9', '#FFD54F', '#66BB6A', '#EF5350'],
    //   plotOptions: {
    //     bar: {
    //       horizontal: false,
    //       columnWidth: '50%',
    //       borderRadius: 0,
    //     },
    //   },
    //   dataLabels: {
    //     enabled: false,
    //   },
    //   xaxis: {
    //     categories: this.appointment_month,
    //     labels: {
    //       style: {

    //         fontSize: '12px',
    //       },
    //     },
    //   },
    //   yaxis: {
    //     title: {
    //       style: {
    //         fontWeight: 'bold',
    //         fontSize: '14px',
    //         color: '#7FC7D9',
    //       },
    //     },
    //   },
    //   series: [
    //     {
    //       name: 'Lead',
    //       data: this.appointment_leadcount,
    //       color: '#3D9DD9',
    //     },
    //   ],
    //   legend: {
    //     position: "bottom",
    //     offsetY: 5
    //   }
    // };
     
    // });
    this.Getcampaignsentchart();


  }
  //20.03.2024////
  Getcrmtilescount() {

    var url = 'CrmDashboard/Getcrmtilescount';
    this.service.get(url).subscribe((result: any) => {
      this.response_data = result;
      this.crmtilescount_list = this.response_data.crmtilescount_list;
      this.total_count = this.crmtilescount_list[0].total_count;
      this.mtd_count = this.crmtilescount_list[0].mtd_count
      this.mtd_month = this.crmtilescount_list[0].mtd_month
      this.ytd_count = this.crmtilescount_list[0].ytd_count
      this.ytd_year = this.crmtilescount_list[0].ytd_year
      this.customer_count = this.crmtilescount_list[0].customer_count

    });
  }
  Getcrmleadchart() {

    var url = 'CrmDashboard/Getcrmleadchart';
    this.service.get(url).subscribe((result: any) => {
      this.response_data = result;
      this.totalperformance_list = this.response_data.crmleadchart_list;
      if (this.totalperformance_list.length > 0) {
        this.flag = true;
      }
      this.months = this.totalperformance_list.map((entry: { months: any }) => entry.months),
        this.lead_count = this.totalperformance_list.map((entry: { lead_count: any }) => entry.lead_count)
      this.customer_count1 = this.totalperformance_list.map((entry: { customer_count: any }) => entry.customer_count)
      // Initialize chart options
      this.crmleadchart = {
        chart: {
          type: 'bar',
          height: 300,
          width: '100%',
          background: 'White',
          foreColor: '#0F0F0F',
          fontFamily: 'inherit',
          toolbar: {
            show: false,
          },
        },
        colors: ['#7FC7D9', '#FFD54F', '#66BB6A', '#EF5350'],
        plotOptions: {
          bar: {
            horizontal: false,
            columnWidth: '50%',
            borderRadius: 0,
          },
        },
        dataLabels: {
          enabled: false,
        },
        xaxis: {
          categories: this.months,
          labels: {
            style: {

              fontSize: '12px',
            },
          },
        },
        yaxis: {
          title: {
            style: {
              fontWeight: 'bold',
              fontSize: '14px',
              color: '#7FC7D9',
            },
          },
        },
        series: [
          {
            name: 'Lead',
            data: this.lead_count,
            color: '#3D9DD9',
          },
          {
            name: 'Purchase',
            data: this.customer_count1,
            color: '#F2D377',
          },

        ],
        // fill: {
        //   type: "gradient",
        //   gradient: {
        //     shadeIntensity: 1,
        //     opacityFrom: 0.7,
        //     opacityTo: 0.9,
        //     stops: [0, 100]
        //   }
        // }
        legend: {
          position: "bottom",
          offsetY: 5
        }
      };

      // this.NgxSpinnerService.hide(); // Move this inside the subscribe to ensure it executes after the data is processed

    });
  }
  Getcrmregionchart() {
    var url = 'CrmDashboard/Getcrmregionchart';
    this.service.get(url).subscribe((result: any) => {
      this.response_data = result;
      this.regionchart_list = this.response_data.crmregionchart_list;
      if (this.regionchart_list.length > 0) {
        this.regionflag = true;
      }
      this.region_count = this.regionchart_list.map((entry: { region_count: any }) => entry.region_count),
        this.region_name = this.regionchart_list.map((entry: { region_name: any }) => entry.region_name),
        this.crmregionchart = {
          chart: {
            type: 'bar',
            height: 355,
            width: '100%',
            background: 'White',
            foreColor: '#0F0F0F',
            fontFamily: 'inherit',
            toolbar: {
              show: false,
            },
          },
          colors: ['#7FC7D9', '#FFD54F', '#66BB6A', '#EF5350'],
          plotOptions: {
            bar: {
              horizontal: false,
              columnWidth: '50%',
              borderRadius: 0,
            },
          },
          dataLabels: {
            enabled: false,
          },
          xaxis: {
            categories: this.region_name,
            labels: {
              style: {

                fontSize: '12px',
              },
            },
          },
          yaxis: {
            title: {
              style: {
                fontWeight: 'bold',
                fontSize: '14px',
                color: '#7FC7D9',
              },
            },
          },
          series: [
            {
              name: 'Region',
              data: this.region_count,
              color: '#9EBF95',
            },

          ],
          // fill: {
          //   type: "gradient",
          //   gradient: {
          //     shadeIntensity: 1,
          //     opacityFrom: 0.7,
          //     opacityTo: 0.9,
          //     stops: [0, 100]
          //   }
          // }

        }
    });
  }
  Getcrmsourcechart() {
    var url = 'CrmDashboard/Getcrmsourcechart';
    this.service.get(url).subscribe((result: any) => {
      this.response_data = result;
      this.sourcechart_list = this.response_data.crmsourcechart_list;
      if (this.sourcechart_list.length > 0) {
        this.sourceflag = true;
      }
      this.source_name = this.sourcechart_list.map((entry: { source_name: any }) => entry.source_name),
        this.source_count = this.sourcechart_list.map((entry: { source_count: any }) => entry.source_count),
        this.crmsourcechart = {
          chart: {
            type: 'bar',
            height: 355,
            width: '100%',
            background: 'White',
            foreColor: '#0F0F0F',
            fontFamily: 'inherit',
            toolbar: {
              show: false,
            },
          },
          colors: ['#7FC7D9', '#FFD54F', '#66BB6A', '#EF5350'],
          plotOptions: {
            bar: {
              horizontal: false,
              columnWidth: '50%',
              borderRadius: 0,
            },
          },
          dataLabels: {
            enabled: false,
          },
          xaxis: {
            categories: this.source_name,
            labels: {
              style: {

                fontSize: '12px',
              },
            },
          },
          yaxis: {
            title: {
              style: {
                fontWeight: 'bold',
                fontSize: '14px',
                color: '#7FC7D9',
              },
            },
          },
          series: [
            {
              name: 'Source',
              data: this.source_count,
              color: '#8C8C8C',
            },
          ],
          // fill: {
          //   type: "gradient",
          //   gradient: {
          //     shadeIntensity: 1,
          //     opacityFrom: 0.7,
          //     opacityTo: 0.9,
          //     stops: [0, 100]
          //   }
          // }

        };
    });
  }


  Getcampaignsentchart() {
    var url = 'CrmDashboard/Getsentcampaignsentchart';
    this.service.get(url).subscribe((result: any) => {
      this.response_data = result;
      this.campaignsentchart_list = this.response_data.campaignsentchart_list;
      if (this.campaignsentchart_list.length > 0) {
        this.campaignflag1 = true;
      }
      this.whatsappsent_count = this.campaignsentchart_list.map((entry: { whatsappsent_count: any }) => entry.whatsappsent_count),
        this.mailsent_count = this.campaignsentchart_list.map((entry: { mailsent_count: any }) => entry.mailsent_count),
        this.months = this.campaignsentchart_list.map((entry: { months: any }) => {
          const monthYear = entry.months.split('-')[0];
          return monthYear.substring(0, 3);
        });
      this.crmcampaignsentchart = {
        chart: {
          type: 'line',
          height: 300,
          width: '100%',
          background: 'White',
          foreColor: '#0F0F0F',
          fontFamily: 'inherit',
          toolbar: {
            show: false,
          },
          offsetY: 30
        },
        colors: ['#7FC7D9', '#FFD54F', '#66BB6A', '#EF5350'],
        plotOptions: {
          bar: {
            horizontal: false,
            columnWidth: '50%',
            borderRadius: 0,
          },
        },
        dataLabels: {
          enabled: false,
        },
        xaxis: {
          categories: this.months,
          labels: {
            style: {

              fontSize: '12px',
            },
          },
        },
        yaxis: {
          title: {
            style: {
              fontWeight: 'bold',
              fontSize: '14px',
              color: '#7FC7D9',
            },
          },
        },
        series: [
          {
            name: 'WhatsApp',
            // type: "column",
            data: this.whatsappsent_count,
            color: '#3ee5ac'
          },
          {
            name: 'Mail',
            // type: "column",
            data: this.mailsent_count,
            color: '#0F7CBF'
          },
          // {
          //   name: 'SmS Campaign',
          //   // type: "line",
          //   data:[10,20,100,40,80,60],
          //   color: '#D9BA1E'
          // },
        ],
        // fill: {
        //   type: "gradient",
        //   gradient: {
        //     shadeIntensity: 1,
        //     opacityFrom: 0.7,
        //     opacityTo: 0.9,
        //     stops: [0, 100]
        //   }
        // },
        legend: {
          position: "bottom",
          offsetY: 5
        }
      };

    });
  }
  /////////////////////
  redirect_menu(data: string, j: number, k: number): void {
    if (data != null && data != "" && data != "#") {
      this.router.navigate([data]);
    }
  }

  Getprospectchart() {
    var url = 'CrmDashboard/Getoverallpropectchart';
    this.service.get(url).subscribe((result: any) => {
      this.response_data = result;
      this.overall_list = this.response_data.leadstagechart_list;
      if (this.overall_list.length > 0) {
        this.overallchartflag = true;
      }

      this.months_detail = this.overall_list.map((entry: { months_detail: any }) => entry.months_detail),
        this.prospects = this.overall_list.map((entry: { prospects: any }) => entry.prospects),
        this.potential = this.overall_list.map((entry: { potential: any }) => entry.potential),
        this.customer = this.overall_list.map((entry: { customer: any }) => entry.customer),
        this.drop_leads = this.overall_list.map((entry: { drop_leads: any }) => entry.drop_leads)
      this.leadstagechart = {
        chart: {
          type: 'bar',
          height: 280,
          width: '100%',
          background: 'White',
          foreColor: '#0F0F0F',
          fontFamily: 'inherit',
          toolbar: {
            show: false,
          },
        },
        colors: ['#7FC7D9', '#FFD54F', '#66BB6A', '#EF5350'],
        plotOptions: {
          bar: {
            horizontal: false,
            columnWidth: '50%',
            borderRadius: 0,
          },
        },
        dataLabels: {
          enabled: false,
        },
        xaxis: {
          categories: this.months_detail,
          labels: {
            style: {

              fontSize: '12px',
            },
          },
        },
        yaxis: {
          title: {
            style: {
              fontWeight: 'bold',
              fontSize: '14px',
              color: '#7FC7D9',
            },
          },
        },
        series: [
          {
            name: 'Evaluation',
            data: this.prospects,
            color: '#CE7F92',
          },
          {
            name: 'Decision',
            data: this.potential,
            color: '#747C8C',
          },
          {
            name: 'Purchase',
            data: this.customer,
            color: '#667967',
          },
          {
            name: 'Drop',
            data: this.drop_leads,
            color: '#B4584B',
          },
        ],
        legend: {
          position: "bottom",
          offsetY: 1
        }
        // fill: {
        //   type: "gradient",
        //   gradient: {
        //     shadeIntensity: 1,
        //     opacityFrom: 0.7,
        //     opacityTo: 0.9,
        //     stops: [0, 100]
        //   }
        // }
      };


    });
  }
  UpdateChart(chartType: string) {
    this.selectedChartType = chartType;
    switch (chartType) {
      case 'Lead':
        this.Getcrmleadchart();
        break;
      case 'Region':
        this.Getcrmregionchart();
        break;
      case 'Source':
        this.Getcrmsourcechart();
        break;
      default:
        this.Getcrmleadchart();
        break;
    }
  }
}