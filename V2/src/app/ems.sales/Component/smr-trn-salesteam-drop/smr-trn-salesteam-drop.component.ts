import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-smr-trn-salesteam-drop',
  templateUrl: './smr-trn-salesteam-drop.component.html',
  styleUrls: ['./smr-trn-salesteam-drop.component.scss']
})
export class SmrTrnSalesteamDropComponent {

  countlist: any [] = [];
  countlist1 : any [] = [];
  dropstatuslist: any [] = [];
  responsedata: any;
  leadchartcountlist: any[] = [];
  leadchartcount: any = {};
  saleschartcountlist: any[] = [];
  saleschartcount: any = {};
  quotationcountlist: any[] = [];
  quotationchartcount: any = {};
  enquirychartcountlist: any[] = [];
  enquirychartcount: any = {};
  response_data: any;
  teamactivitysummary_list: any[] = [];
  selectedChartType: any;
  chartscountsummary_list: any[] = [];

  // overall chart
  totalperformance_list: any[] = [];
  flag2: boolean = false;
  months: any;
  customer_count1:any;
  order_count:any;
  enquiry_count:any;
  quotation_count:any;
  saleschart : any ={};
 
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService,
    public service: SocketService, private route: Router, private NgxSpinnerService: NgxSpinnerService) {

 }
 ngOnInit(): void {
  this.GetDropSummary();
  this.Getsaleschart();
this.Getteamactivitysummary();
this.GetSalesTeamSummary();

  this.NgxSpinnerService.show();
  var url  = 'SmrTrnSalesManager/GetSmrTrnManagerCount';
    this.service.get(url).subscribe((result:any) => {
    this.responsedata = result;
    this.countlist = this.responsedata.teamcount_list; 
    //console.log(this.countlist, 'testdata');
    this.NgxSpinnerService.hide();
    });
  }
  Getsaleschart() {

    var url = 'SmrTrnSalesManager/Getsaleschart';
    this.service.get(url).subscribe((result: any) => {
      this.response_data = result;
      this.totalperformance_list = this.response_data.saleschart_list;
      if (this.totalperformance_list.length > 0) {
        this.flag2 = true;
      }
      this.months = this.totalperformance_list.map((entry: { months: any }) => entry.months),
      this.quotation_count = this.totalperformance_list.map((entry: { quotation_count: any }) => entry.quotation_count)
      this.enquiry_count = this.totalperformance_list.map((entry: { enquiry_count: any }) => entry.enquiry_count)
      this.order_count = this.totalperformance_list.map((entry: { order_count: any }) => entry.order_count),
      this.customer_count1 = this.totalperformance_list.map((entry: { customer_count: any }) => entry.customer_count)
      // Initialize chart options
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
          categories: this.months,
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
            data: this.customer_count1,
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
  
        ],
        // fill: {
        //   type: "gradient",
        //   gradient: {
        //     shadeIntensity: 1,
        //     opacityFrom: 0.7,
        //     opacityTo: 0.9,
        //     stops: [0, 100]
        //   }
        // }
        legend: {
          position: "top",
          offsetY: 5
        }
      };
  
      // this.NgxSpinnerService.hide(); // Move this inside the subscribe to ensure it executes after the data is processed
  
    });
  }
  GetDropSummary(){
    this.NgxSpinnerService.show();
   var url = 'SmrTrnSalesManager/GetSalesManagerdrop'
   this.service.get(url).subscribe((result: any) => {
    $('#dropstatuslist').DataTable().destroy();
     this.responsedata = result;
     this.dropstatuslist = this.responsedata.dropstatuslist;
     //console.log(this.entity_list)
     setTimeout(() => {
       $('#dropstatuslist').DataTable()
       this.NgxSpinnerService.hide();
     }, 1);
 
 
   });
 }
 summary()
 {
    //this.NgxSpinnerService.show();
    this.route.navigate(['/smr/SmrTrnSalesManagerSummary'])
 }
  potentials()
  {
    //this.NgxSpinnerService.show();
    this.route.navigate(['/smr/SmrTrnSalesTeamPotentials'])
    
  }
  prospect()
  {
    //this.NgxSpinnerService.show();
    this.route.navigate(['/smr/SmrTrnSalesTeamProspects'])
  
  }
  
  drop()
  {
     //this.NgxSpinnerService.show();
    this.route.navigate(['/smr/SmrTrnSalesTeamDrop'])
  
  }

  completed()
  {
    //this.NgxSpinnerService.show();
    this.route.navigate(['/smr/SmrTrnSalesTeamComplete'])
  
  }
  Onopen()
  {

  }
  GetSalesTeamSummary() {
    var url = 'SmrTrnSalesManager/GetSalesTeamSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#countlist1').DataTable().destroy();
      this.responsedata = result;
      this.countlist1 = this.responsedata.Salesteam_list1;
      //console.log(this.entity_list)
      setTimeout(() => {
        $('#countlist1').DataTable();
      }, 1);
    });
  }
 
Getteamactivitysummary() {
  debugger
  var url = 'SmrTrnSalesManager/teamactivitySummary'
  this.service.get(url).subscribe((result: any) => {
    $('#chartscountsummary_list').DataTable().destroy();
    this.responsedata = result;
    this.chartscountsummary_list = this.responsedata.chartscounts_list1;
    //let lblamountseperator1 =(parseInt( this.chartscountsummary_list[].quoteorder_amount.replace(/[^\d]+/gi, '')) || 0).toLocaleString('en-IN')
    //console.log(lblamountseperator1)
    setTimeout(() => {
      $('#chartscountsummary_list').DataTable();
    }, 1);
  });
  
}


}
