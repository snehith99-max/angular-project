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
  selector: 'app-smr-rpt-salesinvoicereport',
  templateUrl: './smr-rpt-salesinvoicereport.component.html',
  styleUrls: ['./smr-rpt-salesinvoicereport.component.scss']
})
export class SmrRptSalesinvoicereportComponent {
  chartOptions: any;
  Date: string;
  chart: ApexCharts | null = null;
  GetInvoiceForLastSixMonths_List: any;
  GetInvoiceDetailSummarylist: any;
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
  individualreport_list: any[] = [];
  expandedRowIndex: number | null = null;
  flag: boolean = true;
  maxDate!:string;
  constructor(private formBuilder: FormBuilder, private NgxSpinnerService: NgxSpinnerService, public route: ActivatedRoute, public service: SocketService, private router: Router, private ToastrService: ToastrService) {
    this.Date = new Date().toString();
  }


  ngOnInit(): void {

    this.GetInvoiceForLastSixMonths();
    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);
    
    const today = new Date();
this.maxDate = today.toISOString().split('T')[0];
  }
  GetInvoiceForLastSixMonths() {
    this.NgxSpinnerService.show();
    var url = 'SmrRptInvoiceReport/GetInvoiceForLastSixMonths'
    this.service.get(url).subscribe((result: any) => {
      $('#GetInvoiceForLastSixMonths_List').DataTable().destroy();
      this.responsedata = result;
      this.GetInvoiceForLastSixMonths_List = this.responsedata.GetInvoiceForLastSixMonths_List;
      this.NgxSpinnerService.hide();
      if (this.GetInvoiceForLastSixMonths_List == null) {
        this.flag = false;
      }
      const categories = this.GetInvoiceForLastSixMonths_List.map((entry: { months: any; }) => entry.months);
      const data = this.GetInvoiceForLastSixMonths_List.map((entry: { invoiceamount1: any; }) => entry.invoiceamount1);
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
    var url = 'SmrRptInvoiceReport/GetInvoiceReportForLastSixMonthsSearch';
    this.NgxSpinnerService.show()
    let params = {
      from_date: this.from_date,
      to_date: this.to_date
    }
    this.service.getparams(url, params).subscribe((result: any) => {
      $('#GetInvoiceForLastSixMonths_List').DataTable().destroy();
      this.responsedata = result;
      this.GetInvoiceForLastSixMonths_List = this.responsedata.GetInvoiceForLastSixMonths_List;
      if (this.GetInvoiceForLastSixMonths_List == null) {
        this.flag = false;
      }
      ApexCharts.exec('chart', 'destroy');
      this.chartOptions.xaxis.categories = this.GetInvoiceForLastSixMonths_List.map((entry: { months: any }) => entry.months);
      this.chartOptions.series[0].data = this.GetInvoiceForLastSixMonths_List.map((entry: { invoiceamount1: any }) => entry.invoiceamount1);
      this.NgxSpinnerService.hide()
      this.renderChart()
    })
  }
  onrefreshclick() {
    this.from_date = null!;
    this.to_date = null!;
    this.GetInvoiceForLastSixMonths();
  }
  ondetail(month: any, year: any) {
    this.GetInvoiceDetailSummarylist = null;
    this.isExpand = true;
   console.log("cejcjning")
    var url = 'SmrRptInvoiceReport/GetInvoiceDetailSummary'
    let param = {
      from_date: this.from_date,
      to_date: this.to_date,
      month: month,
      year: year
    }
    setTimeout(() => {
    this.service.getparams(url, param).subscribe((result: any) => {
      // $('#GetInvoiceDetailSummarylist').DataTable().destroy();
      this.responsedata = result;
      this.GetInvoiceDetailSummarylist = result.GetInvoiceDetailSummary;
      window.scrollBy(0, 400);
      setTimeout(() => {
        $('#GetInvoiceDetailSummarylist').DataTable();
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
    this.GetInvoiceForLastSixMonths_List.forEach((data: any, i: number) => {
      data.isExpand = (i === this.expandedRowIndex);
    });
  }
  onindividualreport(invoice_gid: any) {
    this.individualreportopen = true;
    this.NgxSpinnerService.show();
    var url = 'SmrRptOrderReport/GetIndividualreport'
    let param = {
      invoice_gid: invoice_gid,
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.individualreport_list = result.individualreport_list;
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
    this.GetInvoiceForLastSixMonths();
  }

}




