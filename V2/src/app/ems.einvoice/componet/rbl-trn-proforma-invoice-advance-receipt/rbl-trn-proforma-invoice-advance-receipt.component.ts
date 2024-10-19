import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';



@Component({
  selector: 'app-rbl-trn-proforma-invoice-advance-receipt',
  templateUrl: './rbl-trn-proforma-invoice-advance-receipt.component.html',
  styleUrls: ['./rbl-trn-proforma-invoice-advance-receipt.component.scss']
})

export class RblTrnProformaInvoiceAdvanceReceiptComponent {
  proformaadvancerptform: FormGroup | any;
  invoice_gid: any;
  responsedata: any;
  file1!: FileList;
  allattchement: any[] = [];
  formDataObject: FormData = new FormData();
  file_name: any;
  proformainvoiceadvancedata: any;
  GetBankname_list : any[] = [];
  modeofpayment_list: any;
  productdata_list: any;
  mdlReceiptmode:any;
  Mdlbankname:any;
  paymentdate:any;
  swiftdate:any;
  cash_date:any;
  AutoIDkey: any;

 payment_mode: {payment_type :string , modeofpayment_gid: string }[]=[
    { payment_type: 'Cash', modeofpayment_gid: 'Cash' },
    { payment_type: 'Cheque', modeofpayment_gid: 'Cheque' },
    { payment_type: 'DD', modeofpayment_gid: 'DD' },
    { payment_type: 'NEFT', modeofpayment_gid: 'NEFT' },
    { payment_type: 'SWIFT', modeofpayment_gid: 'SWIFT' }
 ];


  constructor(private router: Router, private route: ActivatedRoute, private fb: FormBuilder, private service: SocketService,private ToastrService: ToastrService) { 
    this.proformaadvancerptform = new FormGroup({
      invoice_gid: new FormControl(''),
      proforma_advrpt_salesorder_gid: new FormControl(''),
      proforma_advrpt_invoice_gid: new FormControl(''),
      proforma_advrpt_invoice_ref_no: new FormControl(''),
      proforma_advrpt_invoice_date: new FormControl(''),
      proforma_advrpt_order_ref_no: new FormControl(''),
      proforma_advrpt_order_date: new FormControl(''),
      proforma_advrpt_customer_gid: new FormControl(''),
      proforma_advrpt_company_name: new FormControl(''),
      proforma_advrpt_contact_person: new FormControl(''),
      proforma_advrpt_contact_no: new FormControl(''),
      proforma_advrpt_email_address: new FormControl(''),
      proforma_advrpt_company_address: new FormControl(''),
      proforma_advrpt_department: new FormControl(''),
      proforma_advrpt_order_reference: new FormControl(''),
      proforma_advrpt_remarks: new FormControl(''),
      proforma_advrpt_branch_gid: new FormControl(''),
      proforma_advrpt_payment: new FormControl(''),
      proforma_advrpt_delivery_period: new FormControl(''),
      proforma_advrpt_net_amount: new FormControl(''),
      proforma_advrpt_order_refno: new FormControl(''),
      proforma_advrpt_addon_charges: new FormControl(''),
      proforma_advrpt_additional_discount: new FormControl(''),
      proforma_advrpt_hold_with_tax: new FormControl(''),
      proforma_advrpt_grand_total: new FormControl(''),
      proforma_advrpt_existing_advance: new FormControl(''),
      proforma_advrpt_outstanding_adv_amount: new FormControl(''),
      proforma_advrpt_advance: new FormControl(''),
      proforma_advrpt_payment_mode: new FormControl('',[Validators.required]),
      proforma_advrpt_termsandconditions: new FormControl(''),
      proforma_advrpt_product_group: new FormControl(''),
      proforma_advrpt_product_name: new FormControl(''),
      proforma_advrpt_product_unit: new FormControl(''),
      proforma_advrpt_product_quantity: new FormControl(''),
      proforma_advrpt_product_unitprice: new FormControl(''),
      proforma_advrpt_product_discount: new FormControl(''),
      proforma_advrpt_product_tax: new FormControl(''),
      proforma_advrpt_product_date: new FormControl(''),
      proforma_advrpt_product_amount: new FormControl(''),
      diff_amount_proforma: new FormControl('', [Validators.required, Validators.pattern(/^\d*\.?\d*$/)]),
      swift_amount_proforma: new FormControl('', [Validators.required, Validators.pattern(/^\d*\.?\d*$/)]),
      received_amount_proforma: new FormControl('', [Validators.required, Validators.pattern(/^\d*\.?\d*$/)]),

      transaction_refno_proforma: new FormControl(''),
      bank_name_proforma: new FormControl(''),
      paymentdate: new FormControl(''),
      branch_name_proforma: new FormControl(''),
      depositbank: new FormControl(''),
      cash_date: new FormControl(''),
      swiftdate: new FormControl(''),
      cheque_no_proforma: new FormControl(''),
      
    });
  }
    
  ngOnInit() {   
    
    

    const invoice_gid = this.route.snapshot.paramMap.get('invoice_gid');
    this.invoice_gid = invoice_gid;

    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.invoice_gid, secretKey).toString(enc.Utf8);

    let param = {
      invoice_gid: deencryptedParam
    }

    var api = 'ProformaInvoice/GetProformaInvoiceProductdata';
    this.service.getparams(api,param).subscribe((result: any) => {
      this.productdata_list = result.MdlProformaInvoiceProductdata;
    });
    this.GetProformaInvoiceAdvancedata();

    var api1 = 'ProformaInvoice/GetProformaInvoicemodeofpayment';
    this.service.get(api1).subscribe((result: any) => {
      this.modeofpayment_list = result.GetProformaInvoicemodeofpaymentlist;
    });

    var banknameapi = 'ProformaInvoice/Getdepositebankname';
    this.service.get(banknameapi).subscribe((apiresponse : any) => {
      this.GetBankname_list = apiresponse.GetBankname_list;
    });

    const options: Options = {
      dateFormat: 'd-m-Y',
    };

  flatpickr('.date-picker', options);
      
  }

  get proformaadvrptpaymentControl() {
    return this.proformaadvancerptform.get('proforma_advrpt_payment_mode');
  }
  getCurrentDate(): string {
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = today.getFullYear();

    return dd + '-' + mm + '-' + yyyy;
  }
  onChange2($event: any): void {
    this.file1 = $event.target.files;
  
    if (this.file1 != null && this.file1.length !== 0) {
      for (let i = 0; i < this.file1.length; i++) {
        this.AutoIDkey = this.generateKey();
        this.formDataObject.append(this.AutoIDkey, this.file1[i]);
         this.file_name = this.file1[i].name;
        this.allattchement.push({
          AutoID_Key: this.AutoIDkey,
          file_name: this.file1[i].name
        });
        console.log(this.file1[i]);
      }
    }
  }

  generateKey(): string {

    return `AutoIDKey${new Date().getTime()}`;
  }
  GetProformaInvoiceAdvancedata() {

    const invoice_gid = this.route.snapshot.paramMap.get('invoice_gid');
    this.invoice_gid = invoice_gid;

    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.invoice_gid, secretKey).toString(enc.Utf8);

    let param = {
      invoice_gid: deencryptedParam
    }

    var api = 'ProformaInvoice/GetProformaInvoiceAdvancedata';
    this.service.getparams(api, param).subscribe((result: any) => {
      this.responsedata = result;
      this.proformainvoiceadvancedata = result;
debugger
      this.proformaadvancerptform.get("proforma_advrpt_salesorder_gid")?.setValue(this.proformainvoiceadvancedata.salesorder_gid);
      this.proformaadvancerptform.get("proforma_advrpt_invoice_gid")?.setValue(this.proformainvoiceadvancedata.invoice_gid);
      this.proformaadvancerptform.get("proforma_advrpt_invoice_ref_no")?.setValue(this.proformainvoiceadvancedata.invoice_refno);
      this.proformaadvancerptform.get("proforma_advrpt_invoice_date")?.setValue(this.proformainvoiceadvancedata.invoice_date);
      this.proformaadvancerptform.get("proforma_advrpt_order_ref_no")?.setValue(this.proformainvoiceadvancedata.so_referenceno1);
      this.proformaadvancerptform.get("proforma_advrpt_order_date")?.setValue(this.proformainvoiceadvancedata.salesorder_date);
      this.proformaadvancerptform.get("proforma_advrpt_customer_gid")?.setValue(this.proformainvoiceadvancedata.customer_gid);
      this.proformaadvancerptform.get("proforma_advrpt_company_name")?.setValue(this.proformainvoiceadvancedata.customer_name);
      this.proformaadvancerptform.get("proforma_advrpt_contact_person")?.setValue(this.proformainvoiceadvancedata.customer_contact_person);
      this.proformaadvancerptform.get("proforma_advrpt_contact_no")?.setValue(this.proformainvoiceadvancedata.customer_mobile);
      this.proformaadvancerptform.get("proforma_advrpt_email_address")?.setValue(this.proformainvoiceadvancedata.customer_email);
      this.proformaadvancerptform.get("proforma_advrpt_company_address")?.setValue(this.proformainvoiceadvancedata.customer_address_so);
      this.proformaadvancerptform.get("proforma_advrpt_remarks")?.setValue(this.proformainvoiceadvancedata.so_remarks);
      this.proformaadvancerptform.get("proforma_advrpt_branch_gid")?.setValue(this.proformainvoiceadvancedata.branch_gid);
      this.proformaadvancerptform.get("proforma_advrpt_payment")?.setValue(this.proformainvoiceadvancedata.payment_days);
      this.proformaadvancerptform.get("proforma_advrpt_delivery_period")?.setValue(this.proformainvoiceadvancedata.delivery_days);
      this.proformaadvancerptform.get("proforma_advrpt_net_amount")?.setValue(this.proformainvoiceadvancedata.total_value);
      this.proformaadvancerptform.get("proforma_advrpt_order_refno")?.setValue(this.proformainvoiceadvancedata.so_referenceno1);
      this.proformaadvancerptform.get("proforma_advrpt_addon_charges")?.setValue(this.proformainvoiceadvancedata.addon_charge);
      this.proformaadvancerptform.get("proforma_advrpt_additional_discount")?.setValue(this.proformainvoiceadvancedata.additional_discount);
      this.proformaadvancerptform.get("proforma_advrpt_grand_total")?.setValue(this.proformainvoiceadvancedata.Grandtotal);
      this.proformaadvancerptform.get("proforma_advrpt_existing_advance")?.setValue(this.proformainvoiceadvancedata.salesorder_advance);
      this.proformaadvancerptform.get("proforma_advrpt_outstanding_adv_amount")?.setValue(this.proformainvoiceadvancedata.outstandingadvance);
      this.proformaadvancerptform.get("proforma_advrpt_termsandconditions")?.setValue(this.proformainvoiceadvancedata.termsandconditions);
      // this.proformaadvancerptform.get("proforma_advrpt_product_group")?.setValue(this.proformainvoiceadvancedata.productgroup_name);
      this.proformaadvancerptform.get("proforma_advrpt_product_name")?.setValue(this.proformainvoiceadvancedata.product_name);
      this.proformaadvancerptform.get("proforma_advrpt_product_unit")?.setValue(this.proformainvoiceadvancedata.productuom_name);
      this.proformaadvancerptform.get("proforma_advrpt_product_quantity")?.setValue(this.proformainvoiceadvancedata.qty_invoice);
      this.proformaadvancerptform.get("proforma_advrpt_product_unitprice")?.setValue(this.proformainvoiceadvancedata.product_price);
      this.proformaadvancerptform.get("proforma_advrpt_product_discount")?.setValue(this.proformainvoiceadvancedata.discount_amount);
      this.proformaadvancerptform.get("proforma_advrpt_product_tax")?.setValue(this.proformainvoiceadvancedata.tax_amount);
      this.proformaadvancerptform.get("proforma_advrpt_product_amount")?.setValue(this.proformainvoiceadvancedata.product_total);
    })
  }

  AdvrptProformaInvoiceSubmit() {
    debugger
    
    if(this.file1 != null && this.file1 != undefined){

      var api = 'ProformaInvoice/AdvrptProformaInvoiceSubmit';
      const allattchement = "" + JSON.stringify(this.allattchement) + "";
      this.formDataObject.append("file_name",allattchement);  
      this.formDataObject.append("invoice_gid",this.proformaadvancerptform.value.proforma_advrpt_invoice_gid);  
      this.formDataObject.append("invoice_ref_no",this.proformaadvancerptform.value.proforma_advrpt_invoice_ref_no);  
      this.formDataObject.append("invoice_date",this.proformaadvancerptform.value.proforma_advrpt_invoice_date);  
      this.formDataObject.append("order_ref_no",this.proformaadvancerptform.value.proforma_advrpt_order_ref_no);  
      this.formDataObject.append("order_date",this.proformaadvancerptform.value.proforma_advrpt_order_date);  
      this.formDataObject.append("company_name",this.proformaadvancerptform.value.proforma_advrpt_company_name);  
      this.formDataObject.append("contact_person",this.proformaadvancerptform.value.proforma_advrpt_contact_person);  
      this.formDataObject.append("contact_no",this.proformaadvancerptform.value.proforma_advrpt_contact_no);  
      this.formDataObject.append("email_address",this.proformaadvancerptform.value.proforma_advrpt_email_address);  
      this.formDataObject.append("company_address",this.proformaadvancerptform.value.proforma_advrpt_company_address);  
      this.formDataObject.append("department",this.proformaadvancerptform.value.proforma_advrpt_department);  
      this.formDataObject.append("proforma_advrpt_order_reference",this.proformaadvancerptform.value.proforma_advrpt_order_reference);  
      this.formDataObject.append("remarks",this.proformaadvancerptform.value.proforma_advrpt_remarks);  
      this.formDataObject.append("payment_mode",this.proformaadvancerptform.value.proforma_advrpt_payment_mode);  
      this.formDataObject.append("cheque_no_proforma",this.proformaadvancerptform.value.cheque_no_proforma);  
      this.formDataObject.append("bank_name_proforma",this.proformaadvancerptform.value.bank_name_proforma);  
      this.formDataObject.append("paymentdate",this.proformaadvancerptform.value.paymentdate);  
      this.formDataObject.append("branch_name_proforma",this.proformaadvancerptform.value.branch_name_proforma);  
      this.formDataObject.append("depositbank",this.proformaadvancerptform.value.depositbank.bank_gid);  
      this.formDataObject.append("cash_date",this.proformaadvancerptform.value.cash_date);  
      this.formDataObject.append("transaction_refno_proforma",this.proformaadvancerptform.value.transaction_refno_proforma);  
      this.formDataObject.append("swift_amount_proforma",this.proformaadvancerptform.value.swift_amount_proforma);  
      this.formDataObject.append("received_amount_proforma",this.proformaadvancerptform.value.received_amount_proforma);  
      this.formDataObject.append("swiftdate",this.proformaadvancerptform.value.swiftdate);  
      this.formDataObject.append("diff_amount_proforma",this.proformaadvancerptform.value.diff_amount_proforma);  
      this.formDataObject.append("proforma_advrpt_termsandconditions",this.proformaadvancerptform.value.proforma_advrpt_termsandconditions);  
      this.formDataObject.append("payment",this.proformaadvancerptform.value.proforma_advrpt_payment);  
      this.formDataObject.append("delivery_period",this.proformaadvancerptform.value.proforma_advrpt_delivery_period);  
      this.formDataObject.append("net_amount",this.proformaadvancerptform.value.proforma_advrpt_net_amount);  
      this.formDataObject.append("addon_charges",this.proformaadvancerptform.value.proforma_advrpt_addon_charges);  
      this.formDataObject.append("additional_discount",this.proformaadvancerptform.value.proforma_advrpt_additional_discount);  
      this.formDataObject.append("hold_with_tax",this.proformaadvancerptform.value.proforma_advrpt_hold_with_tax);  
      this.formDataObject.append("grand_total",this.proformaadvancerptform.value.proforma_advrpt_grand_total);  
      this.formDataObject.append("existing_advance",this.proformaadvancerptform.value.proforma_advrpt_existing_advance);  
      this.formDataObject.append("outstanding_adv_amount",this.proformaadvancerptform.value.proforma_advrpt_outstanding_adv_amount);  
      this.formDataObject.append("proforma_advrpt_advance",this.proformaadvancerptform.value.proforma_advrpt_advance);  
      this.formDataObject.append("salesorder_gid",this.proformaadvancerptform.value.proforma_advrpt_salesorder_gid);  
      this.formDataObject.append("branch_gid",this.proformaadvancerptform.value.proforma_advrpt_branch_gid);  
      this.formDataObject.append("customer_gid",this.proformaadvancerptform.value.proforma_advrpt_customer_gid); 

      this.service.post(api, this.formDataObject).subscribe((result: any) => {
        if (result.status == false) {
          window.location.reload();
          this.ToastrService.warning(result.message);       
        }
        else {
          this.router.navigate(['/einvoice/ProformaInvoice']);
          this.ToastrService.success(result.message);
        }
      });
    }
    else{
      
     console.log(this.proformaadvancerptform.value);
     var submitapiadvance = 'ProformaInvoice/PostSubmitAdvanceReceipt';
     this.service.post(submitapiadvance, this.proformaadvancerptform.value).subscribe((result: any) => {
      if (result.status == false) {
        window.location.reload();
        this.ToastrService.warning(result.message);       
      }
      else {
        this.router.navigate(['/einvoice/ProformaInvoice']);
        this.ToastrService.success(result.message);
      }
    });
    }
  }
  
  back() {
    this.router.navigate(['/einvoice/ProformaInvoice']);
  }
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
}
