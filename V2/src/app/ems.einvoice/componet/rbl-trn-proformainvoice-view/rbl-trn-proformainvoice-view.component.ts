import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, MaxLengthValidator, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { AES, enc } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-rbl-trn-proformainvoice-view',
  templateUrl: './rbl-trn-proformainvoice-view.component.html',
  styleUrls: ['./rbl-trn-proformainvoice-view.component.scss']
})
export class RblTrnProformainvoiceViewComponent {

  proformainvoiceeditform: FormGroup | any;
  ProformaInvoiceEditlist: any[] = [];
  ProformaInvoiceEditlist1: any[] = [];
  config: AngularEditorConfig = {
    editable: false,
    spellcheck: false,
    height: '25rem',
    minHeight: '5rem',
    width: '1230px',
    placeholder: 'Enter text here...',
    translate: 'no',
    defaultParagraphSeparator: 'p',
    defaultFontName: 'Arial',
  };
  responsedata: any;
  invoice_gid: any;
  mdlTerms:any;
  constructor(private router: Router,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private service: SocketService,
    private ToastrService: ToastrService) {
  }
  ngOnInit(): void {

    this.proformainvoiceeditform = new FormGroup({
      edit_proforma_invoice_refno: new FormControl(''),
      invoice_gid: new FormControl(''),
      edit_proforma_invoice_date: new FormControl(''),
      edit_proforma_invoice_payterm: new FormControl(''),
      edit_proforma_invoice_due_date: new FormControl(''),
      edit_proforma_invoice_so_reference_no: new FormControl(''),
      edit_proforma_invoice_raised_by: new FormControl(''),
      edit_proforma_invoice_branch: new FormControl(''),
      edit_proforma_invoice_cust_ref_no: new FormControl(''),
      edit_proforma_invoice_customer_name: new FormControl(''),
      edit_proforma_invoice_contact_person: new FormControl(''),
      edit_proforma_invoice_contact_no: new FormControl('', [Validators.pattern(/^[0-9]+$/), Validators.maxLength(10)]),
      edit_proforma_invoice_email_address: new FormControl('', [Validators.pattern('^([a-z0-9]+(?:[-.][a-z0-9]+)*)@([a-z0-9]+\\.[a-z]{2,20}(\\.[a-z]{2})?|\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\]|localhost)$')]),
      edit_proforma_invoice_address: new FormControl(''),
      edit_proforma_invoice_remarks: new FormControl(''),
      edit_proforma_invoice_sales_person: new FormControl(''),
      edit_proforma_invoice_expected_start_date: new FormControl(''),
      edit_proforma_invoice_estimated_arrival_time: new FormControl(''),
      edit_proforma_invoice_freight_terms: new FormControl(''),
      edit_proforma_invoice_payment_terms: new FormControl(''),
      edit_proforma_invoice_currency: new FormControl(''),
      edit_proforma_invoice_exchange_rate: new FormControl(''),
      edit_proforma_invoice_net_amount: new FormControl(''),
      edit_proforma_invoice_overall_tax: new FormControl(''),
      edit_proforma_invoice_total_amount_tax: new FormControl(''),
      edit_proforma_invoice_maximum_addon_amount: new FormControl(''),
      edit_proforma_invoice_maximum_addon_discount: new FormControl(''),
      edit_proforma_invoice_freight_charges: new FormControl(''),
      edit_proforma_invoice_buyback_charges: new FormControl(''),
      edit_proforma_invoice_packing_charges: new FormControl(''),
      edit_proforma_invoice_insurance_charges: new FormControl(''),
      edit_proforma_invoice_roundoff: new FormControl(''),
      edit_proforma_invoice_grandtotal: new FormControl(''),
      edit_proforma_invoice_advance_percentage: new FormControl(''),
      edit_proforma_invoice_advance_amount: new FormControl(''),
      edit_proforma_invoice_advance_roundoff: new FormControl(''),
      edit_proforma_invoice_termsandconditions: new FormControl(''),
      productgroup_name: new FormControl(''),
      product_name: new FormControl(''),
    });


    const invoice_gid = this.route.snapshot.paramMap.get('invoice_gid');
    this.invoice_gid = invoice_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.invoice_gid, secretKey).toString(enc.Utf8);
    this.summary(deencryptedParam)
    this.prodsummary(deencryptedParam)
  }
 


  summary(invoice_gid: any) {
    debugger;
    var url = 'ProformaInvoice/GetProformaInvoiceViewdata'
    let param = { invoice_gid: invoice_gid }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.ProformaInvoiceEditlist = result.ProformaInvoiceEditlist;
    });
  }
  prodsummary(invoice_gid: any) {
    debugger;
    var url = 'ProformaInvoice/GetProformaInvoiceProductsEditdata'
    let param = { invoice_gid: invoice_gid }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.ProformaInvoiceEditlist1 = result.ProformaInvoice_Productlist;
    });
  }
  back() {
    this.router.navigate(['/einvoice/ProformaInvoice']);
  }
}
