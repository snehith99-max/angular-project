import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { NgxSpinnerService } from 'ngx-spinner';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { AES, enc } from 'crypto-js';

@Component({
  selector: 'app-smr-trn-renewal-edit',
  templateUrl: './smr-trn-renewal-edit.component.html',
  styleUrls: ['./smr-trn-renewal-edit.component.scss']
})
export class SmrTrnRenewalEditComponent {

  config: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '20rem',
    placeholder: 'Enter text here...',
    translate: 'no',
    defaultParagraphSeparator: 'p',

  };
  combinedFormData!: FormGroup;
  productform!: FormGroup | any;
  productgroup_list: any[] = [];
  GetTaxSegmentList: any[] = [];
  Getproductgroup: any[] = [];
  branch_list: any[] = [];
  customer_list: any[] = [];
  SOProductList: any[] = [];
  filteredSOProductList: any[] = [];
  individualTaxAmounts: any[] = [];
  GetCustomerDet: any[] = [];
  salesorders_list: any[] = [];
  currency_list1: any[] = [];
  currency_list: any[] = [];
  tax4_list: any[] = [];
  terms_list: any[] = [];
  productsearch: any;
  productcodesearch1: any;
  mdlBranchName: any;
  productcodesearch: any;
  productquantity: any;
  productunitprice: any;
  producttotal_amount: any;
  tax_prefix2: any;
  txtaddress1: any;
  txtcustomer_details: any;
  productdiscount: any=0;
  mdlCustomerName: any;
  taxgid1: any;
  taxgid2: any;
  taxgid3: any;
  taxname1: any;
  taxname2: any;
  taxname3: any;
  tax_prefix: any;
  tax_name4: any;
  currency_code1: any;
  taxsegment_gid: any;
  txtshipping_address: any;
  CurrencyExchangeRate: any;
  responsedata: any;
  addressdetails: any;
  taxamount1: any = 0;
  taxamount2: any = 0;
  taxamount3: any = 0;
  taxprecentage1: any = 0;
  taxprecentage2: any = 0;
  taxprecentage3: any = 0;
  exchange: any;
  mdlTerms: any;
  mdlCurrencyName: any;
  CurrencyName: any;
  grandtotal: any;
  total_price: any;
  mdlTaxName4: any;
  addon_charge: any=0;
  freight_charges: any=0;
  packing_charges: any=0;
  roundoff: any=0;
  insurance_charges: any=0;
  additional_discount: any=0;
  tax_amount4: any=0;
  total_amount: any=0;
  taxprecentage4: any=0;
  productdiscount_amountvalue: number = 0;
  productdiscounted_precentagevalue: number = 0;
  producttotal_tax_amount: number = 0;
  totalamount: number = 0;
  producttotalamount: number = 0;
  renewalgid:any;
  renewal_gid:any;
  customer_gid:any;
  salesorder_gid:any;
  parameterValue:any;

  constructor(private NgxSpinnerService: NgxSpinnerService,
    private service: SocketService,
    private ToastrService: ToastrService,
    private router: Router,
    private route : ActivatedRoute,
  ) { }

  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);

    const key = 'storyboard';
    this.renewalgid = this.route.snapshot.paramMap.get('renewalgid');
    this.renewal_gid = AES.decrypt(this.renewalgid,key).toString(enc.Utf8);

    this.Summary(this.renewal_gid);

    this.combinedFormData = new FormGroup({
      renewal_date: new FormControl(''),
      order_date: new FormControl(''),
      order_ref_no: new FormControl(''),
      customer_name: new FormControl(''),
      contact_person_name: new FormControl(''),
      mobile_number: new FormControl(''),
      email: new FormControl(''),
      address1: new FormControl(''),
      shipping_address: new FormControl(''),
      city: new FormControl(''),
      state: new FormControl(''),
      country: new FormControl(''),
      currency_code: new FormControl(''),
      branch_name: new FormControl(''),
      agreement_referencenumber: new FormControl(''),
      agreement_date: new FormControl(''),
      delivery_days: new FormControl(''),
      payment_days: new FormControl(''),
      customer_details: new FormControl(''),
      exchange_rate: new FormControl(''),
      agreement_remarks: new FormControl(''),
      template_name: new FormControl(''),
      totalamount: new FormControl(''),
      freight_charges: new FormControl(''),
      tax_amount: new FormControl(''),
      tax_name4: new FormControl(''),
      grandtotal: new FormControl(''),
      additional_discount: new FormControl(''),
      termsandconditions: new FormControl(''),
      addon_charge: new FormControl(''),
      tax_amount4: new FormControl(''),
      roundoff: new FormControl(''),
    });

    this.productform = new FormGroup({
      tmpsalesorderdtl_gid: new FormControl(''),
      tax_gid: new FormControl(''),
      product_gid: new FormControl(''),
      product_remarks: new FormControl(''),
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
      producttype_name: new FormControl(''),
      total_amount: new FormControl(''),
      productquantity: new FormControl(''),
      productunitprice: new FormControl(''),
      productdiscount: new FormControl(''),
      producttotal_amount: new FormControl(''),
      producraddproductgroup_name: new FormControl(''),
      tax_prefix: new FormControl(''),
      tax_prefix2: new FormControl(''),
      productdiscount_amountvalue: new FormControl(''),
      taxamount1: new FormControl(''),
      taxamount2: new FormControl(''),
    });

    var taxapi = 'SmrTrnSalesorder/GetTax4Dtl';
    this.service.get(taxapi).subscribe((apiresponse: any) => {
      this.tax4_list = apiresponse.GetTax4Dtl;
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

    var url = 'SmrTrnSalesorder/GetCustomerDtl'
    this.service.get(url).subscribe((result: any) => {
      this.customer_list = result.GetCustomerDtl;
    });

    var productgroupapi = 'SmrTrnSalesorder/GetProductGroup';
    this.service.get(productgroupapi).subscribe((apiresponse: any) => {
      this.Getproductgroup = apiresponse.Getproductgroup;
    });

    var url = 'SmrTrnSalesorder/GetBranchDtl'
    this.service.get(url).subscribe((result: any) => {
      this.branch_list = result.GetBranchDtl;
      const firstBranch = this.branch_list[0];
      const branchName = firstBranch.branch_gid;
      this.combinedFormData.get('branch_name')?.setValue(branchName);
      this.combinedFormData.get('branch_address')?.setValue(this.branch_list[0].address1)
    });
    
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
      const customer_mobile = this.GetCustomerDet[0].customer_mobile;
      const customer_email = this.GetCustomerDet[0].customer_email;
      const gst_number = this.GetCustomerDet[0].gst_number;
      const customerDetails = `${customer_mobile}\n${customer_email}\n${gst_number}`;
      this.combinedFormData.get("customer_details")?.setValue(customerDetails);

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

      this.combinedFormData.get("address1")?.setValue(this.addressdetails);
      this.combinedFormData.get("shipping_address")?.setValue(this.addressdetails);
      // this.combinedFormData.get("shipping_address")?.setValue(address2);
      this.productSearch();
      this.SOproductsummary();
    });

  }
  OnChangeCurrency() {
debugger
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
  async Summary(renewal_gid: any) {
    debugger
    this.NgxSpinnerService.show();
    var url = 'SmrTrnRenewalsummary/GetEditrenewalSummary';
    let param = { renewal_gid: renewal_gid};
    this.service.getparams(url,param).subscribe((result:any)=>{
      this.combinedFormData.get('branch_name')?.setValue(result.renewalview_list[0].branch_name);
      this.combinedFormData.get('customer_name')?.setValue(result.renewalview_list[0].customer_name);
      this.combinedFormData.get('agreement_referencenumber')?.setValue(result.renewalview_list[0].so_referencenumber);
      this.combinedFormData.get('agreement_date')?.setValue(result.renewalview_list[0].salesorder_date);
      this.combinedFormData.get('address1')?.setValue(result.renewalview_list[0].customer_address);
      this.combinedFormData.get('shipping_address')?.setValue(result.renewalview_list[0].shipping_to);
      this.combinedFormData.get('renewal_date')?.setValue(result.renewalview_list[0].renewal_date);
      this.combinedFormData.get('delivery_days')?.setValue(result.renewalview_list[0].delivery_days);
      this.combinedFormData.get('payment_days')?.setValue(result.renewalview_list[0].payment_days);
      //this.combinedFormData.get('currency_code')?.setValue(result.renewalview_list[0].currency_gid);
      
      this.combinedFormData.get('exchange_rate')?.setValue(result.renewalview_list[0].exchange_rate);
      this.combinedFormData.get('agreement_remarks')?.setValue(result.renewalview_list[0].so_remarks);
      this.combinedFormData.get('termsandconditions')?.setValue(result.renewalview_list[0].termsandconditions);
      this.combinedFormData.get('totalamount')?.setValue(result.renewalview_list[0].total_price);
      this.combinedFormData.get('freight_charges')?.setValue(result.renewalview_list[0].freight_charges);
      this.combinedFormData.get('tax_amount4')?.setValue(result.renewalview_list[0].tax_amount);
      this.combinedFormData.get('tax_name4')?.setValue(result.renewalview_list[0].tax_gid);
      this.combinedFormData.get('roundoff')?.setValue(result.renewalview_list[0].roundoff);
      this.combinedFormData.get('grandtotal')?.setValue(result.renewalview_list[0].total_amount);
      this.combinedFormData.get('additional_discount')?.setValue(result.renewalview_list[0].additional_discount);
      this.combinedFormData.get('addon_charge')?.setValue(result.renewalview_list[0].addon_charge);
      this.customer_gid = result.renewalview_list[0].customer_gid;
      this.salesorder_gid = result.renewalview_list[0].salesorder_gid;
      const email = result.renewalview_list[0].customer_email;
      const mobile = result.renewalview_list[0].customer_mobile;
      const gst_number = result.renewalview_list[0].gst_number;
      const customerDetails = `${mobile}\n${email}\n${gst_number}`;
      this.combinedFormData.get('customer_details')?.setValue(customerDetails);
      this.mdlCurrencyName = result.renewalview_list[0].currency_gid
      this.NgxSpinnerService.hide();
    });
    
    this.productSearch();
    await this.ProductSummaryRenewalEdit(this.renewal_gid);
  }
  async ProductSummaryRenewalEdit(renewal_gid: any){
    this.NgxSpinnerService.show();
    var url = 'SmrTrnRenewalsummary/GetProductSummaryRenewalEdit';
    let param = { renewal_gid: renewal_gid};
    await this.service.getparams(url, param).toPromise();
    this.NgxSpinnerService.hide();
    await this.SOproductsummary();
   
  }
  onClearCurrency() {
    this.exchange = null;
  }
  productSearch() {
    var api = 'SmrTrnSalesorder/GetProductsearchSummarySales';
    this.service.get(api).subscribe((result: any) => {
      this.SOProductList = result.GetProductsearchs;
      this.filteredSOProductList = this.SOProductList;
    });
    
  }
  GetOnChangeProductsGroup() {
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
  OnProductCode(event: any) {
    const product_code = this.SOProductList.find(product => product.product_code === event.product_code);
    if (product_code) {
      this.productform.patchValue({
        product_name: product_code.product_gid,
        producraddproductgroup_name: product_code.productgroup_gid,
        product_remarks : product_code.product_desc
      });
    }
  }
  onclearproductcode() {
    this.productsearch = null;
    this.productcodesearch1 = null;
    this.productcodesearch = null;
  }
  onProductSelect(event: any): void {
    const product_name = this.SOProductList.find(product => product.product_gid === event.product_gid);
    if (product_name) {
      this.productform.patchValue({
        product_code: product_name.product_code,
        producraddproductgroup_name: product_name.productgroup_gid,
        unitprice: product_name.unitprice,
        product_remarks : product_name.product_desc
      });
    }
  }
  onclearproduct() {
    this.productform.get("product_code").setValue('');
    this.productform.get("producraddproductgroup_name").setValue('');
    this.productform.get("unitprice").setValue('');
    this.productsearch = null;
    this.productcodesearch1 = null;
  }
  //--------------------------------calculation---------------------------------------//
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
    debugger
    const product_gid = this.productform.value.product_name;
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
          this.tax_prefix2 = this.individualTaxAmounts[1].tax_prefix;
          this.tax_prefix = this.individualTaxAmounts[0].tax_prefix;
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
  //----------------------------------calculation end-----------------------------------------//

  onKeyPress(event: any) {
    const key = event.key;
    if (!/^[0-9.]$/.test(key)) {
      event.preventDefault();
    }
  }
  //------------------------------product submit------------------------------------------------//
  productAddSubmit() {
    this.NgxSpinnerService.show();
    if (this.combinedFormData.value.customer_name == "" || this.combinedFormData.value.customer_name == null || this.combinedFormData.value.customer_name == undefined) {
      window.scrollTo({
        top: 0,
      });
      this.ToastrService.warning('Kindly Select Customer!');
      return
    }
    var postapi = 'SmrTrnRenewalsummary/ProductAddRenewalEdit';
    this.NgxSpinnerService.show();
    let param = {
      product_name: this.productform.value.product_name,
      product_code: this.productform.value.product_code,
      producttype_name: this.productform.value.producttype_name,
      productquantity: this.productform.value.productquantity,
      unitprice: this.productform.value.unitprice,
      productdiscount: this.productform.value.productdiscount,
      producttotal_amount: this.productform.value.producttotal_amount,
      exchange_rate: this.CurrencyExchangeRate,
      customer_gid: this.customer_gid,
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
      tax_prefix: this.productform.value.tax_prefix,
      taxamount1: this.productform.value.taxamount1,
      product_remarks: this.productform.value.product_remarks,
      taxsegment_gid: this.taxsegment_gid,
      productgroup_name: this.productform.value.producraddproductgroup_name,
      renewal_gid: this.renewal_gid
    }
    this.service.post(postapi, param).subscribe((apiresponse: any) => {
      if (apiresponse.status == false) {
        this.ToastrService.warning(apiresponse.message);
        this.NgxSpinnerService.hide();
      }
      else {
        this.ToastrService.success(apiresponse.message);
        this.NgxSpinnerService.hide();
        this.productform.reset();
        this.SOProductList = [];
        this.productSearch();
        this.SOproductsummary();
      }
    });
    this.NgxSpinnerService.hide();
    this.productSearch();
    this.SOproductsummary();
  }
  SOproductsummary() {
    this.NgxSpinnerService.show();
    let customer_gid = this.customer_gid;

    var params = {      
      renewal_gid: this.renewal_gid,
    };
    var api = 'SmrTrnRenewalsummary/GettmpProductSummaryRenewalEdit';

    this.service.getparams(api, params).subscribe((result: any) => {
      this.responsedata = result;
      this.salesorders_list = this.responsedata.ProductSummaryRenewal_list;

      this.totalamount = this.responsedata.grandtotal.toFixed(2);
      this.producttotalamount = this.responsedata.grandtotal.toFixed(2);
      this.combinedFormData.get("totalamount")?.setValue(this.responsedata.grandtotal.toFixed(2));
      this.combinedFormData.get("grandtotal")?.setValue(this.responsedata.grandtotal.toFixed(2));
      this.currency_code1 = "";
      this.combinedFormData.get("totalamount")?.setValue(result.grand_total);
      if (this.tax_name4 == null || this.tax_name4 == "" || this.tax_name4 == "--No Tax--") {
        this.combinedFormData.get("grandtotal")?.setValue(result.grandtotal);
      }
      this.NgxSpinnerService.hide();
    });
    
  }
  OnChangeCustomer1() {
    this.txtshipping_address = '';
    this.txtcustomer_details = '';
    this.txtaddress1 = '';
  }
  onSubmit() {
    debugger
    var params = {
      customer_gid: this.customer_gid,
      branch_name: this.combinedFormData.value.branch_name,
      branch_gid: this.combinedFormData.value.branch_name.branch_gid,
      agreement_date: this.combinedFormData.value.agreement_date,
      renewal_date: this.combinedFormData.value.renewal_date,
      customer_name: this.combinedFormData.value.customer_name,
      agreement_remarks: this.combinedFormData.value.agreement_remarks,
      agreement_referencenumber: this.combinedFormData.value.agreement_referencenumber,
      address1: this.combinedFormData.value.address1,
      shipping_address: this.combinedFormData.value.shipping_address,
      currency_code: this.combinedFormData.value.currency_code,
      delivery_days: this.combinedFormData.value.delivery_days,
      payment_days: this.combinedFormData.value.payment_days,
      user_name: this.combinedFormData.value.user_name,
      exchange_rate: this.combinedFormData.value.exchange_rate,
      termsandconditions: this.combinedFormData.value.termsandconditions,
      template_name: this.combinedFormData.value.template_name,
      template_gid: this.combinedFormData.value.template_gid,
      grandtotal: this.combinedFormData.value.grandtotal,
      roundoff: this.combinedFormData.value.roundoff,
      insurance_charges: this.combinedFormData.value.insurance_charges,
      packing_charges: this.combinedFormData.value.packing_charges,
      buyback_charges: this.combinedFormData.value.buyback_charges,
      freight_charges: this.combinedFormData.value.freight_charges,
      additional_discount: this.combinedFormData.value.additional_discount,
      addon_charge: this.combinedFormData.value.addon_charge,
      tax_amount4: this.combinedFormData.value.tax_amount4,
      tax_name4: this.combinedFormData.value.tax_name4,
      totalamount: this.combinedFormData.value.totalamount,
      total_price: this.combinedFormData.value.total_price,
      taxsegment_gid: this.taxsegment_gid,
      renewal_gid: this.renewal_gid,
      salesorder_gid: this.salesorder_gid
    }

    var url = 'SmrTrnRenewalsummary/PostRenewalEdit'
    this.NgxSpinnerService.show();
    this.service.postparams(url, params).subscribe((result: any) => {
      if (result.status == false) {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message)
      }
      else {
        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message)
        // this.salesordertypecount()
        this.router.navigate(['/smr/SmrTrnRenevalsummary']);
      }
    });
  }
  finaltotal() {
    const addoncharge = isNaN(this.addon_charge) ? 0 : +this.addon_charge;
    const freightcharges = isNaN(this.freight_charges) ? 0 : +this.freight_charges;
    const packing_charges = isNaN(this.packing_charges) ? 0 : +this.packing_charges;
    const insurance_charges = isNaN(this.insurance_charges) ? 0 : +this.insurance_charges;
    const roundoff = isNaN(this.roundoff) ? 0 : +this.roundoff;
    const additional_discount = isNaN(this.additional_discount) ? 0 : +this.additional_discount;
    const tax_amount4 = isNaN(this.tax_amount4) ? 0 : +this.tax_amount4;
    const totalamount = isNaN(this.totalamount) ? 0 : +this.totalamount;
    this.grandtotal = totalamount + tax_amount4 + addoncharge + freightcharges +
    packing_charges + insurance_charges + roundoff - additional_discount;
    this.grandtotal = isNaN(this.grandtotal) ? 0 : +(this.grandtotal).toFixed(2);
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
    debugger
    let tax_gid4 = this.combinedFormData.get("tax_name4")?.value;
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
  getDimensionsByFilter(id: any) {
    return this.tax4_list.filter((x: { tax_gid: any; }) => x.tax_gid === id);
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
    });
  }
  onUpdate(){
  }
  openModaldelete(salesorderdtl_gid:any){ 
    this.parameterValue = salesorderdtl_gid
  }
  ondelete(){
    this.NgxSpinnerService.show();
    var url = 'SmrTrnSalesorder/GetDeleteDirectSOProductSummary'
    this.NgxSpinnerService.show();
    let param = {
      tmpsalesorderdtl_gid: this.parameterValue
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        this.NgxSpinnerService.hide();        
        this.SOproductsummary();
        this.productSearch();
        this.ToastrService.warning(result.message)
      }
      else {
        this.SOproductsummary();
        this.productSearch();
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
      }
    });
  }
}
