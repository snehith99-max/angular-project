
import { HttpClient } from '@angular/common/http';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { AES, enc } from 'crypto-js';
import { map, param } from 'jquery';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { NgxSpinnerService } from 'ngx-spinner';
import { Component, OnInit, Pipe, ElementRef, ViewChild, Renderer2 } from '@angular/core';
import { dE } from '@fullcalendar/core/internal-common';
import { filter, Observable, Subject } from 'rxjs';
import { FilterPipe } from 'src/app/Service/filter';
import { Table } from 'jspdf-autotable';


interface CollapseState {
  [key: string]: boolean;
}

interface SalesOD {
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
  tax_name4: string;
  tax_amount2: string;
  tax_name3: string;
  tax_amount3: string;
  price: string;
  filter: string;

}
@Component({
  selector: 'app-smr-trn-renewaltoraiseso',
  templateUrl: './smr-trn-renewaltoraiseso.component.html',
  styleUrls: ['./smr-trn-renewaltoraiseso.component.scss']
})
export class SmrTrnRenewaltoraisesoComponent {

  showUpdateButton: boolean = false;
  showAddButton: boolean = true;
  config: AngularEditorConfig = {
    editable: false,
    spellcheck: true,
    height: '40rem',
    placeholder: 'Enter text here...',
    translate: 'no',
    defaultParagraphSeparator: 'p',

  };
  cost_price: number = 0;
  mdlProductCode: any;
  mdlProductUom: any;
  productsearchadd: any;
  totaltaxamount: any;
  combinedFormData: FormGroup | any;
  productform: FormGroup | any;
  Cmntaxsegment_gid: any;
  customer_list: any;
  allchargeslist: any[] = [];
  filteredProductCodes: any[] = [];
  customercontact_name: any;
  branch_list: any[] = [];
  contact_list: any[] = [];
  currency_list: any[] = [];
  SO_list: any[] = [];
  user_list: any[] = [];
  product_list: any[] = [];
  tax_list: any[] = [];
  tax2_list: any[] = [];
  tax3_list: any[] = [];
  tax4_list: any[] = [];
  calci_list: any[] = [];
  POproductlist: any[] = [];
  GetTaxSegmentListorder: any[] = [];
  terms_list: any[] = [];
  directeditsalesorder_list: any[] = [];
  mdlBranchName: any;
  GetCustomerDet: any;
  mdlCustomerName: any;
  customercontact_names1: any;
  customer_mobile1: any;
  customer_email1: any;
  customer_address1: any;
  product_code1: any;
  productuom_name1: any;
  unitprice1: any;
  mdlUserName: any;
  tax_name4: any;
  mdlProductName: any;
  mdlTaxName3: any;
  mdlCurrencyName: any;
  mdlTaxName2: any;
  mdlTaxName1: any;
  grandtotal: any;
  taxprecentage4: any;
  GetproductsCode: any;
  mdlContactName: any;
  unitprice: number = 0;
  quantity: number = 0;
  productquantity: number = 0;
  productunitprice: number = 0;
  productdiscount: number = 0;
  discount_percentage: number = 0;
  discountpercentage: number = 0;
  discountamount: any;
  addon_charge: number = 0;
  POdiscountamount: number = 0;
  freight_charges: number = 0;
  forwardingCharges: number = 0;
  insurance_charges: number = 0;
  roundoff: number = 0;
  packing_charges: number = 0;
  // grandtotal: number = 0;
  tax_amount: any;
  buyback_charges: number = 0;
  tax_amount4: any;
  taxpercentage: any;
  productdetails_list: any;
  Product_list: any[] = [];
  tax_amount2: number = 0;
  productdiscount_amountvalue: number = 0;
  tax_amount3: number = 0;
  producttotalamount: any;
  parameterValue: string | undefined;
  productnamelist: any;
  selectedCurrencyCode: any;
  POadd_list: any;
  total_amount: any = 0;
  producttotal_amount: any = 0;
  total_tax_amount: any = 0;
  producttotal_tax_amount: any = 0;
  individualTaxAmounts: any = 0;
  taxamount1: any = 0;
  taxamount2: any = 0;
  taxamount3: any = 0;
  taxprecentage1: any = 0;
  taxprecentage2: any = 0;
  taxprecentage3: any = 0;
  mdlTerms: any;
  additional_discount: number = 0;
  total_price: any;
  taxname1: any;
  tax_prefix: any;
  taxsegment_gid: any;
  taxname2: any;
  taxname3: any;
  taxgid1: any;
  taxgid2: any;
  taxgid3: any;
  mdlproductName: any;
  responsedata: any;
  ExchangeRate: any;
  salesOD!: SalesOD;
  Productsummarys_list: any;
  salesorders_list: any;
  cuscontact_gid: any;
  filteredSOProductList: any[] = [];
  salesorder: any;
  leadbank_gid: any;
  exchange: number = 0;  
  productdiscounted_precentagevalue: number = 0;
  allproductdiscount_amount: number = 0;
  alldiscounted_subtotal: number = 0;
  taxtotal: number = 0;
  currency_code1: any;
  Salesorderdetail_list: any;
  currency_list1: any;
  exchange_rate: any;
  totalamount: any;
  CurrencyExchangeRate: any;
  CurrencyName: any;
  //searchText: any;
  tax_prefix2: any;
  // new design
  sam: boolean = false;
  arrowfst: boolean = false;
  arrowOne: boolean = false;
  mdlproducttype: any;
  producttype_list: any[] = [];
  Getproductgroup: any[] = [];
  branchaddresslist: any[] = [];
  marks: number = 0;
  GetTaxSegmentList: any[] = [];
  totTaxAmountPerQty: any;
  prod_name: any;
  mdlTaxSegment: any;
  taxseg_tax: any;
  mdlcustomeradrress: any = null;
  SOProductList: any[] = [];
  productgroup_list: any[] = [];
  searchText: any;
  productsearch: any;
  productcodesearch: any;
  productcodesearch1: any;
  productname: any;
  encryt: any;
  quotation_gid: any;
  customer_gid: any;
  termsandconditions: any;
  campaign_title: any;
  campaign_gid: any;
  salesorder_gid:any;
  user_gid: any;
  mdldeliverydays: any;
  mdldpaymentdays: any;


  constructor(private http: HttpClient,
    private fb: FormBuilder, private renderer: Renderer2,
    private router: ActivatedRoute, private route: Router, private service: SocketService, private ToastrService: ToastrService, public NgxSpinnerService: NgxSpinnerService,) {
    this.salesOD = {} as SalesOD
  }
  ngOnInit(): void {
    debugger
    this.productSearch();
    
    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);

    this.combinedFormData = new FormGroup({
      tmpsalesorderdtl_gid: new FormControl(''),
      salesorder_gid: new FormControl(''),
      quotation_gid: new FormControl(''),
      salesorder_date: new FormControl(this.getCurrentDate()),
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
      billto: new FormControl(''),
      user_name: new FormControl(''),
      tax_amount4: new FormControl(''),
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
      totalamount: new FormControl(''),
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
      branch_address: new FormControl(''),
      customerinstruction: new FormControl(''),
      customer_details: new FormControl(''),


    });
    this.productform = new FormGroup({
      tmpsalesorderdtl_gid: new FormControl(''),
      tax_gid: new FormControl(''),
      product_gid: new FormControl(''),
      productuom_gid: new FormControl(''),
      productgroup_gid: new FormControl(''),
      product_code: new FormControl(''),
      tax_name: new FormControl(''),
      remarks: new FormControl(''),
      product_name: new FormControl(''),
      productuom_name: new FormControl(''),
      productgroup_name: new FormControl('', Validators.required),
      unitprice: new FormControl(''),
      quantity: new FormControl('', Validators.required),
      discount_percentage: new FormControl('', Validators.required),
      discountamount: new FormControl('', Validators.required),
      taxname1: new FormControl('', Validators.required),
      tax_prefix: new FormControl('', Validators.required),
      tax_prefix2: new FormControl(''),
      taxamount1: new FormControl('', Validators.required),
      taxname2: new FormControl('', Validators.required),
      taxamount2: new FormControl(''),
      taxname3: new FormControl('', Validators.required),
      tax_amount3: new FormControl('', Validators.required),
      totalamount: new FormControl('', Validators.required),
      selling_price: new FormControl(''),
      product_requireddate: new FormControl(''),
      product_requireddateremarks: new FormControl(''),
      exchange_rate: new FormControl(''),
      producttype_name: new FormControl(''),
      total_amount: new FormControl(''),
      productquantity: new FormControl(''),
      productunitprice: new FormControl(''),
      productdiscount: new FormControl(''),
      producttotal_amount: new FormControl(''),
      producraddproductgroup_name: new FormControl(''),
      product_remarks: new FormControl(''),
      productdiscount_amountvalue: new FormControl(''),


    });


    const salesorder_gid = this.router.snapshot.paramMap.get('salesorder_gid');
    this.encryt = salesorder_gid;
    const key = 'storyboarderp';
    this.salesorder_gid = AES.decrypt(this.encryt, key).toString(enc.Utf8);
    this.GetRaiseSORenewalSummary(this.salesorder_gid);
    //// Branch Dropdown /////
    //// Sales person Dropdown ////
    // product type
    var api = 'SmrTrnSalesorder/Getproducttypesales';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.producttype_list = this.responsedata.Getproducttypesales;
    });


    var productgroupapi = 'SmrTrnSalesorder/GetProductGroup';
    this.service.get(productgroupapi).subscribe((apiresponse: any) => {
      this.Getproductgroup = apiresponse.Getproductgroup;
    });

    //// Currency Dropdown ////

   



   

    var taxapi = 'SmrTrnSalesorder/GetTax4Dtl';
    this.service.get(taxapi).subscribe((apiresponse: any) => {
      this.tax4_list = apiresponse.GetTax4Dtl;
    });

    var url = 'SmrTrnQuotation/GetTermsandConditions'
    this.service.get(url).subscribe((result: any) => {
      this.terms_list = result.GetTermsandConditions;
    });

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
  }
async GetRaiseSORenewalSummary(salesorder_gid: any) {
this.NgxSpinnerService.show();
    let param = { salesorder_gid: salesorder_gid }
    var summaryapi = 'SmrTrnRenewalsummary/GetRaiseSORenewalSummary';
    this.service.getparams(summaryapi, param).subscribe((apiresponse: any) => {
    this.SO_list = apiresponse.GetsummaryList1;
    this.combinedFormData.get('branch_name')?.setValue(this.SO_list[0].branch_name);
    this.combinedFormData.get('branch_gid')?.setValue(this.SO_list[0].branch_gid);
    // this.combinedFormData.get('salesorder_date')?.setValue(this.SO_list[0].salesorder_date);
    this.combinedFormData.get('customer_name')?.setValue(this.SO_list[0].customer_name);
    this.combinedFormData.get('customer_email')?.setValue(this.SO_list[0].customer_email);
    this.combinedFormData.get('customercontact_names')?.setValue(this.SO_list[0].customercontact_names);
    this.combinedFormData.get('customer_address')?.setValue(this.SO_list[0].address1);
    this.combinedFormData.get('payment_terms')?.setValue(this.SO_list[0].payment_terms);
    this.combinedFormData.get('freight_terms')?.setValue(this.SO_list[0].freight_terms);
    this.combinedFormData.get('currency_code')?.setValue(this.SO_list[0].currency_code);
    this.combinedFormData.get('exchange_rate')?.setValue(this.SO_list[0].exchange_rate);
    this.combinedFormData.get('user_name')?.setValue(this.SO_list[0].user_name);
    this.combinedFormData.get('addon_charge')?.setValue(this.SO_list[0].addon_charge);
    this.combinedFormData.get('freight_charges')?.setValue(this.SO_list[0].freight_charges);
    this.combinedFormData.get('packing_charges')?.setValue(this.SO_list[0].packing_charges);
    this.combinedFormData.get('insurance_charges')?.setValue(this.SO_list[0].insurance_charges);
    this.combinedFormData.get('additional_discount')?.setValue(this.SO_list[0].additional_discount);
    this.combinedFormData.get('delivery_days')?.setValue(this.SO_list[0].delivery_days);
    this.combinedFormData.get('payment_days')?.setValue(this.SO_list[0].payment_days);
    this.combinedFormData.get('so_remarks')?.setValue(this.SO_list[0].so_remarks);
    this.combinedFormData.get('roundoff')?.setValue(this.SO_list[0].roundoff);
    this.combinedFormData.get('shipping_to')?.setValue(this.SO_list[0].address1);
    this.combinedFormData.get('tax_amount4')?.setValue(this.SO_list[0].tax_amount4);
    this.combinedFormData.get('quotation_gid')?.setValue(this.SO_list[0].quotation_gid);

    this.termsandconditions = this.SO_list[0].termsandconditions;
    this.customer_gid = this.SO_list[0].customer_gid;
    this.user_gid = this.SO_list[0].user_gid;
    this.campaign_gid = this.SO_list[0].campaign_gid;
    this.campaign_title = this.SO_list[0].campaign_title;
    this.mdlcustomeradrress = (this.SO_list[0].customercontact_names + '\n' + this.SO_list[0].customer_mobile + '\n'
       + this.SO_list[0].customer_email + ',\n' + this.SO_list[0].customer_address);
    const customer_mobile = this.SO_list[0].customer_mobile;
        const customer_email = this.SO_list[0].customer_email;
        const gst_number = this.SO_list[0].gst_number;
        const customerDetails = `${customer_mobile}\n${customer_email}\n${gst_number}`;
        this.combinedFormData.get("customer_details")?.setValue(customerDetails);

    });    
    this.QuotationtoSOSummary(this.salesorder_gid);
    this.NgxSpinnerService.hide();
  } 
  QuotationtoSOSummary(salesorder_gid: any){
    
    debugger
    let param = { salesorder_gid : salesorder_gid}
    this.NgxSpinnerService.show();
    var productsummaryapi = 'SmrTrnRenewalsummary/GetRenewaltoSOProductSummary';
    this.service.getparams(productsummaryapi, param).subscribe((apiresponse: any)=>{
    this.SOproductsummary(this.salesorder_gid);
    });
    this.NgxSpinnerService.hide();
  }
  onClearCurrency() {
    this.exchange = 0;
  }
  get branch_name() {
    return this.combinedFormData.get('branch_name')!;
  }
  get payment_days() {
    return this.combinedFormData.get('payment_days')!;
  }
  get delivery_days() {
    return this.combinedFormData.get('delivery_days')!;
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
  get product_code() {
    return this.productform.get('product_code')!;
  }
  get tax_name() {
    return this.productform.get('tax_name')!;
  }
  get tax_name2() {
    return this.productform.get('tax_name2')!;
  }
  get tax_name3() {
    return this.productform.get('tax_name3')!;
  }
  get producttype_name() {
    return this.productform.get('producttype_name')!;
  }
  get productgroup_gid() {
    return this.productform.get('productgroup_gid')!;
  }
  get productgroup_name() {
    return this.productform.get('productgroup_name')!;
  }
  getCurrentDate(): string {
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = today.getFullYear();

    return dd + '-' + mm + '-' + yyyy;
  }


  OnChangeCurrency() {

    let currencyexchange_gid = this.combinedFormData.get("currency_code")?.value;

    let param = {
      currencyexchange_gid: currencyexchange_gid
    }
    var url = 'SmrTrnQuotation/GetOnChangeCurrency';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.currency_list1 = this.responsedata.GetOnchangecurrency;
      this.combinedFormData.get("exchange_rate")?.setValue(this.currency_list1[0].exchange_rate);

    })
  }
  onclearproduct() {
    this.productform.get("product_code").setValue('');
    this.productform.get("productgroup_name").setValue('');
    this.productsearch = null;
  }
  onclearproductcode() {
    this.productsearch = null;
    this.productcodesearch = null;
  } 



  OnChangeCustomer1() {
    this.customercontact_names1 = '';
    this.customer_mobile1 = '';
    this.customer_email1 = '';
    this.customer_address1 = '';
  }

  GetOnChangeProductsName1() {
    this.product_code1 = '';
    this.productuom_name1 = '';
    this.unitprice1 = '';
  }

  OnClearTax() {
    this.tax_amount = 0;
    const subtotal = this.exchange_rate * this.unitprice * this.quantity;
    this.totalamount = (+(subtotal - this.tax_amount));
    this.totalamount = +((this.totalamount).toFixed(2))

  }

  getDimensionsByFilter(id: any) {
    return this.tax4_list.filter((x: { tax_gid: any; }) => x.tax_gid === id);
  }
  OnClearOverallTax() {

    this.tax_amount4 = '';
    this.total_price = '';
    this.total_amount = +((+this.producttotalamount) + (+this.tax_amount4));
    this.total_amount = +this.total_amount.toFixed(2);
    this.grandtotal = ((this.total_amount) + (+this.addon_charge) + (+this.freight_charges) +
      (+this.packing_charges) + (+this.insurance_charges) + (+this.roundoff) + (+this.totalamount)
      - (+this.additional_discount));
    this.grandtotal = +this.grandtotal.toFixed(2);
  }
  OnchangeTaxamount4() {
    let tax_gid4 = this.combinedFormData.value.tax_name4;
    this.taxprecentage4 = this.getDimensionsByFilter(tax_gid4);
    let taxprecentage = this.taxprecentage4[0].percentage
    this.tax_amount4 = +(taxprecentage * this.producttotalamount / 100).toFixed(2);;
    this.total_amount = +this.total_amount.toFixed(2);
    this.total_amount = isNaN(this.total_amount) ? 0 : this.total_amount;
    this.tax_amount4 = isNaN(this.tax_amount4) ? 0 : this.tax_amount4;
    this.addon_charge = isNaN(this.addon_charge) ? 0 : this.addon_charge;
    this.freight_charges = isNaN(this.freight_charges) ? 0 : this.freight_charges;
    this.packing_charges = isNaN(this.packing_charges) ? 0 : this.packing_charges;
    this.insurance_charges = isNaN(this.insurance_charges) ? 0 : this.insurance_charges;
    this.roundoff = isNaN(this.roundoff) ? 0 : this.roundoff;
    this.totalamount = isNaN(this.totalamount) ? 0 : this.totalamount;
    this.additional_discount = isNaN(this.additional_discount) ? 0 : this.additional_discount;
    this.grandtotal = this.total_amount + (+this.tax_amount4) + (+this.addon_charge) + (+this.freight_charges) +
      (+this.packing_charges) + (+this.insurance_charges) + (+this.roundoff) + (+this.totalamount) - (+this.additional_discount);
  }



  GetOnChangeTerms() {

    let template_gid = this.combinedFormData.value.template_name;
    let param = {
      template_gid: template_gid
    }
    var url = 'SmrTrnQuotation/GetOnChangeTerms';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.combinedFormData.get("termsandconditions")?.setValue(result.terms_list[0].termsandconditions);
      this.combinedFormData.value.template_gid = result.terms_list[0].template_gid
      //this.cusraiseform.value.productuom_gid = result.GetProductsName[0].productuom_gid
    });
  }

  onSubmit() {
debugger
    var params = {
      customer_gid: this.customer_gid,
      branch_name: this.combinedFormData.value.branch_name,
      branch_gid: this.combinedFormData.value.branch_gid,
      salesorder_date: this.combinedFormData.value.salesorder_date,
      salesorder_gid: this.combinedFormData.value.quotation_gid,
      customer_name: this.combinedFormData.value.customer_name,
      customercontact_names: this.combinedFormData.value.customercontact_names,
      customer_email: this.combinedFormData.value.customer_email,
      customer_mobile: this.combinedFormData.value.customer_mobile,
      so_remarks: this.combinedFormData.value.so_remarks,
      so_referencenumber: this.combinedFormData.value.so_referencenumber,
      customer_address: this.combinedFormData.value.customer_address,
      shipping_to: this.combinedFormData.value.shipping_to,
      freight_terms: this.combinedFormData.value.freight_terms,
      payment_terms: this.combinedFormData.value.payment_terms,
      currency_code: this.combinedFormData.value.currency_code,
      user_name: this.combinedFormData.value.user_name,
      exchange_rate: this.combinedFormData.value.exchange_rate,
      payment_days: this.combinedFormData.value.payment_days,
      termsandconditions: this.SO_list[0].termsandconditions,
      template_name: this.combinedFormData.value.template_name,
      template_gid: this.combinedFormData.value.template_gid,
      grandtotal: this.combinedFormData.value.grandtotal,
      roundoff: this.combinedFormData.value.roundoff,
      start_date: this.combinedFormData.value.start_date,
      end_date: this.combinedFormData.value.end_date,
      insurance_charges: this.combinedFormData.value.insurance_charges,
      packing_charges: this.combinedFormData.value.packing_charges,
      buyback_charges: this.combinedFormData.value.buyback_charges,
      freight_charges: this.combinedFormData.value.freight_charges,
      additional_discount: this.combinedFormData.value.additional_discount,
      addon_charge: this.combinedFormData.value.addon_charge,
      tax_amount4: this.combinedFormData.value.tax_amount4,
      tax_name4: this.combinedFormData.value.tax_name4,
      totalamount: this.combinedFormData.value.totalamount,
      customercontact_gid: this.cuscontact_gid,
      delivery_days: this.combinedFormData.value.delivery_days,
      total_price: this.combinedFormData.value.total_price,
      branch_address: this.combinedFormData.value.branch_address,
      customerinstruction: this.combinedFormData.value.customerinstruction,
      taxsegment_gid: this.taxsegment_gid,
      contactemailaddress: this.combinedFormData.value.contactemailaddress,
      campaign_title: this.campaign_title,
      campaign_gid: this.campaign_gid,
      user_gid: this.user_gid,
    }

    var url = 'SmrTrnRenewalsummary/PostrenewalToOrder'
    this.NgxSpinnerService.show();
    this.service.postparams(url, params).subscribe((result: any) => {
      if (result.status == false) {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message)
      }
      else {
        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message)
        this.route.navigate(['/smr/SmrTrnRenevalsummary']);
      }
    });
  }

  prodcutaddtotalcal() {
    debugger
    const product_gid = this.productform.value.product_name;
    const customer = this.customer_gid;

    if (customer != null && customer != undefined) {
      var producttax = 'SmrTrnSalesorder/GetProductWithTaxSummary';
      let param = {
        product_gid: product_gid,
        customer_gid: customer
      };
      this.service.getparams(producttax, param).subscribe((result: any) => {
        this.GetTaxSegmentList = result.GetTaxSegmentListorder;
        this.productform.get("unitprice")?.setValue(this.GetTaxSegmentList[0].mrp_price)




        this.calculateProductTotal();
      });
    } else {
      window.scrollTo({
        top: 0,
      });
      this.ToastrService.warning('Kindly Select Customer!');
      return;
    }
  }

  calculateProductTotal() {
    const product_gid = this.productform.value.product_name;
    debugger
    if (this.GetTaxSegmentList != null && this.GetTaxSegmentList != undefined) {
      const taxSegment = this.GetTaxSegmentList.filter(
        (segment: { product_gid: string; tax_percentage: number }) => segment.product_gid === product_gid
      );

      let tax_percentage = 0;
      let taxDetails = [];

      if (taxSegment.length > 0) {
        for (let i = 0; i < taxSegment.length; i++) {
          let percentageStr = taxSegment[i].tax_percentage;
          let percentage = parseFloat(percentageStr);
          if (isNaN(percentage)) {
            let numericPart = parseFloat(percentageStr.replace(/[^\d.]/g, ''));
            if (!isNaN(numericPart)) {
              tax_percentage += numericPart;
            }
          } else {
            tax_percentage += percentage;
            taxDetails.push({
              tax_percentage: percentage,
              product_gid: taxSegment[i].product_gid,
              tax_gid: taxSegment[i].tax_gid,
              tax_name: taxSegment[i].tax_name,
              taxsegment_gid: taxSegment[i].taxsegment_gid,
              tax_prefix: taxSegment[i].tax_prefix,
            });
          }
        }
      }

      const subtotal = this.exchange * this.productform.value.unitprice * this.productform.value.productquantity;
      this.productdiscount_amountvalue = (subtotal * this.productform.value.productdiscount) / 100;
      this.productdiscounted_precentagevalue = subtotal - this.productdiscount_amountvalue;

      let individualTaxAmounts = taxDetails.map(detail => {
        let tax_amount = (this.productdiscounted_precentagevalue * detail.tax_percentage) / 100;
        return {
          product_gid: detail.product_gid,
          tax_gid: detail.tax_gid,
          tax_percentage: detail.tax_percentage,
          taxsegment_gid: detail.taxsegment_gid,
          tax_name: detail.tax_name,
          tax_prefix : detail.tax_prefix,
          tax_amount: tax_amount.toFixed(2)
        };
      });

      this.producttotal_tax_amount = individualTaxAmounts.reduce((sum, taxDetail) => sum + parseFloat(taxDetail.tax_amount), 0);

      const total = subtotal - this.productdiscount_amountvalue + this.producttotal_tax_amount;

      this.producttotal_amount = total.toFixed(2);

      this.individualTaxAmounts = individualTaxAmounts;

      if (this.individualTaxAmounts.length > 0) {
        if (this.individualTaxAmounts[0].tax_gid != null && this.individualTaxAmounts[0].tax_gid != undefined) {
          this.taxgid1 = this.individualTaxAmounts[0].tax_gid;
          this.taxname1 = this.individualTaxAmounts[0].tax_name;
          this.tax_prefix = this.individualTaxAmounts[0].tax_prefix;
          this.taxsegment_gid = this.individualTaxAmounts[0].taxsegment_gid;
        } else {
          this.taxgid1 = 0;
        }

        if (this.individualTaxAmounts[0].tax_amount != null && this.individualTaxAmounts[0].tax_amount != undefined) {
          this.taxamount1 = this.individualTaxAmounts[0].tax_amount;
        } else {
          this.taxamount1 = 0;
        }

        if (this.individualTaxAmounts[0].tax_percentage != null && this.individualTaxAmounts[0].tax_percentage != undefined) {
          this.taxprecentage1 = this.individualTaxAmounts[0].tax_percentage;
        } else {
          this.taxprecentage1 = 0;
        }
      } else {
        this.taxgid1 = 0;
        this.taxamount1 = 0;
        this.taxprecentage1 = 0;
      }

      if (this.individualTaxAmounts.length > 1) {
        if (this.individualTaxAmounts[1].tax_gid != null && this.individualTaxAmounts[1].tax_gid != undefined) {
          this.taxgid2 = this.individualTaxAmounts[1].tax_gid;
          this.taxname2 = this.individualTaxAmounts[1].tax_name;
          this.tax_prefix = this.individualTaxAmounts[0].tax_prefix;
          this.tax_prefix2 = this.individualTaxAmounts[1].tax_prefix;
        } else {
          this.taxgid2 = 0;
        }

        if (this.individualTaxAmounts[1].tax_amount != null && this.individualTaxAmounts[1].tax_amount != undefined) {
          this.taxamount2 = this.individualTaxAmounts[1].tax_amount;
        } else {
          this.taxamount2 = 0;
        }

        if (this.individualTaxAmounts[1].tax_percentage != null && this.individualTaxAmounts[1].tax_percentage != undefined) {
          this.taxprecentage2 = this.individualTaxAmounts[1].tax_percentage;
        } else {
          this.taxprecentage2 = 0;
        }
      } else {
        this.taxgid2 = 0;
        this.taxamount2 = 0;
        this.taxprecentage2 = 0;
      }

      if (this.individualTaxAmounts.length > 2) {
        if (this.individualTaxAmounts[2].tax_gid != null && this.individualTaxAmounts[2].tax_gid != undefined) {
          this.taxgid3 = this.individualTaxAmounts[2].tax_gid;
          this.taxname3 = this.individualTaxAmounts[2].tax_name;
          this.tax_prefix = this.individualTaxAmounts[0].tax_prefix;
          this.tax_prefix2 = this.individualTaxAmounts[1].tax_prefix;
        } else {
          this.taxgid3 = 0;
        }

        if (this.individualTaxAmounts[2].tax_amount != null && this.individualTaxAmounts[2].tax_amount != undefined) {
          this.taxamount3 = this.individualTaxAmounts[2].tax_amount;
        } else {
          this.taxamount3 = 0;
        }

        if (this.individualTaxAmounts[2].tax_percentage != null && this.individualTaxAmounts[2].tax_percentage != undefined) {
          this.taxprecentage3 = this.individualTaxAmounts[2].tax_percentage;
        } else {
          this.taxprecentage3 = 0;
        }
      } else {
        this.taxgid3 = 0;
        this.taxamount3 = 0;
        this.taxprecentage3 = 0;
      }

    }
    else {
      const subtotal = this.exchange * this.productform.value.unitprice * this.productform.value.productquantity;
      this.productdiscount_amountvalue = (subtotal * this.productform.value.productdiscount) / 100;
      this.productdiscounted_precentagevalue = subtotal - this.productdiscount_amountvalue;

      const total = subtotal - this.productdiscount_amountvalue + this.producttotal_tax_amount;

      this.producttotal_amount = total.toFixed(2);

      if (this.individualTaxAmounts.length > 0) {
        if (this.individualTaxAmounts[0].tax_gid != null && this.individualTaxAmounts[0].tax_gid != undefined) {
          this.taxgid1 = this.individualTaxAmounts[0].tax_gid;
          this.taxname1 = this.individualTaxAmounts[0].tax_name;
          this.tax_prefix = this.individualTaxAmounts[0].tax_prefix;
          this.taxsegment_gid = this.individualTaxAmounts[0].taxsegment_gid;
        } else {
          this.taxgid1 = 0;
        }

        if (this.individualTaxAmounts[0].tax_amount != null && this.individualTaxAmounts[0].tax_amount != undefined) {
          this.taxamount1 = this.individualTaxAmounts[0].tax_amount;
        } else {
          this.taxamount1 = 0;
        }

        if (this.individualTaxAmounts[0].tax_percentage != null && this.individualTaxAmounts[0].tax_percentage != undefined) {
          this.taxprecentage1 = this.individualTaxAmounts[0].tax_percentage;
        } else {
          this.taxprecentage1 = 0;
        }
      } else {
        this.taxgid1 = 0;
        this.taxamount1 = 0;
        this.taxprecentage1 = 0;
      }

      if (this.individualTaxAmounts.length > 1) {
        if (this.individualTaxAmounts[1].tax_gid != null && this.individualTaxAmounts[1].tax_gid != undefined) {
          this.taxgid2 = this.individualTaxAmounts[1].tax_gid;
          this.taxname2 = this.individualTaxAmounts[1].tax_name;
          this.tax_prefix = this.individualTaxAmounts[0].tax_prefix;
          this.tax_prefix2 = this.individualTaxAmounts[1].tax_prefix;
        } else {
          this.taxgid2 = 0;
        }

        if (this.individualTaxAmounts[1].tax_amount != null && this.individualTaxAmounts[1].tax_amount != undefined) {
          this.taxamount2 = this.individualTaxAmounts[1].tax_amount;
        } else {
          this.taxamount2 = 0;
        }

        if (this.individualTaxAmounts[1].tax_percentage != null && this.individualTaxAmounts[1].tax_percentage != undefined) {
          this.taxprecentage2 = this.individualTaxAmounts[1].tax_percentage;
        } else {
          this.taxprecentage2 = 0;
        }
      } else {
        this.taxgid2 = 0;
        this.taxamount2 = 0;
        this.taxprecentage2 = 0;
      }

      if (this.individualTaxAmounts.length > 2) {
        if (this.individualTaxAmounts[2].tax_gid != null && this.individualTaxAmounts[2].tax_gid != undefined) {
          this.taxgid3 = this.individualTaxAmounts[2].tax_gid;
          this.taxname3 = this.individualTaxAmounts[2].tax_name;
        } else {
          this.taxgid3 = 0;
        }

        if (this.individualTaxAmounts[2].tax_amount != null && this.individualTaxAmounts[2].tax_amount != undefined) {
          this.taxamount3 = this.individualTaxAmounts[2].tax_amount;
        } else {
          this.taxamount3 = 0;
        }

        if (this.individualTaxAmounts[2].tax_percentage != null && this.individualTaxAmounts[2].tax_percentage != undefined) {
          this.taxprecentage3 = this.individualTaxAmounts[2].tax_percentage;
        } else {
          this.taxprecentage3 = 0;
        }
      } else {
        this.taxgid3 = 0;
        this.taxamount3 = 0;
        this.taxprecentage3 = 0;
      }
    }
  }


  finaltotal() {
    debugger
    {
      const total_amount = parseFloat(this.combinedFormData.value.tax_amount4)
      const totalAmount = isNaN((this.combinedFormData.value.totalamount)) ? 0 : (this.combinedFormData.value.totalamount);
      const addoncharges = isNaN(this.combinedFormData.value.addon_charge) ? 0 : +this.combinedFormData.value.addon_charge;
      const frieghtcharges = isNaN(this.combinedFormData.value.freight_charges) ? 0 : +this.combinedFormData.value.freight_charges;
      const forwardingCharges = isNaN(this.combinedFormData.value.buyback_charges) ? 0 : +this.combinedFormData.value.buyback_charges;
      const insurancecharges = isNaN(this.combinedFormData.value.insurance_charges) ? 0 : +this.combinedFormData.value.insurance_charges;
      const packing_charges = isNaN(this.combinedFormData.value.packing_charges) ? 0 : +this.combinedFormData.value.packing_charges;
      const roundoff = isNaN(this.combinedFormData.value.roundoff) ? 0 : +this.combinedFormData.value.roundoff;
      const discountamount = isNaN(this.combinedFormData.value.additional_discount) ? 0 : +this.combinedFormData.value.additional_discount;

      this.grandtotal = ((totalAmount) + (addoncharges) + (total_amount) + (frieghtcharges)  + (roundoff) - (discountamount) + (packing_charges));
      this.grandtotal = +((this.grandtotal).toFixed(2));

    }
  }

  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }

  ondelete() {
    this.NgxSpinnerService.show();
    var url = 'SmrTrnRenewalsummary/GetDeleteDirectSOProductSummary'
    this.NgxSpinnerService.show();
    let param = {
      tmpsalesorderdtl_gid: this.parameterValue
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message)
      }
      else {
        this.SOproductsummary(this.salesorder_gid);
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
        this.productSearch();
        
      }
    });
  }

  close() {
    this.route.navigate(['/smr/SmrTrnSalesorderSummary']);
  }

  // productSubmit() {
  //   debugger
  //   if (this.combinedFormData.value.customer_name == "" || this.combinedFormData.value.customer_name == null || this.combinedFormData.value.customer_name == undefined) {
  //     window.scrollTo({
  //       top: 0,
  //     });
  //     this.ToastrService.warning('Kindly Select Customer!');
  //     return
  //   }
  //   // this.toggleCollapsesection('section3');

  //   const api = 'SmrTrnSalesorder/PostOnAdds';
  //   this.NgxSpinnerService.show();

  //   const spinnerTimer = setTimeout(() => {
  //     this.NgxSpinnerService.hide();
  //   }, 3000);

  //   const params = {
  //     SOProductList: this.SOProductList,
  //     taxsegment_gid: this.Cmntaxsegment_gid,
  //     exchange_rate: this.CurrencyExchangeRate,
  //     customer_gid: this.combinedFormData.value.customer_name.customer_gid,
  //     taxamount1: this.taxamount1,
  //     taxamount2: this.taxamount2,
  //     taxamount3: this.taxamount3,
  //     taxgid1: this.taxgid1,
  //     taxgid2: this.taxgid2,
  //     taxgid3: this.taxgid3,
  //     taxprecentage1: this.taxprecentage1,
  //     taxprecentage2: this.taxprecentage2,
  //     taxprecentage3: this.taxprecentage3,
  //     taxname1: this.taxname1,
  //     taxname2: this.taxname2,
  //     taxname3: this.taxname3,
  //     discount_amount: this.allproductdiscount_amount,
  //     discountprecentageall: this.alldiscounted_subtotal,
  //     total_amount: this.total_amount,

  //   };

  //   this.service.post(api, params).subscribe((result: any) => {
  //     clearTimeout(spinnerTimer); // Clear the spinner timer
  //     if (result.status == false) {
  //       this.ToastrService.warning(result.message);
  //     } else {
  //       this.ToastrService.success(result.message);
  //       this.SOProductList = [];
  //       this.SOproductsummary(this.quotation_gid);
  //     }
  //     this.NgxSpinnerService.hide();
  //   });
  //   this.productSearch();
  // }

  productAddSubmit() {
    debugger
    if (this.combinedFormData.value.customer_name == "" || this.combinedFormData.value.customer_name == null || this.combinedFormData.value.customer_name == undefined) {
      window.scrollTo({
        top: 0,
      });
      this.ToastrService.warning('Kindly Select Customer!');
      return
    }

    var postapi = 'SmrTrnRenewalsummary/GetProductAdd';
    this.NgxSpinnerService.show();

    let param = {
      product_name: this.productform.value.product_name,
      product_code: this.productform.value.product_code,
      producttype_name: this.productform.value.producttype_name,
      productquantity: this.productform.value.productquantity,
      unitprice: this.productform.value.unitprice,
      productdiscount: this.productform.value.productdiscount,
      producttotal_amount: this.productform.value.producttotal_amount,
      exchange_rate: this.combinedFormData.value.exchange_rate,
      customer_gid: this.customer_gid,
      taxamount1: this.taxamount1,
      taxamount2: this.taxamount2,
      taxamount3: this.taxamount3,
      taxgid1: this.taxgid1,
      taxgid2: this.taxgid2,
      taxgid3: this.taxgid3,
      taxprecentage1: this.taxprecentage1,
      taxprecentage2: this.taxprecentage2,
      taxprecentage3: this.taxprecentage3,
      taxname1: this.taxname1,
      taxname2: this.taxname2,
      taxname3: this.taxname3,
      tax_prefix: this.productform.value.tax_prefix,
      discount_amount: this.productdiscount_amountvalue,
      discountprecentage: this.productdiscounted_precentagevalue,
      product_remarks: this.productform.value.product_remarks,
      salesorder_gid: this.salesorder_gid
    }

    this.service.post(postapi, param).subscribe((apiresponse: any) => {
      if (apiresponse.status == false) {
        this.ToastrService.warning(apiresponse.message);
      }
      else {
        this.ToastrService.success(apiresponse.message);
        this.productform.reset();
        this.SOProductList = [];
        this.SOproductsummary(this.salesorder_gid);
        this.productSearch();
      }
    });
    this.NgxSpinnerService.hide();
    this.productSearch();
  }

  productSearch() {
    debugger
    //let customer_gid = this.combinedFormData.get("customer_name")?.value;
    // var params = {
    //   producttype_gid: this.productform.value.producttype_name,
    //   product_name: this.productform.value.product_name,
    //   customer_gid: customer_gid.customer_gid
    // };
    var api = 'SmrTrnSalesorder/GetProductsearchSummarySales';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.SOProductList = this.responsedata.GetProductsearchs;
      this.filteredSOProductList = this.SOProductList;
      //this.GetTaxSegmentList = this.responsedata.GetTaxSegmentListorder;
    });
  }

  searchOnChange(event: KeyboardEvent) {
    if (event.key !== 'Enter') {
      this.productSearch();
    }
  }

  SOproductsummary(salesorder_gid:any) {
    debugger
    this.NgxSpinnerService.show();
   let param ={ salesorder_gid : salesorder_gid}
    var api = 'SmrTrnRenewalsummary/GetTemporarySummary';

    this.service.getparams(api, param).subscribe((result: any) => {
      this.responsedata = result;
      this.salesorders_list = this.responsedata.temp_list1;

      this.totalamount = this.responsedata.grandtotal.toFixed(2);
      this.producttotalamount = this.responsedata.grandtotal.toFixed(2);
      this.combinedFormData.get("totalamount")?.setValue(this.responsedata.grandtotal.toFixed(2));
      this.combinedFormData.get("grandtotal")?.setValue(this.responsedata.grandtotal.toFixed(2));
      this.currency_code1 = "";
      this.combinedFormData.get("totalamount")?.setValue(result.grand_total);
      this.combinedFormData.get("grandtotal")?.setValue(result.grandtotal);
      // if (this.tax_name4 == null || this.tax_name4 == "" || this.tax_name4 == "--No Tax--") {
      //   this.combinedFormData.get("grandtotal")?.setValue(result.grandtotal);
      // }
    });
    this.NgxSpinnerService.hide();
  }

  /// -------------------------------------new ui changes ------------------------------------------///  

  GetOnChangeProductsGroup() {
    debugger
    let productgroup_gid = this.productform.value.productgroup_name;
    let param = {
      productgroup_gid: productgroup_gid
    };

    var url = 'SmrTrnSalesorder/GetOnChangeProductGroup';
    this.NgxSpinnerService.show();
    this.service.getparams(url, param).subscribe((result: any) => {
      this.productgroup_list = result.GetCustomer;
      this.NgxSpinnerService.hide();
    });
    this.NgxSpinnerService.hide();
  }

  GetOnChangeProductsGroupName() {
    let product_gid = this.productform.value.product_name;
    let param = {
      product_gid: product_gid
    };

    var url = 'SmrTrnSalesorder/GetOnChangeProductGroupName';
    this.NgxSpinnerService.show();
    this.service.getparams(url, param).subscribe((result: any) => {
      this.productform.get("product_code")?.setValue(result.GetProdGrpName[0].product_code);
      this.NgxSpinnerService.hide();
    });
    this.NgxSpinnerService.hide();
  }
  /// ----------------------------------------fliter ------------------------------- -----------------------------//
  onProductSelect(event: any): void {
    debugger

    const product_name = this.SOProductList.find(product => product.product_gid === event.product_gid);
    if (product_name) {
      this.productform.patchValue({
        product_code: product_name.product_code,
        producraddproductgroup_name: product_name.productgroup_gid
      });
    }
  }


  OnProductCode(event: any) {
    const product_code = this.SOProductList.find(product => product.product_code === event.product_code);
    if (product_code) {
      this.productform.patchValue({
        product_name: product_code.product_gid,
        producraddproductgroup_name: product_code.productgroup_gid
      });
    }
  }
  onKeyPress(event: any) {
    const key = event.key;
    if (!/^[0-9.]$/.test(key)) {
      event.preventDefault();
    }
  }


  // prodtotalcal(product: any) {
  //   const { product_gid, unitprice, quantity, discount_percentage } = product;
  //   const customer = this.combinedFormData.value.customer_name?.customer_gid;

  //   if (customer != null && customer != undefined) {
  //     var producttax = 'SmrTrnSalesorder/GetProductWithTaxSummary';
  //     let param = {
  //       product_gid: product_gid,
  //       customer_gid: customer
  //     };
  //     this.service.getparams(producttax, param).subscribe((result: any) => {
  //       this.GetTaxSegmentList = result.GetTaxSegmentListorder;
  //       this.bulkaddProductTotal(product, product_gid, unitprice, quantity, discount_percentage);
  //     });
  //   } else {
  //     window.scrollTo({
  //       top: 0,
  //     });
  //     this.ToastrService.warning('Kindly Select Customer!');
  //     return;
  //   }
  // }

  // bulkaddProductTotal(product: any, product_gid: any, unitprice: any, quantity: any, discount_percentage: any) {

  //   if (this.GetTaxSegmentList != null && this.GetTaxSegmentList != undefined) {
  //     const taxSegment = this.GetTaxSegmentList.filter(
  //       (segment: { product_gid: string; tax_percentage: number }) => segment.product_gid === product_gid
  //     );

  //     let tax_percentage = 0;
  //     let taxDetails = [];

  //     if (taxSegment.length > 0) {
  //       for (let i = 0; i < taxSegment.length; i++) {
  //         let percentageStr = taxSegment[i].tax_percentage;
  //         let percentage = parseFloat(percentageStr);
  //         if (isNaN(percentage)) {
  //           let numericPart = parseFloat(percentageStr.replace(/[^\d.]/g, ''));
  //           if (!isNaN(numericPart)) {
  //             tax_percentage += numericPart;
  //           }
  //         } else {
  //           tax_percentage += percentage;
  //           taxDetails.push({
  //             tax_percentage: percentage,
  //             product_gid: taxSegment[i].product_gid,
  //             tax_gid: taxSegment[i].tax_gid,
  //             tax_name: taxSegment[i].tax_name,
  //           });
  //         }
  //       }
  //     }

  //     const subtotal = this.exchange * unitprice * quantity;
  //     this.allproductdiscount_amount = (subtotal * discount_percentage) / 100;
  //     this.alldiscounted_subtotal = subtotal - this.allproductdiscount_amount;

  //     let individualTaxAmounts = taxDetails.map(detail => {
  //       let tax_amount = (this.alldiscounted_subtotal * detail.tax_percentage) / 100;
  //       return {
  //         product_gid: detail.product_gid,
  //         tax_gid: detail.tax_gid,
  //         tax_percentage: detail.tax_percentage,
  //         tax_name: detail.tax_name,
  //         tax_amount: tax_amount.toFixed(2),
  //       };
  //     });

  //     this.total_tax_amount = individualTaxAmounts.reduce((sum, taxDetail) => sum + parseFloat(taxDetail.tax_amount), 0);

  //     const total = subtotal - this.allproductdiscount_amount + this.total_tax_amount;

  //     this.total_amount = total.toFixed(2);

  //     product.total_amount = this.total_amount;

  //     this.individualTaxAmounts = individualTaxAmounts;

  //     this.setIndividualTaxAmounts();
  //   }
  //   else {

  //     const subtotal = this.combinedFormData.value.exchange_rate * unitprice * quantity;
  //     this.allproductdiscount_amount = (subtotal * discount_percentage) / 100;
  //     this.alldiscounted_subtotal = subtotal - this.allproductdiscount_amount;
  //     // this.total_tax_amount = individualTaxAmounts.reduce((sum, taxDetail) => sum + parseFloat(taxDetail.tax_amount), 0);

  //     const total = subtotal - this.allproductdiscount_amount + this.total_tax_amount;

  //     this.total_amount = total.toFixed(2);

  //     product.total_amount = this.total_amount;

  //     this.setIndividualTaxAmounts();
  //   }
  // }

  // setIndividualTaxAmounts() {
  //   if (this.individualTaxAmounts.length > 0) {
  //     const firstTax = this.individualTaxAmounts[0];
  //     this.taxgid1 = firstTax.tax_gid ?? 0;
  //     this.taxname1 = firstTax.tax_name ?? '';
  //     this.taxamount1 = firstTax.tax_amount ?? 0;
  //     this.taxprecentage1 = firstTax.tax_percentage ?? 0;
  //   } else {
  //     this.taxgid1 = 0;
  //     this.taxamount1 = 0;
  //     this.taxprecentage1 = 0;
  //   }

  //   if (this.individualTaxAmounts.length > 1) {
  //     const secondTax = this.individualTaxAmounts[1];
  //     this.taxgid2 = secondTax.tax_gid ?? 0;
  //     this.taxname2 = secondTax.tax_name ?? '';
  //     this.taxamount2 = secondTax.tax_amount ?? 0;
  //     this.taxprecentage2 = secondTax.tax_percentage ?? 0;
  //   } else {
  //     this.taxgid2 = 0;
  //     this.taxamount2 = 0;
  //     this.taxprecentage2 = 0;
  //   }

  //   if (this.individualTaxAmounts.length > 2) {
  //     const thirdTax = this.individualTaxAmounts[2];
  //     this.taxgid3 = thirdTax.tax_gid ?? 0;
  //     this.taxname3 = thirdTax.tax_name ?? '';
  //     this.taxamount3 = thirdTax.tax_amount ?? 0;
  //     this.taxprecentage3 = thirdTax.tax_percentage ?? 0;
  //   } else {
  //     this.taxgid3 = 0;
  //     this.taxamount3 = 0;
  //     this.taxprecentage3 = 0;
  //   }
  // }

} 

