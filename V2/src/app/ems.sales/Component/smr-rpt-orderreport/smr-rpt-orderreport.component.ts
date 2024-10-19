import { Component, } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators, } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { data } from 'jquery';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { NgxSpinnerService } from 'ngx-spinner';
import { timeout } from 'rxjs';


@Component({
  selector: 'app-smr-rpt-orderreport',
  templateUrl: './smr-rpt-orderreport.component.html',
  styleUrls: ['./smr-rpt-orderreport.component.scss']
})

export class SmrRptOrderreportComponent {
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
  params22:any;
  param11:any;
  mdlUserName:any;
  parameterValue: any;
  from_date!: string;
  salesperson_list:any[]=[];
  to_date!: string;
  isExpand: boolean = false;
  individualreportopen: boolean = false;
  individualreport_list: any[] = [];
  flag: boolean = true;
  maxDate!:string;
  expandedRowIndex: number | null = null;
  expandedRows: any[] = [];
  toggleExpansion(index: number) {
    this.expandedRows[index] = !this.expandedRows[index];
  }
  expandedRows1: any[] = [];
  toggleExpansion1(index: number) {
    this.expandedRows1[index] = !this.expandedRows1[index];
  }
  constructor(private formBuilder: FormBuilder, private NgxSpinnerService: NgxSpinnerService, public route: ActivatedRoute, public service: SocketService, private router: Router, private ToastrService: ToastrService) {
    this.Date = new Date().toString();
  }


  ngOnInit(): void {

    var url = 'SmrRptOrderReport/salespersondropdown';
    this.service.get(url).subscribe((result:any)=>{
      this.salesperson_list = result.salesperson_list
    })

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
    var url = 'SmrRptOrderReport/GetOrderForLastSixMonths'
    this.service.get(url).subscribe((result: any) => {
      $('#GetOrderForLastSixMonths_List').DataTable().destroy();
      this.responsedata = result;
      this.GetOrderForLastSixMonths_List = this.responsedata.GetOrderForLastSixMonths_List;
      this.NgxSpinnerService.hide();
      if (this.GetOrderForLastSixMonths_List == null) {
        this.flag = false;
      }
      const categories = this.GetOrderForLastSixMonths_List.map((entry: { salesorder_date: any; }) => entry.salesorder_date);
      const data = this.GetOrderForLastSixMonths_List.map((entry: { amount1: any; }) => entry.amount1);
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
          labels: {
            formatter: (value: number) => {
              return value.toLocaleString();
            },
          }
        },
        series: [
          {
            name: 'Sales Amount',
            color: '#9b98b8',
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
  onclearsalesperson(){
    this.mdlUserName = null;
  }
  onSearchClick() {
    debugger
    var url = 'SmrRptOrderReport/GetOrderReportForLastSixMonthsSearch';
    this.NgxSpinnerService.show()
    if(this.mdlUserName != null && this.from_date !=null && this.to_date != null){
     this.params22 = {
      from_date: this.from_date,
      to_date: this.to_date,
      sales_person:this.mdlUserName
    }
  }
  else if(this.mdlUserName !=null && this.from_date == null && this.to_date ==null){
     this.params22 = {
      sales_person:this.mdlUserName,
      from_date: undefined,
      to_date: undefined,
    }
  }
  else{
     this.params22 = {
      from_date: this.from_date,
      to_date: this.to_date,
      sales_person:undefined,
    }
  }
    this.service.getparams(url, this.params22).subscribe((result: any) => {
      $('#GetOrderForLastSixMonths_List').DataTable().destroy();
      this.responsedata = result;
      this.GetOrderForLastSixMonths_List = this.responsedata.GetOrderForLastSixMonths_List;
      if (this.GetOrderForLastSixMonths_List == null) {
        this.flag = false;
      }
      ApexCharts.exec('chart', 'destroy');
      this.chartOptions.xaxis.categories = this.GetOrderForLastSixMonths_List.map((entry: { salesorder_date: any }) => entry.salesorder_date);
      this.chartOptions.series[0].data = this.GetOrderForLastSixMonths_List.map((entry: { amount1: any }) => entry.amount1);
      this.NgxSpinnerService.hide()
      this.renderChart()
    })
  }
  onrefreshclick() {
    this.from_date = null!;
    this.to_date = null!;
    this.mdlUserName = null;
    this.GetOrderForLastSixMonths();
  }
  ondetail(month_wise: any) {
    this.isExpand = true;
    this.GetOrderDetailSummary = null;
    this.NgxSpinnerService.show();
    var url = 'SmrRptOrderReport/GetOrderDetailSummary'
    if(this.mdlUserName == null){
    this.param11 = {
      month_wise:month_wise,
      sales_person : undefined
     
    }
  }
  else{
    this.param11 = {
      month_wise:month_wise,
      sales_person : this.mdlUserName
     
    }
  }
    setTimeout(() => {
    this.service.getparams(url, this.param11).subscribe((result: any) => {
      this.responsedata = result;
      this.GetOrderDetailSummary = result.GetOrderDetailSummary;
      window.scrollBy(0, 400);
      setTimeout(() => {
        $('#GetOrderDetailSummary');
        this.NgxSpinnerService.hide();
      }, 1);
    });
  }, 100);
  }
  onRowClick(index: number) {
    if (this.expandedRowIndex === index) {
      this.expandedRowIndex = null; // Collapse if the same row is clicked again
    } else {
      this.expandedRowIndex = index; // Expand the clicked row
    }
    this.GetOrderForLastSixMonths_List.forEach((data: any, i: number) => {
      data.isExpand = (i === this.expandedRowIndex);
    });
  }

  onindividualreport(salesorder_gid: any) {
    this.individualreportopen = true;
    this.NgxSpinnerService.show();
    var url = 'SmrRptOrderReport/GetIndividualreport'
    let param = {
      salesorder_gid: salesorder_gid,
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.individualreport_list = result.individualreport_list;
    });
    this.NgxSpinnerService.hide();
    this.scrollPageToBottom();
  }

  
  private scrollPageToBottom(): void {
    window.scrollTo(0, document.body.scrollHeight);
  }
  individualreportclose() {
    this.individualreportopen = false;
    window.scrollTo({
      top: 0, 
    });
    this.GetOrderForLastSixMonths();
  }

}




