import { Component, OnInit, ViewChild } from "@angular/core";
import { ChartComponent } from "ng-apexcharts";
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { Subscription, Observable, timer } from 'rxjs';
import { first } from 'rxjs/operators';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';

interface MonthlyChartDataItem {
  monthname: string;
  renewal_month_name: string;
  renewal_count: string;
}

export type ChartOptions = {
  series: ApexNonAxisChartSeries;
  chart: ApexChart;
  
  responsive: ApexResponsive[];
  
  labels: any;
};

@Component({
  selector: 'app-pmr-rpt-agreementreport',
  templateUrl: './pmr-rpt-agreementreport.component.html',
  styleUrls: ['./pmr-rpt-agreementreport.component.scss']
})
export class PmrRptAgreementreportComponent {
  chartOptions1: any = {};
  labels_value: any;
  DashboardCount_List1: any[] = [];
  paymentchart:any={};
  chartOptions2: any = {};
  monthlyflag :boolean= false;
  DashboardCount_List: any[] = [];
  response_data: any;
  chart: ApexCharts | null = null;
  overall_list: any[] = [];
  renewalteam_list: any[] = [];
  paymentchartflag: boolean = false;
  open_count: any;
  closed_count: any;
  dropped_count: any;
  chartOptions:any;
  responsedata :any;
  GetRenewalSummary_lists: any = {};
  EmployeeCountList: any[]=[];
  RenewalsCountList: any[]=[];
  Getpaymentchart_list:any[]=[];
  GetMonthlyRenewal_lists:any[]=[];
  open : any;
  closed : any;
  dropped : any;
  series_Value: any;
  isExpand: boolean = false;
  expandedRowIndex: number | null = null;
  GetRenewalForLastSixMonths_List: any;
  GetRenewalDetailSummarylist: any;
  
 
  constructor(public service:SocketService,private router:Router,private route:Router, private ToastrService: ToastrService,public NgxSpinnerService:NgxSpinnerService) {
  }

  ngOnInit(): void {

    this.GetRenewalChart();

    this.GetMonthyRenewal();
    this.GetInvoiceForLastSixMonths();


    
  }
  

  GetRenewalChart(){
    var url = 'PmrTrnPurchaseagreement/GetRenewalChart';
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.GetRenewalSummary_lists= this.responsedata.GetRenewalSummary_lists;
      if ( this.GetRenewalSummary_lists.length === 0) {
        this.paymentchartflag = true;
      }
      this.open_count =  Number ( this.GetRenewalSummary_lists[0].open_count);
      this.closed_count =  Number(this.GetRenewalSummary_lists[0].closed_count);
      this.dropped_count =  Number(this.GetRenewalSummary_lists[0].dropped_count);
      this.series_Value = [ this.open_count,this.closed_count   ];
      this.labels_value = ['Open','Closed'];
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



   GetInvoiceForLastSixMonths() {
    debugger
    this.NgxSpinnerService.show();
    var url = 'PmrTrnPurchaseagreement/GetRenewalReportForLastSixMonths'
    this.service.get(url).subscribe((result: any) => {
      $('#GetRenewalForLastSixMonths_List').DataTable().destroy();
      this.responsedata = result;
      this.GetRenewalForLastSixMonths_List = this.responsedata.GetRenewalForLastSixMonths_List;
      this.NgxSpinnerService.hide();

      const categories = this.GetRenewalForLastSixMonths_List.map((entry: { months: any; }) => entry.months);
    

    })
   
  }
   onSearchClick() {
    debugger
    var url = 'PmrTrnPurchaseagreement/GetRenewalReportForLastSixMonthsSearch';
    this.NgxSpinnerService.show()
  
    this.service.get(url).subscribe((result: any) => {
      $('#GetRenewalForLastSixMonths_List').DataTable().destroy();
      this.responsedata = result;
      this.GetRenewalForLastSixMonths_List = this.responsedata.GetRenewalForLastSixMonths_List;
      
    })
  }
   onRowClick(index: number) {
    if (this.expandedRowIndex === index) {
      this.expandedRowIndex = null; // Collapse if the same row is clicked again
    } else {
      this.expandedRowIndex = index; // Expand the clicked row
    }
    this.GetRenewalForLastSixMonths_List.forEach((data: any, i: number) => {
      data.isExpand = (i === this.expandedRowIndex);
    });
  }
  ondetail(month: any, year: any) {
    debugger
    this.GetRenewalDetailSummarylist = null;
    this.isExpand = true;
   console.log("cejcjning")
    var url = 'PmrTrnPurchaseagreement/GetrenewalDetailSummary'
    let param = {
     
      month: month,
      year: year
    }
    setTimeout(() => {
    this.service.getparams(url, param).subscribe((result: any) => {
      // $('#GetRenewalDetailSummarylist').DataTable().destroy();
      this.responsedata = result;
      this.GetRenewalDetailSummarylist = result.GetRenewalDetailSummarylist;
      window.scrollBy(0, 400);
      setTimeout(() => {
        $('#GetRenewalDetailSummarylist').DataTable();
      }, 1);
    });
  }, 100);
  }
  toggleVisibility(item: any) {
    item.isExpand = !item.isExpand;
  }
  
  private scrollPageToBottom(): void {
    window.scrollTo(0, document.body.scrollHeight);
  }

   GetMonthyRenewal(){
    debugger
    var url = 'PmrTrnPurchaseagreement/GetMonthyRenewal';
    debugger
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.GetMonthlyRenewal_lists= this.responsedata.GetMonthlyRenewal_lists;
      if (this.GetMonthlyRenewal_lists.length>0) {
        this.monthlyflag = false;
      }
      const formattedMonthlyChartData = {
        present:  this.GetMonthlyRenewal_lists.map(item => Number(item.renewal_month_name)),
        absent:  this.GetMonthlyRenewal_lists.map(item => Number(item.renewal_count)),
      };     

    this.chartOptions2 = {
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
      colors: ['#66BB6A', '#FFD54F', '#66BB6A', '#EF5350', '#FFFF33'], // Use a set of colors for better combinations
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
        categories:  this.GetMonthlyRenewal_lists.map(item => item.renewal_month_name),
        axisBorder: { show: false, },
        axisTicks: { show: false, },         
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
          text: 'Count',
          style: {
            fontWeight: 'bold',
            fontSize: '14px',
            color: '#0F0F0F', // Set a different color for the y-axis title
          },
        },
      },
      series: [
        {
          name: 'Month',
          data: formattedMonthlyChartData.present,
        },
        {
          name: 'Count',
          data: formattedMonthlyChartData.absent,
        },
       
      ],
    
    }
    }
  

  )};
  } 



