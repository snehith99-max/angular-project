import { Component, } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators,} from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import * as ApexCharts from 'apexcharts';
import { ApexOptions } from 'apexcharts';
import {
  ApexNonAxisChartSeries,
  ApexResponsive,
  ApexChart,
  ChartComponent 
} from "ng-apexcharts";
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';


@Component({
  selector: 'app-smr-rpt-quotationreport',
  templateUrl: './smr-rpt-quotationreport.component.html',
  styleUrls: ['./smr-rpt-quotationreport.component.scss']
})
export class SmrRptQuotationreportComponent {
  chartOptions: any = {};
  Date: string;
  GetQuotationForLastSixMonths_List :any;
  GetQuotationFromDate_List:any;
  chart: ApexCharts | null = null;
  GetOrderDetailSummary :any;
  GetQuotationSummary : any;
  GetOrderForLastSixMonths_List :any;
  responsedata: any;
  getData: any;
  data: any;  
  quotation_gid: any;
  parameterValue1: any;
  parameterValue: any;
  month:any;
  categories:any;
  from_date!:string;
  to_date!:string;
  isExpand : boolean = false;

 
  constructor(private formBuilder: FormBuilder,public route:ActivatedRoute,public service :SocketService,private router:Router,private ToastrService: ToastrService, private NgxSpinnerService: NgxSpinnerService) {
    this.Date = new Date().toString();
    
    
    

  }
  

  ngOnInit(): void {
    
    this.GetQuotationReportForLastSixMonths();
    const options: Options = {
      dateFormat: 'd-m-Y',    
    };
    flatpickr('.date-picker', options);
  }
  private initializeChart(categories: any, data: any): void {
    this.chartOptions = {
      chart: {
        type: 'bar',
        height: 400,
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
        categories:categories,
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
            color: '#7FC7D9',
          },
        },
        tickAmount: 8,
      },
      series: [
        {
          name: 'Quotation Amount',
          color:'#87CEEB',
          data:data,
        },
      ],
    };
    this.renderChart();
  }

  private renderChart(): void {
    if (this.chart) {
      this.chart.updateOptions(this.chartOptions); // Update existing chart with new options
    } else {
      this.chart = new ApexCharts(document.getElementById('chart'), this.chartOptions);
      this.chart.render();
    }
  }

  
  GetQuotationReportForLastSixMonths( )
  {
    var url = 'SmrRptQuotationReport/GetQuotationReportForLastSixMonths'
    this.service.get(url).subscribe((result: any) => {
      $('#GetQuotationForLastSixMonths_List').DataTable().destroy();
      this.responsedata = result;
      this.GetQuotationForLastSixMonths_List = this.responsedata.GetQuotationForLastSixMonths_List;
    this.initializeChart(
      this.GetQuotationForLastSixMonths_List.map((entry: { quotation_date: any }) => entry.quotation_date),
      this.GetQuotationForLastSixMonths_List.map((entry: { amount: any }) => entry.amount)
    );
    });  
  }
  onSearchClick() {
   
    var url= 'SmrRptQuotationReport/GetQuotationReportForLastSixMonthsSearch';
    this.NgxSpinnerService.show();
    let params ={
      from_date:this.from_date,
      to_date:this.to_date
    }
    
    this.service.getparams(url,params).subscribe((result: any) => {
      $('#GetQuotationForLastSixMonths_List').DataTable().destroy();
      this.responsedata = result;
      this.GetQuotationForLastSixMonths_List = this.responsedata.GetQuotationForLastSixMonths_List;
     

        this.chartOptions.xaxis.categories = this.GetQuotationForLastSixMonths_List.map((entry: { quotation_date: any }) => entry.quotation_date);
    this.chartOptions.series[0].data = this.GetQuotationForLastSixMonths_List.map((entry: { amount: any }) => entry.amount);
      
      this.renderChart();
      this.NgxSpinnerService.hide();
    });
  }
  onrefreshclick(){
    this.from_date = null!;
    this.to_date = null!;
    this.GetQuotationReportForLastSixMonths();
  }
  ondetail(month: any,year:any ,parameter: string,quotation_gid: string) {
    this.isExpand = true;
    this.parameterValue1 = parameter;
    this.quotation_gid = parameter;
    this.month = this.GetQuotationForLastSixMonths_List[0].month;
    this.NgxSpinnerService.show();
    var url = 'SmrRptQuotationReport/GetQuotationDetailSummary'
    let param = {
      from_date:this.from_date,
      to_date: this.to_date,
      quotation_gid : quotation_gid ,
      month: month, year: year
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.GetQuotationSummary = result.GetQuotationSummary;
      window.scrollBy(0,600);
      setTimeout(() => {
        $('#GetQuotationSummary');
      }, 1);
      this.NgxSpinnerService.hide();
    });
  }
}




