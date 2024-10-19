import { HttpClient } from '@angular/common/http';
import { Component, Renderer2 } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES, enc } from 'crypto-js';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';

@Component({
  selector: 'app-smr-trn-tcreditnoteaddselect',
  templateUrl: './smr-trn-tcreditnoteaddselect.component.html',
  styleUrls: ['./smr-trn-tcreditnoteaddselect.component.scss']
})
export class SmrTrnTcreditnoteaddselectComponent {


  invoice_gid : any;
  directinvoiceproductsummary_list:any;
  combinedFormData : FormGroup | any;
  productform : FormGroup | any;
  tax_name: any;
  tax_name2 : any;
  tax_name3 : any;
  tax_amount : any;
  tax_amount2:any;
  tax_amount3 : any;
  responsedata:any;
  cnsummary_list : any[] = [];
  cnprodsummary_list : any[] = [];
  customer_address : any;

  constructor(private http: HttpClient,
    private fb: FormBuilder, private renderer: Renderer2,
    private router: ActivatedRoute, private route: Router, private service: SocketService, private ToastrService: ToastrService, public NgxSpinnerService: NgxSpinnerService) {
    
  }
  
  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);

    const invoice_gid = this.router.snapshot.paramMap.get('invoice_gid'); 
    this.invoice_gid = invoice_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.invoice_gid, secretKey).toString(enc.Utf8);
    this.GetCnSummary(deencryptedParam);
    this.GetProdSummary(deencryptedParam);
    // Form Control
    this.combinedFormData = new FormGroup({
      invoice_gid: new FormControl(''),
      invoice_date: new FormControl(''),
      order_refno: new FormControl(''),
      invoice_refno: new FormControl(''),
      customer_name: new FormControl(''),
      customer_gid: new FormControl(''),
      customer_details: new FormControl(''),
      customer_email: new FormControl(''),
      creditnote_date : new FormControl(this.getCurrentDate(),[Validators.required]),
      remarks: new FormControl(''),
      customercontact_names: new FormControl(''),
      customercontact_gid: new FormControl(''),
      customer_mobile: new FormControl(''),
      customer_address: new FormControl(''),
      netamount : new FormControl(''),
      addon_charge:new FormControl(''),
      additional_discount : new FormControl(''),
      tax_name4 : new FormControl(''),
      tax_amount4 : new FormControl(''),
      grandtotal : new FormControl(''),
      inv_remarks: new FormControl(''),
      creditnote_amount: new FormControl(''),
      outstanding_amount: new FormControl(''),
      credited_amount: new FormControl(''),
      paid_amount: new FormControl(''),
      invoice_amount: new FormControl(''),
      invoice_refno1: new FormControl(''),
    });
    this.productform = new FormGroup({
      productgroup_name: new FormControl(''),
      productgroup_gid: new FormControl(''),
      product_gid : new FormControl(''),
      product_code: new FormControl(''),
      product_name: new FormControl(''),
      display_field: new FormControl(''),
      tax_name: new FormControl(''),
      tax_name2: new FormControl(''),
      tax_name3: new FormControl(''),
      tax_amount: new FormControl(''),
      tax_amount2 : new FormControl(''),
      tax_amount3: new FormControl(''),
      producttotal_amount: new FormControl(''),
      product_amount : new FormControl('')
    });


    
  }
  GetProdSummary(invoice_gid: any) {
    debugger
    var url = 'SmrRptInvoiceReport/viewinvoice';
    this.invoice_gid = invoice_gid
    var params={
      invoice_gid : invoice_gid
    }
 
      var url1='SmrRptInvoiceReport/viewinvoiceproduct'
      this.service.getparams(url1, params).subscribe((result: any) => {
        this.responsedata = result;
        this.directinvoiceproductsummary_list = result.directinvoiceproductsummary_list;
      });
  }

  get creditnotedate(){
    return this.combinedFormData.get("creditnote_date")!;
  }
  getCurrentDate(): string {
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = today.getFullYear();

    return dd + '-' + mm + '-' + yyyy;
  }
  GetCnSummary(params:any){
    var url='SmrTrnCreditNote/GetAddSelectSummary'
    let param = {
      invoice_gid : params
    }
    this.service.getparams(url,param).subscribe((result:any)=>{
      this.responsedata=result;
    this.cnsummary_list = result.addselectCNsummary_list;
    this.combinedFormData.get("order_refno")?.setValue(this.cnsummary_list[0].invoice_reference);
    this.combinedFormData.get("invoice_refno")?.setValue(this.cnsummary_list[0].invoiceref_no);
    this.combinedFormData.get("invoice_date")?.setValue(this.cnsummary_list[0].invoice_date);
    this.combinedFormData.get("customer_name")?.setValue(this.cnsummary_list[0].customer_name);
    this.combinedFormData.get("customer_email")?.setValue(this.cnsummary_list[0].customer_email);
    this.combinedFormData.get("remarks")?.setValue(this.cnsummary_list[0].invoice_remarks);
    this.combinedFormData.get("netamount")?.setValue(this.cnsummary_list[0].price);
    this.combinedFormData.get("addon_charge")?.setValue(this.cnsummary_list[0].additionalcharges_amount);
    this.combinedFormData.get("additional_discount")?.setValue(this.cnsummary_list[0].discount_amount);
    this.combinedFormData.get("tax_name4")?.setValue(this.cnsummary_list[0].tax_name);
    this.combinedFormData.get("tax_amount4")?.setValue(this.cnsummary_list[0].Tax_amount);
    this.combinedFormData.get("grandtotal")?.setValue(this.cnsummary_list[0].invoice_amount);
    this.combinedFormData.get("outstanding_amount")?.setValue(this.cnsummary_list[0].outstanding);
    this.combinedFormData.get("credited_amount")?.setValue(this.cnsummary_list[0].credit_note);
    this.combinedFormData.get("paid_amount")?.setValue(this.cnsummary_list[0].payment_amount);
    this.combinedFormData.get("invoice_amount")?.setValue(this.cnsummary_list[0].invoice_amount);
    this.combinedFormData.get("invoice_refno1")?.setValue(this.cnsummary_list[0].invoiceref_no);

    const address = this.cnsummary_list[0].customer_address;
    const mobile = this.cnsummary_list[0].mobile;
    const contactperson = this.cnsummary_list[0].customer_contactperson;

    const customer_Address = `${contactperson}\n${mobile}\n${address}`; 
    this.combinedFormData.get("customer_details")?.setValue(customer_Address);
    });
  }

  // GetProdSummary(params:any){
  //   debugger
  //   var url='SmrTrnCreditNote/GetAddSelectProdSummary'
  //   let param = {
  //       invoice_gid : params
  //   }
  //   this.service.getparams(url,param).subscribe((result:any)=>{
  //     this.responsedata=result;
  //     this.cnprodsummary_list = result.addselectProdsummary_list;

  //     this.productform.get("productgroup_name")?.setValue(this.cnprodsummary_list[0].productgroup_name);
  //     this.productform.get("product_name")?.setValue(this.cnprodsummary_list[0].product_name);
  //     this.productform.get("product_code")?.setValue(this.cnprodsummary_list[0].product_code);
  //     this.productform.get("display_field")?.setValue(this.cnprodsummary_list[0].display_field);
  //     this.productform.get("product_amount")?.setValue(this.cnprodsummary_list[0].product_price);

  //     this.productform.get("tax_name")?.setValue(this.cnprodsummary_list[0].tax_name);
  //     this.productform.get("tax_name2")?.setValue(this.cnprodsummary_list[0].tax_name2);
  //     this.productform.get("tax_amount")?.setValue(this.cnprodsummary_list[0].tax_amount);
  //     this.productform.get("tax_amount2")?.setValue(this.cnprodsummary_list[0].tax_amount2);
  //     this.productform.get("producttotal_amount")?.setValue(this.cnprodsummary_list[0].price);
      
  //   });
  // }

  onSubmit(){
    
    if(this.combinedFormData.value.creditnote_amount==""||this.combinedFormData.value.creditnote_amount==null){
      debugger;
      window.scrollTo({
        top: 0, 
      });
      this.ToastrService.warning("Kindly enter the Credit Amount");


    }
    else{
    const invoice_gid = this.router.snapshot.paramMap.get('invoice_gid'); 
    this.invoice_gid = invoice_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.invoice_gid, secretKey).toString(enc.Utf8);

    var params = {
      invoice_gid : deencryptedParam,
      inv_remarks : this.combinedFormData.value.inv_remarks,
      creditnote_amount : this.combinedFormData.value.creditnote_amount,
      creditnote_date : this.combinedFormData.value.creditnote_date,
      outstanding_amount: this.combinedFormData.value.outstanding_amount
    }
    var url = 'SmrTrnCreditNote/PostCreditNote'
    this.NgxSpinnerService.show();
    this.service.postparams(url, params).subscribe((result: any) => {
      if (result.status == false) {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message)
      }
      else {
        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message)
        this.route.navigate(['/smr/SmrTrnCreditNoteSummary']);
      }
    });
  }
}
}