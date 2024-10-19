import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { DatePipe } from '@angular/common';
import { AES, enc } from 'crypto-js';

@Component({
  selector: 'app-smr-trn-invoice-add360',
  templateUrl: './smr-trn-invoice-add360.component.html',
  styleUrls: ['./smr-trn-invoice-add360.component.scss']
})
export class SmrTrnInvoiceAdd360Component {
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
  mdlaccpayterm:any;
  productunitlist: any;
  duedate:any;
  unitprice: number = 0;
  quantity: number = 0;
  discountpercentage: number = 0;
  discountamount: number = 0;
  tax_list: any;
  currencylist: any;
  branchgstlist: any;
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
  branch_list: any;
  taxpercentage2: any;
  taxamount2: number = 0;
  producttotalamount: any;
  parameterValue: string | undefined;
  currency_name: any;
  mdlTerms: any;
  terms_list: any[] = [];
  templatecontent_list: any;
  allchargeslist: any[] = [];
  leadbank_gid: any;
  leadbankcontact_gid:any;
  lead2campaign_gid: any;
  lspage:any;
  lspage1:any;
  lspage2:any;
  mdlCustomer: any;
  mdlBranch: any;
  mdlCurrency: any;
  mdlProduct: any;
  mdlTax: any;

  config: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '28rem',
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

    const leadbank_gid = this.route.snapshot.paramMap.get('leadbank_gid');
    const leadbankcontact_gid = this.route.snapshot.paramMap.get('leadbankcontact_gid');
    const lead2campaign_gid = this.route.snapshot.paramMap.get('lead2campaign_gid');
    const lspage = this.route.snapshot.paramMap.get('lspage');

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
    this.leadbankcontact_gid = deencryptedParam1;
    this.lead2campaign_gid = deencryptedParam2

    var api = 'SmrTrnCustomerEnquiry/GetCustomer360';
    let params = {
      leadbank_gid: deencryptedParam,
    }
    this.service.getparams(api, params).subscribe((result: any) => {
      this.responsedata = result;
      this.customernamelist = this.responsedata.GetCustomername;
    });

    this.invoiceform.get('paymentterm').valueChanges.subscribe((value: any) => {
      // Calculate due date based on the entered number (e.g., add the number of days to the current date)
      const currentDate = new Date();
      const dueDate = new Date(currentDate.setDate(currentDate.getDate() + parseInt(value, 10)));
      // Format due date to 'DD-MM-YYYY' using DatePipe
      const formattedDueDate = this.datePipe.transform(dueDate, 'dd-MM-yyyy');
      // Set the calculated due date to the form control
      this.invoiceform.get('duedate').setValue(formattedDueDate);
    });
  


    var api = 'Einvoice/GetProductNamedropDown';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.productnamelist = this.responsedata.Getproductnamedropdown;
    });

    var api = 'Einvoice/GetProductNamedropDown';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.productcodelist = this.responsedata.Getproductnamedropdown;
    });

    var api = 'Einvoice/GetcurrencyCodedropdown';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.currency_list = this.responsedata.Getcurrencycodedropdown;
    });

  
    
    var api = 'Einvoice/GetBranchName';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.branch_list = this.responsedata.GetBranchnamedropdown;
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

    this.invoiceproductsummary();

    var api = 'SmrMstSalesConfig/GetAllChargesConfig';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.allchargeslist = this.responsedata.salesconfigalllist;
      this.addoncharges = this.allchargeslist[0].flag;
      this.invoicediscountamount = this.allchargeslist[1].flag;
      this.frieghtcharges = this.allchargeslist[2].flag;
      this.forwardingCharges = this.allchargeslist[3].flag;
      this.insurancecharges = this.allchargeslist[4].flag;

      if (this.allchargeslist[0].flag == 'Y') {
        this.addoncharges = 0;
      } else {
        this.addoncharges = this.allchargeslist[0].flag;
      }

      if (this.allchargeslist[1].flag == 'Y') {
        this.invoicediscountamount = 0;
      } else {
        this.invoicediscountamount = this.allchargeslist[1].flag;
      }

      if (this.allchargeslist[2].flag == 'Y') {
        this.frieghtcharges = 0;
      } else {
        this.frieghtcharges = this.allchargeslist[2].flag;
      }

      if (this.allchargeslist[3].flag == 'Y') {
        this.forwardingCharges = 0;
      } else {
        this.forwardingCharges = this.allchargeslist[3].flag;
      }

      if (this.allchargeslist[4].flag == 'Y') {
        this.insurancecharges = 0;
      } else {
        this.insurancecharges = this.allchargeslist[4].flag;
      }
    });
  }
  OnclearDate()
  {
    this.duedate = '';
  }

  invoiceproductsummary() {
    var api = 'Einvoice/GetInvoiceProductSummary';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.invoiceproductlist = this.responsedata.invoiceproductsummarylist;
      this.invoiceform.get("producttotalamount")?.setValue(this.responsedata.grand_total);
    });

    // const currentDate = new Date().toISOString().split('T')[0];
    // this.invoiceform.get('invoicedate')?.setValue(currentDate);

    const currentDate = new Date();
    const day = currentDate.getDate();
    const month = currentDate.getMonth() + 1;
    const year = currentDate.getFullYear();

    const formattedDate = `${day.toString().padStart(2, '0')}-${month.toString().padStart(2, '0')}-${year}`;
    this.invoiceform.get('invoicedate')?.setValue(formattedDate);
  }

  constructor(private fb: FormBuilder,private datePipe: DatePipe, private route: ActivatedRoute, private router: Router, private service: SocketService, private ToastrService: ToastrService) {
    this.invoiceform = new FormGroup({
      customergid: new FormControl({ value: '', disabled: false }, Validators.required),
      customer_branch: new FormControl(''),
      customercontactperson: new FormControl(''),
      customercontactnumber: new FormControl(''),
      customeremailaddress: new FormControl(''),
      customeraddress: new FormControl(''),
      branchgid: new FormControl('', Validators.required),
      branch_name: new FormControl(''),
      gst: new FormControl(''),
      currencygid: new FormControl('', Validators.required),
      exchangerate: new FormControl(''),
      salestype: new FormControl('', Validators.required),
      mode_of_dispatch: new FormControl('',),
      remarks: new FormControl('',),
      invoiceref_no: new FormControl('',),
      invoicedate: new FormControl('', Validators.required),
      paymentterm: new FormControl('', Validators.required),
      duedate: new FormControl('', Validators.required),
      deliverydate: new FormControl('', Validators.required),
      payment: new FormControl(''),
      deliveryperiod: new FormControl(''),
      producttotalamount: new FormControl('',),
      addoncharges: new FormControl('',),
      invoicediscountamount: new FormControl('',),
      frieghtcharges: new FormControl('',),
      forwardingCharges: new FormControl('',),
      insurancecharges: new FormControl('',),
      roundoff: new FormControl('',),
      grandtotal: new FormControl('',[Validators.required]),
      template_gid: new FormControl(''),
      template_name: new FormControl(''),
      termsandconditions: new FormControl('',),
      
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

  get branchControl() {
    return this.invoiceform.get('branchgid');
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
   

if(this.invoiceproductlist!=null)
{
    var api = 'Einvoice/InvoiceSubmit';
    this.service.post(api, this.invoiceform.value).subscribe((result: any) => {
      this.router.navigate(['/einvoice/Invoice-Summary']);
      if (result.status == false) {
        this.ToastrService.warning(result.message)
      }
      else {
        this.router.navigate(['/einvoice/Invoice-Summary']);
        this.ToastrService.success(result.message)
      }
    });
  }
  else{
    this.ToastrService.warning(" Kindly Add Atleast One Product")

  }
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

  OnChangeBranchGSt() {
    let branch_gid = this.invoiceform.get("branchgid")?.value;
    
    let param = {
      branch_gid: branch_gid
    }

    var url = 'Einvoice/GetOnChangeBranch';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.branchgstlist = this.responsedata.GetOnChangeBranch;
      this.invoiceform.get("gst")?.setValue(this.branchgstlist[0].GST);
    });
  }

  GetOnChangeTerms() {    
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
      this.productform.get("unitprice")?.setValue(this.productdetails_list[0].unitprice);
    });
  }

  productSubmit() {
    console.log(this.productform)
    if(this.totalamount != 0)
    {
      var api = 'Einvoice/InvoicePostProduct';
      this.service.post(api, this.productform.value).subscribe((result: any) => {
        this.invoiceproductsummary();
        this.productform.reset();
      });
    }
    else
    {
      this.ToastrService.warning("Kindly fill the amount")
    }
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
    const addoncharges = isNaN(this.addoncharges) ? 0 : +this.addoncharges;
    const frieghtcharges = isNaN(this.frieghtcharges) ? 0 : +this.frieghtcharges;
    const forwardingCharges = isNaN(this.forwardingCharges) ? 0 : +this.forwardingCharges;
    const insurancecharges = isNaN(this.insurancecharges) ? 0 : +this.insurancecharges;
    const roundoff = isNaN(this.roundoff) ? 0 : +this.roundoff;
    const invoicediscountamount = isNaN(this.invoicediscountamount) ? 0 : +this.invoicediscountamount;

    this.grandtotal = ((this.producttotalamount) + (addoncharges) + (frieghtcharges) + (forwardingCharges) + (insurancecharges) + (roundoff) - (invoicediscountamount));
    this.invoiceform.get("grandtotal")?.setValue(this.grandtotal);
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
        this.invoiceproductsummary()
      }
      else {
        this.ToastrService.warning(result.message)
      }
    });
  }

  back() {

      this.router.navigate(['/einvoice/Invoice-Summary']);
  }
}
