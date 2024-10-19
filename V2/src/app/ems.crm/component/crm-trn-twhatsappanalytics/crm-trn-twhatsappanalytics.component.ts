import { Component, } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators, } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { data } from 'jquery';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { NgxSpinnerService } from 'ngx-spinner';
import {MatButtonToggleModule} from '@angular/material/button-toggle';


@Component({
  selector: 'app-crm-trn-twhatsappanalytics',
  templateUrl: './crm-trn-twhatsappanalytics.component.html',
  styleUrls: ['./crm-trn-twhatsappanalytics.component.scss']
})
export class CrmTrnTwhatsappanalyticsComponent {
  chartOptions: any;
  Date: string;
  chart: ApexCharts | null = null;
  GetOrderForLastSixMonths_List: any;
  GetOrderDetailSummary: any;
  reactiveForm: FormGroup | any;
  responsedata: any;
  salesteamgrid_list: any;
  getData: any;
  salesorder_gid: any;
  data: any;
  parameterValue: any;
  from_date!: string;
  to_date!: string;
  isExpand: boolean = false;
  individualreportopen: boolean = false;
  individualreport_list: any[] = [];
  flag: boolean = true;
  maxDate!:string;
  activeButtonIndex: number = 0;
  selectedChartType1: any;
  selectedTab: any;

  onButtonClick(index: number) {
    this.activeButtonIndex = index;
  }

  isActive(index: number): boolean {
    return this.activeButtonIndex === index;
  }
  constructor(private formBuilder: FormBuilder, private NgxSpinnerService: NgxSpinnerService, public route: ActivatedRoute, public service: SocketService, private router: Router, private ToastrService: ToastrService) {
    this.Date = new Date().toString();
  }


  ngOnInit(): void {

    this.GetOrderForLastSixMonths();
    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);
    
    const today = new Date();
this.maxDate = today.toISOString().split('T')[0];
  }
  GetOrderForLastSixMonths() {
    this.NgxSpinnerService.show();
    var url = 'Whatsapp/Getsentcampaignsentchart'
    this.service.get(url).subscribe((result: any) => {
      $('#GetOrderForLastSixMonths_List').DataTable().destroy();
      this.responsedata = result;
      this.GetOrderForLastSixMonths_List = this.responsedata.campaignsentchart_lists;
      this.NgxSpinnerService.hide();
      if (this.GetOrderForLastSixMonths_List == null) {
        this.flag = false;
      }
      const categories = this.GetOrderForLastSixMonths_List.map((entry: { months: any; }) => entry.months);
      const data = this.GetOrderForLastSixMonths_List.map((entry: { whatsappsent_count: any; }) => entry.whatsappsent_count);
      this.chartOptions = {
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
        plotOptions: {
          bar: {
            horizontal: false,
            columnWidth: '25%',
            borderRadius: 3,
          },
        },
        dataLabels: {
          enabled: false,
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
            },
          },
        },
        yaxis: {
          title: {
            style: {
              fontWeight: 'bold',
              fontSize: '14px',
              color: '#FF0000',
            },
          },
          tickAmount: 8,
        },
        series: [
          {
            name: 'Campaign Sent',
            color: '#93D94E',
            data: data,
          },
        ],
      };

    })
  }
  private renderChart(): void {
    if (this.chart) {
      this.chart.updateOptions(this.chartOptions); // Update existing chart with new options
    } else {
      this.chart = new ApexCharts(document.getElementById('chart'), this.chartOptions);
      this.chart.render();
    }
  }
  onSearchClick() {
    var url = 'Whatsapp/GetcampaignSearch';
    this.NgxSpinnerService.show()
    let params = {
      from_date: this.from_date,
      to_date: this.to_date
    }
    this.service.getparams(url, params).subscribe((result: any) => {
      $('#GetOrderForLastSixMonths_List').DataTable().destroy();
      this.responsedata = result;
      this.GetOrderForLastSixMonths_List = this.responsedata.campaignsentchart_lists;
      if (this.GetOrderForLastSixMonths_List == null) {
        this.flag = false;
      }
      ApexCharts.exec('chart', 'destroy');
      this.chartOptions.xaxis.categories = this.GetOrderForLastSixMonths_List.map((entry: { months: any }) => entry.months);
      this.chartOptions.series[0].data = this.GetOrderForLastSixMonths_List.map((entry: { whatsappsent_count: any }) => entry.whatsappsent_count);
      this.NgxSpinnerService.hide()
      this.renderChart()
    })
  }
  onrefreshclick() {
    this.from_date = null!;
    this.to_date = null!;
    this.GetOrderForLastSixMonths();
    this.isExpand = false;

  }
  ondetail(month: any, year: any) {
    this.isExpand = true;
    this.NgxSpinnerService.show();
    var url = 'Whatsapp/GetsentDetailSummary'
    let param = {
      month: month,
      year: year
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.GetOrderDetailSummary = result.leadsummary;
      window.scrollBy(0, 400);
      setTimeout(() => {
        $('#GetOrderDetailSummary');
        this.NgxSpinnerService.hide();
      }, 1);
    });
  }

  individualreportclose() {
    this.individualreportopen = false;
    window.scrollTo({
      top: 0, 
    });
    this.GetOrderForLastSixMonths();
  }
  selectTab(status: any) {
    this.selectedTab = status;
    this.NgxSpinnerService.show();
    var url = 'Whatsapp/Getcurrentchart'
    this.NgxSpinnerService.show()
    let params = {
      status: status,
    }
    this.service.getparams(url, params).subscribe((result: any) => {
      $('#GetOrderForLastSixMonths_List').DataTable().destroy();
      this.responsedata = result;
      this.NgxSpinnerService.hide()
      this.GetOrderForLastSixMonths_List = this.responsedata.campaignsentchart_lists;
      if (this.GetOrderForLastSixMonths_List == null) {
        this.flag = false;
      }
      ApexCharts.exec('chart', 'destroy');
      this.chartOptions.xaxis.categories = this.GetOrderForLastSixMonths_List.map((entry: { months: any }) => entry.months);
      this.chartOptions.series[0].data = this.GetOrderForLastSixMonths_List.map((entry: { whatsappsent_count: any }) => entry.whatsappsent_count);
      this.NgxSpinnerService.hide()
      this.renderChart()
    })
  }

}




