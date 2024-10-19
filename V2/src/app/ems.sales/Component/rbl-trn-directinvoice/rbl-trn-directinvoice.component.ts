import { HttpClient } from '@angular/common/http';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { AES, enc } from 'crypto-js';
import { map, param, valHooks } from 'jquery';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { NgxSpinnerService } from 'ngx-spinner';
import { Component, OnInit, Pipe, ElementRef, ViewChild, Renderer2 } from '@angular/core';
import { dE } from '@fullcalendar/core/internal-common';
import { filter, Observable, Subject } from 'rxjs';
import { FilterPipe } from 'src/app/Service/filter';
import { Table } from 'jspdf-autotable';
import { DatePipe } from '@angular/common';


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
  selector: 'app-rbl-trn-directinvoice',
  templateUrl: './rbl-trn-directinvoice.component.html',
  styleUrls: ['./rbl-trn-directinvoice.component.scss']
})


export class RblTrnDirectinvoiceComponent {


  showUpdateButton: boolean = false;
  showAddButton: boolean = true;
  config: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '21rem',
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
  salestype_list: any;
  allchargeslist: any[] = [];
  filteredProductCodes: any[] = [];
  customercontact_name: any;
  branch_list: any[] = [];
  contact_list: any[] = [];
  currency_list: any[] = [];
  directinvoiceproductsummary_list: any[] = [];
  user_list: any[] = [];
  product_list: any[] = [];
  tax4_list: any[] = [];
  calci_list: any[] = [];
  GetTaxSegmentListorder: any[] = [];
  terms_list: any[] = [];
  mdlBranchName: any;
  addressdetails: any;
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
  tax_amount: any;
  buyback_charges: number = 0;
  tax_amount4: number = 0;
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
  tax_prefix2: any;
  tax_prefix3: any;
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
  filteredDIProductList: any[] = [];
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
  DIProductList: any[] = [];
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
  user_gid: any;
  mdldeliverydays: any;
  dispatch_mode: any;
  billemail: any;
  mdldpaymentdays: any;
  appointment_gid: any;
  decryt_appointment_gid: any;
  decryt_customer_gid: any;


  constructor(private http: HttpClient,
    private fb: FormBuilder, private renderer: Renderer2, private datePipe: DatePipe,
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


    var url = "SmrRptInvoiceReport/LoadPage";
    this.service.get(url).subscribe((result: any) => {
    })

    this.combinedFormData = new FormGroup({
      invoice_date: new FormControl(this.getCurrentDate()),
      branch_name: new FormControl(''),
      branch_gid: new FormControl(''),
      order_refno: new FormControl(''),
      customer_name: new FormControl('', Validators.required
      ),
      customer_gid: new FormControl(''),
      customercontact_names: new FormControl(''),
      customercontact_gid: new FormControl(''),
      customer_mobile: new FormControl(''),
      customer_email: new FormControl(''),
      customer_address: new FormControl(''),
      shipping_to: new FormControl(''),
      dispatch_mode: new FormControl(''),
      tax_amount4: new FormControl(''),
      payment_terms: new FormControl(''),
      currencyexchange_gid: new FormControl(''),
      currency_code: new FormControl('', Validators.required),
      exchange_rate: new FormControl(''),
      payment_days: new FormControl('', Validators.required),
      delivery_days: new FormControl('', Validators.required),
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
      due_date: new FormControl(this.getCurrentDate()),
      bill_email: new FormControl(this.getCurrentDate()),
      customer_details: new FormControl(''),
      salestype_name: new FormControl(''),
      salestype_gid: new FormControl('')

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
      tax_prefix2: new FormControl('', Validators.required),
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
      product_desc: new FormControl(''),
      productdiscount_amountvalue: new FormControl(''),


    });
    var api = 'SmrMstSalesConfig/GetAllChargesConfig';
    debugger
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.allchargeslist = this.responsedata.salesconfigalllist;
      // this.addon_charge = this.allchargeslist[0].flag;
      // this.additional_discount = this.allchargeslist[1].flag;
      // this.freight_charges = this.allchargeslist[2].flag;
      // this.buyback_charges = this.allchargeslist[3].flag;
      // this.insurance_charges = this.allchargeslist[4].flag;
      if (this.allchargeslist[0].flag == 'Y') {
        this.addon_charge = 0;
      } else {
        // this.addon_charge = this.allchargeslist[0].flag;
        this.combinedFormData.get("addon_charge")?.setValue(0);
      }

      if (this.allchargeslist[1].flag == 'Y') {
        this.additional_discount = 0;
      } else {
        // this.additional_discount = this.allchargeslist[1].flag;
        this.combinedFormData.get("additional_discount")?.setValue(0);
      }

      if (this.allchargeslist[2].flag == 'Y') {
        this.freight_charges = 0;
      } else {
        // this.freight_charges = this.allchargeslist[2].flag;
        this.combinedFormData.get("freight_charges")?.setValue(0);
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



    var url = 'SmrTrnSalesorder/GetBranchDtl'
    this.service.get(url).subscribe((result: any) => {
      this.branch_list = result.GetBranchDtl;
      const firstBranch = this.branch_list[0];
      const branchName = firstBranch.branch_gid;
      this.combinedFormData.get('branch_name')?.setValue(branchName);
      this.combinedFormData.get('branch_address')?.setValue(this.branch_list[0].address1)

    });

    var api = 'SmrTrnSalesorder/Getproducttypesales';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.producttype_list = this.responsedata.Getproducttypesales;
    });


    var url = 'SmrRptInvoiceReport/GetSalesType'
    this.service.get(url).subscribe((result: any) => {
      this.salestype_list = result.Getsalestype;
    });


    var productgroupapi = 'SmrTrnSalesorder/GetProductGroup';
    this.service.get(productgroupapi).subscribe((apiresponse: any) => {
      this.Getproductgroup = apiresponse.Getproductgroup;
    });

    var taxapi = 'SmrTrnSalesorder/GetTax4Dtl';
    this.service.get(taxapi).subscribe((apiresponse: any) => {
      this.tax4_list = apiresponse.GetTax4Dtl;
    });

    var url = 'SmrTrnQuotation/GetTermsandConditions'
    this.service.get(url).subscribe((result: any) => {
      this.terms_list = result.GetTermsandConditions;
    });

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

    this.combinedFormData.get('payment_days')?.valueChanges.subscribe(() => {
      this.calculateProbationEndDate();
    });

    const secretKey = 'storyboarderp';
    this.decryt_customer_gid = sessionStorage.getItem('CRM_CUSTOMER_GID_QUOTATION');
    this.decryt_appointment_gid = sessionStorage.getItem('CRM_APPOINTMENT_GID');
    if (this.decryt_customer_gid != null && this.decryt_appointment_gid != null) {
      this.customer_gid = AES.decrypt(this.decryt_customer_gid, secretKey).toString(enc.Utf8);
      this.appointment_gid = AES.decrypt(this.decryt_appointment_gid, secretKey).toString(enc.Utf8);
      if (this.customer_gid != null && this.appointment_gid != null) {
        this.OnChangeCustomer();
      }
    }
    else if(this.decryt_customer_gid != null){
      this.customer_gid = AES.decrypt(this.decryt_customer_gid, secretKey).toString(enc.Utf8);
      this.OnChangeCustomer();
      var url = 'SmrTrnSalesorder/GetCustomerDtl'
      this.service.get(url).subscribe((result: any) => {
        this.customer_list = result.GetCustomerDtl;
      });
    }
    else {
      sessionStorage.removeItem('CRM_APPOINTMENT_GID');
      sessionStorage.removeItem('CRM_CUSTOMER_GID_QUOTATION');
      //// Customer Dropdown /////
      var url = 'SmrTrnSalesorder/GetCustomerDtl'
      this.service.get(url).subscribe((result: any) => {
        this.customer_list = result.GetCustomerDtl;
      });
    }
  }
  onback() {
    window.history.back();
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
debugger
    let param;
    if (this.decryt_customer_gid != null && this.decryt_appointment_gid != null) {
      let customer_gid = this.customer_gid;
      param = {
        customer_gid: customer_gid
      }
    }
    else if(this.decryt_customer_gid != null){
      let customer_gid = this.customer_gid;
      param = {
        customer_gid: customer_gid
      }
    }
    else {
      let customer_gid = this.combinedFormData.value.customer_name.customer_gid;
      param = {
        customer_gid: customer_gid
      }
    }
    var url = 'SmrTrnSalesorder/GetOnChangeCustomer';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.GetCustomerDet = this.responsedata.GetCustomer;

      this.combinedFormData.get("customer_mobile")?.setValue(result.GetCustomer[0].customer_mobile);
      this.combinedFormData.get("customercontact_names")?.setValue(result.GetCustomer[0].customercontact_names);
      // this.combinedFormData.get("customer_address")?.setValue(result.GetCustomer[0].address1);
      // this.combinedFormData.get("shipping_to")?.setValue(result.GetCustomer[0].address2);
      this.combinedFormData.get("customer_gid")?.setValue(this.customer_gid);
      this.combinedFormData.value.leadbank_gid = result.GetCustomer[0].leadbank_gid;
      this.combinedFormData.get("customer_email")?.setValue(result.GetCustomer[0].customer_email);
      this.cuscontact_gid = this.combinedFormData.value.customercontact_gid;
      this.Cmntaxsegment_gid = result.GetCustomer[0].taxsegment_gid;
      this.mdlcustomeradrress = 
      (this.GetCustomerDet[0].customercontact_names + '\n' + 
      this.GetCustomerDet[0].customer_mobile + '\n' + this.GetCustomerDet[0].customer_email + ',\n' +
      this.GetCustomerDet[0].address1);
      const customer_mobile = this.GetCustomerDet[0].customer_mobile;
      const customer_email = this.GetCustomerDet[0].customer_email;
      const gst_number = this.GetCustomerDet[0].gst_number;
      const customerDetails = `${customer_mobile}\n${customer_email}\n${gst_number}`;
      this.combinedFormData.get("customer_details")?.setValue(customerDetails);
      debugger
      const address1 = this.GetCustomerDet[0].address1;
      let address2 = this.GetCustomerDet[0].address2;
      const zip_code = this.GetCustomerDet[0].zip_code;
      const city = this.GetCustomerDet[0].city;

      if (address2 === null || address2 === undefined || address2.trim() === '') {
        this.addressdetails = `${address1}\n${city}\n${zip_code}`;
      }
      else {
        this.addressdetails = `${address1}\n${address2}\n${city}\n${zip_code}`;
      }

      this.combinedFormData.get("shipping_to")?.setValue(this.addressdetails);
      this.combinedFormData.get("customer_address")?.setValue(this.addressdetails);
      if (this.decryt_customer_gid != null && this.decryt_appointment_gid != null) {
      this.combinedFormData.get("customer_name")?.setValue(this.GetCustomerDet[0].customer_name);
      
      }
      else if(this.decryt_customer_gid != null){
        this.combinedFormData.get("customer_name")?.setValue(this.GetCustomerDet[0].customer_name);
      }
      this.productSearch();
      this.DIProductSummmary();
    });

  }

  OnChangeBranch(branch_gid: any) {
    debugger

    let param = { branch_gid: branch_gid.branch_gid }
    var branchapi = 'SmrTrnSalesorder/GetOnChangeBranch';
    this.service.getparams(branchapi, param).subscribe((apiresponse: any) => {
      this.branchaddresslist = apiresponse.GetOnChangeBranch_list;

      this.combinedFormData.get('branch_address')?.setValue(this.branchaddresslist[0].address1)
    });
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
    this.productform.get("producraddproductgroup_name").setValue('');
    this.productform.get("product_desc").setValue('');
    this.productform.get("unitprice").setValue('');
    this.productsearch = null;
  }
  onclearproductcode() {
    this.productsearch = null;
    this.productcodesearch = null;
    this.productform.get("product_desc").setValue('');
  }

  // onSearch(term:any){
  //   debugger
  //   if (term.length >= 3) {
  //     this.filteredSOProductList = this.SOProductList.filter(product =>
  //       product.product_name.toLowerCase().includes(term.toLowerCase())
  //     );
  //   } else {
  //     this.filteredSOProductList = this.SOProductList;
  //   }

  // }

  OnChangeCustomer1() {
    this.combinedFormData.get("customer_details")?.setValue('');
    this.combinedFormData.get("shipping_to")?.setValue('');
    this.combinedFormData.get("customer_address")?.setValue('');
  }

  GetOnChangeProductsName1() {
    this.product_code1 = '';
    this.productuom_name1 = '';
    this.unitprice1 = '';
  }

  OnClearTax() {
    this.tax_amount = 0;
    const subtotal = this.unitprice * this.quantity;
    this.totalamount = (+(subtotal - this.tax_amount));
    this.totalamount = +((this.totalamount).toFixed(2))

  }

  getDimensionsByFilter(id: any) {
    return this.tax4_list.filter((x: { tax_gid: any; }) => x.tax_gid === id);
  }
  OnClearOverallTax() {
    this.tax_amount4=0;
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

  onCurrencyCodeChange(event: Event) {


    const target = event.target as HTMLSelectElement;
    const selectedCurrencyCode = target.value;

    this.selectedCurrencyCode = selectedCurrencyCode;
    this.combinedFormData.controls.currency_code.setValue(selectedCurrencyCode);
    this.combinedFormData.get("currency_code")?.setValue(this.currency_list[0].currency_code);

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
    if (this.directinvoiceproductsummary_list == null || this.directinvoiceproductsummary_list == undefined

    ) {
      window.scrollTo({
        top: 0,
      });

      this.ToastrService.warning('Atleast One Product Must Be Added!');
      return
    }
    if (
      this.combinedFormData.value.customer_name == "" ||
      this.combinedFormData.value.customer_name == null ||
      this.combinedFormData.value.customer_name == undefined
    ) {
      window.scrollTo({
        top: 0,
      });
      this.ToastrService.warning('Kindly Fill all the Mandatory Fields!');
      return
    }

    let params;
    if (this.decryt_customer_gid != null && this.decryt_appointment_gid != null) {
      params = {
        customer_gid: this.customer_gid,
        branch_name: this.combinedFormData.value.branch_name,
        invoice_date: this.combinedFormData.value.invoice_date,
        customer_name: this.combinedFormData.value.customer_name,
        customeraddress: this.combinedFormData.value.customer_address,
        customer_details: this.combinedFormData.value.customer_details,
        order_refno: this.combinedFormData.value.order_refno,
        shipping_to: this.combinedFormData.value.shipping_to,
        dispatch_mode: this.combinedFormData.value.dispatch_mode,
        currency_code: this.combinedFormData.value.currency_code,
        exchange_rate: this.combinedFormData.value.exchange_rate,
        payment_days: this.combinedFormData.value.payment_days,
        termsandconditions: this.combinedFormData.value.termsandconditions,
        template_name: this.combinedFormData.value.template_name,
        template_gid: this.combinedFormData.value.template_gid,
        due_date: this.combinedFormData.value.due_date,
        bill_email: this.combinedFormData.value.bill_email,
        grandtotal: this.combinedFormData.value.grandtotal,
        roundoff: this.combinedFormData.value.roundoff,
        insurance_charges: this.combinedFormData.value.insurance_charges,
        freight_charges: this.combinedFormData.value.freight_charges,
        additional_discount: this.combinedFormData.value.additional_discount,
        addon_charge: this.combinedFormData.value.addon_charge,
        tax_amount4: this.combinedFormData.value.tax_amount4,
        tax_name4: this.combinedFormData.value.tax_name4,
        totalamount: this.combinedFormData.value.totalamount,
        delivery_days: this.combinedFormData.value.delivery_days,
        sales_type: this.combinedFormData.value.salestype_name
      }
    }
    else {
      params = {
        customer_gid: this.combinedFormData.value.customer_name.customer_gid,
        branch_name: this.combinedFormData.value.branch_name,
        invoice_date: this.combinedFormData.value.invoice_date,
        customer_name: this.combinedFormData.value.customer_name.customer_name,
        customeraddress: this.combinedFormData.value.customer_address,
        customer_details: this.combinedFormData.value.customer_details,
        order_refno: this.combinedFormData.value.order_refno,
        shipping_to: this.combinedFormData.value.shipping_to,
        dispatch_mode: this.combinedFormData.value.dispatch_mode,
        currency_code: this.combinedFormData.value.currency_code,
        exchange_rate: this.combinedFormData.value.exchange_rate,
        payment_days: this.combinedFormData.value.payment_days,
        termsandconditions: this.combinedFormData.value.termsandconditions,
        template_name: this.combinedFormData.value.template_name,
        template_gid: this.combinedFormData.value.template_gid,
        due_date: this.combinedFormData.value.due_date,
        bill_email: this.combinedFormData.value.bill_email,
        grandtotal: this.combinedFormData.value.grandtotal,
        roundoff: this.combinedFormData.value.roundoff,
        insurance_charges: this.combinedFormData.value.insurance_charges,
        freight_charges: this.combinedFormData.value.freight_charges,
        additional_discount: this.combinedFormData.value.additional_discount,
        addon_charge: this.combinedFormData.value.addon_charge,
        tax_amount4: this.combinedFormData.value.tax_amount4,
        tax_name4: this.combinedFormData.value.tax_name4,
        totalamount: this.combinedFormData.value.totalamount,
        delivery_days: this.combinedFormData.value.delivery_days,
        sales_type: this.combinedFormData.value.salestype_name
      }
    }

    var url = 'SmrRptInvoiceReport/SalesInvoiceSubmit'
    this.NgxSpinnerService.show();
    this.service.postparams(url, params).subscribe((result: any) => {
      if (result.status == false) {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message)
      }
      else {
        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message);
        if (this.decryt_customer_gid != null && this.decryt_appointment_gid != null) {
          this.Leadstage_update();
        }
        window.history.back();
        //this.route.navigate(['/smr/SmrTrnInvoiceSummary']);
      }
    });
  }
  Leadstage_update() {
    if (this.appointment_gid != null) {
      let param = {
        call_response: "Purchase",
        appointment_gid: this.appointment_gid,
      }
      var url = 'Leadbank360/PostLeadStage';
      this.service.post(url, param).pipe().subscribe((result: any) => {
      });
    }
  }
  prodcutaddtotalcal() {
    debugger
    const product_gid = this.productform.value.product_name;
    let customer;
    if (this.decryt_customer_gid != null && this.decryt_appointment_gid != null) {

      customer = this.customer_gid;
    } else {

      customer = this.combinedFormData.value.customer_name.customer_gid;
    }

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
              tax_prefix: taxSegment[i].tax_prefix,
              taxsegment_gid: taxSegment[i].taxsegment_gid,
            });
          }
        }
      }

      const subtotal = this.productform.value.unitprice * this.productform.value.productquantity;
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
          tax_prefix: detail.tax_prefix,
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
          this.taxgid1 = null;
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
        this.taxgid1 = null;
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
          this.taxgid2 = null;
        }

        if (this.individualTaxAmounts[1].tax_amount != null && this.individualTaxAmounts[1].tax_amount != undefined) {
          this.taxamount2 = this.individualTaxAmounts[1].tax_amount;
          this.taxamount1 = this.individualTaxAmounts[0].tax_amount;
        } else {
          this.taxamount2 = 0;
        }

        if (this.individualTaxAmounts[1].tax_percentage != null && this.individualTaxAmounts[1].tax_percentage != undefined) {
          this.taxprecentage2 = this.individualTaxAmounts[1].tax_percentage;
        } else {
          this.taxprecentage2 = 0;
        }
      } else {
        this.taxgid2 = null;
        this.taxamount2 = 0;
        this.taxprecentage2 = 0;
      }

      if (this.individualTaxAmounts.length > 2) {
        if (this.individualTaxAmounts[2].tax_gid != null && this.individualTaxAmounts[2].tax_gid != undefined) {
          this.taxgid3 = this.individualTaxAmounts[2].tax_gid;
          this.taxname3 = this.individualTaxAmounts[2].tax_name;
          this.tax_prefix3 = this.individualTaxAmounts[2].tax_prefix;
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
        this.taxgid3 = null;
        this.taxamount3 = 0;
        this.taxprecentage3 = 0;
      }

    }
    else {
      const subtotal = this.productform.value.unitprice * this.productform.value.productquantity;
      this.productdiscount_amountvalue = (subtotal * this.productform.value.productdiscount) / 100;
      this.productdiscounted_precentagevalue = subtotal - this.productdiscount_amountvalue;

      const total = subtotal - this.productdiscount_amountvalue + this.producttotal_tax_amount;

      this.producttotal_amount = total.toFixed(2);

      if (this.individualTaxAmounts.length > 0) {
        if (this.individualTaxAmounts[0].tax_gid != null && this.individualTaxAmounts[0].tax_gid != undefined) {
          this.taxgid1 = this.individualTaxAmounts[0].tax_gid;
          this.taxname1 = this.individualTaxAmounts[0].tax_name;
          this.taxsegment_gid = this.individualTaxAmounts[0].taxsegment_gid;
        } else {
          this.taxgid1 = null;
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
        this.taxgid1 = null;
        this.taxamount1 = 0;
        this.taxprecentage1 = 0;
      }

      if (this.individualTaxAmounts.length > 1) {
        if (this.individualTaxAmounts[1].tax_gid != null && this.individualTaxAmounts[1].tax_gid != undefined) {
          this.taxgid2 = this.individualTaxAmounts[1].tax_gid;
          this.taxname2 = this.individualTaxAmounts[1].tax_name;
        } else {
          this.taxgid2 = null;
        }

        if (this.individualTaxAmounts[1].tax_amount != null && this.individualTaxAmounts[1].tax_amount != undefined) {
          this.taxamount2 = this.individualTaxAmounts[1].tax_amount;
          this.taxamount1 = this.individualTaxAmounts[0].tax_amount;
        } else {
          this.taxamount2 = 0;
        }

        if (this.individualTaxAmounts[1].tax_percentage != null && this.individualTaxAmounts[1].tax_percentage != undefined) {
          this.taxprecentage2 = this.individualTaxAmounts[1].tax_percentage;
        } else {
          this.taxprecentage2 = 0;
        }
      } else {
        this.taxgid2 = null;
        this.taxamount2 = 0;
        this.taxprecentage2 = 0;
      }

      if (this.individualTaxAmounts.length > 2) {
        if (this.individualTaxAmounts[2].tax_gid != null && this.individualTaxAmounts[2].tax_gid != undefined) {
          this.taxgid3 = this.individualTaxAmounts[2].tax_gid;
          this.taxname3 = this.individualTaxAmounts[2].tax_name;
        } else {
          this.taxgid3 = null;
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
        this.taxgid3 = null;
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

      this.grandtotal = ((totalAmount) + (addoncharges) + (total_amount) + (frieghtcharges) + (roundoff) - (discountamount) + (packing_charges));
      this.grandtotal = +((this.grandtotal).toFixed(2));

    }
  }

  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }

  ondelete() {
    
    var url = 'SmrRptInvoiceReport/GetDeleteInvoiceProductSummary'
    this.NgxSpinnerService.show();
    let param = {
      invoice_gid: this.parameterValue
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == true) {
        this.DIProductSummmary();
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
        this.productSearch();
      }
      else {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message)

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
    else if(this.productquantity == null || this.productquantity == undefined || this.productquantity == 0 ) {
      debugger
      window.scrollTo({
        top: 0,
      });
      this.ToastrService.warning('Kindly give the quantity');
      return

    }
    else{

    var postapi = 'SmrRptInvoiceReport/PostproductDirectinvoice';
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
      customer_gid: this.combinedFormData.value.customer_name.customer_gid,
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
      discount_amount: this.productdiscount_amountvalue,
      discountprecentage: this.productform.value.productdiscount,
      product_desc: this.productform.value.product_desc,
      productgroup_gid: this.productform.value.producraddproductgroup_name,
      productgroup_name: this.productform.value.producraddproductgroup_name.productgroup_name,
      tax_prefix: this.tax_prefix,
      tax_prefix2: this.tax_prefix2,
      tax_prefix3: this.tax_prefix3,
    }

    this.service.post(postapi, param).subscribe((apiresponse: any) => {
      if (apiresponse.status == false) {
        this.ToastrService.warning(apiresponse.message);
        window.scrollTo({
          top: 0
        })
      }
      else {
        this.ToastrService.success(apiresponse.message);
        this.productform.reset();
        this.DIProductList = [];
        this.DIProductSummmary();
        this.productSearch();
      }
    });
    this.NgxSpinnerService.hide();
    this.productSearch();
  }
}

  productSearch() {
    debugger
    var api = 'SmrTrnSalesorder/GetProductsearchSummarySales';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.DIProductList = this.responsedata.GetProductsearchs;
      this.filteredDIProductList = this.DIProductList;


    });
  }

  searchOnChange(event: KeyboardEvent) {
    if (event.key !== 'Enter') {
      this.productSearch();
    }
  }

  DIProductSummmary() {
    debugger
    this.NgxSpinnerService.show();


    var api = 'SmrRptInvoiceReport/GetProductSummary';

    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.directinvoiceproductsummary_list = this.responsedata.directinvoiceproductsummary_list;

      this.totalamount = this.responsedata.grandtotal.toFixed(2);
      this.producttotalamount = this.responsedata.grandtotal.toFixed(2);
      this.combinedFormData.get("totalamount")?.setValue(this.responsedata.grandtotal.toFixed(2));
      this.combinedFormData.get("grandtotal")?.setValue(this.responsedata.grandtotal.toFixed(2));
      this.currency_code1 = "";
      this.combinedFormData.get("totalamount")?.setValue(result.grand_total);
      this.combinedFormData.get("grandtotal")?.setValue(result.grandtotal);

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

    const product_name = this.DIProductList.find(product => product.product_gid === event.product_gid);
    if (product_name) {
      this.productform.patchValue({
        product_code: product_name.product_code,
        producraddproductgroup_name: product_name.productgroup_gid,
        product_desc: product_name.product_desc,
        unitprice: product_name.unitprice,
        product_remarks: product_name.product_desc
      });
    }
  }


  OnProductCode(event: any) {
    const product_code = this.DIProductList.find(product => product.product_code === event.product_code);
    if (product_code) {
      this.productform.patchValue({
        product_name: product_code.product_gid,
        producraddproductgroup_name: product_code.productgroup_gid,
        unitprice: product_code.unitprice,
        product_desc: product_code.product_desc,
      });
    }
  }
  onKeyPress(event: any) {
    const key = event.key;
    if (!/^[0-9.]$/.test(key)) {
      event.preventDefault();
    }
  }


  calculateDueDate() {
    debugger
    const paymentDays = this.mdldpaymentdays;
    if (paymentDays) {
      const currentDate = new Date();
      currentDate.setDate(currentDate.getDate() + paymentDays);
      const formattedDueDate = currentDate.toISOString().substring(0, 10);
      this.combinedFormData.patchValue({ due_date: formattedDueDate });
    }
  }

  calculateProbationEndDate() {
    debugger
    const invoiceDateValue = this.combinedFormData.get('invoice_date')?.value;
    const paymentDays = parseInt(this.combinedFormData.get('payment_days')?.value, 10);
    const parts = invoiceDateValue.split('-');
    const day = parseInt(parts[0], 10);
    const month = parseInt(parts[1], 10) - 1;
    const year = parseInt(parts[2], 10);

    const invoiceDate = new Date(year, month, day);

    let probationEndDate = new Date(invoiceDate);
    let remainingDays = paymentDays


    while (remainingDays > 0) {
      const currentDate = probationEndDate.getDate();
      const daysToAdd = Math.min(remainingDays);

      probationEndDate.setDate(currentDate + daysToAdd);


      if (currentDate + daysToAdd > 30) {

        const currentMonth = probationEndDate.getMonth();
        if (currentMonth === 11) {
          probationEndDate.setFullYear(probationEndDate.getFullYear() + 1);
          probationEndDate.setMonth(0);
        } else {
          probationEndDate.setMonth(currentMonth + 1);
        }
      }

      remainingDays -= daysToAdd;
    }


    const years = probationEndDate.getFullYear();
    const months = String(probationEndDate.getMonth() + 1).padStart(2, '0'); // Month is zero-indexed, so add 1
    const days = String(probationEndDate.getDate()).padStart(2, '0');

    const formattedDate = `${days}-${months}-${years}`;
    this.combinedFormData.get('due_date')?.setValue(formattedDate);


  }
  clear() {
    var LoadPage = 'SmrRptInvoiceReport/LoadPage';
    this.service.get(LoadPage).subscribe((result: any) => { });
    this.route.navigate(['/smr/SmrTrnInvoiceSummary'])
  }
}
