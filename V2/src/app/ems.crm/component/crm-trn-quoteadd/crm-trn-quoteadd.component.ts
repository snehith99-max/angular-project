import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { AES, enc } from 'crypto-js';
import { data } from 'jquery';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { NgxSpinnerService } from 'ngx-spinner';

interface quotationadd {

  salesorder_gid: string;
  salesorder_date: string;
  branch_name: string;
  branch_gid: string;
  so_referencenumber: string;
  customer_gid: string;
  customer_name: string;
  customer_contact_gid: string;
  customercontact_names: string;
  customer_mobile: string;
  customer_email: string;
  salesperson_gid: string;
  user_name: string;
  customer_address: string;
  shipping_to: string;
  so_remarks: string;
  start_date: string;
  end_date: string;
  freight_terms: string;
  payment_terms: string;
  currencyexchange_gid: string;
  currency_code: string;
  exchange_rate: string;
  productgroup_gid: string;
  productgroup_name: string;
  cost_price: string;
  customerproduct_code: string;
  product_code: string;
  product_gid: string;
  product_name: string;
  display_field: string;
  qty_quoted: string;
  mrp_price: string;
  margin_percentage: string;
  margin_amount: string;
  product_price: string;
  tax1_gid: string;
  tax_name: string;
  tax_amount: string;
  tax2_gid: string;
  tax_name2: string;
  tax_amount2: string;
  tax3_gid: string;
  tax_name3: string;
  tax_amount3: string;
  tax_gid4: string;
  tax_name4: string;

  price: string;
  product_requireddate: string;
  product_requireddateremarks: string;
  template_content: string;
}
@Component({
  selector: 'app-crm-trn-quoteadd',
  templateUrl: './crm-trn-quoteadd.component.html',
  styleUrls: ['./crm-trn-quoteadd.component.scss']
})
export class CrmTrnQuoteaddComponent {
  txtContactPerson: any;
  txtContactNo: any;
  txtCustomerAddress: any;
  txtEmail: any;
  showInput: boolean = false;
  inputValue: string = ''
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
  combinedFormData: FormGroup | any;
  template_content: FormGroup | any;
  productform: FormGroup | any;
  branch_list: any[] = [];
  customer_list: any[] = [];
  CurrencyName: any;
  contact_list: any[] = [];
  user_list: any[] = [];
  sales_list: any[] = [];
  currency_list: any[] = [];
  product_list: any[] = [];
  productgroup_list: any[] = [];
  productname_list: any[] = [];
  Quotation360Product_list: any[] = [];
  tax_list: any[] = [];
  tax2_list: any[] = [];
  tax3_list: any[] = [];
  tax4_list: any[] = [];
  QAproductlist: any[] = [];
  terms_list: any[] = [];
  currency_list1: any[] = [];
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
  leadbankcontact_gid: any;
  leadbank_gid: any;
  lspage: any;
  lead2campaign_gid: any;
  taxamount3: number = 0.00;
  taxamount2: number = 0.00;
  taxamount1: number = 0.00;
  grandtotal: number = 0.00;
  totalamount: number = 0.00;
  tax_amount: number = 0.00;
  tax_amount2: number = 0.00;
  tax_amount3: number = 0.00;
  taxpercentage: any;
  discountamount: any;
  discountpercentage: number = 0.00;
  GetproductsCode: any;
  quantity: number = 0.00;
  lspage1: any;
  cost_price: number = 0.00;
  responsedata: any;
  quotationadd!: quotationadd;
  addoncharge: number = 0.00;
  freightcharges: number = 0.00;
  buybackcharges: number = 0.00;
  packing_charges: number = 0.00;
  insurance_charges: number = 0.00;
  roundoff: number = 0.00;
  additional_discount: number = 0.00;
  producttotalamount: any;
  mdlTaxName4: any;
  tax_amount4: any;
  parameterValue: any;
  cuscontact_gid: any;
  total_amount: any;;
  leadgig: any;
  exchange: number = 0.00;
  raisequotedetail_list: any;
  customercontact_names: any;
  selectedItem: any;
  allchargeslist: any[] = [];
  showUpdateButton: boolean = false;
  showAddButton: boolean = true;
  constructor(private formBuilder: FormBuilder, private route: Router, public NgxSpinnerService: NgxSpinnerService, private ToastrService: ToastrService, public service: SocketService, private router: ActivatedRoute) {
    this.quotationadd = {} as quotationadd;
    this.combinedFormData = new FormGroup({
      quotation_date: new FormControl(this.getCurrentDate()),
      branch_name: new FormControl(''),
      quotation_referenceno1: new FormControl(''),
      branch_gid: new FormControl(''),
      customer_gid: new FormControl(''),
      quotation_gid: new FormControl(''),
      customer_name: new FormControl(''),
      customercontact_names: new FormControl(''),
      mobile: new FormControl(''),
      email: new FormControl(''),
      user_gid: new FormControl(''),
      user_name: new FormControl(''),
      address1: new FormControl(''),
      quotation_remarks: new FormControl(''),
      freight_terms: new FormControl(''),
      payment_terms: new FormControl(''),
      currencyexchange_gid: new FormControl(''),
      currency_code: new FormControl(''),
      exchange_rate: new FormControl(''),
      productgroup_gid: new FormControl(''),
      productgroup_name: new FormControl(''),
      cost_price: new FormControl(''),
      customerproduct_code: new FormControl(''),
      product_code: new FormControl(''),
      product_gid: new FormControl(''),
      product_name: new FormControl(''),
      product_requireddate: new FormControl(''),
      product_requireddateremarks: new FormControl(''),
      totalamount: new FormControl(''),
      grandtotal: new FormControl(''),
      termsandconditions: new FormControl(''),
      template_name: new FormControl(''),
      roundoff: new FormControl(''),
      insurance_charges: new FormControl(''),
      packing_charges: new FormControl(''),
      buybackcharges: new FormControl(''),
      freightcharges: new FormControl(''),
      additional_discount: new FormControl(''),
      addoncharge: new FormControl(''),
      total_amount: new FormControl(''),
      tax_amount4: new FormControl(''),
      tax_name4: new FormControl(''),
      producttotalamount: new FormControl(''),
      delivery_days: new FormControl(''),
      payment_days: new FormControl(''),
      customercontact_gid: new FormControl(''),
      tax_gid: new FormControl(''),


    });

    this.productform = new FormGroup({
      productgroup_gid: new FormControl(''),
      productgroup_name: new FormControl(''),
      product_code: new FormControl(''),
      product_gid: new FormControl(''),
      product_name: new FormControl(''),
      quantity: new FormControl(''),
      selling_price: new FormControl(''),
      tax_gid: new FormControl(''),
      tax_name: new FormControl(''),
      tax_amount: new FormControl(''),
      tmpquotationdtl_gid: new FormControl(''),
      totalamount: new FormControl(''),
      product_uom: new FormControl(''),
      productuom_name: new FormControl('', Validators.required),
      product_requireddate: new FormControl(''),
      product_requireddateremarks: new FormControl(''),
      cost_price: new FormControl('', Validators.required),
      discountpercentage: new FormControl('', Validators.required),
      discountamount: new FormControl('', Validators.required),
      taxname: new FormControl('', Validators.required),
      unitprice: new FormControl('')

    });
  }

  ngOnInit(): void {
    this.Quotationproductsummary();
    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);


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
    const deencryptedParam3 = AES.decrypt(this.lspage, secretKey).toString(enc.Utf8);

    this.lspage1 = deencryptedParam3;

    console.log("leadbank_gid =" + deencryptedParam);
    console.log("leadbankcontact_gid = " + deencryptedParam1);
    console.log("lead2campaign_gid = " + deencryptedParam2);
    if (deencryptedParam != null) {
      this.leadbank_gid = (deencryptedParam);
    }

    var url = 'SmrSalesOrder360/GetCustomerDtlCRM'
    let params = {
      leadbank_gid: deencryptedParam,
    }
    this.service.getparams(url, params).subscribe((result: any) => {
      this.customer_list = result.GetCustomerSOCRM;
    });

    //// Branch Dropdown /////
    var url = 'SmrQuotation360CRM/GetBranchQCRM'
    this.service.get(url).subscribe((result: any) => {
      this.branch_list = result.GetBranchQCRM;
      const firstBranch = this.branch_list[0];
      const branchName = firstBranch.branch_gid;
      this.combinedFormData.get('branch_name')?.setValue(branchName);
    });




    //// Sales person Dropdown ////
    var url = 'SmrQuotation360CRM/GetSalesPersonQCRM'
    this.service.get(url).subscribe((result: any) => {
      this.sales_list = result.GetSalesPersonQCRM;
    });

    //// Currency Dropdown ////

    var url = 'SmrQuotation360CRM/GetCurrencyQCRM'
    this.service.get(url).subscribe((result: any) => {
      this.currency_list = result.GetCurrencyQCRM;
    });

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

    //// Tax 1 Dropdown ////
    var url = 'SmrQuotation360CRM/GetTax1QCRM'
    this.service.get(url).subscribe((result: any) => {
      this.tax_list = result.GetTaxQCRM;
    });

    //// Tax 3 Dropdown ////
    var url = 'SmrQuotation360CRM/GetTax2QCRM'
    this.service.get(url).subscribe((result: any) => {
      this.tax4_list = result.GetTax2QCRM;
    });

    //// Product Dropdown ////
    var url = 'SmrQuotation360CRM/GetProductNameQCRM'
    this.service.get(url).subscribe((result: any) => {
      this.product_list = result.GetProductDropdown_QCRM;
    });

    //// T & C Dropdown ////
    var url = 'SmrQuotation360CRM/GetTermsandConditionsQCRM'
    this.service.get(url).subscribe((result: any) => {
      this.terms_list = result.GetTermsQCRM;
    });
  }
  getCurrentDate(): string {
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = today.getFullYear();

    return dd + '-' + mm + '-' + yyyy;
  }

  get branch_name() {
    return this.combinedFormData.get('branch_name')!;
  }
  get customer_name() {
    return this.combinedFormData.get('customer_name')!;
  }
  get user_name() {
    return this.combinedFormData.get('user_name')!;
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
  get currency_code() {
    return this.productform.get('currency_code')!;
  }
  get payment_days() {
    return this.combinedFormData.get('payment_days')!;
  }
  get delivery_days() {
    return this.combinedFormData.get('delivery_days')!;
  }

  OnChangeCustomer() {
    debugger
    let customercontact_gid = this.combinedFormData.value.customer_name.customer_gid;
    let param = {
      customercontact_gid: customercontact_gid
    }
    var url = 'SmrQuotation360CRM/GetOnChangeCustomerQCRM';
    this.NgxSpinnerService.show()
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.GetCustomerDet = result.GetCustomerOnChange_QCRM;

      this.combinedFormData.get("mobile")?.setValue(result.GetCustomerOnChange_QCRM[0].mobile);
      this.combinedFormData.get("customercontact_names")?.setValue(result.GetCustomerOnChange_QCRM[0].customercontact_names);
      this.combinedFormData.get("address1")?.setValue(result.GetCustomerOnChange_QCRM[0].address1);
      this.combinedFormData.get("email")?.setValue(result.GetCustomerOnChange_QCRM[0].email);
      this.combinedFormData.value.leadbank_gid = result.GetCustomerOnChange_QCRM[0].leadbank_gid;
      this.combinedFormData.value.customercontact_gid = result.GetCustomerOnChange_QCRM[0].customercontact_gid;
      this.cuscontact_gid = this.combinedFormData.value.customercontact_gid;
      this.NgxSpinnerService.hide()
      console.log(result.GetCustomerOnChange_QCRM[0])
    });

  }
  GetOnChangeProductsName() {
    debugger
    let product_gid = this.productform.value.product_name.product_gid;
    if (this.combinedFormData.value.customer_name != undefined) {
      let customercontact_gid = this.combinedFormData.value.customer_name.customer_gid;
      let param = {
        product_gid: product_gid,
        customercontact_gid: customercontact_gid
      }
      var url = 'SmrQuotation360CRM/GetOnChangeProductNameQCRM';
      this.NgxSpinnerService.show()
      this.service.getparams(url, param).subscribe((result: any) => {
        this.responsedata = result;
        this.GetproductsCode = this.responsedata.GetProductOnChange_QCRM;
        this.productform.get("product_code")?.setValue(result.GetProductOnChange_QCRM[0].product_code);
        this.productform.get("productuom_name")?.setValue(result.GetProductOnChange_QCRM[0].productuom_name);
        this.productform.get("productgroup_name")?.setValue(result.GetProductOnChange_QCRM[0].productgroup_name);
        this.productform.get("unitprice")?.setValue(result.GetProductOnChange_QCRM[0].unitprice);
        this.productform.value.productgroup_gid = result.GetProductOnChange_QCRM[0].productgroup_gid
        this.NgxSpinnerService.hide()
      });
    }
    else if (this.productform.value.product_name.product_gid) {
      let param = {
        product_gid: product_gid,

      }
      var url = 'SmrQuotation360CRM/GetOnChangeProductNamesQCRM';
      this.NgxSpinnerService.show()
      this.service.getparams(url, param).subscribe((result: any) => {
        this.responsedata = result;
        this.GetproductsCode = this.responsedata.GetProductOnChange_QCRM;
        this.productform.get("product_code")?.setValue(result.GetProductOnChange_QCRM[0].product_code);
        this.productform.get("productuom_name")?.setValue(result.GetProductOnChange_QCRM[0].productuom_name);
        this.productform.get("productgroup_name")?.setValue(result.GetProductOnChange_QCRM[0].productgroup_name);
        this.productform.get("unitprice")?.setValue(result.GetProductOnChange_QCRM[0].unitprice);
        this.productform.value.productgroup_gid = result.GetProductOnChange_QCRM[0].productgroup_gid
        this.NgxSpinnerService.hide()
      });

    }
    else {
      this.ToastrService.warning('Kindly Select Customer Before Adding Product !! ')
    }
  }

  GetOnChangeTerms() {
    let template_gid = this.combinedFormData.value.template_name;
    let param = {
      template_gid: template_gid
    }
    var url = 'SmrQuotation360CRM/GetOnChangeTermsQCRM';
    this.NgxSpinnerService.show()
    this.service.getparams(url, param).subscribe((result: any) => {
      this.combinedFormData.get("termsandconditions")?.setValue(result.GetOnChangeTerms_QCRM[0].termsandconditions);
      this.combinedFormData.value.template_gid = result.GetOnChangeTerms_QCRM[0].template_gid
      this.NgxSpinnerService.hide()
    });
  }

  getDimensionsByFilter(id: any) {
    return this.tax_list.filter((x: { tax_gid: any; }) => x.tax_gid === id);
  }
  prodtotalcal() {
    if (this.quantity == 0 || this.quantity == null) {
      this.totalamount = 0.00;
      this.discountpercentage == 0.00;
      this.discountamount == 0.00;
      this.tax_amount == 0.00;
      this.OnClearTax();

    }
    else {
      console.log(this.CurrencyName)

      if (this.CurrencyName == 'INR') {
        const subtotal = this.exchange * this.cost_price * this.quantity;
        this.discountamount = (subtotal * this.discountpercentage) / 100;
        this.discountamount = (this.discountamount * 100) / 100;
        this.totalamount = subtotal - this.discountamount;
        this.totalamount = (+(subtotal - this.discountamount));
        this.totalamount = parseFloat(this.totalamount.toFixed(2)); // Limit to 2 decimal points

      }
      else {
        const subtotal = this.exchange * this.cost_price * this.quantity;
        this.discountamount = (subtotal * this.discountpercentage) / 100;
        this.discountamount = parseFloat(this.discountamount.toFixed(2)); // Limit to 2 decimal points
        this.totalamount = subtotal - this.discountamount;
        this.totalamount = parseFloat(this.totalamount.toFixed(2)); // Limit to 2 decimal points

        if (this.totalamount % 1 !== 0) {
          this.totalamount = (this.totalamount * 10) / 10
        }
      }
    }


  }

  OnChangeTaxAmount1() {
    debugger
    if (this.CurrencyName == 'INR') {
      let tax_name = this.productform.get('tax_name')?.value;
      let selectedTax = this.tax_list.find(tax => tax.tax_name === tax_name);

      let tax_gid = selectedTax.tax_gid;
      this.taxpercentage = this.getDimensionsByFilter(tax_gid);
      let tax_percentage = this.taxpercentage[0].percentage;

      // Calculate the tax amount with two decimal points
      this.tax_amount = (+(tax_percentage * this.totalamount / 100));

      // Calculate the new total amount
      const subtotal = this.exchange * this.cost_price * this.quantity;
      this.discountamount = (subtotal * this.discountpercentage) / 100;
      this.discountamount = (this.discountamount * 100) / 100;
      this.totalamount = (+(subtotal - this.discountamount + this.tax_amount));

      this.total_amount = +((this.total_amount).toFixed(2));

    }
    else {
      // Calculate the subtotal with two decimal points
      const subtotal = parseFloat((this.exchange * this.cost_price * this.quantity).toFixed(2));

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
      this.totalamount = parseFloat((subtotal - this.discountamount + this.tax_amount).toFixed(2));

      if (this.totalamount % 1 !== 0) {
        this.totalamount = (this.totalamount * 10) / 10
      }

    }
  }

  OnChangeTaxAmount4() {

    let tax_gid = this.combinedFormData.get('tax_name4')?.value;
    this.taxpercentage = this.getDimensionsByFilter(tax_gid);

    let tax_percentage = this.taxpercentage[0].percentage;


    this.tax_amount4 = (+(tax_percentage * this.producttotalamount / 100));
    this.total_amount = +((+this.producttotalamount) + (+this.tax_amount4));
    this.total_amount = (+this.total_amount);
    this.total_amount = parseFloat(this.total_amount.toFixed(2));
    this.grandtotal = ((this.total_amount) + (+this.addoncharge) + (+this.freightcharges) - (+this.buybackcharges) + (+this.packing_charges) + (+this.insurance_charges) + (+this.roundoff) - (+this.additional_discount));
    this.grandtotal = parseFloat(this.grandtotal.toFixed(2));
  }


  finaltotal() {
    if (this.total_amount == null || this.total_amount == "" || this.total_amount == 0 || this.total_amount == "NaN") {
      this.grandtotal = ((this.producttotalamount) + (+this.addoncharge) + (+this.freightcharges) + (+this.packing_charges) + (+this.insurance_charges) + (+this.roundoff) - (+this.buybackcharges) - (+this.additional_discount));
      this.grandtotal = +((this.grandtotal).toFixed(2));
    }
    else {
      const total_amount = parseFloat(this.total_amount)
      const totalAmount = isNaN((total_amount)) ? 0 : (total_amount);
      const addoncharges = isNaN(this.combinedFormData.value.addoncharge) ? 0 : +this.combinedFormData.value.addoncharge;
      const frieghtcharges = isNaN(this.combinedFormData.value.freightcharges) ? 0 : +this.combinedFormData.value.freightcharges;
      const buybackcharges = isNaN(this.combinedFormData.value.buybackcharges) ? 0 : +this.combinedFormData.value.buybackcharges;
      const insurancecharges = isNaN(this.combinedFormData.value.insurance_charges) ? 0 : +this.combinedFormData.value.insurance_charges;
      const packing_charges = isNaN(this.combinedFormData.value.packing_charges) ? 0 : +this.combinedFormData.value.packing_charges;
      const roundoff = isNaN(this.combinedFormData.value.roundoff) ? 0 : +this.combinedFormData.value.roundoff;
      const discountamount = isNaN(this.additional_discount) ? 0 : +this.additional_discount;


      this.grandtotal = (((totalAmount) + (addoncharges) + (frieghtcharges) + (packing_charges) + (insurancecharges) + (roundoff) - (discountamount)) - (buybackcharges));
      this.grandtotal = +((this.grandtotal).toFixed(2));
    }
  }

  Quotationproductsummary() {
    var api = 'SmrQuotation360CRM/GetQCRMTempProductSummary';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.QAproductlist = this.responsedata.Quotation360Product_list;

      this.combinedFormData.get("producttotalamount")?.setValue(result.total_amount);
      if (this.mdlTaxName4 == null || this.mdlTaxName4 == "" || this.mdlTaxName4 == "--No Tax--") {
        this.combinedFormData.get("grandtotal")?.setValue(result.total_amount);
      }
      else {
        let selectedTax = this.tax4_list.find(tax => tax.tax_name4 === this.mdlTaxName4);
        this.taxpercentage = this.getDimensionsByFilter(selectedTax.tax_gid);
        let tax_percentage = this.taxpercentage[0].percentage;
        this.tax_amount4 = +(((tax_percentage * result.grand_total / 100)).toFixed(2));
        const taxamount = +(((result.grand_total + this.tax_amount4)).toFixed(2));
        const newGrandTotal = result.grand_total + parseFloat(this.tax_amount4);
        const newGrandTotal2 = newGrandTotal + parseFloat(this.combinedFormData.value.addoncharge);
        const newGrandTotal3 = newGrandTotal2 - parseFloat(this.combinedFormData.value.additional_discount);
        const newGrandTotal4 = newGrandTotal3 + parseFloat(this.combinedFormData.value.freightcharges);
        const newGrandTotal5 = newGrandTotal4 - parseFloat(this.combinedFormData.value.buybackcharges);
        const newGrandTotal6 = newGrandTotal5 + parseFloat(this.combinedFormData.value.packing_charges);
        const newGrandTotal7 = newGrandTotal6 + parseFloat(this.combinedFormData.value.insurance_charges);
        const newGrandTotal8 = newGrandTotal7 + parseFloat(this.combinedFormData.value.roundoff);
        this.combinedFormData.get("tax_amount4")?.setValue((this.tax_amount4).toFixed(2));
        this.combinedFormData.get("total_amount")?.setValue((taxamount).toFixed(2));
        this.combinedFormData.get("grandtotal")?.setValue((newGrandTotal8).toFixed(2));
      }

    });


  }

  productSubmit() {
    var params = {
      quotation_gid: this.productform.value.quotation_gid,
      tmpquotationdtl_gid: this.productform.tmpquotationdtl_gid,
      product_name: this.productform.value.product_name.product_name,
      product_gid: this.productform.value.product_name.product_gid,
      quantity: this.productform.value.quantity,
      unitprice: this.productform.value.unitprice,
      tax_name: this.productform.value.tax_name,
      tax_gid: this.productform.value.tax_gid,
      tax_amount: this.productform.value.tax_amount,
      discountamount: this.productform.value.discountamount,
      discountpercentage: this.productform.value.discountpercentage,
      product_code: this.productform.value.product_code,
      productuom_gid: this.productform.value.productuom_gid,
      productuom_name: this.productform.value.productuom_name,
      totalamount: this.productform.value.totalamount,
      producttotalamount: this.productform.value.producttotalamount,
    }
    var api = 'SmrQuotation360CRM/PostAddProductQCRM';
    this.NgxSpinnerService.show()
    this.service.post(api, params).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide()
      }
      else {
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide()
      }
      this.Quotationproductsummary();
      this.productform.reset();
    },
    );
  }

  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }

  ondelete() {
    var url = 'SmrQuotation360CRM/DeleteQCRMProductSummary'
    this.NgxSpinnerService.show()
    let param = {
      tmpquotationdtl_gid: this.parameterValue
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
      }
      else {

        this.ToastrService.success(result.message)
        this.Quotationproductsummary();
        this.NgxSpinnerService.hide()
      }
    });
  }

  onInput(event: any) {
    const value = event.target.value;
    const parts = value.split('.');
    const integerPart = parts[0];
    let decimalPart = parts[1] || '';

    // Limit the decimal part to 2 digits
    decimalPart = decimalPart.slice(0, 2);

    // Update the input value
    event.target.value = `${integerPart}.${decimalPart}`;
    this.cost_price = event.target.value; // Update the model value if necessary
  }
  OnClearTax() {

    this.tax_amount = 0;
    const subtotal = this.exchange * this.cost_price * this.quantity;
    this.discountamount = (subtotal * this.discountpercentage) / 100;
    this.totalamount = +(subtotal - this.discountamount + this.tax_amount).toFixed(2);
  }
  OnClearOverallTax() {
    this.tax_amount4 = '';
    this.total_amount = '';
    this.grandtotal = ((this.producttotalamount) + (+this.addoncharge) + (+this.freightcharges) - (+this.buybackcharges) + (+this.packing_charges) + (+this.insurance_charges) + (+this.roundoff) - (+this.additional_discount));
    this.grandtotal = (+this.grandtotal);
  }
  onClearCustomer() {
    debugger
    this.txtContactNo = '';
    this.txtContactPerson = '';
    this.txtCustomerAddress = '';
    this.txtEmail = '';
  }
  onClearProduct() {
    this.mdlProductCode = '';
    this.mdlProductUom = '';
    this.cost_price = 0.00;
    this.quantity = 0.00;
    this.tax_amount = 0.00;
    this.discountamount = 0.00;
    this.mdlTaxName1 = '';
    this.discountpercentage = 0.00;
    this.totalamount = 0.00;
  }
  OnSubmit() {
    var params = {
      branch_name: this.combinedFormData.value.branch_name,
      quotation_referenceno1: this.combinedFormData.value.quotation_referenceno1,
      branch_gid: this.combinedFormData.value.branch_name.branch_gid,
      quotation_date: this.combinedFormData.value.quotation_date,
      quotation_gid: this.combinedFormData.value.quotation_gid,
      customer_name: this.combinedFormData.value.customer_name.customer_name,
      customercontact_names: this.combinedFormData.value.customercontact_names,
      email: this.combinedFormData.value.email,
      mobile: this.combinedFormData.value.mobile,
      quotation_remarks: this.combinedFormData.value.quotation_remarks,
      address1: this.combinedFormData.value.address1,
      freight_terms: this.combinedFormData.value.freight_terms,
      payment_terms: this.combinedFormData.value.payment_terms,
      currency_code: this.combinedFormData.value.currency_code,
      user_name: this.combinedFormData.value.user_name,
      exchange_rate: this.combinedFormData.value.exchange_rate,
      payment_days: this.combinedFormData.value.payment_days,
      customer_gid: this.combinedFormData.value.customer_name.customer_gid,
      termsandconditions: this.combinedFormData.value.termsandconditions,
      template_name: this.combinedFormData.value.template_name,
      template_gid: this.combinedFormData.value.template_gid,
      grandtotal: this.combinedFormData.value.grandtotal,
      roundoff: this.combinedFormData.value.roundoff,
      insurance_charges: this.combinedFormData.value.insurance_charges,
      packing_charges: this.combinedFormData.value.packing_charges,
      buybackcharges: this.combinedFormData.value.buybackcharges,
      freightcharges: this.combinedFormData.value.freightcharges,
      additional_discount: this.combinedFormData.value.additional_discount,
      addoncharge: this.combinedFormData.value.addoncharge,
      total_amount: this.combinedFormData.value.total_amount,
      tax_amount4: this.combinedFormData.value.tax_amount4,
      tax_name4: this.combinedFormData.value.tax_name4,
      producttotalamount: this.combinedFormData.value.producttotalamount,
      customercontact_gid: this.cuscontact_gid,
      delivery_days: this.combinedFormData.value.delivery_days,
      discountamount: this.combinedFormData.value.discountamount,
    }
    var url = 'SmrQuotation360CRM/PostDirectQuotationCRM'
    this.NgxSpinnerService.show()
    this.service.postparams(url, params).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
      }
      else {
        this.ToastrService.success(result.message)
        const secretKey = 'storyboarderp';
        const leadbank_gid = AES.encrypt(this.leadbank_gid, secretKey).toString();

        if (this.lspage1 == 'MM-Total') {
          this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
          this.NgxSpinnerService.hide()
        }
        else if (this.lspage1 == 'MM-Upcoming') {
          this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
          this.NgxSpinnerService.hide()
        }
        else if (this.lspage1 == 'MM-Lapsed') {
          this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
          this.NgxSpinnerService.hide()
        }
        else if (this.lspage1 == 'MM-Longest') {
          this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
          this.NgxSpinnerService.hide()
        }
        else if (this.lspage1 == 'MM-New') {
          this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
          this.NgxSpinnerService.hide()
        }
        else if (this.lspage1 == 'MM-Prospect') {
          this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
          this.NgxSpinnerService.hide()
        }
        else if (this.lspage1 == 'MM-Potential') {
          this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
          this.NgxSpinnerService.hide()
        }
        else if (this.lspage1 == 'MM-mtd') {
          this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
          this.NgxSpinnerService.hide()
        }
        else if (this.lspage1 == 'MM-ytd') {
          this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
          this.NgxSpinnerService.hide()
        }
        else if (this.lspage1 == 'MM-Customer') {
          this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
          this.NgxSpinnerService.hide()
        }
        else if (this.lspage1 == 'MM-Drop') {
          this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
          this.NgxSpinnerService.hide()
        }
        else if (this.lspage1 == 'My-Today') {
          this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
          this.NgxSpinnerService.hide()
        }
        else if (this.lspage1 == 'My-New') {
          this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
          this.NgxSpinnerService.hide()
        }
        else if (this.lspage1 == 'My-Prospect') {
          this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
          this.NgxSpinnerService.hide()
        }
        else if (this.lspage1 == 'My-Potential') {
          this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
          this.NgxSpinnerService.hide()
        }
        else if (this.lspage1 == 'My-Customer') {
          this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
          this.NgxSpinnerService.hide()
        }
        else if (this.lspage1 == 'My-Drop') {
          this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
          this.NgxSpinnerService.hide()
        }
        else if (this.lspage1 == 'My-All') {
          this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
          this.NgxSpinnerService.hide()
        }
        else if (this.lspage1 == 'My-Upcoming') {
          this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
          this.NgxSpinnerService.hide()
        }

        else {
          this.route.navigate(['/smr/SmrTrnQuotationSummary']);
          this.NgxSpinnerService.hide()
        }
      }
    });
  }

  onback() {
    const secretKey = 'storyboarderp';
    const leadbank_gid = AES.encrypt(this.leadbank_gid, secretKey).toString();

    if (this.lspage1 == 'MM-Total') {
      this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
    }
    else if (this.lspage1 == 'MM-Upcoming') {
      this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
    }
    else if (this.lspage1 == 'MM-Lapsed') {
      this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
    }
    else if (this.lspage1 == 'MM-Longest') {
      this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
    }
    else if (this.lspage1 == 'MM-New') {
      this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
    }
    else if (this.lspage1 == 'MM-Prospect') {
      this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
    }
    else if (this.lspage1 == 'MM-Potential') {
      this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
    }
    else if (this.lspage1 == 'MM-mtd') {
      this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
    }
    else if (this.lspage1 == 'MM-ytd') {
      this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
    }
    else if (this.lspage1 == 'MM-Customer') {
      this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
    }
    else if (this.lspage1 == 'MM-Drop') {
      this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
    }
    else if (this.lspage1 == 'My-Today') {
      this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
    }
    else if (this.lspage1 == 'My-New') {
      this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
    }
    else if (this.lspage1 == 'My-Prospect') {
      this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
    }
    else if (this.lspage1 == 'My-Potential') {
      this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
    }
    else if (this.lspage1 == 'My-Customer') {
      this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
    }
    else if (this.lspage1 == 'My-Drop') {
      this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
    }
    else if (this.lspage1 == 'My-All') {
      this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
    }
    else if (this.lspage1 == 'My-Upcoming') {
      this.route.navigate(['/crm/CrmTrn360view', leadbank_gid, this.lead2campaign_gid, this.lspage]);
    }
    else {
      this.route.navigate(['/crm/CrmDashboard']);
    }
  }

  OnChangeCurrency() {
    let currencyexchange_gid = this.combinedFormData.get("currency_code")?.value;
    console.log(currencyexchange_gid)
    let param = {
      currencyexchange_gid: currencyexchange_gid
    }
    var url = 'SmrTrnQuotation/GetOnChangeCurrency';
    this.NgxSpinnerService.show()
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.currency_list1 = this.responsedata.GetOnchangecurrency;
      this.combinedFormData.get("exchange_rate")?.setValue(this.currency_list1[0].exchange_rate);
      this.NgxSpinnerService.hide()

    });
  }

  // PRODUCT EDIT SUMMARY
  editproduct(tmpquotationdtl_gid: any) {
    var url = 'SmrQuotation360CRM/GetQuotation360EditProductSummary'
    this.NgxSpinnerService.show()
    let param = {
      tmpquotationdtl_gid: tmpquotationdtl_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.Quotation360Product_list = result.Quotation360Product_list;
      this.productform.get("tmpquotationdtl_gid")?.setValue(this.Quotation360Product_list[0].tmpquotationdtl_gid);
      this.productform.get("product_name")?.setValue(this.Quotation360Product_list[0].product_name);
      this.productform.get("product_gid")?.setValue(this.Quotation360Product_list[0].product_gid);
      this.productform.get("product_code")?.setValue(this.Quotation360Product_list[0].product_code);
      this.productform.get("productuom_name")?.setValue(this.Quotation360Product_list[0].productuom_name);
      this.productform.get("quantity")?.setValue(this.Quotation360Product_list[0].quantity);
      this.productform.get("totalamount")?.setValue(this.Quotation360Product_list[0].totalamount);
      this.productform.get("tax_name")?.setValue(this.Quotation360Product_list[0].tax_name);
      this.productform.get("tax_gid")?.setValue(this.Quotation360Product_list[0].tax_gid);
      this.productform.get("unitprice")?.setValue(this.Quotation360Product_list[0].unitprice);
      this.productform.get("tax_amount")?.setValue(this.Quotation360Product_list[0].tax_amount);
      this.productform.get("discountpercentage")?.setValue(this.Quotation360Product_list[0].discountpercentage);
      this.productform.get("discountamount")?.setValue(this.Quotation360Product_list[0].discountamount);
      this.NgxSpinnerService.hide()
      this.showUpdateButton = true;
      this.showAddButton = false;
    });
  }
  openModelDetail(product_gid: any) {
    debugger
    var url = 'SmrTrnQuotation/GetRaiseQuotedetail'
    let params = {
      product_gid: product_gid
    }
    this.service.getparams(url, params).subscribe((result: any) => {
      this.responsedata = result;
      this.raisequotedetail_list = this.responsedata.Directeddetailslist1;
    });

  }

  onupdate() {
    var params = {
      tmpquotationdtl_gid: this.productform.value.tmpquotationdtl_gid,
      product_code: this.productform.value.product_code,
      product_name: this.productform.value.product_name.product_name == undefined ? this.productform.value.product_name : this.productform.value.product_name.product_name,
      product_gid: this.productform.value.product_name.product_gid == undefined ? this.productform.value.product_gid : this.productform.value.product_name.product_gid,
      productuom_name: this.productform.value.productuom_name,
      quantity: this.productform.value.quantity,
      unitprice: this.productform.value.unitprice,
      discountamount: this.productform.value.discountamount,
      discountpercentage: this.productform.value.discountpercentage,
      productgroup_gid: this.productform.value.productgroup_gid,
      productuom_gid: this.productform.value.productuom_gid,
      tax_name: this.productform.value.tax_name.tax_name == undefined ? this.productform.value.tax_name : this.productform.value.tax_name.tax_name,
      tax_gid: this.productform.value.tax_name.tax_gid,
      tax_amount: this.productform.value.tax_amount,
      totalamount: this.productform.value.totalamount
    }
    var url = 'SmrQuotation360CRM/UpdateQuotationProductCRM'
    this.NgxSpinnerService.show()
    this.service.post(url, params).pipe().subscribe((result: any) => {
      this.responsedata = result;
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.Quotationproductsummary();
      }
      else {
        this.ToastrService.success(result.message)
        this.productform.reset();
        this.NgxSpinnerService.hide()

      }
      this.Quotationproductsummary();
    });
    this.showUpdateButton = false;
    this.showAddButton = true;
  }

}



