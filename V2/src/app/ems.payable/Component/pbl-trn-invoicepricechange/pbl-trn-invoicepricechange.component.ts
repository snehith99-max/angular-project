import { Component, numberAttribute } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormRecord, Validators } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { DefaultGlobalConfig, ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { __values } from 'tslib';
@Component({
  selector: 'app-pbl-trn-invoicepricechange',
  templateUrl: './pbl-trn-invoicepricechange.component.html',
  styleUrls: ['./pbl-trn-invoicepricechange.component.scss']
})
export class PblTrnInvoicepricechangeComponent {
  InvoiceForm: FormGroup | any;
  showInput: boolean = false;
  inputValue: string = ''
  GrnDetails: any;
  vendor_gid_key: any;
  purchaseorder_gid_key: any;
  grn_gid_key: any;
  response_data: any;
  addoncharge:number = 0;
  InvoicePurchaseOrderGrnDetails_list: any[] = [];
  InvoiceVendorDetails: any[] = [];
  mdlpurchasetype: any;
  responsedata: any;
  net_amount: number = 0;
  qty_delivered: number = 0;
  addon_amount: number = 0;
  netamount: number = 0;
  totalamount: number = 0;
  tax_amount4: number = 0;
  frieghtcharges: number = 0;
  additional_discount: number = 0;
  buybackorscrap: number = 0;
  freightcharges: number = 0;
  discount_amount: number = 0;
  packing_charges: number = 0;
  insurance_charges: number = 0;
  roundoff: number = 0;
  grandtotal: number = 0;
  exchange_rate: number =0;
  Product_amount: number =0;
  net_price: number=0;
  taxpercentage: any;
  tax4_list:any;
  tax_list:any;
  totalproduct_amount: number =0;
  invoicediscountamount: number = 0;
  insurancecharges: number = 0;
  forwardingCharges: number = 0;
  mdlTaxName4:any;
  allchargeslist: any[] = [];
  GetTaxSegmentList: any;
  mdlTaxSegment: any;
  taxsegment_gid: any;
  prod_name: any;
  taxseg_tax: any;
  overalltax: any;
  tax_name: any;
  tax_amount: number = 0;
  taxseg_tax_amount: any;
  taxGids: any;
  taxseg_taxgid: any;
  totTaxAmountPerQty: any;
  po_tax_amount: any;
  decimalPipe: any;
  termsandconditions:any;
  purchaseordergid:any;
  total_amount: number = 0;
purchaseorder_list:any[]=[];
config: AngularEditorConfig = {
  editable: true,
  spellcheck: true,
  height: '20rem',
  minHeight: '0rem',
  placeholder: 'Enter text here...',
  translate: 'no',
};
  constructor(private formBuilder: FormBuilder,
    private ToastrService: ToastrService,
    private route: ActivatedRoute,
    private router: Router,
    public service: SocketService,
    public NgxSpinnerService: NgxSpinnerService) {
    this.InvoiceForm = new FormGroup({
      inv_ref_no: new FormControl(''),
      invoice_date:  new FormControl(this.getCurrentDate(), Validators.required),
      Vendor_ref_no: new FormControl(''),
      purchasetype_name: new FormControl(''),
      branch_name: new FormControl(''),
      vendor_details: new FormControl(''),
      purchaseorder_date: new FormControl(''),
      payment_days: new FormControl(''),
      payment_date: new FormControl(''),
      employee_emailid: new FormControl(''),
      employee_mobileno: new FormControl(''),
      employee_name: new FormControl(''),
      vendor_companyname: new FormControl(''),
      contactperson_name: new FormControl(''),
      contact_telephonenumber: new FormControl(''),
      vendor_address: new FormControl(''),
      fax: new FormControl(''),
      exchange_rate: new FormControl(''),
      currency_code: new FormControl(''),
      product_name: new FormControl(''),
      product_code: new FormControl(''),
      productuom_name: new FormControl(''),
      qty_delivered: new FormControl(''),
      product_price: new FormControl(''),
      discount_amount: new FormControl(''),
      additional_discount: new FormControl(''),
      discount_percentage: new FormControl(''),
      tax_name: new FormControl(''),
      tax_amount: new FormControl(''),
      purchaseorder_gid: new FormControl(''),
      addoncharge: new FormControl(''),
    invoice_remarks : new FormControl(''),
      product_remarks: new FormControl(''),
      tax_name4: new FormControl(''),
      totalamount: new FormControl(''),
      grandtotal: new FormControl(''),
      freightcharges: new FormControl(''),
      packing_charges: new FormControl(''),
      insurance_charges: new FormControl(''),
      roundoff: new FormControl(''),
      tax_amount4: new FormControl(''),    
      priority_n: new FormControl('N', Validators.required),
      vendor_gid: new FormControl(''),
      address1: new FormControl(''),
      payment_terms: new FormControl(''),
      dispatch_mode: new FormControl(''),
      po_covernote: new FormControl(''),
      termsandconditions: new FormControl(''),
      netamount: new FormControl(''),
      total_amount: new FormControl(''),
      invoice_ref_no: new FormControl(''),
      invoice_amount: new FormControl(''),
      due_date:  new FormControl(this.getCurrentDate(), Validators.required),
    });
  }
  getCurrentDate(): string {
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = today.getFullYear();
    return yyyy + '-' + mm + '-' + dd;
}
  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',    
    };
    flatpickr('.date-picker', options);
    debugger
    const purchaseorder_gid = this.route.snapshot.paramMap.get('purchaseorder_gid');
    const secretKey = 'storyboarderp';
    this.purchaseorder_gid_key = purchaseorder_gid;
    const purchaseorder_gid_1 = AES.decrypt(this.purchaseorder_gid_key, secretKey).toString(enc.Utf8);;
    this.purchaseordergid = AES.decrypt(this.purchaseorder_gid_key, secretKey).toString(enc.Utf8);;
    this.GetViewPurchaseOrderSummary(purchaseorder_gid_1);
  }
  showTextBox(event: Event) {
    const target = event.target as HTMLInputElement;
    this.showInput = target.value === 'Y';
  }
  GetViewPurchaseOrderSummary(purchaseorder_gid_1: any) {
    debugger
    var url='PmrTrnPurchaseOrder/GetViewPurchaseOrderSummary'
    let param = {
      purchaseorder_gid : purchaseorder_gid_1 
    }
    this.service.getparams(url,param).subscribe((result:any)=>{
    this.purchaseorder_list = result.GetViewPurchaseOrder;
    this.InvoiceForm.get("vendor_companyname")?.setValue(this.purchaseorder_list[0].vendor_companyname);
    this.InvoiceForm.get("branch_name")?.setValue(this.purchaseorder_list[0].branch_name);
    this.termsandconditions=this.purchaseorder_list[0].termsandconditions;
    this.netamount=this.purchaseorder_list[0].netamount;
    this.addon_amount=this.purchaseorder_list[0].addon_amount;
    this.additional_discount=this.purchaseorder_list[0].additional_discount;
    this.tax_name=this.purchaseorder_list[0].tax_name;
    this.overalltax=this.purchaseorder_list[0].overalltax;
    const emailId = this.purchaseorder_list[0].email_id;
    const contactTelephonenumber = this.purchaseorder_list[0].contact_telephonenumber;
    const gstNo = this.purchaseorder_list[0].tax_number;
    const vendorDetails = `${emailId}\n${contactTelephonenumber}\n${gstNo}`;
    this.InvoiceForm.get("vendor_details")?.setValue(vendorDetails);
    this.InvoiceForm.get("address1")?.setValue(this.purchaseorder_list[0].vendor_address);
    this.InvoiceForm.get("currency_code")?.setValue(this.purchaseorder_list[0].currency_code);
    this.InvoiceForm.get("exchange_rate")?.setValue(this.purchaseorder_list[0].exchange_rate);
    this.InvoiceForm.get("delivery_terms")?.setValue(this.purchaseorder_list[0].delivery_terms);
    this.InvoiceForm.get("payment_terms")?.setValue(this.purchaseorder_list[0].payment_terms);
    this.InvoiceForm.get("dispatch_mode")?.setValue(this.purchaseorder_list[0].mode_despatch);
    this.InvoiceForm.get("invoice_remarks")?.setValue(this.purchaseorder_list[0].po_covernote);
    });
  }
  updateProductTotal(data: any): void {
    data.product_total = data.qyt_unit * data.invoice_amount;
  }
  close() {
    this.router.navigate(['/payable/PmrTrnInvoiceAddselect'])
  }
  onpricechange(){
debugger
    const secretKey = 'storyboarderp';
     const param1 = this.purchaseordergid;
     const purchaseorder_gid = AES.encrypt(param1,secretKey).toString();
     this.router.navigate(['/payable/PblTrnInvoicepricechange', purchaseorder_gid])  
     }
  onSubmit() {
    debugger
    this.NgxSpinnerService.show();
    if(this.InvoiceForm.value.invoice_ref_no==""||this.InvoiceForm.value.invoice_ref_no == null || this.InvoiceForm.value.invoice_ref_no == undefined 
       )
    {
     window.scrollTo({
       top: 0, 
     });
     this.ToastrService.warning('Kindly Fill all the Mandatory Fields!');
     this.NgxSpinnerService.hide();
     return
    }
    var params = {
      inv_ref_no: this.InvoiceForm.value.invoice_ref_no,
      invoice_date: this.InvoiceForm.value.invoice_date,
      GetInvoiceGrnDetails_list: this.purchaseorder_list,
    }
    var api6 = 'PblInvoiceGrnDetails/Postpricerequest'
    this.service.postparams(api6, params).pipe().subscribe((result: any) => {
        this.response_data = result;
        if (result.status == false) {
          window.scrollTo({
            top: 0, 
          });
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
        }
        else {
          window.scrollTo({
            top: 0, 
          });
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.router.navigate(['/payable/PmrTrnInvoice']);
        }
      });
      this.NgxSpinnerService.hide();
  }

}


