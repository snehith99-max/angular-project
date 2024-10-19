import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';

@Component({
  selector: 'app-rbl-trn-invoiceedit-boba',
  templateUrl: './rbl-trn-invoiceedit-boba.component.html',
  styleUrls: ['./rbl-trn-invoiceedit-boba.component.scss']
})
export class RblTrnInvoiceeditBobaComponent {

  invoiceform: FormGroup | any;
  invoiceproductlist: any;
  customer_gid: any;
  customernamelist: any;
  currency_list: any;

  productform: FormGroup | any;
  product_list: any;
  productgroup_list: any;
  productnamelist: any;
  productcodelist: any;
  productunitlist: any;

  unitprice: number = 0;
  quantity: number = 0;
  discountpercentage: number = 0;
  discountamount: number = 0;
  tax_list: any;
  currencylist: any;

  totalamount: number = 0;
  addoncharges: number = 0;
  invoicediscountamount: number = 0;
  frieghtcharges: number = 0;
  forwardingCharges: number = 0;
  insurancecharges: number = 0;
  roundoff: number = 0;
  grandtotal: number = 0;
  responsedata: any;
  customerdetails_list: any;
  productdetails_list: any;
  product_price: any;
  taxpercentage: any;
  taxamount1: number = 0;
  taxpercentage1: any;
  taxpercentage2: any;
  taxamount2: number = 0;
  producttotalamount: any;
  parameterValue: string | undefined;
  invoice_gid: any;
  editinvoiceproductsummarylist: any;
  invoice: any;
  mdlTerms: any;
  terms_list: any[] = [];
  templatecontent_list: any;
  currency_name: any;

  mdlCustomer: any;
  mdlCurrency: any;
  mdlProduct: any;
  mdlTax: any;


  config: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '12rem',
    minHeight: '0rem',
    placeholder: 'Enter text here...',
    translate: 'no',
    defaultParagraphSeparator: 'p',
    defaultFontName: 'Arial',
  };

  ngOnInit(): void {

    const options: Options = {
      dateFormat: 'd-m-Y',    
    };
    
    flatpickr('.date-picker', options);

    const invoice_gid = this.router.snapshot.paramMap.get('invoice_gid');
    this.invoice_gid = invoice_gid;

    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.invoice_gid, secretKey).toString(enc.Utf8);

    var api = 'Einvoice/GetProductNamedropDown';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.productnamelist = this.responsedata.Getproductnamedropdown;
    });

    var api = 'Einvoice/GetcurrencyCodedropdown';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.currency_list = this.responsedata.Getcurrencycodedropdown;
    });

    var api = 'Einvoice/GetCustomerNamedropDown';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.customernamelist = this.responsedata.GetCustomernamedropdown;
    });

    var api = 'Einvoice/Gettaxnamedropdown';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.tax_list = this.responsedata.Gettaxnamedropdown;
    });

    var url = 'Einvoice/GetTermsandConditions'
    this.service.get(url).subscribe((result: any) => {
      this.terms_list = result.terms_lists;
    });

    this.editinvoiceproductsummary()
    this.GetEditInvoice()
  }

  editinvoiceproductsummary() {
    const invoice_gid = this.router.snapshot.paramMap.get('invoice_gid');
    this.invoice_gid = invoice_gid;

    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.invoice_gid, secretKey).toString(enc.Utf8);

    let param = {
      invoice_gid: deencryptedParam
    }
    var api = 'Einvoice/GetEditInvoiceProductSummary';
    this.service.getparams(api, param).subscribe((result: any) => {
      this.responsedata = result;
      this.invoiceproductlist = this.responsedata.editinvoiceproductsummarylist;
      this.invoiceform.get("producttotalamount")?.setValue(this.responsedata.grand_total);
    });
  }

  constructor(private fb: FormBuilder, private router: ActivatedRoute, private route: Router, private service: SocketService, private ToastrService: ToastrService) {
    this.invoiceform = new FormGroup({
      customergid: new FormControl({ value: '', disabled: false },[Validators.required]),
      invoice_gid: new FormControl(''),

      customer_name: new FormControl(''),
      customer_branch: new FormControl(''),
      customercontactperson: new FormControl(''),
      customercontactnumber: new FormControl('',[Validators.pattern(/^[0-9]+$/), Validators.maxLength(10)]),
      customeremailaddress: new FormControl('', [Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$')]),
      customeraddress: new FormControl(''),

      branch: new FormControl('', Validators.required),
      gst: new FormControl(''),
      currencygid: new FormControl(null, Validators.required),
      exchangerate: new FormControl(''),
      salestype: new FormControl('', Validators.required),
      mode_of_dispatch: new FormControl(''),
      remarks: new FormControl(''),

      invoiceref_no: new FormControl('',),
      invoicedate: new FormControl('', Validators.required),
      paymentterm: new FormControl('', Validators.required),
      duedate: new FormControl('', Validators.required),
      deliverydate: new FormControl('', Validators.required),

      payment: new FormControl(''),
      deliveryperiod: new FormControl(''),
      producttotalamount: new FormControl(''),
      addoncharges: new FormControl(''),
      invoicediscountamount: new FormControl(''),
      frieghtcharges: new FormControl(''),
      forwardingCharges: new FormControl(''),
      insurancecharges: new FormControl(''),
      roundoff: new FormControl(''),
      grandtotal: new FormControl(''),
      termsandconditions: new FormControl(''),
      template_gid: new FormControl(''),
      template_name: new FormControl(''),
    })

    this.productform = new FormGroup({
      productgid: new FormControl(''),
      productgroup_gid: new FormControl(''),
      productcode: new FormControl(''),
      hsncode: new FormControl(''),
      hsndescription: new FormControl(''),
      productuom_gid: new FormControl(''),
      unitprice: new FormControl(''),
      mrp: new FormControl(''),
      quantity: new FormControl(''),
      discountpercentage: new FormControl(''),
      discountamount: new FormControl(''),
      taxname1: new FormControl(''),
      taxamount1: new FormControl(''),
      taxname2: new FormControl(''),
      taxamount2: new FormControl(''),
      totalamount: new FormControl(''),
    })
  }

  GetEditInvoice() {
    const invoice_gid = this.router.snapshot.paramMap.get('invoice_gid');
    this.invoice_gid = invoice_gid;

    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.invoice_gid, secretKey).toString(enc.Utf8);
    
    let param = {
      invoice_gid: deencryptedParam
    }

    var api = 'Einvoice/GetEditInvoice';

    console.log(param);
    this.service.getparams(api, param).subscribe((result: any) => {
      this.responsedata=result;
      this.invoice = result;

      // const invoicedate = new Date(this.invoice.invoicedate);
      // const timezoneOffset = invoicedate.getTimezoneOffset() * 60000;
      // const adjustedDate = new Date(invoicedate.getTime() - timezoneOffset);
      // const formattedDate = adjustedDate.toISOString().substring(0, 10);

      // const duedate = new Date(this.invoice.duedate);      
      // const timezoneOffset1 = duedate.getTimezoneOffset() * 60000;
      // const adjustedDate1 = new Date(duedate.getTime() - timezoneOffset1);
      // const formattedDate1 = adjustedDate1.toISOString().substring(0, 10);

      const invoicedate = new Date(this.invoice.invoicedate);
      const timezoneOffset = invoicedate.getTimezoneOffset() * 60000;
      const adjustedDate = new Date(invoicedate.getTime() - timezoneOffset);
      
      function formatDate(date: Date) {
        const day = String(date.getDate()).padStart(2, '0');
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const year = date.getFullYear();
        return `${day}-${month}-${year}`;
      }
      const formattedDate = formatDate(adjustedDate);

      const duedate = new Date(this.invoice.duedate);
      const timezoneOffset1 = duedate.getTimezoneOffset() * 60000;
      const adjustedDate1 = new Date(duedate.getTime() - timezoneOffset1);
      
      function formatDate1(date: Date) {
        const day = String(date.getDate()).padStart(2, '0');
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const year = date.getFullYear();
        return `${day}-${month}-${year}`;
      }
      const formattedDate1 = formatDate1(adjustedDate1);
      

      this.invoiceform.get("invoice_gid")?.setValue(this.invoice.invoice_gid);
      this.invoiceform.get("customer_name")?.setValue(this.invoice.customername);
      this.invoiceform.get("customercontactperson")?.setValue(this.invoice.customercontactperson);
      this.invoiceform.get("customercontactnumber")?.setValue(this.invoice.customercontactnumber);
      this.invoiceform.get("customeremailaddress")?.setValue(this.invoice.customeremailaddress);
      this.invoiceform.get("customeraddress")?.setValue(this.invoice.customeraddress);
      this.invoiceform.get("branch")?.setValue(this.invoice.branch);
      this.invoiceform.get("currencygid")?.setValue(this.invoice.currencygid);
      this.invoiceform.get("exchangerate")?.setValue(this.invoice.exchangerate);
      this.invoiceform.get("remarks")?.setValue(this.invoice.remarks);
      this.invoiceform.get("invoiceref_no")?.setValue(this.invoice.invoiceref_no);
      this.invoiceform.get("invoicedate")?.setValue(formattedDate);
      this.invoiceform.get("paymentterm")?.setValue(this.invoice.paymentterm);
      this.invoiceform.get("duedate")?.setValue(formattedDate1);
      this.invoiceform.get("producttotalamount")?.setValue(this.invoice.grandtotal);
      this.invoiceform.get("addoncharges")?.setValue(this.invoice.addoncharges);
      this.invoiceform.get("invoicediscountamount")?.setValue(this.invoice.invoicediscountamount);
      this.invoiceform.get("frieghtcharges")?.setValue(this.invoice.frieghtcharges);
      this.invoiceform.get("forwardingCharges")?.setValue(this.invoice.forwardingCharges);
      this.invoiceform.get("insurancecharges")?.setValue(this.invoice.insurancecharges);
      this.invoiceform.get("roundoff")?.setValue(this.invoice.roundoff);
      this.invoiceform.get("grandtotal")?.setValue(this.invoice.grandtotal);
    });
  }

  get branchControl() {
    return this.invoiceform.get('branch');
  }

  get invrefnoControl() {
    return this.invoiceform.get('invoice_ref_no');
  }

  get invoicedateControl() {
    return this.invoiceform.get('invoicedate');
  }

  get paytermControl() {
    return this.invoiceform.get('paymentterm');
  }

  get duedateControl() {
    return this.invoiceform.get('duedate');
  }

  get cusnameControl() {
    return this.invoiceform.get('customergid');
  }

  get connumControl() {
    return this.invoiceform.get('customercontactnumber');
  }

  get emailControl() {
    return this.invoiceform.get('customeremailaddress');
  }

  get custaddControl() {
    return this.invoiceform.get('customeraddress');
  }

  get deliverydateControl() {
    return this.invoiceform.get('deliverydate');
  }

  get currencyControl() {
    return this.invoiceform.get('currencygid');
  }

  get exchrateControl() {
    return this.invoiceform.get('exchangerate');
  }

  get salestypenameControl() {
    return this.invoiceform.get('salestype');
  }

  get mfdispControl() {
    return this.invoiceform.get('mode_of_dispatch');
  }

  get remarksControl() {
    return this.invoiceform.get('remarks');
  }

  get prodnameControl() {
    return this.productform.get('productgid');
  }

  onSubmit() {
    console.log(this.invoiceform)

    var api = 'Einvoice/InvoiceSubmit';
    this.service.post(api, this.invoiceform.value).subscribe((result: any) => {
      this.route.navigate(['/einvoice/Invoice-Summary']);
    },
    );
  }

  OnChangeCurrency() {
    let currencyexchange_gid = this.invoiceform.get("currencygid")?.value;

    let param = {
      currencyexchange_gid: currencyexchange_gid
    }

    var url = 'Einvoice/GetOnChangeCurrency';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.currencylist = this.responsedata.GetOnChangeCurrency;
      this.invoiceform.get("exchangerate")?.setValue(this.currencylist[0].exchange_rate);
      this.currency_name = this.currencylist[0].currency_code;
    });
  }

  GetOnChangeTerms() {
    debugger

    let template_gid = this.invoiceform.value.template_name;
    let param = {
      template_gid: template_gid
    }

    var url = 'Einvoice/GetOnChangeTerms';

    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.templatecontent_list = this.responsedata.terms_lists;
      this.invoiceform.get("termsandconditions")?.setValue(this.templatecontent_list[0].template_content);
      this.invoiceform.value.template_gid = result.terms_list[0].template_gid
    });
  }

  customerDetailsFetch() {
    let customer_gid = this.invoiceform.get('customergid')?.value;
    
    let param = {
      customer_gid: customer_gid
    }

    var url = 'Einvoice/GetOnChangeCustomer';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.customerdetails_list = this.responsedata.Getcustomeronchangedetails;
      this.invoiceform.get("customer_branch")?.setValue(this.customerdetails_list[0].customerbranchname);
      this.invoiceform.get("customercontactperson")?.setValue(this.customerdetails_list[0].customercontactname);
      this.invoiceform.get("customercontactnumber")?.setValue(this.customerdetails_list[0].mobile);
      this.invoiceform.get("customeremailaddress")?.setValue(this.customerdetails_list[0].email);
      this.invoiceform.get("customeraddress")?.setValue(this.customerdetails_list[0].address);
    });
  }

  productDetailsFetch() {
    let product_gid = this.productform.get('productgid')?.value;
    let param = {
      product_gid: product_gid
    }

    var url = 'Einvoice/GetOnChangeProduct';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.productdetails_list = this.responsedata.Getproductonchangedetails;
      this.productform.get("productgroup_gid")?.setValue(this.productdetails_list[0].productgroup_name);
      this.productform.get("productcode")?.setValue(this.productdetails_list[0].product_code);
      this.productform.get("hsncode")?.setValue(this.productdetails_list[0].hsn_code);
      this.productform.get("hsndescription")?.setValue(this.productdetails_list[0].hsn_description);
      this.productform.get("productuom_gid")?.setValue(this.productdetails_list[0].productuom_name);
      this.productform.get("mrp")?.setValue(this.productdetails_list[0].product_price);
    })
  }

  productSubmit() {
    console.log(this.productform)
    var api = 'Einvoice/InvoicePostProduct';
    this.service.post(api, this.productform.value).subscribe((result: any) => {
      this.editinvoiceproductsummary()
    },
    );
  }

  prodtotalcal() {
    const subtotal = this.unitprice * this.quantity;
    this.discountamount = (subtotal * this.discountpercentage) / 100;
    this.totalamount = subtotal - this.discountamount;
  }

  taxAmount1() {
    let tax_gid = this.productform.get('taxname1')?.value;

    this.taxpercentage = this.getDimensionsByFilter(tax_gid);
    console.log(this.taxpercentage);
    let tax_percentage = this.taxpercentage[0].tax_percentage;
    console.group(tax_percentage)

    this.taxamount1 = ((tax_percentage) * (this.totalamount) / 100);

    if (this.taxamount1 == undefined) {
      const subtotal = this.unitprice * this.quantity;
      this.discountamount = (subtotal * this.discountpercentage) / 100;
      this.totalamount = subtotal - this.discountamount;
    }
    else {
      this.totalamount = ((this.totalamount) + (+this.taxamount1));
    }
  }

  getDimensionsByFilter(id: any) {
    return this.tax_list.filter((x: { tax_gid: any; }) => x.tax_gid === id);
  }

  finaltotal() {
    this.grandtotal = ((this.producttotalamount) + (+this.addoncharges) + (+this.frieghtcharges) + (+this.forwardingCharges) + (+this.insurancecharges) + (+this.roundoff) - (+this.invoicediscountamount));
  }

  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }

  ondelete() {
    var url = 'Einvoice/GetDeleteInvoiceProductSummary'
    let param = {
      invoicedtl_gid: this.parameterValue
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == true) {
        this.ToastrService.success(result.message)

        this.editinvoiceproductsummary()
      }
      else {
        this.ToastrService.warning(result.message)
      }
    });
  }
 
  updated() {
    console.log(this.invoiceform)

    var api = 'Einvoice/UpdatedInvoice';
    this.service.post(api, this.invoiceform.value).subscribe((result: any) => {
      this.route.navigate(['/einvoice/Invoice-Summary']);
    },
    );
  }

  back() {
    this.route.navigate(['/einvoice/Invoice-Summary']);
  }
}
