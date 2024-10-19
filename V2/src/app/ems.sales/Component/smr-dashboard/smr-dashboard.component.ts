
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ApexOptions } from 'apexcharts';
import { Observable, interval, Subject, ReplaySubject } from 'rxjs';
import { takeWhile, map, takeUntil, catchError } from 'rxjs/operators';
import {
  ApexNonAxisChartSeries,
  ApexResponsive,
  ApexChart
} from "ng-apexcharts";
import { RskMstDashboardComponent } from 'src/app/ems.rsk/component/rsk-mst-dashboard/rsk-mst-dashboard.component';


@Component({
  selector: 'app-smr-dashboard',
  templateUrl: './smr-dashboard.component.html',
  styleUrls: ['./smr-dashboard.component.scss']
})

export class SmrDashboardComponent {
  private destroy$ = new Subject<void>();
  companycode: string | any;
  current_domain: string | any;
  //////////////////////////
  response_data: any;
  Getoverallsalesbarchart_list: any = [];
  Getoverallsalesbarchart_flag: boolean = false;
  overall_enquiry: any;
  overall_quotation: any;
  overall_invoice: any;
  overall_salesorder: any;
  series_Value: any;
  labels_value: any;
  chartOptionsoverall: any = {};
  /////////////////////////
  responsedata: any;
  GetPaymentandDeliveryChart_list: any = [];
  saleschartflag: boolean = false;
  saleschartflag1: boolean = false;
  saleschartflag2: boolean = false;
  saleschartflag3: boolean = false;
  paymentdelivery_month: any;
  payment_count: any;
  delivery_count: any;
  Paymentchart: any = {};
  ///////////////////////////////
  GetTilesDetails_list: any = [];
  customer_count: any;
  mtd_invoice: any;
  month_invoiceamount: any;
  month_invoiccount: any;
  ytd_invoice: any;
  ytd_invoiceamount: any;
  ytd_invoicecount: any;
  minsoft:any;
  currency_symbol: any;
  delivery_list: any = [];
  delivery_month: any;
  delivery_pending: any;
  delivery_completed: any;
  Deliverychart: any = {};
  //////////////////////////////
  selectedChartType1: any;
  ////////
  invoicechartcountlist: any;
  approved_invoice: any;
  pending_invoice: any;
  totalinvoice: any;
  invoice_count: any;
  Invoicechartcount: any = {};
  ////////////////////
  GetSalesStatus_list: any;
  flag2: boolean = false;
  salescustomer_count: any;
  enquiry_count: any;
  quotation_count: any;
  order_count: any;
  sales_months: any;
  saleschart: any = {};
  /////////////
  menu_index: any;
  menu: any;
  submenu: any;
  ///////////////
  quotationcountlist: any = [];
  quotation_approved: any;
  quotation_amended: any;
  quotationchartcount: any = {};
  //////////////////
  enquirychartcountlist: any = [];
  enquirychartcount: any = {};
  ////////////////////
  saleschartcountlist: any = [];
  so_amended: any;
  total_so: any;
  approved_so: any;
  pending_So: any;
  saleschartcount: any = {};
  /////////////////////////////////////bobatea//////////
  bobateaSalesordersixmonthchart_list: any = [];
  bobateasalesordersixmonth_flag: boolean = false;
  salesorder_datesixmonth: any;
  salesordersixmonth: any;
  whatsappordersixmonth: any;
  shopifyordersixmonth: any;
  bobateasalessixmonthchart: any = {};


  constructor(private router: Router, private service: SocketService) {
// this.waitForToken().subscribe(()=>{
//   this.runBackgroundApiCall1();
//   this.runBackgroundApiCall2();
// })

  }
  
  async runBackgroundApiCall1(){
    try{
    var url = 'SmrMstProduct/MintsoftProductDetailsAsync';
    const result  = await this.service.get(url).toPromise();
    }
    catch(error){

    }
  }
  async runBackgroundApiCall2(){
    try{
    var url = 'Mintsoft/Mintsoftgetsalesorders';
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

    // var url  = 'Mintsoft/Mintsoftgetsalesorders';
    // this.service.get(url).subscribe((result:any)=>{
    //   this.minsoft = result;
    // })

    /////commmon/////
    this.GetTilesDetails();
    this.GetSalesStatus();
    this.Getoverallsalesbarchart();
    this.GetPaymentDeliveryChart();
    this.Menudata();
    this.runBackgroundApiCall1();
     this.runBackgroundApiCall2();
    /////
    this.current_domain = window.location.hostname;
    if (this.current_domain == 'crm.bobateacompany.co.uk') {
      //for bobatea
      this.GetSalesordersixmonthchart();
      ////
      this.companycode = 'boba_tea'
    }
    else if (this.current_domain == 'bobatea.storyboardsystems.com') {
      //for bobatea
      this.GetSalesordersixmonthchart();
      ////
      this.companycode = 'boba_tea'
    }
    else {
      this.companycode = 'default';
      this.Getquotationchartcount();
    }
    //this.GetDeliverychart();


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
  ///////common function for all users//////////////

  GetTilesDetails() {

    var url = 'SmrDashboard/GetTilesDetails';
    this.service.get(url).subscribe((result: any) => {
      this.GetTilesDetails_list = result.GetTilesDetails_list;
      this.customer_count = this.GetTilesDetails_list[0].customer_count,
        this.mtd_invoice = this.GetTilesDetails_list[0].mtd_invoice,
        this.month_invoiceamount = this.GetTilesDetails_list[0].month_invoiceamount,
        this.month_invoiccount = this.GetTilesDetails_list[0].month_invoiccount,
        this.ytd_invoice = this.GetTilesDetails_list[0].ytd_invoice,
        this.ytd_invoiceamount = this.GetTilesDetails_list[0].ytd_invoiceamount,
        this.ytd_invoicecount = this.GetTilesDetails_list[0].ytd_invoicecount,
        this.currency_symbol = this.GetTilesDetails_list[0].currency_symbol
    });
  }

  GetSalesStatus() {
    var url = 'SmrDashboard/GetSalesStatus';
    this.service.get(url).subscribe((result: any) => {
      this.response_data = result;
      this.GetSalesStatus_list = this.response_data.GetSalesStatus_list;
      ////console.log('wedew',   this.GetSalesStatus_list)
      if (this.GetSalesStatus_list.length > 0) {
        this.flag2 = true;
      }
      this.salescustomer_count = this.GetSalesStatus_list.map((entry: { customer_count: any }) => entry.customer_count)
      this.enquiry_count = this.GetSalesStatus_list.map((entry: { enquiry_count: any }) => entry.enquiry_count)
      this.quotation_count = this.GetSalesStatus_list.map((entry: { quotation_count: any }) => entry.quotation_count)
      this.order_count = this.GetSalesStatus_list.map((entry: { order_count: any }) => entry.order_count)
      this.invoice_count = this.GetSalesStatus_list.map((entry: { invoice_count: any }) => entry.invoice_count)
      this.sales_months = this.GetSalesStatus_list.map((entry: { Months: any }) => entry.Months)
      ////console.log('ew1',  this.salescustomer_count)
      this.saleschart = {
        chart: {
          type: 'bar',
          height: 300,
          width: '100%',
          background: 'White',
          foreColor: '#0F0F0F',
          fontFamily: 'inherit',
          toolbar: {
            show: true,
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
          categories: this.sales_months,
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
            name: 'Customer',
            data: this.salescustomer_count,
            color: '#3D9DD9',
          },
          {
            name: 'Enquiry',
            data: this.enquiry_count,
            color: '#9EBF95',
          },
          {
            name: 'Quotation',
            data: this.quotation_count,
            color: '#8C8C8C',
          },
          {
            name: 'Order',
            data: this.order_count,
            color: '#F2D377',
          },
          {
            name: 'Invoice',
            data: this.invoice_count,
            color: '#48a363',
          },

        ],
        legend: {
          position: "top",
          offsetY: 5
        }
      };
    });
  }

  Getoverallsalesbarchart() {
    debugger
    var url = 'SmrDashboard/Getoverallsalesbarchart';
    this.service.get(url).subscribe((result: any) => {
      this.Getoverallsalesbarchart_list = result.Getoverallsalesbarchart_list;
      ////console.log('dwpoqw', this.Getoverallsalesbarchart_list[0].overall_enquiry)
      if (this.Getoverallsalesbarchart_list.length > 0 || this.Getoverallsalesbarchart_list != null) {
        this.saleschartflag = true;
      }
      this.overall_enquiry = Number(this.Getoverallsalesbarchart_list[0].overall_enquiry);
      this.overall_quotation = Number(this.Getoverallsalesbarchart_list[0].overall_quotation);
      this.overall_salesorder = Number(this.Getoverallsalesbarchart_list[0].overall_salesorder);
      this.overall_invoice = Number(this.Getoverallsalesbarchart_list[0].overall_invoice);

      this.series_Value = [this.overall_enquiry, this.overall_quotation, this.overall_salesorder, this.overall_invoice];
      this.labels_value = ['Enquiry', 'Quotation', 'Order', 'Invoice'];

      this.chartOptionsoverall = {
        series: this.series_Value,
        labels: this.labels_value,
        chart: {
          width: 360,
          type: "pie",
          toolbar: {
            show: true,
            tools: {
              download: true,
              selection: false,
              zoom: false,
              zoomin: false,
              zoomout: false,
              pan: false,
              reset: false
            },
          },
        },
        colors: ['#9EBF95', '#85929e', '#F2D377', '#48a363'],
        responsive: [
          {
            options: {
              chart: {
                width: 200
              },
            }
          }
        ],
        legend: {
          position: "top"
        },
        fill: {
          type: "solid"
        }
      }
    });
  }

  GetPaymentDeliveryChart() {
    const url = 'SmrDashboard/GetPaymentandDeliveryChart';
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.GetPaymentandDeliveryChart_list = this.responsedata.GetPaymentandDeliveryChart_list;
      if (this.GetPaymentandDeliveryChart_list.length > 0) {
        this.saleschartflag = true;
      }
      this.paymentdelivery_month = this.GetPaymentandDeliveryChart_list
        .map((entry: { pd_month: any }) => entry.pd_month || 0);
      this.payment_count = this.GetPaymentandDeliveryChart_list
        .map((entry: { p_count: number }) => entry.p_count);
      this.delivery_count = this.GetPaymentandDeliveryChart_list
        .map((entry: { d_count: number }) => entry.d_count);
      this.Paymentchart = {
        series: [
          {
            name: "Payment",
            data: this.payment_count
          },
          {
            name: "Delivery",
            data: this.delivery_count
          }
        ],
        chart: {
          height: 290,
          type: "area",
          toolbar: {
            show: true,
            tools: {
              download: true,
              selection: false,
              zoom: false,
              zoomin: false,
              zoomout: false,
              pan: false,
              reset: false
            },
          },
        },
        dataLabels: {
          enabled: false
        },
        stroke: {
          curve: "smooth"
        },
        xaxis: {
          categories: this.paymentdelivery_month
        },
        legend: {
          position: "top",
        }

      };
    });
  }
  GetDeliverychart() {
    const url = 'SmrDashboard/GetDeliveryChart';
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.delivery_list = this.responsedata.GetPaymentandDeliveryChart_list;
      //console.log('ocmewo', this.delivery_list)
      if (this.delivery_list.length > 0) {
        this.saleschartflag = true;
      }
      this.delivery_month = this.delivery_list
        .map((entry: { delivery_month: any }) => entry.delivery_month);
      this.delivery_pending = this.delivery_list
        .map((entry: { delivery_pending: any }) => entry.delivery_pending || 0);
      this.delivery_completed = this.delivery_list
        .map((entry: { delivery_completed: any }) => entry.delivery_completed || 0);
      //console.log('jnkjk', this.delivery_month, this.delivery_pending, this.delivery_completed)
      this.Deliverychart = {
        series: [
          {
            name: "Pending",
            data: this.delivery_pending
          },
          {
            name: "Completed",
            data: this.delivery_completed
          },
        ],
        chart: {
          type: "bar",
          height: 250,
          stacked: true
        },
        plotOptions: {
          bar: {
            horizontal: true
          }
        },
        stroke: {
          width: 1,
          colors: ["#fff"]
        },
        title: {
          text: "Fiction Books Sales"
        },
        xaxis: {
          categories: this.delivery_month,
          labels: {
            formatter: function (val: any) {
              return val;
            }
          }
        },
        yaxis: {
          title: {
            text: undefined
          }
        },
        tooltip: {
          y: {
            formatter: function (val: any) {
              return val;
            }
          }
        },
        fill: {
          opacity: 1
        },
        legend: {
          position: "top",
          horizontalAlign: "left",
          offsetX: 40
        }
      };
    });
  }
  updateChartDefault(chartType: string) {
    this.selectedChartType1 = chartType;

    switch (chartType) {
      case 'Invoice':
        this.GetInvoicecount();
        break;
      case 'Quotation':
        this.Getquotationchartcount();
        break;
      case 'Order':
        this.Getsaleschartcountt();
        break;
      default:
        this.Getquotationchartcount();
        break;
    }
  }
  updateChart(chartType: string) {
    this.selectedChartType1 = chartType;

    switch (chartType) {
      case 'ALL':
        this.Getoverallsalesbarchart();
        break;
      case 'Invoice':
        this.GetInvoicecount();
        break;
      case 'Quotation':
        this.Getquotationchartcount();
        break;
      case 'Order':
        this.Getsaleschartcountt();
        break;
      default:
        this.Getoverallsalesbarchart();
        break;
    }
  }

  GetInvoicecount() {

    var url = 'SmrDashboard/GetSalesOrderCount'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.invoicechartcountlist = this.responsedata.GetSalesOrderCount_List;
      this.approved_invoice = Number(this.invoicechartcountlist[0].approved_invoice)
      this.pending_invoice = Number(this.invoicechartcountlist[0].pending_invoice)
      //this.totalinvoice = Number(this.invoicechartcountlist[0].totalinvoice)
      this.invoice_count = Number(this.invoicechartcountlist[0].invoice_count)
      ////console.log('oimmmlk', this.pending_invoice)
      if (this.pending_invoice != 0 || this.approved_invoice != 0) {
        this.saleschartflag3 = true;
      }
      this.series_Value = [this.pending_invoice, this.approved_invoice];
      ////console.log('owimdlkwqdmoiwqlkdqwnd', this.series_Value)
      this.labels_value = ['Pending Invoice', 'Approved Invoice'];
      this.Invoicechartcount = {
        series: this.series_Value,
        labels: this.labels_value,
        chart: {
          width: 430,
          type: "donut",
          toolbar: {
            show: true,
            tools: {
              download: true,
              selection: false,
              zoom: false,
              zoomin: false,
              zoomout: false,
              pan: false,
              reset: false
            }
          }
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
        }
      };


    })
  }
  Getquotationchartcount() {

    var url = 'SmrDashboard/GetSalesOrderCount'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.quotationcountlist = this.responsedata.GetSalesOrderCount_List;
   
//console.log(';lm,.', this.quotationcountlist )
      //this.total_quotation = Number(this.quotationcountlist[0].total_quotation);
      this.quotation_approved = Number(this.quotationcountlist[0].quotation_completed);
      this.quotation_amended = Number(this.quotationcountlist[0].quotation_canceled);
      if (this.quotation_approved!= 0||this.quotation_amended!= 0) {
        this.saleschartflag1 = true;
      }
      this.series_Value = [this.quotation_amended, this.quotation_approved];
      this.labels_value = ['Amended Quotation', 'Quotation Approved'];

//console.log('gfhf', this.series_Value)
      this.quotationchartcount = {

        series: this.series_Value,
        labels: this.labels_value,
        chart: {
          width: 430,
          type: "donut", toolbar: {
            show: true,
            tools: {
              download: true,
              selection: false,
              zoom: false,
              zoomin: false,
              zoomout: false,
              pan: false,
              reset: false
            },
          },
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
  Getenquirychartcount() {

    var url = 'SmrTrnSalesManager/enquirychartcount'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.enquirychartcountlist = this.responsedata.chartscounts_list1;
      const data = this.enquirychartcountlist.map((entry: { enquirymonthcount: any; }) => entry.enquirymonthcount);
      const categories = this.enquirychartcountlist.map((entry: { enquirymonth: any; }) => entry.enquirymonth);


      this.enquirychartcount = {
        chart: {
          type: 'bar',
          height: 360,
          background: 'White',
          foreColor: '#0F0F0F',
          toolbar: {
            show: false,
          },
        },
        colors: ['#8062D6', '#FFD54F', '#66BB6A', '#EF5350', '#0F0F0F'],
        plotOptions: {
          bar: {
            horizontal: false,
            columnWidth: '1%',
            borderRadius: 0,
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
            text: 'Weeks',
            style: {
              fontWeight: 'bold',
              fontSize: '14px',
              color: '#0F0F0F',
            },
          },
        },
        series: [
          {
            name: 'Enquire',
            data: data,
            color: '#5d1d82',
          },
        ],
      };


    })
  }
  Getsaleschartcountt() {

    var url = 'SmrDashboard/GetSalesOrderCount'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.saleschartcountlist = this.responsedata.GetSalesOrderCount_List;

      this.so_amended = Number(this.saleschartcountlist[0].so_amended)
      //this.total_so = Number(this.saleschartcountlist[0].total_so)
      this.approved_so = Number(this.saleschartcountlist[0].approved_so)
      this.pending_So = Number(this.saleschartcountlist[0].pending_So)
      if (this.so_amended != 0 || this.approved_so != 0 || this.pending_So != 0) {
        this.saleschartflag2 = true;
      }

      this.series_Value = [this.so_amended, this.approved_so, this.pending_So];
      this.labels_value = ['Order Amended', 'Order Approved', 'Order Pending'];


      this.saleschartcount = {
        series: this.series_Value,
        labels: this.labels_value,
        chart: {
          width: 430,
          type: "donut", toolbar: {
            show: true,
            tools: {
              download: true,
              selection: false,
              zoom: false,
              zoomin: false,
              zoomout: false,
              pan: false,
              reset: false
            },
          },
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
        }
      };


    })
  }



  ////////////////////////////for bobatea /////////////////////


  GetSalesordersixmonthchart() {
    var url = 'SmrDashboard/GetSalesordersixmonthchart';
    this.service.get(url).subscribe((result: any) => {
      this.response_data = result;
      this.bobateaSalesordersixmonthchart_list = this.response_data.Salesordersixmonthchart_list;
      if (this.bobateaSalesordersixmonthchart_list.length > 0 || this.bobateaSalesordersixmonthchart_list.length != null) {
        this.bobateasalesordersixmonth_flag = true;
      }
      ////console.log('grfhj', this.bobateaSalesordersixmonthchart_list )
      this.salesorder_datesixmonth = this.bobateaSalesordersixmonthchart_list.map((entry: { salesorder_datesixmonth: any }) => entry.salesorder_datesixmonth)
      this.salesordersixmonth = this.bobateaSalesordersixmonthchart_list.map((entry: { salesordersixmonth: any }) => entry.salesordersixmonth)
      this.whatsappordersixmonth = this.bobateaSalesordersixmonthchart_list.map((entry: { whatsappordersixmonth: any }) => entry.whatsappordersixmonth)
      this.shopifyordersixmonth = this.bobateaSalesordersixmonthchart_list.map((entry: { shopifyordersixmonth: any }) => entry.shopifyordersixmonth)

      this.bobateasalessixmonthchart = {
        chart: {
          type: 'line',
          height: 300,
          width: '100%',
          background: 'White',
          foreColor: '#0F0F0F',
          fontFamily: 'inherit',
          toolbar: {
            show: true,
            tools: {
              download: true,
              selection: false,
              zoom: false,
              zoomin: false,
              zoomout: false,
              pan: false,
              reset: false
            },
          },
        },
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
          categories: this.salesorder_datesixmonth,
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
            name: 'Sales Order',
            data: this.salesordersixmonth,
            color: '#CE7F92',
          },
          {
            name: 'Shopify Order',
            data: this.shopifyordersixmonth,
            color: '#747C8C',
          },
          {
            name: 'WhatsApp Order',
            data: this.whatsappordersixmonth,
            color: '#667967',
          },
        ],

        legend: {
          position: "top",
          offsetY: 5
        }
      };
    });
  }



  ////////////////////////////Menu functions starts////////////////////////////
  Menudata() {

    let user_gid = localStorage.getItem('user_gid');
    let param = {
      user_gid: user_gid
    }
    var url = 'User/Dashboardprivilegelevel';
    this.service.getparams(url, param).subscribe((result: any) => {
      ////console.log('sales', result.menu_list)
      for (let i = 0; i < result.menu_list.length; i++) {
        if (result.menu_list[i].text == 'Sales') {
          this.menu_index = i;
        }
      }
      this.menu = result.menu_list[this.menu_index].submenu;

      this.submenu = this.menu[0].sub1menu;
    });

  }
  redirect_menu(data: string, j: number, k: number): void {

    if (data != null && data != "" && data != "#") {
      this.router.navigate([data]);

    }
  }



}

