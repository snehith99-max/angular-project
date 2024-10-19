import { Component, OnInit, ViewChild } from "@angular/core";
import { ChartComponent } from "ng-apexcharts";
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { Subscription, Observable, timer } from 'rxjs';
import { first } from 'rxjs/operators';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { DatePipe } from '@angular/common';
import { map, share } from "rxjs/operators";
import { Pipe } from "@angular/core";
import {
  ApexNonAxisChartSeries,
  ApexResponsive,
  ApexChart
} from "ng-apexcharts";
import { SharedService } from "src/app/layout/services/shared.service";
import { an } from "@fullcalendar/core/internal-common";
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
  selector: 'app-sbc-mst-dashboard',
  templateUrl: './sbc-mst-dashboard.component.html',
  styleUrls: ['./sbc-mst-dashboard.component.scss']
})
export class SbcMstDashboardComponent {
  currentDayName: string;
  portalchart: any = {};
  response_data: any;
  totalperformance_list: any[] = [];
  flag: boolean = false;
  flag1: boolean = false;
  mtd_month:any;
  server_name: any;
  company_code: any;
  db_count: any;
  server_count: any;
  responsedata: any;
  chart: any;
  DatabaseCount_List: any[] = [];
  total_count: any;
  subscriptiontilescount_list: any[] = [];
  count_server:any;
  monthwisedatabase: any = {};
  month:any;
  performance_list:any[]=[];
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
  ngOnInit(): void {
    this.Getportalchart();
    this.Getmonthwisedatabase();
    this.Getsubscriptiontilescount();
    var api7 = 'Dashboard/Gettotaldatabasecount';
    this.service.get(api7).subscribe((result: any) => {
      this.responsedata = result;
      this.DatabaseCount_List = this.responsedata.portalchart_list;
      this.total_count=this.DatabaseCount_List[0].company_code
    });
    var api7 = 'Dashboard/Gettotalservercount';
    this.service.get(api7).subscribe((result: any) => {
      this.responsedata = result;
      this.DatabaseCount_List = this.responsedata.portalchart_list;
      this.count_server=this.DatabaseCount_List[0].server_name
    });
    var api7 = 'Dashboard/Getmonthwiseservertilescount';
    this.service.get(api7).subscribe((result: any) => {
      this.responsedata = result;
      this.DatabaseCount_List = this.responsedata.portalchart_list;
      this.mtd_month=this.DatabaseCount_List[0].mtd_month
    });
  }
  Getportalchart() {
    var url = 'Dashboard/Getportalchart';
    this.service.get(url).subscribe((result: any) => {
      this.response_data = result;
      this.totalperformance_list = this.response_data.portalchart_list;
      if (this.totalperformance_list.length > 0) {
        this.flag = true;
      }
      this.server_name = this.totalperformance_list.map((entry: { server_name: any }) => entry.server_name),
        this.company_code = this.totalperformance_list.map((entry: { company_code: any }) => entry.company_code)
      // Initialize chart options
      // this.renderChart(),
        this.portalchart = {
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
            categories: this.server_name,
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
              name: 'Database',
              data: this.company_code,
              color: '#3D9DD9',
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
  Getsubscriptiontilescount() {
    var url = 'Dashboard/Getsubscriptiontilescount';
    this.service.get(url).subscribe((result: any) => {
      this.response_data = result;      
      this.company_code = result.company_code;      
    });
    var url = 'Dashboard/Getservertilescount';
    this.service.get(url).subscribe((result: any) => {
      this.response_data = result;      
      this.server_name = result.server_name;      
    });
    var url = 'Dashboard/Getmonthwiseservercount';
    this.service.get(url).subscribe((result: any) => {
      this.response_data = result;
      this.server_name = result.server_name;      
    });
  }
  Getmonthwisedatabase() {
    var url = 'Dashboard/Getmonthwisedbchart';
    this.service.get(url).subscribe((result: any) => {
      this.response_data = result;
      this.performance_list = this.response_data.monthwisedbchart_list;
      if (this.performance_list.length > 0) {
        this.flag1 = true;
      }
      this.db_count = this.performance_list.map((entry: { db_count: any }) => entry.db_count),
      this.server_count = this.performance_list.map((entry: { server_count: any }) => entry.server_count),       
        this.month = this.performance_list.map((entry: { month: any }) => {
          const monthYear = entry.month.split('-')[0];
          return monthYear.substring(0, 3);
        });
      this.monthwisedatabase = {
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
          categories: this.month,
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
            name: 'db',
            // type: "column",
            data: this.db_count,
            color: '#3ee5ac'
          },
          {
            name: 'server',
            // type: "column",
            data: this.server_count,
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
  // private renderChart(): void {
  //   if (this.chart) {
  //     this.chart.updateOptions(this.portalchart); // Update existing chart with new options
  //   } else {
  //     this.chart = new ApexCharts(document.getElementById('chart'), this.portalchart);
  //     this.chart.render();
  //   }
  // }
}
