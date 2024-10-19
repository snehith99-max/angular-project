import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { AES, enc } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
interface Ipurchaseview {
  renewal_gid: string;
  manualporef_no: string;
  Created_date: string;
  branch_name: string;
  vendor_companyname: string;
  deliverytobranch: string;
  branch_add1: string;
  vendor_contactnumber: string;
  vendor_contact_person: string;
  vendor_faxnumber: string;
  vendor_emailid: string;
  vendor_address: string;
  exchange_rate: string;
  ship_via: string;
  payment_terms: string;
  freight_terms: string;
  delivery_location: string;
  currency_code: string;
  shipping_address: string;
  purchaseorder_reference: string;
  purchaseorder_remarks: string;
  productgroup_name: string;
  product_code: string;
  product_name: string;
  productuom_name: string;
  qty_ordered: string;
  product_price: string;
  discount_percentage: string;
  tax_gid: string;
  packing_charges: string;
  insurance_charges: string;
  additional_discount: string;
  entity: string;

  payment_days: string;
  termsandconditions: string;
  priority:string;

}
@Component({
  selector: 'app-pmr-trn-purchaseagreementview',
  templateUrl: './pmr-trn-purchaseagreementview.component.html',
  styleUrls: ['./pmr-trn-purchaseagreementview.component.scss']
})
export class PmrTrnPurchaseagreementviewComponent  {
  
  config: AngularEditorConfig = {
    editable: false,
    spellcheck: false,
    height: '200rem',
    minHeight: '100rem',
    placeholder: 'Enter text here...',
    translate: 'no',
   
  };


  branch_name:any;
  renewal_gid:any;
  purchaseorder_gid:any;
  renewal_description:any;
  Created_date:any;
  email:any;
  contactnumber:any;
  gst_number:any;
  vendor_details:any;
  bill_to:any;
  ship_to:any;
  request_by:any;
  delivery_terms:any;
  payment_terms:any;
  request_contact:any;
  currency_code:any;
  desspatch_mode:any;
  pocover_note:any;
  exchange_rate:any;
  netamount:any;
  addon_amount:any;
  additional_discount:any;
  freightcharges:any;
  tax_name:any;
  overall_tax:any;
  roundoff:any;
  total_amount:any;
  termsandconditions:any;
  overalltax:any;
  expected_date:any;
  po_no:any;
  renewal_date:any;



  allchargeslist: any[] = [];

  product_total: any;

  

  purchaseorder_list: any;
  params:any;
  purchaseview!: Ipurchaseview;
  parameterValue1: any;
  purchaseorder:any;
  responsedata: any;
  insurancecharges: any;
  forwardingCharges: any;
  frieghtcharges: any;
  invoicediscountamount: any;
  addoncharge: any;
  product_totalprice: any;
  purchaseviewform : FormGroup | any;
  constructor(private formBuilder: FormBuilder,private route:Router,private router:ActivatedRoute,public service :SocketService) {
    this.purchaseview = {} as Ipurchaseview;
  }


  ngOnInit(): void {
    debugger
    this.purchaseorder= this.router.snapshot.paramMap.get('renewal_gid');
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.purchaseorder,secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)
    this.GetViewagreementSummary(deencryptedParam);   


  
    
    this.purchaseviewform = new FormGroup({
      termsandconditions : new FormControl('')
    })

  }
  GetViewagreementSummary(renewal_gid: any) {
    debugger
    var url='PmrTrnPurchaseagreement/GetViewagreementSummary'
    let param = {
      renewal_gid : renewal_gid 
    }
    this.service.getparams(url,param).subscribe((result:any)=>{
    this.purchaseorder_list = result.GetViewPurchaseagreement;
    this.purchaseorder_gid=this.purchaseorder_list[0].purchaseorder_gid;
    this.renewal_date=this.purchaseorder_list[0].renewal_date;
    this.renewal_description=this.purchaseorder_list[0].renewal_description;
    this.branch_name=this.purchaseorder_list[0].branch_name;
    this.Created_date=this.purchaseorder_list[0].Created_date;
    this.email=this.purchaseorder_list[0].email_id;
    this.contactnumber=this.purchaseorder_list[0].contact_telephonenumber;
    this.gst_number=this.purchaseorder_list[0].tax_number;
    this.vendor_details=this.purchaseorder_list[0].vendor_companyname;
    this.bill_to=this.purchaseorder_list[0].bill_to;
    this.ship_to=this.purchaseorder_list[0].shipping_address;
    this.request_by=this.purchaseorder_list[0].requested_by;
    this.request_contact=this.purchaseorder_list[0].requested_details;
    this.currency_code=this.purchaseorder_list[0].currency_code;
    this.exchange_rate=this.purchaseorder_list[0].exchange_rate;
    this.desspatch_mode=this.purchaseorder_list[0].mode_despatch;
    this.delivery_terms=this.purchaseorder_list[0].delivery_terms;
    this.payment_terms=this.purchaseorder_list[0].payment_terms;
    this.pocover_note=this.purchaseorder_list[0].po_covernote;
    this.netamount=this.purchaseorder_list[0].netamount;
    this.addon_amount=this.purchaseorder_list[0].addon_amount;
    this.additional_discount=this.purchaseorder_list[0].discount_amount;
    this.freightcharges=this.purchaseorder_list[0].freightcharges;
    this.tax_name=this.purchaseorder_list[0].overalltaxname;
    this.overall_tax=this.purchaseorder_list[0].overall_tax;
    this.roundoff=this.purchaseorder_list[0].roundoff;
    this.total_amount=this.purchaseorder_list[0].total_amount;
    this.termsandconditions=this.purchaseorder_list[0].termsandconditions;
    this.overalltax=this.purchaseorder_list[0].overalltax;
    this.expected_date=this.purchaseorder_list[0].expected_date;
    this.po_no=this.purchaseorder_list[0].po_no;
  

    function stripHtmlTags(html: string): string {
      const tempDiv = document.createElement('div');
      tempDiv.innerHTML = html;
      return tempDiv.textContent || tempDiv.innerText || '';
  }
this.purchaseviewform.get("termsandconditions")?.setValue(this.purchaseorder_list[0].termsandconditions)
const termsAndConditions = this.purchaseorder_list[0]?.termsandconditions || ''; // Get the value from purchaseorder_list or use an empty string if it's undefined
const plainTextTerms = stripHtmlTags(termsAndConditions);
this.purchaseviewform.get('termsandconditions')?.setValue(plainTextTerms);  

  });
  }

  hasTaxData(data: any): boolean {
    return data.taxseg_taxname1 || data.taxseg_taxname2 || data.taxseg_taxname3;
  }
  
 

}
