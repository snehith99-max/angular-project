import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';




@Component({
  selector: 'app-rbl-trn-proforma-invoice-confirm',
  templateUrl: './rbl-trn-proforma-invoice-confirm.component.html',
  styleUrls: ['./rbl-trn-proforma-invoice-confirm.component.scss']
})

export class RblTrnProformaInvoiceConfirmComponent {
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
  proformainvoiceconfirmform: FormGroup | any;
  salesorder_gid: any;
  proformainvoicedata: any;
  responsedata: any;
  proformainvoiceproductlist: any;
  mdlperformainvoice: any;
  mdlTerms: any;
  freightcharges: any;
  buybackscrap: any;
  forwardingcharges: any;
  insurancecharges: any;
  roundoff: any;
  Grandtotal: any;
  Totalamountwithtax: any;
  proformaadvanceamount: any;
  proformaadvancepercentage: any;
  proformaadvanceroundoff: any;
  proformainvoicelist1 :any[]=[];

  ngOnInit() {
    this.GetProformaInvoicedata();
    this.GetProformaInvoiceProductdata();

    this.proformainvoiceconfirmform.get('proforma_invoice_payterm').valueChanges.subscribe((value: any) => {
      if (value) {
        const today = new Date();
        const proforma_invoice_due_date = new Date(today.getTime() + (value * 24 * 60 * 60 * 1000));
        const day = proforma_invoice_due_date.getDate().toString().padStart(2, '0');
        const month = (proforma_invoice_due_date.getMonth() + 1).toString().padStart(2, '0');
        const year = proforma_invoice_due_date.getFullYear().toString();

        const formattedDueDate = `${day}-${month}-${year}`;
        this.proformainvoiceconfirmform.patchValue({ proforma_invoice_due_date: formattedDueDate }, { emitEvent: false });

      }
    });

    const salesorder_gid = this.route.snapshot.paramMap.get('directorder_gid');
    this.salesorder_gid = salesorder_gid;

    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.salesorder_gid, secretKey).toString(enc.Utf8);

    let param = {
      directorder_gid: deencryptedParam
    }
    var api = 'ProformaInvoice/GetEditProformaInvoicedata';
    this.service.getparams(api, param).subscribe((result: any) => {
      this.responsedata = result;
      this.proformainvoiceproductlist = this.responsedata.editproformainvoice_list;
    });
   

    // const currentDate = new Date().toISOString().split('T')[0];
    // this.proformainvoiceconfirmform.get('proforma_invoice_date')?.setValue(currentDate);

    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);

  }
  getCurrentDate(): string {
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = today.getFullYear();

    return dd + '-' + mm + '-' + yyyy;
  }

  constructor(private router: Router, private route: ActivatedRoute, private fb: FormBuilder, private service: SocketService, private ToastrService: ToastrService) {

    this.proformainvoiceconfirmform = new FormGroup({
      proforma_salesorder_gid: new FormControl(''),
      proforma_invoice_refno: new FormControl(''),
      proforma_invoice_date: new FormControl(this.getCurrentDate()),
      proforma_invoice_payterm: new FormControl(''),
      proforma_invoice_due_date: new FormControl(''),
      proforma_invoice_raised_by: new FormControl(''),

      proforma_branch: new FormControl(''),
      proforma_cust_ref_no: new FormControl(''),
      proforma_so_ref_no: new FormControl(''),
      proforma_so_date: new FormControl(''),
      proforma_customer_name: new FormControl(''),
      proforma_contact_person: new FormControl(''),
      proforma_contact_no: new FormControl(''),
      proforma_email_address: new FormControl(''),
      proforma_address: new FormControl(''),
      proforma_remarks: new FormControl(''),

      proforma_sales_person: new FormControl(''),
      proforma_expected_start_date: new FormControl(''),
      proforma_estimated_arrival_time: new FormControl(''),
      proforma_freight_terms: new FormControl(''),
      proforma_payment_terms: new FormControl(''),
      proforma_currency: new FormControl(''),
      proforma_exchange_rate: new FormControl(''),

      proforma_product_gid: new FormControl(''),
      proforma_product_name: new FormControl(''),
      proforma_product_code: new FormControl(''),
      proforma_product_group: new FormControl(''),
      proforma_product_description: new FormControl(''),
      proforma_unit: new FormControl(''),
      proforma_unit_price: new FormControl(''),
      proforma_billed_quantity: new FormControl(''),
      proforma_mrp: new FormControl(''),
      proforma_discountpercentage: new FormControl(''),
      proforma_discountamount: new FormControl(''),
      proforma_taxname1: new FormControl(''),
      proforma_taxamount1: new FormControl(''),
      proforma_taxname2: new FormControl(''),
      proforma_taxamount2: new FormControl(''),
      proforma_total_amount: new FormControl(''),

      proforma_net_amount: new FormControl(''),
      proforma_overall_tax: new FormControl(''),
      proforma_total_amount_tax: new FormControl(''),
      proforma_maximum_addon_amount: new FormControl(''),
      proforma_maximum_addon_discount_amount: new FormControl(''),
      proforma_freight_charges: new FormControl(''),
      proforma_buy_back_scrap_charges: new FormControl(''),
      proforma_packing_forwarding_charges: new FormControl(''),
      proforma_insurance_charges: new FormControl(''),
      proforma_roundoff: new FormControl(''),
      proforma_grandtotal: new FormControl(''),
      proforma_advance_percentage: new FormControl(''),
      proforma_advance_amount: new FormControl(''),
      proforma_advance_roundoff: new FormControl(''),
      proforma_termsandconditions: new FormControl(''),
      template_name: new FormControl(''),
      termsandconditions: new FormControl(''),
    })
  }

  // get proformainvoicedateControl() {
  //   return this.proformainvoiceconfirmform.get('proforma_invoice_date');
  // }

  GetProformaInvoicedata() {
    const salesorder_gid = this.route.snapshot.paramMap.get('directorder_gid');
    this.salesorder_gid = salesorder_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.salesorder_gid, secretKey).toString(enc.Utf8);
    let param = {
      directorder_gid: deencryptedParam
    }

    var api = 'ProformaInvoice/GetProformaInvoicedata';
    const proforma_invoice_date = new Date(this.proformainvoiceconfirmform.proforma_invoice_date);
    const timezoneOffset = proforma_invoice_date.getTimezoneOffset() * 60000;
    const adjustedDate = new Date(proforma_invoice_date.getTime() - timezoneOffset);

    function formatDate(date: Date) {
      const day = String(date.getDate()).padStart(2, '0');
      const month = String(date.getMonth() + 1).padStart(2, '0');
      const year = date.getFullYear();
      return `${day}-${month}-${year}`;
    }
    const formattedDate = formatDate(adjustedDate);


    this.service.getparams(api, param).subscribe((result: any) => {
      this.responsedata = result;
      this.proformainvoicedata = result;
      this.proformainvoiceconfirmform.get("proforma_salesorder_gid")?.setValue(this.proformainvoicedata.salesorder_gid);
      this.proformainvoiceconfirmform.get("proforma_so_date")?.setValue(this.proformainvoicedata.salesorder_date);
      this.proformainvoiceconfirmform.get("proforma_remarks")?.setValue(this.proformainvoicedata.product_remarks);
      this.proformainvoiceconfirmform.get("proforma_sales_person")?.setValue(this.proformainvoicedata.user_firstname);
      this.proformainvoiceconfirmform.get("proforma_expected_start_date")?.setValue(this.proformainvoicedata.start_date);
      this.proformainvoiceconfirmform.get("proforma_estimated_arrival_time")?.setValue(this.proformainvoicedata.end_date);
      this.proformainvoiceconfirmform.get("proforma_payment_terms")?.setValue(this.proformainvoicedata.payment_terms);
      this.proformainvoiceconfirmform.get("proforma_currency")?.setValue(this.proformainvoicedata.currency_code);
      this.proformainvoiceconfirmform.get("proforma_branch")?.setValue(this.proformainvoicedata.branch_name);
      this.proformainvoiceconfirmform.get("proforma_so_ref_no")?.setValue(this.proformainvoicedata.so_referenceno1);
      this.proformainvoiceconfirmform.get("proforma_cust_ref_no")?.setValue(this.proformainvoicedata.so_referencenumber);
      this.proformainvoiceconfirmform.get("proforma_freight_terms")?.setValue(this.proformainvoicedata.freight_terms);
      this.proformainvoiceconfirmform.get("proforma_product_name")?.setValue(this.proformainvoicedata.product_name);
      this.proformainvoiceconfirmform.get("proforma_product_group")?.setValue(this.proformainvoicedata.productgroup_name);
      this.proformainvoiceconfirmform.get("proforma_customer_name")?.setValue(this.proformainvoicedata.customer_name);
      this.proformainvoiceconfirmform.get("proforma_contact_person")?.setValue(this.proformainvoicedata.customer_contact_person);
      this.proformainvoiceconfirmform.get("proforma_contact_no")?.setValue(this.proformainvoicedata.mobile);
      this.proformainvoiceconfirmform.get("proforma_email_address")?.setValue(this.proformainvoicedata.customer_email);
      this.proformainvoiceconfirmform.get("proforma_address")?.setValue(this.proformainvoicedata.customer_address);
      this.proformainvoiceconfirmform.get("proforma_product_gid")?.setValue(this.proformainvoicedata.product_gid);
      this.proformainvoiceconfirmform.get("proforma_total_amount")?.setValue(this.proformainvoicedata.product_price);
      this.proformainvoiceconfirmform.get("proforma_product_description")?.setValue(this.proformainvoicedata.display_field);
      this.proformainvoiceconfirmform.get("proforma_billed_quantity")?.setValue(this.proformainvoicedata.qty_quoted);
      this.proformainvoiceconfirmform.get("proforma_discountpercentage")?.setValue(this.proformainvoicedata.margin_percentage);
      this.proformainvoiceconfirmform.get("proforma_discountamount")?.setValue(this.proformainvoicedata.margin_amount);
      this.proformainvoiceconfirmform.get("proforma_taxname1")?.setValue(this.proformainvoicedata.tax_name);
      this.proformainvoiceconfirmform.get("proforma_taxamount1")?.setValue(this.proformainvoicedata.tax_amount);
      this.proformainvoiceconfirmform.get("proforma_taxname2")?.setValue(this.proformainvoicedata.tax_name2);
      this.proformainvoiceconfirmform.get("proforma_taxamount2")?.setValue(this.proformainvoicedata.tax_amount2);
      this.proformainvoiceconfirmform.get("proforma_total_amount")?.setValue(this.proformainvoicedata.total_amount);
      this.proformainvoiceconfirmform.get("proforma_overall_tax")?.setValue(this.proformainvoicedata.tax_amount);
      this.proformainvoiceconfirmform.get("proforma_total_amount_tax")?.setValue(this.proformainvoicedata.total_amount);
      this.proformainvoiceconfirmform.get("proforma_net_amount")?.setValue(this.proformainvoicedata.total_price);
      this.proformainvoiceconfirmform.get("proforma_maximum_addon_amount")?.setValue(this.proformainvoicedata.addon_charge);
      this.proformainvoiceconfirmform.get("proforma_maximum_addon_discount_amount")?.setValue(this.proformainvoicedata.additional_discount);
      this.proformainvoiceconfirmform.get("proforma_freight_charges")?.setValue(this.proformainvoicedata.freight_charges);
      this.proformainvoiceconfirmform.get("proforma_buy_back_scrap_charges")?.setValue(this.proformainvoicedata.buyback_charges);
      this.proformainvoiceconfirmform.get("proforma_packing_forwarding_charges")?.setValue(this.proformainvoicedata.packing_charges);
      this.proformainvoiceconfirmform.get("proforma_insurance_charges")?.setValue(this.proformainvoicedata.insurance_charges);
      this.proformainvoiceconfirmform.get("proforma_roundoff")?.setValue(this.proformainvoicedata.roundoff);
      this.proformainvoiceconfirmform.get("proforma_grandtotal")?.setValue(this.proformainvoicedata.Grandtotal);
      this.proformainvoiceconfirmform.get("proforma_advance_amount")?.setValue(this.proformainvoicedata.total_amount);
      this.proformainvoiceconfirmform.get("proforma_termsandconditions")?.setValue(this.proformainvoicedata.termsandconditions);
      this.proformainvoiceconfirmform.get("proforma_invoice_raised_by")?.setValue(this.proformainvoicedata.customercontact_name);
      this.proformainvoiceconfirmform.get("proforma_exchange_rate")?.setValue(this.proformainvoicedata.exchange_rate);

    });
  }

  GetProformaInvoiceProductdata() {
    debugger
    var url = 'ProformaInvoice/GetProformaInvoiceProductsdata'
    const salesorder_gid = this.route.snapshot.paramMap.get('directorder_gid');
    this.salesorder_gid = salesorder_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.salesorder_gid, secretKey).toString(enc.Utf8);
    let param = {
      directorder_gid: deencryptedParam
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.proformainvoicelist1 = result.ProformaInvoice_Productlist;
    });
  }

  proformainvoicesubmit() {
    

    var api = 'ProformaInvoice/ProformaInvoiceSubmit';
    this.service.post(api, this.proformainvoiceconfirmform.value).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
      }
      else {
        this.router.navigate(['/einvoice/ProformaInvoice']);
        this.ToastrService.success(result.message)
      }
    },
    );
  }

  back() {
    this.router.navigate(['/einvoice/ProformaInvoice']);
  }
  
  finaltotal() {

    const frieghtcharges = isNaN(this.freightcharges) ? 0 : +this.freightcharges;
    const buybackcharges = isNaN(this.buybackscrap) ? 0 : +this.buybackscrap;
    const insurancecharges = isNaN(this.insurancecharges) ? 0 : +this.insurancecharges;
    const packing_charges = isNaN(this.forwardingcharges) ? 0 : +this.forwardingcharges;
    const roundoff = isNaN(this.roundoff) ? 0 : +this.roundoff;
    const Totalamountwithtax = parseFloat(this.Totalamountwithtax.replace(/,/g, ''));

    this.Grandtotal = (((Totalamountwithtax) + (frieghtcharges) + (packing_charges) + (insurancecharges) + (roundoff) - (buybackcharges)));
    this.Grandtotal = Math.round(+(this.Grandtotal));
    this.proformaadvanceamount = Math.round(+(this.Grandtotal));
  }

  totalvalue() {

    const proformaadvancepercentage = isNaN(this.proformainvoiceconfirmform.value.proforma_advance_percentage) ? 0 : this.proformainvoiceconfirmform.value.proforma_advance_percentage;
    this.proformaadvanceamount = (this.Grandtotal * proformaadvancepercentage) / 100;
  }
  overalltotal() {
    
    if (this.proformainvoiceconfirmform.value.proforma_advance_roundoff == 0 || this.proformainvoiceconfirmform.value.proforma_advance_roundoff == null) {
      const proformaadvancepercentage = isNaN(this.proformainvoiceconfirmform.value.proforma_advance_percentage) ? 0 : this.proformainvoiceconfirmform.value.proforma_advance_percentage;
      this.proformaadvanceamount = (this.Grandtotal * proformaadvancepercentage) / 100;
    }
    else {
      const proformaadvanceroundoff = isNaN(parseFloat(this.proformainvoiceconfirmform.value.proforma_advance_roundoff)) ? 0 : parseFloat(this.proformainvoiceconfirmform.value.proforma_advance_roundoff);
      this.proformaadvanceamount = this.proformainvoiceconfirmform.value.proforma_advance_amount + (+proformaadvanceroundoff);
    }
  }
}
