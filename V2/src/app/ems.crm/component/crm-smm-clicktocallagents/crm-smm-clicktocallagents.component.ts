import { Component, } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { Subscription, Observable, timer } from 'rxjs';
import { map, share } from "rxjs/operators";
import { DatePipe } from '@angular/common';
import {
  ApexNonAxisChartSeries,
  ApexResponsive,
  ApexChart
} from "ng-apexcharts";

@Component({
  selector: 'app-crm-smm-clicktocallagents',
  templateUrl: './crm-smm-clicktocallagents.component.html',
  styleUrls: ['./crm-smm-clicktocallagents.component.scss']
})
export class CrmSmmClicktocallagentsComponent {
  responsedata: any;
  analytics_summarylist: any[] = [];
  agent_report: any[] = [];
  currentDayName: any;
  weekchart: any = {};
  response_data: any;
  series_Value: any;
  labels_value: any;
  answered: any;
  total_count: any;
  customer_missed: any;
  agent_missed: any;
  agentreport: any = {};
  daywisechart: any = {};
  selectedChartType: any;
  inboundanalytics_report: any[] = [];
  outboundanalytics_report: any[] = [];
  show = true;
  emptyFlag: boolean = false;
  agent_barchartreport: any[] = [];
  dateanalytics_report: any[] = [];
  barchart_status: any;
  constructor(private formBuilder: FormBuilder, private NgxSpinnerService: NgxSpinnerService, private route: Router, private router: Router, private ToastrService: ToastrService, public service: SocketService, private datePipe: DatePipe) {
    const today = new Date();
    this.currentDayName = today.toLocaleDateString('en-US', { weekday: 'long' });
  }
  ngOnInit(): void {

    this.GetagentSummary();
    this.Getweekchart();
    this.Getdaywisechart();

    const api = 'clicktocall/callSummary';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.agent_barchartreport = this.responsedata.agent_barchartreport;
      if (this.agent_barchartreport !== null && this.agent_barchartreport.length > 0) {
        this.answered = Number(this.agent_barchartreport[0].answered);
        this.barchart_status = this.agent_barchartreport[0].status;
        this.agent_missed = Number(this.agent_barchartreport[0].agent_missed);
        this.customer_missed = Number(this.agent_barchartreport[0].customer_missed);

        this.series_Value = [this.answered, this.agent_missed, this.customer_missed];
        this.labels_value = ["ANSWERED", "AGENT MISSED", "CUSTOMER MISSED"];

        this.agentreport = {
          series: this.series_Value,
          labels: this.labels_value,
          chart: {
            width: 430,
            type: "donut"
          },
          responsive: [
            {
              breakpoint: 480,
              options: {
                chart: {
                  width: 200
                },
                legend: {
                  position: "bottom"
                }
              }
            }
          ],
          fill: {
            type: "gradient"
          },
        };
      } else {
        // No records found
        this.emptyFlag = true;
        console.log("No Records found");
      }
    });


  }
  Getweekchart() {
    var url = 'clicktocall/Getweekwiseclicktocallchart'
    this.service.get(url).subscribe((result: any) => {

      this.response_data = result;

      this.inboundanalytics_report = this.response_data.inboundanalytics_report;
      this.outboundanalytics_report = this.response_data.outboundanalytics_report;
      this.dateanalytics_report = this.response_data.dateanalytics_report;
      const categories = this.dateanalytics_report.map((entry: { week_date: any; }) => entry.week_date);
      const record = this.inboundanalytics_report.map((entry: { weekly_user: any; }) => entry.weekly_user);
      const record1 = this.outboundanalytics_report.map((entry: { weekly_user: any; }) => entry.weekly_user);

      // console.log(categories)
      // console.log(data)
      // Initialize chart options
      this.weekchart = {
        chart: {
          type: 'bar',
          height: 300, // Adjust the height of the chart as needed
          // width: 600,
          background: 'White',
          foreColor: '#0F0F0F',
          toolbar: {
            show: true,
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
          categories: categories,
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
            name: 'Inbound',
            data: record,
            color: '#b975fe', //#1a70d9
          },
          {
            name: 'Outbound',
            data: record1,

            color: '#ff9898', //#1a70d9
          },
        ],
      };
    })
  }
  Getdaywisechart() {
    var url = 'clicktocall/Getdaywisechartforclicktocall'
    this.service.get(url).subscribe((result: any) => {


      this.response_data = result;

      this.inboundanalytics_report = this.response_data.inboundanalytics_report;
      this.outboundanalytics_report = this.response_data.outboundanalytics_report;
      this.dateanalytics_report = this.response_data.dateanalytics_report;


      // this.monthlySalesData = result || [];

      // Transform data for chart
      const categories1 = this.dateanalytics_report.map((entry: { daily_date: any; }) => entry.daily_date);
      const data = this.inboundanalytics_report.map((entry: { daily_users: any; }) => entry.daily_users);
      const data1 = this.outboundanalytics_report.map((entry: { daily_users: any; }) => entry.daily_users);

      // console.log(categories)
      // console.log(data)
      // Initialize chart options
      this.daywisechart = {
        chart: {
          type: 'bar',
          height: 300, // Adjust the height of the chart as needed
          // width: 600,
          background: 'White',
          foreColor: '#0F0F0F',
          toolbar: {
            show: true, // Set to false to hide the toolbar/menu icon
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
          categories: categories1,
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
            name: 'Inbound',
            data: data,
            color: '#4095F6', //#1a70d9
          },
          {
            name: 'Outbound',
            data: data1,

            color: '#063970', //#1a70d9
          },
        ],
      };

    })
  }

  updateChart(chartType: string) {
    this.selectedChartType = chartType;
    switch (chartType) {
      case 'DayWise':
        this.Getdaywisechart();
        break;
      case 'week':
        this.Getweekchart();
        break;
      default:
        this.daywisechart();
        break;
    }
  }
  GetagentSummary() {
    this.NgxSpinnerService.show();
    var url = 'clicktocall/GetagentSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#agent_report').DataTable().destroy();
      this.responsedata = result;
      this.agent_report = this.responsedata.agent_report;
      setTimeout(() => {
        $('#agent_report').DataTable();
      }, 1);
      this.NgxSpinnerService.hide();
    });
  }
}




