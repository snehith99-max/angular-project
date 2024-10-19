import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { environment } from 'src/environments/environment.development';
import { ExcelService } from 'src/app/Service/excel.service';
import { get } from 'jquery';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { NgxSpinnerService } from 'ngx-spinner';



interface IPaymentSummaryReport {
  branch_name: string;
  department_name: string;
  month: string;
  year: string;
  branch_gid: string;
  department_gid: string;
  salary_gid: string;
}

@Component({
  selector: 'app-pay-rpt-paymentreportsummary',
  templateUrl: './pay-rpt-paymentreportsummary.component.html',
  styleUrls: ['./pay-rpt-paymentreportsummary.component.scss'],
  
})
export class PayRptPaymentreportsummaryComponent {
  expandedRowIndex: number | null = null;
  chartOptions: any;
  chart: ApexCharts | null = null;
  GetEmployeeDetailsSummary: any;
  [x: string]: any;
  mdlBranch: any;
  reactiveForm!: FormGroup;
  branch_name: any;
  branchlist: any[] = [];
  paymentreport_list: any[] = [];
  paymentadd_list: any[] = [];
  Reportpayment_list: any[] = [];
  filteredpayment_list: any[] = [];
  PaymentSummaryReport!: IPaymentSummaryReport;
  responsedata: any;
  overall: any;
  grandtotal: any;
  data1: any;
  data: any;
  individualreportopen: boolean = false;
  maxDate!:string;
  from!: string;
  to!: string;
  flag: boolean = true;
  GetLastSixMonths_List: any[] = [];
  GetOrderForLastSixMonths_List: any[] = [];
  isExpand: boolean = false;




  // totalAmount: number;
  toggleExpansion(index: number) {
    this.expandedRows[index] = !this.expandedRows[index];
  }

  toggleExpansion1(index: number) {
    this.expandedRows1[index] = !this.expandedRows1[index];
  }

  expandedRows1: any[] = [];
  expandedRows: any[] = [];


  constructor(private formBuilder: FormBuilder,
    private excelService: ExcelService,
    private NgxSpinnerService: NgxSpinnerService,
    private route: ActivatedRoute,
    private router: Router, private ToastrService: ToastrService,
    public service: SocketService) {
    this.PaymentSummaryReport = {} as IPaymentSummaryReport;
  }

  ngOnInit(): void {

    
    // this.calculateTotalAmount();
    this.GetOrderForLastSixMonths();

    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);
    this.reactiveForm = new FormGroup({

      branch_name: new FormControl(this.PaymentSummaryReport.branch_name, [
        Validators.required,
        Validators.minLength(1),
      ]),
      from: new FormControl(''),
      to: new FormControl('')
    });


    var api = 'PayRptPayrunSummary/GetBranchDtl'
    this.service.get(api).subscribe((result: any) => {
      this.branchlist = result.GetBranchDtl;
    });


    var url = 'PayTrnReportPayment/GetPaymentSummary'
    this.service.get(url).subscribe((result: any) => {

      this.responsedata = result;
      this.paymentreport_list = this.responsedata.GetPaylist;
      for (let i = 0; i < this.paymentreport_list.length; i++) {
        const amount = parseFloat(this.paymentreport_list[i].Amount);

        if (!isNaN(amount)) {
          this.overall = this.overall + amount; 
        }
      }
      this.overall = this.formatCurrency(this.overall)


      setTimeout(() => {
        $('#paymentreport_list').DataTable();
      },);


    });
  }


  GetOrderForLastSixMonths() {
    debugger;
    this.NgxSpinnerService.show();
    var url = 'PayTrnReportPayment/GetLastSixMonths_List'
    this.service.get(url).subscribe((result: any) => {
      $('#GetLastSixMonths_List').DataTable().destroy();
      this.responsedata = result;
      this.GetLastSixMonths_List = this.responsedata.GetLastSixMonths_List;
      this.NgxSpinnerService.hide();
      if (this.GetLastSixMonths_List == null) {
        this.flag = false;
      }
      const categories = this.GetLastSixMonths_List.map((entry: { month: any; }) => entry.month);
      const data = this.GetLastSixMonths_List.map((entry: { amount: any; }) => entry.amount);
      

      this.chartOptions = {
        chart: {
          type: 'bar',
          height: 300,
          width: '100%',
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
          categories: categories,
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
          labels: {
            formatter: function(value: any) {
              if (value >= 10000000) {
                return (value / 10000000).toFixed(2) + ' Cr';
              } else if (value >= 100000) {
                return (value / 100000).toFixed(2) + ' L';
              } else if (value >= 1000) {
                return (value / 1000).toFixed(2) + ' K';
              } else {
                return value;
              }
            },
          },
        },
        series: [
          {
            name: 'Payment Amount',
            data: data,
            color: '#2d8625',
          }
        ],
        legend: {
          position: 'bottom',
          offsetY: 5,
        },
      };

    })
  }

  GetPaymentReportForLastSixMonthsSearch() {
    debugger;
    var url = 'PayTrnReportPayment/GetPaymentReportForLastSixMonthsSearch';
    this.NgxSpinnerService.show()
    let params = {
      from: this.from,
      to: this.to
    }
    this.service.getparams(url, params).subscribe((result: any) => {
      $('#GetLastSixMonths_List').DataTable().destroy();
      this.responsedata = result;
      this.GetLastSixMonths_List = this.responsedata.GetLastSixMonths_List;
      if (this.GetLastSixMonths_List == null) {
        this.flag = false;
      }
      // ApexCharts.exec('chart', 'destroy');
      // this.chartOptions.xaxis.categories = this.GetLastSixMonths_List.map((entry: { payment_date: any }) => entry.payment_date);
      // this.chartOptions.series[0].data = this.GetLastSixMonths_List.map((entry: { amount: any }) => entry.amount);
      // this.NgxSpinnerService.hide()
      // this.renderChart()
    })
  }


  // private renderChart(): void {
  //   if (this.chart) {
  //     this.chart.updateOptions(this.chartOptions); // Update existing chart with new options
  //   } else {
  //     this.chart = new ApexCharts(document.getElementById('chart'), this.chartOptions);
  //     this.chart.render();
  //   }
  // }

  

  exportExcel(): void {

      const PaymentReport = this.paymentreport_list.map(item => ({
      Year: item.year || '',
      Month: item.month || '',
      EmployeeCount: item.Employee_count || '',
      Amount: item.Amount || '',
    }));

    this.excelService.exportAsExcelFile(PaymentReport, 'Payment_Report');

  }

  exportExcel1(): void {

    const PaymentReport = this.Reportpayment_list.map(item => ({
      Department: item.department_name || '',
      EmployeeCode: item.user_code || '',
      EmployeeName: item.employee_name || '',
      Designation: item.designation_name || '',
      Amount: item.net_salary || ''
  }));

  this.excelService.exportAsExcelFile(PaymentReport, 'Payment_Report');

}

  ondetail1(month: any, year: any) {
    debugger;
    var url = 'PayTrnReportPayment/GetreportPaymentExpand'
    let param = {
      month: month,
      year: year
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      debugger;
      this.Reportpayment_list = result.GetreportExpand;

    });
  }


  exportbankExcel(): void {


    const BankReport = this.paymentadd_list.map(item => ({
      PaymentDate: item.payment_date || '',
      PaidAmount: item.paid_amount || '',
      NoOfEmployeesPaid: item.no_of_employees || '',
      ModeofPayment: item.modeof_payment || '',
      PaidBy: item.paid_by || '',
    }));


    this.excelService.exportAsExcelFile(BankReport, 'Bank_Report');

  }


  // pdf()
  // { debugger
  //   const doc = new jsPDF() as any
  //   var prepare: any[][]=[];
  //   this.paymentreport_list.forEach(e=>{
  //     var tempObj =[];
  //     tempObj.push(e.year);
  //     tempObj.push(e.month);
  //     tempObj.push(e.Employee_count);
  //     tempObj.push(e.Amount);
  //     prepare.push(tempObj);
  //   });
  //   console.log(prepare);
  //   autoTable(doc,{
  //       head: [['Year','Month','EmployeeCount','Amount']],
  //       body: prepare
  //   });
  //   doc.save('Payment_Report' + '.pdf');

  // }


  GetPaymentSummarybasedondate() {
    debugger
    var url = 'PayTrnReportPayment/GetPaymentSummarybasedondate';
    let params = {
      fromdate: this.reactiveForm.value.from,
      todate: this.reactiveForm.value.to,
      branch_name: this.reactiveForm.value.branch_name
    }
    this.service.getparams(url, params).subscribe((result: any) => {
      this.paymentreport_list = result.GetPaylist;
    });

  }

  formatCurrency(value: any) {
    const formattedValue = parseFloat(value).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
    return formattedValue;
  }

  // calculateTotalAmount() {
  //   this.totalAmount = this.paymentreport_list.reduce((sum, item) => sum + item.Amount, 0);
  // }


  onrefreshclick(){
    this.from = null!;
    this.to = null!;
    this.GetOrderForLastSixMonths();
  }

  ondetail(month_wise:any){
    debugger;

    this.isExpand = true;
    this.GetEmployeeDetailsSummary = null;
    this.NgxSpinnerService.show();
    var url = 'PayTrnReportPayment/GetEmployeeDetailsSummary'
    let param = {
      month_wise:month_wise,
     
    }
    setTimeout(() => {
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.GetEmployeeDetailsSummary = result.GetEmployeeDetailsSummary;
      window.scrollBy(0, 400);
      setTimeout(() => {
        $('#GetEmployeeDetailsSummary');
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
    this.GetLastSixMonths_List.forEach((data: any, i: number) => {
      data.isExpand = (i === this.expandedRowIndex);
    });
  }

}

