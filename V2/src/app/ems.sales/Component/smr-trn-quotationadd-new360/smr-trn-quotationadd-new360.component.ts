import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { NgxSpinnerService } from 'ngx-spinner';

import { AES, enc } from 'crypto-js';
interface CollapseState {
  [key: string]: boolean;
}

@Component({
  selector: 'app-smr-trn-quotationadd-new360',
  templateUrl: './smr-trn-quotationadd-new360.component.html',
  styleUrls: ['./smr-trn-quotationadd-new360.component.scss']
})
export class SmrTrnQuotationaddNew360Component {

  isExpanded: { [key: string]: boolean } = {
    product: false,
    another: false,
    summary: false
  };

  // new
  QuoteAddForm: any;
  txtContactNo: any;
  txtpricesegment_name: any;
  txttaxsegment_name: any;
  txtgst_number: any;
  txtEmail: any;
  txtaddress1: any;
  txtaddress2: any;
  txtcity: any;
  txtregion_name: any;
  txtcountry_name: any;
  txtzip_code: any;
  salesorders_list: any;
  taxname1: any;
  tax_prefix: any;
  tax_prefix2: any;
  tax_prefix3: any;
  customer_gid: any;
  producttotal_tax_amount: any = 0;
  productdiscount_amountvalue: number = 0;
  individualTaxAmounts: any = 0;
  taxgid1: any;
  taxgid2: any;
  taxgid3: any;
  taxprecentage1: any = 0;
  taxprecentage2: any = 0;
  taxprecentage3: any = 0;
  taxname2: any;
  taxname3: any;
  productdiscounted_precentagevalue: number = 0;
  productcodesearch1: any;
  productsearch: any;
  productcodesearch: any;
  productnamesearch: any;
  filteredSOProductList: any[] = [];
  // end

  vendorError: any;
  sam: boolean = false;
  formBuilder: any;
  marks: number = 0;
  arrowfst: boolean = false;
  arrowOne: boolean = false;
  GetTaxSegmentList: any;
  mdlTaxSegment: any;
  taxSegmentFlag: any;
  showNoTaxSegments: boolean | undefined;
  taxsegment_gid: any;
  taxGids: string[] = [];
  taxseg_taxgid: string | undefined;
  taxseg_tax_amount = 0;
  tax_amount: any;
  taxseg_tax: any;
  totTaxAmountPerQty: any;
  Cmntaxsegment_gid: any;
  GetproductsCode: any;
  prod_name: any;
  selectedProductGID: any;
  mdlTerms: any;
  mdlBranchName: any;
  GetCustomerDet: any;
  mdlCustomerName: any;
  mdlUserName: any;
  mdlCurrencyName: any;
  mdlProductName: any;
  mdlProductUom: any;
  mdlProductCode: any;
  mdlTaxName3: any;
  mdlTaxName2: any;
  mdlTaxName1: any;
  customer_list: any;
  txtContactPerson: any;
  taxsegment_name: any;
  txtCustomerAddress: any;
  cuscontact_gid: any;
  sales_list: any;
  CurrencyName: any;
  CurrencyExchangeRate: any;
  buybackcharges: any;


  tax2_list: any;
  tax3_list: any;
  txtAddress: any;
  Getproductgroup: any[] = [];
  appointment_gid: any;
  toggleExpand(section: string) {
    this.isExpanded[section] = !this.isExpanded[section];
  }
  currency_code1: any
  showInput: boolean = false;
  inputValue: string = ''
  config: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '20rem',
    minHeight: '0rem',
    placeholder: 'Enter text here...',
    translate: 'no',
  };
  tax1: any;
  tax2: any;
  selectedValue: string = '';

  product_list: any;
  branch_list: any;
  producttype_list: any;
  vendor_list: any;
  dispatch_list: any;
  productorder_list: any;
  mdlDispatchadd: any;
  currency_list: any;
  currency_list1: any;
  productsummary_list: any;
  netamount: any;
  overall_tax: any;
  tax_list: any;
  tax4_list: any;
  delivery_days: number = 0;
  // payment_day : number = 0;
  productcode_list: any;
  productgroup_list: any;
  terms_list: any[] = [];
  productform: FormGroup | any;
  responsedata: any;
  productunit_list: any;
  mdlProductGroup: any;
  mdlProductUnit: any;
  mdlproducttype: any;
  mdlVendorName: any;
  mdlDispatchName: any;
  prototal: number = 0;
  quantity: number = 0;
  discountpercentage: number = 0;
  discountamount: any;
  totalamount: number = 0;
  addoncharge: number = 0;
  POdiscountamount: number = 0;
  frieghtcharges: number = 0;
  forwardingCharges: number = 0;
  insurancecharges: number = 0;
  roundoff: number = 0;
  grandtotal: number = 0;
  taxamount1: any;
  taxamount: number = 0;
  taxpercentage: any;
  productdetails_list: any;
  productSaleseorder_list: any;
  taxamount2: number = 0;
  taxamount3: number = 0;
  productamount: number = 0;
  producttotalamount: any;
  parameterValue: string | undefined;
  POProductList1: any[] = [];
  productnamelist: any;
  selectedCurrencyCode: any;
  POadd_list: any;
  total_amount: any;
  insurance_charges: number = 0;
  additional_discount: number = 0;
  freight_charges: number = 0;
  packing_charges: number = 0;
  buybackorscrap: any;
  tax_amount4: number = 0;
  mdlTaxName4: any;
  exchange: any;
  mdlProductcode: any;
  mdlProductunit: any;
  unitprice: number = 0;
  mdlvendoraddress: any;
  mdlemailaddress: any;
  mdlcontactnumber: any;
  mdlcontactperson: any;
  mdlvendorfax: any;
  GetVendord: any;
  invoicediscountamount: number = 0;
  allchargeslist: any[] = [];
  mdlcustomeradrress: any = null;
  productquantity: number = 0;
  productunitprice: number = 0;
  productdiscount: number = 0;
  producttotal_amount: any = 0;
  tax_percentage: any;
  Description: any;
  productunit: any;
  SOProductList: any[] = [];
  deencryptedParam: any;
  show() {
    const toggleBtn = document.getElementById('toggleBtn');
    const collapseContent = document.getElementById('collapseContent');
    toggleBtn?.addEventListener('click', () => {
      // Toggle the 'show' class on the collapse content element
      collapseContent?.classList.toggle('show');
    });
  }

  ngOnInit(): void {
    debugger
    const secretKey = 'storyboarderp';
    const customer_gid = this.route.snapshot.paramMap.get('customer_gid');
    this.deencryptedParam = customer_gid;
    this.customer_gid = AES.decrypt(this.deencryptedParam, secretKey).toString(enc.Utf8);
    const appointment_gid = this.route.snapshot.paramMap.get('appointment_gid');
    this.appointment_gid = appointment_gid;
    const deencryptedParam3 = AES.decrypt(this.appointment_gid, secretKey).toString(enc.Utf8);
    this.appointment_gid = deencryptedParam3;
    this.productSearch();
    this.QuoteAddForm = new FormGroup({
      // new form 
      customer_name: new FormControl(''),
      user_name: new FormControl(''),
      branch_name: new FormControl('', Validators.required),
      branch: new FormControl('', Validators.required),
      quotation_date: new FormControl(this.getCurrentDate()),
      po_date: new FormControl(this.getCurrentDate(), Validators.required),
      email: new FormControl(''),
      gst_number: new FormControl(''),
      currency_code: new FormControl(''),
      address1: new FormControl(''),
      address2: new FormControl(''),
      city: new FormControl(''),
      payment_term: new FormControl(''),
      contact_number: new FormControl(''),
      currency: new FormControl('', Validators.required),
      exchange_rate: new FormControl(''),
      zip_code: new FormControl(''),
      country_name: new FormControl(''),
      region_name: new FormControl(''),
      delivery_days: new FormControl(''),
      payment_days: new FormControl(''),
      remarks: new FormControl(''),
      template_content: new FormControl(''),
      mobile: new FormControl(''),
      quotation_remarks: new FormControl(''),
      //  end 
      tax_amount4: new FormControl(''),
      quantity: new FormControl(''),
      display_name: new FormControl(''),
      productcode: new FormControl(''),
      productgroup: new FormControl(''),
      priority_remarks: new FormControl(''),
      pocovernote_address: new FormControl(''),
      roundoff: new FormControl(''),
      ship_via: new FormControl(''),
      po_no: new FormControl('', [Validators.required]),
      grandtotal: new FormControl('', [Validators.required]),
      additional_discount: new FormControl(''),
      insurance_charges: new FormControl(''),
      freight_charges: new FormControl(''),
      addoncharge: new FormControl(''),
      template_name: new FormControl(''),
      total_amount: new FormControl(''),
      packing_charges: new FormControl(''),
      tax_name4: new FormControl(''),
      quotation_referenceno1: new FormControl(''),
      customercontact_names: new FormControl(''),
      taxsegment_name: new FormControl(''),
      pricesegment_name: new FormControl(''),
      totalamount: new FormControl(''),

    })
    this.productform = new FormGroup({
      tmppurchaseorderdtl_gid: new FormControl(''),
      branch_name: new FormControl('', Validators.required),
      product_gid: new FormControl(''),
      productuom_gid: new FormControl(''),
      productgroup_gid: new FormControl(''),
      product_code: new FormControl(''),
      productcode: new FormControl(''),
      productgroup: new FormControl(''),
      productuom_name: new FormControl(''),
      display_name: new FormControl(''),
      productname: new FormControl(''),
      tax_name1: new FormControl(''),
      tax_name2: new FormControl(''),
      tax_name3: new FormControl(''),
      remarks: new FormControl(''),
      product_name: new FormControl(''),
      productgroup_name: new FormControl(''),
      unitprice: new FormControl(''),
      quantity: new FormControl(''),
      discount_persentage: new FormControl(''),
      discount_amount: new FormControl(''),
      taxname1: new FormControl(''),
      tax_prefix: new FormControl(''),
      tax_prefix2: new FormControl(''),
      taxamount1: new FormControl(''),
      taxname2: new FormControl(''),
      taxamount2: new FormControl(''),
      taxname3: new FormControl(''),
      taxamount3: new FormControl(''),
      totalamount: new FormControl(''),
      tax_name: new FormControl(''),
      total_amount: new FormControl(''),
      packing_charges: new FormControl(''),
      netamount: new FormControl(''),
      overall_tax: new FormControl(''),
      template_content: new FormControl(''),
      producttype_name: new FormControl(''),
      producttype_gid: new FormControl(''),
      producraddproductgroup_name: new FormControl(''),
      productquantity: new FormControl(''),
      productunitprice: new FormControl(''),
      productdiscount: new FormControl(''),
      producttotal_amount: new FormControl(''),
      tax_percentage: new FormControl(''),
      Description: new FormControl(''),
      productunit: new FormControl(''),
      productdiscount_amountvalue: new FormControl(''),
      product_remarks: new FormControl(''),

    })

    this.OnChangeCustomer(this.customer_gid);
    this.show();
    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);
    this.Quoteproductsummary();

    //// Branch Dropdown /////
    var url = 'SmrTrnQuotation/GetBranchDtl'
    this.service.get(url).subscribe((result: any) => {
      this.branch_list = result.GetBranchDt;
      const firstBranch = this.branch_list[0];
      const branchName = firstBranch.branch_gid;
      this.QuoteAddForm.get('branch_name')?.setValue(branchName);
    });
    //// Customer Dropdown /////
    // var url = 'SmrTrnQuotation/GetCustomerDtl'
    // this.service.get(url).subscribe((result: any) => {
    //   this.customer_list = result.GetCustomerDt;
    // });
    //// Sales person Dropdown ////
    var url = 'SmrTrnQuotation/GetSalesDtl'
    this.service.get(url).subscribe((result: any) => {
      this.sales_list = result.GetSalesDtl;
    });
    var productgroupapi = 'SmrTrnSalesorder/GetProductGroup';
    this.service.get(productgroupapi).subscribe((apiresponse: any) => {
      this.Getproductgroup = apiresponse.Getproductgroup;
    });
    //// Product Type Dropdown ////
    var api = 'PmrTrnPurchaseOrder/Getproducttype';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.producttype_list = this.responsedata.Getproducttype;
    });
    var url = 'SmrTrnQuotation/GetCurrencyDtl'
    this.service.get(url).subscribe((result: any) => {
      this.currency_list = result.GetCurrencyDt;
      this.mdlCurrencyName = this.currency_list[0].currencyexchange_gid;
      const defaultCurrency = this.currency_list.find((currency: { default_currency: string; }) => currency.default_currency === 'Y');
      const defaultCurrencyExchangeRate = defaultCurrency.exchange_rate;
      if (defaultCurrency) {
        this.mdlCurrencyName = defaultCurrency.currencyexchange_gid;
        this.QuoteAddForm.get("exchange_rate")?.setValue(defaultCurrencyExchangeRate);
      }
      this.CurrencyName = defaultCurrency.currency_code;
      this.CurrencyExchangeRate = defaultCurrencyExchangeRate;
      console.log(this.CurrencyName)
    });


    //// Optional Field Config ////
    var api = 'SmrMstSalesConfig/GetAllChargesConfig';
    debugger
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.allchargeslist = this.responsedata.salesconfigalllist;
      this.addoncharge = this.allchargeslist[0].flag;
      this.additional_discount = this.allchargeslist[1].flag;
      this.freight_charges = this.allchargeslist[2].flag;
      this.buybackcharges = this.allchargeslist[3].flag;
      this.insurance_charges = this.allchargeslist[4].flag;
      if (this.allchargeslist[0].flag == 'Y') {
        this.addoncharge = 0;
      } else {
        this.addoncharge = this.allchargeslist[0].flag;
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
        this.buybackcharges = 0;
      } else {
        this.buybackcharges = this.allchargeslist[3].flag;
      }

      if (this.allchargeslist[4].flag == 'Y') {
        this.insurance_charges = 0;
      } else {
        this.insurance_charges = this.allchargeslist[4].flag;
      }

    });
    //// Tax 3 Dropdown ////
    var url = 'SmrTrnQuotation/GetTaxFourSDtl'
    this.service.get(url).subscribe((result: any) => {
      this.tax4_list = result.GetTaxFourSDtl;
    });

    //// Product Dropdown ////
    var url = 'SmrTrnSalesorder/GetProductNamDtl'
    this.service.get(url).subscribe((result: any) => {
      this.product_list = result.GetProductNamDtl;
    });

    //// T & C Dropdown ////
    var url = 'SmrTrnQuotation/GetTermsandConditions'
    this.service.get(url).subscribe((result: any) => {
      this.terms_list = result.GetTermsandConditions;
    });
  }

  constructor(private fb: FormBuilder, private route: ActivatedRoute, private router: Router, private service: SocketService, private ToastrService: ToastrService, public NgxSpinnerService: NgxSpinnerService) {
  }
  getCurrentDate(): string {
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = today.getFullYear();
    return dd + '-' + mm + '-' + yyyy;
  }
  get quotation_date() {
    return this.QuoteAddForm.get('quotation_date')!;
  }
  get customer_name() {
    return this.QuoteAddForm.get('customer_name')!;
  }
  get user_name() {
    return this.QuoteAddForm.get('user_name')!;
  }
  get branch_name() {
    return this.QuoteAddForm.get('branch_name')!;
  }
  get currency_code() {
    return this.QuoteAddForm.get('currency_code')!;
  }
  get payment_days() {
    return this.QuoteAddForm.get('payment_days')!;
  };
  get productgroup_name() {
    return this.productform.get('productgroup_name')!;
  }
  get product_code() {
    return this.productform.get('product_code')!;
  }
  get product_name() {
    return this.productform.get('product_name')!;
  }
  OnChangeCustomer(deencryptedParam: any) {
    let customercontact_gid = deencryptedParam;
    let param = {
      customercontact_gid: customercontact_gid
    }
    var url = 'SmrTrnQuotation/GetOnChangeCustomerDtls';
    this.NgxSpinnerService.show()
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.GetCustomerDet = this.responsedata.GetCustomerdetls;
      console.log('oewinkcld', this.GetCustomerDet)
      this.QuoteAddForm.get("mobile")?.setValue(result.GetCustomerdetls[0].mobile);
      this.QuoteAddForm.get("email")?.setValue(result.GetCustomerdetls[0].email);
      this.QuoteAddForm.get("gst_number")?.setValue(result.GetCustomerdetls[0].gst_number);
      this.QuoteAddForm.get("taxsegment_name")?.setValue(result.GetCustomerdetls[0].taxsegment_name);
      this.QuoteAddForm.get("pricesegment_name")?.setValue(result.GetCustomerdetls[0].pricesegment_name);
      this.QuoteAddForm.get("address1")?.setValue(result.GetCustomerdetls[0].address1);
      this.QuoteAddForm.get("address2")?.setValue(result.GetCustomerdetls[0].address2);
      this.QuoteAddForm.get("city")?.setValue(result.GetCustomerdetls[0].city);
      this.QuoteAddForm.get("zip_code")?.setValue(result.GetCustomerdetls[0].zip_code);
      this.QuoteAddForm.get("country_name")?.setValue(result.GetCustomerdetls[0].country_name);
      this.QuoteAddForm.get("region_name")?.setValue(result.GetCustomerdetls[0].region_name);
      this.QuoteAddForm.get("customer_name")?.setValue(result.GetCustomerdetls[0].customer_name);
      this.NgxSpinnerService.hide()
    });
    this.productSearch();
    this.Quoteproductsummary();
  }
  onClearCustomer() {
    this.txtContactNo = '';
    this.txtpricesegment_name = '';
    this.txttaxsegment_name = '';
    this.txtgst_number = '';
    this.txtEmail = '';
    this.txtaddress1 = '';
    this.txtaddress2 = '';
    this.txtcity = '';
    this.txtregion_name = '';
    this.txtcountry_name = '';
    this.txtzip_code = '';
  }
  onclearproduct() {
    this.productform.get("product_code").setValue('');
    this.productform.get("producraddproductgroup_name").setValue('');
    this.productform.get("unitprice").setValue('');
    this.productsearch = null;
    this.productcodesearch1 = null;
  }
  onclearproductcode() {
    this.productsearch = null;
    this.productcodesearch1 = null;
    this.productcodesearch = null;
  }
  GetOnChangeTerms() {
    let template_gid = this.QuoteAddForm.value.template_name;
    let param = {
      template_gid: template_gid
    }
    var url = 'SmrTrnQuotation/GetOnChangeTerms';
    this.NgxSpinnerService.show()
    this.service.getparams(url, param).subscribe((result: any) => {
      this.QuoteAddForm.get("template_content")?.setValue(result.terms_list[0].termsandconditions);
      this.QuoteAddForm.value.template_gid = result.terms_list[0].template_gid
      this.NgxSpinnerService.hide()

    });

  }
  onInput(event: any) {
    const value = event.target.value;
    const parts = value.split('.');
    // Keep only the first two parts (before and after the decimal point)
    const integerPart = parts[0];
    let decimalPart = parts[1] || '';
    // Limit the decimal part to 2 digits
    decimalPart = decimalPart.slice(0, 2);
    // Update the input value
    event.target.value = `${integerPart}.${decimalPart}`;
    this.unitprice = event.target.value; // Update the model value if necessary
  }
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
  searchOnChange(event: KeyboardEvent) {

    if (event.key !== 'Enter') {

      this.productSearch();
    }
  }
  productAddSubmit() {
    debugger
    var postapi = 'SmrTrnQuotation/GetProductAdd';
    this.NgxSpinnerService.show();
    let param = {

      product_gid: this.productform.value.product_name,
      product_code: this.productform.value.product_code,
      producttype_name: this.productform.value.producttype_name,
      productquantity: this.productform.value.productquantity,
      unitprice: this.productform.value.unitprice,
      productdiscount: this.productform.value.productdiscount,
      producttotal_amount: this.productform.value.producttotal_amount,
      exchange_rate: this.QuoteAddForm.value.exchange_rate,
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
      discount_amount: this.productdiscount_amountvalue,
      discountprecentage: this.productdiscounted_precentagevalue,
      product_remarks: this.productform.value.product_remarks,
      tax_prefix: this.productform.value.tax_prefix,
      taxsegment_gid: this.taxsegment_gid,
      productgroup_name: this.productform.value.producraddproductgroup_name

    }

    this.service.post(postapi, param).subscribe((apiresponse: any) => {
      if (apiresponse.status == false) {
        this.ToastrService.warning(apiresponse.message);
      }
      else {
        this.ToastrService.success(apiresponse.message);
        this.productform.reset();
        this.SOProductList = [];
        this.Quoteproductsummary();
        this.productSearch();
      }
    });
    this.NgxSpinnerService.hide();
    this.productSearch();
  }
  hasTaxData(data: any): boolean {
    return data.taxseg_taxname1 || data.taxseg_taxname2 || data.taxseg_taxname3;
  }
  prodtotalcal(i: any) {
    this.marks = this.POProductList1[i].quantity;
    const unitPrice = this.POProductList1[i].unitprice;
    const quantity = this.POProductList1[i].quantity;
    const discountPercentage = this.POProductList1[i].discount_persentage;

    // Initialize total tax amount
    let totalTaxAmount = 0;

    // Iterate through tax segments of the product
    for (let taxSegment of this.POProductList1[i].taxSegments) {
      // Calculate tax amount for the current tax segment
      const taxAmount = (((quantity * unitPrice) - (quantity * unitPrice * discountPercentage / 100)) * (parseFloat(taxSegment.tax_percentage) / 100));

      // Add tax amount to the total tax amount
      totalTaxAmount += taxAmount;

      // Store individual tax amount for each tax segment
      taxSegment.taxAmount = taxAmount.toFixed(2);
    }

    // Calculate total discount amount
    const discountAmount = (quantity * unitPrice * discountPercentage) / 100;

    // Calculate total amount including tax
    const totalAmount = (quantity * unitPrice) - discountAmount + totalTaxAmount;

    // Update values in the product list
    this.POProductList1[i].discount_amount = discountAmount.toFixed(2);
    this.POProductList1[i].totalTaxAmount = totalTaxAmount.toFixed(2);
    this.POProductList1[i].total_amount = totalAmount.toFixed(2);

    // Calculate total tax amount for all tax segments
    const totalTaxAmountForProduct = this.POProductList1[i].taxSegments.reduce((acc: number, cur: { taxAmount: string; }) => acc + parseFloat(cur.taxAmount), 0);

    totalTaxAmount = totalTaxAmountForProduct.toFixed(2);
    console.log(totalTaxAmount)
    // Log the total tax amount for debugging
    console.log('Total Tax Amount for Product:', totalTaxAmount);
  }
  onclearbranch() {
    this.mdlDispatchadd = null;
  }
  onclearcurrency() {
    this.exchange = null;
  }
  OnClearTax1(i: any) {
    this.POProductList1[i].taxamount1 = 0;
    const subtotal = this.exchange * this.POProductList1[i].unitprice * this.POProductList1[i].quantity;
    this.POProductList1[i].discount_amount = (subtotal * this.POProductList1[i].discount_persentage) / 100;
    this.POProductList1[i].discount_amount = +(this.POProductList1[i].discount_amount).toFixed(2);
    this.POProductList1[i].total_amount = +(subtotal - this.POProductList1[i].discount_amount + this.POProductList1[i].taxamount1 + this.POProductList1[i].taxamount2).toFixed(2);
    this.POProductList1[i].total_amount = +(this.POProductList1[i].total_amount).toFixed(2);
  }
  OnClearTax2(i: any) {
    this.POProductList1[i].taxamount2 = 0;
    const subtotal = this.exchange * this.POProductList1[i].unitprice * this.POProductList1[i].quantity;
    this.POProductList1[i].discount_amount = (subtotal * this.POProductList1[i].discount_persentage) / 100;
    this.POProductList1[i].discount_amount = +(this.POProductList1[i].discount_amount).toFixed(2);
    this.POProductList1[i].total_amount = +(subtotal - this.POProductList1[i].discount_amount + this.POProductList1[i].taxamount2).toFixed(2);
    this.POProductList1[i].total_amount = +(this.POProductList1[i].total_amount).toFixed(2);
  }
  OnChangeTaxAmount1(i: any) {
    let tax_gid = this.POProductList1[i].tax1;
    this.taxpercentage = this.getDimensionsByFilter(tax_gid);
    let tax_percentage = this.taxpercentage[0].percentage;
    this.POProductList1[i].taxamount1 = ((tax_percentage) * (this.POProductList1[i].total_amount) / 100);
    this.POProductList1[i].taxamount1 = +(this.POProductList1[i].taxamount1).toFixed(2);
    if (this.POProductList1[i].taxamount1 == undefined) {
      const subtotal = this.unitprice * this.quantity;
      this.discountamount = (subtotal * this.discountpercentage) / 100;
      this.totalamount = subtotal - this.discountamount;
      this.totalamount = +(this.totalamount).toFixed(2);
    }
    else {
      this.POProductList1[i].totalamount = parseFloat(this.POProductList1[i].total_amount) + parseFloat(this.POProductList1[i].taxamount1);
      this.POProductList1[i].total_amount = +this.POProductList1[i].totalamount.toFixed(2);
    }
  }
  OnChangeTaxAmount4() {
    debugger
    let tax_gid = this.QuoteAddForm.get('tax_name4')?.value;
    this.taxpercentage = this.getDimensionsByFilter(tax_gid);
    let tax_percentage = this.taxpercentage[0].percentage;
    this.tax_amount4 = +(tax_percentage * this.producttotalamount / 100).toFixed(2);
    this.total_amount = +((+this.producttotalamount) + (+this.tax_amount4));
    this.total_amount = +this.total_amount.toFixed(2);
    // this.grandtotal = ((this.total_amount) + (+this.tax_amount4) + (+this.addoncharge) + (+this.freightcharges) +  (+this.packing_charges) + (+this.insurance_charges) + (+this.roundoff) + (+this.totalamount)  - (+this.additional_discount));
    // Convert NaN to 0 for each field
    this.total_amount = isNaN(this.total_amount) ? 0 : this.total_amount;
    this.tax_amount4 = isNaN(this.tax_amount4) ? 0 : this.tax_amount4;
    this.addoncharge = isNaN(this.addoncharge) ? 0 : this.addoncharge;
    this.freight_charges = isNaN(this.freight_charges) ? 0 : this.freight_charges;
    this.packing_charges = isNaN(this.packing_charges) ? 0 : this.packing_charges;
    this.insurance_charges = isNaN(this.insurance_charges) ? 0 : this.insurance_charges;
    this.roundoff = isNaN(this.roundoff) ? 0 : this.roundoff;
    this.totalamount = isNaN(this.totalamount) ? 0 : this.totalamount;
    this.additional_discount = isNaN(this.additional_discount) ? 0 : this.additional_discount;

    // Calculate the grand total
    this.grandtotal = this.total_amount + (+this.tax_amount4) + (+this.addoncharge) + (+this.freight_charges) +
      (+this.packing_charges) + (+this.insurance_charges) + (+this.roundoff) + (+this.totalamount) - (+this.additional_discount);

  }
  OnClearOverallTax() {
    this.tax_amount4 = 0;
    this.total_amount = +((+this.producttotalamount) + (+this.tax_amount4));
    this.total_amount = +this.total_amount.toFixed(2);
    this.grandtotal = ((this.total_amount) + (+this.addoncharge) + (+this.freight_charges) + (+this.packing_charges) + (+this.insurance_charges) + (+this.roundoff) + (+this.totalamount) - (+this.additional_discount));
    this.grandtotal = +this.grandtotal.toFixed(2);
  }
  OnChangeTaxAmount2(i: any) {
    let tax_gid = this.POProductList1[i].tax2;
    this.taxpercentage = this.getDimensionsByFilter(tax_gid);
    let tax_percentage = this.taxpercentage[0].percentage;
    const subtotal = this.POProductList1[i].unitprice * this.POProductList1[i].quantity;
    this.POProductList1[i].discountamount = (subtotal * this.POProductList1[i].discount_persentage) / 100;
    this.POProductList1[i].totalamount = subtotal - this.POProductList1[i].discountamount;
    this.POProductList1[i].taxamount2 = ((tax_percentage) * (this.POProductList1[i].totalamount) / 100);
    if (this.POProductList1[i].taxamount1 == undefined && this.taxamount2 == undefined) {
      const subtotal = this.unitprice * this.quantity;
      this.discountamount = (subtotal * this.discountpercentage) / 100;
      this.totalamount = subtotal - this.discountamount;
    }
    else {
      this.POProductList1[i].totalamount = (parseFloat(this.POProductList1[i].total_amount) + parseFloat(this.POProductList1[i].taxamount2));
      this.POProductList1[i].total_amount = +this.POProductList1[i].totalamount.toFixed(2);
    }
  }
  OnChangeTaxAmount3() {
    let tax_gid3 = this.productform.get('tax_name3')?.value;
    this.taxpercentage = this.getDimensionsByFilter(tax_gid3);
    let tax_percentage = this.taxpercentage[0].percentage;
    const subtotal = this.unitprice * this.quantity;
    this.discountamount = (subtotal * this.discountpercentage) / 100;
    this.totalamount = subtotal - this.discountamount;
    this.taxamount3 = ((tax_percentage) * (this.totalamount) / 100);
    if (this.taxamount1 == undefined && this.taxamount2 == undefined && this.taxamount3 == undefined) {
      const subtotal = this.unitprice * this.quantity;
      this.discountamount = (subtotal * this.discountpercentage) / 100;
      this.totalamount = subtotal - this.discountamount;
    }
    else {
      this.totalamount = ((this.totalamount) + (+this.taxamount1) + (+this.taxamount2) + (+this.taxamount3));
    }
  }
  getDimensionsByFilter(id: any) {
    return this.tax4_list.filter((x: { tax_gid: any; }) => x.tax_gid === id);
  }




  finaltotal() {
    // Ensure all variables are valid numbers
    const addoncharge = isNaN(this.addoncharge) ? 0 : +this.addoncharge;
    const freight_charges = isNaN(this.freight_charges) ? 0 : +this.freight_charges;
    const packing_charges = isNaN(this.packing_charges) ? 0 : +this.packing_charges;
    const insurance_charges = isNaN(this.insurance_charges) ? 0 : +this.insurance_charges;
    const roundoff = isNaN(this.roundoff) ? 0 : +this.roundoff;
    const additional_discount = isNaN(this.additional_discount) ? 0 : +this.additional_discount;
    const tax_amount4 = isNaN(this.tax_amount4) ? 0 : +this.tax_amount4;
    const totalamount = isNaN(this.totalamount) ? 0 : +this.totalamount;
    // Log values for debugging
    console.log("Addon Charge:", addoncharge);
    console.log("Freight Charges:", freight_charges);
    console.log("Packing Charges:", packing_charges);
    console.log("Insurance Charges:", insurance_charges);
    console.log("Round Off:", roundoff);
    console.log("Additional Discount:", additional_discount);
    console.log("Tax Amount4:", tax_amount4);
    this.grandtotal = totalamount + tax_amount4 + addoncharge + freight_charges +
      packing_charges + insurance_charges + roundoff - additional_discount;
    console.log("Grand Total:", this.grandtotal);
    this.grandtotal = isNaN(this.grandtotal) ? 0 : +(this.grandtotal).toFixed(2);
  }


  // finaltotal() {
  //   // Ensure all variables are valid numbers
  //   const addoncharge = isNaN(this.addon_charge) ? 0 : +this.addon_charge;
  //   const freightcharges = isNaN(this.freight_charges) ? 0 : +this.freight_charges;
  //   const packing_charges = isNaN(this.packing_charges) ? 0 : +this.packing_charges;
  //   const insurance_charges = isNaN(this.insurance_charges) ? 0 : +this.insurance_charges;
  //   const roundoff = isNaN(this.roundoff) ? 0 : +this.roundoff;
  //   const additional_discount = isNaN(this.additional_discount) ? 0 : +this.additional_discount;
  //   const tax_amount4 = isNaN(this.tax_amount4) ? 0 : +this.tax_amount4;
  //   const totalamount = isNaN(this.totalamount) ? 0 : +this.totalamount;
  //   this.grandtotal = totalamount + tax_amount4 + addoncharge + freightcharges +
  //   packing_charges + insurance_charges + roundoff - additional_discount;
  //   this.grandtotal = isNaN(this.grandtotal) ? 0 : +(this.grandtotal).toFixed(2);
  // }

  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }
  showTextBox(event: Event) {
    const target = event.target as HTMLInputElement;
    this.showInput = target.value === 'Y';
  }
  OnChangeCurrency() {
    let currencyexchange_gid = this.QuoteAddForm.get("currency_code")?.value;
    let param = {
      currencyexchange_gid: currencyexchange_gid
    }
    var url = 'PmrTrnPurchaseOrder/GetOnChangeCurrency';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.currency_list1 = this.responsedata.GetOnchangeCurrency;
      this.QuoteAddForm.get("exchange_rate")?.setValue(this.currency_list1[0].exchange_rate);
      this.currency_code1 = this.currency_list1[0].currency_code
    });

  }
  onCurrencyCodeChange(event: Event) {
    const target = event.target as HTMLSelectElement;
    const selectedCurrencyCode = target.value;
    this.selectedCurrencyCode = selectedCurrencyCode;
    this.QuoteAddForm.controls.currency_code.setValue(selectedCurrencyCode);
    this.QuoteAddForm.get("currency_code")?.setValue(this.currency_list[0].currency_code);

  }
  onSubmit() {
    debugger
    if (this.productsummary_list == null || this.productsummary_list == undefined) {
      window.scrollTo({
        top: 0,
      });
      this.ToastrService.warning('Atleast One Product Must Be Added!');
      return
    }
    if (
      this.QuoteAddForm.value.quotation_date == "" || this.QuoteAddForm.value.quotation_date == null || this.QuoteAddForm.value.quotation_date == undefined &&
      this.QuoteAddForm.value.customer_name == "" || this.QuoteAddForm.value.customer_name == null || this.QuoteAddForm.value.customer_name == undefined &&
      this.QuoteAddForm.value.user_name == "" || this.QuoteAddForm.value.user_name == null || this.QuoteAddForm.value.user_name == undefined &&
      // this.QuoteAddForm.value.branch_name == "" || this.QuoteAddForm.value.branch_name == null || this.QuoteAddForm.value.branch_name == undefined &&
      this.QuoteAddForm.value.currency_code == "" || this.QuoteAddForm.value.currency_code == null || this.QuoteAddForm.value.currency_code == undefined
    ) {
      window.scrollTo({
        top: 0,
      });
      this.ToastrService.warning('Kindly Fill all the Mandatory Fields!');
      return
    }
    var params = {
      branch_name: this.QuoteAddForm.value.branch_name,
      quotation_referenceno1: this.QuoteAddForm.value.quotation_referenceno1,
      branch_gid: this.QuoteAddForm.value.branch_name.branch_gid,
      quotation_date: this.QuoteAddForm.value.quotation_date,
      quotation_gid: this.QuoteAddForm.value.quotation_gid,
      customer_name: this.QuoteAddForm.value.customer_name,
      customer_gid: this.customer_gid,
      email: this.QuoteAddForm.value.email,
      mobile: this.QuoteAddForm.value.mobile,
      gst_number: this.QuoteAddForm.value.gst_number,
      taxsegment_name: this.QuoteAddForm.value.taxsegment_name,
      pricesegment_name: this.QuoteAddForm.value.pricesegment_name,
      address1: this.QuoteAddForm.value.address1,
      address2: this.QuoteAddForm.value.address2,
      city: this.QuoteAddForm.value.city,
      zip_code: this.QuoteAddForm.value.zip_code,
      country_name: this.QuoteAddForm.value.country_name,
      region_name: this.QuoteAddForm.value.region_name,
      user_name: this.QuoteAddForm.value.user_name,
      exchange_rate: this.QuoteAddForm.value.exchange_rate,
      delivery_days: this.QuoteAddForm.value.delivery_days,
      termsandconditions: this.QuoteAddForm.value.template_content,
      template_name: this.QuoteAddForm.value.template_name,
      template_gid: this.QuoteAddForm.value.template_gid,
      grandtotal: this.QuoteAddForm.value.grandtotal,
      roundoff: this.QuoteAddForm.value.roundoff,
      insurance_charges: this.QuoteAddForm.value.insurance_charges,
      packing_charges: this.QuoteAddForm.value.packing_charges,
      buybackcharges: this.QuoteAddForm.value.buybackcharges,
      freightcharges: this.QuoteAddForm.value.freight_charges,
      additional_discount: this.QuoteAddForm.value.additional_discount,
      addoncharge: this.QuoteAddForm.value.addoncharge,
      total_amount: this.QuoteAddForm.value.totalamount,
      tax_amount4: this.QuoteAddForm.value.tax_amount4,
      currency_code: this.QuoteAddForm.value.currency_code,
      quotation_remarks: this.QuoteAddForm.value.quotation_remarks,
      tax_name4: this.QuoteAddForm.value.tax_name4,
      tax_gid: this.QuoteAddForm.value.tax_gid,
      producttotalamount: this.QuoteAddForm.value.grandtotal,
      payment_days: this.QuoteAddForm.value.payment_days,
      discountamount: this.QuoteAddForm.value.discountamount,
      taxsegment_gid: this.Cmntaxsegment_gid,
    }
    var url = 'SmrTrnQuotation/PostDirectQuotation'
    this.NgxSpinnerService.show()
    this.service.post(url, params).subscribe((result: any) => {
      if (result.status == false) {
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide()
      }
      else {
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.success(result.message);
        this.Leadstage_update();
        window.history.back();
        this.NgxSpinnerService.hide();
      }
    });

  }
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
  Quoteproductsummary() {
    debugger;
    let customercontact_gid = this.customer_gid;
    // var params = { vendor_gid: customercontact_gid, product_gid: "" };
    var api = 'SmrTrnQuotation/GetTempProductsSummary';

    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.productsummary_list = this.responsedata.Gettemporarysummary;

      this.totalamount = this.responsedata.grandtotal.toFixed(2);
      this.producttotalamount = this.responsedata.grandtotal.toFixed(2);
      this.grandtotal = this.responsedata.grandtotal.toFixed(2);

      this.QuoteAddForm.get("totalamount")?.setValue(result.grand_total);
      this.QuoteAddForm.get("grandtotal")?.setValue(result.grandtotal);
      this.currency_code1 = "";



      this.productsummary_list.forEach((product: any) => {
        this.fetchProductSummaryAndTax(product.product_gid);
      });
    });
  }
  fetchProductSummaryAndTax(product_gid: string) {
    if (this.QuoteAddForm.value.vendor_companyname !== undefined) {
      let vendor_gid = this.QuoteAddForm.get("vendor_companyname")?.value;
      let param = {
        product_gid: product_gid,
        vendor_gid: vendor_gid
      };

      var api = 'PmrTrnPurchaseOrder/GetProductSummary';


      this.service.getparams(api, param).subscribe((result: any) => {
        this.responsedata = result;
        this.GetTaxSegmentList = this.responsedata.GetTaxSegmentList;

        // Handle tax segments for the current product
        this.handleTaxSegments(product_gid, this.GetTaxSegmentList);


      }, (error) => {
        console.error('Error fetching tax details:', error);

      });
    }
  }

  // Method to handle tax segments for the current product
  handleTaxSegments(product_gid: string, taxSegments: any[]) {
    // Find tax segments for the current product_gid
    const productTaxSegments = taxSegments.filter((taxSegment: { product_gid: string; }) => taxSegment.product_gid === product_gid);

    if (productTaxSegments.length > 0) {
      // Assign tax segments to the current product
      this.productsummary_list.forEach((product: { product_gid: string; taxSegments: any[]; }) => {
        if (product.product_gid === product_gid) {
          product.taxSegments = productTaxSegments;
        }
      });
    } else {
      // No tax segments found for the current product
      console.warn('No tax segments found for product_gid:', product_gid);
    }
  }

  // Inside your component class
  checkDuplicateTaxSegment(taxSegments: any[], currentIndex: number): boolean {
    // Extract the taxsegment_gid of the current tax segment
    const currentTaxSegmentId = taxSegments[currentIndex].taxsegment_gid;

    // Check if the current tax segment exists before the current index in the array
    for (let i = 0; i < currentIndex; i++) {
      if (taxSegments[i].taxsegment_gid === currentTaxSegmentId) {
        // Duplicate found
        return true;
      }
    }

    // No duplicates found
    return false;
  }
  // Inside your component class
  removeDuplicateTaxSegments(taxSegments: any[]): any[] {
    const uniqueTaxSegmentsMap = new Map<string, any>();
    taxSegments.forEach(taxSegment => {
      uniqueTaxSegmentsMap.set(taxSegment.tax_name, taxSegment);
    });
    // Convert the Map back to an array
    return Array.from(uniqueTaxSegmentsMap.values());
  }
  onProductSelect(event: any): void {

    const product_name = this.SOProductList.find(product => product.product_gid === event.product_gid);
    if (product_name) {
      this.productform.patchValue({
        product_code: product_name.product_code,
        producraddproductgroup_name: product_name.productgroup_gid,
        unitprice: product_name.unitprice,

      });
    }
  }
  onClearCurrency() {
    this.exchange = 0;
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
  ondelete() {
    var url = 'SmrTrnQuotation/GetDeleteQuotationProductSummary'
    this.NgxSpinnerService.show()
    let param = {
      tmpsalesorderdtl_gid: this.parameterValue
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide();
      }
      else {
        window.scrollTo({
          top: 0,
        });
        this.Quoteproductsummary();
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
      }

    });
  }
  onKeyPress(event: any) {
    const key = event.key;
    if (!/^[0-9.]$/.test(key)) {
      event.preventDefault();
    }
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


        this.calculateProductTotal(product_gid);
      });
    }
    else {
      window.scrollTo({
        top: 0,
      });
      this.ToastrService.warning('Kindly Select Customer!');
      return;
    }
  }
  calculateProductTotal(product_gid: any) {
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
          this.tax_prefix = this.individualTaxAmounts[0].tax_prefix,
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
        this.taxgid2 = 0;
        this.taxamount2 = 0;
        this.taxprecentage2 = 0;
      }

      if (this.individualTaxAmounts.length > 2) {
        if (this.individualTaxAmounts[2].tax_gid != null && this.individualTaxAmounts[2].tax_gid != undefined) {
          this.taxgid3 = this.individualTaxAmounts[2].tax_gid;
          this.taxname3 = this.individualTaxAmounts[2].tax_name;
          this.tax_prefix = this.individualTaxAmounts[2].tax_prefix;
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


    }
  }
  OnProductCode(event: any) {
    const product_code = this.SOProductList.find(product => product.product_code === event.product_code);
    // const product_desc = this.SOProductList.find(productdesc => productdesc.product_desc === event.product_desc);
    if (product_code) {
      this.productform.patchValue({
        product_name: product_code.product_gid,
        producraddproductgroup_name: product_code.productgroup_gid
      });
      //   if (product_desc) {
      //     this.productform.patchValue({
      //       product_name: product_desc.product_gid,
      //       producraddproductgroup_name: product_desc.productgroup_gid,
      //       product_remarks: product_desc.product_desc

      //     });
      // }
    }
  }

  onback() {
    window.history.back();
  }
  Leadstage_update() {

    //this.reactiveForm2.get('appointment_gid')?.setValue(this.appointment_gid1);
    if (this.appointment_gid != null) {
      //this.reactiveForm2.get('call_response')?.setValue(sales_stages);
      let param = {
        call_response: "Decision",
        appointment_gid: this.appointment_gid,
      }
      var url = 'Leadbank360/PostLeadStage';
      this.service.post(url, param).pipe().subscribe((result: any) => {

      });
    }
  }
}
