import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { AES, enc } from 'crypto-js';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { NgxSpinnerService } from 'ngx-spinner';
interface SalesOD{
  salesorder_gid: string;
  salesorder_date: string;
  branch_name: string;
  branch_gid: string;
  so_referencenumber: string;
  leadbank_name: string;
  leadbank_gid: string;
  customercontact_names: string;
  customercontact_gid: string;
  customer_mobile: string;
  customer_email: string;
  so_remarks: string;
  customer_address: string;
  shipping_to: string;
  user_name: string;
  user_gid: string;
  start_date: string;
  end_date: string;
  freight_terms: string;
  payment_terms: string;
  currencyexchange_gid: string;
  currency_code: string;
  exchange_rate: string;
  product_name: string;
  product_gid: string;
  productgroup_name: string;
  customerproduct_code: string;
  product_code: string;
  productuom: string;
  product_price: string;
  qty_quoted: string;
  margin_percentage: string;
  margin_amount: string;
  selling_price: string;
  product_requireddate: string;
  product_requireddateremarks: string;
  tax_name: string;
  tax_gid: string;
  tax_amount: string;
  tax_name2: string;
  tax_amount2: string;
  tax_name3: string;
  tax_amount3: string;
  price: string;
}

@Component({
  selector: 'app-crm-trn-traiseorder',
  templateUrl: './crm-trn-traiseorder.component.html',
  styleUrls: ['./crm-trn-traiseorder.component.scss']
})
export class CrmTrnTraiseorderComponent {
  
  config: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '38rem',
    minHeight: '5rem',
    width: '925px',
    placeholder: 'Enter text here...',
    translate: 'no',
    defaultParagraphSeparator: 'p',
    defaultFontName: 'Arial',
  };
  combinedFormData: FormGroup | any;
  productform: FormGroup | any;
  customer_list: any ;
  branch_list: any [] = [];
  contact_list: any [] = [];
  currency_list: any [] = [];
  allchargeslist: any[] = [];
  user_list:any [] = [];
  product_list: any [] = [];
  tax_list: any [] = [];
  tax2_list: any [] = [];
  tax3_list: any [] = [];
  tax4_list: any [] = [];
  calci_list: any [] = [];
  POproductlist: any [] = [];
  terms_list: any[] = [];
  mdlBranchName:any;
  Salesorderdetail_list:any [] = [];
  directeditsalesorder_listsocrm: any [] = [];
  GetCustomerDetSOCRM:any;
  mdlCustomerName:any;
  mdlUserName:any;
  mdlProductName:any;
  mdlTaxName3:any;
  mdlCurrencyName:any;
  mdlTaxName2:any;
  mdlTaxName1:any;
  mdlunitprice:any;
  GetproductsCode:any;
  mdlContactName:any;
  unitprice: number = 0;
  quantity: number = 0;
  discountpercentage: number = 0;
  discountamount: any;
  totalamount: any;
  addon_charge: number = 0;
  POdiscountamount: number = 0;
  freight_charges: number = 0;
  forwardingCharges: number = 0;
  insurance_charges: number = 0;
  roundoff: number = 0;
  packing_charges: number = 0;
  grandtotal: any;
  tax_amount: number = 0;
  taxamount: number = 0;
  buyback_charges: number =0;
  tax_amount4: any;
  taxpercentage: any;
  productdetails_list: any;
  tax_amount2: number = 0;
  tax_amount3: number = 0;
  producttotalamount: any;
  CurrencyName:any;
  parameterValue: string | undefined; 
  productnamelist: any;
  selectedCurrencyCode: any;
  POadd_list: any;
  total_amount: any;
  mdlTerms:any;
  additional_discount: number=0;
  total_price:any;
  mdlproductName: any;
  txtcontactnumber :any;
  responsedata: any;
  ExchangeRate: any;
  salesOD!: SalesOD;
  Productsummarys_list: any;
  salesorders_list: any;
  mdlTaxName4:any;
  cuscontact_gid: any;
  salesorder: any;
  exchange: any;
  leadbank_gid: any;
  leadbankcontact_gid: any;
  lead2campaign_gid: any;
  lspage: any;
  txtemail :any;
  txtaddress :any;
  txtcontactrperson :any;
  mdlProductUom :any;
  mdlProductCode :any;
  lspage1: any;
  showUpdateButton: boolean = false;
  showAddButton: boolean = true;
  constructor(private http:HttpClient, public NgxSpinnerService:NgxSpinnerService, private fb: FormBuilder, private router: ActivatedRoute, private route: Router, private service: SocketService, private ToastrService: ToastrService) {
  this.salesOD = {} as SalesOD
  this.combinedFormData = new FormGroup ({
    tmpsalesorderdtl_gid: new FormControl(''),
    salesorder_gid : new FormControl (''),
    salesorder_date:new FormControl (this.getCurrentDate()),
    branch_name: new FormControl(''),
    branch_gid: new FormControl(''),
    so_referencenumber: new FormControl(''),
    customer_name: new FormControl(''),
    customer_gid: new FormControl(''),
    customercontact_names: new FormControl(''),
    customercontact_gid: new FormControl(''),
    customer_mobile: new FormControl(''),
    customer_email: new FormControl(''),
    so_remarks: new FormControl(''),
    customer_address: new FormControl(''),
    shipping_to: new FormControl(''),
    user_name: new FormControl(''),
    user_gid: new FormControl(''),
    start_date: new FormControl(''),
    end_date: new FormControl(''),
    freight_terms: new FormControl(''),
    payment_terms: new FormControl(''),
    currencyexchange_gid: new FormControl(''),
    currency_code: new FormControl(''),
    exchange_rate: new FormControl(''),
    payment_days: new FormControl(''),
    delivery_days: new FormControl(''),
        total_price: new FormControl(''),
        tax_name4: new FormControl(''),
        tax4_gid: new FormControl(''),
        producttotalamount: new FormControl(''),
        txttaxamount_1: new FormControl(''),
        addon_charge: new FormControl(''),
        additional_discount: new FormControl(''),
        freight_charges: new FormControl(''),
        buyback_charges: new FormControl(''),
        insurance_charges: new FormControl(''),
        roundoff: new FormControl(''),
        grandtotal: new FormControl(''),
        packing_charges: new FormControl(''),
        termsandconditions: new FormControl(''),
        template_gid: new FormControl(''),
        template_name: new FormControl(''),
        tax_amount4:new FormControl(''),
        totalamount:new FormControl(''),
        total_amount : new FormControl('')
        
  

});
this.productform = new FormGroup({
tmpsalesorderdtl_gid: new FormControl(''),
tax_gid:new FormControl(''),
product_gid: new FormControl(''),
productuom_gid: new FormControl(''),
productgroup_gid: new FormControl(''),
product_code: new FormControl(''),
tax_name: new FormControl(''),
remarks: new FormControl(''),
product_name: new FormControl(''),
productuom_name: new FormControl(''),
productgroup_name: new FormControl('', Validators.required),
unitprice: new FormControl('', Validators.required),
quantity: new FormControl('', Validators.required),
discount_percentage: new FormControl('', Validators.required),
discountamount: new FormControl('', Validators.required),
taxname1: new FormControl('', Validators.required),
tax_amount: new FormControl('', Validators.required),
taxname2: new FormControl('', Validators.required),
tax_amount2: new FormControl('', Validators.required),
taxname3: new FormControl('', Validators.required),
tax_amount3: new FormControl('', Validators.required),
totalamount: new FormControl('', Validators.required),
selling_price: new FormControl(''),
product_requireddate: new FormControl(''),
product_requireddateremarks: new FormControl(''),
});

}
    ngOnInit(): void {

      var api = 'SmrMstSalesConfig/GetAllChargesConfig';
     this.service.get(api).subscribe((result: any) => {
       this.responsedata = result;
       this.allchargeslist = this.responsedata.salesconfigalllist;
       this.addon_charge = this.allchargeslist[0].flag;
       this.additional_discount = this.allchargeslist[1].flag;
       this.freight_charges = this.allchargeslist[2].flag;
       this.buyback_charges = this.allchargeslist[3].flag;
       this.insurance_charges = this.allchargeslist[4].flag;
       
 
       if (this.allchargeslist[0].flag == 'Y') {
         this.addon_charge = 0;
       } else {
         this.addon_charge = this.allchargeslist[0].flag;
       }
 
       if (this.allchargeslist[1].flag == 'Y') {
         this.additional_discount = 0;
       } else {
         this.additional_discount = this.allchargeslist[1].flag;
       }
 
       if (this.allchargeslist[2].flag == 'Y') {
         this.freight_charges = 0;
       } else {
         this.freight_charges = this.allchargeslist[2].flag;
       }
 
       if (this.allchargeslist[3].flag == 'Y') {
         this.buyback_charges = 0;
       } else {
         this.buyback_charges = this.allchargeslist[3].flag;
       }
 
       if (this.allchargeslist[4].flag == 'Y') {
         this.insurance_charges = 0;
       } else {
         this.insurance_charges = this.allchargeslist[4].flag;
       }
     
     });
        const options: Options = {
          dateFormat: 'd-m-Y',    
        };
        flatpickr('.date-picker', options); 
  
      this.salesorederSummary();
      this.combinedFormData = new FormGroup ({
        tmpsalesorderdtl_gid: new FormControl(''),
        salesorder_gid : new FormControl (''),
        salesorder_date:new FormControl (this.getCurrentDate()),
        branch_name: new FormControl(''),
        branch_gid: new FormControl(''),
        so_referencenumber: new FormControl(''),
        customer_name: new FormControl(''),
        customer_gid: new FormControl(''),
        customercontact_names: new FormControl(''),
        customercontact_gid: new FormControl(''),
        customer_mobile: new FormControl(''),
        customer_email: new FormControl(''),
        so_remarks: new FormControl(''),
        customer_address: new FormControl(''),
        shipping_to: new FormControl(''),
        user_name: new FormControl(''),
        user_gid: new FormControl(''),
        start_date: new FormControl(''),
        end_date: new FormControl(''),
        freight_terms: new FormControl(''),
        payment_terms: new FormControl(''),
        currencyexchange_gid: new FormControl(''),
        currency_code: new FormControl(''),
        exchange_rate: new FormControl(''),
        payment_days: new FormControl(''),
        delivery_days: new FormControl(''),
            total_price: new FormControl(''),
            tax_name4: new FormControl(''),
            tax4_gid: new FormControl(''),
            producttotalamount: new FormControl(''),
            txttaxamount_1: new FormControl(''),
            addon_charge: new FormControl(''),
            additional_discount: new FormControl(''),
            freight_charges: new FormControl(''),
            buyback_charges: new FormControl(''),
            insurance_charges: new FormControl(''),
            roundoff: new FormControl(''),
            grandtotal: new FormControl(''),
            packing_charges: new FormControl(''),
            termsandconditions: new FormControl(''),
            template_gid: new FormControl(''),
            template_name: new FormControl(''),
            tax_amount4:new FormControl(''),
            totalamount:new FormControl(''),
            total_amount : new FormControl('')
            
      

});
this.productform = new FormGroup({
  tmpsalesorderdtl_gid: new FormControl(''),
  tax_gid:new FormControl(''),
  product_gid: new FormControl(''),
  productuom_gid: new FormControl(''),
  productgroup_gid: new FormControl(''),
  product_code: new FormControl(''),
  tax_name: new FormControl(''),
  remarks: new FormControl(''),
  product_name: new FormControl(''),
  productuom_name: new FormControl(''),
  productgroup_name: new FormControl('', Validators.required),
  unitprice: new FormControl('', Validators.required),
  quantity: new FormControl('', Validators.required),
  discount_percentage: new FormControl('', Validators.required),
  discountamount: new FormControl('', Validators.required),
  taxname1: new FormControl('', Validators.required),
  tax_amount: new FormControl('', Validators.required),
  taxname2: new FormControl('', Validators.required),
  tax_amount2: new FormControl('', Validators.required),
  taxname3: new FormControl('', Validators.required),
  tax_amount3: new FormControl('', Validators.required),
  totalamount: new FormControl('', Validators.required),
  selling_price: new FormControl(''),
  product_requireddate: new FormControl(''),
  product_requireddateremarks: new FormControl(''),
});

//// Branch Dropdown /////
var url = 'SmrSalesOrder360/GetBranchDtlSOCRM'
this.service.get(url).subscribe((result:any)=>{
  this.branch_list = result.GetBranchSOCRM;
  const firstBranch = this.branch_list[0];
  const branchName = firstBranch.branch_gid;
  this.combinedFormData.get('branch_name')?.setValue(branchName);
 });

/// Customer Name Dropdown ////
const leadbank_gid = this.router.snapshot.paramMap.get('leadbank_gid');
const leadbankcontact_gid = this.router.snapshot.paramMap.get('leadbankcontact_gid');
const lead2campaign_gid = this.router.snapshot.paramMap.get('lead2campaign_gid');
const lspage = this.router.snapshot.paramMap.get('lspage');


this.leadbank_gid = leadbank_gid;
this.leadbankcontact_gid = leadbankcontact_gid;
this.lead2campaign_gid = lead2campaign_gid;
this.lspage = lspage;

const secretKey = 'storyboarderp';
const deencryptedParam = AES.decrypt(this.leadbank_gid, secretKey).toString(enc.Utf8);
const deencryptedParam1 = AES.decrypt(this.leadbankcontact_gid, secretKey).toString(enc.Utf8);
const deencryptedParam2 = AES.decrypt(this.lead2campaign_gid, secretKey).toString(enc.Utf8);
const deencryptedParam3 = AES.decrypt(this.lspage,secretKey).toString(enc.Utf8);

this.lspage1 = deencryptedParam3;

console.log("leadbank_gid ="+ deencryptedParam);
console.log("leadbankcontact_gid = "+deencryptedParam1);
console.log("lead2campaign_gid = "+deencryptedParam2);
if (deencryptedParam != null) {
  this.leadbank_gid = (deencryptedParam);
}

var url = 'SmrSalesOrder360/GetCustomerDtlCRM'
let params = {
  leadbank_gid: deencryptedParam,
}
this.service.getparams(url, params).subscribe((result:any)=>{
  this.customer_list = result.GetCustomerSOCRM;
 });

 //// Sales person Dropdown ////
 var url = 'SmrSalesOrder360/GetPersonDtlCRM'
this.service.get(url).subscribe((result:any)=>{
  this.contact_list = result.GetPersonSOCRM;
 });

  //// Currency Dropdown ////
  var url = 'SmrTrnSalesorder/GetCurrencyDtl'
  this.service.get(url).subscribe((result:any)=>{
    this.currency_list = result.GetCurrencyDtl;
   });

   //// Tax 1 Dropdown ////
   var url = 'SmrSalesOrder360/GetTax1DtlCRM'
   this.service.get(url).subscribe((result:any)=>{
     this.tax_list = result.GetTax1SOCRM;
    });

 

      //// Tax 3 Dropdown ////
   var url = 'SmrSalesOrder360/GetTax4DtlSOCRM'
   this.service.get(url).subscribe((result:any)=>{
     this.tax4_list = result.GetTax4SOCRM;
    });

      //// Product Dropdown ////
   var url = 'SmrSalesOrder360/GetProductNamDtlCRM'
   this.service.get(url).subscribe((result:any)=>{
     this.product_list = result.GetProductNameSOCRM;
    });

    // Termd and Conditions //
    //// T & C Dropdown ////
  var url = 'SmrSalesOrder360/GetTermsandConditionsSOCRM'
  this.service.get(url).subscribe((result:any)=>{
    this.terms_list = result.GetTermsandConditionsSOCRM;
   });
}

getCurrentDate(): string {
  const today = new Date();
  const dd = String(today.getDate()).padStart(2, '0');
  const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
  const yyyy = today.getFullYear();
 
  return dd + '-' + mm + '-' + yyyy;
}
get salesorder_date() {
  return this.combinedFormData.get('salesorder_date')!;
}
  get branch_name() {
    return this.combinedFormData.get('branch_name')!;
  }
  get customer_name() {
    return this.combinedFormData.get('customer_name')!;
  }
  get customercontact_names() {
    return this.combinedFormData.get('customercontact_names')!;
  }
  get user_name() {
    return this.combinedFormData.get('user_name')!;
  }
  get currency_code() {
    return this.combinedFormData.get('currency_code')!;
  }
  get product_name() {
    return this.productform.get('product_name')!;
  }
  get tax_name(){
    return this.productform.get('tax_name')!;
  }
  get tax_name2(){
    return this.productform.get('tax_name2')!;
  }
  get tax_name3(){
    return this.productform.get('tax_name3')!;
  }

  OnChangeCustomer(){
    
    debugger
    let customer_gid = this.combinedFormData.value.customer_name.customer_gid;
    let param ={
      customer_gid :customer_gid
    }
    var url = 'SmrSalesOrder360/GetCustomerOnchangeCRM';
    
    this.service.getparams(url, param).subscribe((result: any) => {
      
      this.responsedata = result;
      this.GetCustomerDetSOCRM = this.responsedata.GetCustomerdetSOCRM;
      
      this.combinedFormData.get("customer_mobile")?.setValue(result.GetCustomerdetSOCRM[0].customer_mobile);
      
      this.combinedFormData.get("customercontact_names")?.setValue(result.GetCustomerdetSOCRM[0].customercontact_names);
      this.combinedFormData.get("customer_address")?.setValue(result.GetCustomerdetSOCRM[0].customer_address);
      this.combinedFormData.value.customer_gid = result.GetCustomerdetSOCRM[0].customer_gid;
      this.combinedFormData.get("customer_email")?.setValue(result.GetCustomerdetSOCRM[0].customer_email);
      //this.cuscontact_gid =this.combinedFormData.value.customercontact_gid;
      this.combinedFormData.value.leadbank_gid = result.GetCustomerdetSOCRM[0].leadbank_gid
    });

  }
  OnClearTax() {
     
    this.tax_amount = 0; 
    const subtotal = this.exchange *  this.unitprice * this.quantity;
    this.totalamount = (+(subtotal - this.tax_amount));
    this.totalamount = +((this.totalamount).toFixed(2))
  }
  onClearCustomer() {
 
    this.txtcontactnumber = '';
    this.txtemail='';
    this.txtaddress='';
    this.txtcontactrperson='';
  }
  OnChangeCurrency() {
    
    let currencyexchange_gid = this.combinedFormData.get("currency_code")?.value;
    console.log(currencyexchange_gid)
    let param = {
      currencyexchange_gid: currencyexchange_gid
    }
    var url = 'SmrSalesOrder360/GetOnChangeCurrencySOCRM';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.currency_list = this.responsedata.GetOnchangecurrencySOCRM;
      this.combinedFormData.get("exchange_rate")?.setValue(this.currency_list[0].exchange_rate);
    });
  }
  
  onCurrencyCodeChange(event: Event){

  }

  OnClearOverallTax() {
    this.tax_amount4 = '';
    this.total_price ='';
      this.grandtotal = ((this.producttotalamount) + (+this.addon_charge) + (+this.freight_charges) - (+this.buyback_charges) + (+this.packing_charges) + (+this.insurance_charges) + (+this.roundoff)  - (+this.additional_discount));
      this.grandtotal = ((this.grandtotal).toFixed(2));
  }
  onClearProduct()
  {
    this.mdlProductCode='';
    this.mdlProductUom='';
    this.mdlunitprice='';
  }

  GetOnChangeProductsName(){
    
    let product_gid = this.productform.value.product_name.product_gid;
    if (this.combinedFormData.value.customer_name != undefined) {
      let customercontact_gid = this.combinedFormData.value.customer_name.customer_gid;
      let param = {
        product_gid: product_gid,
        customercontact_gid: customercontact_gid
      }
    var url = 'SmrSalesOrder360/GetOnChangeProductsNameCRM';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.GetproductsCode = this.responsedata.ProductsCodesSOCRM;
      this.productform.get("product_code")?.setValue(result.ProductsCodesSOCRM[0].product_code);
      this.productform.get("productuom_name")?.setValue(result.ProductsCodesSOCRM[0].productuom_name);
      this.productform.get("productgroup_name")?.setValue(result.ProductsCodesSOCRM[0].productgroup_name);
      this.productform.get("unitprice")?.setValue(result.ProductsCodesSOCRM[0].unitprice);
      this.productform.value.productgroup_gid = result.ProductsCodesSOCRM[0].productgroup_gid
      // this.productform.value.productuom_gid = result.GetProductsCode[0].productuom_gid
    });
  }
  else if (this.productform.value.product_name.product_gid) {
    
    let param = {
      product_gid: product_gid,

    }
    var url = 'SmrSalesOrder360/GetOnChangeProductsNamesCRM';
    this.NgxSpinnerService.show()
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.GetproductsCode = this.responsedata.ProductsCodesSOCRM;
      this.productform.get("product_code")?.setValue(result.ProductsCodesSOCRM[0].product_code);
      this.productform.get("productuom_name")?.setValue(result.ProductsCodesSOCRM[0].productuom_name);
      this.productform.get("productgroup_name")?.setValue(result.ProductsCodesSOCRM[0].productgroup_name);
      this.productform.get("unitprice")?.setValue(result.ProductsCodesSOCRM[0].unitprice);
      this.productform.value.productgroup_gid = result.ProductsCodesSOCRM[0].productgroup_gid
      this.NgxSpinnerService.hide()
    });

  }
  else {
    this.ToastrService.warning('Kindly Select Customer Before Adding Product !! ')
  }

  }
 
productAdd(){
    
console.log(this.productform.value)
var params = {
  productgroup_name: this.productform.value.productgroup_name,
  customerproduct_code: this.productform.value.customerproduct_code,
  product_code: this.productform.value.product_code,
  product_name: this.productform.value.product_name.product_name,
  productuom_name: this.productform.value.productuom_name,
  qty_requested: this.productform.value.qty_requested,
  potential_value: this.productform.value.potential_value,
  product_requireddate: this.productform.value.product_requireddate,
  product_gid: this.productform.value.product_name.product_gid,
  productgroup_gid: this.productform.value.productgroup_gid,
  productuom_gid: this.productform.value.productuom_gid,
  //selling_price: this.productform.value.selling_price,
  unitprice: this.productform.value.unitprice,
  quantity: this.productform.value.quantity,
  discount_percentage: this.productform.value.discount_percentage,
  discountamount: this.productform.value.discountamount,
  product_requireddateremarks: this.productform.value.product_requireddateremarks,
  tax_name: this.productform.value.tax_name,
  tax_amount: this.productform.value.tax_amount,  
  totalamount: this.productform.value.totalamount,
  producttotalamount:this.productform.value.producttotalamount,
}
    var api = 'SmrSalesOrder360/PostOnAdds';
    this.NgxSpinnerService.show()
    this.service.post(api, params).subscribe((result: any) => {
      if(result.status == false){
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide()
        this.salesorederSummary();
      }
      else{
        this.ToastrService.success(result.message)
        this.salesorederSummary();
        this.productform.reset();
        this.NgxSpinnerService.hide()
      }
    },
    );
  }
  salesorederSummary() {
  
    var api = 'SmrSalesOrder360/GetSalesOrdersummary';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.salesorders_list = this.responsedata.salesorderslistSOCRM;
      this.combinedFormData.get("totalamount")?.setValue(result.grand_total);
      if(this.mdlTaxName4 == null || this.mdlTaxName4 == "" || this.mdlTaxName4 == "--No Tax--")
      {
        this.combinedFormData.get("grandtotal")?.setValue(result.grand_total);
      }
      else
      {
        let selectedTax = this.tax4_list.find(tax => tax.tax_name4 === this.mdlTaxName4);
      this.taxpercentage = this.getDimensionsByFilter(selectedTax.tax_gid);
      let tax_percentage = this.taxpercentage[0].percentage;
      this.tax_amount4 = +(((tax_percentage * result.grand_total / 100)).toFixed(2));
      const taxamount = +(((result.grand_total + this.tax_amount4)).toFixed(2));
      const newGrandTotal = result.grand_total + parseFloat(this.tax_amount4);      
      const newGrandTotal2 = newGrandTotal + parseFloat(this.combinedFormData.value.addon_charge);
      const newGrandTotal3 = newGrandTotal2 - parseFloat(this.combinedFormData.value.additional_discount);
      const newGrandTotal4 = newGrandTotal3 + parseFloat(this.combinedFormData.value.freight_charges);
      const newGrandTotal5 = newGrandTotal4 - parseFloat(this.combinedFormData.value.buyback_charges);
      const newGrandTotal6 = newGrandTotal5 + parseFloat(this.combinedFormData.value.packing_charges);
      const newGrandTotal7 = newGrandTotal6 + parseFloat(this.combinedFormData.value.insurance_charges);
      const newGrandTotal8 = newGrandTotal7 + parseFloat(this.combinedFormData.value.roundoff);
      this.combinedFormData.get("tax_amount4")?.setValue(this.tax_amount4);
      this.combinedFormData.get("total_price")?.setValue(taxamount);
      this.combinedFormData.get("grandtotal")?.setValue(newGrandTotal8);
      }

      
      
    });
  }

  GetOnChangeTerms() {
    

    let template_gid = this.combinedFormData.value.template_name;
    let param = {
      template_gid: template_gid
    }
    var url = 'SmrSalesOrder360/GetOnChangeTerms';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.combinedFormData.get("termsandconditions")?.setValue(result.terms_listSOCRM[0].termsandconditions);
      this.combinedFormData.value.template_gid = result.terms_listSOCRM[0].template_gid
      //this.cusraiseform.value.productuom_gid = result.GetProductsName[0].productuom_gid
    });
  }


  onSubmit()  
    {
      debugger
      console.log(this.combinedFormData.value)
      var params = {
        branch_name:this.combinedFormData.value.branch_name,
        branch_gid: this.combinedFormData.value.branch_name.branch_gid,
        salesorder_date:this.combinedFormData.value.salesorder_date,
        salesorder_gid:this.combinedFormData.value.salesorder_gid,
        customer_name: this.combinedFormData.value.customer_name.customer_name,
        customercontact_names: this.combinedFormData.value.customercontact_names,
        customer_email: this.combinedFormData.value.customer_email,
        customer_mobile: this.combinedFormData.value.customer_mobile,
        so_remarks: this.combinedFormData.value.so_remarks,
        so_referencenumber: this.combinedFormData.value.so_referencenumber,
        customer_address: this.combinedFormData.value.customer_address,
        freight_terms: this.combinedFormData.value.freight_terms,
        payment_terms : this.combinedFormData.value.payment_terms,
        currency_code: this.combinedFormData.value.currency_code,
        user_name: this.combinedFormData.value.user_name,
        exchange_rate: this.combinedFormData.value.exchange_rate,
        payment_days: this.combinedFormData.value.payment_days,
        customer_gid:this.combinedFormData.value.customer_name.customer_gid,
        termsandconditions:this.combinedFormData.value.termsandconditions,
        template_name:this.combinedFormData.value.template_name,
        template_gid:this.combinedFormData.value.template_gid,
        grandtotal:this.combinedFormData.value.grandtotal,
        roundoff:this.combinedFormData.value.roundoff,
        start_date:this.combinedFormData.value.start_date,
        end_date:this.combinedFormData.value.end_date,
        insurance_charges:this.combinedFormData.value.insurance_charges,
        packing_charges:this.combinedFormData.value.packing_charges,
        buyback_charges:this.combinedFormData.value.buyback_charges,
        freight_charges:this.combinedFormData.value.freight_charges,
        additional_discount:this.combinedFormData.value.additional_discount,
        addon_charge:this.combinedFormData.value.addon_charge,
        total_amount:this.combinedFormData.value.total_amount,
        tax_amount4:this.combinedFormData.value.tax_amount4,
        tax_name4:this.combinedFormData.value.tax_name4,
        totalamount:this.combinedFormData.value.totalamount,
        customercontact_gid:this.cuscontact_gid,
        delivery_days: this.combinedFormData.value.delivery_days,
        shipping_to: this.combinedFormData.value.shipping_to
      }
      
      //console.log(this.cuscontact_gid)
      var url='SmrSalesOrder360/PostSalesOrder'
      this.NgxSpinnerService.show()
      this.service.postparams(url, params).subscribe((result: any) => {
        if(result.status == false){
          this.ToastrService.warning(result.message)    
          this.NgxSpinnerService.hide()    
        }
        else{
          this.ToastrService.success(result.message)
          const secretKey = 'storyboarderp';
          const leadbank_gid = AES.encrypt(this.leadbank_gid, secretKey).toString();
       
         if (this.lspage1 == 'MM-Total') {
           this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,this.lead2campaign_gid,this.lspage ]);
           this.NgxSpinnerService.hide()
         }
         else if (this.lspage1 == 'MM-Upcoming') {
           this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,this.lead2campaign_gid,this.lspage ]);
           this.NgxSpinnerService.hide()
         }
         else if (this.lspage1 == 'MM-Lapsed') {
           this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,this.lead2campaign_gid,this.lspage ]);
           this.NgxSpinnerService.hide()
         }
         else if (this.lspage1 == 'MM-Longest') {
           this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,this.lead2campaign_gid,this.lspage ]);
           this.NgxSpinnerService.hide()
         }
         else if (this.lspage1 == 'MM-New') {
           this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,this.lead2campaign_gid,this.lspage ]);
           this.NgxSpinnerService.hide()
         }
         else if (this.lspage1 == 'MM-Prospect') {
           this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,this.lead2campaign_gid,this.lspage ]);
           this.NgxSpinnerService.hide()
         }
         else if (this.lspage1 == 'MM-Potential') {
           this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,this.lead2campaign_gid,this.lspage ]);
           this.NgxSpinnerService.hide()
         }
         else if (this.lspage1 == 'MM-mtd') {
           this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,this.lead2campaign_gid,this.lspage ]);
           this.NgxSpinnerService.hide()
         }
         else if (this.lspage1 == 'MM-ytd') {
           this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,this.lead2campaign_gid,this.lspage ]);
           this.NgxSpinnerService.hide()
         }
         else if (this.lspage1 == 'MM-Customer') {
           this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,this.lead2campaign_gid,this.lspage ]);
           this.NgxSpinnerService.hide()
         }
         else if (this.lspage1 == 'MM-Drop') {
           this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,this.lead2campaign_gid,this.lspage ]);
           this.NgxSpinnerService.hide()
         }
         else if (this.lspage1 == 'My-Today') {
           this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,this.lead2campaign_gid,this.lspage ]);
           this.NgxSpinnerService.hide()
         }
         else if (this.lspage1 == 'My-New') {
           this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,this.lead2campaign_gid,this.lspage ]);
           this.NgxSpinnerService.hide()
         }
         else if (this.lspage1 == 'My-Prospect') {
           this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,this.lead2campaign_gid,this.lspage ]);
           this.NgxSpinnerService.hide()
         }
         else if (this.lspage1 == 'My-Potential') {
           this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,this.lead2campaign_gid,this.lspage ]);
           this.NgxSpinnerService.hide()
         }
         else if (this.lspage1 == 'My-Customer') {
           this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,this.lead2campaign_gid,this.lspage ]);
           this.NgxSpinnerService.hide()
         }
         else if (this.lspage1 == 'My-Drop') {
           this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,this.lead2campaign_gid,this.lspage ]);
           this.NgxSpinnerService.hide()
         }
         else if (this.lspage1 == 'My-All') {
           this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,this.lead2campaign_gid,this.lspage ]);
           this.NgxSpinnerService.hide()
         }
         else if (this.lspage1 == 'My-Upcoming') {
           this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,this.lead2campaign_gid,this.lspage ]);
           this.NgxSpinnerService.hide()
         }
         else {
          this.route.navigate(['/smr/SmrTrnSalesorderSummary']);   
          this.NgxSpinnerService.hide()
         }
        }       
      });
    }
  getDimensionsByFilter(id: any) {
    return this.tax_list.filter((x: { tax_gid: any; }) => x.tax_gid === id);
  }

  // prodtotalcal() {
  //   const subtotal = this.unitprice * this.quantity;
  //   this.discountamount = (subtotal * this.discountpercentage) / 100;
  //   this.totalamount = subtotal - this.discountamount;
  // }
  prodtotalcal() {
    
    if(this.quantity==0 || this.quantity==null){
      this.totalamount=0;
      this.discountpercentage==0;
      this.discountamount==0;
      this.tax_amount==0;
      this.OnClearTax();

    }
    else{
      const subtotal = this.exchange * this.unitprice * this.quantity;
      this.discountamount = (subtotal * this.discountpercentage) / 100;
      this.discountamount = this.discountamount.toFixed(2);
      this.totalamount = subtotal - this.discountamount;
      this.totalamount = +(subtotal - this.discountamount).toFixed(2);
    }

    
  }
  OnChangeTaxAmount1() {
    if(this.CurrencyName=='INR')
    {
    let tax_name = this.productform.get('tax_name')?.value;
    this.taxpercentage = this.getDimensionsByFilter(tax_name);
    let tax_percentage = this.taxpercentage[0].percentage;
   
   // Calculate the tax amount with two decimal points
   this.tax_amount = Math.round(+(tax_percentage * this.totalamount / 100));
   const subtotal =  this.exchange * this.unitprice * this.quantity;
     this.discountamount = (subtotal * this.discountpercentage) / 100;
     this.discountamount = Math.round(this.discountamount);
     this.totalamount = Math.round(+(subtotal - this.discountamount + this.tax_amount));
    }
    else{
      // Calculate the subtotal with two decimal points
  const subtotal = parseFloat((this.exchange * this.unitprice * this.quantity).toFixed(2));
  
  // Calculate the discount amount with two decimal points
  this.discountamount = parseFloat(((subtotal * this.discountpercentage) / 100).toFixed(2));
  
  // Calculate the tax amount
  const tax_name = this.productform.get('tax_name')?.value;
  const selectedTax = this.tax_list.find(tax => tax.tax_name === tax_name);
  const tax_gid = selectedTax.tax_gid;
  this.taxpercentage = this.getDimensionsByFilter(tax_gid);
  const tax_percentage = this.taxpercentage[0].percentage;
  this.tax_amount = parseFloat((tax_percentage * subtotal / 100).toFixed(2));
  
  // Calculate the new total amount with two decimal points
  this.totalamount = (subtotal - this.discountamount + this.tax_amount);
  this.totalamount = ((this.totalamount).toFixed(2));
  
  if (this.totalamount % 1 !== 0) {
    this.totalamount =(this.totalamount * 10) / 10
    }
  
    }
  }


  OnChangeTaxAmount4() {
  debugger
    const tax_name = this.combinedFormData.get('tax_name4')?.value;
    // const selectedTax = this.tax4_list.find(tax => tax.tax_name4 === tax_name);
    // const tax_gid = selectedTax.tax_gid;
    this.taxpercentage = this.getDimensionsByFilter(tax_name);
    const tax_percentage = this.taxpercentage[0].percentage;

    this.tax_amount4 = (+(tax_percentage * this.producttotalamount / 100));
    this.total_price = +((+this.producttotalamount) + (+this.tax_amount4));
    //this.total_price = (+this.total_price);
    this.total_price = parseFloat(this.total_price.toFixed(2));
    this.grandtotal = ((this.total_price) + (+this.addon_charge) + (+this.freight_charges) - (+this.buyback_charges) + (+this.packing_charges) + (+this.insurance_charges) + (+this.roundoff)  - (+this.additional_discount));
    this.grandtotal = parseFloat(this.grandtotal.toFixed(2));
  }

  finaltotal(){
    
    if(this.total_price == null ||this.total_price == "" || this.total_price == 0 || this.total_price == "NaN")
    {
      this.grandtotal = ((this.producttotalamount) + (+this.addon_charge) + (+this.freight_charges) - (+this.buyback_charges) + (+this.packing_charges) + (+this.insurance_charges) + (+this.roundoff)  - (+this.additional_discount));
      this.grandtotal = +((this.grandtotal).toFixed(2));
    }
    else
    {
      const total_amount = parseFloat(this.total_price)
      const totalAmount = isNaN((total_amount)) ? 0 : (total_amount);
    const addoncharges = isNaN(this.combinedFormData.value.addon_charge) ? 0 : +this.combinedFormData.value.addon_charge;
    const frieghtcharges = isNaN(this.combinedFormData.value.freight_charges) ? 0 : +this.combinedFormData.value.freight_charges;
    const forwardingCharges = isNaN(this.combinedFormData.value.buyback_charges) ? 0 : +this.combinedFormData.value.buyback_charges;
    const insurancecharges = isNaN(this.combinedFormData.value.insurance_charges) ? 0 : +this.combinedFormData.value.insurance_charges;
    const packing_charges = isNaN(this.combinedFormData.value.packing_charges) ? 0 : +this.combinedFormData.value.packing_charges;
    const roundoff = isNaN(this.combinedFormData.value.roundoff) ? 0 : +this.combinedFormData.value.roundoff;
    const discountamount = isNaN(this.combinedFormData.value.additional_discount) ? 0 : +this.combinedFormData.value.additional_discount;
 
    this.grandtotal = ((totalAmount) + (addoncharges) + (frieghtcharges) - (forwardingCharges) + (insurancecharges) + (roundoff) - (discountamount)+(packing_charges));
    this.grandtotal = +((this.grandtotal).toFixed(2));

    }
  }

  openModaldelete(parameter: string){
    this.parameterValue = parameter
      }
  
  ondelete() {    
    var url = 'SmrSalesOrder360/GetDeleteDirectSOProductSummary'
    let param = {
      tmpsalesorderdtl_gid : this.parameterValue 
    }
    this.service.getparams(url,param).subscribe((result: any) => {
      if(result.status ==false){
        this.ToastrService.warning(result.message)
      }
      else{
        
        this.ToastrService.success(result.message)
        this.salesorederSummary();          
      }     
      });
  }

  onback() {
    const secretKey = 'storyboarderp';
    const leadbank_gid = AES.encrypt(this.leadbank_gid, secretKey).toString();
 
   if (this.lspage1 == 'MM-Total') {
     this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,this.lead2campaign_gid,this.lspage ]);
   }
   else if (this.lspage1 == 'MM-Upcoming') {
     this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,this.lead2campaign_gid,this.lspage ]);
   }
   else if (this.lspage1 == 'MM-Lapsed') {
     this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,this.lead2campaign_gid,this.lspage ]);
   }
   else if (this.lspage1 == 'MM-Longest') {
     this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,this.lead2campaign_gid,this.lspage ]);
   }
   else if (this.lspage1 == 'MM-New') {
     this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,this.lead2campaign_gid,this.lspage ]);
   }
   else if (this.lspage1 == 'MM-Prospect') {
     this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,this.lead2campaign_gid,this.lspage ]);
   }
   else if (this.lspage1 == 'MM-Potential') {
     this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,this.lead2campaign_gid,this.lspage ]);
   }
   else if (this.lspage1 == 'MM-mtd') {
     this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,this.lead2campaign_gid,this.lspage ]);
   }
   else if (this.lspage1 == 'MM-ytd') {
     this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,this.lead2campaign_gid,this.lspage ]);
   }
   else if (this.lspage1 == 'MM-Customer') {
     this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,this.lead2campaign_gid,this.lspage ]);
   }
   else if (this.lspage1 == 'MM-Drop') {
     this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,this.lead2campaign_gid,this.lspage ]);
   }
   else if (this.lspage1 == 'My-Today') {
     this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,this.lead2campaign_gid,this.lspage ]);
   }
   else if (this.lspage1 == 'My-New') {
     this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,this.lead2campaign_gid,this.lspage ]);
   }
   else if (this.lspage1 == 'My-Prospect') {
     this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,this.lead2campaign_gid,this.lspage ]);
   }
   else if (this.lspage1 == 'My-Potential') {
     this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,this.lead2campaign_gid,this.lspage ]);
   }
   else if (this.lspage1 == 'My-Customer') {
     this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,this.lead2campaign_gid,this.lspage ]);
   }
   else if (this.lspage1 == 'My-Drop') {
     this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,this.lead2campaign_gid,this.lspage ]);
   }
   else if (this.lspage1 == 'My-All') {
     this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,this.lead2campaign_gid,this.lspage ]);
   }
   else if (this.lspage1 == 'My-Upcoming') {
     this.route.navigate(['/crm/CrmTrn360view', leadbank_gid,this.lead2campaign_gid,this.lspage ]);
   }
   else {
     this.route.navigate(['/crm/CrmDashboard']);
   }
 }
 openModelDetail(product_gid:any){
  
  var url='SmrSalesOrder360/GetSalesorderdetail'
  let params={
    product_gid: product_gid
  }
  this.service.getparams(url,params).subscribe((result: any) => {
    this.responsedata = result;
     this.Salesorderdetail_list = this.responsedata.DirecteddetailslistSOCRM;
  });

}

editproductSO(tmpsalesorderdtl_gid: any) {
  var url = 'SmrSalesOrder360/GetDirectSalesOrderEditProductSummary'
  let param = {
    tmpsalesorderdtl_gid: tmpsalesorderdtl_gid
  }
  this.service.getparams(url, param).subscribe((result: any) => {
    this.directeditsalesorder_listsocrm = result.directeditsalesorder_listsocrm;
    this.productform.get("tmpsalesorderdtl_gid")?.setValue(this.directeditsalesorder_listsocrm[0].tmpsalesorderdtl_gid);
    this.productform.get("product_name")?.setValue(this.directeditsalesorder_listsocrm[0].product_name);
    this.productform.get("product_gid")?.setValue(this.directeditsalesorder_listsocrm[0].product_gid);
    this.productform.get("product_code")?.setValue(this.directeditsalesorder_listsocrm[0].product_code);
    this.productform.get("productuom_name")?.setValue(this.directeditsalesorder_listsocrm[0].productuom_name);      
    this.productform.get("quantity")?.setValue(this.directeditsalesorder_listsocrm[0].quantity);
    this.productform.get("totalamount")?.setValue(this.directeditsalesorder_listsocrm[0].totalamount);
    this.productform.get("tax_name")?.setValue(this.directeditsalesorder_listsocrm[0].tax_name);   
    this.productform.get("tax_gid")?.setValue(this.directeditsalesorder_listsocrm[0].tax_gid);   
    this.productform.get("unitprice")?.setValue(this.directeditsalesorder_listsocrm[0].unitprice);
    this.productform.get("tax_amount")?.setValue(this.directeditsalesorder_listsocrm[0].tax_amount);  
    this.productform.get("discount_percentage")?.setValue(this.directeditsalesorder_listsocrm[0].discount_percentage);
    this.productform.get("discountamount")?.setValue(this.directeditsalesorder_listsocrm[0].discountamount);  

  });
  this.showUpdateButton = true;
    this.showAddButton = false;
}

onupdate() {
  debugger
  var params = {
    tmpsalesorderdtl_gid: this.productform.value.tmpsalesorderdtl_gid,
    product_code: this.productform.value.product_code,
    product_name: this.productform.value.product_name.product_name == undefined ? this.productform.value.product_name : this.productform.value.product_name.product_name,
    product_gid: this.productform.value.product_name.product_gid == undefined ? this.productform.value.product_gid : this.productform.value.product_name.product_gid,
    productuom_name: this.productform.value.productuom_name,
    quantity: this.productform.value.quantity,
    unitprice: this.productform.value.unitprice,
    discountamount: this.productform.value.discountamount,
    discount_percentage: this.productform.value.discount_percentage,
    productuom_gid: this.productform.value.productuom_gid,
    tax_name: this.productform.value.tax_name,
    tax_gid: this.productform.value.tax_gid,
    tax_amount: this.productform.value.tax_amount,
    totalamount: this.productform.value.totalamount
  }
  var url = 'SmrSalesOrder360/PostUpdateDirectSOProduct'

  this.service.postparams(url,params).pipe().subscribe((result:any)=>{
    this.responsedata=result;
    if(result.status ==false){
      this.ToastrService.warning(result.message)       
      this.salesorederSummary();
    }
    else{
      this.ToastrService.success(result.message)
      this.productform.reset();
      
    }
    this.salesorederSummary();
  });
  this.showUpdateButton = false;
    this.showAddButton = true;
}
close(){

}
}
