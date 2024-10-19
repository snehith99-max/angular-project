
import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { AES, enc } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
interface Ipurchaseview {
  purchaseorder_gid: string;
  manualporef_no: string;
  purchaseorder_date: string;
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
  selector: 'app-pbl-trn-invoice-approval-add',
  templateUrl: './pbl-trn-invoice-approval-add.component.html',
  styleUrls: ['./pbl-trn-invoice-approval-add.component.scss']
})
export class PblTrnInvoiceApprovalAddComponent {

  
  config: AngularEditorConfig = {
    editable: false,
    spellcheck: false,
    height: '33rem',
    minHeight: '0rem',
    placeholder: 'Enter text here...',
    translate: 'no',
   
  };


  branch_name:any;
  purchaseorder_gid:any;
  purchaseorder_date:any;
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
  response_data:any;
  addon_amount:any;
  additional_discount:any;
  freightcharges:any;
  tax_name:any;
  overall_tax:any;
  roundoff:any;
  total_amount:any;
  termsandconditions:any;
  overalltax:any;
  grand_total : any




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

  
  constructor(private formBuilder: FormBuilder,private route:Router,private router:ActivatedRoute,public service :SocketService,
    public ToastrService :ToastrService,public NgxSpinnerService:NgxSpinnerService) {
    this.purchaseview = {} as Ipurchaseview;
  }


  ngOnInit(): void {
     
    this.purchaseorder= this.router.snapshot.paramMap.get('purchaseorder_gid');
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.purchaseorder,secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)
    this.GetViewPurchaseOrderSummary(deencryptedParam);   

    this.GetViewPurchaseOrderSummary(deencryptedParam);   
    var api = 'PmrMstPurchaseConfig/GetAllChargesConfig';
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
    
    this.purchaseviewform = new FormGroup({
      termsandconditions : new FormControl('')
    })

  }
  GetViewPurchaseOrderSummary(purchaseorder_gid: any) {
     
    var url='PblInvoiceGrnDetails/GetViewApprovalAddSummary'
    let param = {
      purchaseorder_gid : purchaseorder_gid 
    }
    this.service.getparams(url,param).subscribe((result:any)=>{
    this.purchaseorder_list = result.GetViewApproval;
    this.purchaseorder_gid=this.purchaseorder_list[0].purchaseorder_gid;
    this.branch_name=this.purchaseorder_list[0].branch_name;
    this.purchaseorder_date=this.purchaseorder_list[0].purchaseorder_date;
    this.email=this.purchaseorder_list[0].email_id;
    this.contactnumber=this.purchaseorder_list[0].contact_telephonenumber;
    this.gst_number=this.purchaseorder_list[0].tax_number;
    this.vendor_details=this.purchaseorder_list[0].vendor_companyname;
    this.bill_to=this.purchaseorder_list[0].vendor_address;
    this.ship_to=this.purchaseorder_list[0].address2;
    this.request_by=this.purchaseorder_list[0].requested_by;
    this.request_contact=this.purchaseorder_list[0].requested_details;
    this.currency_code=this.purchaseorder_list[0].currency_code;
    this.exchange_rate=this.purchaseorder_list[0].exchange_rate;
    this.desspatch_mode=this.purchaseorder_list[0].mode_despatch;
    this.delivery_terms=this.purchaseorder_list[0].delivery_terms;
    this.payment_terms=this.purchaseorder_list[0].payment_terms;
    this.pocover_note=this.purchaseorder_list[0].po_covernote;
    this.netamount=Number(this.purchaseorder_list[0].netamount.replace(/,,/g, ',').replace(/,/g, ''));
    this.addon_amount=Number(this.purchaseorder_list[0].addon_amount.replace(/,,/g, ',').replace(/,/g, ''));
    this.additional_discount=Number(this.purchaseorder_list[0].discount_amount.replace(/,,/g, ',').replace(/,/g, ''));
    this.freightcharges=Number(this.purchaseorder_list[0].freightcharges.replace(/,,/g, ',').replace(/,/g, ''));
    this.tax_name=this.purchaseorder_list[0].overalltaxname;
    this.overall_tax=this.purchaseorder_list[0].overall_tax;
    this.roundoff=Number(this.purchaseorder_list[0].roundoff.replace(/,,/g, ',').replace(/,/g, ''));
    this.total_amount=this.purchaseorder_list[0].total_amount;
    this.termsandconditions=this.purchaseorder_list[0].termsandconditions;
    this.overalltax=Number(this.purchaseorder_list[0].overalltax.replace(/,,/g, ',').replace(/,/g, ''));
    debugger
    this.grand_total = (this.netamount +   this.addon_amount- this.additional_discount + this.freightcharges + this.overalltax + this.roundoff)
  

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
  

  Submit() {
        
   
    var params = {    
      purchaseorder_gid: this.purchaseorder_list[0].purchaseorder_gid,
     
    }
  
    this.NgxSpinnerService.show();
    var api6 = 'PblInvoiceGrnDetails/PostSubmit'
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
          this.route.navigate(['/payable/PmrTrnInvoice']);
        }
      });
      this.NgxSpinnerService.hide();
  }
 

}
