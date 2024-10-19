import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { HttpClient } from '@angular/common/http';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { Component } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { CountryISO, SearchCountryField, } from "ngx-intl-tel-input";
import { TabDirective } from 'ngx-bootstrap/tabs';
import { escapeSelector } from 'jquery';

@Component({
  selector: 'app-smr-trn-amend-quotation',
  templateUrl: './smr-trn-amend-quotation.component.html',
  styleUrls: ['./smr-trn-amend-quotation.component.scss']
})
export class SmrTrnAmendQuotationComponent {
  customer_mobile1: any;
  customer_name1: any;
  salesorder_gid: any;
  customercontact_names1: any;
  customer_mobile: any;
  SearchCountryField = SearchCountryField;
  CountryISO = CountryISO;
  preferredCountries: CountryISO[] = [
    CountryISO.India,
    CountryISO.India
  ];
  config: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '38rem',
    minHeight: '5rem',
    width: '930px',
    placeholder: 'Enter text here...',
    translate: 'no',
    defaultParagraphSeparator: 'p',
    defaultFontName: 'Arial',
  };
  showUpdateButton: boolean = false;
  showAddButton: boolean = true;
  QuotationForm: FormGroup | any;
  productform: FormGroup | any;
  customer_list: any;
  contact_list: any[] = [];
  currency_list: any[] = [];
  mdlTaxName4: any;
  user_list: any[] = [];
  product_list: any[] = [];
  tax_list: any[] = [];
  tax2_list: any[] = [];
  tax3_list: any[] = [];
  tax4_list: any[] = [];
  calci_list: any[] = [];
  POproductlist: any[] = [];
  CurrencyName:any;
  terms_list: any[] = [];
  mdlBranchName: any;
  GetCustomerDet: any;
  mdlCustomerName: any;
  mdlUserName: any;
  mdlProductName: any;
  mdlTaxName3: any;
  mdlCurrencyName: any;
  mdlTaxName2: any;
  mdlTaxName1: any;
  GetproductsCode: any;
  mdlContactName: any;
  unitprice: number = 0;
  quantity: number = 0;
  discountpercentage: number = 0;
  discountamount: any;
  totalamount: number = 0;
  addon_charge: number = 0;
  POdiscountamount: number = 0;
  freight_charges: number = 0;
  forwardingCharges: number = 0;
  insurance_charges: number = 0;
  roundoff: number = 0;
  grandtotal: number = 0;
  tax_amount: number = 0;
  ForBBCharges: number = 0;
  taxpercentage: any;
  txtProductUnit: any;
  txtUnitPrice: any;
  tax_amount4: any;
  txtProductcode: any;
  txtExchangeRate: any;
  productdetails_list: any;
  tax_amount2: number = 0;
  tax_amount3: number = 0;
  producttotalamount: number = 0;
  parameterValue: any;
  productnamelist: any;
  selectedCurrencyCode: any;
  POadd_list: any;
  total_amount: any;
  mdlTerms: any;
  additional_discount: number = 0;
  mdlproductName: any;
  responsedata: any;
  ExchangeRate: any;
  Productsummarys_list: any;
  salesorders_list: any;
  cuscontact_gid: any;
  quotation_gid: any;
  quotationamend_list: any[] = [];
  currency_list1: any[] = [];
  currency_list2 :any [] =[];
  quotationproductlist: any;
  currencylist: any;
  currencycode: any
  mdlTaxName: any;
  total_price: any;
  packing_charges: any;
  amendquote_list: any;

  constructor(private http: HttpClient, private fb: FormBuilder, public NgxSpinnerService: NgxSpinnerService, private router: ActivatedRoute, private route: Router, private service: SocketService, private ToastrService: ToastrService, public ngxspinner: NgxSpinnerService) {
    this.QuotationForm = new FormGroup({
      tmpquotationdtl_gid: new FormControl(''),
      quotation_gid: new FormControl(''),
      quotationrefno: new FormControl(''),
      quotation_date: new FormControl(''),
      branch_name: new FormControl(''),
      branch_gid: new FormControl(''),
      Quo_referencenumber: new FormControl(''),
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
      Grandtotal: new FormControl(''),
      packing_charges: new FormControl(''),
      termsandconditions: new FormControl(''),
      template_gid: new FormControl(''),
      template_name: new FormControl(''),
      tax_amount4: new FormControl(''),
      total_amount: new FormControl(''),


    });
    this.productform = new FormGroup({
      tmpquotationdtl_gid: new FormControl(''),
      tax_gid: new FormControl(''),
      product_gid: new FormControl(''),
      productuom_gid: new FormControl(''),
      productgroup_gid: new FormControl(''),
      product_code: new FormControl('', Validators.required),
      productcode: new FormControl('', Validators.required),
      productgroup: new FormControl('', Validators.required),
      productuom: new FormControl('', Validators.required),
      productname: new FormControl('', Validators.required),
      tax_name: new FormControl('', Validators.required),
      remarks: new FormControl('', Validators.required),
      product_name: new FormControl('', Validators.required),
      productuom_name: new FormControl('', Validators.required),
      productgroup_name: new FormControl('', Validators.required),
      unitprice: new FormControl('', Validators.required),
      quantity: new FormControl('', Validators.required),
      discountpercentage: new FormControl('', Validators.required),
      discountamount: new FormControl('', Validators.required),
      tax_amount: new FormControl('', Validators.required),
      totalamount: new FormControl('', Validators.required),
      customerproduct_code: new FormControl(''),
      selling_price: new FormControl(''),
      product_requireddate: new FormControl(''),
      product_requireddateremarks: new FormControl(''),
    });

  }
  ngOnInit(): void {

    this.QuotationForm = new FormGroup({
      tmpquotationdtl_gid: new FormControl(''),
      quotation_gid: new FormControl(''),
      quotationrefno: new FormControl(''),
      quotation_date: new FormControl(''),
      branch_name: new FormControl(''),
      branch_gid: new FormControl(''),
      Quo_referencenumber: new FormControl(''),
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
      Grandtotal: new FormControl(''),
      packing_charges: new FormControl(''),
      termsandconditions: new FormControl(''),
      template_gid: new FormControl(''),
      template_name: new FormControl(''),
      tax_amount4: new FormControl(''),
      total_amount: new FormControl(''),


    });
    this.productform = new FormGroup({
      tmpquotationdtl_gid: new FormControl(''),
      tax_gid: new FormControl(''),
      product_gid: new FormControl(''),
      productuom_gid: new FormControl(''),
      productgroup_gid: new FormControl(''),
      product_code: new FormControl('', Validators.required),
      productcode: new FormControl('', Validators.required),
      productgroup: new FormControl('', Validators.required),
      productuom: new FormControl('', Validators.required),
      productname: new FormControl('', Validators.required),
      tax_name: new FormControl('', Validators.required),
      remarks: new FormControl('', Validators.required),
      product_name: new FormControl('', Validators.required),
      productuom_name: new FormControl('', Validators.required),
      productgroup_name: new FormControl('', Validators.required),
      unitprice: new FormControl('', Validators.required),
      quantity: new FormControl('', Validators.required),
      discountpercentage: new FormControl('', Validators.required),
      discountamount: new FormControl('', Validators.required),
      tax_amount: new FormControl('', Validators.required),
      totalamount: new FormControl('', Validators.required),
      customerproduct_code: new FormControl(''),
      selling_price: new FormControl(''),
      product_requireddate: new FormControl(''),
      product_requireddateremarks: new FormControl(''),
    });

    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);

    const quotation_gid = this.router.snapshot.paramMap.get('quotation_gid');
    this.quotation_gid = quotation_gid;

    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.quotation_gid, secretKey).toString(enc.Utf8);
    
    /// Customer Name Dropdown ////
    var url = 'SmrTrnSalesorder/GetCustomerDtl'
    this.service.get(url).subscribe((result: any) => {
      this.customer_list = result.GetCustomerDtl;
    });
    //// Sales person Dropdown ////
    var url = 'SmrTrnSalesorder/GetPersonDtl'
    this.service.get(url).subscribe((result: any) => {
      this.contact_list = result.GetPersonDtl;
    });

    //// Tax 3 Dropdown ////
    var url = 'SmrTrnSalesorder/GetTax4Dtl'
    this.service.get(url).subscribe((result: any) => {
      this.tax4_list = result.GetTax4Dtl;
    });

    //// Currency Dropdown ////
    var url = 'SmrTrnSalesorder/GetCurrencyDtl'
    this.service.get(url).subscribe((result: any) => {
      this.currency_list1 = result.GetCurrencyDtl;
    });
    //// Tax 1 Dropdown ////
    var url = 'SmrTrnSalesorder/GetTax1Dtl'
    this.service.get(url).subscribe((result: any) => {
      this.tax_list = result.GetTax1Dtl;
      
    });
    //// Product Dropdown ////
    var url = 'SmrTrnSalesorder/GetProductNamDtl'
    this.service.get(url).subscribe((result: any) => {
      this.product_list = result.GetProductNamDtl;
    });
    // Termd and Conditions //
    //// T & C Dropdown ////
    var url = 'SmrTrnQuotation/GetTermsandConditions'
    this.service.get(url).subscribe((result: any) => {
      this.terms_list = result.GetTermsandConditions;
    });
    this.quotation_gid = deencryptedParam;
    this.GetQuotationamend(deencryptedParam);
  }


  GetQuotationamend(quotation_gid: any) {
    
    let param = {
      quotation_gid: this.quotation_gid
    }

    var api = 'SmrQuotationAmend/GetQuotationamend'
    this.NgxSpinnerService.show()

    this.service.getparams(api, param).subscribe((result: any) => {
      this.quotationamend_list = result.quotationamend_list;
      this.QuotationForm.get("quotation_gid")?.setValue(this.quotation_gid);
      this.QuotationForm.get("quotationrefno")?.setValue(this.quotationamend_list[0].quotation_referencenumber);
      this.QuotationForm.get("customer_gid")?.setValue(this.quotationamend_list[0].customer_gid);
      this.QuotationForm.get("quotation_date")?.setValue(this.quotationamend_list[0].quotation_date);
      this.QuotationForm.get("branch_name")?.setValue(this.quotationamend_list[0].branch_name);
      this.QuotationForm.get("Quo_referencenumber")?.setValue(this.quotationamend_list[0].quotation_referenceno1);
      this.QuotationForm.get("customer_name")?.setValue(this.quotationamend_list[0].customer_name);
      this.QuotationForm.get("customercontact_names")?.setValue(this.quotationamend_list[0].contact_person);
      this.QuotationForm.get("customer_mobile")?.setValue(this.quotationamend_list[0].contact_no);
      this.QuotationForm.get("customer_email")?.setValue(this.quotationamend_list[0].contact_mail);
      this.QuotationForm.get("customer_address")?.setValue(this.quotationamend_list[0].customer_address);
      this.QuotationForm.get("customer_address")?.setValue(this.quotationamend_list[0].customer_address);
      this.QuotationForm.get("so_remarks")?.setValue(this.quotationamend_list[0].quotation_remarks);
      this.QuotationForm.get("shipping_to")?.setValue(this.quotationamend_list[0].customer_email);
      this.QuotationForm.get("user_name")?.setValue(this.quotationamend_list[0].salesperson_gid);
      this.QuotationForm.get("freight_terms")?.setValue(this.quotationamend_list[0].freight_terms);
      this.QuotationForm.get("payment_terms")?.setValue(this.quotationamend_list[0].payment_terms);
      this.QuotationForm.get("currency_code")?.setValue(this.quotationamend_list[0].currency_code);
      this.mdlCurrencyName = this.quotationamend_list[0].currency_gid;
      this.QuotationForm.get("exchange_rate")?.setValue(this.quotationamend_list[0].exchange_rate);
      this.QuotationForm.get("termsandconditions")?.setValue(this.quotationamend_list[0].termsandconditions);
      this.QuotationForm.get("delivery_days")?.setValue(this.quotationamend_list[0].delivery_days);
      this.QuotationForm.get("payment_days")?.setValue(this.quotationamend_list[0].payment_days);
      this.QuotationForm.get("tax_name4")?.setValue(this.quotationamend_list[0].tax_name);
      //this.tax_amount4=this.quotationamend_list[0].tax_amount4;
      //this.total_amount=this.quotationamend_list[0].total_amount;
      this.addon_charge = this.quotationamend_list[0].addon_charge;
      this.additional_discount=this.quotationamend_list[0].additional_discount;
      this.freight_charges=this.quotationamend_list[0].freight_charges;
      this.ForBBCharges=this.quotationamend_list[0].buyback_charges;
      this.packing_charges=this.quotationamend_list[0].packing_charges;
      this.insurance_charges=this.quotationamend_list[0].insurance_charges;
      this.roundoff=this.quotationamend_list[0].roundoff ;
      //this.QuotationForm.get("Grandtotal")?.setValue(this.quotationamend_list[0].Grandtotal);
      this.mdlUserName = this.quotationamend_list[0].salesperson_gid

      this.ProductSummary();
      this.responsedata = result.quoteamend_productlist
      this.NgxSpinnerService.hide()
    });
  }


  get payment_days() {
    return this.QuotationForm.get('payment_days')!;
  }
  get delivery_days() {
    return this.QuotationForm.get('delivery_days')!;
  }
 
  GetOnChangeProductsName() {

    let product_gid = this.productform.value.product_name.product_gid;
    if( this.QuotationForm.value.customer_name != undefined){
      let customercontact_gid = this.QuotationForm.value.customer_gid;
    let param = {
      product_gid: product_gid,
      customercontact_gid:customercontact_gid
    }
    var url = 'SmrQuotationAmend/GetOnChangeProductNameQOAmend'
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.GetproductsCode = this.responsedata.GetProductChange_QOAmend;
      this.productform.get("product_code")?.setValue(result.GetProductChange_QOAmend[0].product_code);
      this.productform.get("productuom_name")?.setValue(result.GetProductChange_QOAmend[0].productuom_name);
      this.productform.get("productgroup_name")?.setValue(result.GetProductChange_QOAmend[0].productgroup_name);
      this.productform.get("unitprice")?.setValue(result.GetProductChange_QOAmend[0].unitprice);
    });
  }
  else {
    this.ToastrService.warning('Kindly Select Customer Before Adding Product !! ')
  }
  }
  OnClearProduct() {
    this.txtProductcode = '';
    this.txtUnitPrice = '';
    this.txtProductUnit = '';
  }

  OnClearCurrency() {
    this.txtExchangeRate = '';
  }



  getDimensionsByFilter(id: any) {
    return this.tax_list.filter((x: { tax_gid: any; }) => x.tax_gid === id);
  }

  prodtotalcal() {
    if(this.quantity==0 || this.quantity==null){
      this.totalamount=0;
      this.discountpercentage==0;
      this.discountamount==0;
      this.tax_amount==0;
      this.OnClearTax();

    }
    else{
    const subtotal = this.txtExchangeRate * this.unitprice * this.quantity;
    this.discountamount = (subtotal * this.discountpercentage) / 100;
    this.discountamount = ((this.discountamount * 100) / 100).toFixed(2);
    this.totalamount = subtotal - this.discountamount;
    this.totalamount = +((+(subtotal - this.discountamount)).toFixed(2));
  }
}
taxAmount() {

  if(this.CurrencyName=='INR')
  {
  let tax_name = this.productform.get('tax_name')?.value; 
  let selectedTax = this.tax_list.find(tax => tax.tax_name === tax_name);
 
    let tax_gid = selectedTax.tax_gid; 
    this.taxpercentage = this.getDimensionsByFilter(tax_gid); 
    let tax_percentage = this.taxpercentage[0].percentage;

    // Calculate the tax amount with two decimal points
    this.tax_amount = (+(tax_percentage * this.totalamount / 100));

    // Calculate the new total amount
    const subtotal =  this.txtExchangeRate *this.unitprice * this.quantity;
    this.discountamount = (subtotal * this.discountpercentage) / 100;
    this.discountamount = (this.discountamount * 100) / 100;
    this.totalamount = (+(subtotal - this.discountamount + this.tax_amount));
  
      this.total_amount = +((this.total_amount).toFixed(2));
    
}
else{
  const subtotal = parseFloat((this.txtExchangeRate * this.unitprice * this.quantity).toFixed(2));

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
this.totalamount = +((subtotal - this.discountamount + this.tax_amount).toFixed(2));

if (this.totalamount % 1 !== 0) {
this.totalamount =(this.totalamount * 10) / 10
}
}
}


//   taxAmount() {
// debugger
//     if(this.CurrencyName=='INR')
//     {
//     let tax_name = this.productform.get('tax_name')?.value; 
//     let selectedTax = this.tax_list.find(tax => tax.tax_gid === tax_name);
   
//       let tax_gid = selectedTax.tax_gid; 
//       this.taxpercentage = this.getDimensionsByFilter(tax_gid); 
//       let tax_percentage = this.taxpercentage[0].percentage;
  
//       // Calculate the tax amount with two decimal points
//       this.tax_amount = (+(tax_percentage * this.totalamount / 100));
  
//       // Calculate the new total amount
//       const subtotal =  this.txtExchangeRate*this.unitprice * this.quantity;
//       this.discountamount = (subtotal * this.discountpercentage) / 100;
//       this.discountamount = (this.discountamount * 100) / 100;
//       this.totalamount = (+(subtotal - this.discountamount + this.tax_amount));
    
//         this.total_amount = +((this.total_amount).toFixed(2));
      
//   }
//   else{
    
//     // Calculate the subtotal with two decimal points
// const subtotal = parseFloat((this.txtExchangeRate * this.unitprice * this.quantity).toFixed(2));

// // Calculate the discount amount with two decimal points
// this.discountamount = parseFloat(((subtotal * this.discountpercentage) / 100).toFixed(2));

// // Calculate the tax amount
// const tax_name = this.productform.get('tax_name')?.value;
// const selectedTax = this.tax_list.find(tax => tax.tax_gid === tax_name);
// const tax_gid = selectedTax.tax_gid;
// this.taxpercentage = this.getDimensionsByFilter(tax_gid);
// const tax_percentage = this.taxpercentage[0].percentage;
// this.tax_amount = parseFloat((tax_percentage * subtotal / 100).toFixed(2));

// // Calculate the new total amount with two decimal points
// this.totalamount = parseFloat((subtotal - this.discountamount + this.tax_amount).toFixed(2));

// if (this.totalamount % 1 !== 0) {
//   this.totalamount =(this.totalamount * 10) / 10
//   }

//   }
//   }

  finaltotal() {

    if(this.total_amount == null ||this.total_amount == "" || this.total_amount == 0 || this.total_amount == "NaN")
    {
      this.grandtotal = ((this.producttotalamount) + (+this.addon_charge) + (+this.freight_charges)  + (+this.packing_charges) + (+this.insurance_charges) + (+this.roundoff)  - (+this.additional_discount) - (+this.ForBBCharges));
      this.grandtotal = +((this.grandtotal));
    }
    
else
{
// Convert total amount to a valid number
const total_amount = parseFloat(this.total_amount)
const totalAmount = isNaN((total_amount)) ? 0 : (total_amount);
//const product = isNaN((this.producttotalamount)) ? 0 : (this.total_amount.replace(/,/g, ''));


const addoncharges = isNaN((this.QuotationForm.value.addon_charge)) ? 0 : (this.QuotationForm.value.addon_charge);
const discount = isNaN((this.QuotationForm.value.additional_discount)) ? 0 : (this.QuotationForm.value.additional_discount);
const forwardingCharges = isNaN((this.QuotationForm.value.packing_charges)) ? 0 : (this.QuotationForm.value.packing_charges);
const insuranceCharges = isNaN((this.QuotationForm.value.insurance_charges)) ? 0 : (this.QuotationForm.value.insurance_charges);
const buybackCharges = isNaN((this.QuotationForm.value.buyback_charges)) ? 0 : (this.QuotationForm.value.buyback_charges);
const roundoff = isNaN((this.QuotationForm.value.roundoff)) ? 0 : (this.QuotationForm.value.roundoff);
const freight = isNaN((this.QuotationForm.value.freight_charges)) ? 0 : (this.QuotationForm.value.freight_charges);

// Perform the calculation

this.grandtotal = ( (+totalAmount) + (+addoncharges) + (+freight) + (+forwardingCharges) + (+insuranceCharges) + (+roundoff) - (+discount) - (+buybackCharges));
this.grandtotal = +((this.grandtotal));
  }
}

  onadd() {

    var params = {
      quotation_gid: this.quotation_gid,
      productgroup_name: this.productform.value.productgroup_name,
      product_code: this.productform.value.product_code,
      product_name: this.productform.value.product_name.product_name,
      productuom_name: this.productform.value.productuom_name,
      quantity: this.productform.value.quantity,
      product_gid: this.productform.value.product_name.product_gid,
      productgroup_gid: this.productform.value.productgroup_gid,
      productuom_gid: this.productform.value.productuom_gid,
      unitprice: this.productform.value.unitprice,
      discountpercentage: this.productform.value.discountpercentage,
      discountamount: this.productform.value.discountamount,
      tax_name: this.productform.value.tax_name,
      tax_amount: this.productform.value.tax_amount,
      totalamount: this.productform.value.totalamount


    }

    var api = 'SmrQuotationAmend/PostAmendProduct'
    this.NgxSpinnerService.show()
    console.log(this.productform.value);
    this.service.post(api, params).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide()

      }
      else {
        this.ToastrService.success(result.message)
        this.productform.reset()
        this.NgxSpinnerService.hide()

      }
      this.ProductSummary()
    },
    );
  }
  ProductSummary() {

    let param = {
      quotation_gid: this.quotation_gid
    }
    var api = 'SmrQuotationAmend/QuotationAmendProductSummary';
    this.service.getparams(api, param).subscribe((result: any) => {
      this.responsedata = result;
      this.quotationproductlist = this.responsedata.quoteamend_productlist;

      let n = this.quotationproductlist.length;
      
      this.QuotationForm.get("producttotalamount")?.setValue(result.grandtotal);
   
      if(this.mdlTaxName4 == null || this.mdlTaxName4 == "" || this.mdlTaxName4 == "--No Tax--")
      {
        this.QuotationForm.get("Grandtotal")?.setValue(result.grandtotal);
      }
      else
      {
      let selectedTax = this.tax4_list.find(tax => tax.tax_name4 === this.mdlTaxName4);
      this.taxpercentage = this.getDimensionsByFilter(selectedTax.tax_gid);
      let tax_percentage = this.taxpercentage[0].percentage;
      this.tax_amount4 = (+(tax_percentage * result.grandtotal / 100));
     const tax4 =  this.tax_amount4; 
      const taxamount = (+(result.grandtotal + tax4));
      const newGrandTotal = result.grandtotal + parseFloat(tax4);
      const newGrandTotal2 = newGrandTotal + parseFloat(this.QuotationForm.value.addon_charge);
      const newGrandTotal3 = newGrandTotal2 - parseFloat(this.QuotationForm.value.additional_discount);
      const newGrandTotal4 = newGrandTotal3 + parseFloat(this.QuotationForm.value.freight_charges);
      const newGrandTotal5 = newGrandTotal4 - parseFloat(this.QuotationForm.value.buyback_charges);
      const newGrandTotal6 = newGrandTotal5 + parseFloat(this.QuotationForm.value.packing_charges);
      const newGrandTotal7 = newGrandTotal6 + parseFloat(this.QuotationForm.value.insurance_charges);
      const newGrandTotal8 = newGrandTotal7 + parseFloat(this.QuotationForm.value.roundoff);
      this.QuotationForm.get("tax_amount4")?.setValue((tax4).toFixed(2));
      this.QuotationForm.get("total_amount")?.setValue((taxamount).toFixed(2));
      this.QuotationForm.get("Grandtotal")?.setValue((newGrandTotal8).toFixed(2));
      }
    });
    

  }



  
  OnChangeCurrency() {

    let selectedCurrencyName = this.mdlCurrencyName;

    let currencyexchange_gid = selectedCurrencyName;
    let param = {
      currencyexchange_gid: currencyexchange_gid
    };

    var url = 'SmrTrnQuotation/GetOnChangeCurrency';

    // Fetch currency exchange data
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.currency_list2 = this.responsedata.GetOnchangecurrency;
      this.QuotationForm.get("exchange_rate")?.setValue(this.currency_list1[0].exchange_rate);
    });
  }
  overallsub() {

    var api = 'SmrQuotationAmend/PostQuotationAmend';
    this.NgxSpinnerService.show()
    this.service.post(api, this.QuotationForm.value).subscribe(
      (result: any) => {

        if (result.status == true) {
          this.ToastrService.success(result.message)
          this.NgxSpinnerService.hide()
          
        }
        else {
          this.ToastrService.warning(result.message)
          this.NgxSpinnerService.hide()
        }
      }
    );
  }


  GetOnChangeTerms() {


    let template_gid = this.QuotationForm.value.template_name;
    let param = {
      template_gid: template_gid
    }
    var url = 'SmrTrnQuotation/GetOnChangeTerms';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.QuotationForm.get("termsandconditions")?.setValue(result.terms_list[0].termsandconditions);
      this.QuotationForm.value.template_gid = result.terms_list[0].template_gid
      //this.cusraiseform.value.productuom_gid = result.GetProductsName[0].productuom_gid
    });
  }

  openModaldelete(parameter: string) {

    this.parameterValue = parameter
  }

  ondelete() {
    var url = 'SmrQuotationAmend/DeleteAmendProductSummary'
    this.NgxSpinnerService.show()
    let param = {
      tmpquotationdtl_gid: this.parameterValue
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        this.NgxSpinnerService.hide()
        this.ToastrService.warning(result.message)
      }
      else {

        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide()
      }
      this.ProductSummary();
    });
  }

  edit(tmpquotationdtl_gid :any){

    var url = 'SmrQuotationAmend/GetQuotationAmendEditProductSummary'
    this.NgxSpinnerService.show()
    let param = {
      tmpquotationdtl_gid: tmpquotationdtl_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.amendquote_list = result.Amendproduct_List;
      this.productform.get("tmpquotationdtl_gid")?.setValue(this.amendquote_list[0].tmpquotationdtl_gid);
      this.productform.get("product_name")?.setValue(this.amendquote_list[0].product_name);
      this.productform.get("product_gid")?.setValue(this.amendquote_list[0].product_gid);
      this.productform.get("product_code")?.setValue(this.amendquote_list[0].product_code);
      this.productform.get("productuom_name")?.setValue(this.amendquote_list[0].productuom_name); 
      this.productform.get("quantity")?.setValue(this.amendquote_list[0].qty_quoted);
      this.productform.get("unitprice")?.setValue(this.amendquote_list[0].product_price);
      this.productform.get("discountpercentage")?.setValue(this.amendquote_list[0].discount_percentage);
      this.productform.get("discountamount")?.setValue(this.amendquote_list[0].discount_amount);
      this.productform.get("totalamount")?.setValue(this.amendquote_list[0].price);
      this.productform.get("tax_gid")?.setValue(this.amendquote_list[0].tax_gid);    
      this.productform.get("tax_name")?.setValue(this.amendquote_list[0].tax_name);    
      this.productform.get("tax_amount")?.setValue(this.amendquote_list[0].tax_amount);    

      this.NgxSpinnerService.hide()
      this.showUpdateButton = true;
      this.showAddButton = false;
    });
    console.log(this.productform.value)
    
  }
   
   
  productUpdate() {


    var params = {
      tmpquotationdtl_gid: this.amendquote_list[0].tmpquotationdtl_gid,
      tax_name: this.productform.value.tax_name == undefined ? this.productform.value.tax_name : this.productform.value.tax_name,
      tax_gid: this.productform.value.tax_gid == undefined ? this.productform.value.tax_gid : this.productform.value.tax_gid,
      tax_amount: this.productform.value.tax_amount,
      total_amount: this.productform.value.totalamount,
      //productgroup_name: this.productform.value.productgroup_name,
      discountamount: this.productform.value.discountamount,
      discountpercentage: this.discountpercentage,
      quantity: this.productform.value.quantity,
      unitprice: this.productform.value.unitprice,
      unit: this.productform.value.productuom_name,
      product_name: this.productform.value.product_name,
      product_code: this.productform.value.product_code
    }

    var api = 'SmrQuotationAmend/PostUpdateQuotationAmendProduct'
    this.NgxSpinnerService.show();
    console.log(params);
    this.service.post(api, params).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      this.ProductSummary()
      this.productform.reset();
    });
    this.showAddButton = true;
    this.showUpdateButton = false;
    
  }

  OnChangeTaxAmount4() {
    
    let selectedTax = this.tax4_list.find(tax => tax.tax_name4 === this.mdlTaxName4);
    this.taxpercentage = this.getDimensionsByFilter(selectedTax.tax_gid);

    let tax_percentage = this.taxpercentage[0].percentage;

    this.tax_amount4 = (+(tax_percentage * this.producttotalamount / 100));
    this.total_amount = (+(this.producttotalamount + this.tax_amount4));

    //this.total_amount = Math.round(+this.total_amount);
    const addoncharges = isNaN((this.QuotationForm.value.addon_charge)) ? 0 : (this.QuotationForm.value.addon_charge);
    const discount = isNaN((this.QuotationForm.value.additional_discount)) ? 0 : (this.QuotationForm.value.additional_discount);
    const forwardingCharges = isNaN((this.QuotationForm.value.packing_charges)) ? 0 : (this.QuotationForm.value.packing_charges);
    const insuranceCharges = isNaN((this.QuotationForm.value.insurance_charges)) ? 0 : (this.QuotationForm.value.insurance_charges);
    const buybackCharges = isNaN((this.QuotationForm.value.buyback_charges)) ? 0 : (this.QuotationForm.value.buyback_charges);
    const roundoff = isNaN((this.QuotationForm.value.roundoff)) ? 0 : (this.QuotationForm.value.roundoff);
    const freight = isNaN((this.QuotationForm.value.freight_charges)) ? 0 : (this.QuotationForm.value.freight_charges);
    this.grandtotal = ((this.total_amount) + (+addoncharges) + (+freight) - (+buybackCharges) + (+insuranceCharges) + (+forwardingCharges) + (+roundoff) - (+discount));


  }


    OnClearTax() {

      this.tax_amount = 0; 
      const subtotal = this.txtExchangeRate *  this.unitprice * this.quantity;
      this.totalamount = (+(subtotal - this.tax_amount));
      this.totalamount = +((this.totalamount).toFixed(2))
        
      
     

  }

  OnClearOverallTax() {
 
    this.tax_amount4 = '';
  this.total_amount ='';
    this.grandtotal = ((this.producttotalamount) + (+this.addon_charge) + (+this.freight_charges) - (+this.ForBBCharges) + (+this.packing_charges) + (+this.insurance_charges) + (+this.roundoff)  - (+this.additional_discount));
    this.grandtotal = (+this.grandtotal);

  
  }
  onKeyPress(event: any) {
    // Get the pressed key
    const key = event.key;

    if (!/^[0-9.]$/.test(key)) {
      // If not a number or dot, prevent the default action (key input)
      event.preventDefault();
    }
  }

  // oncleartotal()
  // {
  //   debugger
  //   if(this.QuotationForm.value.addon_charge == "" || this.QuotationForm.value.addon_charge == 0 || this.QuotationForm.value.addon_charge == null || this.QuotationForm.value.addon_charge == "NaN")
  //   {
  //     this.grandtotal -=parseFloat(this.QuotationForm.value.addon_charge)
  //   }

  //   else if(this.QuotationForm.value.additional_discount == "" || this.QuotationForm.value.additional_discount == 0 || this.QuotationForm.value.additional_discount == null || this.QuotationForm.value.additional_discount == "NaN")
  //   {
  //     this.grandtotal +=parseFloat(this.QuotationForm.value.additional_discount)
  //   }

  //   else if(this.QuotationForm.value.packing_charges == "" || this.QuotationForm.value.packing_charges == 0 || this.QuotationForm.value.packing_charges == null || this.QuotationForm.value.packing_charges == "NaN")
  //   {
  //     this.grandtotal -=parseFloat(this.QuotationForm.value.packing_charges)
  //   }
  //   else if(this.QuotationForm.value.insurance_charges == "" || this.QuotationForm.value.insurance_charges == 0 || this.QuotationForm.value.insurance_charges == null || this.QuotationForm.value.insurance_charges == "NaN")
  //   {
  //     this.grandtotal -=parseFloat(this.QuotationForm.value.insurance_charges)
  //   }
  //   else if(this.QuotationForm.value.buyback_charges == "" || this.QuotationForm.value.buyback_charges == 0 || this.QuotationForm.value.insurance_charges == null || this.QuotationForm.value.buyback_charges == "NaN")
  //   {
  //     this.grandtotal +=parseFloat(this.QuotationForm.value.buyback_charges)
  //   }
  //   else if(this.QuotationForm.value.freight_charges == "" || this.QuotationForm.value.freight_charges == 0 || this.QuotationForm.value.freight_charges == null || this.QuotationForm.value.freight_charges == "NaN")
  //   {
  //     this.grandtotal -=parseFloat(this.QuotationForm.value.freight_charges)
  //   }
  //   else if(this.QuotationForm.value.roundoff == "" || this.QuotationForm.value.roundoff == 0 || this.QuotationForm.value.roundoff == null || this.QuotationForm.value.roundoff == "NaN")
  //   {
  //     this.grandtotal -=parseFloat(this.QuotationForm.value.roundoff)
  //   }
  // }

  onsubmit() {

    console.log(this.QuotationForm.value)
    var params = {
      branch_name: this.QuotationForm.value.branch_name,

      quotation_gid: this.QuotationForm.value.quotation_gid,
      quotation_referencenumber: this.QuotationForm.value.quotationrefno,
      quotation_date: this.QuotationForm.value.quotation_date,
      customer_name: this.QuotationForm.value.customer_name,
      quotation_referenceno1: this.QuotationForm.value.Quo_referencenumber,
      customercontact_names: this.QuotationForm.value.customercontact_names,
      customer_email: this.QuotationForm.value.customer_email,
      customer_mobile: this.QuotationForm.value.customer_mobile,
      quotation_remarks: this.QuotationForm.value.so_remarks,
      customer_address: this.QuotationForm.value.customer_address,
      freight_terms: this.QuotationForm.value.freight_terms,
      payment_terms: this.QuotationForm.value.payment_terms,
      currency_code: this.QuotationForm.value.currency_code,
      user_name: this.QuotationForm.value.user_name,
      exchange_rate: this.QuotationForm.value.exchange_rate,
      payment_days: this.QuotationForm.value.payment_days,
      customer_gid: this.QuotationForm.value.customer_name.customer_gid,
      termsandconditions: this.QuotationForm.value.termsandconditions,
      template_name: this.QuotationForm.value.template_name,
      roundoff: this.QuotationForm.value.roundoff,
      insurance_charges: this.QuotationForm.value.insurance_charges,
      packing_charges: this.QuotationForm.value.packing_charges,
      buyback_charges: this.QuotationForm.value.buyback_charges,
      freight_charges: this.QuotationForm.value.freight_charges,
      additional_discount: this.QuotationForm.value.additional_discount,
      addon_charge: this.QuotationForm.value.addon_charge,
      producttotalamount: this.QuotationForm.value.producttotalamount,
      delivery_days: this.QuotationForm.value.delivery_days,
      Grandtotal: this.QuotationForm.value.Grandtotal,
      total_amount: this.QuotationForm.value.total_amount,
      tax_name4: this.QuotationForm.value.tax_name4,
      tax_amount4: this.QuotationForm.value.tax_amount4

    }
    var url = 'SmrQuotationAmend/postQuotationAmend'

    this.NgxSpinnerService.show()
    this.service.post(url, params).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide()
      }
      else {
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide()
        this.route.navigate(['/smr/SmrTrnQuotationSummary']);
      }
    });
  }
  onback() {
    this.route.navigate(['/smr/SmrTrnQuotationSummary']);
  }

}



