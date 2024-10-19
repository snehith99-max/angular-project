import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { padStart } from '@fullcalendar/core/internal';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-rbl-trn-invoiceaccountingaddconfirm',
  templateUrl: './rbl-trn-invoiceaccountingaddconfirm.component.html',
  styleUrls: ['./rbl-trn-invoiceaccountingaddconfirm.component.scss']
})

export class RblTrnInvoiceaccountingaddconfirmComponent {
  invoiceaccountinglist: any;
  invoiceaccountingform: FormGroup | any;
  invoiceaccountingduedateControl: any;
  salesorder_gid: any;
  responsedata: any;
  invoiceaccountingdata: any;
  currency_code: any;
  quantity: any;
  mdlTerms: any;
  terms_list: any[] = [];
  templatecontent_list: any;
  lspage1: any;
  mdlaccpayterm: any;
  mdlaccsalestype: any;

  config: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '410px',
    minHeight: '0rem',
    placeholder: 'Enter text here...',
    translate: 'no',
  };
  parameter: any;
  parameter1: any;
  allchargeslist: any;
  invoiceaccounting_addon_amount: any;
  invoiceaccounting_freightcharges: any;
  invoiceaccounting_insurancecharges: any;
  invoiceaccounting_discountamount: any;
  orderdiscount: any;
  orderaddon: any;
  orderamount: any;
  dicosuntamount: number = 0;
  freightcharges: any;
  buybackcharges: any;
  packingcharges: any;
  insurancecharges: any;
  outstandingamout: number = 0;
  roundoff: any;
  grandtotal: any;
  taxamount: any;
  netamount: any;
  tax_list: any;
  Mdltax: any;
  salesinvoiceproduct_list: any;


  // Model Name

  discount_percentage: any;
  discount_amount: any;
  product_price: any;
  tax_amount1: any;
  total_amount: any;
  taxpercentage: any;

  data = { qty_quoted: 0 };
  invoiceaccouting_orderdiscountamount: any;
  GetTaxSegmentList: any;

  ngOnInit() {
this.invoiceaccouting_orderdiscountamount=0;
this.invoiceaccounting_addon_amount =0;
    this.invoiceaccountingform = new FormGroup({
      invoiceaccounting_salesorder_gid: new FormControl(''),
      invoiceaccounting_refno: new FormControl('', Validators.required),
      invoice_date: new FormControl(this.getCurrentDate()),
      invoiceaccounting_payterm: new FormControl(''),
      invoiceaccounting_duedate: new FormControl(''),
      invoiceaccounting_orderrefno: new FormControl(''),
      customer_gid: new FormControl(''),
      invoiceaccounting_orderdate: new FormControl(''),
      invoiceaccounting_customername: new FormControl(''),
      invoiceaccounting_email: new FormControl(''),
      invoiceaccounting_customeraddress: new FormControl(''),
      invoiceaccounting_contactnumber: new FormControl(''),
      invoiceaccounting_contactperson: new FormControl(''),
      invoiceaccounting_branch: new FormControl(''),
      invoiceaccounting_currency: new FormControl(''),
      invoiceaccounting_exchangerate: new FormControl(''),
      invoiceaccounting_salestype: new FormControl('', [Validators.required]),
      invoiceaccounting_remarks: new FormControl(''),
      customerproduct_code: new FormControl(''),
      productgroup_name: new FormControl(''),
      description: new FormControl(''),
      qty_invoice: new FormControl(''),
      Vendor_price: new FormControl(''),
      margin_percentage: new FormControl(''),
      orderdetails: new FormControl(''),
      product_price: new FormControl(''),
      final_amount: new FormControl(''),
      invoiceaccounting_net_amount: new FormControl(''),
      invoiceaccounting_overall_tax: new FormControl(''),
      invoiceaccouting_orderdiscountamount: new FormControl(''),
      invoiceaccounting_addon_amount: new FormControl(''),
      invoiceaccounting_ordertotal: new FormControl(''),
      invoiceaccounting_ordertotal_withtax: new FormControl(''),
      invoiceaccounting_discountamount: new FormControl(''),
      invoiceaccounting_addonamount: new FormControl(''),
      invoiceaccounting_freightcharges: new FormControl(''),
      invoiceaccounting_buybackcharges: new FormControl(''),
      invoiceaccounting_packingcharges: new FormControl(''),
      invoiceaccounting_insurancecharges: new FormControl(''),
      invoiceaccounting_outstandingamount: new FormControl(''),
      invoiceaccounting_roundoff: new FormControl(''),
      invoiceaccounting_grandtotal: new FormControl(''),
      invoiceaccounting_termsandconditions: new FormControl(''),
      invoiceaccounting_taxname: new FormControl(''),
      invoiceaccounting_taxamount: new FormControl(''),
      invoiceaccounting_taxname2: new FormControl(''),
      invoiceaccounting_taxamount2: new FormControl(''),
      invoiceaccounting_taxname3: new FormControl(''),
      invoiceaccounting_taxamount3: new FormControl(''),
      template_gid: new FormControl(''),
      template_name: new FormControl(''),
      product_name: new FormControl(''),
      product_code: new FormControl(''),
      unit: new FormControl(''),
      qty_quoted: new FormControl(''),
      tax_name: new FormControl(''),
      tax_amount1: new FormControl(''),
      total_amount: new FormControl(''),
      discount_amount: new FormControl(''),
      discount_percentage: new FormControl('')
    })
    const options: Options = {
      dateFormat: 'd-m-Y',
    };

    flatpickr('.date-picker', options);

    const salesorder_gid = this.route.snapshot.paramMap.get('directorder_gid');
    this.salesorder_gid = salesorder_gid;

    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.salesorder_gid, secretKey).toString(enc.Utf8);

    let param = {
      directorder_gid: deencryptedParam
    }

    var api = 'SmrRptInvoiceReport/Getproductsummarydata';

    this.service.getparams(api, param).subscribe((result: any) => {
      this.responsedata = result;
      this.invoiceaccountinglist = this.responsedata.salesinvoiceproduct_list;
      //this.invoiceaccountingform.get("product_name")?.setValue(this.salesinvoiceproduct_list[0].product_name);

    });
    var url = 'SmrTrnSalesorder/GetTax1Dtl'
    this.service.get(url).subscribe((result: any) => {
      this.tax_list = result.GetTax1Dtl;
    });

    var url = 'Einvoice/GetTermsandConditions'
    this.service.get(url).subscribe((result: any) => {
      this.terms_list = result.terms_lists;
    });

    this.GetSalesinvoicedata();


    var api = 'SmrMstSalesConfig/GetAllChargesConfig';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.allchargeslist = this.responsedata.salesconfigalllist;
      this.invoiceaccounting_addon_amount = this.allchargeslist[0].flag;
      this.invoiceaccounting_discountamount = this.allchargeslist[1].flag;
      this.invoiceaccounting_freightcharges = this.allchargeslist[2].flag;
      // this.forwardingCharges = this.allchargeslist[3].flag;
      this.invoiceaccounting_insurancecharges = this.allchargeslist[3].flag;

      if (this.allchargeslist[0].flag == 'Y') {
        this.invoiceaccounting_addon_amount = 0;
      } else {
        this.invoiceaccounting_addon_amount = this.allchargeslist[0].flag;
      }

      if (this.allchargeslist[1].flag == 'Y') {
        this.invoiceaccounting_discountamount = 0;
      } else {
        this.invoiceaccounting_discountamount = this.allchargeslist[1].flag;
      }

      if (this.allchargeslist[2].flag == 'Y') {
        this.invoiceaccounting_freightcharges = 0;
      } else {
        this.invoiceaccounting_freightcharges = this.allchargeslist[2].flag;
      }

      // if (this.allchargeslist[3].flag == 'Y') {
      //   this.forwardingCharges = 0;
      // } else {
      //   this.forwardingCharges = this.allchargeslist[3].flag;
      // }

      if (this.allchargeslist[3].flag == 'Y') {
        this.invoiceaccounting_insurancecharges = 0;
      } else {
        this.invoiceaccounting_insurancecharges = this.allchargeslist[3].flag;
      }
    });

    this.invoiceaccountingform.get('invoiceaccounting_payterm').valueChanges.subscribe((value: any) => {
      if (value) {
        const today = new Date();
        const invoiceaccounting_duedate = new Date(today.getTime() + (value * 24 * 60 * 60 * 1000));
        const day = invoiceaccounting_duedate.getDate().toString().padStart(2, '0');
        const month = (invoiceaccounting_duedate.getMonth() + 1).toString().padStart(2, '0');
        const year = invoiceaccounting_duedate.getFullYear().toString();

        const formattedDueDate = `${day}-${month}-${year}`;
        this.invoiceaccountingform.patchValue({ invoiceaccounting_duedate: formattedDueDate }, { emitEvent: false });

      }
    });



  }

  getCurrentDate(): string {
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = today.getFullYear();

    return dd + '-' + mm + '-' + yyyy;
  }
  constructor(private router: Router, private route: ActivatedRoute, private fb: FormBuilder, private service: SocketService, private ToastrService: ToastrService,public NgxSpinnerService:NgxSpinnerService) {
    this.invoiceaccountingform = new FormGroup({
      invoiceaccounting_salesorder_gid: new FormControl(''),
      invoiceaccounting_refno: new FormControl('', Validators.required),
      invoice_date: new FormControl(this.getCurrentDate()),
      invoiceaccounting_payterm: new FormControl(''),
      invoiceaccounting_duedate: new FormControl(''),
      invoiceaccounting_orderrefno: new FormControl(''),
      customer_gid: new FormControl(''),
      invoiceaccounting_orderdate: new FormControl(''),
      invoiceaccounting_customername: new FormControl(''),
      invoiceaccounting_email: new FormControl(''),
      invoiceaccounting_customeraddress: new FormControl(''),
      invoiceaccounting_contactnumber: new FormControl(''),
      invoiceaccounting_contactperson: new FormControl(''),
      invoiceaccounting_branch: new FormControl(''),
      invoiceaccounting_currency: new FormControl(''),
      invoiceaccounting_exchangerate: new FormControl(''),
      invoiceaccounting_salestype: new FormControl('', [Validators.required]),
      invoiceaccounting_remarks: new FormControl(''),
      customerproduct_code: new FormControl(''),
      productgroup_name: new FormControl(''),
      description: new FormControl(''),
      qty_invoice: new FormControl(''),
      Vendor_price: new FormControl(''),
      margin_percentage: new FormControl(''),
      orderdetails: new FormControl(''),
      product_price: new FormControl(''),
      final_amount: new FormControl(''),
      invoiceaccounting_net_amount: new FormControl(''),
      invoiceaccounting_overall_tax: new FormControl(''),
      invoiceaccouting_orderdiscountamount: new FormControl(''),
      invoiceaccounting_addon_amount: new FormControl(''),
      invoiceaccounting_ordertotal: new FormControl(''),
      invoiceaccounting_ordertotal_withtax: new FormControl(''),
      invoiceaccounting_discountamount: new FormControl(''),
      invoiceaccounting_addonamount: new FormControl(''),
      invoiceaccounting_freightcharges: new FormControl(''),
      invoiceaccounting_buybackcharges: new FormControl(''),
      invoiceaccounting_packingcharges: new FormControl(''),
      invoiceaccounting_insurancecharges: new FormControl(''),
      invoiceaccounting_outstandingamount: new FormControl(''),
      invoiceaccounting_roundoff: new FormControl(''),
      invoiceaccounting_grandtotal: new FormControl(''),
      invoiceaccounting_termsandconditions: new FormControl(''),
      invoiceaccounting_taxname: new FormControl(''),
      invoiceaccounting_taxamount: new FormControl(''),
      invoiceaccounting_taxname2: new FormControl(''),
      invoiceaccounting_taxamount2: new FormControl(''),
      invoiceaccounting_taxname3: new FormControl(''),
      invoiceaccounting_taxamount3: new FormControl(''),
      template_gid: new FormControl(''),
      template_name: new FormControl(''),
      product_name: new FormControl(''),
      product_code: new FormControl(''),
      unit: new FormControl(''),
      qty_quoted: new FormControl(''),
      tax: new FormControl(''),
      tax_amount1: new FormControl(''),
      total_amount: new FormControl(''),
      discount_amount: new FormControl(''),
      discount_percentage: new FormControl('')
    })
  }

  get invoiceaccountingformrefnoControl() {
    return this.invoiceaccountingform.get('invoiceaccounting_refno');
  }
  get invoiceaccountingdateControl() {
    return this.invoiceaccountingform.get('invoice_date');
  }
  get invoiceaccountingformsalestypeControl() {
    return this.invoiceaccountingform.get('invoiceaccounting_salestype');
  }

  GetOnChangeTerms() {

    let template_gid = this.invoiceaccountingform.value.template_name;
    let param = {
      template_gid: template_gid
    }

    var url = 'Einvoice/GetOnChangeTerms';

    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.templatecontent_list = this.responsedata.terms_lists;
      this.invoiceaccountingform.get("invoiceaccounting_termsandconditions")?.setValue(this.templatecontent_list[0].template_content);
      this.invoiceaccountingform.value.template_gid = result.terms_list[0].template_gid
    });
  }

  GetSalesinvoicedata() {

debugger;
    const salesorder_gid = this.route.snapshot.paramMap.get('directorder_gid');
    this.salesorder_gid = salesorder_gid;

    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.salesorder_gid, secretKey).toString(enc.Utf8);

    let param = {
      directorder_gid: deencryptedParam
    }

    var api = 'Einvoice/GetSalesinvoicedata';

    this.service.getparams(api, param).subscribe((result: any) => {
      this.responsedata = result;
      this.invoiceaccountingdata = result;
      this.invoiceaccountingform.get("invoiceaccounting_salesorder_gid")?.setValue(this.invoiceaccountingdata.serviceorder_gid);
      // this.invoiceaccountingform.get("invoice_date")?.setValue(formattedDate);
      this.invoiceaccountingform.get("invoiceaccounting_refno")?.setValue(this.invoiceaccountingdata.invoice_no);
      this.invoiceaccountingform.get("invoiceaccounting_payterm")?.setValue(this.invoiceaccountingdata.invoiceaccounting_payterm);
      this.invoiceaccountingform.get("invoiceaccounting_duedate")?.setValue(this.invoiceaccountingdata.invoiceaccounting_duedate);
      this.invoiceaccountingform.get("invoiceaccounting_orderrefno")?.setValue(this.invoiceaccountingdata.so_reference);
      this.invoiceaccountingform.get("invoiceaccounting_orderdate")?.setValue(this.invoiceaccountingdata.serviceorder_date)
      // this.invoiceaccountingform.get("invoiceaccounting_orderdate")?.setValue(formattedDate1);
      this.invoiceaccountingform.get("invoiceaccounting_customername")?.setValue(this.invoiceaccountingdata.customer_name);
      this.invoiceaccountingform.get("invoiceaccounting_email")?.setValue(this.invoiceaccountingdata.email);
      this.invoiceaccountingform.get("invoiceaccounting_customeraddress")?.setValue(this.invoiceaccountingdata.customer_address);
      this.invoiceaccountingform.get("invoiceaccounting_contactnumber")?.setValue(this.invoiceaccountingdata.customer_mobile);
      this.invoiceaccountingform.get("invoiceaccounting_contactperson")?.setValue(this.invoiceaccountingdata.customercontact_name);
      this.invoiceaccountingform.get("invoiceaccounting_branch")?.setValue(this.invoiceaccountingdata.branch_name);
      this.invoiceaccountingform.get("invoiceaccounting_currency")?.setValue(this.invoiceaccountingdata.currency_code);
      this.invoiceaccountingform.get("invoiceaccounting_exchangerate")?.setValue(this.invoiceaccountingdata.exchange_rate);
      this.invoiceaccountingform.get("invoiceaccounting_salestype")?.setValue(this.invoiceaccountingdata.invoiceaccounting_salestype);
      this.invoiceaccountingform.get("invoiceaccounting_remarks")?.setValue(this.invoiceaccountingdata.remarks);
      this.invoiceaccountingform.get("product_name")?.setValue(this.invoiceaccountingdata.product_name);
      this.invoiceaccountingform.get("description")?.setValue(this.invoiceaccountingdata.description);
      //this.invoiceaccountingform.get("qty_quoted")?.setValue(this.invoiceaccountingdata.qty_quoted);
      this.invoiceaccountingform.get("vendoramount")?.setValue(this.invoiceaccountingdata.vendoramount);
      this.invoiceaccountingform.get("discount_amount")?.setValue(this.invoiceaccountingdata.discountamount);
      this.invoiceaccountingform.get("product_price")?.setValue(this.invoiceaccountingdata.product_price);
      this.invoiceaccountingform.get("unit")?.setValue(this.invoiceaccountingdata.unit);
      this.invoiceaccountingform.get("discount_percentage")?.setValue(this.invoiceaccountingdata.discount_percentage);
      // this.Mdltax = this.invoiceaccountingdata.tax1_gid;
      this.invoiceaccountingform.get("product_code")?.setValue(this.invoiceaccountingdata.product_code);
      this.invoiceaccountingform.get("tax_name")?.setValue(this.invoiceaccountingdata.tax_name);
      this.invoiceaccountingform.get("tax_amount1")?.setValue(this.invoiceaccountingdata.tax_amount1);
      this.invoiceaccountingform.get("total_amount")?.setValue(this.invoiceaccountingdata.total_amount1);
      this.invoiceaccountingform.get("orderdetails")?.setValue(this.invoiceaccountingdata.orderdetails);
      this.invoiceaccountingform.get("net_amount")?.setValue(this.invoiceaccountingdata.grand_total);
      this.invoiceaccountingform.get("invoiceaccounting_taxname")?.setValue(this.invoiceaccountingdata.tax_name1);
      this.invoiceaccountingform.get("invoiceaccounting_taxamount")?.setValue(this.invoiceaccountingdata.tax_amount1);
      this.invoiceaccountingform.get("invoiceaccounting_taxname2")?.setValue(this.invoiceaccountingdata.tax_name2);
      this.invoiceaccountingform.get("invoiceaccounting_taxamount2")?.setValue(this.invoiceaccountingdata.tax_amount2);
      this.invoiceaccountingform.get("invoiceaccounting_taxname3")?.setValue(this.invoiceaccountingdata.tax_name3);
      this.invoiceaccountingform.get("invoiceaccounting_taxamount3")?.setValue(this.invoiceaccountingdata.tax_amount3);
      this.invoiceaccountingform.get("final_amount")?.setValue(this.invoiceaccountingdata.final_amount);
      this.taxamount = this.invoiceaccountingdata.taxamount;
      this.orderdiscount = this.invoiceaccountingdata.additionaldiscount;
      this.netamount = this.invoiceaccountingdata.total_price;
      this.orderamount = this.invoiceaccountingdata.total_amount;
      this.invoiceaccountingform.get("invoiceaccounting_discountamount")?.setValue(this.invoiceaccountingdata.gst_amount);
      this.orderaddon = this.invoiceaccountingdata.addoncharge;
     this.invoiceaccountingform.get("invoiceaccounting_addon_amount")?.setValue(this.invoiceaccountingdata.addoncharge);
      this.freightcharges = this.invoiceaccountingdata.freight_charges;
      this.buybackcharges = this.invoiceaccountingdata.buyback_charges;
      this.packingcharges = this.invoiceaccountingdata.packing_charges;
      this.insurancecharges = this.invoiceaccountingdata.insurance_charges;
      this.roundoff = this.invoiceaccountingdata.roundoff;
      this.grandtotal = this.invoiceaccountingdata.grand_total;
      this.invoiceaccountingform.get("invoiceaccounting_termsandconditions")?.setValue(this.invoiceaccountingdata.termsandconditions);
      this.invoiceaccountingform.get("customer_gid")?.setValue(this.invoiceaccountingdata.customer_gid);
      this.responsedata = result.salesinvoiceproduct_list;

    });
  }

  salesinvoicesubmit() {
    console.log(this.invoiceaccountingform)
    debugger
    if( this.invoiceaccountinglist == null || this.invoiceaccountinglist == undefined 
      ){
      window.scrollTo({
        top: 0, 
      });
      this.ToastrService.warning('Atleast One Product Must Be Added!');
      return
    }
    if(
    this.invoiceaccountingform.value.invoice_date==""||this.invoiceaccountingform.value.invoice_date == null || this.invoiceaccountingform.value.invoice_date == undefined &&      
        
        this.invoiceaccountingform.value.invoiceaccounting_duedate==""||this.invoiceaccountingform.value.invoiceaccounting_duedate == null || this.invoiceaccountingform.value.invoiceaccounting_duedate == undefined &&
        this.invoiceaccountingform.value.invoiceaccounting_salestype==""||this.invoiceaccountingform.value.invoiceaccounting_salestype == null || this.invoiceaccountingform.value.invoiceaccounting_salestype == undefined &&
        this.invoiceaccountingform.value.invoiceaccounting_remarks==""||this.invoiceaccountingform.value.invoiceaccounting_remarks == null || this.invoiceaccountingform.value.invoiceaccounting_remarks == undefined 
        
        )
    {
      window.scrollTo({
        top: 0, 
      });
      this.ToastrService.warning('Kindly Fill all the Mandatory Fields!');
      return
    }

    var api = 'SmrRptInvoiceReport/salesinvoiceOnsubmit';
    this.NgxSpinnerService.show();
    this.service.post(api, this.invoiceaccountingform.value).subscribe((result: any) => {
      const lspage1 = this.route.snapshot.paramMap.get('lspage');
      this.parameter = this.route.snapshot.paramMap.get('leadbank_gid');
      this.parameter1 = this.route.snapshot.paramMap.get('lead2campaign_gid');
      const secretKey = 'storyboarderp';

      const lspage2 = AES.encrypt(this.lspage1, secretKey).toString();

      if (result.status == false) {
        this.NgxSpinnerService.hide()
        window.scrollTo({
          top: 0, 
        });
        this.ToastrService.warning(result.message)
      }
      else {
        this.NgxSpinnerService.hide()
          window.scrollTo({
      top: 0, 
    });
        this.ToastrService.success(result.message);
            
      if (lspage1 == 'Invoice-Summary') {
        this.router.navigate(['/einvoice/Invoice-Summary'])
      }
      else {
        this.router.navigate(['/crm/CrmTrn360view', this.parameter, this.parameter1, lspage2])
      }
    }
    this.NgxSpinnerService.hide()
    },
    );
  }

  back1() {
    this.router.navigate(['/einvoice/Invoice-Summary']);
  }

  back() {

    const lspage1 = this.route.snapshot.paramMap.get('lspage');
    this.parameter = this.route.snapshot.paramMap.get('leadbank_gid');
    this.parameter1 = this.route.snapshot.paramMap.get('lead2campaign_gid');
    const secretKey = 'storyboarderp';

    const lspage2 = AES.encrypt(this.lspage1, secretKey).toString();
    if (lspage1 == 'Invoice-Summary') {
      this.router.navigate(['/einvoice/Invoice-Summary'])
    }
    else {
      this.router.navigate(['/crm/CrmTrn360view', this.parameter, this.parameter1, lspage2])
    }
  }

  prodtotalcal1(i: any) {
    const quantity = this.invoiceaccountinglist[i].qty_quoted;
    const subtotal = quantity * this.invoiceaccountinglist[i].product_price;
    this.invoiceaccountinglist[i].discount_amount = (subtotal * this.invoiceaccountinglist[i].discount_percentage) / 100;
    this.invoiceaccountinglist[i].discount_amount = (this.invoiceaccountinglist[i].discount_amount * 100) / 100;
    this.invoiceaccountinglist[i].total_amount = subtotal - this.invoiceaccountinglist[i].discount_amount;
    this.invoiceaccountinglist[i].total_amount = (+(subtotal - this.invoiceaccountinglist[i].discount_amount));
    // var param={
    //   productlist : this.invoiceaccountinglist.values,
    // }
  }
  onChangediscountamount(i: any) {
    this.invoiceaccountinglist[i].product_price = (this.invoiceaccountinglist[i].product_price - this.invoiceaccountinglist[i].discount_percentage) / 100;
  }
  getDimensionsByFilter(id: any) {
    return this.tax_list.filter((x: { tax_gid: any; }) => x.tax_gid === id);
  }
 
  
  onChangeTaxAmount(event: any, i: number) {
    if (event === null || event === undefined) {
        this.invoiceaccountinglist[i].tax_amount4 = 0;
        return;
    }

    const tax_name = event.tax_name;
    this.invoiceaccountinglist[i].tax4_gid = event.tax_gid;
  
    // Perform calculation only for the specific tax field (tax4)
    const subtotal = parseFloat((this.invoiceaccountinglist[i].product_price * this.invoiceaccountinglist[i].qty_quoted).toFixed(2));
    this.invoiceaccountinglist[i].discount_amount = parseFloat(((subtotal * this.invoiceaccountinglist[i].discount_percentage) / 100).toFixed(2));

    const selectedTax = this.tax_list.find((tax: { tax_name: any; }) => tax.tax_name === tax_name);
    if (!selectedTax) {
        this.invoiceaccountinglist[i].tax_amount4 = 0;
        return;
    }

    const tax_gid = selectedTax.tax_gid;
    this.taxpercentage = this.getDimensionsByFilter(tax_gid);
    const tax_percentage = this.taxpercentage[0].percentage;
    this.invoiceaccountinglist[i].tax_amount4 = parseFloat((tax_percentage * subtotal / 100).toFixed(2));
    debugger
    const tax_amount1 = parseFloat(this.invoiceaccountinglist[i].tax_amount1) || 0;
    const tax_amount2 = parseFloat(this.invoiceaccountinglist[i].tax_amount2) || 0;
    const tax_amount3 = parseFloat(this.invoiceaccountinglist[i].tax_amount3) || 0;
    const tax_amount4 = parseFloat(this.invoiceaccountinglist[i].tax_amount4) || 0;

    // Calculate the total amount considering all tax amounts
    const total_tax_amount = tax_amount1 + tax_amount2 + tax_amount3 + tax_amount4;
    this.invoiceaccountinglist[i].total_amount = parseFloat((subtotal - this.invoiceaccountinglist[i].discount_amount + total_tax_amount).toFixed(2));
}


  // onChangeTaxAmount(event: any, i: number) {
  //   debugger
  //   if (event === null || event === undefined) {
  //     this.invoiceaccountinglist[i].tax_amount1 = 0;
  //     return;
  //   }

  //   const tax_name = event.tax_name;
  //   this.invoiceaccountinglist[i].tax = tax_name;
  //   const subtotal = parseFloat((this.invoiceaccountinglist[i].product_price * this.invoiceaccountinglist[i].qty_quoted).toFixed(2));
  //   this.invoiceaccountinglist[i].discount_amount = parseFloat(((subtotal * this.invoiceaccountinglist[i].discount_percentage) / 100).toFixed(2));
  //   const selectedTax = this.tax_list.find((tax: { tax_name: any; }) => tax.tax_name === tax_name);
  //   const tax_gid = selectedTax.tax_gid;
  //   this.taxpercentage = this.getDimensionsByFilter(tax_gid);
  //   const tax_percentage = this.taxpercentage[0].percentage;
  //   this.invoiceaccountinglist[i].tax_amount1 = parseFloat((tax_percentage * subtotal / 100).toFixed(2));
  //   this.invoiceaccountinglist[i].total_amount = parseFloat((subtotal - this.invoiceaccountinglist[i].discount_amount + this.invoiceaccountinglist[i].tax_amount1).toFixed(2));

  // }


  calcNetAmount() {
    this.netamount = 0;

    for (let i = 0; i < this.invoiceaccountinglist.length; i++) {
      let amount = this.invoiceaccountinglist[i].total_amount;
      if (typeof amount === 'string') {
        amount = parseFloat(amount.replace(/,/g, ''));
      }
      this.netamount += amount;
    }

     // For over all total 
     if (!isNaN(this.netamount) && typeof this.netamount === 'number') {
      this.netamount = parseFloat(this.netamount.toFixed(2));
    }
    if (!isNaN(this.taxamount) && typeof this.taxamount === 'number') {
      this.taxamount = parseFloat(this.taxamount.toFixed(2));
    }
    this.netamount = this.netamount.toLocaleString();
    const netamount = parseFloat(this.netamount.replace(/,/g, ''));
    const taxamount = parseFloat(this.taxamount.replace(/,/g, ''));
    // this.grandtotal = (+(netamount)+ (+taxamount)).toLocaleString(); 

    const grandtotal = netamount + taxamount;

    // Convert the result to a localized string
    this.grandtotal = grandtotal.toLocaleString();
  }


  finaltotal() {
    let netamount = typeof this.netamount === 'string' ? parseFloat(this.netamount.replace(/,/g, '')) : parseFloat(this.netamount) || 0;
    let taxamount = isNaN(parseFloat(this.taxamount.replace(/,/g, ''))) ? 0 : parseFloat(this.taxamount.replace(/,/g, ''));
    let orderdiscount = isNaN(parseFloat(this.orderdiscount.replace(/,/g, ''))) ? 0 : parseFloat(this.orderdiscount.replace(/,/g, ''));
    let orderaddon = isNaN(parseFloat(this.orderaddon.replace(/,/g, ''))) ? 0 : parseFloat(this.orderaddon.replace(/,/g, ''));
    let freightcharges = isNaN(parseFloat(this.freightcharges.replace(/,/g, ''))) ? 0 : parseFloat(this.freightcharges.replace(/,/g, ''));
    let buybackcharges = isNaN(parseFloat(this.buybackcharges.replace(/,/g, ''))) ? 0 : parseFloat(this.buybackcharges.replace(/,/g, ''));
    let packingcharges = isNaN(parseFloat(this.packingcharges.replace(/,/g, ''))) ? 0 : parseFloat(this.packingcharges.replace(/,/g, ''));
    let insurancecharges = isNaN(parseFloat(this.insurancecharges.replace(/,/g, ''))) ? 0 : parseFloat(this.insurancecharges.replace(/,/g, ''));
    let roundoff = isNaN(parseFloat(this.roundoff.replace(/,/g, ''))) ? 0 : parseFloat(this.roundoff.replace(/,/g, ''));



    if (orderdiscount == 0 || this.orderdiscount === '' || orderdiscount == null || orderdiscount == undefined) {

      this.grandtotal = (+(netamount) + (+taxamount) + (+orderaddon) + (+freightcharges) - (+buybackcharges) + (+packingcharges) + (+insurancecharges) + (+roundoff));
    }
    else if (orderaddon == 0 || this.orderaddon === '' || orderaddon == null || orderaddon == undefined) {

      this.grandtotal = (+(netamount) + (+taxamount) - (+orderdiscount) + (+freightcharges) - (+buybackcharges) + (+packingcharges) + (+insurancecharges) + (+roundoff));
    }
    else if (freightcharges == 0 || this.freightcharges === '' || freightcharges == null || freightcharges == undefined) {

      this.grandtotal = (+(netamount) + (+taxamount) - (+orderdiscount) + (+orderaddon) - (+buybackcharges) + (+packingcharges) + (+insurancecharges) + (+roundoff));
    }
    else if (buybackcharges == 0 || this.buybackcharges === '' || buybackcharges == null || buybackcharges == undefined) {

      this.grandtotal = (+(netamount) + (+taxamount) - (+orderdiscount) + (+orderaddon) + (+freightcharges) + (+packingcharges) + (+insurancecharges) + (+roundoff));
    }
    else if (packingcharges == 0 || this.packingcharges === '' || packingcharges == null || packingcharges == undefined) {

      this.grandtotal = (+(netamount) + (+taxamount) - (+orderdiscount) + (+orderaddon) + (+freightcharges) - (+buybackcharges) + (+insurancecharges) + (+roundoff));
    }
    else if (insurancecharges == 0 || this.insurancecharges === '' || insurancecharges == null || insurancecharges == undefined) {

      this.grandtotal = (+(netamount) + (+taxamount) - (+orderdiscount) + (+orderaddon) + (+freightcharges) - (+buybackcharges) + (+packingcharges) + (+roundoff));
    }
    else if (roundoff == 0 || this.roundoff === '' || roundoff == null || roundoff == undefined) {

      this.grandtotal = (+(netamount) + (+taxamount) - (+orderdiscount) + (+orderaddon) + (+freightcharges) - (+buybackcharges) + (+packingcharges) + (+insurancecharges));
    }
    else {
      this.grandtotal = (+(netamount) + (+taxamount) - (+orderdiscount) + (+orderaddon) + (+freightcharges) - (+buybackcharges) + (+packingcharges) + (+insurancecharges) + (+roundoff));
    }
  }

  
  // for tax segment
  fetchProductSummaryAndTax(product_gid: any,salesorder_gid:any,customer_gid:any) {
    debugger
        let param = {
        product_gid: product_gid,
        salesorder_gid: salesorder_gid,
        customer_gid:customer_gid
      };
  
      var api = 'SmrTrnSalesorder/GetViewsalesorderdetails';
      this.service.getparams(api, param).subscribe((result: any) => {
        this.responsedata = result;
        this.GetTaxSegmentList = result.GetTaxSegmentListorder;
  
        // Handle tax segments for the current product
        this.handleTaxSegments(product_gid, this.GetTaxSegmentList);
  
       
      }, (error) => {
        console.error('Error fetching tax details:', error);
        
      });
    
  }
  
  // Method to handle tax segments for the current product
  handleTaxSegments(product_gid: string ,taxSegments: any[]) {
    // Find tax segments for the current product_gid
    const productTaxSegments = taxSegments.filter((taxSegment: { product_gid: string; }) => taxSegment.product_gid === product_gid);
  
    if (productTaxSegments.length > 0) {
      // Assign tax segments to the current product
      this.invoiceaccountinglist.forEach((product: { product_gid: string; taxSegments: any[]; }) => {
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

}
