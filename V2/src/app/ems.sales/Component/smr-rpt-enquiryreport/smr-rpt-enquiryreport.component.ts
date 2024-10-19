import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators,} from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { data } from 'jquery';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';

@Component({
  selector: 'app-smr-rpt-enquiryreport',
  templateUrl: './smr-rpt-enquiryreport.component.html',
  styleUrls: ['./smr-rpt-enquiryreport.component.scss']
})
export class SmrRptEnquiryreportComponent {
  chartOptions: any;
  Date: string;
  GetEnquiryForLastSixMonths_List :any;
  GetEnquiryDetailSummary :any;
  chart: ApexCharts | null = null;
  reactiveForm: FormGroup | any;
  responsedata: any;
  salesteamgrid_list :any;
  getData: any;
  salesorder_gid : any;
  data: any;  
  parameterValue: any;
  from_date!:string;
  to_date!:string;
  

  constructor(private formBuilder: FormBuilder,public route:ActivatedRoute,public service :SocketService,private router:Router,private ToastrService: ToastrService) {
    this.Date = new Date().toString();
    
    
    

  }
  

  ngOnInit(): void {
    
    this.GetEnquiryForLastSixMonths();
  const options: Options = {
      dateFormat: 'd-m-Y',    
    };
    flatpickr('.date-picker', options);

  }
  GetEnquiryForLastSixMonths( )
 {
  debugger
  var url = 'SmrRptEnquiryReport/GetEnquiryForLastSixMonths'
  this.service.get(url).subscribe((result: any) => {
    $('#GetEnquiryForLastSixMonths_List').DataTable().destroy();
    this.responsedata = result;
    this.GetEnquiryForLastSixMonths_List = this.responsedata.GetEnquiryForLastSixMonths_List;
    const categories = this.GetEnquiryForLastSixMonths_List.map((entry: { salesorder_date: any; }) => entry.salesorder_date);
    const data = this.GetEnquiryForLastSixMonths_List.map((entry: { ordercount: any; }) => entry.ordercount);

    // this.chartOptions.xaxis.categories = categories;
    // this.chartOptions.series[0].data = data;
    // setTimeout(() => {
    //   $('#GetOrderForLastSixMonths_List');
    // }, 1);
    this.chartOptions={
      chart: {
        type: 'bar',
        height: 400,
        width: 570,
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
          columnWidth: '25%',
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
        categories:categories,
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
            color: '#7FC7D9',
          },
        },
      },
      series: [
        {
          name: 'Enquiry Count',
          color:'#87CEEB',
          data:data,
        },
      ],
    };
    this.renderChart()
  
  })

  
  
}
private renderChart(): void {
  if (this.chart) {
    this.chart.updateOptions(this.chartOptions); // Update existing chart with new options
  } else {
    this.chart = new ApexCharts(document.getElementById('chart'), this.chartOptions);
    this.chart.render();
  }

}
onSearchClick(){
  debugger
  var url='SmrRptOrderReport/GetEnquiryForLastSixMonthsSearch';
    let params ={
      from_date:this.from_date,
      to_date:this.to_date
    }
    this.service.getparams(url,params).subscribe((result: any) => {
      $('#GetEnquiryForLastSixMonths_List').DataTable().destroy();
      this.responsedata = result;
      this.GetEnquiryForLastSixMonths_List = this.responsedata.GetEnquiryForLastSixMonths_List;
      ApexCharts.exec('chart', 'destroy');
      this.chartOptions.xaxis.categories = this.GetEnquiryForLastSixMonths_List.map((entry: { salesorder_date: any }) => entry.salesorder_date);
    this.chartOptions.series[0].data = this.GetEnquiryForLastSixMonths_List.map((entry: { ordercount: any }) => entry.ordercount);

    this.renderChart()
    })

}
onrefreshclick(){
  this.from_date = null!;
    this.to_date = null!;
    this.GetEnquiryForLastSixMonths()
    window.location.reload()
}

ondetail(month: any,year:any ,parameter: string,quotation_gid: string) {
  debugger
  var url = 'SmrRptEnquiryReport/GetEnquiryDetailSummary'
  let param = {
    quotation_gid : quotation_gid ,
    month: month, year: year
  }
  this.service.getparams(url, param).subscribe((result: any) => {
    this.responsedata = result;
    this.GetEnquiryDetailSummary = result.GetEnquiryDetailSummary;
    console.log(this.GetEnquiryDetailSummary)
    setTimeout(() => {
      $('#GetEnquiryDetailSummary');
    }, 1);

  });
}
}


