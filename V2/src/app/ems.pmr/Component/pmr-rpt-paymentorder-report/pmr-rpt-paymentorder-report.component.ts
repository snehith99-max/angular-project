
import { Component, } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators, } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { data } from 'jquery';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { NgxSpinnerService } from 'ngx-spinner';
@Component({
  selector: 'app-pmr-rpt-paymentorder-report',
  templateUrl: './pmr-rpt-paymentorder-report.component.html',
  styleUrls: ['./pmr-rpt-paymentorder-report.component.scss']
})
export class PmrRptPaymentorderReportComponent {
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
  expandedRowIndex: number | null = null;
  expandedRows: any[] = [];
  toggleExpansion(index: number) {
    this.expandedRows[index] = !this.expandedRows[index];
  }
  expandedRows1: any[] = [];
  toggleExpansion1(index: number) {
    this.expandedRows1[index] = !this.expandedRows1[index];
  }
  isExpand: boolean = false;
  individualreportopen: boolean = false;
  individualreport_list: any[] = [];
  flag: boolean = true;
  maxDate!:string;
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
    var url = 'PmrRptPaymentReport/GetPaymentReportforSixMonths'
    this.service.get(url).subscribe((result: any) => {
      $('#GetPurchaseOrderForLastSixMonths_List').DataTable().destroy();
      this.responsedata = result;
      this.GetOrderForLastSixMonths_List = this.responsedata.GetPaymentOrderForLastSixMonths_List;
      this.NgxSpinnerService.hide();
      if (this.GetOrderForLastSixMonths_List == null) {
        this.flag = false;
      }
      const categories = this.GetOrderForLastSixMonths_List.map((entry: { month_na: any; }) => entry.month_na);
      const data = this.GetOrderForLastSixMonths_List.map((entry: { payment_amount: any; }) => entry.payment_amount);
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
            name: 'Purchase Amount',
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
  onSearchClick() {
    var url = 'PmrRptPaymentReport/GetPaymentReportForLastSixMonthsSearch';
    this.NgxSpinnerService.show()
    let params = {
      from_date: this.from_date,
      to_date: this.to_date
    }
    this.service.getparams(url, params).subscribe((result: any) => {
      $('#GetPurchaseOrderForLastSixMonths_List').DataTable().destroy();
      this.responsedata = result;
      this.GetOrderForLastSixMonths_List = this.responsedata.GetPaymentForLastSixMonths_List;
      if (this.GetOrderForLastSixMonths_List == null) {
        this.flag = false;
      }
      ApexCharts.exec('chart', 'destroy');
      this.chartOptions.xaxis.categories = this.GetOrderForLastSixMonths_List.map((entry: { payment_date: any }) => entry.payment_date);
      this.chartOptions.series[0].data = this.GetOrderForLastSixMonths_List.map((entry: { payment_amount: any }) => entry.payment_amount);
      this.NgxSpinnerService.hide()
      this.renderChart()
    })
  }
  onrefreshclick() {
    this.from_date = null!;
    this.to_date = null!;
    this.GetOrderForLastSixMonths();
  }
  // ondetail(month: any, year: any, parameter: string, purchaseorder_gid: string) {
  //   this.isExpand = true;
  //   this.NgxSpinnerService.show();
  //   var url = 'PmrRptPurchaseOrder/GetPurchaseOrderDetailSummary'
  //   let param = {
  //     from_date: this.from_date,
  //     to_date: this.to_date,
  //     purchaseorder_gid: purchaseorder_gid,
  //     month: month,
  //     year: year
  //   }
  //   this.service.getparams(url, param).subscribe((result: any) => {
  //     this.responsedata = result;
  //     this.GetOrderDetailSummary = result.GetOrderDetailSummary;
  //     window.scrollBy(0, 400);
  //     setTimeout(() => {
  //       $('#GetOrderDetailSummary');
  //       this.NgxSpinnerService.hide();
  //     }, 1);
  //   });
  // }
  ondetail(month_na: any, year: any, parameter: string, payment_gid: string){
    debugger
    this.isExpand = true;
    this.GetOrderDetailSummary = null;
    this.NgxSpinnerService.show();
    var url = 'PmrRptPaymentReport/GetPaymentReportDetailSummary'
    let param = {
          from_date: this.from_date,
          to_date: this.to_date,
          purchaseorder_gid: payment_gid,
          month: month_na,
          year: year
        }
    setTimeout(() => {
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.GetOrderDetailSummary = result.GetPaymentDetailSummary;
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


  onindividualreport(purchaseorder_gid: any) {
    this.individualreportopen = true;
    this.NgxSpinnerService.show();
    var url = 'PmrRptPurchaseOrder/GetIndividualreport'
    let param = {
      purchaseorder_gid: purchaseorder_gid,
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





