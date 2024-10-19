import { HttpClient } from '@angular/common/http';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { AES, enc } from 'crypto-js';
import { param } from 'jquery';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { NgxSpinnerService } from 'ngx-spinner';
import { Component, OnInit } from '@angular/core';
import { dE } from '@fullcalendar/core/internal-common';


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
}
@Component({
  selector: 'app-smr-trn-orderfrom360-new',
  templateUrl: './smr-trn-orderfrom360-new.component.html',
  styleUrls: ['./smr-trn-orderfrom360-new.component.scss']
})
export class SmrTrnOrderfrom360NewComponent {
  showUpdateButton: boolean = false;
  showAddButton: boolean = true;
  config: AngularEditorConfig = {
    editable: false,
    spellcheck: true,
    height: '37rem',
    minHeight: '5rem',
    width: '730px',
    placeholder: 'Enter text here...',
    translate: 'no',
    defaultParagraphSeparator: 'p',

  };
  combinedFormData: FormGroup | any;
  productform: FormGroup | any;
  Cmntaxsegment_gid: any;
  customer_list: any;
  allchargeslist: any[] = [];
  customercontact_name: any;
  branch_list: any[] = [];
  contact_list: any[] = [];
  currency_list: any[] = [];
  user_list: any[] = [];
  product_list: any[] = [];
  tax_list: any[] = [];
  tax2_list: any[] = [];
  tax3_list: any[] = [];
  tax4_list: any[] = [];
  calci_list: any[] = [];
  POproductlist: any[] = [];
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
  GetproductsCode: any;
  mdlContactName: any;
  unitprice: number = 0;
  quantity: number = 0;
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
  tax_amount2: number = 0;
  tax_amount3: number = 0;
  producttotalamount: any;
  parameterValue: string | undefined;
  productnamelist: any;
  selectedCurrencyCode: any;
  POadd_list: any;
  total_amount: any;
  mdlTerms: any;
  additional_discount: number = 0;
  total_price: any;
  mdlproductName: any;
  responsedata: any;
  ExchangeRate: any;
  salesOD!: SalesOD;
  Productsummarys_list: any;
  salesorders_list: any;
  cuscontact_gid: any;
  salesorder: any;
  leadbank_gid: any;
  exchange: number = 0;
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
  marks: number = 0;
  GetTaxSegmentList: any[] = [];
  totTaxAmountPerQty: any;
  prod_name: any;
  mdlTaxSegment: any;
  taxseg_tax: any;
  mdlcustomeradrress: any = null;
  SOProductList: any[] = [];
  deencryptedParam: any;
  customer_gid: any;
  constructor(private http: HttpClient, private fb: FormBuilder, private router: ActivatedRoute, private route: Router, private service: SocketService, private ToastrService: ToastrService, public NgxSpinnerService: NgxSpinnerService,) {
    this.salesOD = {} as SalesOD
  }
  back() {

    const customer_gid = this.router.snapshot.paramMap.get('customer_gid');
    const leadbank_gid = this.router.snapshot.paramMap.get('customer_gid');
    const lead2campaign_gid = this.router.snapshot.paramMap.get('lead2campaign_gid');
    const leadbankcontact_gid = this.router.snapshot.paramMap.get('leadbankcontact_gid');
    const lspage = this.router.snapshot.paramMap.get('lspage');
    this.route.navigate(['/smr/SmrTrnSales360', leadbank_gid, lead2campaign_gid, leadbankcontact_gid, lspage])

  }
  ngOnInit(): void {


    const customer_gid = this.router.snapshot.paramMap.get('customer_gid');
    const leadbank_gid = this.router.snapshot.paramMap.get('customer_gid');
    const lead2campaign_gid = this.router.snapshot.paramMap.get('lead2campaign_gid');
    const leadbankcontact_gid = this.router.snapshot.paramMap.get('leadbankcontact_gid');
    const lspage = this.router.snapshot.paramMap.get('lspage');
    this.deencryptedParam = customer_gid;
    const secretKey = 'storyboarderp';
    this.customer_gid = AES.decrypt(this.deencryptedParam, secretKey).toString(enc.Utf8);

    this.SOproductsummary();
    this.OnChangeCustomer();
    // this.SmrTrnSalesorderSummary();
    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);

    // this.SOproductsummary();
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
      exchange_rate: new FormControl(''),
      producttype_name: new FormControl('')
    });

    //// Branch Dropdown /////
    var url = 'SmrTrnSalesorder/GetBranchDtl'
    this.service.get(url).subscribe((result: any) => {
      this.branch_list = result.GetBranchDtl;
      const firstBranch = this.branch_list[0];
      const branchName = firstBranch.branch_gid;
      this.combinedFormData.get('branch_name')?.setValue(branchName);
    });


    var url = 'SmrTrnSalesorder/GetCustomerDtl'
    this.service.get(url).subscribe((result: any) => {
      this.customer_list = result.GetCustomerDtl;
    });

    //// Sales person Dropdown ////
    var url = 'SmrTrnSalesorder/GetPersonDtl'
    this.service.get(url).subscribe((result: any) => {
      this.contact_list = result.GetPersonDtl;
    });

    // product type
    var api = 'SmrTrnSalesorder/Getproducttypesales';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.producttype_list = this.responsedata.Getproducttypesales;
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
    //// Tax 1 Dropdown ////
    var url = 'SmrTrnSalesorder/GetTax1Dtl'
    this.service.get(url).subscribe((result: any) => {
      this.tax_list = result.GetTax1Dtl;
    });

    //// Tax 2 Dropdown ////
    var url = 'SmrTrnSalesorder/GetTax2Dtl'
    this.service.get(url).subscribe((result: any) => {
      this.tax2_list = result.GetTax2Dtl;
    });

    //// Tax 3 Dropdown ////
    var url = 'SmrTrnSalesorder/GetTax3Dtl'
    this.service.get(url).subscribe((result: any) => {
      this.tax3_list = result.GetTax3Dtl;
    });

    //// Tax 3 Dropdown ////
    var url = 'SmrTrnSalesorder/GetTax4Dtl'
    this.service.get(url).subscribe((result: any) => {
      this.tax4_list = result.GetTax4Dtl;
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
  // for new design
  sample() {
    this.sam = !this.sam;
  }

  arrow() {
    this.arrowfst = !this.arrowfst;
  }

  arrowone() {
    this.arrowOne = !this.arrowOne;
  }

  isCollapsed: CollapseState = {
    section1: true,
    section2: true,
    section3: true,
  };

  toggleCollapse(section: string) {

    this.isCollapsed[section] = !this.isCollapsed[section];
  }

  toggleCollapsesection(section: string) {
    this.isCollapsed[section] = false;
  }
  // end
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
  getCurrentDate(): string {
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = today.getFullYear();

    return dd + '-' + mm + '-' + yyyy;
  }


  OnChangeCustomer() {

    let customer_gid = this.customer_gid;
    let param = {
      customer_gid: customer_gid
    }
    var url = 'SmrTrnSalesorder/GetOnChangeCustomer';

    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.GetCustomerDet = this.responsedata.GetCustomer;
      this.Cmntaxsegment_gid = result.GetCustomer[0].taxsegment_gid;
      this.combinedFormData.get("customer_name")?.setValue(result.GetCustomer[0].customer_name);
      this.combinedFormData.get("customer_mobile")?.setValue(result.GetCustomer[0].customer_mobile);
      this.combinedFormData.get("customercontact_names")?.setValue(result.GetCustomer[0].customercontact_names);
      this.combinedFormData.get("customer_address")?.setValue(result.GetCustomer[0].customer_address);
      this.combinedFormData.value.leadbank_gid = result.GetCustomer[0].leadbank_gid;
      this.combinedFormData.get("customer_email")?.setValue(result.GetCustomer[0].customer_email);
      this.cuscontact_gid = this.combinedFormData.value.customercontact_gid;
      this.mdlcustomeradrress = (this.GetCustomerDet[0].customercontact_names + '\n' + this.GetCustomerDet[0].customer_mobile + '\n' + this.GetCustomerDet[0].customer_email + ',\n' + this.GetCustomerDet[0].customer_address)
      //this.combinedFormData.value.leadbank_gid = result.GetCustomer[0].leadbank_gid
      this.productSearch();
      this.SOproductsummary();
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
  OnClearOverallTax() {

    this.tax_amount4 = '';
    this.total_price = '';
    this.grandtotal = ((this.producttotalamount) + (+this.addon_charge) + (+this.freight_charges) - (+this.buyback_charges) + (+this.packing_charges) + (+this.insurance_charges) + (+this.roundoff) - (+this.additional_discount));
    this.grandtotal = ((this.grandtotal).toFixed(2));
  }
  onCurrencyCodeChange(event: Event) {


    const target = event.target as HTMLSelectElement;
    const selectedCurrencyCode = target.value;

    this.selectedCurrencyCode = selectedCurrencyCode;
    this.combinedFormData.controls.currency_code.setValue(selectedCurrencyCode);
    this.combinedFormData.get("currency_code")?.setValue(this.currency_list[0].currency_code);

  }

  GetOnChangeProductsName() {

    let product_gid = this.productform.value.product_name.product_gid;
    //let customercontact_gid = this.combinedFormData.value.customer_name.customer_gid;
    if (this.combinedFormData.value.customer_name != undefined) {
      let customercontact_gid = this.combinedFormData.value.customer_name.customer_gid;
      let param = {
        product_gid: product_gid,
        customercontact_gid: customercontact_gid
      }

      var url = 'SmrTrnSalesorder/GetOnChangeProductsName';
      this.NgxSpinnerService.show();
      this.service.getparams(url, param).subscribe((result: any) => {
        this.productform.get("product_code")?.setValue(result.ProductsCode[0].product_code);
        this.productform.get("productuom_name")?.setValue(result.ProductsCode[0].productuom_name);
        this.productform.get("productgroup_name")?.setValue(result.ProductsCode[0].productgroup_name);
        this.productform.get("unitprice")?.setValue(result.ProductsCode[0].unitprice);
        this.productform.value.productgroup_gid = result.ProductsCode[0].productgroup_gid
        // this.productform.value.productuom_gid = result.GetProductsCode[0].productuom_gid
        this.NgxSpinnerService.hide();
      });
    }
    else if (this.productform.value.product_name.product_gid) {
      let param = {
        product_gid: product_gid,

      }
      var url = 'SmrTrnSalesorder/GetOnChangeProductsNames';
      this.NgxSpinnerService.show()
      this.service.getparams(url, param).subscribe((result: any) => {
        this.productform.get("product_code")?.setValue(result.ProductsCode[0].product_code);
        this.productform.get("productuom_name")?.setValue(result.ProductsCode[0].productuom_name);
        this.productform.get("productgroup_name")?.setValue(result.ProductsCode[0].productgroup_name);
        this.productform.get("unitprice")?.setValue(result.ProductsCode[0].unitprice);
        this.productform.value.productgroup_gid = result.ProductsCode[0].productgroup_gid
        this.NgxSpinnerService.hide()
      });

    }
    else {
      this.ToastrService.warning('Kindly Select Customer Before Adding Product !! ')
    }
  }
  productAdd() {


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
      // tax_name2: this.productform.value.tax_name2,
      // tax_amount2: this.productform.value.tax_amount2,
      // tax_name3: this.productform.value.tax_name3,
      // tax_amount3: this.productform.value.tax_amount3,
      totalamount: this.productform.value.totalamount,
      producttotalamount: this.productform.value.producttotalamount,
      exchange_rate: this.exchange,
      currency_code: this.mdlCurrencyName
    }

    var api = 'SmrTrnSalesorder/PostOnAdds';
    this.NgxSpinnerService.show();

    this.service.post(api, params).subscribe((result: any) => {
      if (result.status == false) {
        this.NgxSpinnerService.hide();

        this.ToastrService.warning(result.message)
        this.SOproductsummary();
      }
      else {
        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message)
        this.SOproductsummary();
        this.productform.reset();
      }
    },
    );
  }
  // SOproductsummary() {
  //   debugger
  //   var api = 'SmrTrnSalesorder/GetSalesOrdersummary';
  //   this.service.get(api).subscribe((result: any) => {
  //     this.responsedata = result;
  //     this.salesorders_list = this.responsedata.salesorders_list;
  //     console.log('dsds', this.salesorders_list)
  //     this.combinedFormData.get("totalamount")?.setValue(result.grand_total);
  //     if (this.tax_name4 == null || this.tax_name4 == "" || this.tax_name4 == "--No Tax--") {
  //       this.combinedFormData.get("grandtotal")?.setValue(result.grand_total);
  //     }
  //     else {
  //       let selectedTax = this.tax4_list.find(tax => tax.tax_name4 === this.tax_name4);
  //       this.taxpercentage = this.getDimensionsByFilter(selectedTax.tax_gid);
  //       let tax_percentage = this.taxpercentage[0].percentage;
  //       this.tax_amount4 = Math.round(+(tax_percentage * result.grand_total / 100));
  //       const taxamount = Math.round(+(result.grand_total + this.tax_amount4));
  //       const newGrandTotal = result.grand_total + parseFloat(this.tax_amount4);
  //       const newGrandTotal2 = newGrandTotal + parseFloat(this.combinedFormData.value.addon_charge);
  //       const newGrandTotal3 = newGrandTotal2 - parseFloat(this.combinedFormData.value.additional_discount);
  //       const newGrandTotal4 = newGrandTotal3 + parseFloat(this.combinedFormData.value.freight_charges);
  //       const newGrandTotal5 = newGrandTotal4 - parseFloat(this.combinedFormData.value.buyback_charges);
  //       const newGrandTotal6 = newGrandTotal5 + parseFloat(this.combinedFormData.value.packing_charges);
  //       const newGrandTotal7 = newGrandTotal6 + parseFloat(this.combinedFormData.value.insurance_charges);
  //       const newGrandTotal8 = newGrandTotal7 + parseFloat(this.combinedFormData.value.roundoff);
  //       this.combinedFormData.get("tax_amount4")?.setValue(this.tax_amount4);
  //       this.combinedFormData.get("total_price")?.setValue(taxamount);
  //       this.combinedFormData.get("grandtotal")?.setValue(newGrandTotal8);
  //     }

  //   });
  // }

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

    var params = {
      customer_gid: this.customer_gid,
      branch_name: this.combinedFormData.value.branch_name,
      branch_gid: this.combinedFormData.value.branch_name.branch_gid,
      salesorder_date: this.combinedFormData.value.salesorder_date,
      salesorder_gid: this.combinedFormData.value.salesorder_gid,
      customer_name: this.combinedFormData.value.customer_name.customer_name,
      customercontact_names: this.combinedFormData.value.customercontact_names,
      customer_email: this.combinedFormData.value.customer_email,
      customer_mobile: this.combinedFormData.value.customer_mobile,
      so_remarks: this.combinedFormData.value.so_remarks,
      so_referencenumber: this.combinedFormData.value.so_referencenumber,
      customer_address: this.combinedFormData.value.customer_address,
      freight_terms: this.combinedFormData.value.freight_terms,
      payment_terms: this.combinedFormData.value.payment_terms,
      currency_code: this.combinedFormData.value.currency_code,
      user_name: this.combinedFormData.value.user_name,
      exchange_rate: this.combinedFormData.value.exchange_rate,
      payment_days: this.combinedFormData.value.payment_days,
      termsandconditions: this.combinedFormData.value.termsandconditions,
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
      shipping_to: this.combinedFormData.value.shipping_to
    }
    //console.log('ref', params)
    var url = 'SmrTrnSalesorder/PostSalesOrder'
    this.NgxSpinnerService.show();
    this.service.postparams(url, params).subscribe((result: any) => {
      if (result.status == false) {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message)
      }
      else {
        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message)
        this.route.navigate(['/smr/SmrTrnSalesorderSummary']);
      }
    });
  }
  getDimensionsByFilter(id: any) {
    return this.tax_list.filter((x: { tax_gid: any; }) => x.tax_gid === id);
  }


  prodtotalcal(i: any) {

    const subtotal = this.exchange * this.SOProductList[i].unitprice * this.SOProductList[i].quantity;
    this.marks = this.SOProductList[i].quantity;
    var discount_amount = (subtotal * this.SOProductList[i].discount_percentage) / 100;
    this.SOProductList[i].discount_amount = (discount_amount).toFixed(2);
    //console.log('rejiofj', this.SOProductList[i].discount_amount);

    // Calculate total tax amount for the product
    let totalTaxAmount = 0;
    for (let taxSegment of this.SOProductList[i].taxSegments) {
      totalTaxAmount += (parseFloat(this.SOProductList[i].unitprice) * parseFloat(taxSegment.tax_percentage) / 100);
    }
    const totalTaxAmountPerQty = totalTaxAmount * this.SOProductList[i].quantity;
    this.SOProductList[i].totalTaxAmount = totalTaxAmountPerQty.toFixed(2);
    if (totalTaxAmountPerQty > 0) {
      total_amount = (totalTaxAmountPerQty + subtotal) - discount_amount;
    } else
      var total_amount = subtotal - this.SOProductList[i].discount_amount;
    this.SOProductList[i].total_amount = total_amount.toFixed(2);
    const value = this.total_amount.value;
    const formattedValue = parseFloat(value).toFixed(2);
    this.total_amount.setValue(formattedValue, { emitEvent: false });

  }


  OnChangeTaxAmount1() {

    if (this.CurrencyName == 'INR') {
      let tax_name = this.productform.get('tax_name')?.value;
      this.taxpercentage = this.getDimensionsByFilter(tax_name);
      let tax_percentage = this.taxpercentage[0].percentage;

      // Calculate the tax amount with two decimal points
      this.tax_amount = ((+(tax_percentage * this.totalamount / 100))).toFixed(2);
      const subtotal = this.exchange * this.unitprice * this.quantity;
      this.discountamount = (subtotal * this.discountpercentage) / 100;
      this.discountamount = +((this.discountamount).toFixed(2));
      this.totalamount = (+(subtotal - this.discountamount + this.tax_amount)).toFixed();
    }
    else {
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
        this.totalamount = (this.totalamount * 10) / 10
      }

    }
  }

  OnChangeTaxAmount2() {
    let tax_gid2 = this.productform.get('tax_name2')?.value;
    this.taxpercentage = this.getDimensionsByFilter(tax_gid2);
    let tax_percentage = this.taxpercentage[0].percentage;

    const subtotal = this.exchange_rate * this.unitprice * this.quantity;
    this.discountamount = (subtotal * this.discountpercentage) / 100;
    this.totalamount = subtotal - this.discountamount;
    this.tax_amount2 = ((tax_percentage) * (this.totalamount) / 100);

    if (this.tax_amount == undefined && this.tax_amount2 == undefined) {
      const subtotal = this.exchange_rate * this.unitprice * this.quantity;
      this.discountamount = (subtotal * this.discountpercentage) / 100;
      this.totalamount = subtotal - this.discountamount;
    }
    else {
      this.totalamount = ((this.totalamount) + (+this.tax_amount) + (+this.tax_amount2));
    }
  }
  OnChangeTaxAmount3() {
    let tax_gid3 = this.productform.get('tax_name3')?.value;
    this.taxpercentage = this.getDimensionsByFilter(tax_gid3);
    let tax_percentage = this.taxpercentage[0].percentage;

    const subtotal = this.exchange_rate * this.unitprice * this.quantity;
    this.discountamount = (subtotal * this.discountpercentage) / 100;
    this.totalamount = subtotal - this.discountamount;
    this.tax_amount3 = ((tax_percentage) * (this.totalamount) / 100);

    if (this.tax_amount == undefined && this.tax_amount2 == undefined && this.tax_amount3 == undefined) {
      const subtotal = this.exchange_rate * this.unitprice * this.quantity;
      this.discountamount = (subtotal * this.discountpercentage) / 100;
      this.totalamount = subtotal - this.discountamount;
    }
    else {
      this.totalamount = ((this.totalamount) + (+this.tax_amount) + (+this.tax_amount2) + (+this.tax_amount3));
    }
  }

  OnChangeTaxAmount4() {
    const tax_name = this.combinedFormData.get('tax_name4')?.value;
    const selectedTax = this.tax4_list.find(tax => tax.tax_name4 === tax_name);
    const tax_gid = selectedTax.tax_gid;
    this.taxpercentage = this.getDimensionsByFilter(tax_gid);
    const tax_percentage = this.taxpercentage[0].percentage;

    this.tax_amount4 = (+(tax_percentage * this.producttotalamount / 100));
    this.total_price = +((+this.producttotalamount) + (+this.tax_amount4));
    this.total_price = (+this.total_price);
    this.total_price = parseFloat(this.total_price.toFixed(2));
    this.grandtotal = ((this.total_price) + (+this.addon_charge) + (+this.freight_charges) - (+this.buyback_charges) + (+this.packing_charges) + (+this.insurance_charges) + (+this.roundoff) - (+this.additional_discount));
    this.grandtotal = parseFloat(this.grandtotal.toFixed(2));
  }

  finaltotal() {

    if (this.total_price == null || this.total_price == "" || this.total_price == 0 || this.total_price == "NaN") {
      this.grandtotal = ((this.producttotalamount) + (+this.addon_charge) + (+this.freight_charges) - (+this.buyback_charges) + (+this.packing_charges) + (+this.insurance_charges) + (+this.roundoff) - (+this.additional_discount));
      this.grandtotal = +((this.grandtotal).toFixed(2));
    }
    else {
      const total_amount = parseFloat(this.total_price)
      const totalAmount = isNaN((total_amount)) ? 0 : (total_amount);
      const addoncharges = isNaN(this.combinedFormData.value.addon_charge) ? 0 : +this.addon_charge;
      const frieghtcharges = isNaN(this.combinedFormData.value.freight_charges) ? 0 : +this.freight_charges;
      const forwardingCharges = isNaN(this.combinedFormData.value.buyback_charges) ? 0 : +this.buyback_charges;
      const insurancecharges = isNaN(this.combinedFormData.value.insurance_charges) ? 0 : +this.insurance_charges;
      const packing_charges = isNaN(this.combinedFormData.value.packing_charges) ? 0 : +this.packing_charges;
      const roundoff = isNaN(this.combinedFormData.value.roundoff) ? 0 : +this.roundoff;
      const discountamount = isNaN(this.combinedFormData.value.additional_discount) ? 0 : +this.additional_discount;

      this.grandtotal = ((totalAmount) + (addoncharges) + (frieghtcharges) - (forwardingCharges) + (insurancecharges) + (roundoff) - (discountamount) + (packing_charges));
      this.grandtotal = +((this.grandtotal).toFixed(2));

    }
    // this.total_price = this.producttotalamount + this.tax_amount4;
    // this.grandtotal =  ((this.total_price) + (+this.addon_charge) + (+this.freight_charges) + (+this.buyback_charges) + (+this.insurance_charges) + (+this.packing_charges) + (+this.roundoff)  - (+this.additional_discount)).toFixed(2);
  }



  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }

  ondelete() {
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

        this.ToastrService.success(result.message)
        this.SOproductsummary();
        this.NgxSpinnerService.hide();
      }
    });
  }

  // PRODUCT EDIT SUMMARY
  // editproductSO(tmpsalesorderdtl_gid: any) {

  //   var url = 'SmrTrnSalesorder/GetDirectSalesOrderEditProductSummary'
  //   this.NgxSpinnerService.show();
  //   let param = {
  //     tmpsalesorderdtl_gid: tmpsalesorderdtl_gid
  //   }
  //   this.service.getparams(url, param).subscribe((result: any) => {
  //     this.directeditsalesorder_list = result.directeditsalesorder_list;
  //     this.productform.get("tmpsalesorderdtl_gid")?.setValue(this.directeditsalesorder_list[0].tmpsalesorderdtl_gid);
  //     this.productform.get("product_name")?.setValue(this.directeditsalesorder_list[0].product_name);
  //     this.productform.get("product_gid")?.setValue(this.directeditsalesorder_list[0].product_gid);
  //     this.productform.get("product_code")?.setValue(this.directeditsalesorder_list[0].product_code);
  //     this.productform.get("productuom_name")?.setValue(this.directeditsalesorder_list[0].productuom_name);
  //     this.productform.get("quantity")?.setValue(this.directeditsalesorder_list[0].quantity);
  //     this.productform.get("totalamount")?.setValue(this.directeditsalesorder_list[0].totalamount);
  //     this.productform.get("tax_name")?.setValue(this.directeditsalesorder_list[0].tax_name);
  //     this.productform.get("tax_gid")?.setValue(this.directeditsalesorder_list[0].tax_gid);
  //     this.productform.get("unitprice")?.setValue(this.directeditsalesorder_list[0].unitprice);
  //     this.productform.get("tax_amount")?.setValue(this.directeditsalesorder_list[0].tax_amount);
  //     this.productform.get("discount_percentage")?.setValue(this.directeditsalesorder_list[0].discount_percentage);
  //     this.productform.get("discountamount")?.setValue(this.directeditsalesorder_list[0].discountamount);
  //     this.NgxSpinnerService.hide();
  //   });
  //   this.showUpdateButton = true;
  //   this.showAddButton = false;
  // }

  // onupdate() {
  //   var params = {
  //     tmpsalesorderdtl_gid: this.productform.value.tmpsalesorderdtl_gid,
  //     product_code: this.productform.value.product_code,
  //     product_name: this.productform.value.product_name.product_name == undefined ? this.productform.value.product_name : this.productform.value.product_name.product_name,
  //     quantity: this.productform.value.quantity,
  //     unitprice: this.productform.value.unitprice,
  //     discountamount: this.productform.value.discountamount,
  //     discount_percentage: this.productform.value.discount_percentage,
  //     product_gid: this.productform.value.product_name.product_gid,
  //     productuom_gid: this.productform.value.productuom_gid,
  //     tax_name: this.productform.value.tax_name,
  //     tax_gid: this.productform.value.tax_name.tax_gid,
  //     tax_amount: this.productform.value.tax_amount,
  //     totalamount: this.productform.value.totalamount
  //   }
  //   var url = 'SmrTrnSalesorder/PostUpdateDirectSOProduct'
  //   this.NgxSpinnerService.show();
  //   this.service.postparams(url, params).pipe().subscribe((result: any) => {
  //     this.responsedata = result;
  //     if (result.status == false) {
  //       this.NgxSpinnerService.hide();
  //       this.ToastrService.warning(result.message)
  //       this.SOproductsummary();
  //     }
  //     else {
  //       this.NgxSpinnerService.hide();
  //       this.ToastrService.success(result.message)
  //       this.productform.reset();
  //     }
  //     this.SOproductsummary();
  //   });
  //   this.showAddButton = true;
  //   this.showUpdateButton = false
  // }


  close() {
    this.route.navigate(['/smr/SmrTrnSalesorderSummary']);
  }
  openModelDetail(product_gid: any) {

    var url = 'SmrTrnSalesorder/GetSalesorderdetail'
    let params = {
      product_gid: product_gid
    }
    this.service.getparams(url, params).subscribe((result: any) => {
      this.responsedata = result;
      this.Salesorderdetail_list = this.responsedata.Directeddetailslist2;
    });

  }

  productSubmit() {

    if (this.combinedFormData.value.customer_name == "" || this.combinedFormData.value.customer_name == null || this.combinedFormData.value.customer_name == undefined) {
      window.scrollTo({
        top: 0,
      });
      this.ToastrService.warning('Kindly Select Customer!');
      return
    }
    this.toggleCollapsesection('section3');

    const api = 'SmrTrnSalesorder/PostOnAdds';
    this.NgxSpinnerService.show();

    // Set a timer to hide the spinner after 3 seconds
    const spinnerTimer = setTimeout(() => {
      this.NgxSpinnerService.hide();
    }, 3000);
    const params = {
      SOProductList: this.SOProductList,
      taxsegment_gid: this.Cmntaxsegment_gid,
      exchange_rate: this.CurrencyExchangeRate,

    };
    //console.log('sad', params)
    this.service.post(api, params).subscribe((result: any) => {
      clearTimeout(spinnerTimer); // Clear the spinner timer
      if (result.status == false) {
        this.ToastrService.warning(result.message);
      } else {
        this.ToastrService.success(result.message);
        this.SOProductList = [];

        this.SOproductsummary();
      }
      this.NgxSpinnerService.hide();
    });


    const toggleBtn = document.getElementById('toggleBtn');
    const collapseContent = document.getElementById('collapseContent');
    toggleBtn?.addEventListener('click', () => {
      collapseContent?.classList.toggle('show');
    });
  }
  productSearch() {

    //let customer_gid = this.combinedFormData.get("customer_name")?.value;
    var params = {
      producttype_gid: this.productform.value.producttype_name,
      product_name: this.productform.value.product_name,
      customer_gid: this.customer_gid
    };
    var api = 'SmrTrnSalesorder/GetProductsearchSummarySales';
    this.service.getparams(api, params).subscribe((result: any) => {
      this.responsedata = result;
      this.SOProductList = this.responsedata.GetProductsearchs;

      // Iterate over each product in salesorders_list
      for (let product of this.SOProductList) {
        // Filter tax segments based on the current product's product_gid
        const taxSegmentsForProduct = this.responsedata.GetTaxSegmentListorder.filter((taxSegment: { product_gid: any; }) => taxSegment.product_gid === product.product_gid);

        // Map tax segments to include tax_name, tax_percentage, tax_gid, and taxsegment_gid
        const mappedTaxSegments = taxSegmentsForProduct.map((taxSegment: { tax_name: any; tax_percentage: any; tax_gid: any; taxsegment_gid: any; }) => ({
          tax_name: taxSegment.tax_name,
          tax_percentage: taxSegment.tax_percentage,
          tax_gid: taxSegment.tax_gid,
          taxsegment_gid: taxSegment.taxsegment_gid
        }));

        // Get unique tax_gids and taxsegment_gids for the product
        const uniqueTaxGids = [...new Set(mappedTaxSegments.map((segment: { tax_gid: any; }) => segment.tax_gid))];
        const uniqueTaxSegmentGids = [...new Set(mappedTaxSegments.map((segment: { taxsegment_gid: any; }) => segment.taxsegment_gid))];

        // Assign the mapped tax segments and unique tax_gids and taxsegment_gids to the current product
        product.taxSegments = mappedTaxSegments;
        product.tax_gids = uniqueTaxGids;
        product.taxsegment_gids = uniqueTaxSegmentGids;
        product.tax_gids_string = uniqueTaxGids.join(', ');

        // Initialize total tax amount for the product
        let totalTaxAmountPerQty = 0;

        // Iterate over tax segments associated with the current product
        for (let taxSegment of product.taxSegments) {
          // Calculate tax amount for the current tax segment
          const taxAmount = parseFloat(product.unitprice) * parseFloat(taxSegment.tax_percentage) / 100;
          // Assign tax amount to the tax segment object for display (optional)
          taxSegment.taxAmount = taxAmount.toFixed(2);
          // Add tax amount to the total tax amount for the product
          totalTaxAmountPerQty += taxAmount;
        }
        this.totTaxAmountPerQty = totalTaxAmountPerQty.toFixed(2);
      }
    });
  }

  searchOnChange(event: KeyboardEvent) {
    if (event.key !== 'Enter') {
      this.productSearch();
    }
  }

  SOproductsummary() {

    let customer_gid = this.customer_gid;
    var params = { customer_gid: customer_gid, product_gid: "" };
    var api = 'SmrTrnSalesorder/ProductSalesSummary';

    this.service.getparams(api, params).subscribe((result: any) => {
      this.responsedata = result;
      this.salesorders_list = this.responsedata.salesorders_list;
      //console.log('dsds', this.salesorders_list)
      this.totalamount = this.responsedata.grandtotal.toFixed(2);
      this.producttotalamount = this.responsedata.grandtotal.toFixed(2);
      this.combinedFormData.get("totalamount")?.setValue(this.responsedata.grandtotal.toFixed(2));
      this.combinedFormData.get("grandtotal")?.setValue(this.responsedata.grandtotal.toFixed(2));
      this.currency_code1 = "";
      this.combinedFormData.get("totalamount")?.setValue(result.grand_total);
      if (this.tax_name4 == null || this.tax_name4 == "" || this.tax_name4 == "--No Tax--") {
        this.combinedFormData.get("grandtotal")?.setValue(result.grandtotal);
      }
      else {
        let selectedTax = this.tax4_list.find(tax => tax.tax_name4 === this.tax_name4);
        this.taxpercentage = this.getDimensionsByFilter(selectedTax.tax_gid);
        let tax_percentage = this.taxpercentage[0].percentage;
        this.tax_amount4 = Math.round(+(tax_percentage * result.grand_total / 100));
        const taxamount = Math.round(+(result.grand_total + this.tax_amount4));
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

      this.salesorders_list.forEach((product: any) => {
        this.fetchProductSummaryAndTax(product.product_gid);
      });
    });
  }


  fetchProductSummaryAndTax(product_gid: string) {
    if (this.combinedFormData.value.customer_name !== undefined) {
      let customer_gid = this.customer_gid;
      let param = {
        product_gid: product_gid,
        customer_gid: customer_gid
      };

      var api = 'SmrTrnSalesorder/ProductSalesSummary';
      this.service.getparams(api, param).subscribe((result: any) => {
        this.responsedata = result;
        this.GetTaxSegmentList = this.responsedata.GetTaxSegmentListorder;

        // Handle tax segments for the current product
        this.handleTaxSegments(product_gid, this.GetTaxSegmentList);
      }, (error) => { });
    }
  }

  handleTaxSegments(product_gid: string, GetTaxSegmentList: any[]) {
    // Find tax segments for the current product_gid
    const productTaxSegments = GetTaxSegmentList.filter((taxSegment: { product_gid: string; }) => taxSegment.product_gid === product_gid);

    if (productTaxSegments.length > 0) {
      // Assign tax segments to the current product
      this.salesorders_list.forEach((product: { product_gid: string; taxSegments: any[]; }) => {
        if (product.product_gid === product_gid) {
          product.taxSegments = productTaxSegments;
        }
      });
    } else { }
  }

  checkDuplicateTaxSegment(GetTaxSegmentList: any[], currentIndex: number): boolean {
    // Extract the taxsegment_gid of the current tax segment
    const currentTaxSegmentId = GetTaxSegmentList[currentIndex].taxsegment_gid;

    // Check if the current tax segment exists before the current index in the array
    for (let i = 0; i < currentIndex; i++) {
      if (GetTaxSegmentList[i].taxsegment_gid === currentTaxSegmentId) {
        // Duplicate found
        return true;
      }
    }

    // No duplicates found
    return false;
  }
  // Inside your component class
  removeDuplicateTaxSegments(GetTaxSegmentList: any[]): any[] {
    const uniqueTaxSegmentsMap = new Map<string, any>();
    GetTaxSegmentList.forEach(taxSegment => {
      uniqueTaxSegmentsMap.set(taxSegment.tax_name, taxSegment);
    });
    // Convert the Map back to an array
    return Array.from(uniqueTaxSegmentsMap.values());
  }


}
