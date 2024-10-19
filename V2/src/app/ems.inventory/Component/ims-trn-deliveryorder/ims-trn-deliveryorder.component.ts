import { Component } from '@angular/core';
import { FormBuilder, FormGroup,} from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { AES } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-ims-trn-deliveryorder',
  templateUrl: './ims-trn-deliveryorder.component.html',
  styleUrls: ['./ims-trn-deliveryorder.component.scss'],
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
export class ImsTrnDeliveryorderComponent {
  responsedata: any;
  deliveryorders_list: any[] = [];
  getData: any;
  company_code: any;
  showOptionsDivId:any;
  chart: ApexCharts | null = null;
  chartOptions: any;
  flag: boolean = true;
  salesorderlastsixmonths_list:any[]=[];
  ordertoinvoicecount:any;
  ordercount:any;
  productsummary_list:any;
  constructor(private formBuilder: FormBuilder,public NgxSpinnerService: NgxSpinnerService,public service :SocketService,private route:Router,private ToastrService: ToastrService) {
  }
  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
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
  ngOnInit(): void {
    this.runBackgroundApiCall2();
     this.GetImsTrnDeliveryorderSummary();
     this.getordersixmonths();
     document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
 }

 getordersixmonths(){
  var url = 'ImsTrnDeliveryorderSummary/GetDOsixmonthschart'
  this.service.get(url).subscribe((result:any)=>{
    this.salesorderlastsixmonths_list = result.Dolastsixmonths_list;
    this.ordertoinvoicecount = result.ordertodocount;
    this.ordercount = result.ordercount;
    if (this.salesorderlastsixmonths_list == null) {
      this.flag = false;
    }
    const categories = this.salesorderlastsixmonths_list.map((entry: { months: any; }) => entry.months);
      const data = this.salesorderlastsixmonths_list.map((entry: { ordercount: any; }) => entry.ordercount);
      this.chartOptions = {
        chart: {
          type: "bar",
          height: 240,
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
            columnWidth: '15%',
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
              color: '#7FC7D9',
            },
          },
          tickAmount: 8,
          labels: {
                  formatter: (value: number) => {
                    return  value.toLocaleString(); 
                  },
        }
      },
        legend: {
             horizontalAlign: "right"
           },
        series: [
          {
            name: 'DO Count',
            color: '#87CEEB',
            data: data,
          },
        ],
      };
  })
  this.renderChart()
}
private renderChart(): void {
  if (this.chart) {
    this.chart.updateOptions(this.chartOptions); 
  } else {
    this.chart = new ApexCharts(document.getElementById('chart'), this.chartOptions);
    this.chart.render();
  }
}
GetImsTrnDeliveryorderSummary() {
   var url = 'ImsTrnDeliveryorderSummary/GetImsTrnDeliveryorderSummary'
   this.NgxSpinnerService.show()
    this.service.get(url).subscribe((result: any) => {
      $('#deliveryorder_list').DataTable().destroy();
      this.responsedata = result;
      this.deliveryorders_list = result.deliveryorder_list;
      setTimeout(() => {
        $('#deliveryorder_list').DataTable();
              }, 1);
      this.NgxSpinnerService.hide()   
   })
}
PrintPDF(directorder_gid: any) {
  const api = 'ImsTrnDeliveryorderSummary/GetDeliveryOrderRpt';
  this.NgxSpinnerService.show()
  let param = {
    directorder_gid:directorder_gid
  }
  this.service.getparams(api,param).subscribe((result: any) => {
    if(result!=null){
      this.service.filedownload1(result);
    }
    this.NgxSpinnerService.hide()
  });
}
 onview(params:any){
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.route.navigate(['/ims/ImsTrnDeliveryorderView',encryptedParam]);
  }
   splitIntoLines(text: string, lineLength: number): string[] {
    const lines = [];
    for (let i = 0; i < text.length; i += lineLength) {
      lines.push(text.substr(i, lineLength));
    }
    return lines;
  }

  Getproduct(directorder_refno: any){
    debugger;
    let param = {
      directorder_refno:directorder_refno
    } 
    var url = 'ImsTrnDeliveryorderSummary/GetDetialViewProduct'
    this.service.getparams(url,param).subscribe((result: any) => {
      debugger
      $('#productsummary_list').DataTable().destroy();
      this.responsedata = result;
      this.productsummary_list = this.responsedata.product_list;
      setTimeout(() => {
        $('#productsummary_list').DataTable();
      }, 1);
    });
  }

}
