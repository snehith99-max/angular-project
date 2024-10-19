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
  selector: 'app-smr-rpt-productsellingreport',
  templateUrl: './smr-rpt-productsellingreport.component.html',
  styleUrls: ['./smr-rpt-productsellingreport.component.scss']
})
export class SmrRptProductsellingreportComponent {


  chartOptions: any;
  Date: string;
  chart: ApexCharts | null = null;
  GetproductssellingForLastSixMonths_List: any;
  GetproudctssellingDetailSummarylist: any;
  reactiveForm: FormGroup | any;
  responsedata: any;
  salesteamgrid_list: any;
  getData: any;
  searchclicked:boolean = false;
  mdlproductName:any;
  params22:any;
  product_list: any[] = [];
  expandedRows: any[] = [];
  categories: any;
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


  ngOnInit(){

    this.productdropdown();
    this.GetproductsForLastSixMonths();
    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);
    
    const today = new Date();
this.maxDate = today.toISOString().split('T')[0];
  }
  private renderChart(): void {
    if (this.chart) {
      this.chart.updateOptions(this.chartOptions); // Update existing chart with new options
    } else {
      this.chart = new ApexCharts(document.getElementById('chartOptions'), this.chartOptions);
      this.chart.render();
    }
  }
  GetproductsForLastSixMonths(){

    var url = 'product_reports/productsellingforlastsixmonths';
    this.service.get(url).subscribe((result:any)=>{
      this.GetproductssellingForLastSixMonths_List = result.GetproductssellingForLastSixMonths_List;
      if (this.GetproductssellingForLastSixMonths_List == null) {
        this.flag = false;
      }
      this.categories = this.GetproductssellingForLastSixMonths_List.map((entry: { months: any; }) => entry.months);
      const data = this.GetproductssellingForLastSixMonths_List.map((entry: { productcounts: any; }) => entry.productcounts);
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
          categories: this.categories,
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
          labels: {
            formatter: (value: number) => {
              return value.toLocaleString();
            },
          }
        },
        series: [
          {
            name: 'Products',
            color: '#9b98b8',
            data: data,
          },
        ],
      };

    });
    this.renderChart()

  }
 
  productdropdown(){

    var url = "product_reports/productdropdown";
    this.service.get(url).subscribe((result:any)=>{
      this.product_list = result.productreport_list
    });

  }
  onrefreshclick(){
    this.GetproductsForLastSixMonths();
    this.searchclicked = false;
  }

  onSearchClick(){
    if(this.mdlproductName != null){
    var url = 'product_reports/getproductsellingforsixmonthssearch';
      this.params22 = {
        product:this.mdlproductName,
     }
   this.service.getparams(url,this.params22).subscribe((result:any)=>{
    this.GetproudctssellingDetailSummarylist = result.GetproudctssellingDetailSummarylist;
    this.GetproductssellingForLastSixMonths_List = result.GetproductssellingForLastSixMonths_List;
      if (this.GetproductssellingForLastSixMonths_List == null) {
        this.flag = false;
      }
      else{
      this.flag = true;
      }
      // ApexCharts.exec('chartOptions', 'destroy');
      const categories = this.GetproductssellingForLastSixMonths_List.map((entry: { months: any }) => entry.months);
      console.log(this.categories)
      const data = this.GetproductssellingForLastSixMonths_List.map((entry: { productcounts: any }) => entry.productcounts);
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
          labels: {
            formatter: (value: number) => {
              return value.toLocaleString();
            },
          }
        },
        series: [
          {
            name: 'Products',
            color: '#9b98b8',
            data: data,
          },
        ],
      };
   
      this.renderChart();
      this.searchclicked = true;
   });
  }
  else{
    this.ToastrService.warning("Kindly select the product")
  }
  }
  ondetail(month: any, year: any) { 

  }

  onRowClick(index: number) {
    if (this.expandedRowIndex === index) {
      this.expandedRowIndex = null; // Collapse if the same row is clicked again
    } else {
      this.expandedRowIndex = index; // Expand the clicked row
    }
    this.GetproductssellingForLastSixMonths_List.forEach((data: any, i: number) => {
      data.isExpand = (i === this.expandedRowIndex);
    });
  }

  toggleVisibility(item: any) {
    item.isExpand = !item.isExpand;
  }

  
  onclearproduct(){
    this.mdlproductName = null;
    this.searchclicked = false;
  }
  

}
