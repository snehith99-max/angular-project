// import { Component } from '@angular/core';

// @Component({
//   selector: 'app-pmr-trn-purchaseagreement',
//   templateUrl: './pmr-trn-purchaseagreement.component.html',
//   styleUrls: ['./pmr-trn-purchaseagreement.component.scss']
// })
// export class PmrTrnPurchaseagreementComponent {onview

// }

import { Component, DebugEventListener, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { FormGroup } from '@angular/forms';
import { environment } from 'src/environments/environment';
import { NgxSpinnerService } from 'ngx-spinner';


@Component({
  selector: 'app-pmr-trn-purchaseagreement',
  templateUrl: './pmr-trn-purchaseagreement.component.html',
  styleUrls: ['./pmr-trn-purchaseagreement.component.scss']
})
export class PmrTrnPurchaseagreementComponent {
  purchaseagreementorder_list:any[]=[];
  responsedata: any;
  parameterValue1: any;
  company_code: any;
  showOptionsDivId: any; 
  parameterValue: any;
  rows: any[] = [];
  showContractpo: boolean = false;
  chart: ApexCharts | null = null;
  salesorderlastsixmonths_list:any[]=[];
  ordertoinvoicecount:any;
  ordercount:any;
  chartOptions: any;
  flag: boolean = true;

  constructor(public service :SocketService,private router:Router,private route:Router, private ToastrService: ToastrService,public NgxSpinnerService:NgxSpinnerService) {
    
  }

  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }


  ngOnInit(): void {
    this.GetPurchaseagreementOrderSummary();
    this.getpurchaseordersixmonths();
    this.purchaseagreementorder_list.sort((a,b) => {
      return new (b.created_date) - new (a.created_date); 
    });

    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
  }

  getpurchaseordersixmonths(){
    var url = 'PmrTrnPurchaseOrder/Getpurchaseordersixmonthschart'
    this.service.get(url).subscribe((result:any)=>{
      this.salesorderlastsixmonths_list = result.purchaseorderlastsixmonths_list;
      this.ordertoinvoicecount = result.ordertoinvoicecount;
      this.ordercount = result.ordercount;
      if (this.salesorderlastsixmonths_list == null) {
        this.flag = false;
      }
      const categories = this.salesorderlastsixmonths_list.map((entry: { months: any; }) => entry.months);
        const data = this.salesorderlastsixmonths_list.map((entry: { orderamount: any; }) => entry.orderamount);
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
              name: 'Order Value',
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
      this.chart.updateOptions(this.chartOptions); // Update existing chart with new options
    } else {
      this.chart = new ApexCharts(document.getElementById('chart'), this.chartOptions);
      this.chart.render();
    }
  }

  filedownload(file_path: any, file_name:any) {
    debugger
    if (file_path != null) {
      let param = { file_path: file_path, file_name: file_name }
      this.service.post('PmrTrnPurchaseOrder/DownloadDocument', param).subscribe((result: any) => {
        if (result.status == true) {
          this.service.filedownload1(result);
        }
      });
      // /this.service.downloadfile(file_path,"sales");
    }
    else {
      this.ToastrService.warning("No file has been uploaded for this order");
    }
  }
  Mail(params : string)
  {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.router.navigate(['/pmr/PmrTrnPurchaseordermail',encryptedParam])
  }
  		
  PrintPDF(purchaseorder_gid: any) {
    // API endpoint URL
    const api = 'PmrTrnPurchaseOrder/GetPurchaseOrderRpt';
    this.NgxSpinnerService.show()
    let param = {
      purchaseorder_gid:purchaseorder_gid
    } 
    this.service.getparams(api,param).subscribe((result: any) => {
      if(result!=null){
        this.service.filedownload1(result);
      }
      this.NgxSpinnerService.hide()
    });
  }
  WithoutPricePrintPDF(purchaseorder_gid: any) {
    // API endpoint URL
    const api = 'PmrTrnPurchaseOrder/GetPurchaserwithoutpricepdf';
    this.NgxSpinnerService.show()
    let param = {
      purchaseorder_gid:purchaseorder_gid
    } 
    this.service.getparams(api,param).subscribe((result: any) => {
      if(result!=null){
        this.service.filedownload1(result);
      }
      this.NgxSpinnerService.hide()
    });
  }

  GetPurchaseagreementOrderSummary(){
    var url = 'PmrTrnPurchaseagreement/GetPurchaseagreementOrderSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#purchaseagreementorder_list').DataTable().destroy();
      this.responsedata = result;
      this.purchaseagreementorder_list = this.responsedata.Getpurchaseagreementorder_list;
      console.log(this.purchaseagreementorder_list )
      setTimeout(() => {
        $('#purchaseagreementorder_list').DataTable();
      }, 1);
    });
    
  }

  onadd(){
    this.router.navigate(['/pmr/PmrTrnRasieAgreementorder']);
  }
  onaddselect(){
    
    this.router.navigate(['/pmr/PmrTrnPurchaseorderAddselect']);
  }
  onaddcontract(){
    this.router.navigate(['/pmr/PmrTrnContractvendor']);
  }
  onview(params:any){
    debugger
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.router.navigate(['/pmr/PmrTrnPurchaseagreementview',encryptedParam])
     
  }
  onedit(params:any){
    debugger
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.router.navigate(['/pmr/PmrTrnEditAgreementorder',encryptedParam])
     
  }
  
  RaisetoOrder(params: any){
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
     this.route.navigate(['/pmr/PmrTrnAgreementtoInvoice',encryptedParam]) 
  }
  Invoicetotag(params: any){
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
     this.route.navigate(['/pmr/PmrTrnAgreementtoinvoicetag',encryptedParam]) 
  }
  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  
  }

  ondelete() {
    debugger
 // API endpoint URL
  this.NgxSpinnerService.show()
    var url = 'PmrTrnPurchaseagreement/Getpurchaseagreementdelete'
    let param = {
      renewal_gid : this.parameterValue 
    }
    this.service.getparams(url,param).subscribe((result: any) => {
      if(result.status ==false){
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide();
      }
      else{
           
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
        window.location.reload();
    }
  });
}
}


