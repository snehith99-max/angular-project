import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-pmr-trn-purchase-requisition',
  templateUrl: './pmr-trn-purchase-requisition.component.html',
  styleUrls: ['./pmr-trn-purchase-requisition.component.scss'],
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
export class PmrTrnPurchaseRequisitionComponent {

  purchaserequest_list: any [] = [];
  responsedata: any;
  quotation_gid: any;
  parameterValue1: any;
  purchaserequisition_gid:any;
  productlistdetailspr:any;
  showOptionsDivId: any; 
  rows: any[] = [];

  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService, private route: ActivatedRoute,private router: Router,private NgxSpinnerService:NgxSpinnerService) {
  }

  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }

  ngOnInit(): void {
    this.GetPurchaseRequisitionSummary();
    
document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
  }

  GetPurchaseRequisitionSummary(){
    this.NgxSpinnerService.show();
    var url = 'PmrTrnPurchaseRequisition/GetPmrTrnPurchaseRequisition'
    this.service.get(url).subscribe((result: any) => {
     $('#purchaserequest_list').DataTable().destroy();
      this.responsedata = result;
      this.purchaserequest_list = this.responsedata.purchaserequest_list;
      //console.log(this.entity_list)
      setTimeout(() => {
        $('#purchaserequest_list').DataTable()
      }, 1);
  
      this.NgxSpinnerService.hide();
    });
  
 }
 onadd()
 {
  this.router.navigate(['/pmr/PmrTrnRaiseRequisition'])
 }


  openModaledit()
  {

  }

  openModaladd()
  {

  }

  onview(params:any)
  {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.router.navigate(['/pmr/PmrTrnPurchaseRequisitionView',encryptedParam]) 
  }
  
  PrintPDF(purchaserequisition_gid: any) {
    // API endpoint URL
    const api = 'PmrTrnPurchaseRequisition/GetPurchaseRequisitionRpt';
    this.NgxSpinnerService.show()
    let param = {
      purchaserequisition_gid:purchaserequisition_gid
    } 
    this.service.getparams(api,param).subscribe((result: any) => {
      if(result!=null){
        this.service.filedownload1(result);
      }
      this.NgxSpinnerService.hide()
    });
  }

  openModaldelete()
  {
    
  }
  Details(parameter: string,purchaserequisition_gid: string){
    this.parameterValue1 = parameter;
    this.purchaserequisition_gid = parameter;
  
    var url='PmrTrnPurchaseRequisition/GetProductdetails'
      let param = {
        purchaserequisition_gid : purchaserequisition_gid 
      }
      this.service.getparams(url,param).subscribe((result:any)=>{
      this.responsedata=result;
       this.productlistdetailspr = result.productlistdetailspr;   
      });
    
  }

}
