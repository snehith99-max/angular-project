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
import { filter, Observable, ReplaySubject, Subject } from 'rxjs';
import { FilterPipe } from 'src/app/Service/filter';
import { Table } from 'jspdf-autotable';

@Component({
  selector: 'app-smr-trn-portalorders-customer-approval',
  templateUrl: './smr-trn-portalorders-customer-approval.component.html',
  styleUrls: ['./smr-trn-portalorders-customer-approval.component.scss']
})
export class SmrTrnPortalordersCustomerApprovalComponent {
  showUpdateButton: boolean = false;
  showAddButton: boolean = true;
  config: AngularEditorConfig = {
    editable: false,
    spellcheck: true,
    height: '33rem',
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
  PortalCustomerSalesorderViewdetails_list: any[] = [];
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
  encryt2: any;
  salesorder_gid: any;
  customer_gid: any;
  termsandconditions: any;
  campaign_title: any;
  campaign_gid: any;
  user_gid: any;
  mdldeliverydays: any;
  mdldpaymentdays: any;


  constructor(private http: HttpClient,
    private fb: FormBuilder, private renderer: Renderer2,
    private router: ActivatedRoute, private route: Router, private service: SocketService, private ToastrService: ToastrService, public NgxSpinnerService: NgxSpinnerService,) {
    
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
      taxamount1: new FormControl('', Validators.required),
      taxname2: new FormControl('', Validators.required),
      taxamount2: new FormControl('', Validators.required),
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


    const salesorder_gid = this.router.snapshot.paramMap.get('salesordergid1');
    const customer_gid = this.router.snapshot.paramMap.get('customergid1');
    this.encryt = salesorder_gid;
    this.encryt2 = customer_gid;
    const key = 'storyboarderp';
    this.salesorder_gid = AES.decrypt(this.encryt, key).toString(enc.Utf8);
    this.customer_gid = AES.decrypt(this.encryt2, key).toString(enc.Utf8);

    this.GetQuotationSummary(this.salesorder_gid);
    
    
    //// Branch Dropdown /////
   

    var url = 'SmrTrnSalesorder/GetBranchDtl'
    this.service.get(url).subscribe((result: any) => {
      this.branch_list = result.GetBranchDtl;
      const firstBranch = this.branch_list[0];
      const branchName = firstBranch.branch_gid;
      this.combinedFormData.get('branch_name')?.setValue(branchName);
      this.combinedFormData.get('branch_address')?.setValue(this.branch_list[0].address1)

    });
   

  
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

    var url = 'SmrTrnQuotation/GetCurrencyDtl'
    this.service.get(url).subscribe((result: any) => {
      this.currency_list = result.GetCurrencyDt;
      this.mdlCurrencyName = this.currency_list[0].currencyexchange_gid;
      const defaultCurrency = this.currency_list.find(currency => currency.default_currency === 'Y');
      const defaultCurrencyExchangeRate = defaultCurrency.exchange_rate;
      if (defaultCurrency) {
        this.mdlCurrencyName = defaultCurrency.currencyexchange_gid;
        this.combinedFormData.get("exchange_rate")?.setValue(defaultCurrencyExchangeRate);
      }
      this.CurrencyName = defaultCurrency.currency_code;
      this.CurrencyExchangeRate = defaultCurrencyExchangeRate;
    });



   

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

  async GetQuotationSummary(salesorder_gid: any) {
debugger
this.NgxSpinnerService.show();
    let param = { salesorder_gid: salesorder_gid }
    var summaryapi = 'CustomerPortalOrders/GetPortalCustomerViewsalesorderSummary';
    this.service.getparams(summaryapi, param).subscribe((apiresponse: any) => {
    this.SO_list = apiresponse.PortalCustomerSalesorderView_list; 
    
    });    
    this.QuotationtoSOSummary(this.salesorder_gid);
    this.NgxSpinnerService.hide();
  } 
  QuotationtoSOSummary(salesorder_gid: any){
    
    debugger
    let param = { salesorder_gid : salesorder_gid}
    this.NgxSpinnerService.show();
    var productsummaryapi = 'CustomerPortalOrders/GetPortalCustomerViewsalesorderDetails';
    this.service.getparams(productsummaryapi, param).subscribe((apiresponse: any)=>{
      this.PortalCustomerSalesorderViewdetails_list = apiresponse.PortalCustomerSalesorderViewdetails_list;
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

  OnChangeCustomer() {

    let customer_gid = this.combinedFormData.value.customer_name.customer_gid;
    let param = {
      customer_gid: customer_gid
    }
    var url = 'SmrTrnSalesorder/GetOnChangeCustomer';

    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.GetCustomerDet = this.responsedata.GetCustomer;

      this.combinedFormData.get("customer_mobile")?.setValue(result.GetCustomer[0].customer_mobile);
      this.combinedFormData.get("customercontact_names")?.setValue(result.GetCustomer[0].customercontact_names);
      this.combinedFormData.get("customer_address")?.setValue(result.GetCustomer[0].customer_address);
      this.combinedFormData.value.leadbank_gid = result.GetCustomer[0].leadbank_gid;
      this.combinedFormData.get("customer_email")?.setValue(result.GetCustomer[0].customer_email);
      this.cuscontact_gid = this.combinedFormData.value.customercontact_gid;
      this.Cmntaxsegment_gid = result.GetCustomer[0].taxsegment_gid;
      this.mdlcustomeradrress = (this.GetCustomerDet[0].customercontact_names + '\n' 
        + this.GetCustomerDet[0].customer_mobile + '\n' +
         this.GetCustomerDet[0].customer_email + ',\n' + 
         this.GetCustomerDet[0].customer_address)
         const customer_mobile = this.GetCustomerDet[0].customer_mobile;
         const customer_email = this.GetCustomerDet[0].email;
         const gst_number = this.GetCustomerDet[0].gst_number;
         const customerDetails = `${customer_mobile}\n${customer_email}\n${gst_number}`;
         this.combinedFormData.get("customer_details")?.setValue(customerDetails);

      //this.combinedFormData.value.leadbank_gid = result.GetCustomer[0].leadbank_gid
      this.productSearch();
      this.SOproductsummary(this.salesorder_gid);
    });

  }


  onSubmit() {

   let params = { salesorder_gid : this.salesorder_gid, customer_gid: this.customer_gid}

    var url = 'CustomerPortalOrders/PostCustomerOrderTOSalesOrder'
    this.NgxSpinnerService.show();
    this.service.getparams(url, params).subscribe((result: any) => {
      if (result.status == false) {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message)
      }
      else {
        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message)
        this.route.navigate(['/smr/SmrTrnPortalOrderSummary']);
      }
    });
  }

  

  

  

  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }

  ondelete() {
    this.NgxSpinnerService.show();
    var url = 'SmrTrnSalesorder/GetDeleteDirectSOProductSummary'
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

  
  productAddSubmit() {
    debugger
    if (this.combinedFormData.value.customer_name == "" || this.combinedFormData.value.customer_name == null || this.combinedFormData.value.customer_name == undefined) {
      window.scrollTo({
        top: 0,
      });
      this.ToastrService.warning('Kindly Select Customer!');
      return
    }

    var postapi = 'SmrQuoteToOrder/GetProductAdd';
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
    var api = 'CustomerPortalOrders/GetTempProductSummary';

    this.service.getparams(api, param).subscribe((result: any) => {
      this.responsedata = result;
      this.salesorders_list = this.responsedata.CustomerPortalGettemporarysummary;

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

  
  /// ----------------------------------------fliter ------------------------------- -----------------------------//
  
  
}
