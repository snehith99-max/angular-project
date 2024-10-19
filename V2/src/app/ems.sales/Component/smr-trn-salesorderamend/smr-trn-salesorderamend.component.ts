import { Component, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { HttpClient } from '@angular/common/http';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { NgxSpinnerService } from 'ngx-spinner';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
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
  tax_amount: Number;
  tax_name2: string;
  tax_amount2: string;
  tax_name3: string;
  tax_amount3: string;
  price: string;
}
@Component({
  selector: 'app-smr-trn-salesorderamend',
  templateUrl: './smr-trn-salesorderamend.component.html',
  styleUrls: ['./smr-trn-salesorderamend.component.scss']
})
export class SmrTrnSalesorderamendComponent {
  branch_name1: any;
  customer_mobile1: any;
  customer_name1: any;
  salesorder_gid: any;
  customercontact_names1: any;
  editorderList: any[] = [];
  config: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '38rem',
    minHeight: '5rem',
    width: '880px',
    placeholder: 'Enter text here...',
    translate: 'no'

  };
  showUpdateButton: boolean = false;
  showAddButton: boolean = true;
  combinedFormData: FormGroup | any;
  productform: FormGroup | any;
  customer_list: any;
  currency_code1: any;
  SOAmendsummary_List: any[] = [];
  contact_list: any[] = [];
  branch_list: any[] = [];
  currency_list: any[] = [];
  currency_list1: any[] = [];
  user_list: any[] = [];
  product_list: any[] = [];
  tax_list: any[] = [];
  tax2_list: any[] = [];
  CurrencyName: any;
  tax3_list: any[] = [];
  tax4_list: any[] = [];
  calci_list: any[] = [];
  POproductlist: any[] = [];
  terms_list: any[] = [];
  mdlBranchName: any;
  GetCustomerDet: any;
  mdlTaxName4: any;
  mdlCustomerName: any;
  mdlUserName: any;
  mdlProductName: any;
  mdlCurrencyName: any;
  salesorder_date: any;
  mdlTaxName: any;
  GetproductsCode: any;
  mdlContactName: any;
  packing_charges: any;
  unitprice: number = 0;
  quantity: number = 0;
  discountpercentage: number = 0;
  discountamount: any;
  tax_amount4: any;
  totalamount: number = 0;
  addon_charge: number = 0;
  POdiscountamount: number = 0;
  freight_charges: number = 0;
  forwardingCharges: number = 0;
  insurance_charges: number = 0;
  roundoff: number = 0;
  grandtotal: any;
  grand_total: any;
  tax_amount: number = 0;
  buyback_charges: number = 0;
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
  allchargeslist: any[] = [];
  additional_discount: number = 0;
  mdlproductName: any;
  responsedata: any;
  ExchangeRate: any;
  salesOD!: SalesOD;
  Productsummarys_list: any;
  salesorders_list: any;
  cuscontact_gid: any;
  mdlProductCode: any;
  mdlProductUom: any;
  mdlCurrencyRate: any;
  constructor(private http: HttpClient, private fb: FormBuilder, private router: ActivatedRoute,
    public NgxSpinnerService: NgxSpinnerService, private route: Router, private service: SocketService, private ToastrService: ToastrService) {
    this.salesOD = {} as SalesOD

    this.productform = new FormGroup({
      unit: new FormControl(''),
      tmpsalesorderdtl_gid: new FormControl(''),
      tax_gid: new FormControl(''),
      product_gid: new FormControl(''),
      productuom_gid: new FormControl(''),
      productgroup_gid: new FormControl(''),
      product_code: new FormControl(''),
      productcode: new FormControl('', Validators.required),
      productgroup: new FormControl('', Validators.required),
      productuom: new FormControl('', Validators.required),
      productname: new FormControl('', Validators.required),
      tax_name: new FormControl('', Validators.required),
      tax_name2: new FormControl('', Validators.required),
      tax_name3: new FormControl('', Validators.required),
      remarks: new FormControl('', Validators.required),
      product_name: new FormControl('', Validators.required),
      productuom_name: new FormControl('', Validators.required),
      productgroup_name: new FormControl('', Validators.required),
      unitprice: new FormControl('', Validators.required),
      quantity: new FormControl('', Validators.required),
      discountpercentage: new FormControl('', Validators.required),
      discountamount: new FormControl('', Validators.required),
      taxname1: new FormControl('', Validators.required),
      tax_amount: new FormControl('', Validators.required),
      taxname2: new FormControl('', Validators.required),
      tax_amount2: new FormControl('', Validators.required),
      taxname3: new FormControl('', Validators.required),
      tax_amount3: new FormControl('', Validators.required),
      totalamount: new FormControl('', Validators.required),
      customerproduct_code: new FormControl(''),
      selling_price: new FormControl(''),
      product_requireddate: new FormControl(''),
      product_requireddateremarks: new FormControl(''),
    });

  }
  ngOnInit(): void {

    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);

    this.combinedFormData = new FormGroup({
      tmpsalesorderdtl_gid: new FormControl(''),
      salesorder_gid: new FormControl(''),
      salesorder_date: new FormControl(''),
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
      tax_amount4: new FormControl(''),
      total_amount: new FormControl(''),

    });
    this.productform = new FormGroup({
      unit: new FormControl(''),
      tmpsalesorderdtl_gid: new FormControl(''),
      tax_gid: new FormControl(''),
      product_gid: new FormControl(''),
      productuom_gid: new FormControl(''),
      productgroup_gid: new FormControl(''),
      product_code: new FormControl(''),
      productcode: new FormControl('', Validators.required),
      productgroup: new FormControl('', Validators.required),
      productuom: new FormControl('', Validators.required),
      productname: new FormControl('', Validators.required),
      tax_name: new FormControl('', Validators.required),
      tax_name2: new FormControl('', Validators.required),
      tax_name3: new FormControl('', Validators.required),
      remarks: new FormControl('', Validators.required),
      product_name: new FormControl('', Validators.required),
      productuom_name: new FormControl('', Validators.required),
      productgroup_name: new FormControl('', Validators.required),
      unitprice: new FormControl('', Validators.required),
      quantity: new FormControl('', Validators.required),
      discountpercentage: new FormControl('', Validators.required),
      discountamount: new FormControl('', Validators.required),
      taxname1: new FormControl('', Validators.required),
      tax_amount: new FormControl('', Validators.required),
      taxname2: new FormControl('', Validators.required),
      tax_amount2: new FormControl('', Validators.required),
      taxname3: new FormControl('', Validators.required),
      tax_amount3: new FormControl('', Validators.required),
      totalamount: new FormControl('', Validators.required),
      customerproduct_code: new FormControl(''),
      selling_price: new FormControl(''),
      product_requireddate: new FormControl(''),
      product_requireddateremarks: new FormControl(''),
    });




    const salesorder_gid = this.router.snapshot.paramMap.get('salesorder_gid');

    this.salesorder_gid = salesorder_gid;

    const secretKey = 'storyboarderp';

    const deencryptedParam = AES.decrypt(this.salesorder_gid, secretKey).toString(enc.Utf8);
    this.salesorder_gid = deencryptedParam;





    //// Sales person Dropdown ////
    var url = 'SmrTrnSalesorder/GetPersonDtl'
    this.service.get(url).subscribe((result: any) => {
      this.contact_list = result.GetPersonDtl;
    });

    //// Currency Dropdown ////
    var url = 'SmrTrnSalesorder/GetCurrencyDtl'
    this.service.get(url).subscribe((result: any) => {
      this.currency_list = result.GetCurrencyDtl;
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
    this.GetAmendSummary(deencryptedParam);

    var api = 'SmrMstSalesConfig/GetAllChargesConfig';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.allchargeslist = this.responsedata.salesconfigalllist;
      this.addon_charge = this.allchargeslist[0].flag;
      this.additional_discount = this.allchargeslist[1].flag;
      this.freight_charges = this.allchargeslist[2].flag;
      this.buyback_charges = this.allchargeslist[3].flag;
      this.insurance_charges = this.allchargeslist[4].flag;
      this.roundoff = this.allchargeslist[5].flag;

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
  GetTemporarysummary() {
    debugger
    var url = 'SmrSalesOrderAmend/GetSOProductAmendSummary'
    let param = {
      salesorder_gid: this.salesorder_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.salesorders_list = result.amendtemp_list;

      this.combinedFormData.get("producttotalamount")?.setValue((result.grand_total).toFixed(2))
      if (this.mdlTaxName4 == null || this.mdlTaxName4 == "" || this.mdlTaxName4 == "--No Tax--") {
        this.combinedFormData.get("Grandtotal")?.setValue(result.grand_total);
      }
      else {
        let selectedTax = this.tax4_list.find(tax => tax.tax_name4 === this.mdlTaxName4);
        this.taxpercentage = this.getDimensionsByFilter(selectedTax.tax_gid);
        let tax_percentage = this.taxpercentage[0].percentage;
        this.tax_amount4 = (+(tax_percentage * result.grand_total / 100));
        const taxamount = (+(result.grand_total + this.tax_amount4));
        const newGrandTotal = result.grand_total + parseFloat(this.tax_amount4);
        const newGrandTotal2 = newGrandTotal + parseFloat(this.combinedFormData.value.addon_charge);
        const newGrandTotal3 = newGrandTotal2 - parseFloat(this.combinedFormData.value.additional_discount);
        const newGrandTotal4 = newGrandTotal3 + parseFloat(this.combinedFormData.value.freight_charges);
        const newGrandTotal5 = newGrandTotal4 - parseFloat(this.combinedFormData.value.buyback_charges);
        const newGrandTotal6 = newGrandTotal5 + parseFloat(this.combinedFormData.value.packing_charges);
        const newGrandTotal7 = newGrandTotal6 + parseFloat(this.combinedFormData.value.insurance_charges);
        const newGrandTotal8 = newGrandTotal7 + parseFloat(this.combinedFormData.value.roundoff);
        this.combinedFormData.get("tax_amount4")?.setValue((this.tax_amount4).toFixed(2));
        this.combinedFormData.get("total_amount")?.setValue((taxamount).toFixed(2));
        this.combinedFormData.get("grandtotal")?.setValue((newGrandTotal8).toFixed(2));
      }

    });

  }
  GetAmendSummary(salesorder_gid: any) {

    var url = 'SmrSalesOrderAmend/GetAmendSalesOrderDtl'
    let params = {
      salesorder_gid: this.salesorder_gid
    }

    this.service.getparams(url, params).subscribe((result: any) => {

      this.responsedata = result;
      this.SOAmendsummary_List = result.SOAmendsummary_List;
      this.combinedFormData.get("salesorder_gid")?.setValue(this.SOAmendsummary_List[0].salesorder_gid);
      this.combinedFormData.get("salesorder_date")?.setValue(this.SOAmendsummary_List[0].salesorder_date);
      this.combinedFormData.get("branch_name")?.setValue(this.SOAmendsummary_List[0].branch_name);
      this.combinedFormData.get("so_referencenumber")?.setValue(this.SOAmendsummary_List[0].so_referencenumber);
      this.combinedFormData.get("customer_name")?.setValue(this.SOAmendsummary_List[0].customer_name);
      this.combinedFormData.get("customercontact_names")?.setValue(this.SOAmendsummary_List[0].customer_contact_person);
      this.combinedFormData.get("customer_address")?.setValue(this.SOAmendsummary_List[0].customer_address);
      this.combinedFormData.get("freight_terms")?.setValue(this.SOAmendsummary_List[0].freight_terms);
      this.combinedFormData.get("payment_terms")?.setValue(this.SOAmendsummary_List[0].payment_terms);
      this.combinedFormData.get("exchange_rate")?.setValue(this.SOAmendsummary_List[0].exchange_rate);
      this.combinedFormData.get("shipping_to")?.setValue(this.SOAmendsummary_List[0].shipping_to);
      this.combinedFormData.get("payment_days")?.setValue(this.SOAmendsummary_List[0].payment_days);
      this.combinedFormData.get("delivery_days")?.setValue(this.SOAmendsummary_List[0].delivery_days);
      this.combinedFormData.get("customer_mobile")?.setValue(this.SOAmendsummary_List[0].customer_mobile);
      this.combinedFormData.get("customer_email")?.setValue(this.SOAmendsummary_List[0].customer_email);
      this.combinedFormData.get("so_remarks")?.setValue(this.SOAmendsummary_List[0].so_remarks);
      this.combinedFormData.get("customer_gid")?.setValue(this.SOAmendsummary_List[0].customer_gid);
      this.additional_discount = this.SOAmendsummary_List[0].additional_discount;
      this.mdlUserName = this.SOAmendsummary_List[0].salesperson_gid;
      this.mdlCurrencyName = this.SOAmendsummary_List[0].currency_gid;
      this.combinedFormData.get("currency_gid")?.setValue(this.SOAmendsummary_List[0].currency_gid);
      this.combinedFormData.get("termsandconditions")?.setValue(this.SOAmendsummary_List[0].termsandconditions);
      this.combinedFormData.get("start_date")?.setValue(this.SOAmendsummary_List[0].start_date);
      this.combinedFormData.get("end_date")?.setValue(this.SOAmendsummary_List[0].end_date);
      this.combinedFormData.get("tax_name4")?.setValue(this.SOAmendsummary_List[0].tax_name4);
      this.currency_code1 = this.SOAmendsummary_List[0].currency_code;
      this.roundoff = this.SOAmendsummary_List[0].roundoff;
      this.total_amount = this.SOAmendsummary_List[0].total_amount;
      this.freight_charges = this.SOAmendsummary_List[0].freight_charges;
      this.buyback_charges = this.SOAmendsummary_List[0].buyback_charges;
      this.packing_charges = this.SOAmendsummary_List[0].packing_charges;
      this.insurance_charges = this.SOAmendsummary_List[0].insurance_charges;
      this.tax_amount4 = this.SOAmendsummary_List[0].tax_amount;
      this.addon_charge = this.SOAmendsummary_List[0].addon_charge;


      this.GetTemporarysummary();
    });

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
  get tax_name() {
    return this.productform.get('tax_name')!;
  }
  get tax_name2() {
    return this.productform.get('tax_name2')!;
  }
  get tax_name3() {
    return this.productform.get('tax_name3')!;
  }


  OnChangeCurrency() {

    let currencyexchange_gid = this.combinedFormData.get("currency_code")?.value;

    let param = {
      currencyexchange_gid: currencyexchange_gid
    }
    var url = 'SmrTrnSalesorder/GetOnChangeCurrency';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.currency_list1 = this.responsedata.GetOnchangeCurrency;
      this.combinedFormData.get("exchange_rate")?.setValue(this.currency_list1[0].exchange_rate);
      this.currency_code1 = this.currency_list1[0].currency_code
    });
  }

  onCurrencyCodeChange(event: Event) {

  }

  GetOnChangeProductsNameAmend() {
    let product_gid = this.productform.value.product_name.product_gid;
    let param = {
      product_gid: product_gid
    }
    var url = 'SmrTrnSalesorder/GetOnChangeProductsNameAmend';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.GetproductsCode = this.responsedata.ProductsCodes;
      this.productform.get("product_code")?.setValue(result.ProductsCodes[0].product_code);
      this.productform.get("unit")?.setValue(result.ProductsCodes[0].productuom_name);
      this.productform.get("productgroup_name")?.setValue(result.ProductsCodes[0].productgroup_name);
      this.productform.get("unitprice")?.setValue(result.ProductsCodes[0].unitprice);
      this.productform.value.productgroup_gid = result.ProductsCodes[0].productgroup_gid

    });


  }

  productAdd() {

    var params = {
      salesorder_gid: this.salesorder_gid,
      product_code: this.productform.value.product_code,
      product_name: this.productform.value.product_name.product_name,
      productuom_name: this.productform.value.unit,
      quantity: this.productform.value.quantity,
      product_gid: this.productform.value.product_name.product_gid,
      unitprice: this.productform.value.unitprice,
      discountpercentage: this.productform.value.discountpercentage,
      discountamount: this.productform.value.discountamount,
      tax_name: this.productform.value.tax_name,
      tax_gid: this.productform.value.tax_gid,
      tax_amount: this.productform.value.tax_amount,
      totalamount: this.productform.value.totalamount,
    }

    var api = 'SmrSalesOrderAmend/PostSOAmendProduct';
    this.NgxSpinnerService.show();
    this.service.postparams(api, params).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      this.productform.reset();
      this.GetTemporarysummary()
    });
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

  overallsubmit() {
    var params = {
      salesorder_gid: this.combinedFormData.value.salesorder_gid,
      salesorder_date: this.combinedFormData.value.salesorder_date,
      customer_branch: this.combinedFormData.value.branch_name,
      branch_gid: this.combinedFormData.value.branch_gid,
      so_referencenumber: this.combinedFormData.value.so_referencenumber,
      customer_name: this.combinedFormData.value.customer_name,
      customer_contact_person: this.combinedFormData.value.customercontact_names,
      customer_email: this.combinedFormData.value.customer_email,
      customer_mobile: this.combinedFormData.value.customer_mobile,
      so_remarks: this.combinedFormData.value.so_remarks,
      customer_address: this.combinedFormData.value.customer_address,
      freight_terms: this.combinedFormData.value.freight_terms,
      payment_terms: this.combinedFormData.value.payment_terms,
      currency_code: this.combinedFormData.value.currency_code,
      user_name: this.combinedFormData.value.user_name,
      exchange_rate: this.combinedFormData.value.exchange_rate,
      payment_days: this.combinedFormData.value.payment_days,
      customer_gid: this.combinedFormData.value.customer_gid,
      termsandconditions: this.combinedFormData.value.termsandconditions,
      template_name: this.combinedFormData.value.template_name,
      template_gid: this.combinedFormData.value.template_gid,
      Grandtotal: this.combinedFormData.value.grandtotal,
      roundoff: this.combinedFormData.value.roundoff,
      start_date: this.combinedFormData.value.start_date,
      end_date: this.combinedFormData.value.end_date,
      tax_name4: this.combinedFormData.value.tax_name4,
      insurance_charges: this.combinedFormData.value.insurance_charges,
      packing_charges: this.combinedFormData.value.packing_charges,
      buyback_charges: this.combinedFormData.value.buyback_charges,
      freight_charges: this.combinedFormData.value.freight_charges,
      additional_discount: this.combinedFormData.value.additional_discount,
      addon_charge: this.combinedFormData.value.addon_charge,
      total_amount: this.combinedFormData.value.total_amount,
      producttotalamount: this.combinedFormData.value.producttotalamount,
      customercontact_gid: this.cuscontact_gid,
      tax_amount4: this.combinedFormData.value.tax_amount4,
      delivery_days: this.combinedFormData.value.delivery_days,
      shipping_to: this.combinedFormData.value.shipping_to,
    }
    var url = 'SmrSalesOrderAmend/AmendSalesOrder'
    this.NgxSpinnerService.show();
    this.service.postparams(url, params).subscribe((result: any) => {

      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide();
      }
      else {
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
        this.route.navigate(['/smr/SmrTrnSalesorderSummary']);
      }
    });
  }
  getDimensionsByFilter(id: any) {
    return this.tax_list.filter((x: { tax_gid: any; }) => x.tax_gid === id);
  }

  prodtotalcal() {
    debugger
    if (this.quantity == 0 || this.quantity == null) {
      this.totalamount = 0;
      this.discountpercentage == 0;
      this.discountamount == 0;
      this.tax_amount == 0;
      this.OnClearTax();

    }
    else {
      let subtotal = this.mdlCurrencyRate * Number(this.unitprice) * Number(this.quantity);
      this.discountamount = (subtotal * this.discountpercentage) / 100 || 0;
      this.discountamount = (this.discountamount * 100) / 100;
      this.discountamount = +((+this.discountamount)).toFixed(2);
      const tax_amount = Number(this.tax_amount || 0);
      subtotal = Number(subtotal) + tax_amount
      this.totalamount = (+(subtotal - this.discountamount));
      this.totalamount = +((+this.totalamount)).toFixed(2);
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
      const subtotal = this.mdlCurrencyRate * this.unitprice * this.quantity;
      this.discountamount = (subtotal * this.discountpercentage) / 100;
      this.discountamount = (this.discountamount * 100) / 100;
      this.totalamount = (+(subtotal - this.discountamount + this.tax_amount));

      this.total_amount = +((this.total_amount).toFixed(2));

    }
    else {
      const subtotal = parseFloat((this.mdlCurrencyRate * this.unitprice * this.quantity).toFixed(2));

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
        this.totalamount = (this.totalamount * 10) / 10
      }
    }

  }
  finaltotal() {
    if (this.total_amount == null || this.total_amount == "" || this.total_amount == 0 || this.total_amount == "NaN") {
      this.grandtotal = ((this.producttotalamount) + (+this.addon_charge) + (+this.freight_charges) - (+this.buyback_charges) + (+this.packing_charges) + (+this.insurance_charges) + (+this.roundoff) - (+this.additional_discount));
      this.grandtotal = (+this.grandtotal);
    }
    else {
      const total_amount = parseFloat(this.total_amount)
      const totalAmount = isNaN((total_amount)) ? 0 : (total_amount);
      //const product = isNaN((this.producttotalamount)) ? 0 : (this.total_amount.replace(/,/g, ''));


      const addoncharges = isNaN((this.combinedFormData.value.addon_charge)) ? 0 : (this.combinedFormData.value.addon_charge);
      const discount = isNaN((this.combinedFormData.value.additional_discount)) ? 0 : (this.combinedFormData.value.additional_discount);
      const forwardingCharges = isNaN((this.combinedFormData.value.packing_charges)) ? 0 : (this.combinedFormData.value.packing_charges);
      const insuranceCharges = isNaN((this.combinedFormData.value.insurance_charges)) ? 0 : (this.combinedFormData.value.insurance_charges);
      const buybackCharges = isNaN((this.combinedFormData.value.buyback_charges)) ? 0 : (this.combinedFormData.value.buyback_charges);
      const roundoff = isNaN((this.combinedFormData.value.roundoff)) ? 0 : (this.combinedFormData.value.roundoff);
      const freight = isNaN((this.combinedFormData.value.freight_charges)) ? 0 : (this.combinedFormData.value.freight_charges);

      // Perform the calculation

      this.grandtotal = ((totalAmount) + (+addoncharges) + (+freight) + (+forwardingCharges) + (+insuranceCharges) + (+roundoff) - (+discount) - (+buybackCharges));
    }

  }
  oncleartotal() {
    debugger
    if (this.combinedFormData.value.addon_charge == "" || this.combinedFormData.value.addon_charge == 0 || this.combinedFormData.value.addon_charge == null || this.combinedFormData.value.addon_charge == "NaN") {
      this.grandtotal -= parseFloat(this.combinedFormData.value.addon_charge)
    }

    else if (this.combinedFormData.value.additional_discount == "" || this.combinedFormData.value.additional_discount == 0 || this.combinedFormData.value.additional_discount == null || this.combinedFormData.value.additional_discount == "NaN") {
      this.grandtotal += parseFloat(this.combinedFormData.value.additional_discount)
    }

    else if (this.combinedFormData.value.packing_charges == "" || this.combinedFormData.value.packing_charges == 0 || this.combinedFormData.value.packing_charges == null || this.combinedFormData.value.packing_charges == "NaN") {
      this.grandtotal -= parseFloat(this.combinedFormData.value.packing_charges)
    }
    else if (this.combinedFormData.value.insurance_charges == "" || this.combinedFormData.value.insurance_charges == 0 || this.combinedFormData.value.insurance_charges == null || this.combinedFormData.value.insurance_charges == "NaN") {
      this.grandtotal -= parseFloat(this.combinedFormData.value.insurance_charges)
    }
    else if (this.combinedFormData.value.buyback_charges == "" || this.combinedFormData.value.buyback_charges == 0 || this.combinedFormData.value.insurance_charges == null || this.combinedFormData.value.buyback_charges == "NaN") {
      this.grandtotal += parseFloat(this.combinedFormData.value.buyback_charges)
    }
    else if (this.combinedFormData.value.freight_charges == "" || this.combinedFormData.value.freight_charges == 0 || this.combinedFormData.value.freight_charges == null || this.combinedFormData.value.freight_charges == "NaN") {
      this.grandtotal -= parseFloat(this.combinedFormData.value.freight_charges)
    }
    else if (this.combinedFormData.value.roundoff == "" || this.combinedFormData.value.roundoff == 0 || this.combinedFormData.value.roundoff == null || this.combinedFormData.value.roundoff == "NaN") {
      this.grandtotal -= parseFloat(this.combinedFormData.value.roundoff)
    }
  }
  onKeyPress(event: any) {
    // Get the pressed key
    const key = event.key;

    if (!/^[0-9.]$/.test(key)) {
      // If not a number or dot, prevent the default action (key input)
      event.preventDefault();
    }
  }

  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }

  ondelete() {
    var url = 'SmrTrnSalesorder/GetDeleteDirectSOProductSummary'
    let param = {
      tmpsalesorderdtl_gid: this.parameterValue
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
      }
      else {

        this.ToastrService.success(result.message)
        this.GetTemporarysummary()
      }
    });
  }

  close() {
    this.route.navigate(['/smr/SmrTrnSalesorderSummary']);
  }
  edit(tmpsalesorderdtl_gid: any) {
    var url = 'SmrSalesOrderAmend/GetProductEditSummary'
    let param = {
      tmpsalesorderdtl_gid: tmpsalesorderdtl_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.editorderList = result.editorderList;
      this.productform.get("tmpsalesorderdtl_gid")?.setValue(this.editorderList[0].tmpsalesorderdtl_gid);
      this.productform.get("product_name")?.setValue(this.editorderList[0].product_name);
      this.productform.get("product_gid")?.setValue(this.editorderList[0].product_gid);
      this.productform.get("product_code")?.setValue(this.editorderList[0].product_code);
      this.productform.get("unit")?.setValue(this.editorderList[0].unit);
      this.productform.get("quantity")?.setValue(this.editorderList[0].quantity);
      this.productform.get("totalamount")?.setValue(this.editorderList[0].totalamount);
      this.productform.get("tax_name")?.setValue(this.editorderList[0].tax_name);
      this.productform.get("tax_gid")?.setValue(this.editorderList[0].tax_gid);
      this.productform.get("unitprice")?.setValue(this.editorderList[0].unitprice);
      this.productform.get("tax_amount")?.setValue(this.editorderList[0].tax_amount);
      this.productform.get("discountpercentage")?.setValue(this.editorderList[0].discountpercentage);
      this.productform.get("discountamount")?.setValue(this.editorderList[0].discountamount);
    });
    this.showUpdateButton = true;
    this.showAddButton = false;
  }
  productUpdate() {
    var params = {
      tmpsalesorderdtl_gid: this.productform.value.tmpsalesorderdtl_gid,
      tax_name: this.productform.value.tax_name == undefined ? this.productform.value.tax_name : this.productform.value.tax_name,
      tax_gid: this.productform.value.tax_gid,
      tax_amount: this.productform.value.tax_amount,
      total_amount: this.productform.value.totalamount,
      productgroup_name: this.productform.value.productgroup_name,
      discountamount: this.productform.value.discountamount,
      discountpercentage: this.productform.value.discountpercentage,
      quantity: this.productform.value.quantity,
      unitprice: this.productform.value.unitprice,
      unit: this.productform.value.unit,
      product_name: this.productform.value.product_name.product_name == undefined ? this.productform.value.product_name : this.productform.value.product_name.product_name,
      product_code: this.productform.value.product_code
    }

    var api = 'SmrSalesOrderAmend/updateSalesOrderedit'
    this.NgxSpinnerService.show();

    this.service.postparams(api, params).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      this.GetTemporarysummary()
      this.productform.reset();
    });
    this.showAddButton = true;
    this.showUpdateButton = false;
  }

  OnChangeTaxAmount4() {
    debugger
    let selectedTax = this.tax4_list.find(tax => tax.tax_name4 === this.mdlTaxName4);
    this.taxpercentage = this.getDimensionsByFilter(selectedTax.tax_gid);

    const producttotalamount = parseFloat(this.producttotalamount.replace(/,/g, ''));
    let tax_percentage = this.taxpercentage[0].percentage;

    this.tax_amount4 = (+(tax_percentage * this.producttotalamount / 100));
    this.total_amount = (+(producttotalamount + this.tax_amount4));
    this.total_amount = parseFloat(this.total_amount.toFixed(2));

    const addoncharges = isNaN((this.combinedFormData.value.addon_charge)) ? 0 : (this.combinedFormData.value.addon_charge);
    const discount = isNaN((this.combinedFormData.value.additional_discount)) ? 0 : (this.combinedFormData.value.additional_discount);
    const forwardingCharges = isNaN((this.combinedFormData.value.packing_charges)) ? 0 : (this.combinedFormData.value.packing_charges);
    const insuranceCharges = isNaN((this.combinedFormData.value.insurance_charges)) ? 0 : (this.combinedFormData.value.insurance_charges);
    const buybackCharges = isNaN((this.combinedFormData.value.buyback_charges)) ? 0 : (this.combinedFormData.value.buyback_charges);
    const roundoff = isNaN((this.combinedFormData.value.roundoff)) ? 0 : (this.combinedFormData.value.roundoff);
    const freight = isNaN((this.combinedFormData.value.freight_charges)) ? 0 : (this.combinedFormData.value.freight_charges);
    this.grandtotal = ((this.total_amount) + (+addoncharges) + (+freight) - (+buybackCharges) + (+insuranceCharges) + (+forwardingCharges) + (+roundoff) - (+discount));

  }

  OnClearOverallTax() {

    this.tax_amount4 = '';
    this.total_amount = '';
    this.grandtotal = ((this.producttotalamount) + (+this.addon_charge) + (+this.freight_charges) - (+this.buyback_charges) + (+this.packing_charges) + (+this.insurance_charges) + (+this.roundoff) - (+this.additional_discount));
    this.grandtotal = (+this.grandtotal);

  }

  OnClearTax() {

    this.tax_amount = 0;
    this.totalamount = (+(this.totalamount) - (+this.tax_amount));
    this.totalamount = +((+this.totalamount)).toFixed(2)
  }
  onClearProduct() {
    this.mdlProductCode = '';
    this.mdlProductUom = '';
    this.totalamount = 0;
    this.tax_amount = 0;
    this.mdlTaxName = '';
    this.discountamount = 0;
    this.discountpercentage = 0;
    this.quantity = 0;
    this.unitprice = 0;
  }
  onClearCurrency() {
    this.mdlCurrencyRate = ''
  }
}

