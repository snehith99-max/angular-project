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
  selector: 'app-smr-trn-quotationfrom360-new',
  templateUrl: './smr-trn-quotationfrom360-new.component.html',
  styleUrls: ['./smr-trn-quotationfrom360-new.component.scss']
})
export class SmrTrnQuotationfrom360NewComponent {


  isExpanded: { [key: string]: boolean } = {
    product: false,
    another: false,
    summary: false
  };

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
  txtContactNo: any;
  txtContactPerson: any;
  txtCustomerAddress: any;
  txtEmail: any;
  QuoteAddForm: any;
  cuscontact_gid: any;
  sales_list: any;
  CurrencyName: any;
  CurrencyExchangeRate: any;
  buybackcharges: any;
  tax2_list: any;
  tax3_list: any;
  toggleExpand(section: string) {
    this.isExpanded[section] = !this.isExpanded[section];
  }
  currency_code1: any
  showInput: boolean = false;
  inputValue: string = ''
  config: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '33rem',
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
  freightcharges: any;
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
  customer_gid: any;
  deencryptedParam: any;
  show() {
    const toggleBtn = document.getElementById('toggleBtn');
    const collapseContent = document.getElementById('collapseContent');
    toggleBtn?.addEventListener('click', () => {
      // Toggle the 'show' class on the collapse content element
      collapseContent?.classList.toggle('show');
    });
  }
  back(){
   
    const customer_gid = this.route.snapshot.paramMap.get('customer_gid');
    const leadbank_gid = this.route.snapshot.paramMap.get('customer_gid');
    const lead2campaign_gid = this.route.snapshot.paramMap.get('lead2campaign_gid');
    const leadbankcontact_gid = this.route.snapshot.paramMap.get('leadbankcontact_gid');
    const lspage = this.route.snapshot.paramMap.get('lspage');
    this.router.navigate(['/smr/SmrTrnSales360',leadbank_gid,lead2campaign_gid,leadbankcontact_gid,lspage]) 
  
  }
  
  ngOnInit(): void {


    const customer_gid = this.route.snapshot.paramMap.get('customer_gid');
    const leadbank_gid = this.route.snapshot.paramMap.get('customer_gid');
    const lead2campaign_gid = this.route.snapshot.paramMap.get('lead2campaign_gid');
    const leadbankcontact_gid = this.route.snapshot.paramMap.get('leadbankcontact_gid');
    const lspage = this.route.snapshot.paramMap.get('lspage');
    this.deencryptedParam = customer_gid;
    const secretKey = 'storyboarderp';
    this.customer_gid = AES.decrypt(this.deencryptedParam, secretKey).toString(enc.Utf8);
 

    this.QuoteAddForm = new FormGroup({
      customer_name: new FormControl(''),
      user_name: new FormControl(''),
      branch: new FormControl('', Validators.required),
      branch_name: new FormControl('', Validators.required),
      quotation_date: new FormControl(this.getCurrentDate()),
      dispatch_name: new FormControl('', Validators.required),
      po_date: new FormControl(this.getCurrentDate(), Validators.required),
      vendor_companyname: new FormControl('', Validators.required),
      tax_amount4: new FormControl(''),
      address1: new FormControl(''),
      quotation_remarks: new FormControl(''),
      currency_code: new FormControl(''),
      payment_term: new FormControl(''),
      contact_person: new FormControl(''),
      email_address: new FormControl(''),
      contact_number: new FormControl(''),
      currency: new FormControl('', Validators.required),
      exchange_rate: new FormControl(''),
      remarks: new FormControl(''),
      shipping_address: new FormControl(''),
      vendor_address: new FormControl(''),
      vendor_fax: new FormControl(''),
      priority_n: new FormControl('N'),
      taxamount1: new FormControl(''),
      buybackorscrap: new FormControl(''),
      payment_terms: new FormControl(''),
      freight_terms: new FormControl(''),
      delivery_location: new FormControl(''),
      template_content: new FormControl(''),
      delivery_period: new FormControl(''),
      payment_days: new FormControl('', [Validators.required]),
      product_total: new FormControl(''),
      tax_name: new FormControl(''),
      discount_percentage: new FormControl(''),
      qty: new FormControl(''),
      mrp: new FormControl(''),
      unitprice: new FormControl(''),
      productuom_name: new FormControl(''),
      product_code: new FormControl(''),
      productgroup_name: new FormControl(''),
      product_name: new FormControl(''),
      totalamount: new FormControl(''),
      totalamount3: new FormControl(''),
      tax_name3: new FormControl(''),
      taxamount2: new FormControl(''),
      tax_name2: new FormControl(''),
      tax_name1: new FormControl(''),
      discountamount: new FormControl(''),
      discountpercentage: new FormControl(''),
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
      freightcharges: new FormControl(''),
      addoncharge: new FormControl(''),
      delivery_days: new FormControl('', [Validators.required]),
      template_name: new FormControl(''),
      total_amount: new FormControl(''),
      packing_charges: new FormControl(''),
      tax_name4: new FormControl(''),
      quotation_referenceno1: new FormControl(''),
      customercontact_names: new FormControl(''),
      mobile: new FormControl(''),
      email: new FormControl(''),
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
    })
    this.OnChangeCustomer();

    this.productSearch();
    this.show();

    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);
    // this.Quoteproductsummary();

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
      //console.log(this.CurrencyName)
    });

    //// Optional Field Config ////
    var api = 'SmrMstSalesConfig/GetAllChargesConfig';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.allchargeslist = this.responsedata.salesconfigalllist;
      this.addoncharge = this.allchargeslist[0].flag;
      this.additional_discount = this.allchargeslist[1].flag;
      this.freightcharges = this.allchargeslist[2].flag;
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
        this.freightcharges = 0;
      } else {
        this.freightcharges = this.allchargeslist[2].flag;
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

    //// Tax 1 Dropdown ////no need
    // var url = 'SmrTrnSalesorder/GetTax1Dtl'
    // this.service.get(url).subscribe((result: any) => {
    //   this.tax_list = result.GetTax1Dtl;
    // });

    //// Tax 2 Dropdown ////no need
    // var url = 'SmrTrnSalesorder/GetTax2Dtl'
    // this.service.get(url).subscribe((result: any) => {
    //   this.tax2_list = result.GetTax2Dtl;
    // });

    //// Tax 3 Dropdown ////no need
    // var url = 'SmrTrnSalesorder/GetTax3Dtl'
    // this.service.get(url).subscribe((result: any) => {
    //   this.tax3_list = result.GetTax3Dtl;
    // });

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
  get shipping_address() {
    return this.QuoteAddForm.get('shipping_address')!;
  }
  get vendor_address() {
    return this.QuoteAddForm.get('vendor_address')!;
  }
  get customer_name() {
    return this.QuoteAddForm.get('customer_name')!;
  }
  get user_name() {
    return this.QuoteAddForm.get('user_name')!;
  }
  get contact_person() {
    return this.QuoteAddForm.get('contact_person')!;
  }
  get contact_number() {
    return this.QuoteAddForm.get('contact_number')!;
  }
  get vendor_fax() {
    return this.QuoteAddForm.get('vendor_fax')!;
  }
  get email_address() {
    return this.QuoteAddForm.get('email_address')!;
  }
  get product_name() {
    return this.productform.get('product_name')!;
  }
  get product_code() {
    return this.productform.get('product_code')!;
  }

  get branch_name() {
    return this.QuoteAddForm.get('branch_name')!;
  }
  get dispatch_name() {
    return this.QuoteAddForm.get('dispatch_name')!;
  }
  get vendor_companyname() {
    return this.QuoteAddForm.get('vendor_companyname')!;
  }
  get tax_name1() {
    return this.productform.get('tax_name1')!;
  }
  get productuom_name() {
    return this.productform.get('productuom_name')!;
  }
  get display_name() {
    return this.productform.get('display_name')!;
  }
  get productgroup_name() {
    return this.productform.get('productgroup_name')!;
  }
  get prodnameControl() {
    return this.productform.get('productgid');
  }
  get priority_remarks() {
    return this.productform.get('priority_remarks')
  }
  get tax() {
    return this.productform.get('tax')!;
  }
  get payment_days() {
    return this.QuoteAddForm.get('payment_days')!;
  };
  get producttype_name() {
    return this.productform.get('producttype_name')!;
  }


  onClearCustomer() {

    this.txtContactNo = '';
    this.txtContactPerson = '';
    this.txtCustomerAddress = '';
    this.txtEmail = '';
  }

  OnChangeCustomer() {
    let customercontact_gid = this.customer_gid;
    let param = {
      customercontact_gid: customercontact_gid
    }
    var url = 'SmrTrnQuotation/GetOnChangeCustomerDtls';
    this.NgxSpinnerService.show();
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.GetCustomerDet = this.responsedata.GetCustomerdetls;
      this.QuoteAddForm.get("customer_name")?.setValue(result.GetCustomerdetls[0].customer_name);
      this.QuoteAddForm.get("mobile")?.setValue(result.GetCustomerdetls[0].mobile);
      this.QuoteAddForm.get("customercontact_names")?.setValue(result.GetCustomerdetls[0].customercontact_names);
      this.QuoteAddForm.get("address1")?.setValue(result.GetCustomerdetls[0].address1);
      this.QuoteAddForm.get("email")?.setValue(result.GetCustomerdetls[0].email);
      this.Cmntaxsegment_gid = result.GetCustomerdetls[0].taxsegment_gid;
      this.QuoteAddForm.value.leadbank_gid = result.GetCustomerdetls[0].leadbank_gid;
      this.QuoteAddForm.value.customercontact_gid = result.GetCustomerdetls[0].customercontact_gid;
      this.cuscontact_gid = this.QuoteAddForm.value.customercontact_gid;
      this.NgxSpinnerService.hide();


    });
    this.Quoteproductsummary();
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

  productSearch() {
    let customercontact_gid = this.customer_gid;
    var params = {
      producttype_gid: this.productform.value.producttype_name,
      product_name: this.productform.value.product_name,
      customer_gid: customercontact_gid
    };
    var api = 'SmrTrnQuotation/GetProductsearchSummary';
    this.service.getparams(api, params).subscribe((result: any) => {
      this.responsedata = result;
      this.POProductList1 = this.responsedata.GetProductsearch;
      //console.log('taxsegment',this.POProductList1)

      // Iterate over each product in POProductList1
      for (let product of this.POProductList1) {
        // Filter tax segments based on the current product's product_gid
        const taxSegmentsForProduct = this.responsedata.GetTaxSegmentList.filter((taxSegment: { product_gid: any; }) => taxSegment.product_gid === product.product_gid);

        // Map tax segments to include tax_name, tax_percentage, tax_gid, and taxsegment_gid
        const mappedTaxSegments = taxSegmentsForProduct.map((taxSegment: { tax_name: any; tax_percentage: any; tax_gid: any; taxsegment_gid: any; }) => ({
          tax_name: taxSegment.tax_name,
          tax_percentage: taxSegment.tax_percentage,
          tax_gid: taxSegment.tax_gid,
          taxsegment_gid: taxSegment.taxsegment_gid
        }));

        // Get unique tax_gids and taxsegment_gids for the product
        const uniqueTaxGids = [new Set(mappedTaxSegments.map((segment: { tax_gid: any; }) => segment.tax_gid))];
        const uniqueTaxSegmentGids = [new Set(mappedTaxSegments.map((segment: { taxsegment_gid: any; }) => segment.taxsegment_gid))];

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

  // searchOnChange(event: KeyboardEvent) {

  //   if (event.key !== 'Enter') {

  //     this.productSearch();
  //   }
  // }

  productSubmit() {
    if (this.QuoteAddForm.value.customer_name == "" || this.QuoteAddForm.value.customer_name == null || this.QuoteAddForm.value.customer_name == undefined) {
      window.scrollTo({
        top: 0,
      });
      this.ToastrService.warning('Kindly Select Customer!');
      return
    }
    this.toggleCollapsesection('section3');
    const api = 'SmrTrnQuotation/PostOnAddproduct';
    this.NgxSpinnerService.show();

    // Set a timer to hide the spinner after 3 seconds
    const spinnerTimer = setTimeout(() => {
      this.NgxSpinnerService.hide();
    }, 3000);
    const params = {
      ProductList: this.POProductList1,
      taxsegment_gid: this.Cmntaxsegment_gid,
      exchange_rate: this.CurrencyExchangeRate,
      currency_code: this.CurrencyName,
      quotation_gid: this.productform.value.quotation_gid,
      tmpquotationdtl_gid: this.productform.tmpquotationdtl_gid,
    };
    //console.log(params)
    this.service.post(api, params).subscribe((result: any) => {
      clearTimeout(spinnerTimer); // Clear the spinner timer
      if (result.status == false) {
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.warning(result.message);
      } else {
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.success(result.message);
        this.POProductList1 = [];

        this.Quoteproductsummary();
      }
      this.NgxSpinnerService.hide();
    });
    const toggleBtn = document.getElementById('toggleBtn');
    const collapseContent = document.getElementById('collapseContent');
    toggleBtn?.addEventListener('click', () => {
      collapseContent?.classList.toggle('show');
    });
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
    //console.log(totalTaxAmount)
    // Log the total tax amount for debugging
    //console.log('Total Tax Amount for Product:', totalTaxAmount);
  }


  onclearvendor() {
    this.mdlcontactperson = null;
    this.mdlcontactnumber = null;
    this.mdlemailaddress = null;
    this.mdlvendoraddress = null;
    this.mdlvendorfax = null;
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
    this.freightcharges = isNaN(this.freightcharges) ? 0 : this.freightcharges;
    this.packing_charges = isNaN(this.packing_charges) ? 0 : this.packing_charges;
    this.insurance_charges = isNaN(this.insurance_charges) ? 0 : this.insurance_charges;
    this.roundoff = isNaN(this.roundoff) ? 0 : this.roundoff;
    this.totalamount = isNaN(this.totalamount) ? 0 : this.totalamount;
    this.additional_discount = isNaN(this.additional_discount) ? 0 : this.additional_discount;

    // Calculate the grand total
    this.grandtotal = this.total_amount + (+this.tax_amount4) + (+this.addoncharge) + (+this.freightcharges) +
      (+this.packing_charges) + (+this.insurance_charges) + (+this.roundoff) + (+this.totalamount) - (+this.additional_discount);

  }
  OnClearOverallTax() {
    this.tax_amount4 = 0;
    this.total_amount = +((+this.producttotalamount) + (+this.tax_amount4));
    this.total_amount = +this.total_amount.toFixed(2);
    this.grandtotal = ((this.total_amount) + (+this.addoncharge) + (+this.freightcharges) + (+this.packing_charges) + (+this.insurance_charges) + (+this.roundoff) + (+this.totalamount) - (+this.additional_discount));
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
    return this.tax_list.filter((x: { tax_gid: any; }) => x.tax_gid === id);
  }
  finaltotal() {


    // Ensure all variables are valid numbers
    const addoncharge = isNaN(this.addoncharge) ? 0 : +this.addoncharge;
    const freightcharges = isNaN(this.freightcharges) ? 0 : +this.freightcharges;
    const packing_charges = isNaN(this.packing_charges) ? 0 : +this.packing_charges;
    const insurance_charges = isNaN(this.insurance_charges) ? 0 : +this.insurance_charges;
    const roundoff = isNaN(this.roundoff) ? 0 : +this.roundoff;
    const additional_discount = isNaN(this.additional_discount) ? 0 : +this.additional_discount;
    const tax_amount4 = isNaN(this.tax_amount4) ? 0 : +this.tax_amount4;
    const totalamount = isNaN(this.totalamount) ? 0 : +this.totalamount;
    // Log values for debugging
    // console.log("Addon Charge:", addoncharge);
    //console.log("Freight Charges:", freightcharges);
    // console.log("Packing Charges:", packing_charges);
    // console.log("Insurance Charges:", insurance_charges);
    // console.log("Round Off:", roundoff);
    // console.log("Additional Discount:", additional_discount);
    // console.log("Tax Amount4:", tax_amount4);

    // Calculate the grand total
    this.grandtotal = totalamount + tax_amount4 + addoncharge + freightcharges +
      packing_charges + insurance_charges + roundoff - additional_discount;

    // Log grand total for debugging
    //console.log("Grand Total:", this.grandtotal);

    // Ensure the grand total is a valid number with 2 decimal places
    this.grandtotal = isNaN(this.grandtotal) ? 0 : +(this.grandtotal).toFixed(2);
  }


  openModaldelete(parameter: string) {
    this.parameterValue = parameter
    //console.log("dea", this.parameterValue)
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
      this.QuoteAddForm.value.branch_name == "" || this.QuoteAddForm.value.branch_name == null || this.QuoteAddForm.value.branch_name == undefined &&
      this.QuoteAddForm.value.payment_days == "" || this.QuoteAddForm.value.payment_days == null || this.QuoteAddForm.value.payment_days == undefined &&
      this.QuoteAddForm.value.delivery_days == "" || this.QuoteAddForm.value.delivery_days == null || this.QuoteAddForm.value.delivery_days == undefined &&
      this.QuoteAddForm.value.currency_code == "" || this.QuoteAddForm.value.currency_code == null || this.QuoteAddForm.value.currency_code == undefined
    ) {
      window.scrollTo({
        top: 0,
      });
      this.ToastrService.warning('Kindly Fill all the Mandatory Fields!');
      return
    }
    debugger
    var params = {
      branch_name: this.QuoteAddForm.value.branch_name,
      quotation_referenceno1: this.QuoteAddForm.value.quotation_referenceno1,
      branch_gid: this.QuoteAddForm.value.branch_name.branch_gid,
      quotation_date: this.QuoteAddForm.value.quotation_date,
      quotation_gid: this.QuoteAddForm.value.quotation_gid,
      customer_name: this.QuoteAddForm.value.customer_name,
      customercontact_names: this.QuoteAddForm.value.customercontact_names,
      email: this.QuoteAddForm.value.email,
      mobile: this.QuoteAddForm.value.mobile,
      quotation_remarks: this.QuoteAddForm.value.quotation_remarks,
      address1: this.QuoteAddForm.value.address1,
      freight_terms: this.QuoteAddForm.value.freight_terms,
      payment_terms: this.QuoteAddForm.value.payment_terms,
      currency_code: this.QuoteAddForm.value.currency_code,
      user_name: this.QuoteAddForm.value.user_name,
      exchange_rate: this.QuoteAddForm.value.exchange_rate,
      payment_days: this.QuoteAddForm.value.payment_days,
      customer_gid: this.customer_gid,
      termsandconditions: this.QuoteAddForm.value.template_content,
      template_name: this.QuoteAddForm.value.template_name,
      template_gid: this.QuoteAddForm.value.template_gid,
      grandtotal: this.QuoteAddForm.value.grandtotal,
      roundoff: this.QuoteAddForm.value.roundoff,
      insurance_charges: this.QuoteAddForm.value.insurance_charges,
      packing_charges: this.QuoteAddForm.value.packing_charges,
      buybackcharges: this.QuoteAddForm.value.buybackcharges,
      freightcharges: this.QuoteAddForm.value.freightcharges,
      additional_discount: this.QuoteAddForm.value.additional_discount,
      addoncharge: this.QuoteAddForm.value.addoncharge,
      total_amount: this.QuoteAddForm.value.totalamount,
      tax_amount4: this.QuoteAddForm.value.tax_amount4,
      tax_name4: this.QuoteAddForm.value.tax_name4,
      tax_gid: this.QuoteAddForm.value.tax_gid,
      producttotalamount: this.QuoteAddForm.value.grandtotal,
      customercontact_gid: this.cuscontact_gid,
      delivery_days: this.QuoteAddForm.value.delivery_days,
      discountamount: this.QuoteAddForm.value.discountamount,
      taxsegment_gid: this.Cmntaxsegment_gid,
    }
//console.log('csigid',params)
    var url = 'SmrTrnQuotation/PostDirectQuotation'
    this.NgxSpinnerService.show()
    this.service.postparams(url, params).subscribe((result: any) => {
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
        this.ToastrService.success(result.message)
        this.router.navigate(['/smr/SmrTrnQuotationSummary']);
        this.NgxSpinnerService.hide()
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
    let customercontact_gid = this.customer_gid;
    // var params = { vendor_gid: customercontact_gid, product_gid: "" };
    var api = 'SmrTrnQuotation/GetTempProductsSummary';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.productsummary_list = this.responsedata.prodsummary_list;
      this.totalamount = this.responsedata.total_amount.toFixed(2);
      this.producttotalamount = this.responsedata.ltotalamount.toFixed(2);
      this.QuoteAddForm.get("totalamount")?.setValue(this.responsedata.total_amount.toFixed(2));
      this.QuoteAddForm.get("grandtotal")?.setValue(this.responsedata.ltotalamount.toFixed(2));
      this.currency_code1 = "";
      //console.log('peolist',  this.productsummary_list)
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
        //console.error('Error fetching tax details:', error);

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
      //console.warn('No tax segments found for product_gid:', product_gid);
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
  onClearCurrency() {
    this.exchange = 0;
  }
  ondelete() {
    var url = 'SmrTrnQuotation/GetDeleteQuotationProductSummary'
    this.NgxSpinnerService.show()
    let param = {
      tmpquotationdtl_gid: this.parameterValue
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)

        this.NgxSpinnerService.hide()
      }
      else {

        this.ToastrService.success(result.message)

        this.Quoteproductsummary();
        this.NgxSpinnerService.hide()
      }
    });
  }












}
