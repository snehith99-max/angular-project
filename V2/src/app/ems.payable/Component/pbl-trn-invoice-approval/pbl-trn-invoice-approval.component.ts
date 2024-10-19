import { Component, DebugEventListener, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { FormGroup } from '@angular/forms';
import { environment } from 'src/environments/environment';
import { NgxSpinnerService } from 'ngx-spinner';


@Component({
  selector: 'app-pbl-trn-invoice-approval',
  templateUrl: './pbl-trn-invoice-approval.component.html',
  styleUrls: ['./pbl-trn-invoice-approval.component.scss']
})
export class PblTrnInvoiceApprovalComponent {
  
  purchaseorder_list:any[]=[];
  responsedata: any;
  parameterValue1: any;
  company_code: any;
  showOptionsDivId: any; 
  rows: any[] = [];
 
  

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
    this.GetPurchaseOrderSummary();
    this.purchaseorder_list.sort((a,b) => {
      return new (b.created_date) - new (a.created_date); 
    });

    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
  }

  GetPurchaseOrderSummary(){
     this.NgxSpinnerService.show();
    var url = 'PmrTrnInvoice/Getinvoiceordersummary'
    this.service.get(url).subscribe((result: any) => {
      $('#purchaseorder_list').DataTable().destroy();
      this.responsedata = result;
      this.purchaseorder_list = this.responsedata.GetPurchaseOrder_lists;
      console.log(this.purchaseorder_list )
      setTimeout(() => {
        $('#purchaseorder_list').DataTable();
      }, 1);
  
      this.NgxSpinnerService.hide();
    });
  }

 
  onview(params:any){
    debugger
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
     this.router.navigate(['/payable/PblTrnInvoiceApprovalAdd',encryptedParam])
     
  }
  
}

