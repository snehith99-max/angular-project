

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
  selector: 'app-pmr-rpt-purchase-invoice-report',
  templateUrl: './pmr-rpt-purchase-invoice-report.component.html',
  styleUrls: ['./pmr-rpt-purchase-invoice-report.component.scss']
})
export class PmrRptPurchaseInvoiceReportComponent {
  chartOptions: any;
  Date: string;
  chart: ApexCharts | null = null;
  GetpmrInvoiceForLastSixMonths_List: any;
  GetpmrInvoiceDetailSummarylist: any;
  reactiveForm: FormGroup | any;
  responsedata: any;
  salesteamgrid_list: any;
  getData: any;
  expandedRows: any[] = [];
  toggleExpansion(index: number) {
    this.expandedRows[index] = !this.expandedRows[index];
  }
  expandedRows1: any[] = [];
  toggleExpansion1(index: number) {
    this.expandedRows1[index] = !this.expandedRows1[index];
  }
  salesorder_gid: any;
  data: any;
  parameterValue: any;
  from_date!: string;
  to_date!: string;
  isExpand: boolean = false;
  individualreportopen: boolean = false;
  pmrindividualreport_list: any[] = [];
  expandedRowIndex: number | null = null;
  flag: boolean = true;
  maxDate!:string;
  constructor(private formBuilder: FormBuilder, private NgxSpinnerService: NgxSpinnerService, public route: ActivatedRoute, public service: SocketService, private router: Router, private ToastrService: ToastrService) {
    this.Date = new Date().toString();
  }


  ngOnInit(): void {

    this.GetpmrInvoiceForLastSixMonths();
    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);
    
    const today = new Date();
this.maxDate = today.toISOString().split('T')[0];
  }
  GetpmrInvoiceForLastSixMonths() {
    this.NgxSpinnerService.show();
    var url = 'PmrTrnInvoice/GetpmrInvoiceForLastSixMonths'
    this.service.get(url).subscribe((result: any) => {
      $('#GetpmrInvoiceForLastSixMonths_List').DataTable().destroy();
      this.responsedata = result;
      this.GetpmrInvoiceForLastSixMonths_List = this.responsedata.GetpmrInvoiceForLastSixMonths_List;
      this.NgxSpinnerService.hide();
      if (this.GetpmrInvoiceForLastSixMonths_List == null) {
        this.flag = false;
      }
      const categories = this.GetpmrInvoiceForLastSixMonths_List.map((entry: { months: any; }) => entry.months);
      const data = this.GetpmrInvoiceForLastSixMonths_List.map((entry: { invoiceamount1: any; }) => entry.invoiceamount1);
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
            name: 'Sales Amount',
            color: '#9b98b8',
            data: data,
          },
        ],
      };

    })
    this.renderChart()
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
    var url = 'PmrTrnInvoice/GetpmrInvoiceReportForLastSixMonthsSearch';
    this.NgxSpinnerService.show()
    let params = {
      from_date: this.from_date,
      to_date: this.to_date
    }
    this.service.getparams(url, params).subscribe((result: any) => {
      $('#GetpmrInvoiceForLastSixMonths_List').DataTable().destroy();
      this.responsedata = result;
      this.GetpmrInvoiceForLastSixMonths_List = this.responsedata.GetpmrInvoiceForLastSixMonths_List;
      if (this.GetpmrInvoiceForLastSixMonths_List == null) {
        this.flag = false;
      }
      ApexCharts.exec('chart', 'destroy');
      this.chartOptions.xaxis.categories = this.GetpmrInvoiceForLastSixMonths_List.map((entry: { months: any }) => entry.months);
      this.chartOptions.series[0].data = this.GetpmrInvoiceForLastSixMonths_List.map((entry: { invoiceamount1: any }) => entry.invoiceamount1);
      this.NgxSpinnerService.hide()
      this.renderChart()
    })
  }
  onrefreshclick() {
    this.from_date = null!;
    this.to_date = null!;
    this.GetpmrInvoiceForLastSixMonths();
  }
  ondetail(month: any, year: any) {
    this.GetpmrInvoiceDetailSummarylist = null;
    this.isExpand = true;
   console.log("cejcjning")
    var url = 'PmrTrnInvoice/GetpmrInvoiceDetailSummary'
    let param = {
      from_date: this.from_date,
      to_date: this.to_date,
      month: month,
      year: year
    }
    setTimeout(() => {
    this.service.getparams(url, param).subscribe((result: any) => {
      // $('#GetpmrInvoiceDetailSummarylist').DataTable().destroy();
      this.responsedata = result;
      this.GetpmrInvoiceDetailSummarylist = result.GetpmrInvoiceDetailSummary;
      window.scrollBy(0, 400);
      setTimeout(() => {
        $('#GetpmrInvoiceDetailSummarylist').DataTable();
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
    this.GetpmrInvoiceForLastSixMonths_List.forEach((data: any, i: number) => {
      data.isExpand = (i === this.expandedRowIndex);
    });
  }
  onindividualreport(invoice_gid: any) {
    this.individualreportopen = true;
    this.NgxSpinnerService.show();
    var url = 'PmrTrnInvoice/GetIndividualreport'
    let param = {
      invoice_gid: invoice_gid,
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.pmrindividualreport_list = result.pmrindividualreport_list;
    });
    this.NgxSpinnerService.hide();
    this.scrollPageToBottom();
  }

  toggleVisibility(item: any) {
    item.isExpand = !item.isExpand;
  }
  
  private scrollPageToBottom(): void {
    window.scrollTo(0, document.body.scrollHeight);
  }
  individualreportclose() {
    this.individualreportopen = false;
    window.scrollTo({
      top: 0, 
    });
    this.GetpmrInvoiceForLastSixMonths();
  }

}





