import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
@Component({
  selector: 'app-smr-trn-sales-manager-summary',
  templateUrl: './smr-trn-sales-manager-summary.component.html',
  styleUrls: ['./smr-trn-sales-manager-summary.component.scss'],
  styles: [`
  table thead th, 
  .table tbody td { 
   position: relative; 
  z-index: 0;
  } 
  .table thead th:last-child, 
  
  .table tbody td:last-child { 
   position: sticky; 
  
  right: 0; 
   z-index: 0; 
  
  } 
  .table td:last-child, 
  
  .table th:last-child { 
  
  padding-right: 50px; 
  
  } 
  .table.table-striped tbody tr:nth-child(odd) td:last-child { 
  
   background-color: #ffffff; 
    
    } 
    .table.table-striped tbody tr:nth-child(even) td:last-child { 
     background-color: #f2fafd; 
  
  } 
  `]
})
export class SmrTrnSalesManagerSummaryComponent {
  responsedata:any;
  countlist: any [] = [];
  countlist1 : any[]=[];
  totallist: any [] = [];
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
  showOptionsDivId:any;
  rows:any []=[];

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
  document.addEventListener('click', (event: MouseEvent) => {
    if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
      this.showOptionsDivId = null;
    }
  });
  debugger
 this.GetTotalSummary();
//  this.Getcustomercount();
this.Getteamactivitysummary();
this.GetSalesTeamSummary();
this.Getsaleschart();

 var url  = 'SmrTrnSalesManager/GetSmrTrnManagerCount';
    this.service.get(url).subscribe((result:any) => {
    this.responsedata = result;
    this.countlist = this.responsedata.teamcount_list; 
    console.log(this.countlist, 'testdata');
    });
 }
 GetTotalSummary(){
  var url = 'SmrTrnSalesManager/GetSalesManagerTotal'
  this.service.get(url).subscribe((result: any) => {
   $('#totallist').DataTable().destroy();
    this.responsedata = result;
    this.totallist = this.responsedata.totalalllist;
    //console.log('ewfef',this.totallist)
    setTimeout(() => {
      $('#totallist').DataTable()
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

  completed()
  {
    //this.NgxSpinnerService.show();
    this.route.navigate(['/smr/SmrTrnSalesTeamProspects'])
  
  }
  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }

  drop()
  {
    //this.NgxSpinnerService.show();
    this.route.navigate(['/smr/SmrTrnSalesTeamDrop'])
  
  }
  Onopen(leadbank_gid:any,lead2campaign_gid:any,leadbankcontact_gid:any)
  {
 debugger
  const secretKey = 'storyboarderp';
  const lspage1 = "Total";
  const lspage = AES.encrypt(lspage1, secretKey).toString();
  const param = (leadbank_gid);
  console.log('sdas',leadbank_gid)
  const param1 = (lead2campaign_gid);
  const param2 = (leadbankcontact_gid);
  const encryptedParam = AES.encrypt(param,secretKey).toString();
  const encryptedParam1 = AES.encrypt(param1,secretKey).toString();
  const encryptedParam2 = AES.encrypt(param2,secretKey).toString();
  this.route.navigate(['/smr/SmrTrnSales360',encryptedParam,encryptedParam1,encryptedParam2,lspage]) 
  }



  
   GetSalesTeamSummary() {
    debugger
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
    console.log(this.countlist1)
  }
 /*Charts*/
//  Getcustomercount() {

//   var url = 'SmrTrnSalesManager/customercount'
//   this.service.get(url).subscribe((result: any) => {
//     this.responsedata = result;
//     this.leadchartcountlist = this.responsedata.chartscounts_list1;
//     const data = this.leadchartcountlist.map((entry: { customermonthcount: any; }) => entry.customermonthcount);
//     const categories = this.leadchartcountlist.map((entry: { customermonth: any; }) => entry.customermonth);

//     // Initialize chart options
//     this.leadchartcount = {
//       chart: {
//         type: 'bar',
//         height: 360, // Adjust the height of the chart as needed
//         // width: 600,
//         background: 'White',
//         foreColor: '#0F0F0F',
//         toolbar: {
//           show: false, // Set to false to hide the toolbar/menu icon
//         },
//       },
//       colors: ['#8062D6', '#FFD54F', '#66BB6A', '#EF5350', '#0F0F0F'], // Use a set of colors for better combinations
//       plotOptions: {
//         bar: {
//           horizontal: false,
//           columnWidth: '1%', // Adjust the width of the bars
//           borderRadius: 0, // Add some border radius for a more modern look
//         },
//       },
//       dataLabels: {
//         enabled: false, // Disable data labels for a cleaner look
//       },
//       stroke: {
//         show: true,
//         width: 2,
//         colors: ['transparent'],
//       },
//       xaxis: {
//         categories: categories,
//         labels: {
//           style: {
//             fontWeight: 'bold',
//             fontSize: '14px',
//             //colors: ['#FF5733', '#33FF57', '#5733FF', '#FFFF33'], // Set different colors for each label
//           },
//         },
//       },
//       yaxis: {
//         title: {
//           text: 'Weeks',
//           style: {
//             fontWeight: 'bold',
//             fontSize: '14px',
//             color: '#0F0F0F', // Set a different color for the y-axis title
//           },
//         },
//       },
//       series: [
//         {
//           name: 'Customer',
//           data: data,
//           color: '#b55604', //#1a70d9
//         },
//       ],
//     };


//   })
// }
// Getquotationchartcount() {

//   var url = 'SmrTrnSalesManager/quotationchartcount'
//   this.service.get(url).subscribe((result: any) => {
//     this.responsedata = result;
//     this.quotationcountlist = this.responsedata.chartscounts_list1;
//     const data = this.quotationcountlist.map((entry: { quotationmonthcount: any; }) => entry.quotationmonthcount);
//     const categories = this.quotationcountlist.map((entry: { quotationmonth: any; }) => entry.quotationmonth);

//     // Initialize chart options
//     this.quotationchartcount = {
//       chart: {
//         type: 'bar',
//         height: 360, // Adjust the height of the chart as needed
//         // width: 600,
//         background: 'White',
//         foreColor: '#0F0F0F',
//         toolbar: {
//           show: false, // Set to false to hide the toolbar/menu icon
//         },
//       },
//       colors: ['#8062D6', '#FFD54F', '#66BB6A', '#EF5350', '#0F0F0F'], // Use a set of colors for better combinations
//       plotOptions: {
//         bar: {
//           horizontal: false,
//           columnWidth: '1%', // Adjust the width of the bars
//           borderRadius: 0, // Add some border radius for a more modern look
//         },
//       },
//       dataLabels: {
//         enabled: false, // Disable data labels for a cleaner look
//       },
//       stroke: {
//         show: true,
//         width: 2,
//         colors: ['transparent'],
//       },
//       xaxis: {
//         categories: categories,
//         labels: {
//           style: {
//             fontWeight: 'bold',
//             fontSize: '14px',
//             //colors: ['#FF5733', '#33FF57', '#5733FF', '#FFFF33'], // Set different colors for each label
//           },
//         },
//       },
//       yaxis: {
//         title: {
//           text: 'Weeks',
//           style: {
//             fontWeight: 'bold',
//             fontSize: '14px',
//             color: '#0F0F0F', // Set a different color for the y-axis title
//           },
//         },
//       },
//       series: [
//         {
//           name: 'Quotation',
//           data: data,
//           color: '#096e36', //#1a70d9
//         },
//       ],
//     };


//   })
// }
// Getenquirychartcount() {

//   var url = 'SmrTrnSalesManager/enquirychartcount'
//   this.service.get(url).subscribe((result: any) => {
//     this.responsedata = result;
//     this.enquirychartcountlist = this.responsedata.chartscounts_list1;
//     const data = this.enquirychartcountlist.map((entry: { enquirymonthcount: any; }) => entry.enquirymonthcount);
//     const categories = this.enquirychartcountlist.map((entry: { enquirymonth: any; }) => entry.enquirymonth);

//     // Initialize chart options
//     this.enquirychartcount = {
//       chart: {
//         type: 'bar',
//         height: 360, // Adjust the height of the chart as needed
//         // width: 600,
//         background: 'White',
//         foreColor: '#0F0F0F',
//         toolbar: {
//           show: false, // Set to false to hide the toolbar/menu icon
//         },
//       },
//       colors: ['#8062D6', '#FFD54F', '#66BB6A', '#EF5350', '#0F0F0F'], // Use a set of colors for better combinations
//       plotOptions: {
//         bar: {
//           horizontal: false,
//           columnWidth: '1%', // Adjust the width of the bars
//           borderRadius: 0, // Add some border radius for a more modern look
//         },
//       },
//       dataLabels: {
//         enabled: false, // Disable data labels for a cleaner look
//       },
//       stroke: {
//         show: true,
//         width: 2,
//         colors: ['transparent'],
//       },
//       xaxis: {
//         categories: categories,
//         labels: {
//           style: {
//             fontWeight: 'bold',
//             fontSize: '14px',
//             //colors: ['#FF5733', '#33FF57', '#5733FF', '#FFFF33'], // Set different colors for each label
//           },
//         },
//       },
//       yaxis: {
//         title: {
//           text: 'Weeks',
//           style: {
//             fontWeight: 'bold',
//             fontSize: '14px',
//             color: '#0F0F0F', // Set a different color for the y-axis title
//           },
//         },
//       },
//       series: [
//         {
//           name: 'Enquire',
//           data: data,
//           color: '#5d1d82', //#1a70d9
//         },
//       ],
//     };


//   })
// }
// Getsaleschartcountt() {

//   var url = 'SmrTrnSalesManager/saleschartcount'
//   this.service.get(url).subscribe((result: any) => {
//     this.responsedata = result;
//     this.saleschartcountlist = this.responsedata.chartscounts_list1;
//     const data = this.saleschartcountlist.map((entry: { salesmonthcount: any; }) => entry.salesmonthcount);
//     const categories = this.saleschartcountlist.map((entry: { salesmonth: any; }) => entry.salesmonth);

//     // Initialize chart options
//     this.saleschartcount = {
//       chart: {
//         type: 'bar',
//         height: 360, // Adjust the height of the chart as needed
//         // width: 600,
//         background: 'White',
//         foreColor: '#0F0F0F',
//         toolbar: {
//           show: false, // Set to false to hide the toolbar/menu icon
//         },
//       },
//       colors: ['#8062D6', '#FFD54F', '#66BB6A', '#EF5350', '#0F0F0F'], // Use a set of colors for better combinations
//       plotOptions: {
//         bar: {
//           horizontal: false,
//           columnWidth: '1%', // Adjust the width of the bars
//           borderRadius: 0, // Add some border radius for a more modern look
//         },
//       },
//       dataLabels: {
//         enabled: false, // Disable data labels for a cleaner look
//       },
//       stroke: {
//         show: true,
//         width: 2,
//         colors: ['transparent'],
//       },
//       xaxis: {
//         categories: categories,
//         labels: {
//           style: {
//             fontWeight: 'bold',
//             fontSize: '14px',
//             //colors: ['#FF5733', '#33FF57', '#5733FF', '#FFFF33'], // Set different colors for each label
//           },
//         },
//       },
//       yaxis: {
//         title: {
//           text: 'Weeks',
//           style: {
//             fontWeight: 'bold',
//             fontSize: '14px',
//             color: '#0F0F0F', // Set a different color for the y-axis title
//           },
//         },
//       },
//       series: [
//         {
//           name: 'Sales',
//           data: data,
//           color: '#91346c', //#1a70d9
//         },
//       ],
//     };


//   })
// }
Getteamactivitysummary() {
  
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

// updateChart(chartType: string) {
//   this.selectedChartType = chartType;
//   switch (chartType) {
//     case 'DayWise':
//       this.Getcustomercount();
//       break;
//     case 'week':
//       this.Getenquirychartcount();
//       break;
//     case 'month':
//       this.Getquotationchartcount();
//       break;
//     case 'year':
//       this.Getsaleschartcountt();
//       break;
//     default:
//       this.Getcustomercount();
//       break;
//   }
// }
  


// overall chart 
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
}

