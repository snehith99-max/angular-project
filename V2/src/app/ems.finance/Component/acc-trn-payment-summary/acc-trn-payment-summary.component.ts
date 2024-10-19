// import { Component } from '@angular/core';

// @Component({
//   selector: 'app-acc-trn-payment-summary',
//   templateUrl: './acc-trn-payment-summary.component.html',
//   styleUrls: ['./acc-trn-payment-summary.component.scss']
// })
// export class AccTrnPaymentSummaryComponent {

// }
import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { environment } from 'src/environments/environment';
import { AES } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
interface IPaymentRpt {
 
}
@Component({
    selector: 'app-acc-trn-payment-summary',
    templateUrl: './acc-trn-payment-summary.component.html',
    styleUrls: ['./acc-trn-payment-summary.component.scss']
  })
  export class AccTrnPaymentSummaryComponent {


  responsedata: any;
  paymentrpt_list: any[] = [];
  PaymentRpt!: IPaymentRpt;
  company_code : any;
  parameterValue: any;
  invoicedetails:any;
  showOptionsDivId: any; 
  rows: any[] = [];
  constructor(private formBuilder: FormBuilder, private route: ActivatedRoute,private NgxSpinnerService:NgxSpinnerService, private router: Router, private ToastrService: ToastrService, public service: SocketService) {
    this.PaymentRpt = {} as IPaymentRpt;
  }
    
  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }
  
  ngOnInit(): void {
    //// Summary Grid//////

    var url = 'AccTrnPayment/GetPaymentRptSummary'
    this.service.get(url).subscribe((result: any) => {
   
      this.responsedata = result;
      this.paymentrpt_list = this.responsedata.paymentrptlist;
      //console.log(this.paymentrptlist)
      setTimeout(() => {
        $('#paymentrpt_list').DataTable();
      }, 1);
   
   
    });
    
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
   
    }
    addpayment(){
      this.router.navigate(['/finance/AccTrnAddPayment'])
    }
    openModaldetail(){
      
    }

    openModalcancel(){

    }
    openModalapproval(){

    }
    openModalprint(){

    }
    opendetails(parameter: string){
      debugger;
      this.parameterValue = parameter
      //this.reactiveForm.get("invoice_gid")?.setValue(this.parameterValue.invoice_gid);
      console.log(this.parameterValue)
      const invoicegid = this.parameterValue;
      this.getInvoicedetails(invoicegid);
    }
    
    onview(params:any){
      debugger
      const secretKey = 'storyboarderp';
      const param = (params);
      const encryptedParam = AES.encrypt(param,secretKey).toString();
      this.router.navigate(['/finance/AccTrnPaymentview',encryptedParam])         
      }
      PrintPDF(payment_gid: any) {
        // API endpoint URL
        const api = 'PblTrnPaymentRpt/GetPaymentrpt';
        this.NgxSpinnerService.show()
        let param = {
          payment_gid:payment_gid
        } 
        this.service.getparams(api,param).subscribe((result: any) => {
          if(result!=null){
            this.service.filedownload1(result);
          }
          this.NgxSpinnerService.hide()
        });
      }
    
    openModalpayment(payment_gid:string){
      debugger
      this.company_code = localStorage.getItem('c_code')
    window.location.href = "http://" + environment.host + "/Print/EMS_print/pbl_trn_paymentsummaryrpt.aspx?payment_gid=" + payment_gid + "&companycode=" + this.company_code
    }
    openModalword(payment_gid:string){
      debugger
      this.company_code = localStorage.getItem('c_code')
    window.location.href = "http://" + environment.host + "/Print/EMS_print/pbl_crp_txtilepaymentreceiptpdfword.aspx?payment_gid=" + payment_gid + "&companycode=" + this.company_code
    }
    getInvoicedetails(invoice_gid:any){
      debugger
      var url = 'PblTrnPaymentRpt/GetInvoicedetails'
      let param = {
        invoice_gid : invoice_gid 
          }
      this.service.getparams(url, param).subscribe((result: any) => {
        this.responsedata = result;
        this.invoicedetails = this.responsedata.getinvoice;
     //this.invoicedetails = result.getinvoice;
    
    
    });
    }
    oncancel(params:any){
      const secretKey = 'storyboarderp';
      const param = (params);
      const encryptedParam = AES.encrypt(param,secretKey).toString();
      this.router.navigate(['/payable/PblTrnPaymentCancel',encryptedParam]) 
    }
  }



  

