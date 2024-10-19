import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { Router } from '@angular/router';
import { AES } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';

interface IDrop {
  remarks: any;
  leadbank_name: any;
}
@Component({
  selector: 'app-crm-trn-dropmarketing',
  templateUrl: './crm-trn-dropmarketing.component.html',
  styleUrls: ['./crm-trn-dropmarketing.component.scss']
})
export class CrmTrnDropmarketingComponent {
  teamdetails: any[] = [];
  responsedata: any;
  remarks: string | undefined;
  leadbank_name: string | undefined;
  Drop: any;
  countlist: any[] = [];
  totaltilecountdetails: any[] = [];
  totallapsedlongest: any[] = [];
  ////////
  chartscountlist: any[] = [];
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
  getdropremark_list: any;
  Months: any;
  new_leads: any;
  potentialscount: any;
  prospectcount: any;
  orderscount: any;
  internal_notes: any;
  showOptionsDivId: any;
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService, private route: Router,
    private NgxSpinnerService: NgxSpinnerService) {

  }
  ngOnInit(): void {
    this.GetDropManagerSummary();
    this.GetMarketingManagerSummary();
    this.Getteamcount();
    this.GetTotaltilecount();
    this.GetTotallapsedlongest();
    this.Getleadchartcount();
    this.Getteamactivitysummary();

    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
  }

  //teamcount//
  Getteamcount() {
    var url = 'MarketingManager/Getteamcount'
    this.service.get(url).subscribe((result: any) => {
      $('#teamdetails').DataTable().destroy();
      this.responsedata = result;
      this.teamdetails = this.responsedata.teamdetails;
      //console.log(this.entity_list)
      setTimeout(() => {
        $('#teamdetails').DataTable();
      }, 1);
    });
  }

  //Get Total tile count
  GetTotaltilecount() {
    var url = 'MarketingManager/GetTotaltilecount'
    this.service.get(url).subscribe((result: any) => {
      $('#totaltilecountdetails').DataTable().destroy();
      this.responsedata = result;
      this.totaltilecountdetails = this.responsedata.totaltilecount_lists;
      //console.log(this.entity_list)
      setTimeout(() => {
        $('#totaltilecountdetails').DataTable();
      }, 1);
    });
  }
  //Get Total lapsed longest count
  GetTotallapsedlongest() {
    var url = 'MarketingManager/Totallapsedlongest'
    this.service.get(url).subscribe((result: any) => {
      $('#totaltilecountdetails').DataTable().destroy();
      this.responsedata = result;
      this.totallapsedlongest = this.responsedata.totallapsedlongest;
      //console.log(this.entity_list)
      setTimeout(() => {
        $('#totaltilecountdetails').DataTable();
      }, 1);
    });
  }

  //// Summary Grid//////
  GetDropManagerSummary() {
    debugger
    this.NgxSpinnerService.show();
    var url = 'Marketingmanager/GetDropManagerSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#Drop').DataTable().destroy();
      this.responsedata = result;
      this.Drop = this.responsedata.Drop;
      this.NgxSpinnerService.hide();
      //console.log(this.source_list)
      setTimeout(() => {
        $('#Drop').DataTable();
      }, 1);
    });
  }
  //360//
  Onopen(param1: any, param2: any,param3:any) {
    const secretKey = 'storyboarderp';
    const lspage1 = "MM-Drop";
    const lspage = AES.encrypt(lspage1, secretKey).toString();
    console.log(param1);
    console.log(param2);
    const leadbank_gid = AES.encrypt(param1, secretKey).toString();
    const lead2campaign_gid = AES.encrypt(param2, secretKey).toString();
    const appointment_gid = AES.encrypt(param3, secretKey).toString();
    this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, appointment_gid,lspage]);
  }
  //Tiles count//
  GetMarketingManagerSummary() {
    var url = 'MarketingManager/GetMarketingManagerSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#countlist').DataTable().destroy();
      this.responsedata = result;
      this.countlist = this.responsedata.marketingmanager_lists;
      //console.log(this.entity_list)
      setTimeout(() => {
        $('#countlist').DataTable();
      }, 1);
    });
  }

  popmodal(parameter: string, parameter1: string) {

    this.internal_notes = parameter;
    this.leadbank_name = parameter1;
  }



  Getleadchartcount() {
    var url = 'MarketingManager/leadchartcount'
    this.service.get(url).subscribe((result: any) => {
      this.response_data = result;
      this.chartscountlist = this.response_data.chartscount_list;
      this.Months = this.chartscountlist.map((entry: { Months: any }) => entry.Months),
      this.new_leads = this.chartscountlist.map((entry: { new_leads: any }) => entry.new_leads)
      this.potentialscount = this.chartscountlist.map((entry: { potentialscount: any }) => entry.potentialscount)
      this.prospectcount = this.chartscountlist.map((entry: { prospectcount: any }) => entry.prospectcount),
      this.orderscount = this.chartscountlist.map((entry: { orderscount: any }) => entry.orderscount)
      // Initialize chart options
      this.leadchartcount = {
        chart: {
          type: 'bar',
          height: 360, // Adjust the height of the chart as needed
          // width: 600,
          background: 'White',
          foreColor: '#0F0F0F',
          fontFamily: 'inherit',
          toolbar: {
            show: false, // Set to false to hide the toolbar/menu icon
          },
        },
        colors: ['#8062D6', '#FFD54F', '#66BB6A', '#EF5350', '#0F0F0F'], // Use a set of colors for better combinations
        plotOptions: {
          bar: {
            horizontal: false,
            columnWidth: '50%', // Adjust the width of the bars
            borderRadius: 0, // Add some border radius for a more modern look
          },
        },
        dataLabels: {
          enabled: false, // Disable data labels for a cleaner look
        },
        xaxis: {
          categories: this.Months,
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
            style: {
              fontWeight: 'bold',
              fontSize: '14px',
              color: '#0F0F0F', // Set a different color for the y-axis title
            },
          },
        },
        series: [
          {
            name: 'Appointments',
            data: this.new_leads,
            color: '#CE7F92', //#1a70d9
          },
          {
            name: 'Enquiries',
            data: this.prospectcount,
            color: '#747C8C', //#1a70d9
          },
          {
            name: 'Quotations',
            data: this.potentialscount,
            color: '#667967', //#1a70d9
          },
          {
            name: 'Sales',
            data: this.orderscount,
            color: '#B4584B', //#1a70d9
          },
        ],
		legend: {
        position: "top",
        offsetY:5
      }
      };


    })
  }
  // Getquotationchartcount() {

  //   var url = 'MarketingManager/quotationchartcount'
  //   this.service.get(url).subscribe((result: any) => {
  //     this.response_data = result;
  //     this.quotationcountlist = this.response_data.chartscount_list;
  //     const data = this.quotationcountlist.map((entry: { potentialscount: any; }) => entry.potentialscount);
  //     const categories = this.quotationcountlist.map((entry: { potentialsmonth: any; }) => entry.potentialsmonth);

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
  //   debugger
  //   var url = 'MarketingManager/enquirychartcount'
  //   this.service.get(url).subscribe((result: any) => {
  //     this.response_data = result;
  //     this.enquirychartcountlist = this.response_data.chartscount_list;
  //     const data = this.enquirychartcountlist.map((entry: { prospectcount: any; }) => entry.prospectcount);
  //     const categories = this.enquirychartcountlist.map((entry: { prospectmonth: any; }) => entry.prospectmonth);

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

  //   var url = 'MarketingManager/saleschartcount'
  //   this.service.get(url).subscribe((result: any) => {
  //     this.response_data = result;
  //     this.saleschartcountlist = this.response_data.chartscount_list;
  //     const data = this.saleschartcountlist.map((entry: { orderscount: any; }) => entry.orderscount);
  //     const categories = this.saleschartcountlist.map((entry: { ordersmonth: any; }) => entry.ordersmonth);

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
    var url = 'MarketingManager/teamactivitysummary'
    this.service.get(url).subscribe((result: any) => {
      $('#chartscountsummary_list').DataTable().destroy();
      this.responsedata = result;
      this.chartscountsummary_list = this.responsedata.chartscount_list;
      //let lblamountseperator1 =(parseInt( this.chartscountsummary_list[].quoteorder_amount.replace(/[^\d]+/gi, '')) || 0).toLocaleString('en-IN')
      //console.log(lblamountseperator1)
      // setTimeout(() => {
      //   $('#chartscountsummary_list').DataTable();
      // }, 1);
    });

  }
  // updateChart(chartType: string) {
  //   this.selectedChartType = chartType;
  //   switch (chartType) {
  //     case 'lead':
  //       this.Getleadchartcount();
  //       break;
  //     case 'enquiry':
  //       this.Getenquirychartcount();
  //       break;
  //     case 'quote':
  //       this.Getquotationchartcount();
  //       break;
  //     case 'salesorder':
  //       this.Getsaleschartcountt();
  //       break;
  //     default:
  //       this.Getleadchartcount();
  //       break;
  //   }
  // }
  getdropremarks(gid: string) {
    debugger
    var param = {
      appointment_gid: gid,
    }
    var url1 = 'MarketingManager/GetMarketingManagerDropRemarks'
    this.service.getparams(url1,param ).subscribe((result: any) => {
      this.getdropremark_list = result.campaignbin_list;
    });
  }
  Onteamview(campaign_gid:any){
    const secretKey = 'storyboarderp';
    const param = (campaign_gid);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.route.navigate(['/crm/CrmTrnTteamleadsview', encryptedParam])
  }

  toggleOptions(appointment_gid: any) {
    if (this.showOptionsDivId === appointment_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = appointment_gid;
    }
  }
}
