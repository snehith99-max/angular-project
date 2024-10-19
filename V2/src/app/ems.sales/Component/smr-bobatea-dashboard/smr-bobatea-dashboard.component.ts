import { Component, OnInit, ViewChild } from "@angular/core";
import { ChartComponent } from "ng-apexcharts";
import {
  ApexNonAxisChartSeries,
  ApexResponsive,
  ApexChart
} from "ng-apexcharts";
import { SocketService } from "src/app/ems.utilities/services/socket.service";
@Component({
  selector: 'app-smr-bobatea-dashboard',
  templateUrl: './smr-bobatea-dashboard.component.html',
  styleUrls: ['./smr-bobatea-dashboard.component.scss']
})
export class SmrBobateaDashboardComponent {
  shopifycount_lits: any;
  shopifymonth_date: any;
  shopifymonth_count: any;
  shopifymonthchart: any = {};
  systemmonth_date: any;
  systemmonth_count: any;
  systemmonthchart: any = {};
  whatsappmonth_date: any;
  whatsappmonth_count: any;
  whatsappmonthchart: any = {};
  shopifymonth_amount: any;
  systemmonth_amount: any;
  whatsappmonth_amount: any;
  shopifymonth_flag: boolean = false;
  systemmonth_flag: boolean = false;
  whatsappmonth_flag: boolean = true;
  shopifymonth_first_amount: number = 0;
  systemmonth_first_amount: number = 0;
  whatsappmonth_first_amount: number = 0;
  salescustomer_count: any;
  response_data: any;
  GetSalesStatus_list: any;
  flag2: any;
  enquiry_count: any;
  quotation_count: any;
  order_count: any;
  sales_months: any;
  saleschart: any;
  invoicechartcountlist: any;
  shopify_count: any;
  sale_count: any;
  totalinvoice: any;
  invoice_count: any;
  series_Value: any;
  labels_value: any;
  saleschartflag: boolean = false;
  Invoicechartcount: any = {};
  Salesordersixmonthchart_list: any;
  salesordersixmonth_flag: boolean = false;
  salesorder_datesixmonth: any;
  whatsappordersixmonth: any;
  shopifyorder_month: any;
  shopifyordersixmonth: any;
  salesordersixmonth: any;
  salessixmonthchart: any = {};





  constructor(public service: SocketService) {
  }
  ngOnInit(): void {
    this.GetSalesordershopify();

  }

  GetSalesordershopify() {
    var api7 = 'SmrBobateaDashoard/GetSalesorder';
    this.service.get(api7).subscribe((result: any) => {
      this.shopifycount_lits = result.shopifyorder_month;
      console.log('oiemwo',this.shopifycount_lits)
      this.shopifymonth_date = this.shopifycount_lits
        .filter((entry: { source_flag: string }) => entry.source_flag === 'S')
        .map((entry: { formatted_date: any }) => entry.formatted_date || 0);
      this.shopifymonth_count = this.shopifycount_lits
        .filter((entry: { source_flag: string }) => entry.source_flag === 'S')
        .map((entry: { order_count: number }) => entry.order_count || 0);
      this.shopifymonth_amount = this.shopifycount_lits
        .filter((entry: { source_flag: string }) => entry.source_flag === 'S')
        .map((entry: { monthlyamount: number }) => entry.monthlyamount || 0);
      if (this.shopifymonth_date != 0 || this.shopifymonth_count != 0) {
        this.shopifymonth_flag = true;
        this.shopifymonth_first_amount = this.shopifymonth_amount[0];
      }
      this.systemmonth_date = this.shopifycount_lits
        .filter((entry: { source_flag: string }) => entry.source_flag === 'I')
        .map((entry: { formatted_date: any }) => entry.formatted_date || 0);
      this.systemmonth_count = this.shopifycount_lits
        .filter((entry: { source_flag: string }) => entry.source_flag === 'I')
        .map((entry: { order_count: number }) => entry.order_count || 0);
      this.systemmonth_amount = this.shopifycount_lits
        .filter((entry: { source_flag: string }) => entry.source_flag === 'I')
        .map((entry: { monthlyamount: number }) => entry.monthlyamount || 0);
      if (this.systemmonth_date != 0 || this.systemmonth_count != 0) {
        this.systemmonth_flag = true;
        this.systemmonth_first_amount = this.systemmonth_amount[0];
      }
      this.whatsappmonth_date = this.shopifycount_lits
        .filter((entry: { source_flag: string }) => entry.source_flag === 'W')
        .map((entry: { formatted_date: any }) => entry.formatted_date || 0);
      this.whatsappmonth_count = this.shopifycount_lits
        .filter((entry: { source_flag: string }) => entry.source_flag === 'W')
        .map((entry: { order_count: number }) => entry.order_count || 0);
      this.whatsappmonth_amount = this.shopifycount_lits
        .filter((entry: { source_flag: string }) => entry.source_flag === 'W')
        .map((entry: { monthlyamount: number }) => entry.monthlyamount || 0);
      if (this.whatsappmonth_date != 0 || this.whatsappmonth_count != 0) {
        this.whatsappmonth_flag = true;
        this.whatsappmonth_first_amount = this.whatsappmonth_amount[0];
      }

      this.shopifymonthchart = {
        chart: {
          type: 'area',
          height: 100,
          background: 'White',
          foreColor: '#0F0F0F',
          fontFamily: 'inherit',
          toolbar: {
            show: false,
          },
          sparkline: {
            enabled: true
          },
        },
        colors: ['#96bf48'],
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
          type: 'datetime',
        },
        yaxis: {
          min: 0,
        },
        series: [{
          name: 'Shopify Orders',
          data: randomizeArray1(this.shopifymonth_count),
        }],
        labels: Array.from(this.shopifymonth_date),
      };
      this.systemmonthchart = {
        chart: {
          type: 'area',
          height: 100,
          background: 'White',
          foreColor: '#0F0F0F',
          fontFamily: 'inherit',
          toolbar: {
            show: false,
          },
          sparkline: {
            enabled: true
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
          type: 'datetime',
        },
        yaxis: {
          min: 0,
        },
        series: [{
          name: 'Sales Orders',
          data: randomizeArray2(this.systemmonth_count),
        }],
        labels: Array.from(this.systemmonth_date),
      };
  
      this.whatsappmonthchart = {
        chart: {
          type: 'area',
          height: 100,
          background: 'White',
          foreColor: '#0F0F0F',
          fontFamily: 'inherit',
          toolbar: {
            show: false,
          },
          sparkline: {
            enabled: true
          },
        },
        colors: ['#25D366'],
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
          type: 'datetime',
        },
        yaxis: {
          min: 0,
        },
        series: [{
          name: 'WhatsApp Orders',
          data: randomizeArray3(this.whatsappmonth_count),
        }],
        labels: Array.from(this.whatsappmonth_date),
      };

    });
    this.GetSalesStatus();
  }
  GetSalesStatus() {
    var url = 'SmrDashboard/GetSalesStatus';
    this.service.get(url).subscribe((result: any) => {
      this.response_data = result;
      this.GetSalesStatus_list = this.response_data.GetSalesStatus_list;
      //console.log('wedew',   this.GetSalesStatus_list)
      if (this.GetSalesStatus_list.length > 0) {
        this.flag2 = true;
      }
      this.salescustomer_count = this.GetSalesStatus_list.map((entry: { customer_count: any }) => entry.customer_count)
      //this.enquiry_count = this.GetSalesStatus_list.map((entry: { enquiry_count: any }) => entry.enquiry_count)
      //this.quotation_count = this.GetSalesStatus_list.map((entry: { quotation_count: any }) => entry.quotation_count)
      this.order_count = this.GetSalesStatus_list.map((entry: { order_count: any }) => entry.order_count)
      this.sales_months = this.GetSalesStatus_list.map((entry: { Months: any }) => entry.Months)
      //console.log('ew1',  this.salescustomer_count)
      this.saleschart = {
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
          // {
          //   name: 'Enquiry',
          //   data: this.enquiry_count,
          //   color: '#9EBF95',
          // },
          // {
          //   name: 'Quotation',
          //   data: this.quotation_count,
          //   color: '#8C8C8C',
          // },
          {
            name: 'Order',
            data: this.order_count,
            color: '#F2D377',
          },

        ],

        legend: {
          position: "top",
          offsetY: 5
        }
      };
    });
    this.GetInvoicecount();
  }
  GetInvoicecount() {

    var url = 'SmrBobateaDashoard/GetInvoicechart'
    this.service.get(url).subscribe((result: any) => {
      this.response_data = result;
      this.invoicechartcountlist = this.response_data.Invoicechart_list;
      this.shopify_count = Number(this.invoicechartcountlist[0].shopify_count)
      this.sale_count = Number(this.invoicechartcountlist[0].sale_count)
      if (this.invoicechartcountlist.length > 0) {
        this.saleschartflag = true;
      }
      console.log('lkmlk', this.sale_count)
      this.series_Value = [this.sale_count, this.shopify_count];
      this.labels_value = ['System Invoice', 'Shopify Invoice'];

      this.Invoicechartcount = {
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
    this.GetSalesordersixmonthchart();
  }
  GetSalesordersixmonthchart() {
    debugger
    var url = 'SmrBobateaDashoard/GetSalesordersixmonthchart';
    this.service.get(url).subscribe((result: any) => {
      this.response_data = result;
      this.Salesordersixmonthchart_list = this.response_data.Salesordersixmonthchart_list;
      console.log('wedewmlmc', this.Salesordersixmonthchart_list)
      if (this.Salesordersixmonthchart_list.length > 0 ||this.Salesordersixmonthchart_list.length!=null) {
        this.salesordersixmonth_flag = true;
      }
      this.salesorder_datesixmonth = this.Salesordersixmonthchart_list.map((entry: { salesorder_datesixmonth: any }) => entry.salesorder_datesixmonth)
      this.salesordersixmonth = this.Salesordersixmonthchart_list.map((entry: { salesordersixmonth: any }) => entry.salesordersixmonth)
      this.whatsappordersixmonth = this.Salesordersixmonthchart_list.map((entry: { whatsappordersixmonth: any }) => entry.whatsappordersixmonth)
      this.shopifyordersixmonth = this.Salesordersixmonthchart_list.map((entry: { shopifyordersixmonth: any }) => entry.shopifyordersixmonth)

      this.salessixmonthchart = {
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
            name: 'Shopify Orders',
            data: this.shopifyordersixmonth,
            color: '#747C8C',
          },
          {
            name: 'WhatsApp Orders',
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

}
const randomizeArray1 = (arg: number[]): number[] => {
  const array = arg.slice();
  let currentIndex = array.length, temporaryValue, randomIndex;

  while (0 !== currentIndex) {
    randomIndex = Math.floor(Math.random() * currentIndex);
    currentIndex -= 1;

    temporaryValue = array[currentIndex];
    array[currentIndex] = array[randomIndex];
    array[randomIndex] = temporaryValue;
  }

  console.log('eoidweo', array)
  return array;
};
const randomizeArray2 = (arg: number[]): number[] => {
  const array = arg.slice();
  let currentIndex = array.length, temporaryValue, randomIndex;

  while (0 !== currentIndex) {
    randomIndex = Math.floor(Math.random() * currentIndex);
    currentIndex -= 1;

    temporaryValue = array[currentIndex];
    array[currentIndex] = array[randomIndex];
    array[randomIndex] = temporaryValue;
  }

  console.log('eoidweo', array)
  return array;
};
const randomizeArray3 = (arg: number[]): number[] => {
  const array = arg.slice();
  let currentIndex = array.length, temporaryValue, randomIndex;

  while (0 !== currentIndex) {
    randomIndex = Math.floor(Math.random() * currentIndex);
    currentIndex -= 1;

    temporaryValue = array[currentIndex];
    array[currentIndex] = array[randomIndex];
    array[randomIndex] = temporaryValue;
  }

  console.log('eoidweo', array)
  return array;
};
