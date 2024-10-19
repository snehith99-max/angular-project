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
  selector: 'app-pbl-trn-service-invoice-add',
  templateUrl: './pbl-trn-service-invoice-add.component.html',
  styleUrls: ['./pbl-trn-service-invoice-add.component.scss']
})
export class PblTrnServiceInvoiceAddComponent {

  InvoiceForm: FormGroup | any;
  showInput: boolean = false;
  inputValue: string = ''
  GrnDetails: any;
  vendor_gid_key: any;
  purchaseorder_gid_key: any;
  grn_gid_key: any;
  response_data: any;
  terms_list: any[] = [];
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
  producttotalamount: any;
  tax4_list:any;
  purchaseorder:any;
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
  mdlTerms: any;
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
  showPriceChangeButton: boolean = false;
  total_amount: number = 0;
  purchaseorderproduct_list:any[]=[];
  purchaseorder_list:any[]=[];
  config: AngularEditorConfig = {
  editable: true,
  spellcheck: true,
  height: '20rem',
  minHeight: '0rem',
  placeholder: 'Enter text here...',
  translate: 'no',
};
purchasetype_list : any[]=[];
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
      delivery_terms: new FormControl(''),
      roundoff: new FormControl(''),
      tax_amount4: new FormControl(''),    
      priority_n: new FormControl('N', Validators.required),
      vendor_gid: new FormControl(''),
      address1: new FormControl(''),
      shipping_address: new FormControl(''),
      payment_terms: new FormControl(''),
      dispatch_mode: new FormControl(''),
      po_covernote: new FormControl(''),
      termsandconditions: new FormControl(''),
      netamount: new FormControl(''),
      total_amount: new FormControl(''),
      invoice_ref_no: new FormControl(''),
      billing_mail: new FormControl(''),
      due_date:  new FormControl(this.getCurrentDate(), Validators.required),
      template_content : new FormControl(''),
      tax_gid : new FormControl(''),
  

     
    });
  }
  getCurrentDate(): string {
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = today.getFullYear();
    return dd + '-' + mm + '-' + yyyy;
}
  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',    
    };
    flatpickr('.date-picker', options);
    debugger
    
    this.purchaseorder= this.route.snapshot.paramMap.get('purchaseorder_gid');
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.purchaseorder,secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)   
    this.purchaseordergid = deencryptedParam 
    this.GetViewPurchaseOrderSummary(deencryptedParam);
    this.GetViewPurchaseOrderProduct(deencryptedParam);
    
    var url = 'PmrTrnPurchaseOrder/GetTax4Dtl'
    this.service.get(url).subscribe((result:any)=>{
      this.tax4_list = result.GetTax4Dtl;
     });
     var api = 'PmrTrnPurchaseOrder/GetTax';
     this.service.get(api).subscribe((result: any) => {
     this.responsedata = result;
     this.tax_list = result.GetTax;
     });


     var api = 'PmrMstPurchaseType/GetpurchaseType';
    this.service.get(api).subscribe((result: any) => {
    this.responsedata = result;
    this.purchasetype_list = result.GetpurchaseType_List;
    });

    this.InvoiceForm.get('branch_name')?.valueChanges.subscribe((value: string) => {
      if (value) {
        this.OnChangeBranch();
      }
    });

    var api = 'PmrTrnDirectInvoice/PblGetAllChargesConfig';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.allchargeslist = this.responsedata.salesconfigalllist;
      this.addoncharge = this.allchargeslist[0].flag;
      this.invoicediscountamount = this.allchargeslist[1].flag;
      this.frieghtcharges = this.allchargeslist[2].flag;
      this.forwardingCharges = this.allchargeslist[3].flag;
      this.insurancecharges = this.allchargeslist[4].flag;
      if (this.allchargeslist[0].flag == 'Y') {
        this.addoncharge = 0;
      } else {
        this.addoncharge = this.allchargeslist[0].flag;
        this.InvoiceForm.get("addoncharge")?.setValue(0);
      }

      if (this.allchargeslist[1].flag == 'Y') {
        this.additional_discount = 0;
      } else {
        this.additional_discount = this.allchargeslist[1].flag;
        this.InvoiceForm.get("additional_discount")?.setValue(0);
      }

      if (this.allchargeslist[2].flag == 'Y') {
        this.frieghtcharges = 0;
      } else {
        this.frieghtcharges = this.allchargeslist[2].flag;
        this.InvoiceForm.get("freightcharges")?.setValue(0);
      }

      if (this.allchargeslist[3].flag == 'Y') {
        this.forwardingCharges = 0;
      } else {
        this.forwardingCharges = this.allchargeslist[3].flag;
        this.InvoiceForm.get("forwardingCharges")?.setValue(0);
      }

      if (this.allchargeslist[4].flag == 'Y') {
        this.insurancecharges = 0;
      } else {
        this.insurancecharges = this.allchargeslist[4].flag;
        this.InvoiceForm.get("insurancecharges")?.setValue(0);
      }
    });

  }
  get purchase_type(){
    return this.InvoiceForm.get("purchasetype_name");
  }
  showTextBox(event: Event) {
    const target = event.target as HTMLInputElement;
    this.showInput = target.value === 'Y';
  }

  OnChangeBranch() {
    debugger
    let branch_gid = this.InvoiceForm.get("branch_gid")?.value;
    let param = {
      branch_gid: branch_gid
    }
    var url = 'PmrTrnPurchaseOrder/GetOnChangeBranch';
    this.service.getparams(url, param).subscribe((result: any) => {
        const address1 = result.GetBranch[0].address1;
          const zip_code = result.GetBranch[0].postal_code;
          const city = result.GetBranch[0].city;
          const state = result.GetBranch[0].state;
          result.addressdetails = `${address1}\n${city}\n${state}\n${zip_code}`;
      this.InvoiceForm.get("shipping_address")?.setValue(result.addressdetails);
    
    });

  }
  onclearBranch(){
    this.InvoiceForm='';

  }


  GetOnChangeTerms() {
    let termsconditions_gid = this.InvoiceForm.value.template_name;
    let param = {
      termsconditions_gid: termsconditions_gid
    }
    var url = 'PmrTrnPurchaseQuotation/GetOnChangeTerms';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.InvoiceForm.get("template_content")?.setValue(result.terms_list[0].termsandconditions);
      this.InvoiceForm.value.termsconditions_gid = result.terms_list[0].termsconditions_gid
    });
    var api = 'PmrTrnPurchaseQuotation/GetTermsandConditions';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.terms_list = this.responsedata.GetTermsandConditions
    });
 
  }
  OnChangeTaxAmount4() {
    debugger
    let tax_gid = this.InvoiceForm.get('tax_name4')?.value;   
    this.taxpercentage = this.getDimensionsByFilter(tax_gid);
    let tax_percentage = this.taxpercentage[0].percentage;
    this.tax_amount4 = +(tax_percentage * this.producttotalamount / 100).toFixed(2);
    this.total_amount = +((+this.producttotalamount) + (+this.tax_amount4));  
    this.total_amount = +this.total_amount.toFixed(2);
    // this.grandtotal = ((this.total_amount) + (+this.tax_amount4) + (+this.addoncharge) + (+this.freightcharges) +  (+this.packing_charges) + (+this.insurance_charges) + (+this.roundoff) + (+this.totalamount)  - (+this.additional_discount));
    // Convert NaN to 0 for each field
    this.total_amount = isNaN(this.total_amount) ? 0 : this.total_amount;
    this.tax_amount4 = isNaN(this.tax_amount4) ? 0 : this.tax_amount4;
    this.addoncharge = isNaN(this.addoncharge) ? 0 : this.addoncharge;
    this.freightcharges = isNaN(this.freightcharges) ? 0 : this.freightcharges;
    this.packing_charges = isNaN(this.packing_charges) ? 0 : this.packing_charges;
    this.insurance_charges = isNaN(this.insurance_charges) ? 0 : this.insurance_charges;
    this.roundoff = isNaN(this.roundoff) ? 0 : this.roundoff;
    this.totalamount = isNaN(this.totalamount) ? 0 : this.totalamount;
    this.additional_discount = isNaN(this.additional_discount) ? 0 : this.additional_discount;

// Calculate the grand total
this.grandtotal = this.total_amount + (+this.tax_amount4) + (+this.addoncharge) + (+this.freightcharges) +
    (+this.packing_charges) + (+this.insurance_charges) + (+this.roundoff) + (+this.totalamount) - (+this.additional_discount);

    }
    getDimensionsByFilter(id: any) {
      return this.tax_list.filter((x: { tax_gid: any; }) => x.tax_gid === id);
    }
  OnClearOverallTax() {
    this.tax_amount4=0;
    this.total_amount = +((+this.producttotalamount) + (+this.tax_amount4));  
    this.total_amount = +this.total_amount.toFixed(2);
     this.grandtotal = ((this.total_amount) + (+this.addoncharge) + (+this.freightcharges) +  (+this.packing_charges) + (+this.insurance_charges) + (+this.roundoff) + (+this.totalamount)  - (+this.additional_discount));
    this.grandtotal =+this.grandtotal.toFixed(2);
  }
 

  close() {
    this.router.navigate(['/payable/PmrTrnInvoiceAddselect'])
  }
  onpricechange(){

    const secretKey = 'storyboarderp';
     const param1 = this.purchaseordergid;
     const purchaseorder_gid = AES.encrypt(param1,secretKey).toString();
     this.router.navigate(['/payable/PblTrnInvoicepricechange', purchaseorder_gid])  
     }

     finaltotal() {
      
      const addoncharge = isNaN(this.InvoiceForm.value.addoncharge) ? 0 : +this.InvoiceForm.value.addoncharge;
      const freightcharges = isNaN(this.InvoiceForm.value.freightcharges) ? 0 : +this.InvoiceForm.value.freightcharges;
      const packing_charges = isNaN(this.InvoiceForm.value.packing_charges) ? 0 : +this.InvoiceForm.value.packing_charges;
      const insurance_charges = isNaN(this.InvoiceForm.value.insurance_charges) ? 0 : +this.InvoiceForm.value.insurance_charges;
      const roundoff = isNaN(this.InvoiceForm.value.roundoff) ? 0 : +this.InvoiceForm.value.roundoff;
      const additional_discount = isNaN(this.InvoiceForm.value.additional_discount) ? 0 : +this.InvoiceForm.value.additional_discount;
      const tax_amount4 = isNaN(this.InvoiceForm.value.tax_amount4) ? 0 : +this.InvoiceForm.value.tax_amount4;
      const totalamount = isNaN(this.producttotalamount) ? 0 : +this.producttotalamount;
      // Log values for debugging
      console.log("Addon Charge:", addoncharge);
      console.log("Freight Charges:", freightcharges);
      console.log("Packing Charges:", packing_charges);
      console.log("Insurance Charges:", insurance_charges);
      console.log("Round Off:", roundoff);
      console.log("Additional Discount:", additional_discount);
      console.log("Tax Amount4:", tax_amount4);
      console.log("totalamount:", totalamount);
      console.log(this.producttotalamount)
  
      // Calculate the grand total
      this.grandtotal = totalamount + tax_amount4 + addoncharge + freightcharges +
          packing_charges + insurance_charges + roundoff - additional_discount;
  
      // Log grand total for debugging
      console.log("Grand Total:", this.grandtotal);
  
      // Ensure the grand total is a valid number with 2 decimal places
      this.grandtotal = isNaN(this.grandtotal) ? 0 : +(this.grandtotal).toFixed(2);
  }
  
  onSubmit() {
    // if(this.InvoiceForm.value.invoice_ref_no==""||this.InvoiceForm.value.invoice_ref_no == null || this.InvoiceForm.value.invoice_ref_no == undefined 
    //    )
    // {
    //  window.scrollTo({
    //    top: 0, 
    //  });
    //  this.ToastrService.warning('Kindly Fill all the Mandatory Fields!');
    //  return
    // }
    
    var params = {
      inv_ref_no: this.InvoiceForm.value.invoice_ref_no,
      invoice_date: this.InvoiceForm.value.invoice_date,
      invoice_gid: this.InvoiceForm.value.invoice_gid,
      branch_name: this.purchaseorder_list[0].branch_name,
      Vendor_ref_no :  this.InvoiceForm.value.Vendor_ref_no,
      // purchaseorder_gid: this.purchaseorder_list[0].purchaseorder_gid,
      purchaseorder_date: this.purchaseorder_list[0].purchaseorder_date,
      payment_days: this.purchaseorder_list[0].payment_days,
      payment_date: this.purchaseorder_list[0].payment_date,
      vendor_companyname: this.purchaseorder_list[0].vendor_companyname,
      contactperson_name: this.InvoiceForm.contactperson_name,
      contact_telephonenumber: this.InvoiceForm.contact_telephonenumber,
      exchange_rate: this.InvoiceForm.value.exchange_rate,
      invoice_remarks: this.InvoiceForm.value.invoice_remarks,
      priority_n: this.InvoiceForm.value.priority_n,
      purchasetype_name: this.InvoiceForm.value.purchasetype_name,
      product_remarks: this.InvoiceForm.value.product_remarks,
      totalamount: this.InvoiceForm.value.totalamount,
      netamount: this.InvoiceForm.value.netamount,
      total_amount: this.InvoiceForm.value.total_amount,
      tax_name4: this.InvoiceForm.value.tax_name4,
      tax_amount4: this.InvoiceForm.value.tax_amount4,
      addoncharge: this.InvoiceForm.value.addoncharge,
      freightcharges:this.InvoiceForm.value.freightcharges,
      packing_charges: this.InvoiceForm.value.packing_charges,
      insurance_charges: this.InvoiceForm.value.insurance_charges,
      additional_discount: this.InvoiceForm.value.additional_discount.toString().replace(/,/g, '').replace(/\.00$/, ''),
      roundoff: this.InvoiceForm.value.roundoff,
      grandtotal: this.InvoiceForm.value.grandtotal,
      billing_email: this.InvoiceForm.value.billing_mail,
      shipping_address: this.InvoiceForm.value.shipping_address,
      template_content: this.InvoiceForm.value.template_content,
      payment_terms: this.InvoiceForm.value.payment_terms,
      delivery_term: this.InvoiceForm.value.delivery_terms,
      dispatch_mode: this.InvoiceForm.value.dispatch_mode,
      purchase_type : this.InvoiceForm.value.purchasetype_name,
      address1 : this.InvoiceForm.value.address1,
      purchaseorder_gid :this.purchaseordergid,
    }

    this.NgxSpinnerService.show();
    var api6 = 'PblInvoiceGrnDetails/PostOverAllServiceSubmit'
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
     
  }


  GetViewPurchaseOrderSummary(purchaseorder_gid :any) {
    debugger
    var url='PblInvoiceGrnDetails/GetPurchaseServiceInvoicesummary'  
    let param = {
      purchaseorder_gid : purchaseorder_gid,

    }
    this.service.getparams(url,param).subscribe((result:any)=>{
    this.purchaseorder_list = result.GetinviocePO;
    this.InvoiceForm.get("vendor_companyname")?.setValue(this.purchaseorder_list[0].vendor_companyname);
    this.InvoiceForm.get("branch_name")?.setValue(this.purchaseorder_list[0].branch_name);
    this.termsandconditions=this.purchaseorder_list[0].termsandconditions;
    const emailId = this.purchaseorder_list[0].email_id;
    const contactTelephonenumber = this.purchaseorder_list[0].contact_telephonenumber;
    const gstNo = this.purchaseorder_list[0].tax_number;
    const vendorDetails = `${emailId}\n${contactTelephonenumber}\n${gstNo}`;
    this.InvoiceForm.get("vendor_details")?.setValue(vendorDetails);
    this.InvoiceForm.get("address1")?.setValue(this.purchaseorder_list[0].bill_to);
    this.InvoiceForm.get("shipping_address")?.setValue(this.purchaseorder_list[0].shipping_address);
    this.InvoiceForm.get("currency_code")?.setValue(this.purchaseorder_list[0].currency_code);
    this.InvoiceForm.get("exchange_rate")?.setValue(this.purchaseorder_list[0].exchange_rate);
    this.InvoiceForm.get("delivery_terms")?.setValue(this.purchaseorder_list[0].delivery_terms);
    this.InvoiceForm.get("payment_terms")?.setValue(this.purchaseorder_list[0].payment_terms);
    this.InvoiceForm.get("dispatch_mode")?.setValue(this.purchaseorder_list[0].mode_despatch);
    this.InvoiceForm.get("invoice_remarks")?.setValue(this.purchaseorder_list[0].po_covernote);
    this.InvoiceForm.get("addoncharge")?.setValue(this.purchaseorder_list[0].addon_amount);
    this.InvoiceForm.get("additional_discount")?.setValue(this.purchaseorder_list[0].additional_discount);
    this.InvoiceForm.get("freightcharges")?.setValue(this.purchaseorder_list[0].freightcharges);
    // this.InvoiceForm.get("tax_name4")?.setValue(this.purchaseorder_list[0].tax_prefix);
    this.InvoiceForm.get("tax_amount4")?.setValue(this.purchaseorder_list[0].overalltax);
    this.InvoiceForm.get("roundoff")?.setValue(this.purchaseorder_list[0].roundoff);
    //  this.InvoiceForm.get("grandtotal")?.setValue(this.purchaseorder_list[0].grandtotal);
    // this.InvoiceForm.get("netamount")?.setValue(this.purchaseorder_list[0].netamount);
    this.InvoiceForm.get("tax_name4")?.setValue(this.purchaseorder_list[0].tax_gid);
    this.InvoiceForm.get("template_content")?.setValue(this.purchaseorder_list[0].termsandconditions)
    // this.InvoiceForm.get("vendor_gid")?.setValue(this.purchaseorder_list[0].vendor_gid)
    // this.showPriceChangeButton = this.purchaseorder_list.some(po => po.po_type === 'Contract PO');
    
    });
    
  }
  GetViewPurchaseOrderProduct(purchaseorder_gid: any) {
     
    var url='PblInvoiceGrnDetails/GetPurchaseServiceInvoiceproduct'  
    let param = {
      purchaseorder_gid : purchaseorder_gid
    }
    this.service.getparams(url,param).subscribe((result:any)=>{
      this.purchaseorderproduct_list = result.GetinvioceProduct;
      this.InvoiceForm.get("netamount")?.setValue(this.purchaseorderproduct_list[0].netamount)
   
      
    });
    this.GetViewPurchaseOrderSummary
  }

}


