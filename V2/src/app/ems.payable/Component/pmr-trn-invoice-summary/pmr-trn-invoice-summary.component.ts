import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AES } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
@Component({
  selector: 'app-pmr-trn-invoice-summary',
  templateUrl: './pmr-trn-invoice-summary.component.html',
  styleUrls: ['./pmr-trn-invoice-summary.component.scss']
})
export class PmrTrnInvoiceSummaryComponent {
  invoice_list: any[] = [];
  responsedata: any;
  showOptionsDivId: any; 
  rows: any[] = [];
  parameterValue: any;

  constructor(public service :SocketService,private route:Router,
    public NgxSpinnerService:NgxSpinnerService,private ToastrService: ToastrService) {
    
    
  }
  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }
  ngOnInit(): void {
    this.GetInvoiceSummary();
  }

    GetInvoiceSummary(){
   
    var url='PmrTrnInvoice/GetInvoiceSummary'
    this.NgxSpinnerService.show();
    debugger
    this.service.get(url).subscribe((result:any)=>{
      this.NgxSpinnerService.hide();
      this.responsedata=result;
      this.invoice_list = this.responsedata.invoice_lista;  
      setTimeout(()=>{  
        $('#invoice_list').DataTable();
      }, 1);
    });
    document.addEventListener('click', (event: MouseEvent) => {
    if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
      this.showOptionsDivId = null;
    }
  });
  }
  
  onview(params:any){
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.route.navigate(['/payable/PmrTrnInvoiceview',encryptedParam]) 
  }

  onedit(params:any){
    debugger
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.route.navigate(['/payable/PmrTrnInvoiceEdit',encryptedParam])
  }

  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }

  // ondelete ()
  //   {
  //     this.NgxSpinnerService.show();
  //     var url = 'PmrTrnInvoice/GetInvoiceDelete'
  //     let param = {
  //       invoice_gid: this.parameterValue
  //     }
  //     this.service.getparams(url, param).subscribe((result: any) => {
  //       if (result.status == false) {
  //         window.scrollTo({
  //           top: 0, // Code is used for scroll top after event done
  //         }); 
  //         this.NgxSpinnerService.hide();
  //         this.ToastrService.warning(result.message)
  //         this.GetInvoiceSummary();
  //         this.NgxSpinnerService.hide();
          
  //       }
        
  //       else {
  //         window.scrollTo({
  //           top: 0, // Code is used for scroll top after event done
  //         });   
  //         this.NgxSpinnerService.hide();
  //         this.ToastrService.warning(result.message)
  //       }
  //       this.GetInvoiceSummary();
  //     });
  // }

  ondelete() {
    this.NgxSpinnerService.show();
    const url = 'PmrTrnInvoice/GetInvoiceDelete';
    const param = {
        invoice_gid: this.parameterValue
    };

    this.service.getparams(url, param).subscribe((result: any) => {
        this.NgxSpinnerService.hide();
        window.scrollTo({ top: 0 });  // Scroll to the top

        if (result.status) {
            this.ToastrService.success(result.message);  // Change to 'success' for a successful operation
        } else {
            this.ToastrService.warning(result.message);
        }

       
        this.GetInvoiceSummary();
    }, (error) => {
        this.NgxSpinnerService.hide();
        this.ToastrService.error("An error occurred while deleting the invoice.");
    });
}

 
 
}
