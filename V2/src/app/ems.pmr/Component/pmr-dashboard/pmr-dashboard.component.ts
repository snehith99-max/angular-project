import { Component, OnInit,ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { Observable, interval, Subject, ReplaySubject } from 'rxjs';
import { takeWhile, map, takeUntil, catchError } from 'rxjs/operators';
import {
  ApexNonAxisChartSeries,
  ApexResponsive,
  ApexChart,
  ApexFill,
  ApexDataLabels,
  ApexLegend
} from "ng-apexcharts";
export type ChartOptions1 = {
  series: ApexNonAxisChartSeries;
  chart: ApexChart;
  responsive: ApexResponsive[];
  labels: any;
  DashboardCount_List: any
};
export type ChartOptions4 = {
  series: ApexNonAxisChartSeries;
  chart: ApexChart;
  responsive: ApexResponsive[];
  labels: any;
  DashboardCount_List: any
};

@Component({
  selector: 'app-pmr-dashboard',
  templateUrl: './pmr-dashboard.component.html',
  styleUrls: ['./pmr-dashboard.component.scss']
})

export class PmrDashboardComponent implements OnInit{
  chartOptions1: any = {};
  private destroy$ = new Subject<void>();
  // chartOptions!: ApexOptions; 
  chartOptions4: any = {};
  chartOptions: any = {};

  noleadstatus: any;
  response_data: any;

  GetOverallSalesOrderChart_List: any;

  DashboardQuotationAmt_List: any;

  noquotation: any;

  year: any;

  noquotation_status: any;

  show = true;
  emptyFlag: boolean = false;
  series_Value: any;
  labels_value: any;
  chartOptions2: any = {};
  GetOrderForLastSixMonths_List: any;
  GetOrderForLastSixMonths_List1: any;
  GetMonthSalesReportCount_list: any;
  GetDaySalesReportCount_List: any;
  GetWeekSalesReportCount_List: any;
  GetMonthSalesReportCount_List: any;
  GetSalesOrderCount_List: any[] = [];
  total_so: any;
  responsedata: any;
  // chartOptions: any;
  parameter: any;
  parameterValue: any;
  getData: any;
  total_payment: any;
  todaytask_count: any;
  pending_So: any;
  approved_so: any;
  advanced_paid: any;
  rejected_so: any;
  invoice_raised: any
  totalinvoice: any;
  delivery_done_partial: any;
  delivery_completed: any;
  approvalpendinginnvoice: any;
  payment_pending: any;
  approval_pending: any;
  quotation_canceled: any;
  quotation_status: any;
  total_quotation: any;
  completed_quotation: any;
  total_quotation1: number = 0;
  quotation_canceled1: number = 0;
  so_amended: any;
  quotation_amended: any;
  work_in_progress: any;
  payment_don_partial: any;
  compelted_payment: any;
  today_total_so: any;
  today_total_do: any;
  today_total_invoice: any;
  today_total_payment: any;
  today_invoice_amount: any;
  today_payment_amount: any;
  today_outstanding_amount: any;
  yest_total_so: any;
  yest_total_do: any;
  yest_total_invoice: any;
  yest_total_payment: any;
  yest_invoice_amount: any;
  yest_payment_amount: any;
  yest_outstanding_amount: any;
  cw_total_so: any;
  cw_total_do: any;
  cw_total_invoice: any;
  cw_total_payment: any;
  cw_invoice_amount: any;
  cw_payment_amount: any;
  cw_outstanding_amount: any;
  lw_total_so: any;
  lw_total_do: any;
  lw_total_invoice: any;
  lw_total_payment: any;
  lw_invoice_amount: any;
  lw_payment_amount: any;
  lw_outstanding_amount: any;
  cm_total_so: any;
  cm_total_do: any;
  cm_total_invoice: any;
  cm_total_payment: any;
  cm_invoice_amount: any;
  cm_payment_amount: any;
  cm_outstanding_amount: any;
  lm_total_so: any;
  lm_total_do: any;
  lm_total_invoice: any;
  lm_total_payment: any;
  lm_invoice_amount: any;
  lm_payment_amount: any;
  lm_outstanding_amount: any;
  cy_total_so: any;
  cy_total_do: any;
  cy_total_invoice: any;
  cy_total_payment: any;
  cy_invoice_amount: any;
  cy_payment_amount: any;
  cy_outstanding_amount: any;
  ly_total_so: any;
  ly_total_do: any;
  ly_total_invoice: any;
  ly_total_payment: any;
  ly_invoice_amount: any;
  ly_payment_amount: any;
  ly_outstanding_amount: any;
  mtd_over_due_payment: any;
  mtd_over_due_payment_amount: any;
  mtd_over_due_invoice_amount: any;
  mtd_over_due_invoice: any;
  ytd_over_due_payment: any;
  ytd_over_due_payment_amount: any;
  ytd_over_due_invoice_amount: any;
  ytd_over_due_invoice: any;
  mtd_payment: any;
  ytd_payment: any;
  countOwnValues: any;

  paymentDay: string[] = [];
  amount: number[] = [];
  monthlySalesData: any;
  invctotalcount: any;
  grntotalcount: any;
  count_total: any;
  vendor_count: any;
  cancel_invoice: any;
  pending_count: any;
  count_product: any;
  payablesummary_list: any;
  approved_invoice: any;
  cancelled_invoice: any;
  pending_invoice: any;
  completed_invoice: any;
  cancelled_payment: any;
  approved_payment: any;
  completed_payment: any;
  minsoft:any;
  month_invoiceamount: any;
  ytd_invoiceamount: any;
  mtd_invoice: any;
  ytd_invoice: any;
  total_vendor: any;
  pototalcount: any;
  month_invoicecount: any;
  ytd_invoicecount: any;
  currency_symbol: any;
  Getpmrdashboard_list:any[]=[];
  overallchartflag:boolean=false;
  invoicechartflag:boolean=false;
  paymentchartflag:boolean=false;
  purchasechartflag:boolean=false;
  menu!: any[];
  submenu!: any[];
  selectedIndex: number = 0;
  menu_name!: any;
  menu_index!: number;
  invoicechart:any={};
  paymentchart:any={};
  purchasechartcount:any={};
  Getinvoicechart_list:any[]=[];
  Getpaymentchart_list:any[]=[];
  Getpurchaseorderchart_list:any[]=[];
  po_approved:any;
  po_pending:any;
  po_cancelled:any;
  po_completed:any;
  constructor(private router: Router, private service: SocketService) {
this.waitForToken().subscribe(()=>{
  this.runBackgroundApiCall2();
  this.runBackgroundApiCall4();
  this.runBackgroundApiCall1();
})


  }

  async runBackgroundApiCall2() {
    var api2 = 'Mintsoft/MintsoftCourierDetails';
    try {
      //await this.service.get(api2).toPromise();
      const result = await this.service.get(api2).toPromise();

    } catch (error) {
    }
  }
  async runBackgroundApiCall4() {
    var api3 = 'Mintsoft/MintsoftAsnstatusgoodssupplier';
    try {
      // const result = await this.service.get(api3).toPromise();\
      const result = await this.service.get(api3).toPromise();

    } catch (error) {
    }
  }
  async runBackgroundApiCall1(){
    try{
    var url = 'SmrMstProduct/MintsoftProductDetailsAsync';
    const result  = await this.service.get(url).toPromise();
    }
    catch(error){

    }
  }

  ngOnInit() {
    // var url = 'SmrMstProduct/MintsoftProductDetailsAsync'
    // this.service.get(url).subscribe((result: any) => {
    //   this.minsoft = result;
    // });

    this.runBackgroundApiCall1();
    this.runBackgroundApiCall2();
    this.runBackgroundApiCall4();
    this.GetTilesdetails();
    this.getpaymentchart();
    this.getinvoicechart();
    this.Getpurchaseorderchart();

 
    this.getMonthlySalesChart();
 
    let user_gid = localStorage.getItem('user_gid');
    let param = {
      user_gid: user_gid
    }
    var url = 'User/Dashboardprivilegelevel';
    this.service.getparams(url, param).subscribe((result: any) => {
      for (let i = 0; i < result.menu_list.length; i++) {
        if (result.menu_list[i].text == 'Purchase') {
          this.menu_index = i;
        }
      }
      this.menu = result.menu_list[this.menu_index].submenu;
      this.submenu = this.menu[0].sub1menu;
    });
  }
  waitForToken(): Observable<boolean> {
    return interval(2000) // interval every 2 seconds  
      .pipe(
        takeUntil(this.destroy$), // Cleanup when the component is destroyed
        map(() => {
          const token = localStorage.getItem('token');
          return token !== null && token !== '';
        }),
        takeWhile((tokenAvailable) => !tokenAvailable, true),
        catchError((error) => {
          console.error('Error while polling for token:', error);
          return [];
        })  
      );
  }
  
  
  GetTilesdetails(){
    
    var url = 'PmrDashboard/GetPurchaseCount';
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.Getpmrdashboard_list= this.responsedata.Getpmrdashboard_list;
      this.total_vendor = this.Getpmrdashboard_list[0].total_vendor;
      this.pototalcount = this.Getpmrdashboard_list[0].pototalcount;
      this.month_invoiceamount = this.Getpmrdashboard_list[0].month_invoiceamount;
      this.mtd_invoice = this.Getpmrdashboard_list[0].mtd_invoice;
      this.month_invoicecount = this.Getpmrdashboard_list[0].month_invoicecount;
      this.ytd_invoiceamount = this.Getpmrdashboard_list[0].ytd_invoiceamount;
      this.ytd_invoicecount = this.Getpmrdashboard_list[0].ytd_invoicecount;
      this.ytd_invoice = this.Getpmrdashboard_list[0].ytd_invoice;
      this.currency_symbol = this.Getpmrdashboard_list[0].currency_symbol;
    });
    console.log('iewk', this.Getpmrdashboard_list)
  }
  
  change_menu_tab(n: number): void {
    this.submenu = this.menu[n].sub1menu;
    this.selectedIndex = n;
  }
  redirectToVendorPage(): void {
    this.router.navigate(['/pmr/PmrMstVendorregister']);
}
redirectToPOPage(): void {
  this.router.navigate(['/pmr/PmrTrnPurchaseorderSummary']);
}
redirectToInvoicePage(): void {
  this.router.navigate(['/payable/PmrTrnInvoice']);
}

  getMonthlySalesChart() {

    var url = 'PmrDashboard/GetPurchaseLiabilityReportChart'
    this.service.get(url).subscribe((result: any) => {

      this.response_data = result;

      this.GetOverallSalesOrderChart_List = this.response_data.Getpurchasechart_list;

      const categories = this.GetOverallSalesOrderChart_List.map((entry: { Months: any; }) => entry.Months);
      const totalAmountData = this.GetOverallSalesOrderChart_List.map((entry: { purchase_count: any }) => entry.purchase_count);
      const invoiceAmountData = this.GetOverallSalesOrderChart_List.map((entry: { invoice_count: any }) => entry.invoice_count);
      const paymentAmountData = this.GetOverallSalesOrderChart_List.map((entry: { payment_count: any }) => entry.payment_count);

      if (this.GetOverallSalesOrderChart_List.length === 0) {
        // If no records are returned, set series data arrays to contain zero values
        this.overallchartflag = true;
      }
      this.chartOptions = {
        chart: {
          type: 'bar',
          height: 300,
          width: '100%',
          background: 'White',
          foreColor: '#0F0F0F',
          toolbar: {
            show: false, // Set to false to hide the toolbar/menu icon
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
            name: 'Purchase Order',
            data: totalAmountData,
          },
          {
            name: 'Invoice',
            data: invoiceAmountData,
          },
          {
            name: 'Payment',
            data: paymentAmountData,
          },
        ],
      };

    })
  };

  getinvoicechart(){
    var url = 'PmrDashboard/GetInvoiceCount';
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.Getinvoicechart_list = (this.responsedata.Getinvoicechart_list);
      //console.log('wefewve', this.Getinvoicechart_list)
      if ( this.Getinvoicechart_list.length === 0) {
        this.invoicechartflag = true;
      }
      this.approved_invoice =Number( this.Getinvoicechart_list[0].approved_invoice );
      this.cancelled_invoice = Number(this.Getinvoicechart_list[0].cancel_invoice);
      this.pending_invoice = Number(this.Getinvoicechart_list[0].pending_invoice);
      this.completed_invoice = Number(this.Getinvoicechart_list[0].completed_invoice);
      this.series_Value = [this.approved_invoice, this.completed_invoice , this.pending_invoice,this.cancelled_invoice  ];
      this.labels_value = ['Invoice Approved','Invoice Completed', 'Invoice Pending','Invoice Cancelled'];
      this.invoicechart = {
        series: this.series_Value,
        labels: this.labels_value,
        chart: {
          width: 430,
          type: "donut"
        },   
        colors: ['#7FC7D9', '#66BB6A','#FFD54F', '#DC143C'],   
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
    });
  }


  getpaymentchart(){
    var url = 'PmrDashboard/GetPaymentCount';
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.Getpaymentchart_list= this.responsedata.Getpaymentchart_list;
      if ( this.Getpaymentchart_list.length === 0) {
        this.paymentchartflag = true;
      }
      this.cancelled_payment =  Number ( this.Getpaymentchart_list[0].cancelled_payment);
      this.approved_payment =  Number(this.Getpaymentchart_list[0].approved_payment);
      this.completed_payment =  Number(this.Getpaymentchart_list[0].completed_payment);
      this.series_Value = [ this.approved_payment,this.completed_payment ,this.cancelled_payment  ];
      this.labels_value = ['Payment Approved','Payment Completed','Payment Cancelled'];
      this.paymentchart = {
        series: this.series_Value,
        labels: this.labels_value,
        chart: {
          width: 430,
          type: "pie"
        },
        colors: ['#E2D686','#26C485','#DC143C'], // Update colors as needed
        fill: {
          type: "solid"
        },
      };

    });
  }


  Getpurchaseorderchart() {

    var url = 'PmrDashboard/GetPurchasetCount'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.Getpurchaseorderchart_list = this.responsedata.Getpurchaseorderchart_list;
      if (this.Getpurchaseorderchart_list.length== 0) {
        this.purchasechartflag = true;
      }
      this.po_approved = Number(this.Getpurchaseorderchart_list[0].po_approved);
      this.po_pending = Number(this.Getpurchaseorderchart_list[0].po_pending);
      this.po_completed = Number(this.Getpurchaseorderchart_list[0].po_completed);
      this.po_cancelled = Number(this.Getpurchaseorderchart_list[0].po_cancelled);

      this.series_Value = [this.po_approved,  this.po_completed,this.po_pending, this.po_cancelled];
      this.labels_value = ['PO Approved', 'PO Completed','PO Pending','PO Cancelled'];


      this.purchasechartcount = {

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
    })
  }
  redirect_menu(data: string, j: number, k: number): void {
    if (data != null && data != "" && data != "#") {
      this.router.navigate([data]);
    }
  }
}





