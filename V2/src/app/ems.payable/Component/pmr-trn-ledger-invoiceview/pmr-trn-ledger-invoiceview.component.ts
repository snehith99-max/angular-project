import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { FormGroup,FormControl } from '@angular/forms';
import { AngularEditorConfig } from '@kolkov/angular-editor';
interface Iinvoiceview {
termsandconditions: string;
invoice_refno: string;
invoice_date: string;
due_date:string;
branch_name: string;
vendorinvoiceref_no: string;
vendor_companyname: string;
vendor_address: string;
invoice_remarks: string;
payment_days: string;
invoice_type: string;
currency_code: string;
delivery_term: string;
exchange_rate: string;
total_amount:string;
netamount:string;
product_total:string;
invoice_amount:string;
buybackorscrap:string;


}
@Component({
  selector: 'app-pmr-trn-ledger-invoiceview',
  templateUrl: './pmr-trn-ledger-invoiceview.component.html',
  styleUrls: ['./pmr-trn-ledger-invoiceview.component.scss']
})
export class PmrTrnLedgerInvoiceviewComponent {

  config: AngularEditorConfig = {
    editable: false,
    spellcheck: false,
    height: '33rem',
    minHeight: '0rem',
    placeholder: 'Enter text here...',
    translate: 'no',
   
  };


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
  round_off:any;
  invoice_amount:any;
  freightcharges:any;
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
        debugger
        
     
       
        this.invoiceview= this.router.snapshot.paramMap.get('invoice_gid');
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.invoiceview,secretKey).toString(enc.Utf8);
    this.lspage = this.router.snapshot.paramMap.get('lspage');
    this.lspage = this.lspage;
    const lspage = AES.decrypt(this.lspage, secretKey).toString(enc.Utf8);
    this.lspage = lspage;
    console.log(deencryptedParam)
    this.GetPmrTrnInvoiceview(deencryptedParam);
    this.GetPmrTrnInvoiceviewproduct(deencryptedParam);
      }
     
      GetPmrTrnInvoiceview(invoice_gid: any) {
        var url='PmrTrnInvoice/GetPmrTrnInvoiceview'
        debugger
        let param = {
          invoice_gid : invoice_gid
        }
        debugger
        this.service.getparams(url,param).subscribe((result:any)=>{
        this.invoice_lists = result.invoice_lists;
        //this.grand_total = result.grand_total;
        console.log(this.grand_total)
        //console.log(this.employeeedit_list)
       
    
      });
      this.invoiceviewform = new FormGroup({
        termsandconditions : new FormControl('')
      })
      // this.Invoiceform = new FormGroup({
      //   netamount: new FormControl(''),
      //   addon_amount: new FormControl(''),
      //   addon_charge: new FormControl(''),
      //   packing_charges: new FormControl(''),
      //   insurance_charges: new FormControl(''),
      //   roundoff: new FormControl(''),
      //   freightcharges: new FormControl(''),
      //   total_amount: new FormControl(''),
      //   overall_tax: new FormControl(''),
      //   discount_amount: new FormControl(''),
      // })
    
      }

      GetPmrTrnInvoiceviewproduct(invoice_gid: any) {
        var url='PmrTrnInvoice/GetPmrTrnInvoiceviewproduct'
        let param = {
          invoice_gid : invoice_gid
        }
        this.service.getparams(url,param).subscribe((result:any)=>{
        this.invoiceProduct_list = result.invoiceProduct_list;
        this.grand_total = result.grand_total;
        //console.log(this.employeeedit_list)
        
        
        // this.total_amount=this.invoiceProduct_list[0].total_amount;
        // this.invoice_amount=this.invoiceProduct_list[0].invoice_amount;
        // this.product_total=this.invoiceProduct_list[0].product_total;
        // this.buybackorscrap=this.invoiceProduct_list[0].buybackorscrap;
        // this.freightcharges=this.invoiceProduct_list[0].freightcharges;
        // this.round_off=this.invoiceProduct_list[0].round_off;
        // this.addon_amount=this.invoiceProduct_list[0].addon_amount;
        // this.additional_name=this.invoiceProduct_list[0].additional_name;
        // this.discount_name=this.invoiceProduct_list[0].discount_name;
        // this.extraadditional_amount=this.invoiceProduct_list[0].extraadditional_amount;
        // this.extradiscount_amount=this.invoiceProduct_list[0].extradiscount_amount;



        function stripHtmlTags(html: string): string {
          const tempDiv = document.createElement('div');
          tempDiv.innerHTML = html;
          return tempDiv.textContent || tempDiv.innerText || '';
      }
        this.invoiceviewform.get("termsandconditions")?.setValue(this.invoiceProduct_list[0].termsandconditions)
const termsAndConditions = this.invoiceProduct_list[0]?.termsandconditions || ''; // Get the value from purchaseorder_list or use an empty string if it's undefined
const plainTextTerms = stripHtmlTags(termsAndConditions);
this.invoiceviewform.get('termsandconditions')?.setValue(plainTextTerms);  
        
        
    
    // this.Invoiceform.get("totalamount")?.setValue(this.invoiceProduct_list[0].netamount);
    // this.Invoiceform.get("addon_amount")?.setValue(this.invoiceProduct_list[0].addon_amount);
    // this.Invoiceform.get("addon_charge")?.setValue(this.invoiceProduct_list[0].addon_charge);
    // this.Invoiceform.get("packing_charges")?.setValue(this.invoiceProduct_list[0].packing_charges);
    // this.Invoiceform.get("insurance_charges")?.setValue(this.invoiceProduct_list[0].insurance_charges);
    // this.Invoiceform.get("roundoff")?.setValue(this.invoiceProduct_list[0].roundoff);
    // this.Invoiceform.get("freightcharges")?.setValue(this.invoiceProduct_list[0].freightcharges);
    // this.Invoiceform.get("discount_amount")?.setValue(this.invoiceProduct_list[0].discount_amount);
    // this.Invoiceform.get("total_amount")?.setValue(this.invoiceProduct_list[0].total_amount);
    // this.Invoiceform.get("overall_tax")?.setValue(this.invoiceProduct_list[0].overall_tax);
    
      });
      
    
      }
      hasTaxData(data: any): boolean {
        return data.taxseg_taxname1 || data.taxseg_taxname2 || data.taxseg_taxname3;
      }
      get totalamount() {
        return this.Invoiceform.get('totalamount');
      }
      onback(){
        if(this.lspage == 'Finance'){
          this.route.navigate(['/finance/PmrTrnPurchaseLegderFin']);
         }
         else{
           this.route.navigate(['/pmr/PmrTrnPurchaseLedger']);  
         }
      }
      // get additional_name() {
      //   return this.Invoiceform.get('additional_name');
      // } 
      // get roundoff() {
      //   return this.Invoiceform.get('roundoff');
      // } 
      // get discount_name() {
      //   return this.Invoiceform.get('discount_name');
      // }
      // get freightcharges() {
      //   return this.Invoiceform.get('freightcharges');
      // } 
      // get addoncharge() {
      //   return this.Invoiceform.get('addoncharge');
      // }   
      // get insurance_charges() {
      //   return this.Invoiceform.get('insurance_charges');
      // } 
      // get addon_amount() {
      //   return this.Invoiceform.get('addon_amount');
      // } 
      // get packing_charges() {
      //   return this.Invoiceform.get('packing_charges');
      // } 

}
