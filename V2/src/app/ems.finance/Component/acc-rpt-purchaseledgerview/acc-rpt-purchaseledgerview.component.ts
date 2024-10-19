import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { FormGroup,FormControl } from '@angular/forms';
import { AngularEditorConfig } from '@kolkov/angular-editor';
@Component({
  selector: 'app-acc-rpt-purchaseledgerview',
  templateUrl: './acc-rpt-purchaseledgerview.component.html',
  styleUrls: ['./acc-rpt-purchaseledgerview.component.scss']
})
export class AccRptPurchaseledgerviewComponent {
  invoice_lists: any;
  invoiceview: any;
  invoice_refno:any;
  invoice_date:any;
  invoice_gid:any;
  branch_name:any;
  vendorinvoiceref_no:any;
  vendor_companyname:any;
  vendor_address:any;
  total_amount:any;
  invoice_remarks:any;
  payment_days:any;
  netamount:any;
  product_total:any;
  payment_date:any;
  invoice_type:any;
  exchange_rate:any;
  currency_code:any;
  delivery_term: any;
  buybackorscrap:any;
  additional_name:any;
  addon_amount:any;
  extraadditional_amount:any;
  extradiscount_amount:any;
  grand_total : any;
  discount_name:any;
  invoiceProduct_list:any;
  Invoiceform!: FormGroup | any;
  termsandconditions:any;
  invoiceviewform : FormGroup | any;
  lspage:any;
  constructor(private router:ActivatedRoute,public service :SocketService , private route: Router,) { 

  }
  ngOnInit(): void {
    this.invoiceview= this.router.snapshot.paramMap.get('invoice_gid');
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.invoiceview,secretKey).toString(enc.Utf8);
    this.GetPmrTrnInvoiceview(deencryptedParam); 
    this.GetPmrTrnInvoiceviewproduct(deencryptedParam); 
  }
  GetPmrTrnInvoiceview(deencryptedParam: any) {
    var url='PurchaseLedger/GetPurchaselegderView'
    let param = {
      invoice_gid : deencryptedParam
    }
    debugger
    this.service.getparams(url,param).subscribe((result:any)=>{
    this.invoice_lists = result.GetPurchaseLedgerView_list;
  });
  this.invoiceviewform = new FormGroup({
    termsandconditions : new FormControl('')
  })
  }
  GetPmrTrnInvoiceviewproduct(deencryptedParam: any) {
    var url='PurchaseLedger/GetPurchaselegderProductView'
    let param = {
      invoice_gid : deencryptedParam
    }
    this.service.getparams(url,param).subscribe((result:any)=>{
    this.invoiceProduct_list = result.GetPurchaseLedgerProductView_list;
    this.grand_total = result.grand_total;
    this.grand_total = formatter.format(this.grand_total);
    function stripHtmlTags(html: string): string {
      const tempDiv = document.createElement('div');
      tempDiv.innerHTML = html;
      return tempDiv.textContent || tempDiv.innerText || '';
  }
    this.invoiceviewform.get("termsandconditions")?.setValue(this.invoiceProduct_list[0].termsandconditions)
const termsAndConditions = this.invoiceProduct_list[0]?.termsandconditions || ''; // Get the value from purchaseorder_list or use an empty string if it's undefined
const plainTextTerms = stripHtmlTags(termsAndConditions);
this.invoiceviewform.get('termsandconditions')?.setValue(plainTextTerms);  

  });
  const formatter = new Intl.NumberFormat('en-US', {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2
  });
  }
  onback(){
      this.route.navigate(['/finance/PmrTrnPurchaseLegderFin']);
     }
}
