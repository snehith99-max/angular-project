import { Component, ViewChild} from "@angular/core";
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { Router } from '@angular/router';
import {
  ChartComponent,
  ApexAxisChartSeries,
  ApexChart,
  ApexXAxis,
  ApexTitleSubtitle,
  ApexResponsive,
  ApexStroke,
  ApexFill,
  ApexGrid, ApexMarkers ,ApexDataLabels,
} from "ng-apexcharts";

export type ChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  responsive: ApexResponsive[];
  xaxis: ApexXAxis;
  title: ApexTitleSubtitle;
  labels: any;
  stroke: ApexStroke;
  fill: ApexFill;
  markers: ApexMarkers;
  dataLabels: ApexDataLabels;
  grid: ApexGrid;
};
@Component({
  selector: 'app-lgl-dashboard',
  templateUrl: './lgl-dashboard.component.html',
  styleUrls: ['./lgl-dashboard.component.scss']
})
export class LglDashboardComponent {
  @ViewChild("chart") chart: ChartComponent | any;
  public chartOptions: Partial<ChartOptions> | any;
  public Options: Partial<ChartOptions> | any;
  public polarArea: Partial<ChartOptions> | any;
  public cOptions: Partial<ChartOptions> | any;
  casetype_count: any;
  institute_count: any;
  Inactive_count: any;
  active_count: any;
  case_week: any;
  case_month: any;
  case_count: any;
  responsedata: any;
  lgldashboard_list: any[] = [];
  constructor(private router: Router, private service: SocketService) {
    this.chartOptions = {
      series: [
        {
          name: "My-series",
          data: [10, 35, 41, 51, 59, 62, 69, 91, 148]
        }
      ],
      chart: {
        height: 250,
        type: "bar"
      },
      
      xaxis: {
        categories: ["Jan", "Feb",  "Mar",  "Apr",  "May",  "Jun",  "Jul",  "Aug", "Sep","Oct"]
      }
    };
    this.Options = {
      series: [44, 55, 43, 43, 22],
      chart: {
        type: "donut"
      },
      labels: ["Team A", "Team B", "Team C", "Team D", "Team E"],
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
      ]
    };
    this.polarArea = {
      series: [
        {
          name: "Desktops",
          data: [10, 41, 35, 51, 49, 62, 69, 91, 148]
        }
      ],
      chart: {
        height: 250,
        type: "line",
        zoom: {
          enabled: false
        }
      },
      dataLabels: {
        enabled: false
      },
      stroke: {
        curve: "straight"
      },
      title: {
        text: "Product Trends by Month",
        align: "left"
      },
      grid: {
        row: {
          colors: ["#f3f3f3", "transparent"], // takes an array which will be repeated on columns
          opacity: 0.5
        }
      },
      xaxis: {
        categories: [
          "Jan",
          "Feb",
          "Mar",
          "Apr",
          "May",
          "Jun",
          "Jul",
          "Aug",
          "Sep"
        ]
      }
    };
  
    this.cOptions = {
      series: [
        {
          name: "Case",
          type: "area",
          data: [60, 55, 31, 47, 31, 43, 26, 41, 31, 47, 33,12]
        },
        {
          name: "Institute",
          type: "line",
          data: [85, 69, 45, 61, 43, 54, 37, 52, 44, 61, 43,21]
        }
      ],
      chart: {
        height: 250,
        type: "line"
      },
      stroke: {
        curve: "smooth"
      },
      fill: {
        type: "solid",
        opacity: [0.15, 1]
      },
      labels: [
        "Jan",
          "Feb",
          "Mar",
          "Apr",
          "May",
          "Jun",
          "Jul",
          "Aug",
          "Sep",
          "Oct",
          "Nov",
          "Dec"
      ],
      markers: {
        size: 0
      },
      yaxis: [
        {
          title: {
            text: "Series A"
          }
        },
        {
          opposite: true,
          title: {
            text: "Series B"
          }
        }
      ],
      xaxis: {
        labels: {
          trim: false
        }
      },
      tooltip: {
        shared: true,
        intersect: false,
        y: {
          formatter: function(y: number) {
            if (typeof y !== "undefined") {
              return y.toFixed(0) + " points";
            }
            return y;
          }
        }
      }
    };
  }

  ngOnInit() {
     this.getdashboarddata();
  }
  getdashboarddata(){
    debugger
    var api = 'LglMstDashboard/Getlgldashboardcountsummary';
    this.service.get(api).subscribe((result: any) => {

      this.responsedata = result;
      this.lgldashboard_list = this.responsedata.lgldashboard_list;
      this.casetype_count = this.lgldashboard_list[0].casetype_count;
      this.institute_count =this.lgldashboard_list[0].institute_count;
      this.Inactive_count =this.lgldashboard_list[0].Inactive_count;
      this.active_count = this.lgldashboard_list[0].active_count;
      this.case_week = this.lgldashboard_list[0].case_week;
      this.case_month = this.lgldashboard_list[0].case_month;
      this.case_count =this.lgldashboard_list[0].case_count;
    });
  }
  public generateData(count:any, yrange:any) {
    var i = 0;
    var series = [];
    while (i < count) {
      var x = "w" + (i + 1).toString();
      var y =
        Math.floor(Math.random() * (yrange.max - yrange.min + 1)) + yrange.min;

      series.push({
        x: x,
        y: y
      });
      i++;
    }
    return series;
  }

  }